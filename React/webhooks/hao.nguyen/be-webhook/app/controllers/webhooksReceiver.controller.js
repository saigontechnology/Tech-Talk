const db = require("../models");
const axios = require('axios');
const WebhooksReceiver = db.webhooksReceiver;
const Op = db.Sequelize.Op;

// Create and Save a new WebhooksReceiver
exports.create = (req, res) => {
  // Validate request
  if (!req.body.receiverUrl) {
    res.status(400).send({
      message: "Content can not be empty!"
    });
    return;
  }

  // Create a WebhooksReceiver
  const webhooksReceiver = {
    receiverUrl: req.body.receiverUrl,
    token: req.body.token,
  };

  // Save WebhooksReceiver in the database
  WebhooksReceiver.create(webhooksReceiver)
    .then(data => {
      res.send(data);
    })
    .catch(err => {
      res.status(500).send({
        message:
          err.message || "Some error occurred while creating the WebhooksReceiver."
      });
    });
};

// Retrieve all WebhooksReceiver from the database.
exports.findAll = (req, res) => {
  WebhooksReceiver.findAll({ raw: true })
    .then(data => {
      res.send(data);
    })
    .catch(err => {
      res.status(500).send({
        message:
          err.message || "Some error occurred while retrieving WebhooksReceiver."
      });
    });
};

// Find a single WebhooksReceiver with an id
exports.findOne = (req, res) => {
  const id = req.params.id;

  WebhooksReceiver.findByPk(id)
    .then(data => {
      if (data) {
        res.send(data);
      } else {
        res.status(404).send({
          message: `Cannot find WebhooksReceiver with id=${id}.`
        });
      }
    })
    .catch(err => {
      res.status(500).send({
        message: "Error retrieving WebhooksReceiver with id=" + id
      });
    });
};

// Update a WebhooksReceiver by the id in the request
exports.update = (req, res) => {
  const id = req.params.id;

  WebhooksReceiver.update(req.body, {
    where: { id: id }
  })
    .then(num => {
      if (num == 1) {
        res.send({
          message: "WebhooksReceiver was updated successfully."
        });
      } else {
        res.send({
          message: `Cannot update WebhooksReceiver with id=${id}. Maybe WebhooksReceiver was not found or req.body is empty!`
        });
      }
    })
    .catch(err => {
      res.status(500).send({
        message: "Error updating WebhooksReceiver with id=" + id
      });
    });
};

// Delete a WebhooksReceiver with the specified id in the request
exports.delete = (req, res) => {
  const id = req.params.id;

  WebhooksReceiver.destroy({
    where: { id: id }
  })
    .then(num => {
      if (num == 1) {
        res.send({
          message: "WebhooksReceiver was deleted successfully!"
        });
      } else {
        res.send({
          message: `Cannot delete WebhooksReceiver with id=${id}. Maybe WebhooksReceiver was not found!`
        });
      }
    })
    .catch(err => {
      res.status(500).send({
        message: "Could not delete WebhooksReceiver with id=" + id
      });
    });
};

// Delete all WebhooksReceiver from the database.
exports.deleteAll = (req, res) => {
  WebhooksReceiver.destroy({
    where: {},
    truncate: false
  })
    .then(nums => {
      res.send({ message: `${nums} WebhooksReceiver were deleted successfully!` });
    })
    .catch(err => {
      res.status(500).send({
        message:
          err.message || "Some error occurred while removing all WebhooksReceiver."
      });
    });
};

// find all published WebhooksReceiver
exports.findAllPublished = (req, res) => {
  WebhooksReceiver.findAll({ where: { published: true } })
    .then(data => {
      res.send(data);
    })
    .catch(err => {
      res.status(500).send({
        message:
          err.message || "Some error occurred while retrieving WebhooksReceiver."
      });
    });
};

exports.sendWebhookMessage = async (receiverUrl, token, data, customHeaders = {}) => {
  try {
    var url = new URL(receiverUrl);

    // webhook message
    var payload = JSON.stringify({
      data: {
        data: data, // order data
      },
      version: '1.0',
      // event_type: event, // ex: 'order.created'
    });

    const headers = {
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
      ...customHeaders, // Include custom headers here
    };

    const response = await axios({
      method: 'post',
      url: url.href,
      headers: headers,
      data: payload,
    });

    // Handle the response if needed
    console.log(response.data);
  } catch (error) {
    // Handle errors here
    console.error(error);
  }
};
