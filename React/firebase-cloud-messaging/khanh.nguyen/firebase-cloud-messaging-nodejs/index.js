const express = require("express");
const axios = require("axios");
const app = express();
const port = 8000;
const cors = require("cors");
require("dotenv").config();
app.use(express.json());
app.use(cors());

app.listen(port, () => {
  console.log(`Example app listening on port ${port}`);
});

app.get("/", (req, res) => {
  res.send("OK");
});
app.post("/", (req, res) => {
  const token = req.body.token;
  console.log("token=", token);
  axios
    .post(
      "https://fcm.googleapis.com/fcm/send",
      {
        to: token,
        notification: {
          title: "Test Title",
          body: "Test body",
        },
      },
      {
        headers: {
          Authorization: "key=" + process.env.API_KEY,
          "Content-Type": "application/json",
        },
      }
    )
    .then(() => {
      console.log("send success");
    });
  res.send("OK");
});
