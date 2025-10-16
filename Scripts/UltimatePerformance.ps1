# Enable Ultimate Performance Power Plan
# This script enables the Ultimate Performance power plan in Windows

Write-Host "Enabling Ultimate Performance power plan..."

try {
    # Enable Ultimate Performance power plan (this is a hidden plan by default)
    $result = powercfg /duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Ultimate Performance power plan enabled successfully."
        
        # Set the Ultimate Performance plan as active
        $ultimatePlanGuid = "e9a42b02-d5df-448d-aa00-03f14749eb61"
        powercfg /setactive $ultimatePlanGuid
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Ultimate Performance power plan is now active."
        } else {
            Write-Host "Failed to activate Ultimate Performance power plan."
        }
    } else {
        Write-Host "Failed to enable Ultimate Performance power plan. It may already be enabled."
    }
} catch {
    Write-Host "Error enabling Ultimate Performance power plan: $($_.Exception.Message)"
}

Write-Host "Script completed."