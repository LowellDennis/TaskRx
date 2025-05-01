# Ensure running as Administrator
$adminCheck = [System.Security.Principal.WindowsPrincipal] [System.Security.Principal.WindowsIdentity]::GetCurrent()
if (-not $adminCheck.IsInRole([System.Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Output "This script must be run as Administrator. Exiting..."
    exit 1
}
Write-Output "Administrator execution verified!"

# Define the download path in the user's Downloads folder
$downloadPath = [System.IO.Path]::Combine([System.Environment]::GetFolderPath("UserProfile"), "Downloads", "AppInstaller.msixbundle")

# Check if Winget is installed
$wingetInstalled = $false
try {
    $installedVersion = winget --version
    Write-Output "Winget version $installedVersion currently installed!"
    $wingetInstalled = $true
} catch {
    $installedVersion = "0.0.0"  # Default if not installed
    Write-Output "Winget is not installed!"
}

# If Winget is already installed, check for the latest version
$updateRequired = $false
if ($wingetInstalled) {
    try {
        $latestVersion = (Invoke-RestMethod -Uri "https://api.github.com/repos/microsoft/winget-cli/releases/latest").tag_name -replace "[^0-9\.]"
        Write-Output "Latest available Winget version: $latestVersion"

        if ($installedVersion -lt $latestVersion) {
            Write-Output "A newer version of Winget is available."
            $updateRequired = $true
        } else {
            Write-Output "Winget is already up to date!"
            exit 0
        }
    } catch {
        Write-Output "Failed to fetch the latest Winget version. Skipping update check."
        exit 2
    }
}

# Download and install/update Winget (App Installer)
Write-Output "Downloading Winget installer to: $downloadPath"
Invoke-WebRequest -Uri "https://aka.ms/getwinget" -OutFile $downloadPath -UseBasicParsing
Add-AppxPackage -Path $downloadPath -ForceUpdate

# Display a clear final message
if (-not $wingetInstalled) {
    Write-Output "Winget has been successfully installed!"
} elseif ($updateRequired) {
    Write-Output "Winget has been successfully updated to version $latestVersion!"
} else {
    Write-Output "Winget was already up to date. No changes were made."
}
