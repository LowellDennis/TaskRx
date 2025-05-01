namespace TaskRx
{
    public partial class TaskRx : Form
    {
        // This file contains modifications to the existing Log method
        // to use Serilog with the required format

        /// <summary>
        /// Logs a task divider (80 equals signs)
        /// </summary>
        private void LogTaskDivider()
        {
            string divider = new string('=', 80);
            Serilog.Log.ForContext("IsDivider", true).Information(divider);

            // Also log to the output text box
            UpdateOutputTextBox(divider, false);
        }

        /// <summary>
        /// Logs a post-task divider (80 hyphens)
        /// </summary>
        private void LogPostTaskDivider()
        {
            string divider = new string('-', 80);
            Serilog.Log.ForContext("IsDivider", true).Information(divider);

            // Also log to the output text box
            UpdateOutputTextBox(divider, false);
        }
    }
}