using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskRx.Utilities;

namespace TaskRx
{
    public partial class TaskRx : Form
    {
        /// <summary>
        /// Execute a single main task and its post-tasks
        /// </summary>
        private async Task ExecuteMainTask(ExecutionTask executionTask, Runner runner)
        {
            try
            {
                // Get the main task details
                MainTask task = allTasks[executionTask.Id];
                string name = isUpdateMode ? task.UpdateName : task.SetupName;
                string command = isUpdateMode ? task.UpdateCommand : task.SetupCommand;
                string arguments = isUpdateMode ? task.UpdateArguments : task.SetupArguments;

                // Add a task divider before starting a new task
                LogTaskDivider();
                
                // Set the current task for HTML logging
                SetCurrentTask(executionTask.Id, name);

                // Update status with task name
                UpdateStatus($"Executing: {name}");
                
                // Execute the main task command if it exists
                if (!string.IsNullOrEmpty(command))
                {
                    Log($"Running command: {command} {arguments}");
                    
                    try
                    {
                        var result = await runner.ExecuteCommand(command, arguments, (output, isError) => 
                        {
                            // Log real-time output
                            Log(output, isError);
                        });
                        
                        // Log the result
                        Log($"Command completed with exit code: {result.ExitCode}");
                        
                        // Check for errors
                        if (result.ExitCode != 0)
                        {
                            Log($"Error: Command failed with exit code {result.ExitCode}", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log any exceptions
                        Log($"Error executing command: {ex.Message}");
                    }
                }
                
                // Mark the task as completed
                ExecutionStepCompleted();
                
                // Process each post-task
                foreach (var postTask in executionTask.PostTasks)
                {
                    await ExecutePostTask(postTask, task, runner);
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions
                Log($"Error executing task: {ex.Message}");
                Log($"Stack trace: {ex.StackTrace}");
            }
        }
        
        /// <summary>
        /// Execute a single post-task
        /// </summary>
        private async Task ExecutePostTask(ExecuteTask postTask, MainTask parentTask, Runner runner)
        {
            try
            {
                // Get the post-task details
                PostTask task = allPostTasks[postTask.Id];
                string name = isUpdateMode ? task.UpdateName : task.SetupName;
                string command = isUpdateMode ? task.UpdateCommand : task.SetupCommand;
                string arguments = isUpdateMode ? task.UpdateArguments : task.SetupArguments;
                
                // Add a post-task divider before starting a new post-task
                LogPostTaskDivider();
                
                // Set the current post-task for HTML logging
                SetCurrentPostTask(postTask.Id, name);
                
                // Update status with post-task name
                UpdateStatus($"Executing post-task: {name}");
                
                // Execute the post-task command if it exists
                if (!string.IsNullOrEmpty(command))
                {
                    Log($"Running command: {command} {arguments}");
                    
                    try
                    {
                        var result = await runner.ExecuteCommand(command, arguments, (output, isError) => 
                        {
                            // Log real-time output
                            Log(output, isError);
                        });
                        
                        // Log the result
                        Log($"Command completed with exit code: {result.ExitCode}");
                        
                        // Check for errors
                        if (result.ExitCode != 0)
                        {
                            Log($"Error: Command failed with exit code {result.ExitCode}", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log any exceptions
                        Log($"Error executing command: {ex.Message}");
                    }
                }
                
                // Mark the post-task as completed
                ExecutionStepCompleted();
            }
            catch (Exception ex)
            {
                // Log any exceptions
                Log($"Error executing post-task: {ex.Message}");
                Log($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}