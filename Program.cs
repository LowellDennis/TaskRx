using System.Text;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using TaskRx.Utilities;

namespace TaskRx
{
    internal static class Program
    {
        /// <summary>
        /// Helper method to show an error message and exit the application
        /// </summary>
        /// <param name="message">The error message to display</param>
        /// <param name="title">The title of the message box</param>
        /// <param name="errorCode">The error code to exit with (default: 1)</param>
        private static void ExitWithMessage(string message, string title, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Error, int errorCode = 1)
        {
            MessageBoxHelper.ShowCentered(message, title, buttons, icon);
            Environment.Exit(errorCode); // Exit the program with the specified error code
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configure Serilog with a custom destructuring policy to sanitize strings and custom output format
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(
                    PathConfig.LogFile,
                    outputTemplate: "{Timestamp:yyyyMMddHHmmssfff} [{LevelMap}] {Message}{NewLine}{Exception}",
                    formatProvider: null,
                    shared: true)
                .Enrich.With(new CustomLevelMapEnricher())
                .Destructure.With<ControlCharacterDestructuringPolicy>()
                .CreateLogger();

            // Log the paths being used
            Log.Information($"Application running in {(PathConfig.IsDevMode ? "development" : "production")} mode");
            Log.Information($"Install path: {PathConfig.InstallPath}");
            Log.Information($"User data path: {PathConfig.UserPath}");
            Log.Information($"Log path: {PathConfig.LogPath}");
            Log.Information($"ExePath: {PathConfig.ExePath}");
            Log.Information($"ProjectPath: {PathConfig.ProjectPath}");
            Log.Information($"ScriptsPath: {PathConfig.ScriptsPath}");

            // Verify that the Scripts directory exists
            if (!Directory.Exists(PathConfig.ScriptsPath))
            {
                Log.Error("Scripts directory does not exist!");
                ExitWithMessage("Scripts directory does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Log.Information("Scripts directory exists!");
                // List some files in the Scripts directory to verify it's the correct one
                string[] files = Directory.GetFiles(PathConfig.ScriptsPath, "*.cmd");
                foreach (string file in files)
                {
                    Log.Information($"Found script: {Path.GetFileName(file)}");
                }
            }

            // Add a divider after startup information
            string divider = new string('=', 80);
            Log.ForContext("IsDivider", true).Information(divider);

            try
            {
                Log.Information("Starting application");

                // To customize application AllTasks such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();

                // Use a synchronous approach to initialize the form
                RunApplication();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
                MessageBoxHelper.ShowCentered($"Error starting application: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Checks if the application is running in update mode by verifying if UserInfo.json and UserTasks.json exist
        /// </summary>
        /// <returns>True if both files exist, indicating update mode; otherwise, false</returns>
        private static bool IsUpdateMode()
        {
            string userInfoPath = PathConfig.GetUserFilePath("UserInfo.json");
            string userTasksPath = PathConfig.GetUserFilePath("UserTasks.json");

            bool isUpdate = File.Exists(userInfoPath) && File.Exists(userTasksPath);

            if (isUpdate)
            {
                Log.Information("Running in update mode - UserInfo.json and UserTasks.json found");
            }
            else
            {
                Log.Information("Running in initial setup mode - UserInfo.json and/or UserTasks.json not found");
            }

            return isUpdate;
        }

        private static void RunApplication()
        {
            // Check if we're in update mode
            bool updateMode = IsUpdateMode();

            // Show prerequisites message box only if not in update mode
            if (!updateMode)
            {
                // Show prerequisites message box before starting the application
                string message = "For TaskRx to run the following conditions must be met:\n\n" +
                                 "   • You must be connected to the Internet.\n" +
                                 "   • You must be fully authenticated with HPE.\n" +
                                 "     (VPN, VirtualDigitalBadge, Okta)\n" +
                                 "   • You must be running on HPE owned hardware.\n\n" +
                                 "Click 'Yes' if all of these conditions are met.\n" +
                                 "Click 'No' if one or more of these conditions are not met.\n\n" +
                                 "Proceed?";

                DialogResult result = MessageBoxHelper.ShowCentered(
                    message,
                    "TaskRx Prerequisites",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    Log.Information("User chose not to proceed due to prerequisites not being met");
                    Environment.Exit(0);
                }
            }

            // Create and run the form
            var form = new TaskRx();
            Application.Run(form);
        }
    }

    /// <summary>
    /// Custom enricher to map log levels to the required format (INF, OUT, ERR, DIV)
    /// </summary>
    public class CustomLevelMapEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            string levelMap = logEvent.Level switch
            {
                LogEventLevel.Information => "INF",
                LogEventLevel.Error => "ERR",
                LogEventLevel.Fatal => "ERR",
                LogEventLevel.Warning => "OUT",
                LogEventLevel.Debug => "OUT",
                LogEventLevel.Verbose => "OUT",
                _ => "INF"
            };

            // Check if this is a divider message
            if (logEvent.Properties.TryGetValue("IsDivider", out var isDividerProperty) &&
                isDividerProperty is ScalarValue scalarValue &&
                scalarValue.Value is bool isDivider && isDivider)
            {
                levelMap = "DIV";
            }

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("LevelMap", levelMap));
        }
    }

    /// <summary>
    /// Custom destructuring policy for Serilog to sanitize strings by removing control characters
    /// </summary>
    public class ControlCharacterDestructuringPolicy : IDestructuringPolicy
    {
        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out LogEventPropertyValue? result)
        {
            // Only process string values
            if (value is string stringValue)
            {
                // Sanitize the string by removing control characters
                string sanitized = SanitizeString(stringValue);

                // Create a scalar value from the sanitized string
                // Note: Multi-line handling is done in the Log method, not here
                result = propertyValueFactory.CreatePropertyValue(sanitized, true);
                return true;
            }

            // For non-string values, indicate that this policy doesn't apply
            result = null;
            return false;
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
            var sanitized = new StringBuilder(input.Length);

            // Process each character
            foreach (char c in input)
            {
                // Keep only printable characters
                // This includes standard ASCII printable range and extended ASCII
                // We preserve newlines for proper multi-line handling
                if (!char.IsControl(c) || c == '\t' || c == '\n' || c == '\r')
                {
                    sanitized.Append(c);
                }
                else
                {
                    // Skip control characters
                }
            }

            return sanitized.ToString();
        }
    }
}