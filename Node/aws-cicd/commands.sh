#!/bin/bash

# Install CodeDeploy Agent
sudo yum update
sudo yum install ruby

# Download the agent (replace the region)
wget https://aws-codedeploy-eu-west-3.s3.eu-west-3.amazonaws.com/latest/install
chmod +x ./install
sudo ./install auto
sudo service codedeploy-agent status

# Install node npm
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.3/install.sh | bash
. ~/.nvm/nvm.sh
nvm install --lts
sudo yum install -y nodejs

# Install pm2
sudo npm install pm2 -g