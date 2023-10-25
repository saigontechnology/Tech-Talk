const db = require("../models");
const WebhooksLogs = db.webhooksLogs;

const crypto = require('crypto');

function computeHmacSha256(data, secret) {
  const hmac = crypto.createHmac('sha256', secret);
  hmac.update(data);
  return hmac.digest('hex');
}

// Retrieve all User from the database.
exports.findAll = (req, res) => {
  WebhooksLogs.findAll({ order: [['createdAt', 'DESC']] })
    .then(data => {
      res.send(data);
    })
    .catch(err => {
      res.status(500).send({
        message:
          err.message || "Some error occurred while retrieving WebhooksLogs."
      });
    });
};

// Create and Save a new WebhooksLogs
exports.create = (req, res) => {
  const signatureReq = req.get('signature'); // Get the 'signature' header from the request

  const secretKey = 'your-secret-key'; // Replace with your actual secret key
  const calculatedSignature = computeHmacSha256(JSON.stringify(req.body?.data?.data || {}), secretKey);

  // Compare signatureReq with calculatedSignature
  if (signatureReq === calculatedSignature) {
    // Signatures match; proceed with processing the request
    // Create a WebhooksReceiver
    const webhooksLogs = {
      content: req.body?.data?.data,
    };

    // Save WebhooksLogs in the database
    WebhooksLogs.create(webhooksLogs)
      .then(data => {
        res.send(data);
      })
      .catch(err => {
        res.status(500).send({
          message:
            err.message || "Some error occurred while creating the WebhooksReceiver."
        });
      });
  } else {
    // Signatures don't match; reject the request with a 403 status code
    res.status(403).send({
      message: "Invalid signature",
    });
  }
};

// Delete all WebhooksLogs from the database.
exports.deleteAll = (req, res) => {
  WebhooksLogs.destroy({
    where: {},
    truncate: false
  })
    .then(nums => {
      res.send({ message: `${nums} WebhooksLogs were deleted successfully!` });
    })
    .catch(err => {
      res.status(500).send({
        message:
          err.message || "Some error occurred while removing all WebhooksLogs."
      });
    });
};