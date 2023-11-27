#!/bin/bash
set -x

echo $1 #IMAGE_NAME:TAG
echo $2 #severity:CRITICAL,HIGH,MEDIUM
echo $3 #exit-code: Default 0, if set 1, skip next steps
# Scan progressing
mkdir -p results

# Image container
docker run --rm \
    -v $PWD/html.tpl:/tmp/html.tpl \
    -v /var/run/docker.sock:/var/run/docker.sock \
    -v $PWD/results:/home aquasec/trivy:latest \
    -q image \
    --scanners vuln,config,secret \
    --exit-code $3 \
    --severity $2 \
    --format template \
    --template "@/tmp/html.tpl" \
    -o /home/trivyScan.html \
    --light $1

# SBOM
# docker run --rm \
#     -v /var/run/docker.sock:/var/run/docker.sock \
#     -v $PWD/results:/home aquasec/trivy:latest \
#     -q image \
#     --format cyclonedx \
#     -o /home/result.json \
#     --light $1