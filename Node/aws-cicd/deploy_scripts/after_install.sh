#!/bin/bash

# Delete unused files

cd /var/www/myapp
sudo rm package-lock.json

echo "after_install script completed successfully."