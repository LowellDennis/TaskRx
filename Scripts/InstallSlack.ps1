
# Define the download URL for the latest Slack Windows installer (64-bit)
$slackInstallerUrl = "https://slack.com/ssb/download-win64-msi"

# Define the local path to save the installer
$installerPath = "$env:TEMP\slack-standalone.msi"

# Function to check if Slack is already installed
function Is-SlackInstalled {
    $slackPath = "C:\Program Files\Slack\slack.exe"
    return (Test-Path $slackPath)
}

# Function to detect the user's domain
function Get-UserDomain {
    return $env:USERDOMAIN
}

# Function to construct the Slack workspace URL
function Get-WorkspaceUrl {
    $domain = Get-UserDomain
    return "https://$domain.slack.com"
}

# Log function
function Log {
    param (
        [string]$message
    )
    Write-Output $message
}

# Main script logic
Log "Starting Slack installation script..."

if (Is-SlackInstalled) {
    Log "Slack is already installed. Upgrading..."
} else {
    Log "Slack is not installed. Downloading and installing..."
}

# Download the installer
Log "Downloading Slack installer from $slackInstallerUrl..."
Invoke-WebRequest -Uri $slackInstallerUrl -OutFile $installerPath

# Install Slack silently
Log "Installing Slack silently..."
Start-Process -FilePath $installerPath /NoRestart /Silent

# Construct the workspace URL
$workspaceUrl = Get-WorkspaceUrl
Log "Launching Slack with workspace URL: $workspaceUrl"
Start-Process "slack://open" -ArgumentList "--url=$workspaceUrl"

Log "Slack installation script completed."
