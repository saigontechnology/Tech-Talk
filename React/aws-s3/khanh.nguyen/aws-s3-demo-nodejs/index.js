const express = require("express");
const app = express();
const port = 8000;
const AWS = require("aws-sdk");
const cors = require("cors");
require("dotenv").config();
app.use(express.json());
app.use(cors());

AWS.config.update({
  accessKeyId: process.env.AWS_ACCESS_KEY,
  secretAccessKey: process.env.AWS_SECRET_KEY,
  region: process.env.REGION,
  signatureVersion: "v4",
});
const s3 = new AWS.S3();

app.get("/", (req, res) => {
  const myBucket = process.env.BUCKET;
  const myKey = process.env.EXISTED_FILE_NAME;
  const signedUrlExpireSeconds = 60 * 10;
  const url = s3.getSignedUrl("getObject", {
    Bucket: myBucket,
    Key: myKey,
    Expires: signedUrlExpireSeconds,
  });
  console.log("url=", url);
  res.send(url);
});
app.post("/", (req, res) => {
  const myBucket = process.env.BUCKET;
  console.log("req=", req.body);
  const myKey = req.body.name;
  const signedUrlExpireSeconds = 60 * 1;
  const url = s3.getSignedUrl("putObject", {
    Bucket: myBucket,
    Key: `${myKey}`,
    ContentType: "image/png",
    Expires: signedUrlExpireSeconds,
  });
  console.log("url=", url);
  res.send(url);
});

app.listen(port, () => {
  console.log(`Example app listening on port ${port}`);
});
