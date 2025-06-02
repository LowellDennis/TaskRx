using System.Text.Json;
using TaskRx.Utilities;

namespace TaskRx
{
    public partial class TaskRx : Form
    {
        /// <summary>
        /// Load information from AllTasks.json
        /// On Exit tasksList contains information for all supported tasks.
        ///         allTasks is set up for accessing tasks by their unique ID.
        ///         allPostTasks is set up for accessing post tasks by their unique ID.
        /// </summary>
        private async Task<bool> GetAllTasksAsync()
        {
            // Make sure AllTasks.json file exists
            string jsonFilePath = PathConfig.GetInstallFilePath("AllTasks.json");
            if (!File.Exists(jsonFilePath))
            {
                ExitWithMessage($"AllTasks.json file not found at path: {jsonFilePath}\nExecutable directory: {AppDomain.CurrentDomain.BaseDirectory}", "File Not Found", 1);
                return false;
            }

            try
            {
                // Load AllTasks.json in allTasks
                string jsonContent = await File.ReadAllTextAsync(jsonFilePath);
                taskList = JsonSerializer.Deserialize<TaskList>(jsonContent);

                // Initialize the dictionaries
                allTasks = new Dictionary<string, MainTask>();
                allPostTasks = new Dictionary<string, PostTask>();

                // Build allTasks and allPostTasks dictionaries
                foreach (MainTask task in taskList.Tasks)
                {
                    try
                    {
                        allTasks.Add(task.Id, task);
                        foreach (PostTask postTask in task.PostTask)
                        {
                            allPostTasks.Add(postTask.Id, postTask);
                        }
                    }
                    catch
                    {
                        ExitWithMessage("Possible duplicate Id in AllTasks.json file: " + task.Id, "JSON Error", 2);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                ExitWithMessage("Format of AllTasks.json file is invalid", "JSON Error", 2);
                return false;
            }
        }

        /// <summary>
        /// Non-async version for backward compatibility
        /// </summary>
        private void GetAllTasks()
        {
            // Call the async version and wait for it to complete
            GetAllTasksAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Load user information from UserInfo.json
        /// On Exit with true, userTasks contains information for user tasks.
        /// </summary>
        /// <returns>True if it exists, false otherwise</returns>
        private async Task<bool> GetUserInfoAsync()
        {
            // See if UserInfo.json file exits
            string userInfoFilePath = PathConfig.GetUserFilePath("UserInfo.json");
            if (File.Exists(userInfoFilePath))
            {
                // Load UserInfo.json ==> userInfo
                try
                {
                    string userInfoJsonContent = await File.ReadAllTextAsync(userInfoFilePath);
                    userInfo = JsonSerializer.Deserialize<UserInfo>(userInfoJsonContent);

                    // UI updates must be done on the UI thread
                    await Task.Run(() =>
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            txtFirst.Text = userInfo.First;
                            txtLast.Text = userInfo.Last;
                            txtDomain.Text = userInfo.Domain;
                            txtUsername.Text = userInfo.Username;
                            txtWork.Text = userInfo.Work;
                            txtPersonal.Text = userInfo.Personal;

                            // Load the ReposFolder value if it exists
                            if (!string.IsNullOrEmpty(userInfo.ReposFolder))
                            {
                                txtBase.Text = userInfo.ReposFolder;
                                ReposFolder = userInfo.ReposFolder;

                                // Validate if the directory exists
                                if (!Directory.Exists(userInfo.ReposFolder))
                                {
                                    executionStatus.Text = "Directory does not exist: " + userInfo.ReposFolder;
                                }
                            }
                        });
                    });
                    return true;
                }
                catch
                {
                    ExitWithMessage("Format of UserInfo.json file is invalid", "JSON Error", 3);
                    return false;
                }
            }
            else
            {
                string domain   = Environment.UserDomainName;
                string username = Environment.UserName;

                // See if domain can be obtained from the environment
                if (!domain.Equals(""))
                {
                    await Task.Run(() =>
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            txtDomain.Text = domain;
                        });
                    });
                }
                // See if Username can be obtained from the environment
                if (!username.Equals("Administrator"))
                {
                    await Task.Run(() =>
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            txtUsername.Text = username;
                        });
                    });
                }
                return true;
            }
        }

        /// <summary>
        /// Non-async version for backward compatibility
        /// </summary>
        private void GetUserInfo()
        {
            // Call the async version and wait for it to complete
            GetUserInfoAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Load information from UserTasks.json
        /// On Exit with true, userTasks contains information for user tasks.
        /// </summary>
        /// <returns>True if it exists, false otherwise</returns>
        private async Task<bool> GetUserTasksAsync()
        {
            // See if UserTasks.json file exits
            string userTasksFilePath = PathConfig.GetUserFilePath("UserTasks.json");
            if (File.Exists(userTasksFilePath))
            {
                // Load UserTasks.json ==> userTasks
                try
                {
                    string userTasksJsonContent = await File.ReadAllTextAsync(userTasksFilePath);
                    userTasks = JsonSerializer.Deserialize<List<UserTask>>(userTasksJsonContent);
                    return true;
                }
                catch
                {
                    ExitWithMessage("Format of UserTasks.json file is invalid", "JSON Error", 3);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Non-async version for backward compatibility
        /// </summary>
        private bool GetUserTasks()
        {
            // Call the async version and wait for it to complete
            return GetUserTasksAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Create and populate tabControls
        /// Note: there are two tab controls setupTabcontrol and updateTabControl
        /// </summary>
        private void SetupTabControls()
        {
            // Clear existing tabs to prevent duplicates
            setupTabControl.TabPages.Clear();

            // Determine the different task groups
            var groups = taskList.Tasks.Where(t => !t.Hidden).GroupBy(t => t.Group);

            // Create a tab for each task group
            specificPostTasks = new Dictionary<string, List<PostTask>>();
            foreach (var group in groups)
            {
                // Initialize setup tab stuff
                TabPage setupTabPage = new TabPage(group.Key);
                TreeView setupTreeView = new TreeView();
                setupTreeView.CheckBoxes = true;
                setupTreeView.Dock = DockStyle.Fill;
                setupTreeView.AfterCheck += TreeView_AfterCheck;

                // Loop through tasks in groups
                foreach (var task in group)
                {
                    // Initialize setup node
                    TreeNode setupTaskNode = new TreeNode(task.Name);
                    setupTaskNode.Tag = task;
                    specificPostTasks[task.Name] = task.PostTask;

                    // Loop through post tasks that are not hidden (hidden post tasks are not visible)
                    foreach (var postTask in task.PostTask.Where(pt => !pt.Hidden))
                    {
                        // Initialize setup post task
                        TreeNode setupPostTaskNode = new TreeNode(postTask.Name);
                        setupPostTaskNode.Tag = postTask;

                        // Add setup post task
                        setupTaskNode.Nodes.Add(setupPostTaskNode);
                    }

                    // Add to nodes
                    setupTreeView.Nodes.Add(setupTaskNode);
                }

                // Add setup tab page and tabs control
                setupTabPage.Controls.Add(setupTreeView);
                setupTabControl.TabPages.Add(setupTabPage);

                // Expand all nodes by default
                setupTreeView.ExpandAll();
            }
        }

        /// <summary>
        /// Set selected (checked) check boxes within the tab controls
        /// </summary>
        /// <param name="tabControl"></param>
        private void LoadCheckBoxes(TabControl tabControl)
        {
            // Loop through the tasks in UserTasks.json
            foreach (var userTask in userTasks)
            {
                // Loop through tab pages
                foreach (TabPage tabPage in tabControl.TabPages)
                {
                    // Loop through trees
                    foreach (TreeView treeView in tabPage.Controls.OfType<TreeView>())
                    {
                        // Loop through nodes
                        foreach (TreeNode taskNode in treeView.Nodes)
                        {
                            // Compare task Id to user task Id
                            var task = (MainTask)taskNode.Tag;
                            if (task.Id == userTask.Id)
                            {
                                // MATCH - so this task is selected!
                                taskNode.Checked = true;

                                // Since this user task is selected, its post tasks need to be checked
                                // Note: post tasks for unselected tasks are by default also unselected!
                                // Loop through the post tasks in this task in UserTasks.json
                                foreach (var Id in userTask.PostTasks)
                                {
                                    // Loop through post task nodes
                                    foreach (TreeNode postTaskNode in taskNode.Nodes)
                                    {
                                        // Compare this post task node Id name to user post task Id
                                        var postTask = (PostTask)postTaskNode.Tag;
                                        if (postTask.Id == Id)
                                        {
                                            // MATCH - so this post task is selected!
                                            postTaskNode.Checked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// TaskRx Load handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskRx_Load(object sender, EventArgs e)
        {
            // Start the async initialization process
            _ = InitializeFormAsync();
        }

        /// <summary>
        /// Asynchronously initializes the form
        /// </summary>
        private async Task InitializeFormAsync()
        {
            try
            {
                bool isUpdateMode;

                // Execute button is disabled until info is supplied
                executeButton.Enabled = false;

                // Load AllTasks.json
                await GetAllTasksAsync();

                // Use the user data path from PathConfig
                userDir = PathConfig.UserPath;

                // Get UserInfo.json (if present)
                await GetUserInfoAsync();

                // Load UserTasks.json(if present)
                isUpdateMode = await GetUserTasksAsync();

                // Populate tab controls with task groups - UI operations
                SetupTabControls();

                LoadCheckBoxes(setupTabControl);
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowCentered(
                    this, // Pass the owner form to center the dialog on it
                    $"Error initializing application: {ex.Message}",
                    "Initialization Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}