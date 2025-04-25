#!/bin/bash

PROJECT_ROOT=$(dirname "$(dirname "$(realpath "$0")")")
API_RELATIVE_FOLDER="WorklogTrackingSystem.API"
FRONTEND_RELATIVE_FOLDER="WorklogTrackingSystem.Front"
DEV_FRONTEND_URL="http://localhost:5666/"
PROD_PREVIEW_URL="http://localhost:4173/"
API_FOLDER="$PROJECT_ROOT/$API_RELATIVE_FOLDER"
FRONTEND_FOLDER="$PROJECT_ROOT/$FRONTEND_RELATIVE_FOLDER"
API_PROCESS_PID=""
FRONTEND_PROCESS_PID=""

function write_prompt() {
    echo -e "\033[36m$1\033[0m"
}

function write_success() {
    echo -e "\033[32m$1\033[0m"
}

function write_warning() {
    echo -e "\033[33m$1\033[0m"
}

function write_error() {
    echo -e "\033[31m$1\033[0m"
}

function stop_processes() {
    if [[ -n "$API_PROCESS_PID" ]] && kill -0 "$API_PROCESS_PID" 2>/dev/null; then
        kill "$API_PROCESS_PID"
        write_success "Stopped API process (PID: $API_PROCESS_PID)."
        API_PROCESS_PID=""
    fi

    if [[ -n "$FRONTEND_PROCESS_PID" ]] && kill -0 "$FRONTEND_PROCESS_PID" 2>/dev/null; then
        kill "$FRONTEND_PROCESS_PID"
        write_success "Stopped Frontend process (PID: $FRONTEND_PROCESS_PID)."
        FRONTEND_PROCESS_PID=""
    fi
}

function open_browser() {
    local url=$1
    if command -v xdg-open &> /dev/null; then
        xdg-open "$url"
        write_success "Browser launched successfully."
    elif command -v open &> /dev/null; then
        open "$url"
        write_success "Browser launched successfully."
    elif command -v start &> /dev/null; then
        start "$url"
        write_success "Browser launched successfully."
    else
        write_warning "No suitable command found to open the browser. Please open '$url' manually."
    fi
}

function run_development_environment() {
    echo "--- Starting Development Environment (API + Frontend) ---"
    if [[ ! -d "$API_FOLDER" ]]; then
        write_error "API project folder not found at: '$API_FOLDER'"
        return
    fi
    if [[ ! -d "$FRONTEND_FOLDER" ]]; then
        write_error "Frontend folder not found at: '$FRONTEND_FOLDER'"
        return
    fi
    echo "Starting API server (Dev Profile)..."
    (cd "$API_FOLDER" && dotnet run --launch-profile Development) &
    API_PROCESS_PID=$!
    write_success "API server started (PID: $API_PROCESS_PID)."
    echo "Starting Frontend dev server..."
    (cd "$FRONTEND_FOLDER" && npm i && npm run dev) &
    FRONTEND_PROCESS_PID=$!
    write_success "Frontend dev server started (PID: $FRONTEND_PROCESS_PID)."
    echo "Waiting 5 seconds for servers to initialize..."
    sleep 5
    echo "Launching browser at: $DEV_FRONTEND_URL"
    open_browser "$DEV_FRONTEND_URL"
}

function run_production_environment() {
    echo "--- Starting Production Environment Simulation (API + Frontend Build/Preview) ---"
    if [[ ! -d "$API_FOLDER" ]]; then
        write_error "API project folder not found at: '$API_FOLDER'"
        return
    fi
    if [[ ! -d "$FRONTEND_FOLDER" ]]; then
        write_error "Frontend folder not found at: '$FRONTEND_FOLDER'"
        return
    fi
    echo "Starting API server (Production Profile)..."
    (cd "$API_FOLDER" && dotnet run --launch-profile Production) &
    API_PROCESS_PID=$!
    write_success "API server started (PID: $API_PROCESS_PID)."
    echo "Building Frontend for production..."
    (cd "$FRONTEND_FOLDER" && npm i && npm run build)
    if [[ $? -ne 0 ]]; then
        write_error "Frontend build failed."
        return
    fi
    write_success "Frontend build completed successfully."
    echo "Starting Frontend preview server..."
    (cd "$FRONTEND_FOLDER" && npm run preview) &
    FRONTEND_PROCESS_PID=$!
    write_success "Frontend preview server started (PID: $FRONTEND_PROCESS_PID)."
    echo "Waiting 5 seconds for preview server to initialize..."
    sleep 5
    echo "Launching browser at: $PROD_PREVIEW_URL"
    open_browser "$PROD_PREVIEW_URL"
}

clear
echo "========================================"
echo "  WorklogTrackingSystem Environment Setup"
echo "========================================"

while true; do
    write_prompt "\nPlease select the target environment:"
    echo "  (D)evelopment"
    echo "  (P)roduction"
    echo "  (E)xit"
    read -p "Enter your choice (D/P/E): " env_input
    case "$env_input" in
        [Dd]*)
            run_development_environment
            ;;
        [Pp]*)
            run_production_environment
            ;;
        [Ee]*)
            write_success "Exiting script. Stopping all running processes..."
            stop_processes
            break
            ;;
        *)
            write_warning "Invalid input '$env_input'. Please enter 'D', 'P', or 'E'."
            ;;
    esac
done

echo -e "\nScript execution finished."