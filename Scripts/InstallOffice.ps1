param (
    [string]$domain = "AMERICAS",
    [string]$username = $(throw "Parameter 'username' is required."),
    [string]$email = $(throw "Parameter 'email' is required."),
    [string]$initials = $(throw "Parameter 'initials' is required.")
)

# Global Variables
$logIt   = $true
$logFile = "$env:USERPROFILE\Downloads\InstallOffice.txt"

function Write-Log {
    param (
        [string]$message
    )
    if ($logIt) {
        Write-Output $message | Out-File -Append -FilePath $logFile
    }
    Write-Host $message
}

# Function to download and extract the Office Deployment Tool
function DownloadAndExtractODT {
    param (
        [string]$odtUrl = "https://download.microsoft.com/download/6c1eeb25-cf8b-41d9-8d0d-cc1dbc032140/officedeploymenttool_18827-20140.exe",
        [string]$downloadDir = "$env:USERPROFILE\Downloads"
    )

    # Set paths
    $odtExePath = "$downloadDir\OfficeDeploymentTool.exe"

    # Create Downloads directory if it doesn't exist
    if (-not (Test-Path $downloadDir)) {
        Write-Log "Creating Downloads directory at $downloadDir..."
        try {
            New-Item -ItemType Directory -Path $downloadDir -Force | Out-Null
            Write-Log "Downloads directory created."
        } catch {
            Write-Log "Failed to create Downloads directory: $($_.Exception.Message)"
            exit 1
        }
    }

    # Download the Office Deployment Tool
    Write-Log "Downloading Office Deployment Tool..."
    try {
        Invoke-WebRequest -Uri $odtUrl -OutFile $odtExePath -ErrorAction Stop
        Write-Log "Downloaded Office Deployment Tool to $odtExePath."
    } catch {
        Write-Log "Failed to download Office Deployment Tool: $($_.Exception.Message)"
        exit 1
    }

    # Run the ODT executable to extract files
    Write-Log "Extracting Office Deployment Tool files..."
    if (Test-Path $odtExePath) {
        try {
            Start-Process -FilePath $odtExePath -ArgumentList "/extract:$downloadDir", "/quiet" -Wait
            Write-Log "Extraction completed successfully."
        } catch {
            Write-Log "Failed to extract Office Deployment Tool files: $($_.Exception.Message)"
            exit 1
        }
    } else {
        Write-Log "Office Deployment Tool executable not found."
        exit 1
    }
}

# Function to check installed Office applications
function GetInstalledOfficeApps {
    Write-Log "Checking installed Office applications..."

    $installedApps = @()

    # Registry paths for Office applications
    $officeRegistryPaths = @(
        "HKLM:\SOFTWARE\Microsoft\Office\ClickToRun\Configuration",
        "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
        "HKCU:\Software\Microsoft\Windows\CurrentVersion\Uninstall"
    )

    # List of Office applications to check
    $officeAppIdentifiers = @{
        "Word" = "Microsoft Word";
        "Excel" = "Microsoft Excel";
        "PowerPoint" = "Microsoft PowerPoint";
        "Outlook" = "Microsoft Outlook";
        "Access" = "Microsoft Access";
        "Publisher" = "Microsoft Publisher";
        "OneNote" = "Microsoft OneNote";
        "Visio" = "Microsoft Visio";
        "Teams" = "Microsoft Teams";
        "SkypeForBusiness" = "Skype for Business"
    }

    try {
        foreach ($registryPath in $officeRegistryPaths) {
            foreach ($appId in $officeAppIdentifiers.Keys) {
                $appName = $officeAppIdentifiers[$appId]
                $registryQuery = Get-ChildItem -Path $registryPath -ErrorAction SilentlyContinue | Where-Object {
                    $_.Name -match $appName
                }

                if ($registryQuery) {
                    Write-Log "$appName is installed."
                    $installedApps += $appId
                }
            }
        }
    } catch {
        Write-Log "Failed to check installed applications: $($_.Exception.Message)"
    }

    return $installedApps
}

function CreateODTConfig {
    param (
        [string]$outputPath = "$env:USERPROFILE\Downloads\configuration.xml",
        [ValidateSet("32", "64")]
        [string]$edition = "64",
        [ValidateSet("Current", "Monthly", "SemiAnnual", "SemiAnnualPreview")]
        [string]$channel = "Current",
        [ValidateSet("O365ProPlusRetail", "VisioProRetail", "ProjectProRetail")]
        [string]$productID = "O365ProPlusRetail",
        [string]$language = "en-us",
        [bool]$autoActivate = $true,
        [ValidateSet("None", "Full")]
        [string]$displayLevel = "None"
    )

    # Get already installed Office apps
    $installedApps = GetInstalledOfficeApps

    # Validate inputs for excluded apps
    $validApps = @("Access", "Publisher", "Outlook", "Teams", "SkypeForBusiness", "Word", "Excel", "PowerPoint", "OneNote", "Visio")
    foreach ($app in $installedApps) {
        if ($app -notin $validApps) {
            Write-Log "Invalid installed app detected: $app. Skipping."
        }
    }

    # Build XML content
    Write-Log "Creating ODT configuration XML file..."
    $xmlContent = @"
<Configuration>
  <Add OfficeClientEdition="$edition" Channel="$channel">
    <Product ID="$productID">
      <Language ID="$language" />
"@

    foreach ($app in $installedApps) {
        $xmlContent += "      <ExcludeApp ID=""$app"" />`n"
    }

    $xmlContent += @"
    </Product>
  </Add>
  <Display Level="$displayLevel" AcceptEULA="True" />
"@

    if ($autoActivate) {
        $xmlContent += "  <Property Name=""AUTOACTIVATE"" Value=""1"" />`n"
    }

    $xmlContent += "</Configuration>"

    try {
        Set-Content -Path $outputPath -Value $xmlContent
        Write-Log "ODT Configuration XML file created at $outputPath."
    } catch {
        Write-Log "Failed to create ODT configuration XML file: $($_.Exception.Message)"
        exit 1
    }
}

function InstallOfficeWithODT {
    param (
        [string]$odtSetupPath = "$env:USERPROFILE\Downloads\setup.exe",
        [string]$configPath = "$env:USERPROFILE\Downloads\configuration.xml"
    )

    Write-Log "Starting Office install/upgrade using ODT..."

    # Check if configuration file excludes all applications
    $excludedApps = Get-Content -Path $configPath | Select-String -Pattern "<ExcludeApp"
    if ($excludedApps.Count -eq 0) {
        Write-Log "No applications to install or upgrade using ODT. Skipping ODT execution."
        return
    }

    if ((Test-Path $odtSetupPath) -and (Test-Path $configPath)) {
        try {
            $process = Start-Process -FilePath $odtSetupPath -ArgumentList "/configure $configPath" -PassThru -WindowStyle Hidden
            Write-Host -NoNewline "Install/upgrade processing"
            $timeout = 3600 # Timeout after 1 hour
            $elapsedTime = 0
            while (-not $process.HasExited -and $elapsedTime -lt $timeout) {
                Write-Host -NoNewline "."
                Start-Sleep -Seconds 5
                $elapsedTime += 5
            }

            if ($process.HasExited) {
                if ($process.ExitCode -eq 0) {
                    Write-Log "Office install/upgrade using ODT completed successfully."
                } else {
                    Write-Log "Office install/upgrade using ODT failed with exit code: $($process.ExitCode)"
                    exit 1
                }
            } else {
                Write-Log "Office install/upgrade with ODT timed out after $($timeout / 60) minutes."
                exit 1
            }
        } catch {
            Write-Log "Failed to install/upgrade Office with ODT: $($_.Exception.Message)"
            exit 1
        }
    } else {
        Write-Log "Either setup.exe or configuration.xml not found in the Downloads directory."
        exit 1
    }
}

function Cleanup {
    Write-Log "Cleaning up temporary files..."
    $filesToRemove = @("$env:USERPROFILE\Downloads\OfficeDeploymentTool.exe", "$env:USERPROFILE\Downloads\configuration.xml", "$env:USERPROFILE\Downloads\setup.exe")
    foreach ($file in $filesToRemove) {
        if (Test-Path $file) {
            try {
                Remove-Item -Path $file -Force
                Write-Log "Removed temporary file: $file"
            } catch {
                Write-Log "Failed to remove temporary file: $file. Error: $($_.Exception.Message)"
            }
        }
    }
}

# Auto-elevation check
if (-not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole(`
    [Security.Principal.WindowsBuiltInRole] "Administrator")) {

    $paramList = $PSBoundParameters.GetEnumerator() | ForEach-Object {
        $key = $_.Key
        $value = $_.Value
        if ($value -is [switch] -and $value.IsPresent) {
            "-$key"
        } elseif ($value -is [string]) {
            "-$key `"$value`""
        } else {
            "-$key $value"
        }
    }

    $scriptPath = $MyInvocation.MyCommand.Definition
    $paramString = $paramList -join ' '

    Start-Process powershell -Verb RunAs -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$scriptPath`" $paramString"
    Get-Content -Path $logFile
    Remove-Item -Path $logFile -Force
} else {
    $logIt = $false
}

# Main Script Execution
try {
    Write-Log "Starting the Microsoft Office Install/Upgrade script..."
    DownloadAndExtractODT
    CreateODTConfig
    InstallOfficeWithODT
    Cleanup
    Write-Log "Microsoft Office installation/upgrade completed successfully."
} catch {
    Write-Log "An error occurred during installation: $($_.Exception.Message)"
    exit 1
}
