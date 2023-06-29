#!/bin/bash

# Install node packages manager

sudo yum install -y npm
sudo npm update -g

# Install application dependencies

cd /var/www/myapp
sudo npm install

echo "before_install script completed successfully."