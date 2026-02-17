# Enable Ultimate Performance Power Plan
# This script enables the Ultimate Performance power plan in Windows

Write-Host "Enabling Ultimate Performance power plan..."

try {
    # Check if Ultimate Performance is already active
    $activePlan = powercfg /getactivescheme
    if ($activePlan -match "Ultimate Performance") {
        Write-Host "Ultimate Performance power plan is already active."
        Write-Host "Script completed."
        exit 0
    }

    # Enable Ultimate Performance power plan (this is a hidden plan by default)
    $duplicateOutput = powercfg /duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61 2>&1
    $newGuid = $duplicateOutput | Select-String -Pattern "([a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12})" | ForEach-Object { $_.Matches[0].Value }

    if ($LASTEXITCODE -eq 0 -and $newGuid) {
        Write-Host "Ultimate Performance power plan enabled successfully with GUID: $newGuid"
        
        # Set the duplicated Ultimate Performance plan as active
        powercfg /setactive $newGuid
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Ultimate Performance power plan is now active."
        } else {
            Write-Host "Failed to activate Ultimate Performance power plan. It may require a system restart or the plan may not be supported on this system."
        }
    } else {
        Write-Host "Failed to enable Ultimate Performance power plan. It may already be enabled or not supported."
    }
} catch {
    Write-Host "Error enabling Ultimate Performance power plan: $($_.Exception.Message)"
}

Write-Host "Script completed."