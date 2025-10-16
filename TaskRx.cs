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

        private void TreeView_BeforeCheck(object? sender, TreeViewCancelEventArgs e)
        {
            // Prevent checking/unchecking of auto tasks
            if (e.Node?.Tag is MainTask mainTask && mainTask.Auto)
            {
                e.Cancel = true;
                return;
            }
            
            if (e.Node?.Tag is PostTask postTask && postTask.Auto)
            {
                e.Cancel = true;
                return;
            }
        }

        private void TreeView_AfterCheck(object? sender, TreeViewEventArgs e)
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
            lblInitials.Visible = txtInitials.Enabled = txtInitials.Visible = !isExecuting;
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
        /// Handles the click of the Execute button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void executeButton_Click(object sender, EventArgs e)
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

            // Add auto tasks to execution list
            AddAutoTasks();

            // Add remaining tasks to execution list
            AddCheckedTasks(setupTabControl);

            // Save UserTasks.json
            SaveUserTasks();

            // Execute selected tasks
            ExecuteTasks();
            // Note: The application will exit if the user chooses to do so in the ExecuteTasks method
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
                        MainTask? Task = node.Tag as MainTask;
                        // Don't modify auto tasks - they stay checked
                        if (Task != null && !Task.Auto)
                        {
                            node.Checked = Task.Workstation;
                        }
                        foreach (TreeNode childNode in node.Nodes)
                        {
                            PostTask? postTask = childNode.Tag as PostTask;
                            // Don't modify auto post tasks - they stay checked
                            if (postTask != null && !postTask.Auto)
                            {
                                childNode.Checked = postTask.Workstation;
                            }
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
                        MainTask? Task = node.Tag as MainTask;
                        // Don't modify auto tasks - they stay checked
                        if (Task != null && !Task.Auto)
                        {
                            node.Checked = Task.Jumpstation;
                        }
                        foreach (TreeNode childNode in node.Nodes)
                        {
                            PostTask? postTask = childNode.Tag as PostTask;
                            // Don't modify auto post tasks - they stay checked
                            if (postTask != null && !postTask.Auto)
                            {
                                childNode.Checked = postTask.Jumpstation;
                            }
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
                        // Auto tasks are already checked, so this is safe
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
                            // Auto post tasks are already checked, so this is safe
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
                        MainTask? mainTask = node.Tag as MainTask;
                        // Don't uncheck auto tasks - they stay checked
                        if (mainTask != null && !mainTask.Auto)
                        {
                            node.Checked = false;
                        }
                        foreach (TreeNode childNode in node.Nodes)
                        {
                            PostTask? postTask = childNode.Tag as PostTask;
                            // Don't uncheck auto post tasks - they stay checked
                            if (postTask != null && !postTask.Auto)
                            {
                                childNode.Checked = false;
                            }
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
                        MainTask? mainTask = node.Tag as MainTask;
                        // Don't uncheck auto tasks - they stay checked
                        if (mainTask != null && !mainTask.Auto)
                        {
                            node.Checked = false;
                        }
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
                            PostTask? postTask = childNode.Tag as PostTask;
                            // Don't uncheck auto post tasks - they stay checked
                            if (postTask != null && !postTask.Auto)
                            {
                                childNode.Checked = false;
                            }
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