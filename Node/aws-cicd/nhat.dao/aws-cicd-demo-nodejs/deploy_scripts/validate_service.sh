#!/bin/bash

# Define the process name to check
PROCESS_NAME="demo"

# Check if the service is running
SERVICE_STATUS=$(sudo pm2 list | grep $PROCESS_NAME)

if [[ -n $SERVICE_STATUS ]]; then
  echo "Service is running."
  exit 0
else
  echo "Service is not running."
  exit 1
fi

echo "validate_service script completed successfully."