namespace TaskRx
{
    public partial class TaskRx : Form
    {
        // Global variable containing all tasks after load
        private TaskList taskList = new TaskList();

        // Global variable for looking up task information by its Id
        private Dictionary<string, MainTask> allTasks = new Dictionary<string, MainTask>();

        // Global variable for looking up post task information by its Id
        private Dictionary<string, PostTask> allPostTasks = new Dictionary<string, PostTask>();

        // Global variable for looking up post task information by its Id
        private Dictionary<string, List<PostTask>> specificPostTasks = new Dictionary<string, List<PostTask>>();

        // Global varible containing user info after load (if present)
        private UserInfo userInfo = new UserInfo();

        // Global varible containing user tasks after load (if present)
        private List<UserTask> userTasks = new List<UserTask>();

        // Global variable containing the current user's domain
        private string Domain = string.Empty;

        // Global variable containing the current user's initials
        private string Initials = string.Empty;

        // Global variable containing the current user's username
        private string Username = string.Empty;

        // Global variable containing the current user's work Email address
        private string WorkEmail = string.Empty;

        // Global variable containing the current user's personal Email address
        private string PersonalEmail = string.Empty;

        // Global variable containing the base address of the repositories
        private string ReposFolder = string.Empty;

        // Global varaiable containing tasks to be executed after execute button is pressed
        private List<ExecutionTask> executionTasks = new List<ExecutionTask>();

        // Global variable containing the number of steps to be executed
        private int executionSteps = 0;

        // Global variable indicating if current mode is update (setupif not)
        private bool isUpdateMode = false;

        // Global variable indicating user directory for storing UserTasks.json
        private string userDir = string.Empty;

        // Global variable containing variable replacements for command arguments
        private Dictionary<string, string>? variableDictionary = new Dictionary<string, string>();
    }

    /// <summary>
    /// Format of a post task entry in AllTasks.json
    /// </summary>
    public class PostTask
    {
        public string? Id { get; set; }
        public bool Auto { get; set; }
        public bool Workstation { get; set; }
        public bool Jumpstation { get; set; }
        public string Name { get; set; } = string.Empty; // Fix: Initialize with a default value
        public string Command { get; set; } = string.Empty; // Fix: Initialize with a default value
        public string Arguments { get; set; } = string.Empty; // Fix: Initialize with a default value
    }

    /// <summary>
    /// Format of a task entry in AllTasks.json
    /// </summary>
    public class MainTask
    {
        public string Id { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public bool Auto { get; set; }
        public bool Workstation { get; set; }
        public bool Jumpstation { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string Arguments { get; set; } = string.Empty;
        public List<PostTask> PostTask { get; set; } = new List<PostTask>();
    }

    /// <summary>
    /// Format of a tasks in AllTasks.json
    /// </summary>
    public class TaskList
    {
        public List<MainTask>? Tasks { get; set; }
    }

    /// <summary>
    /// Format of user information in UserInfo.json
    /// </summary>
    public class UserInfo
    {
        public string? First { get; set; }
        public string? Last { get; set; }
        public string? Domain { get; set; }
        public string? Initials { get; set; }
        public string? Username { get; set; }
        public string? Work { get; set; }
        public string? Personal { get; set; }
        public string? ReposFolder { get; set; }
    }

    /// <summary>
    /// Format of an task entry in UserTasks.json
    /// Also used as the format of the execution list that is built
    /// </summary>
    public class UserTask
    {
        public string Id { get; set; } = string.Empty;
        public List<string> PostTasks { get; set; } = new List<string>();
    }

    /// <summary>
    /// Format of an task entry in UserTasks.json
    /// Also used as the format of the execution list that is built
    /// </summary>
    public class ExecuteTask
    {
        public string? Id { get; set; }
        public bool Auto { get; set; }
    }

    /// <summary>
    /// Format of an task entry in UserTasks.json
    /// Also used as the format of the execution list that is built
    /// </summary>
    public class ExecutionTask
    {
        public string Id { get; set; } = string.Empty;
        public bool Auto { get; set; }
        public List<ExecuteTask> PostTasks { get; set; } = new List<ExecuteTask>();
    }
}