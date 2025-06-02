
param (
    [string]$email,
    [string]$domain,
    [string]$username
)

function Install-WebView2 {
    Write-Host "Checking for WebView2 runtime..."
    $webview2Key = "HKLM:\SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{F2C6E2B0-E5AA-4E0C-8A0A-6A2A4A8C3F9A}"
    if (-not (Test-Path $webview2Key)) {
        Write-Host "WebView2 runtime not found. Downloading and installing..."
        $webview2Installer = "https://go.microsoft.com/fwlink/p/?LinkId=2124703"
        $webview2InstallerPath = "$env:TEMP\MicrosoftEdgeWebview2Setup.exe"
        Invoke-WebRequest -Uri $webview2Installer -OutFile $webview2InstallerPath
        Start-Process -FilePath $webview2InstallerPath -ArgumentList "/silent /install" -Wait
        Write-Host "WebView2 runtime installed."
    } else {
        Write-Host "WebView2 runtime is already installed."
    }
}

function Install-Microsoft365Apps {
    Write-Host "Downloading Office Deployment Tool..."
    $odtUrl = "https://www.microsoft.com/en-us/download/details.aspx?id=49117"
    $odtPath = "$env:TEMP\OfficeDeploymentTool.exe"
    Invoke-WebRequest -Uri $odtUrl -OutFile $odtPath
    Write-Host "Extracting Office Deployment Tool..."
    Start-Process -FilePath $odtPath -ArgumentList "/quiet /extract:$env:TEMP\ODT" -Wait

    Write-Host "Creating configuration.xml..."
    $configXml = @"
<Configuration>
  <Add OfficeClientEdition="64" Channel="Current">
    <Product ID="O365ProPlusRetail">
      <Language ID="en-us" />
      <ExcludeApp ID="Outlook" />
    </Product>
  </Add>
  <Display Level="None" AcceptEULA="TRUE" />
</Configuration>
"@
    $configXmlPath = "$env:TEMP\ODT\configuration.xml"
    $configXml | Out-File -FilePath $configXmlPath -Encoding utf8

    Write-Host "Installing Microsoft 365 Apps for Enterprise..."
    Start-Process -FilePath "$env:TEMP\ODT\setup.exe" -ArgumentList "/configure $configXmlPath" -Wait
    Write-Host "Microsoft 365 Apps for Enterprise installed."
}

function Install-NewOutlook {
    Write-Host "Downloading New Outlook installer..."
    $newOutlookUrl = "https://outlook.office.com/OutlookSetup.exe"
    $newOutlookPath = "$env:TEMP\OutlookSetup.exe"
    Invoke-WebRequest -Uri $newOutlookUrl -OutFile $newOutlookPath

    Write-Host "Installing New Outlook..."
    Start-Process -FilePath $newOutlookPath -ArgumentList "/silent /install" -Wait
    Write-Host "New Outlook installed."
}

function Install-NewTeams {
    Write-Host "Downloading New Teams installer..."
    $newTeamsUrl = "https://aka.ms/teamsbootstrapper"
    $newTeamsPath = "$env:TEMP\TeamsBootstrapper.exe"
    Invoke-WebRequest -Uri $newTeamsUrl -OutFile $newTeamsPath

    Write-Host "Installing New Teams..."
    Start-Process -FilePath $newTeamsPath -ArgumentList "/silent /install" -Wait
    Write-Host "New Teams installed."
}

function PostInstallChecks {
    Write-Host "Performing post-installation checks..."
    if (Test-Path "C:\Program Files\Microsoft Office\root\Office16\WINWORD.EXE") {
        Write-Host "Microsoft 365 Apps for Enterprise installed successfully."
    } else {
        Write-Host "Microsoft 365 Apps for Enterprise installation failed."
    }

    if (Test-Path "C:\Program Files\Microsoft\Outlook\Outlook.exe") {
        Write-Host "New Outlook installed successfully."
    } else {
        Write-Host "New Outlook installation failed."
    }

    if (Test-Path "C:\Program Files\Microsoft\Teams\current\Teams.exe") {
        Write-Host "New Teams installed successfully."
    } else {
        Write-Host "New Teams installation failed."
    }
}

Write-Host "Starting installation process..."
Install-WebView2
Install-Microsoft365Apps
Install-NewOutlook
Install-NewTeams
PostInstallChecks
Write-Host "Installation process completed."
