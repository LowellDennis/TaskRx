using System.Diagnostics;
using TaskRx.Utilities;

namespace TaskRx
{
    public partial class TaskRx : Form
    {
        public TaskRx()
        {
            InitializeComponent();

            // Apply default value for txtBase if default exits
            if (Path.Exists("C:\\HPE\\Dev\\ROMS"))
            {
                txtBase.Text = "C:\\HPE\\Dev\\ROMS";
            }

            // Supply load handler
            this.Load += new System.EventHandler(TaskRx_Load);
        }

        // The async initialization is now handled in the TaskRx_Load event handler

        private void TreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            UpdateExecuteButtonState();
        }

        /// <summary>
        /// Determines if execute button should be enabled
        /// Note: Only enabled if at least one of the check boxes is checked
        /// </summary>
        private void UpdateExecuteButtonState()
        {
            // Check if all text boxes are filled
            bool allTextBoxesFilled = !string.IsNullOrEmpty(txtFirst.Text) &&
                                      !string.IsNullOrEmpty(txtLast.Text) &&
                                      !string.IsNullOrEmpty(txtInitials.Text) &&
                                      !string.IsNullOrEmpty(txtDomain.Text) &&
                                      !string.IsNullOrEmpty(txtWork.Text) &&
                                      !string.IsNullOrEmpty(txtPersonal.Text) &&
                                      !string.IsNullOrEmpty(txtBase.Text);

            // Check if any task is checked in both setup and update tab controls
            bool anyTaskChecked = false;
            foreach (TabPage tabPage in setupTabControl.TabPages)
            {
                foreach (TreeView treeView in tabPage.Controls.OfType<TreeView>())
                {
                    if (treeView.Nodes.Cast<TreeNode>().Any(node => node.Checked))
                    {
                        anyTaskChecked = true;
                        break;
                    }
                }
                if (anyTaskChecked) break;
            }

            // Enable or disable the execute button based on both conditions
            executeButton.Enabled = allTextBoxesFilled && anyTaskChecked;
        }

        // Update event handlers to call the combined method
        private void txtFirst_Changed(object sender, EventArgs e)
        {
            txtFirst.Text = txtFirst.Text.Trim();
            GenerateInitials();
            GenerateUsername();
            GenerateEmail();
            UpdateExecuteButtonState();
        }

        private void txtLast_Changed(object sender, EventArgs e)
        {
            txtLast.Text = txtLast.Text.Trim();
            GenerateInitials();
            GenerateUsername();
            GenerateEmail();
            UpdateExecuteButtonState();
        }

        private void txtInitials_TextChanged(object sender, EventArgs e)
        {
            if (txtInitials.Modified)
            {
                Initials = txtInitials.Text;
            }
            UpdateExecuteButtonState();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if (txtUsername.Modified)
            {
                Username = txtUsername.Text;
            }
            UpdateExecuteButtonState();
        }

        private void txtWork_TextChanged(object sender, EventArgs e)
        {
            if (txtWork.Modified)
            {
                WorkEmail = txtWork.Text;
            }
            UpdateExecuteButtonState();
        }

        private void txtPersonal_TextChanged(object sender, EventArgs e)
        {
            if (txtPersonal.Modified)
            {
                PersonalEmail = txtPersonal.Text;
            }
            UpdateExecuteButtonState();
        }

        private void txtBase_TextChanged(object sender, EventArgs e)
        {
            if (txtBase.Modified)
            {
                ReposFolder = txtBase.Text;

                // Validate if the directory exists
                if (!string.IsNullOrEmpty(txtBase.Text) && !Directory.Exists(txtBase.Text))
                {
                    executionStatus.Text = "Directory does not exist: " + txtBase.Text;
                }
                else
                {
                    executionStatus.Text = string.Empty;
                }
            }
            UpdateExecuteButtonState();
        }

        /// <summary>
        /// Handle a chane in the operating mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitWithMessage(string message, string title, int errorCode = 1)
        {
            MessageBoxHelper.ShowCentered(
                this, // Pass the owner form to center the dialog on it
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            Environment.Exit(errorCode); // Exit the program with a non-zero exit code
        }

        /// <summary>
        /// Adjusts enablement and visibility of controls that share the same space
        /// </summary>
        /// <param name="isExecuting">True if tasks and post task are executing, false otherwise</param>
        private void ChangeState(bool isExecuting)
        {
            // Set enable/disable state and visibility of controls occupying the same spave
            txtOutput.Enabled = txtOutput.Visible = isExecuting;
            lblFirst.Visible = txtFirst.Enabled = txtFirst.Visible = !isExecuting;
            lblLast.Visible = txtLast.Enabled = txtLast.Visible = !isExecuting;
            lblInitials.Visible = txtInitials.Enabled = !isExecuting;
            lblDomain.Visible = txtDomain.Enabled = txtDomain.Visible = !isExecuting;
            lblUsername.Visible = txtUsername.Enabled = txtUsername.Visible = !isExecuting;
            lblWork.Visible = txtWork.Enabled = txtWork.Visible = !isExecuting;
            lblPersonal.Visible = txtPersonal.Enabled = txtPersonal.Visible = !isExecuting;
            lblBase.Visible = txtBase.Enabled = txtBase.Visible = !isExecuting;
            btnBase.Enabled = btnBase.Visible = !isExecuting;
            checkAllSplitButton.Enabled = checkAllSplitButton.Visible = !isExecuting;
            uncheckAllSplitButton.Enabled = uncheckAllSplitButton.Visible = !isExecuting;
            checkWorkstationButton.Enabled = checkWorkstationButton.Visible = !isExecuting;
            uncheckAllButton.Enabled = uncheckAllButton.Visible = !isExecuting;
            setupTabControl.Enabled = setupTabControl.Visible = !isExecuting;
            executeButton.Enabled = !isExecuting;
        }

        /// <summary>
        /// Determines if a command/arguments pair references WinGet
        /// </summary>
        /// <param name="command"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private bool ReferencesWinGet(string command, string arguments)
        {
            if ((command != null) && (command.IndexOf("winget", StringComparison.OrdinalIgnoreCase) >= 0))
            {
                return true;
            }
            return ((arguments != null) && (arguments.IndexOf("winget", StringComparison.OrdinalIgnoreCase) >= 0));
        }

        /// <summary>
        /// Handles the click of the Execute button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void executeButton_Click(object sender, EventArgs e)
        {
            // Disable and hide most controls during execution
            ChangeState(true);

            // Make output text box visible
            setupTabControl.Visible = false;
            txtOutput.Visible = true;

            // Create empty execution list list
            executionTasks = new List<ExecutionTask>();
            executionSteps = 0;

            // Save UserTasks.json
            SaveUserInfo();

            // Add hidden tasks to execution list
            AddHiddenTasks();

            // Add remaining tasks to execution list
            AddCheckedTasks(setupTabControl);

            // Check if winget is needed by examining the setupCommand and UpdateCommand strings
            bool wingetNeeded = false;
            foreach (ExecutionTask task in executionTasks)
            {
                // Check main task commands
                MainTask mainTask = allTasks[task.Id];
                if (ReferencesWinGet(mainTask.Command, mainTask.Arguments))
                {
                    wingetNeeded = true;
                    break;
                }

                // Check post task commands
                foreach (ExecuteTask postTask in task.PostTasks)
                {
                    PostTask post = allPostTasks[postTask.Id];
                    if (ReferencesWinGet(post.Command, post.Arguments))
                    {
                        wingetNeeded = true;
                        break;
                    }
                }

                if (wingetNeeded)
                {
                    break;
                }
            }

            // If winget is needed, ensure the latest version is installed
            if (wingetNeeded)
            {
                UpdateStatus("Winget installation check ...");
                UpdateOutputTextBox("PLEASE BE PATIENT, this could take a while!", true);
                var result = await InsureLatestWingetIsInstalled();
                UpdateOutputTextBox($"{result.Output.Trim()}\nWinget check result: Exit Code = {result.ExitCode}", result.ExitCode != 0);
                UpdateStatus("Winget installation check completed.");
            }

            // Save UserTasks.json
            SaveUserTasks();

            // Execute selected tasks
            ExecuteTasks();
            // Note: The application will exit if the user chooses to do so in the ExecuteTasks method
        }

        // Function to ensure the latest version of Winget is installed
        private async Task<(string Output, int ExitCode)> InsureLatestWingetIsInstalled()
        {
            try
            {
                // Create a temporary output file for elevation purposes
                string tempOutputFile = Path.Combine(Path.GetTempPath(), $"ps_output_{Guid.NewGuid()}.txt");

                // We need to use Process for elevation, but we'll use a simpler approach
                // that leverages the temporary file for output capture
                string installWinGetScript = Path.Combine(PathConfig.ScriptsPath, "InstallWinGet.ps1");
                string psCommand = $@"
                    try {{
                        $ErrorActionPreference = 'Continue'
                        $output = & '{installWinGetScript}' 2>&1 | Out-String
                        $exitCode = $LASTEXITCODE
                        if ($null -eq $exitCode) {{ $exitCode = 0 }}
                        $output | Out-File -FilePath '{tempOutputFile}' -Encoding utf8
                        [System.Environment]::ExitCode = $exitCode
                    }} catch {{
                        $_ | Out-File -FilePath '{tempOutputFile}' -Encoding utf8 -Append
                        [System.Environment]::ExitCode = 1
                    }}
                ";

                // StartInfo settings to run the script with elevation
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-WindowStyle Minimized -ExecutionPolicy Bypass -Command \"{psCommand}\"",
                    Verb = "runas", // This is the key property for elevation
                    UseShellExecute = true, // Required for the Verb property to work
                    WindowStyle = ProcessWindowStyle.Minimized, // Minimize the window
                    WorkingDirectory = PathConfig.ScriptsPath
                };

                using (Process? process = Process.Start(psi))
                {
                    if (process == null)
                    {
                        return ("Failed to start process", -1);
                    }

                    // Wait for the process to exit
                    await process.WaitForExitAsync();

                    // Load the output from the temporary file
                    string output = "";
                    if (File.Exists(tempOutputFile))
                    {
                        // Give the file system a moment to complete writing
                        await System.Threading.Tasks.Task.Delay(100);

                        // Read temporary file contents
                        output = await File.ReadAllTextAsync(tempOutputFile);

                        // Remove temporary file
                        File.Delete(tempOutputFile);
                    }

                    // Return output and exit code
                    return (output, process.ExitCode);
                }
            }
            catch (Exception ex)
            {
                // Give error information and return code
                return ($"Exception: {ex.Message}", -2);
            }
        }

        private void GenerateInitials()
        {
            if (string.IsNullOrEmpty(Initials))
            {
                char First = (txtFirst.Text.Length > 0) ? txtFirst.Text[0] : ' ';
                char Last = (txtLast.Text.Length > 0) ? txtLast.Text[0] : ' ';
                txtInitials.Text = $"{First}{Last}";
            }
        }
        private void GenerateUsername()
        {
            if (string.IsNullOrEmpty(Username))
            {
                char Initial = (txtFirst.Text.Length > 0) ? txtFirst.Text.ToLower()[0] : ' ';
                txtUsername.Text = txtLast.Text.ToLower() + Initial;
            }
        }
        private void GenerateEmail()
        {
            if (string.IsNullOrEmpty(WorkEmail))
            {
                txtWork.Text = txtFirst.Text.ToLower() + '.' + txtLast.Text.ToLower() + "@hpe.com";
            }
        }

        /// <summary>
        /// Check defaults for Workstation checkboxes
        /// </summary>
        private void checkDefaultWorkstation_ButtonClick(object sender, EventArgs e)
        {
            // Process all nodes in tree view for tab control
            foreach (TabPage tabPage in setupTabControl.TabPages)
            {
                foreach (TreeView treeView in tabPage.Controls.OfType<TreeView>())
                {
                    foreach (TreeNode node in treeView.Nodes)
                    {
                        MainTask Task = node.Tag as MainTask;
                        node.Checked = Task.Workstation;
                        foreach (TreeNode childNode in node.Nodes)
                        {
                            PostTask postTask = childNode.Tag as PostTask;
                            childNode.Checked = postTask.Workstation;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check defaults for Jumpstation checkboxes
        /// </summary>
        private void checkDefaultJumpstation_ButtonClick(object sender, EventArgs e)
        {
            // Process all nodes in tree view for tab control
            foreach (TabPage tabPage in setupTabControl.TabPages)
            {
                foreach (TreeView treeView in tabPage.Controls.OfType<TreeView>())
                {
                    foreach (TreeNode node in treeView.Nodes)
                    {
                        MainTask Task = node.Tag as MainTask;
                        node.Checked = Task.Jumpstation;
                        foreach (TreeNode childNode in node.Nodes)
                        {
                            PostTask postTask = childNode.Tag as PostTask;
                            childNode.Checked = postTask.Jumpstation;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check all checkboxes
        /// </summary>
        private void checkAllMenuItem_ButtonClick(object sender, EventArgs e)
        {
            // Show warning message before proceeding
            DialogResult result = MessageBoxHelper.ShowCentered(
                this, // Pass the owner form to center the dialog on it
                "Checking all can take a very long time to execute.\n\nDo you wish to proceed?",
                "Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            // Only proceed if user clicks Yes
            if (result != DialogResult.Yes)
            {
                return; // Exit without doing anything if user clicks No
            }

            // Process all nodes in tree view for tab control
            foreach (TabPage tabPage in setupTabControl.TabPages)
            {
                foreach (TreeView treeView in tabPage.Controls.OfType<TreeView>())
                {
                    foreach (TreeNode node in treeView.Nodes)
                    {
                        node.Checked = true;
                        foreach (TreeNode childNode in node.Nodes)
                        {
                            childNode.Checked = true;
                        }
                    }
                }
            }

            // Update the execute button state
            UpdateExecuteButtonState();
        }

        /// <summary>
        /// Check only the main task nodes (not post tasks)
        /// </summary>
        private void checkAllTasksMenuItem_Click(object sender, EventArgs e)
        {
            // Process all main task nodes in all tree views fortab control
            foreach (TabPage tabPage in setupTabControl.TabPages)
            {
                foreach (TreeView treeView in tabPage.Controls.OfType<TreeView>())
                {
                    foreach (TreeNode node in treeView.Nodes)
                    {
                        node.Checked = true;
                    }
                }
            }

            // Update the execute button state
            UpdateExecuteButtonState();
        }

        /// <summary>
        /// Check only the post task nodes (not main tasks)
        /// </summary>
        private void checkAllPostTasksMenuItem_Click(object sender, EventArgs e)
        {
            // Show warning message before proceeding
            DialogResult result = MessageBoxHelper.ShowCentered(
                this, // Pass the owner form to center the dialog on it
                "Checked post tasks do not execute unless parent task is also checked!\n\nDo you wish to proceed?",
                "Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            // Process all post task nodes in all tree views for both tab controls
            foreach (TabPage tabPage in setupTabControl.TabPages)
            {
                foreach (TreeView treeView in tabPage.Controls.OfType<TreeView>())
                {
                    foreach (TreeNode node in treeView.Nodes)
                    {
                        foreach (TreeNode childNode in node.Nodes)
                        {
                            childNode.Checked = true;
                        }
                    }
                }
            }

            // Update the execute button state
            UpdateExecuteButtonState();
        }

        /// <summary>
        /// Uncheck all checkboxes in setup tab control
        /// </summary>
        private void uncheckAllButton_ButtonClick(object sender, EventArgs e)
        {
            // Process all nodes in all tree views for tab control
            foreach (TabPage tabPage in setupTabControl.TabPages)
            {
                foreach (TreeView treeView in tabPage.Controls.OfType<TreeView>())
                {
                    foreach (TreeNode node in treeView.Nodes)
                    {
                        node.Checked = false;
                        foreach (TreeNode childNode in node.Nodes)
                        {
                            childNode.Checked = false;
                        }
                    }
                }
            }
            // Update the execute button state
            UpdateExecuteButtonState();
        }

        /// <summary>
        /// Uncheck only the main task nodes (not post tasks) in both setup tab control
        /// </summary>
        private void uncheckAllTasksMenuItem_Click(object sender, EventArgs e)
        {
            // Process all main task nodes in all tree views for tab control
            foreach (TabPage tabPage in setupTabControl.TabPages)
            {
                foreach (TreeView treeView in tabPage.Controls.OfType<TreeView>())
                {
                    foreach (TreeNode node in treeView.Nodes)
                    {
                        node.Checked = false;
                    }
                }
            }

            // Update the execute button state
            UpdateExecuteButtonState();
        }

        /// <summary>
        /// Uncheck only the post task nodes (not main tasks) in setup tab control
        /// </summary>
        private void uncheckAllPostTasksMenuItem_Click(object sender, EventArgs e)
        {
            // Process all post task nodes in all tree views for tab control
            foreach (TabPage tabPage in setupTabControl.TabPages)
            {
                foreach (TreeView treeView in tabPage.Controls.OfType<TreeView>())
                {
                    foreach (TreeNode node in treeView.Nodes)
                    {
                        foreach (TreeNode childNode in node.Nodes)
                        {
                            childNode.Checked = false;
                        }
                    }
                }
            }

            // Update the execute button state
            UpdateExecuteButtonState();
        }

        /// <summary>
        /// Opens a folder browser dialog to select the repository base directory
        /// </summary>
        private void btnBase_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                // Set initial directory if one is already specified
                if (!string.IsNullOrEmpty(txtBase.Text) && Directory.Exists(txtBase.Text))
                {
                    folderDialog.InitialDirectory = txtBase.Text;
                }

                // Set dialog properties
                folderDialog.Description = "Select Repository Base Directory";
                folderDialog.UseDescriptionForTitle = true;
                folderDialog.ShowNewFolderButton = true;

                // Show the dialog and check if user clicked OK
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    // Update the text box with the selected path
                    txtBase.Text = folderDialog.SelectedPath;
                }
            }
        }
    }
}