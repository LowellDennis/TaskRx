param (
    [string]$domain = "AMERICAS",
    [string]$username = $(throw "Parameter 'username' is required."),
)

# Global Variables
$logIt   = $true
$logFile = "$env:USERPROFILE\Downloads\EnableLongPaths.txt"

function Write-Log {
    param (
        [string]$message
    )
    if ($logIt) {
        Write-Output $message | Out-File -Append -FilePath $logFile
    }
    Write-Host $message
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
    Write-Log "Enabling long paths..."
    New-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Control\FileSystem" -Name "LongPathsEnabled" -Value 1 -PropertyType DWORD -Force
    Write-Log "Long paths enabled successfully."
} catch {
    Write-Log "An error occurred during long paths enablement: $($_.Exception.Message)"
    exit 1
}
