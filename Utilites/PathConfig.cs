namespace TaskRx.Utilities
{
    /// <summary>
    /// Helper class to manage file paths for both development and production environments
    /// </summary>
    public static class PathConfig
    {
        public static string UserPath = string.Empty;    // %USERPROFILE%\.TaskRx)
        public static string LogPath = string.Empty;    // %USERPROFILE%\.TaskRx\logs)
        public static string ExePath = string.Empty;    // Path of the current executable)
        public static string InstallPath = string.Empty;    // Path where tool is insalled
        public static string ProjectPath = string.Empty;    // Path of project files
        public static string ScriptsPath = string.Empty;    // Path to scripts directory
        public static string LogFile = string.Empty;    // Path to log file
        public static bool IsDevMode = false;           // Indicated mode of execution, True=development, False=production

        static PathConfig()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize the path configuration
        /// </summary>
        public static void Initialize()
        {
            // Get the path to the user profile directory
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // Path to user directory
            UserPath = Path.Combine(userProfile, ".TaskRx");

            // Log path is always a subdirectory of the the user directory
            LogPath = Path.Combine(UserPath, "logs");

            // Get path to the execuatable
            ExePath = AppDomain.CurrentDomain.BaseDirectory;

            // Set the installation path to the executable directory
            InstallPath = ExePath;

            // See if executing from a typical build output directory
            bool runningFromBuildOutput = ExePath.Contains("bin\\Debug") || ExePath.Contains("bin\\Release");

            // See if source code files exist in parent directories
            // (may have to navigate up to 3 levels to find project files)
            string projectRootCandidate = ExePath;
            bool sourceFilesExist = false;
            for (int i = 0; i < 3; i++)
            {
                projectRootCandidate = Path.GetDirectoryName(projectRootCandidate) ?? string.Empty;
                if (string.IsNullOrEmpty(projectRootCandidate)) break;

                // Check for typical development files
                if (File.Exists(Path.Combine(projectRootCandidate, "TaskRx.csproj")) ||
                    File.Exists(Path.Combine(projectRootCandidate, "TaskRx.sln")))
                {
                    sourceFilesExist = true;
                    break;
                }
            }

            // Indicate mode of execution
            IsDevMode = runningFromBuildOutput || sourceFilesExist;

            // Set the project path based on mode of execution
            if (IsDevMode)
            {
                // In development, use the detected project root if available
                if (sourceFilesExist && !string.IsNullOrEmpty(projectRootCandidate))
                {
                    ProjectPath = projectRootCandidate;
                }
                else
                {
                    // Fallback to the hardcoded project directory if detection fails
                    ProjectPath = "d:/HPE/Dev/code/TaskRx";
                }
            }
            else
            {
                // In production, this is the same as the InstallPath
                ProjectPath = InstallPath;
            }

            // Scripts path is always a subdirectory of the project directory
            ScriptsPath = Path.Combine(ProjectPath, "Scripts");

            // Ensure user and log paths exist
            Directory.CreateDirectory(UserPath);
            Directory.CreateDirectory(LogPath);
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            LogFile = Path.Combine(LogPath, $"TaskRxLog_{timestamp}.log");

            // Delete existing log file if it exists
            if (File.Exists(LogFile))
            {
                File.Delete(LogFile);
            }
        }

        /// <summary>
        /// Gets the path to a file in the installation directory
        /// </summary>
        public static string GetInstallFilePath(string fileName)
        {
            return Path.Combine(InstallPath, fileName);
        }

        /// <summary>
        /// Gets the path to a file in the user directory
        /// </summary>
        public static string GetUserFilePath(string fileName)
        {
            return Path.Combine(UserPath, fileName);
        }

        /// <summary>
        /// Gets the path to a file in the log directory
        /// </summary>
        public static string GetLogFilePath(string fileName)
        {
            return Path.Combine(LogPath, fileName);
        }

        /// <summary>
        /// Gets the path to a file in the project directory
        /// </summary>
        public static string GetProjectFilePath(string fileName)
        {
            return Path.Combine(ProjectPath, fileName);
        }

        /// <summary>
        /// Gets the path to a file in the script directory
        /// </summary>
        public static string GetScriptFilePath(string fileName)
        {
            return Path.Combine(ScriptsPath, fileName);
        }
    }
}