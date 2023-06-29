#!/bin/bash
# Stop the Node.js application managed by PM2

# Define the process or namespace to check
PROCESS_NAME="demo"

# Check if the process or namespace exists
EXIST=$(sudo pm2 list | grep -E "(name[^:]*:\s*$PROCESS_NAME|namespace[^:]*:\s*$PROCESS_NAME)")

if [[ -n $EXIST ]]; then
    sudo pm2 stop $PROCESS_NAME
else
    echo "Process or namespace $PROCESS_NAME does not exist in PM2."
fi

echo "application_stop script completed successfully."