const AWS = require("aws-sdk");
const fs = require("fs");

const accessKeyId = "accessKeyId";
const secretAccessKey = "secretAccessKey";

// Configure AWS SDK with your credentials
AWS.config.update({
  accessKeyId: accessKeyId,
  secretAccessKey: secretAccessKey,
});

// Create an instance of the S3 service
const s3 = new AWS.S3();

// Specify the bucket and key for the destination in S3
const bucketName = "demo-nhatdao-s3";
const filePath = "cat.jpg";
const keyName = "cats/" + filePath;

// Read the file from the local file system
const fileContent = fs.readFileSync(filePath);

// Set the parameters for the S3 upload
const params = {
  Bucket: bucketName,
  Key: keyName,
  Body: fileContent,
};

// Upload the file to S3
s3.upload(params, function (err, data) {
  if (err) {
    console.error("Error uploading file:", err);
  } else {
    console.log("File uploaded successfully:", data.Location);
    fs.appendFile("images.txt", data.Location, function (err) {
      if (err) throw err;
      console.log("File is created successfully.");
    });
  }
});
