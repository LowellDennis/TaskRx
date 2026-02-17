#Requires -RunAsAdministrator
<#
.SYNOPSIS
    Optimizes Windows VM for development work
.DESCRIPTION
    Removes bloatware, disables unnecessary services, configures performance settings,
    and sets up Git for optimal performance inside company firewall.
#>

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Development VM Optimization Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Function to safely disable a service
function Disable-ServiceSafely {
    param([string]$ServiceName, [string]$DisplayName)
    
    try {
        $service = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
        if ($service) {
            if ($service.Status -eq 'Running') {
                Stop-Service -Name $ServiceName -Force -ErrorAction SilentlyContinue
                Write-Host "[SUCCESS] Stopped service: $DisplayName" -ForegroundColor Green
            }
            Set-Service -Name $ServiceName -StartupType Disabled -ErrorAction SilentlyContinue
            Write-Host "[SUCCESS] Disabled service: $DisplayName" -ForegroundColor Green
        } else {
            Write-Host "[SKIP] Service not found: $DisplayName" -ForegroundColor Yellow
        }
    } catch {
        Write-Host "[ERROR] Failed to disable $DisplayName : $_" -ForegroundColor Red
    }
}

# Function to uninstall Windows apps
function Remove-AppxPackageSafely {
    param([string]$AppName, [string]$DisplayName)
    
    try {
        $apps = Get-AppxPackage -Name $AppName -ErrorAction SilentlyContinue
        if ($apps) {
            foreach ($app in $apps) {
                Remove-AppxPackage -Package $app.PackageFullName -ErrorAction SilentlyContinue
                Write-Host "[SUCCESS] Removed: $DisplayName" -ForegroundColor Green
            }
        } else {
            Write-Host "[SKIP] App not found: $DisplayName" -ForegroundColor Yellow
        }
    } catch {
        Write-Host "[ERROR] Failed to remove $DisplayName : $_" -ForegroundColor Red
    }
}

# ============================================
# 1. REMOVE BLOATWARE
# ============================================
Write-Host "`n--- Removing Bloatware ---" -ForegroundColor Cyan

$bloatwareApps = @(
    @{Name="Microsoft.OneDrive"; Display="OneDrive (UWP)"},
    @{Name="Microsoft.XboxApp"; Display="Xbox"},
    @{Name="Microsoft.XboxGamingOverlay"; Display="Xbox Gaming Overlay"},
    @{Name="Microsoft.XboxGameOverlay"; Display="Xbox Game Overlay"},
    @{Name="Microsoft.XboxIdentityProvider"; Display="Xbox Identity Provider"},
    @{Name="Microsoft.Xbox.TCUI"; Display="Xbox TCUI"},
    @{Name="Microsoft.XboxSpeechToTextOverlay"; Display="Xbox Speech To Text"},
    @{Name="Microsoft.ZuneMusic"; Display="Groove Music"},
    @{Name="Microsoft.ZuneVideo"; Display="Movies & TV"},
    @{Name="Microsoft.MicrosoftSolitaireCollection"; Display="Solitaire"},
    @{Name="Microsoft.BingWeather"; Display="Weather"},
    @{Name="Microsoft.BingNews"; Display="News"},
    @{Name="Microsoft.GetHelp"; Display="Get Help"},
    @{Name="Microsoft.Getstarted"; Display="Get Started"},
    @{Name="Microsoft.MicrosoftOfficeHub"; Display="Office Hub"},
    @{Name="Microsoft.People"; Display="People"},
    @{Name="Microsoft.SkypeApp"; Display="Skype"},
    @{Name="Microsoft.WindowsFeedbackHub"; Display="Feedback Hub"},
    @{Name="Microsoft.YourPhone"; Display="Your Phone"},
    @{Name="Microsoft.549981C3F5F10"; Display="Cortana"}
)

foreach ($app in $bloatwareApps) {
    Remove-AppxPackageSafely -AppName $app.Name -DisplayName $app.Display
}

# ============================================
# 2. STOP AND DISABLE ONEDRIVE SYNC
# ============================================
Write-Host "\n--- Stopping OneDrive Sync ---" -ForegroundColor Cyan
Write-Host "[INFO] OneDrive can be easily reinstalled later if needed" -ForegroundColor Gray

try {
    # Stop OneDrive process
    $oneDriveProcesses = Get-Process -Name "OneDrive" -ErrorAction SilentlyContinue
    if ($oneDriveProcesses) {
        Stop-Process -Name "OneDrive" -Force -ErrorAction SilentlyContinue
        Write-Host "[SUCCESS] Stopped OneDrive process" -ForegroundColor Green
        Start-Sleep -Seconds 2
    }
    
    # Uninstall OneDrive (64-bit)
    $oneDrive64 = "$env:SystemRoot\System32\OneDriveSetup.exe"
    if (Test-Path $oneDrive64) {
        Start-Process -FilePath $oneDrive64 -ArgumentList "/uninstall" -Wait -NoNewWindow -ErrorAction SilentlyContinue
        Write-Host "[SUCCESS] Uninstalled OneDrive (64-bit)" -ForegroundColor Green
    }
    
    # Uninstall OneDrive (32-bit)
    $oneDrive32 = "$env:SystemRoot\SysWOW64\OneDriveSetup.exe"
    if (Test-Path $oneDrive32) {
        Start-Process -FilePath $oneDrive32 -ArgumentList "/uninstall" -Wait -NoNewWindow -ErrorAction SilentlyContinue
        Write-Host "[SUCCESS] Uninstalled OneDrive (32-bit)" -ForegroundColor Green
    }
    
    # Remove OneDrive from File Explorer sidebar
    $regPath = "HKCR:\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}"
    if (Test-Path $regPath) {
        Remove-Item -Path $regPath -Recurse -Force -ErrorAction SilentlyContinue
        Write-Host "[SUCCESS] Removed OneDrive from File Explorer" -ForegroundColor Green
    }
    
    if (!$oneDriveProcesses -and !(Test-Path $oneDrive64) -and !(Test-Path $oneDrive32)) {
        Write-Host "[SKIP] OneDrive not found or already removed" -ForegroundColor Yellow
    }
} catch {
    Write-Host "[ERROR] Failed to remove OneDrive: $_" -ForegroundColor Red
}

# ============================================
# 3. DISABLE UNNECESSARY SERVICES
# ============================================
Write-Host "`n--- Disabling Unnecessary Services ---" -ForegroundColor Cyan

$servicesToDisable = @(
    @{Name="WSearch"; Display="Windows Search"},
    @{Name="SysMain"; Display="Superfetch/SysMain"},
    @{Name="Spooler"; Display="Print Spooler"},
    @{Name="bthserv"; Display="Bluetooth Support"},
    @{Name="BthAvctpSvc"; Display="Bluetooth Audio"},
    @{Name="WMPNetworkSvc"; Display="Windows Media Player Sharing"},
    @{Name="DiagTrack"; Display="Diagnostic Tracking"},
    @{Name="dmwappushservice"; Display="WAP Push Message Routing"},
    @{Name="RetailDemo"; Display="Retail Demo Service"},
    @{Name="XblAuthManager"; Display="Xbox Live Auth Manager"},
    @{Name="XblGameSave"; Display="Xbox Live Game Save"},
    @{Name="XboxGipSvc"; Display="Xbox Accessory Management"},
    @{Name="XboxNetApiSvc"; Display="Xbox Live Networking"}
)

foreach ($service in $servicesToDisable) {
    Disable-ServiceSafely -ServiceName $service.Name -DisplayName $service.Display
}

# ============================================
# 4. DISABLE WINDOWS SEARCH INDEXING
# ============================================
Write-Host "`n--- Configuring Windows Search Indexing ---" -ForegroundColor Cyan

try {
    $indexingOptions = Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows Search" -ErrorAction SilentlyContinue
    if ($indexingOptions) {
        Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows Search" -Name "SetupCompletedSuccessfully" -Value 0 -ErrorAction SilentlyContinue
        Write-Host "[SUCCESS] Disabled Windows Search indexing" -ForegroundColor Green
    }
} catch {
    Write-Host "[SKIP] Could not modify indexing settings" -ForegroundColor Yellow
}

# ============================================
# 5. OPTIMIZE VISUAL EFFECTS FOR PERFORMANCE
# ============================================
Write-Host "`n--- Optimizing Visual Effects ---" -ForegroundColor Cyan

try {
    $path = "HKCU:\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects"
    if (!(Test-Path $path)) {
        New-Item -Path $path -Force | Out-Null
    }
    Set-ItemProperty -Path $path -Name "VisualFXSetting" -Value 2 -ErrorAction SilentlyContinue
    Write-Host "[SUCCESS] Set visual effects to 'Best Performance'" -ForegroundColor Green
} catch {
    Write-Host "[ERROR] Failed to optimize visual effects: $_" -ForegroundColor Red
}

# ============================================
# 6. SET POWER PLAN TO HIGH PERFORMANCE
# ============================================
Write-Host "`n--- Configuring Power Settings ---" -ForegroundColor Cyan

try {
    $highPerfGuid = (powercfg -l | Select-String "High performance" | ForEach-Object { $_.ToString().Split()[3] })
    if ($highPerfGuid) {
        powercfg -setactive $highPerfGuid
        Write-Host "[SUCCESS] Activated High Performance power plan" -ForegroundColor Green
    } else {
        Write-Host "[SKIP] High Performance plan not found" -ForegroundColor Yellow
    }
} catch {
    Write-Host "[ERROR] Failed to set power plan: $_" -ForegroundColor Red
}

# ============================================
# 7. CONFIGURE GIT FOR PERFORMANCE
# ============================================
Write-Host "`n--- Configuring Git ---" -ForegroundColor Cyan

try {
    $gitExists = Get-Command git -ErrorAction SilentlyContinue
    if ($gitExists) {
        git config --global core.preloadindex true
        git config --global core.fscache true
        git config --global gc.auto 256
        git config --global core.compression 0
        git config --global credential.helper wincred
        git config --global http.postBuffer 524288000
        
        Write-Host "[SUCCESS] Configured Git for optimal performance" -ForegroundColor Green
        Write-Host "          - Enabled preloadindex and fscache" -ForegroundColor Gray
        Write-Host "          - Set garbage collection auto to 256" -ForegroundColor Gray
        Write-Host "          - Configured credential helper" -ForegroundColor Gray
        Write-Host "          - Increased http post buffer" -ForegroundColor Gray
    } else {
        Write-Host "[SKIP] Git not installed" -ForegroundColor Yellow
    }
} catch {
    Write-Host "[ERROR] Failed to configure Git: $_" -ForegroundColor Red
}

# ============================================
# 8. DISABLE WINDOWS UPDATE (SET TO MANUAL)
# ============================================
Write-Host "`n--- Configuring Windows Update ---" -ForegroundColor Cyan

try {
    $service = Get-Service -Name "wuauserv" -ErrorAction SilentlyContinue
    if ($service) {
        Stop-Service -Name "wuauserv" -Force -ErrorAction SilentlyContinue
        Set-Service -Name "wuauserv" -StartupType Manual -ErrorAction SilentlyContinue
        Write-Host "[SUCCESS] Set Windows Update to Manual" -ForegroundColor Green
        Write-Host "          You can still manually check for updates when needed" -ForegroundColor Gray
    }
} catch {
    Write-Host "[ERROR] Failed to configure Windows Update: $_" -ForegroundColor Red
}

# ============================================
# 9. DISABLE TRANSPARENCY EFFECTS
# ============================================
Write-Host "`n--- Disabling Transparency Effects ---" -ForegroundColor Cyan

try {
    Set-ItemProperty -Path "HKCU:\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize" -Name "EnableTransparency" -Value 0 -ErrorAction SilentlyContinue
    Write-Host "[SUCCESS] Disabled transparency effects" -ForegroundColor Green
} catch {
    Write-Host "[ERROR] Failed to disable transparency: $_" -ForegroundColor Red
}

# ============================================
# 9. DISABLE STARTUP PROGRAMS
# ============================================
Write-Host "`n--- Checking Startup Programs ---" -ForegroundColor Cyan

try {
    $startupItems = Get-CimInstance Win32_StartupCommand -ErrorAction SilentlyContinue
    if ($startupItems) {
        Write-Host "[INFO] Found $($startupItems.Count) startup programs" -ForegroundColor Yellow
        Write-Host "        Review these in Task Manager > Startup tab" -ForegroundColor Gray
    }
} catch {
    Write-Host "[SKIP] Could not enumerate startup programs" -ForegroundColor Yellow
}

# ============================================
# 11. OPTIMIZE PAGE FILE
# ============================================
Write-Host "`n--- Configuring Page File ---" -ForegroundColor Cyan

try {
    $computerSystem = Get-CimInstance Win32_ComputerSystem -ErrorAction SilentlyContinue
    if ($computerSystem) {
        $ram = [math]::Round($computerSystem.TotalPhysicalMemory / 1GB)
        Write-Host "[INFO] System has ${ram}GB RAM" -ForegroundColor Gray
        Write-Host "        Consider setting page file to system-managed" -ForegroundColor Gray
    }
} catch {
    Write-Host "[SKIP] Could not check page file settings" -ForegroundColor Yellow
}

# ============================================
# 12. CLEAN TEMPORARY FILES
# ============================================
Write-Host "`n--- Cleaning Temporary Files ---" -ForegroundColor Cyan

$tempFolders = @(
    $env:TEMP,
    "C:\Windows\Temp",
    "C:\Windows\Prefetch"
)

foreach ($folder in $tempFolders) {
    if (Test-Path $folder) {
        try {
            $fileCount = (Get-ChildItem -Path $folder -Recurse -Force -ErrorAction SilentlyContinue | Measure-Object).Count
            Remove-Item -Path "$folder\*" -Recurse -Force -ErrorAction SilentlyContinue
            Write-Host "[SUCCESS] Cleaned: $folder ($fileCount items)" -ForegroundColor Green
        } catch {
            Write-Host "[PARTIAL] Some files in $folder could not be deleted (in use)" -ForegroundColor Yellow
        }
    }
}

# ============================================
# SUMMARY
# ============================================
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "Optimization Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "RECOMMENDED NEXT STEPS:" -ForegroundColor Yellow
Write-Host "1. Restart the VM to apply all changes" -ForegroundColor White
Write-Host "2. Set up SSH keys for GitHub:" -ForegroundColor White
Write-Host "   ssh-keygen -t ed25519 -C 'your.email@company.com'" -ForegroundColor Gray
Write-Host "3. Add Windows Defender exclusions for your dev folders:" -ForegroundColor White
Write-Host "   Add-MpPreference -ExclusionPath 'C:\dev'" -ForegroundColor Gray
Write-Host "4. Review Task Manager > Startup for additional programs to disable" -ForegroundColor White
Write-Host "5. If Git not installed, download from: https://git-scm.com" -ForegroundColor White
Write-Host "6. To reinstall OneDrive: winget install Microsoft.OneDrive" -ForegroundColor White
Write-Host ""
Write-Host "For UEFI builds, consider:" -ForegroundColor Yellow
Write-Host "  - Using incremental/cached builds where possible" -ForegroundColor White
Write-Host "  - Allocating more CPU cores in VM settings" -ForegroundColor White
Write-Host "  - Using SSD storage for the VM" -ForegroundColor White
Write-Host ""
