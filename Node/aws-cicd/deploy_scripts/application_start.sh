#!/bin/bash

# Define the process or namespace
PROCESS_NAME="demo"

# Install application dependencies
cd /var/www/myapp

# Start the Node.js application managed by PM2
sudo pm2 start npm --name $PROCESS_NAME -- start --watch --log /var/log/$PROCESS_NAME.log

echo "application_start script completed successfully."