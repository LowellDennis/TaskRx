
param (
    [string]$email,
    [string]$domain,
    [string]$username
)

function Install-WebView2 {
    Write-Output "Checking for WebView2 runtime..."
    $webview2Key = "HKLM:\SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}"
    if (-not (Test-Path $webview2Key)) {
        Write-Output "WebView2 runtime not found. Downloading and installing..."
        $webview2Installer = "https://go.microsoft.com/fwlink/p/?LinkId=2124703"
        $webview2InstallerPath = "$env:TEMP\MicrosoftEdgeWebview2Setup.exe"
        Invoke-WebRequest -Uri $webview2Installer -OutFile $webview2InstallerPath
        Start-Process -FilePath $webview2InstallerPath -ArgumentList "/silent /install" -Wait
        Write-Output "WebView2 runtime installed."
    } else {
        Write-Output "WebView2 runtime is already installed."
    }
}

function Install-Microsoft365Apps {
    Write-Output "Downloading Office Deployment Tool..."
    $odtUrl = "https://download.microsoft.com/download/6c1eeb25-cf8b-41d9-8d0d-cc1dbc032140/officedeploymenttool_18730-20142.exe"
    $odtPath = "$env:USERPROFILE\Downloads\OfficeDeploymentTool.exe"
    Invoke-WebRequest -Uri $odtUrl -OutFile $odtPath
    Write-Output "Extracting Office Deployment Tool..."
    Start-Process -FilePath $odtPath -ArgumentList "/quiet /extract:$env:USERPROFILE\Downloads\ODT" -Wait

    Write-Output "Creating configuration.xml..."
    $configXml = @"
<Configuration>
  <Add OfficeClientEdition="64" Channel="Current">
    <Product ID="O365ProPlusRetail">
      <Language ID="en-us" />
      <ExcludeApp ID="Access" />
      <ExcludeApp ID="Outlook" />
      <ExcludeApp ID="Publisher" />
    </Product>
  </Add>
  <Display Level="None" AcceptEULA="TRUE" />
  <Property Name="AUTOACTIVATE" Value="1" />
  <Property Name="FORCEAPPSHUTDOWN" Value="TRUE" />
  <Property Name="DeviceBasedLicensing" Value="1" />
</Configuration>
"@
    $configXmlPath = "$env:USERPROFILE\Downloads\ODT\configuration.xml"
    $configXml | Out-File -FilePath $configXmlPath -Encoding utf8

    Write-Output "Installing Excel, OneNote, PowerPoint, and Word ..."
    Start-Process -FilePath "$env:USERPROFILE\Downloads\ODT\setup.exe" -Wait
    Write-Output "Excel, OneNote, PowerPoint, and Word installed!"
}

function Install-NewOutlook {
    Write-Host "Downloading New Outlook installer..."
    $newOutlookUrl = "https://outlook.office.com/OutlookSetup.exe"
    $newOutlookPath = "$env:TEMP\OutlookSetup.exe"
    Invoke-WebRequest -Uri $newOutlookUrl -OutFile $newOutlookPath

    Write-Host "Installing Outlook(new)..."
    Start-Process -FilePath $newOutlookPath -ArgumentList "/silent /install" -Wait
    Write-Host "Outlook(new) installed."
}

function Install-NewTeams {
    Write-Output "Downloading Teams(new) installer..."
    $newTeamsUrl = "https://aka.ms/teamsbootstrapper"
    $newTeamsPath = "$env:USERPROFILE\Downloads\TeamsBootstrapper.exe"
    Invoke-WebRequest -Uri $newTeamsUrl -OutFile $newTeamsPath

    Write-Output "Installing Teams(new)..."
    Start-Process -FilePath $newTeamsPath -ArgumentList "/silent /install" -Wait
    Write-Output "Teams(new) installed."
}

function PostInstallChecks {
    Write-Output "Performing post-installation checks..."
    if (Test-Path "C:\Program Files\Microsoft Office\root\Office16\EXCEL.EXE") {
        Write-Output "Excel installed successfully."
    } else {
        Write-Output "Excel installation failed."
    }

    if (Test-Path "C:\Program Files\Microsoft Office\root\Office16\ONENOTE.EXE") {
        Write-Output "OneNote installed successfully."
    } else {
        Write-Output "OneNote installation failed."
    }

    if (Test-Path "C:\Program Files\Microsoft Office\root\Office16\POWERPNT.EXE") {
        Write-Output "PowerPoint installed successfully."
    } else {
        Write-Output "PowerPoint installation failed."
    }

    if (Test-Path "C:\Program Files\Microsoft Office\root\Office16\WINWORD.EXE") {
        Write-Output "Word installed successfully."
    } else {
        Write-Output "Word installation failed."
    }

    if (Test-Path "C:\Program Files\Microsoft\Outlook\Outlook.exe") {
        Write-Output "Outlook(new) installed successfully."
    } else {
        Write-Output "Outlook(new) installation failed."
    }

    if (Test-Path "C:\Program Files\Microsoft\Teams\current\Teams.exe") {
        Write-Output "Teams(new) installed successfully."
    } else {
        Write-Output "Teams(new) installation failed."
    }
}

Write-Output "Starting Microsoft Office installation..."
Install-WebView2
Install-Microsoft365Apps
Install-NewOutlook
Install-NewTeams
PostInstallChecks
Write-Output "Microsot Office Installation completed."
