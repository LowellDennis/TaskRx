using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;
using CliWrap;
using CliWrap.EventStream;

namespace TaskRx.Utilities
{
    public class Runner
    {
        /// <summary>
        /// Executes a command asynchronously and captures its stdout and stderr seperately
        /// </summary>
        /// <param name="command">Command to be executed</param>
        /// <param name="arguments">Arguments for command (use "" if none)</param>
        /// <param name="workingDirectory">Directory from which to execute the command (current directory is used if "")</param>
        /// <returns>Tuple(StdOut string - multi-line, StdErr string - multi-line, ExitCode)</returns>
        private async Task<(string StdOut, string StdErr, int ExitCode)> RunCommandAsync(
            string command,
            string arguments,
            string workingDirectory = "")
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(command))
            {
                return ("", "Error: Command is null or empty", -1);
            }

            var stdOut = new StringBuilder();
            var stdErr = new StringBuilder();

            // For PowerShell commands, add a WindowStyle parameter to minimize the window
            string effectiveArguments = arguments;
            if (command.EndsWith("powershell.exe", StringComparison.OrdinalIgnoreCase) ||
                command.Equals("powershell", StringComparison.OrdinalIgnoreCase))
            {
                // Check if arguments already contain -WindowStyle
                if (!arguments.Contains("-WindowStyle"))
                {
                    // Add WindowStyle Minimized parameter
                    if (arguments.StartsWith("-Command", StringComparison.OrdinalIgnoreCase))
                    {
                        // Insert WindowStyle before the Command parameter
                        effectiveArguments = "-WindowStyle Minimized " + arguments;
                    }
                    else
                    {
                        // Add WindowStyle parameter
                        effectiveArguments = "-WindowStyle Minimized " + arguments;
                    }

                    // Log the modified command for debugging
                    Debug.WriteLine($"Modified PowerShell command: {command} {effectiveArguments}");
                }
            }

            var processStartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = effectiveArguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = string.IsNullOrWhiteSpace(workingDirectory)
                    ? Environment.CurrentDirectory
                    : workingDirectory
            };

            using var process = new Process { StartInfo = processStartInfo };

            process.OutputDataReceived += (sender, args) => { if (args.Data != null) stdOut.AppendLine(args.Data); };
            process.ErrorDataReceived += (sender, args) => { if (args.Data != null) stdErr.AppendLine(args.Data); };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            await process.WaitForExitAsync();

            return (stdOut.ToString(), stdErr.ToString(), process.ExitCode);
        }

        /// <summary>
        /// Executes a command and captures its output
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="arguments">The arguments to pass to the command</param>
        /// <param name="outputCallback">Optional callback to receive real-time output</param>
        /// <returns>A tuple containing the complete output and exit code</returns>
        public async Task<(string Output, int ExitCode)> ExecuteCommand(string command, string arguments, Action<string, bool>? outputCallback = null)
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(command))
            {
                string errorMsg = "Error: Command is null or empty";
                outputCallback?.Invoke(errorMsg, true);
                return (errorMsg, -1);
            }

            try
            {
                // Log what we're about to execute for debugging
                Debug.WriteLine($"Executing command: {command} {arguments}");

                // Get the Scripts directory path
                string scriptsPath;
                if (PathConfig.IsDevMode)
                {
                    scriptsPath = Path.Combine("D:\\HPE\\Dev\\code\\TaskRx", "Scripts");
                }
                else
                {
                    scriptsPath = Path.Combine(PathConfig.InstallPath, "Scripts");
                }

                // Check if the command executable exists in the Scripts directory or in the PATH
                string fullCommandPath = Path.Combine(scriptsPath, command);
                bool commandExists = File.Exists(fullCommandPath) || IsCommandInPath(command);

                if (!commandExists)
                {
                    string errorMsg = $"Error: Command not found: {command} (checked in {scriptsPath} and PATH)";
                    outputCallback?.Invoke(errorMsg, true);
                    return (errorMsg, -1);
                }

                // Create a StringBuilder to capture the complete output
                var stdOutBuffer = new StringBuilder();
                var stdErrBuffer = new StringBuilder();

                // Configure the command with the Scripts directory as the working directory
                // For PowerShell commands, add a WindowStyle parameter to minimize the window
                string effectiveArguments = arguments;
                if (command.EndsWith("powershell.exe", StringComparison.OrdinalIgnoreCase) ||
                    command.Equals("powershell", StringComparison.OrdinalIgnoreCase))
                {
                    // Check if arguments already contain -WindowStyle
                    if (!arguments.Contains("-WindowStyle"))
                    {
                        // Add WindowStyle Minimized parameter
                        if (arguments.StartsWith("-Command", StringComparison.OrdinalIgnoreCase))
                        {
                            // Insert WindowStyle before the Command parameter
                            effectiveArguments = "-WindowStyle Minimized " + arguments;
                        }
                        else
                        {
                            // Add WindowStyle parameter
                            effectiveArguments = "-WindowStyle Minimized " + arguments;
                        }

                        // Log the modified command for debugging
                        Debug.WriteLine($"Modified PowerShell command: {command} {effectiveArguments}");
                    }
                }

                var cmd = Cli.Wrap(command)
                    .WithArguments(effectiveArguments)
                    .WithWorkingDirectory(scriptsPath)
                    .WithValidation(CommandResultValidation.None); // Don't throw on non-zero exit codes

                // For PowerShell commands, add a WindowStyle parameter to minimize the window
                if (command.EndsWith("powershell.exe", StringComparison.OrdinalIgnoreCase) ||
                    command.Equals("powershell", StringComparison.OrdinalIgnoreCase))
                {
                    // Check if arguments already contain -WindowStyle
                    if (!arguments.Contains("-WindowStyle"))
                    {
                        // Add WindowStyle Minimized parameter
                        string newArgs = arguments;
                        if (newArgs.StartsWith("-Command", StringComparison.OrdinalIgnoreCase))
                        {
                            // Insert WindowStyle before the Command parameter
                            newArgs = "-WindowStyle Minimized " + newArgs;
                        }
                        else
                        {
                            // Add WindowStyle parameter
                            newArgs = "-WindowStyle Minimized " + newArgs;
                        }

                        // Update the command with the new arguments
                        cmd = cmd.WithArguments(newArgs);

                        // Log the modified command for debugging
                        Debug.WriteLine($"Modified PowerShell command: {command} {newArgs}");
                    }
                }

                // If we have a callback for real-time output, use Observe()
                if (outputCallback != null)
                {
                    // Create a task completion source to wait for the command to complete
                    var tcs = new TaskCompletionSource<(string Output, int ExitCode)>();
                    int exitCode = 0;

                    // Start the command and observe the events
                    using var subscription = cmd.Observe()
                        .Subscribe(
                            onNext: cmdEvent =>
                            {
                                switch (cmdEvent)
                                {
                                    case CliWrap.EventStream.StandardOutputCommandEvent stdOut:
                                        stdOutBuffer.AppendLine(stdOut.Text);
                                        outputCallback(stdOut.Text, false);
                                        break;

                                    case CliWrap.EventStream.StandardErrorCommandEvent stdErr:
                                        // Don't add the ERROR: prefix anymore, just log the original error text
                                        stdErrBuffer.AppendLine(stdErr.Text);
                                        // Pass true for isError to display in red
                                        outputCallback(stdErr.Text, true);
                                        break;

                                    case CliWrap.EventStream.ExitedCommandEvent exited:
                                        exitCode = exited.ExitCode;
                                        break;
                                }
                            },
                            onError: ex => tcs.SetException(ex),
                            onCompleted: () => tcs.SetResult((stdOutBuffer.ToString(), exitCode))
                        );

                    // Wait for the command to complete
                    var result = await tcs.Task;
                    Debug.WriteLine($"Command completed with exit code: {result.ExitCode}");

                    // Return the complete output and exit code
                    return result;
                }
                else
                {
                    // If no callback is provided, use the original ExecuteAsync method
                    var result = await cmd
                        .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                        .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                        .ExecuteAsync();

                    // Combine stdout and stderr
                    var output = stdOutBuffer.ToString();
                    if (stdErrBuffer.Length > 0)
                    {
                        output += Environment.NewLine + "ERROR: " + stdErrBuffer.ToString();
                    }

                    Debug.WriteLine($"Command completed with exit code: {result.ExitCode}");
                    return (output, result.ExitCode);
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"Exception: {ex.Message}\nStack trace: {ex.StackTrace}";
                Debug.WriteLine($"Exception in ExecuteCommand: {ex}");
                outputCallback?.Invoke(errorMsg, true);
                return (errorMsg, -1);
            }
        }

        // Helper method to check if a command is in the system PATH
        private bool IsCommandInPath(string command)
        {
            try
            {
                // Get the PATH environment variable
                string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;

                // Split the PATH into individual directories
                string[] pathDirs = pathEnv.Split(Path.PathSeparator);

                // Common executable extensions on Windows
                string[] extensions = new[] { ".exe", ".cmd", ".bat", ".com" };

                // Check if the command exists in any of the PATH directories
                foreach (string dir in pathDirs)
                {
                    if (string.IsNullOrEmpty(dir)) continue;

                    // Check for the command with each possible extension
                    string fullPath = Path.Combine(dir, command);
                    if (File.Exists(fullPath))
                    {
                        return true;
                    }

                    foreach (string ext in extensions)
                    {
                        fullPath = Path.Combine(dir, command + ext);
                        if (File.Exists(fullPath))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch
            {
                // If there's any error checking the PATH, assume the command doesn't exist
                return false;
            }
        }
    }
}