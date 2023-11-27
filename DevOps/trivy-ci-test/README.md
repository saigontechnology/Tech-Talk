# trivy-ci-test

## CLI useful
1. Container image
```bash
trivy image [YOUR_IMAGE_NAME]
```

2. Filesystem
```bash
trivy fs /path/to/project
```

3. Rootfs
```bash
trivy rootfs /path/to/rootfs
```

4. Code repository
```bash
trivy repo (REPO_PATH | REPO_URL)
```


5. Virtual Machine Image
```bash
#Local file
trivy vm --scanners vuln disk.vmdk #VM image

#Amazon Machine Image (AMI)
trivy vm ami:${your_ami_id} #VM image

```

6. Kubernetes
```bash
trivy k8s all
trivy k8s pods
trivy k8s deploy myapp
trivy k8s pod/mypod
trivy k8s pods,deploy
trivy k8s cluster
```

7. Amazon Web Services
```bash
trivy aws --region us-east-1
trivy aws --service s3
trivy aws --service s3 --service ec2
```

8. SBOM
```bash
trivy sbom /path/to/sbom_file
```
