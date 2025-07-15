using System.Data;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using TaskRx.Utilities;

namespace TaskRx
{
    public partial class TaskRx : Form
    {
        /// <summary>
        /// Add all hidden tasks tasks to the executuion list
        /// Note: Only takes place in setup mode
        /// </summary>
        private void AddHiddenTasks()
        {
            // Hidden tasks are only added in setup mode
            if (!isUpdateMode)
            {
                // Loop through all possible tasks
                foreach (var task in taskList.Tasks)
                {
                    // Look for hidden tasks
                    if (task.Hidden == true)
                    {
                        // Initialize info for this execution task
                        var taskItem = new ExecutionTask { Id = task.Id, Hidden = true, PostTasks = new List<ExecuteTask>() };

                        // Look for hidden post tasks
                        foreach (var postTask in task.PostTask)
                        {
                            if (postTask.Hidden)
                            {
                                // Add post task to execution task
                                ExecuteTask post = new ExecuteTask { Id = postTask.Id, Hidden = true };
                                executionSteps++;
                                taskItem.PostTasks.Add(post);
                            }
                        }

                        // Add task info to execution list
                        executionSteps++;
                        executionTasks.Add(taskItem);
                    }
                }
            }
        }

        /// <summary>
        /// Adds all selected (checked) tasks to the execution list
        /// </summary>
        /// <param name="tabControl"></param>
        private void AddCheckedTasks(TabControl tabControl)
        {
            // Loop through the tab pages
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                // Loop through the trees on the tab page
                foreach (TreeView treeView in tabPage.Controls.OfType<TreeView>())
                {
                    // Loop through the nodes on the tree
                    foreach (TreeNode taskNode in treeView.Nodes)
                    {
                        // Look for selected tasks
                        if (taskNode.Checked)
                        {
                            // Get task info
                            var task = (MainTask)taskNode.Tag;
                            var taskItem = new ExecutionTask { Id = task.Id, Hidden = false, PostTasks = new List<ExecuteTask>() };

                            // Loop through any post tasks for selected task
                            foreach (var postTask in specificPostTasks[task.Name])
                            {
                                // Look for hidden or selected post tasks
                                if (postTask.Hidden || taskNode.Nodes.Cast<TreeNode>().Any(n => n.Checked))
                                {
                                    // Add post task to execution task
                                    ExecuteTask post = new ExecuteTask { Id = postTask.Id, Hidden = postTask.Hidden };
                                    executionSteps++;
                                    taskItem.PostTasks.Add(post);
                                }
                            }

                            // Add task info to execution list
                            executionSteps++;
                            executionTasks.Add(taskItem);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves execution list to %USERPROFILE%\.TaskRx\UserTasks.json
        /// Note: Only takes place in setup mode and does not include any hidden tasks
        /// </summary>
        /// <param name="executionTasks"></param>
        private void SaveUserInfo()
        {
            // Save information from GUI to structure
            userInfo = new UserInfo();
            userInfo.First = txtFirst.Text;
            userInfo.Last = txtLast.Text;
            userInfo.Initials = txtInitials.Text;
            userInfo.Domain = txtDomain.Text;
            userInfo.Username = txtUsername.Text;
            userInfo.Work = txtWork.Text;
            userInfo.Personal = txtPersonal.Text;
            userInfo.ReposFolder = txtBase.Text;

            // Write resulting information to UserInfo.json file
            string userInfoFilePath = PathConfig.GetUserFilePath("UserInfo.json");
            var userInfoOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string userInfoJsonContent = JsonSerializer.Serialize(userInfo, userInfoOptions);
            File.WriteAllText(userInfoFilePath, userInfoJsonContent);
        }

        /// <summary>
        /// Saves execution list to %USERPROFILE%\.TaskRx\UserTasks.json
        /// Note: Does not include any hidden tasks
        /// </summary>
        /// <param name="executionTasks"></param>
        private void SaveUserTasks()
        {
            // Get all tasks from executionTasks that are not hidden
            string userTasksFilePath = PathConfig.GetUserFilePath("UserTasks.json");
            var tempTasks = executionTasks.Where(t => !t.Hidden);

            // Move the items to user tasks
            userTasks = new List<UserTask>();
            foreach (var task in tempTasks)
            {
                // Get user task information
                var user = new UserTask { Id = task.Id, PostTasks = new List<string>() };

                // Add any post task that are not hidden
                foreach (ExecuteTask postTask in task.PostTasks)
                {
                    if (!postTask.Hidden)
                    {
                        user.PostTasks.Add(postTask.Id);
                    }
                }

                // Add results to userTasks
                userTasks.Add(user);
            }

            // Write resulting tasks to UserTasks.json file
            var userTasksOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string userTasksJsonContent = JsonSerializer.Serialize(userTasks, userTasksOptions);
            File.WriteAllText(userTasksFilePath, userTasksJsonContent);
        }

        // Dictionary to track task execution status (taskId -> hasError)
        private Dictionary<string, bool> taskErrorStatus = new Dictionary<string, bool>();

        // Dictionary to track post-task execution status (taskId_postTaskId -> hasError)
        private Dictionary<string, bool> postTaskErrorStatus = new Dictionary<string, bool>();

        // Current task and post-task being executed
        private string currentTaskId = null;
        private string currentPostTaskId = null;

        private void Log(string message)
        {
            // Call the overloaded method with auto-detection of errors
            try
            {
                if (message == null) return;

                // Sanitize the message by removing control characters
                string sanitizedMessage = SanitizeString(message);

                // Determine if this is an error message
                bool isError = sanitizedMessage.IndexOf("error", StringComparison.OrdinalIgnoreCase) >= 0 ||
                               sanitizedMessage.IndexOf("exception", StringComparison.OrdinalIgnoreCase) >= 0 ||
                               sanitizedMessage.IndexOf("failed", StringComparison.OrdinalIgnoreCase) >= 0;

                // Call the overloaded method with the detected error status
                Log(message, isError);
            }
            catch (Exception ex)
            {
                // Last resort if something goes wrong with logging
                Debug.WriteLine($"Error in Log method: {ex.Message}");
            }
        }

        /// <summary>
        /// Logs a message to the debug console, log file, and optionally marks it as an error
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="isError">Whether the message is an error</param>
        private void Log(string message, bool isError)
        {
            try
            {
                if (message == null) return;

                // Sanitize the message by removing control characters
                string sanitizedMessage = SanitizeString(message);

                // Split the message by newlines to handle multi-line text
                string[] lines = sanitizedMessage.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                // Log each line separately to maintain proper formatting
                foreach (string line in lines)
                {
                    // Log using Serilog with the appropriate level
                    if (isError)
                    {
                        Serilog.Log.Error(line);
                    }
                    else
                    {
                        Serilog.Log.Information(line);
                    }
                }

                // Update error status for current task/post-task if this is an error
                if (isError)
                {
                    if (currentTaskId != null)
                    {
                        taskErrorStatus[currentTaskId] = true;

                        if (currentPostTaskId != null)
                        {
                            string postTaskKey = $"{currentTaskId}_{currentPostTaskId}";
                            postTaskErrorStatus[postTaskKey] = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Last resort if something goes wrong with logging
                Debug.WriteLine($"Error in Log method: {ex.Message}");
            }
        }

        /// <summary>
        /// Sanitizes a string by removing control characters that could cause issues in log files
        /// </summary>
        /// <param name="input">The input string to sanitize</param>
        /// <returns>A sanitized string with control characters removed</returns>
        private string SanitizeString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Create a StringBuilder with the same capacity as the input
            var sanitized = new System.Text.StringBuilder(input.Length);

            // Process each character
            foreach (char c in input)
            {
                // Keep only printable characters
                // This includes standard ASCII printable range and extended ASCII
                if (!char.IsControl(c) || c == '\t' || c == '\n' || c == '\r')
                {
                    sanitized.Append(c);
                }
            }

            return sanitized.ToString();
        }

        private void UpdateStatus(string message)
        {
            try
            {
                // Log the status update
                Log(message);

                // Update the UI on the UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        executionStatus.Text = "Executing: " + message;
                    }));
                }
                else
                {
                    executionStatus.Text = "Executing: " + message;
                }

                // Allow UI to update
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                // If we can't update the status, at least log the error
                Debug.WriteLine($"Error updating status: {ex.Message}");
            }
        }

        private void ExecutionStepCompleted()
        {
            try
            {
                // Update the progress bar on the UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        executionProgress.Increment(1);
                    }));
                }
                else
                {
                    executionProgress.Increment(1);
                }

                // Allow UI to update
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                // If we can't update the progress, at least log the error
                Debug.WriteLine($"Error updating progress: {ex.Message}");
            }
        }

        /// <summary>
        /// Execute all selected tasks and their post-tasks
        /// </summary>
        private async void ExecuteTasks()
        {
            try
            {
                // Initialize the progress bar
                executionProgress.Minimum = 0;
                executionProgress.Maximum = (int)executionSteps;
                executionProgress.Value = 0;

                // Create a command runner
                var runner = new Runner();

                // Initialize the variable dictionary for replacements
                InitializeVariableDictionary();

                // Update status with the start of execution message
                UpdateStatus("Starting task execution...");

                // Process each main task
                foreach (var executionTask in executionTasks)
                {
                    await ExecuteMainTask(executionTask, runner);
                }

                // Update status with completion message
                UpdateStatus("All tasks completed successfully");

                // Wait a moment to allow the user to see the completion message
                await Task.Delay(2000);

                // Ask the user if they want to exit the application
                // Show the message box and capture the result
                // Passing 'this' as the owner form will center the dialog on the application window
                DialogResult result = MessageBoxHelper.ShowCentered(
                    this, // Pass the owner form to center the dialog on the application window
                    "All tasks completed successfully. Do you want to exit the application?",
                    "Task Execution Complete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
                else
                {
                    // Tf the user chooses not to exit, the controls need to be reconfigured
                    ChangeState(false);
                }
            }
            catch (Exception ex)
            {
                // Log any fatal exceptions
                Log($"Fatal error in ExecuteTasks: {ex.Message}");
                Log($"Stack trace: {ex.StackTrace}");
                UpdateStatus($"Error: {ex.Message}");

                // Show error message to user centered on the form
                MessageBoxHelper.ShowCentered(
                    this, // Pass the owner form to center the dialog on it
                    $"An error occurred during execution: {ex.Message}",
                    "Execution Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Re-enable controls after error
                ChangeState(false);
            }
        }

        /// <summary>
        /// Updates the output text box with the given text and logs it to the log file
        /// </summary>
        /// <param name="text">The text to append to the output text box</param>
        /// <param name="isError">Whether the text is an error message (to be displayed in red)</param>
        private void UpdateOutputTextBox(string text, bool isError = false)
        {
            try
            {
                if (string.IsNullOrEmpty(text)) return;

                // Log the output to the log file with error status
                Log(text, isError);

                // Update the output text box on the UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        // Store the current selection position
                        int currentPosition = txtOutput.TextLength;

                        // Append the text
                        txtOutput.AppendText(text + Environment.NewLine);

                        // Apply color formatting if it's an error
                        if (isError)
                        {
                            // Select the text we just added
                            txtOutput.SelectionStart = currentPosition;
                            txtOutput.SelectionLength = text.Length;

                            // Set the color to red for error messages
                            txtOutput.SelectionColor = Color.Red;

                            // Reset selection
                            txtOutput.SelectionStart = txtOutput.TextLength;
                            txtOutput.SelectionLength = 0;
                        }

                        // Scroll to the end
                        txtOutput.SelectionStart = txtOutput.TextLength;
                        txtOutput.ScrollToCaret();
                    }));
                }
                else
                {
                    // Store the current selection position
                    int currentPosition = txtOutput.TextLength;

                    // Append the text
                    txtOutput.AppendText(text + Environment.NewLine);

                    // Apply color formatting if it's an error
                    if (isError)
                    {
                        // Select the text we just added
                        txtOutput.SelectionStart = currentPosition;
                        txtOutput.SelectionLength = text.Length;

                        // Set the color to red for error messages
                        txtOutput.SelectionColor = Color.Red;

                        // Reset selection
                        txtOutput.SelectionStart = txtOutput.TextLength;
                        txtOutput.SelectionLength = 0;
                    }

                    // Scroll to the end
                    txtOutput.SelectionStart = txtOutput.TextLength;
                    txtOutput.ScrollToCaret();
                }
            }
            catch (Exception ex)
            {
                // If we can't update the text box, at least log the error
                Debug.WriteLine($"Error updating output text box: {ex.Message}");
            }
        }

        /// <summary>
        /// Initializes the global variable dictionary with user information
        /// </summary>
        private void InitializeVariableDictionary()
        {
            // Create a new case-insensitive dictionary
            variableDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Add user information variables
            variableDictionary["FIRST_NAME"] = txtFirst.Text.Trim();
            variableDictionary["LAST_NAME"] = txtLast.Text.Trim();
            variableDictionary["FULL_NAME"] = txtFirst.Text.Trim() + " " + txtLast.Text.Trim();
            variableDictionary["DOMAIN"] = txtDomain.Text.Trim();
            variableDictionary["USERNAME"] = txtUsername.Text.Trim();
            variableDictionary["WORK_EMAIL"] = txtWork.Text.Trim();
            variableDictionary["PERSONAL_EMAIL"] = txtPersonal.Text.Trim();
            variableDictionary["REPOS_BASE"] = txtBase.Text.Trim();
            variableDictionary["SCRIPTS"] = PathConfig.ScriptsPath.Trim();
        }

        /// <summary>
        /// Replaces variables in the format ${VARIABLE_NAME} with their values from the global dictionary
        /// </summary>
        /// <param name="text">The text containing variables to replace</param>
        /// <returns>Text with variables replaced</returns>
        private string ReplaceVariables(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Use regex to find all ${VARIABLE_NAME} patterns
            var regex = new System.Text.RegularExpressions.Regex(@"\$\{([^\}]+)\}");
            return regex.Replace(text, match =>
            {
                string variableName = match.Groups[1].Value;
                if (variableDictionary.TryGetValue(variableName, out string value))
                    return value;
                else
                    return match.Value; // Keep the original if variable not found
            });
        }

        private async Task<(string Output, int ExitCode)> ExecuteCommandWithOutput(string command, string arguments, Runner runner)
        {
            if (string.IsNullOrEmpty(command))
            {
                return ("Command is empty", -1);
            }

            Log($"Running command: {command} {arguments}");

            try
            {
                // Execute the command with real-time output to the text box
                // Create a wrapper for the callback to handle the isError parameter
                var result = await runner.ExecuteCommand(command, arguments, (text, isError) => UpdateOutputTextBox(text, isError));

                // Log the completion (output is already logged in real-time)
                Log($"Command completed with exit code: {result.ExitCode}");

                return result;
            }
            catch (Exception ex)
            {
                Log($"Error executing command: {ex.Message}");
                throw; // Re-throw to be caught by the outer try-catch
            }
        }

        private async Task ExecuteMainTask(ExecutionTask executionTask, Runner runner)
        {
            try
            {
                // Get the main task details
                MainTask task = allTasks[executionTask.Id];

                // Replace variables in command and arguments
                string command = ReplaceVariables(task.Command);
                string arguments = ReplaceVariables(task.Arguments);

                // Add a horizontal double line separator between tasks
                UpdateOutputTextBox("=================================================", false);

                // Update status with task name
                UpdateStatus($"Executing: {task.Name}");

                // Execute the main task command if it exists
                if (!string.IsNullOrEmpty(command))
                {
                    await ExecuteCommandWithOutput(command, arguments, runner);
                }

                // Mark this step as completed
                ExecutionStepCompleted();

                // Process any post-tasks
                foreach (var postTask in executionTask.PostTasks)
                {
                    await ExecutePostTask(postTask, runner);
                }
            }
            catch (Exception ex)
            {
                Log($"Error in ExecuteMainTask: {ex.Message}");
                throw; // Re-throw to be caught by the outer try-catch
            }
        }

        /// <summary>
        /// Execute a single post-task
        /// </summary>
        private async Task ExecutePostTask(ExecuteTask postTask, Runner runner)
        {
            try
            {
                // Get the post task details
                PostTask task = allPostTasks[postTask.Id];

                // Replace variables in command and arguments
                string command = ReplaceVariables(task.Command);
                string arguments = ReplaceVariables(task.Arguments);

                // Add a horizontal single line separator between post-tasks
                UpdateOutputTextBox("-------------------------------------------------", false);

                // Update status with post task name
                UpdateStatus($"Executing post-task: {task.Name}");

                // Execute the post task command if it exists
                if (!string.IsNullOrEmpty(command))
                {
                    await ExecuteCommandWithOutput(command, arguments, runner);
                }

                // Mark this step as completed
                ExecutionStepCompleted();
            }
            catch (Exception ex)
            {
                Log($"Error in ExecutePostTask: {ex.Message}");
                throw; // Re-throw to be caught by the outer try-catch
            }
        }
    }
}