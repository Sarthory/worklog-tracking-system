function Write-Prompt { param($Message) Write-Host $Message -ForegroundColor Cyan }
function Write-Success { param($Message) Write-Host $Message -ForegroundColor Green }
function Write-Warning { param($Message) Write-Host $Message -ForegroundColor Yellow }
function Write-ErrorMsg { param($Message) Write-Host $Message -ForegroundColor Red }

$ProjectRoot = Split-Path -Path $PSScriptRoot -Parent

$ApiRelativeFolder = "WorklogTrackingSystem.API"
$ApiCsprojFile = "WorklogTrackingSystem.API.csproj"
$FrontEndRelativeFolder = "WorklogTrackingSystem.Front"

$DevFrontendUrl = "http://localhost:5666/"
$ProdPreviewUrl = "http://localhost:4173/"

$FullApiFolderPath = Join-Path $ProjectRoot $ApiRelativeFolder
$FullFrontEndPath = Join-Path $ProjectRoot $FrontEndRelativeFolder
$FullApiCsprojPath = Join-Path $FullApiFolderPath $ApiCsprojFile

# Variables to track processes
$ApiProcess = $null
$FrontendProcess = $null

function Stop-Processes {
    if ($ApiProcess -and !$ApiProcess.HasExited) {
        try {
            Stop-Process -Id $ApiProcess.Id -Force
            Write-Success "Stopped API process (PID: $($ApiProcess.Id))."
        } catch {
            Write-Warning "Failed to stop API process (PID: $($ApiProcess.Id)): $($_.Exception.Message)"
        }
        $ApiProcess = $null
    }

    if ($FrontendProcess -and !$FrontendProcess.HasExited) {
        try {
            Stop-Process -Id $FrontendProcess.Id -Force
            Write-Success "Stopped Frontend process (PID: $($FrontendProcess.Id))."
        } catch {
            Write-Warning "Failed to stop Frontend process (PID: $($FrontendProcess.Id)): $($_.Exception.Message)"
        }
        $FrontendProcess = $null
    }
}

function Invoke-RunDevelopmentEnvironment {
    Write-Host "`n--- Starting Development Environment (API + Frontend) ---" -ForegroundColor Yellow

    # Validate paths
    if (-not (Test-Path $FullApiFolderPath -PathType Container)) {
        Write-ErrorMsg "API project folder not found at: '$FullApiFolderPath'"
        Pause; return
    }
    if (-not (Test-Path $FullApiCsprojPath -PathType Leaf)) {
        Write-ErrorMsg "API project file not found at: '$FullApiCsprojPath'"
        Pause; return
    }
    if (-not (Test-Path $FullFrontEndPath -PathType Container)) {
        Write-ErrorMsg "Frontend folder not found at: '$FullFrontEndPath'"
        Pause; return
    }

    # Start API server
    try {
        Write-Host "Starting API server (Dev Profile)..."
        $ApiProcess = Start-Process -FilePath "dotnet" -ArgumentList "run --launch-profile Development" -WorkingDirectory $FullApiFolderPath -PassThru
        Write-Success "API server started (PID: $($ApiProcess.Id))."
    } catch {
        Write-ErrorMsg "Failed to start API server: $($_.Exception.Message)"
        Pause
    }

    # Start Frontend dev server
    try {
        Write-Host "Starting Frontend dev server..."
        $FrontendProcess = Start-Process -FilePath "cmd.exe" -ArgumentList "/c npm run dev" -WorkingDirectory $FullFrontEndPath -PassThru
        Write-Success "Frontend dev server started (PID: $($FrontendProcess.Id))."
    } catch {
        Write-ErrorMsg "Failed to start Frontend dev server: $($_.Exception.Message)"
        Pause
    }

    # Launch browser
    Write-Host "Waiting 5 seconds for servers to initialize..."
    Start-Sleep -Seconds 5
    Write-Host "Launching browser at: $DevFrontendUrl"
    try {
        Start-Process $DevFrontendUrl
        Write-Success "Browser launched."
    } catch {
        Write-Warning "Failed to launch browser. Open '$DevFrontendUrl' manually."
    }
}

function Invoke-RunProductionEnvironment {
    Write-Host "`n--- Starting Production Environment Simulation (API + Frontend Build/Preview) ---" -ForegroundColor Magenta

    # Validate paths
    if (-not (Test-Path $FullApiFolderPath -PathType Container)) {
        Write-ErrorMsg "API project folder not found at: '$FullApiFolderPath'"
        Pause; return
    }
    if (-not (Test-Path $FullApiCsprojPath -PathType Leaf)) {
        Write-ErrorMsg "API project file not found at: '$FullApiCsprojPath'"
        Pause; return
    }
    if (-not (Test-Path $FullFrontEndPath -PathType Container)) {
        Write-ErrorMsg "Frontend folder not found at: '$FullFrontEndPath'"
        Pause; return
    }

    # Start API server
    try {
        Write-Host "Starting API server (Production Profile)..."
        $ApiProcess = Start-Process -FilePath "dotnet" -ArgumentList "run --launch-profile Production" -WorkingDirectory $FullApiFolderPath -PassThru
        Write-Success "API server started (PID: $($ApiProcess.Id))."
    } catch {
        Write-ErrorMsg "Failed to start API server: $($_.Exception.Message)"
        Pause
    }

    # Build Frontend
    try {
        Write-Host "Building Frontend for production..."
        Start-Process -FilePath "cmd.exe" -ArgumentList "/c npm run build" -WorkingDirectory $FullFrontEndPath -NoNewWindow -Wait
        Write-Success "Frontend build completed successfully."
    } catch {
        Write-ErrorMsg "Failed to build Frontend: $($_.Exception.Message)"
        Pause; return
    }

    # Start Frontend preview server
    try {
        Write-Host "Starting Frontend preview server..."
        $FrontendProcess = Start-Process -FilePath "cmd.exe" -ArgumentList "/c npm run preview" -WorkingDirectory $FullFrontEndPath -PassThru
        Write-Success "Frontend preview server started (PID: $($FrontendProcess.Id))."
    } catch {
        Write-ErrorMsg "Failed to start Frontend preview server: $($_.Exception.Message)"
        Pause; return
    }

    # Launch browser
    Write-Host "Waiting 5 seconds for preview server to initialize..."
    Start-Sleep -Seconds 5
    Write-Host "Launching browser at: $ProdPreviewUrl"
    try {
        Start-Process $ProdPreviewUrl
        Write-Success "Browser launched."
    } catch {
        Write-Warning "Failed to launch browser. Open '$ProdPreviewUrl' manually."
    }
}

Clear-Host
Write-Host "========================================" -ForegroundColor Green
Write-Host "  WorklogTrackingSystem Environment Setup"
Write-Host "========================================" -ForegroundColor Green

$validEnvironments = @("Development", "Production", "Exit")
$SelectedEnvironment = $null

while ($true) {
    Write-Prompt "`nPlease select the target environment:"
    Write-Host "  (D)evelopment"
    Write-Host "  (P)roduction"
    Write-Host "  (E)xit"
    $envInput = Read-Host "Enter your choice (D/P/E or full name)"

    if ($envInput -match '^D(evelopment)?$' -or $envInput -eq 'Development') {
        $SelectedEnvironment = "Development"
        Invoke-RunDevelopmentEnvironment
    } elseif ($envInput -match '^P(roduction)?$' -or $envInput -eq 'Production') {
        $SelectedEnvironment = "Production"
        Invoke-RunProductionEnvironment
    } elseif ($envInput -match '^E(xit)?$' -or $envInput -eq 'Exit') {
        Write-Success "Exiting script. Stopping all running processes..."
        Stop-Processes
        break
    } else {
        Write-Warning "Invalid input '$envInput'. Please enter 'D', 'P', or 'E'."
    }
}

Write-Host "`nScript execution finished."