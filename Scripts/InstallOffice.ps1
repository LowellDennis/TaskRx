param (
    [string]$domain = "AMERICAS",
    [string]$username = $(throw "Parameter 'username' is required."),
    [string]$email = $(throw "Parameter 'email' is required."),
    [string]$initials = $(throw "Parameter 'initials' is required.")
)

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
        Write-Host "Creating Downloads directory at $downloadDir..."
        try {
            New-Item -ItemType Directory -Path $downloadDir -Force | Out-Null
            Write-Host "Downloads directory created."
        } catch {
            Write-Host "Failed to create Downloads directory: $($_.Exception.Message)"
            exit 1
        }
    }

    # Download the Office Deployment Tool
    Write-Host "Downloading Office Deployment Tool..."
    try {
        Invoke-WebRequest -Uri $odtUrl -OutFile $odtExePath -ErrorAction Stop
        Write-Host "Downloaded Office Deployment Tool to $odtExePath."
    } catch {
        Write-Host "Failed to download Office Deployment Tool: $($_.Exception.Message)"
        exit 1
    }

    # Run the ODT executable to extract files
    Write-Host "Extracting Office Deployment Tool files..."
    if (Test-Path $odtExePath) {
        try {
            Start-Process -FilePath $odtExePath -ArgumentList "/extract:$downloadDir", "/quiet" -Wait
            Write-Host "Extraction completed successfully."
        } catch {
            Write-Host "Failed to extract Office Deployment Tool files: $($_.Exception.Message)"
            exit 1
        }
    } else {
        Write-Host "Office Deployment Tool executable not found."
        exit 1
    }
}

# Function to check installed Office applications
function GetInstalledOfficeApps {
    $results = @{}

    # List off all Office applications
    $knownApps = @{
        "Word"       = "WINWORD.EXE"
        "Excel"      = "EXCEL.EXE"
        "PowerPoint" = "POWERPNT.EXE"
        "Outlook"    = "OUTLOOK.EXE"
        "Access"     = "MSACCESS.EXE"
        "Publisher"  = "MSPUB.EXE"
        "OneNote"    = "ONENOTE.EXE"
        "Teams"      = "Teams.exe"
        "Skype"      = "Skype.exe"
        "Visio"      = "VISIO.EXE"
        "Project"    = "WINPROJ.EXE"
    }

    # Initialize results
    foreach ($app in $knownApps.Keys) {
        $results[$app] = $false
    }

    # --- Method 1: Click-to-Run Registry Detection ---
    $c2rKey = "HKLM:\SOFTWARE\Microsoft\Office\ClickToRun\Configuration"
    if (Test-Path $c2rKey) {
        $releaseIds = (Get-ItemProperty -Path $c2rKey -ErrorAction SilentlyContinue).ProductReleaseIds
        if ($releaseIds) {
            $appsFromRegistry = $releaseIds -split ',' | ForEach-Object { $_.Trim() }
            foreach ($app in $appsFromRegistry) {
                if ($results.ContainsKey($app)) {
                    $results[$app] = $true
                }
            }
        }
    }

    # --- Method 2: Executable Detection in Common Paths ---
    $possiblePaths = @(
        "${env:ProgramFiles}\Microsoft Office\root\Office16",
        "${env:ProgramFiles(x86)}\Microsoft Office\root\Office16",
        "${env:ProgramFiles}\Microsoft Office\Office16",
        "${env:ProgramFiles(x86)}\Microsoft Office\Office16"
    )

    foreach ($path in $possiblePaths) {
        foreach ($app in $knownApps.GetEnumerator()) {
            $exePath = Join-Path $path $app.Value
            if (Test-Path $exePath) {
                $results[$app.Key] = $true
            }
        }
    }

    # --- Method 3: COM Object Instantiation ---
    $comMap = @{
        "Word"       = "Word.Application"
        "Excel"      = "Excel.Application"
        "Outlook"    = "Outlook.Application"
        "PowerPoint" = "PowerPoint.Application"
        "Access"     = "Access.Application"
        "Publisher"  = "Publisher.Application"
    }

    foreach ($app in $comMap.GetEnumerator()) {
        try {
            $com = New-Object -ComObject $app.Value -ErrorAction Stop
            $results[$app.Key] = $true
            $com.Quit() | Out-Null
        } catch {
            # Not installed
        }
    }

    # --- Optional: Method 4 — WMI fallback (disabled by default) ---
    # WARNING: This method is slow and can trigger Office repairs.
    # To enable, uncomment below block.

    <#
    $wmiApps = Get-WmiObject -Class Win32_Product | Where-Object {
        $_.Name -match "Office|Word|Excel|PowerPoint|Outlook|Access|Publisher|Visio|Project"
    }
    foreach ($item in $wmiApps) {
        foreach ($key in $results.Keys) {
            if ($item.Name -match $key) {
                $results[$key] = $true
            }
        }
    }
    #>

    # Return only installed apps
    return $results.GetEnumerator() | Where-Object { $_.Value } | Select-Object -ExpandProperty Key
}

function CreateODTConfig {
    param (
        [string[]]$includeApps = @("Excel", "OneNote", "PowerPoint", "Visio", "Word"),
        [string[]]$excludeApps = @("Access", "Outlook", "Project", "Publisher", "SkypeforBusiness", "Teams")
    )

    # Define Office365 App IDs (used for ExcludeApp)
    $odtApps = @("Access", "Excel", "OneNote", "Outlook", "PowerPoint", "Publisher", "SkypeforBusiness", "Teams", "Word")

    # Define supported standalone App IDs (not part of ExcludeApp)
    $standaloneApps = @("Project", "Visio")

    # Validate inputs
    $invalidIncludes = $includeApps | Where-Object { ($odtApps + $standaloneApps) -notcontains $_ }
    $invalidExcludes = $excludeApps | Where-Object { ($odtApps + $standaloneApps) -notcontains $_ }
    if ($invalidIncludes.Count -gt 0 -or $invalidExcludes.Count -gt 0) {
        Write-Warning "Invalid app IDs detected:"
        if ($invalidIncludes) { Write-Warning "  Invalid includeApps: $($invalidIncludes -join ', ')" }
        if ($invalidExcludes) { Write-Warning "  Invalid excludeApps: $($invalidExcludes -join ', ')" }
    }

    # Filter only valid entries
    $includeApps = $includeApps | Where-Object { ($odtApps + $standaloneApps) -contains $_ }
    $excludeApps = $excludeApps | Where-Object { ($odtApps + $standaloneApps) -contains $_ }

    # Detect installed O365 apps
    $installedApps = GetInstalledOfficeApps
    Write-Host "Installed applications detected: $($installedApps -join ', ')"

    # Build ExcludeApp list for Office365 suite
    $excludeFromO365 = ($excludeApps + ($includeApps | Where-Object { $installedApps -contains $_ })) | Where-Object { $odtApps -contains $_ } | Sort-Object -Unique
    Write-Host "Applications to exclude from Office suite: $($excludeFromO365 -join ', ')"

    # Output path
    $outputPath = "$env:USERPROFILE\Downloads\configuration.xml"

    # Start XML
    Write-Host "Creating ODT configuration XML file..."
    $xml = @()
    $xml += '<Configuration>'
    $xml += '  <Add OfficeClientEdition="64" Channel="Current">'
    $xml += '    <Product ID="O365ProPlusRetail">'
    $xml += '      <Language ID="en-us" />'

    # Add Office365 excludes
    foreach ($app in $excludeFromO365) {
        $xml += "      <ExcludeApp ID=""$app"" />"
    }

    $xml += '    </Product>'

    # Add Project if requested
    if (
        $includeApps -contains "Project" -and
        $excludeApps -notcontains "Project" -and
        -not ($installedApps -contains "Project")
    ) {
        $xml += '    <Product ID="ProjectProRetail">'
        $xml += '      <Language ID="en-us" />'
        $xml += '    </Product>'
    }

    # Add Visio if requested
    if (
        $includeApps -contains "Visio" -and
        $excludeApps -notcontains "Visio" -and
        -not ($installedApps -contains "Visio")
    ) {
        $xml += '    <Product ID="VisioProRetail">'
        $xml += '      <Language ID="en-us" />'
        $xml += '    </Product>'
    }

    # Finish XML
    $xml += '  </Add>'
    $xml += '  <Display Level="None" AcceptEULA="True" />'
    $xml += '  <Property Name="AUTOACTIVATE" Value="1" />'
    $xml += '</Configuration>'

    # Write XML to file
    try {
        $xml -join "`r`n" | Set-Content -Path $outputPath -Encoding UTF8
        Write-Host "ODT Configuration XML file created at `"$outputPath`"."
        Write-Host "Configuration file content:"
        $xml | Tee-Object -FilePath $logFile | Write-Host
    } catch {
        Write-Host "Failed to create ODT configuration XML file: $($_.Exception.Message)"
        exit 1
    }
}

function InstallOfficeWithODT {
    param (
        [string]$odtSetupPath = "$env:USERPROFILE\Downloads\setup.exe",
        [string]$configPath = "$env:USERPROFILE\Downloads\configuration.xml"
    )

    Write-Host "Starting Office install/upgrade using ODT..."

    if ((Test-Path $odtSetupPath) -and (Test-Path $configPath)) {
        try {
            $process = Start-Process -FilePath $odtSetupPath `
                -ArgumentList "/configure $configPath /log $env:USERPROFILE\Downloads\ODTInstall.log" `
                -PassThru -WindowStyle Hidden
            Write-Host -NoNewline "Install/upgrade processing"
            $timeout = 3600 # Timeout after 1 hour
            $elapsedTime = 0
            while (-not $process.HasExited -and $elapsedTime -lt $timeout) {
                Write-Host -NoNewline "."
                Start-Sleep -Seconds 5
                $elapsedTime += 5
            }
            Write-Host "."

            if ($process.HasExited) {
                if ($process.ExitCode -eq 0) {
                    Write-Host "Office install/upgrade using ODT completed successfully."
                } else {
                    Write-Host "Office install/upgrade using ODT failed with exit code: $($process.ExitCode)"
                    exit 1
                }
            } else {
                Write-Host "Office install/upgrade with ODT timed out after $($timeout / 60) minutes."
                exit 1
            }
        } catch {
            Write-Host "."
            Write-Host "Failed to install/upgrade Office with ODT: $($_.Exception.Message)"
            exit 1
        }
    } else {
        Write-Host "Either setup.exe or configuration.xml not found in the Downloads directory."
        exit 1
    }
}

function InstallNewerOfficeApps {
    Write-Host "Installing or updating New Outlook and Teams 2.0 using winget with Store ProductIds..."

    $appsToInstall = @(
        @{ Name = "Microsoft Outlook (new)"; ProductId = "9NRX63209R7B"; VerifyPath = "$env:LOCALAPPDATA\Packages\Microsoft.OutlookForWindows_*" },
        @{ Name = "Microsoft Teams (2.0)";    ProductId = "9NBLGGH42THS";     VerifyPath = "$env:LOCALAPPDATA\Microsoft\Teams" }
    )

    foreach ($app in $appsToInstall) {
        Write-Host "Checking for $($app.Name)..."

        # Check if installed by looking for expected app package folder
        $isInstalled = Get-ChildItem -Path $app.VerifyPath -ErrorAction SilentlyContinue | Measure-Object | Select-Object -ExpandProperty Count
        if ($isInstalled -gt 0) {
            Write-Host "$($app.Name) found. Attempting upgrade..."
            $wingetArgs = "upgrade -i -e --id $($app.ProductId) --source msstore --accept-package-agreements --accept-source-agreements"
        } else {
            Write-Host "$($app.Name) not found. Installing..."
            $wingetArgs = "install -i -e --id $($app.ProductId) --source msstore --accept-package-agreements --accept-source-agreements"
        }

        try {
            $process = Start-Process -FilePath "winget" -ArgumentList $wingetArgs -Wait -NoNewWindow -PassThru
            if ($process.ExitCode -eq 0) {
                Write-Host "$($app.Name) installed/upgraded successfully."
            } else {
                Write-Host "$($app.Name) install/upgrade failed with exit code: $($process.ExitCode)"
            }
        } catch {
            Write-Host "Error during $($app.Name) install/upgrade: $($_.Exception.Message)"
        }
    }
}

function Cleanup {
    Write-Host "Cleaning up temporary files..."
    $filesToRemove = @("$env:USERPROFILE\Downloads\OfficeDeploymentTool.exe", "$env:USERPROFILE\Downloads\configuration.xml", "$env:USERPROFILE\Downloads\setup.exe")
    foreach ($file in $filesToRemove) {
        if (Test-Path $file) {
            try {
                Remove-Item -Path $file -Force
                Write-Host "Removed temporary file: $file"
            } catch {
                Write-Host "Failed to remove temporary file: $file. Error: $($_.Exception.Message)"
            }
        }
    }
}

# Main Script Execution
try {
    Write-Host "Starting the Microsoft Office Install/Upgrade script..."
    DownloadAndExtractODT
    CreateODTConfig
    InstallOfficeWithODT
    InstallNewerOfficeApps
    Cleanup
    Write-Host "Microsoft Office installation/upgrade completed successfully."
} catch {
    Write-Host "An error occurred during installation: $($_.Exception.Message)"
    exit 1
}
