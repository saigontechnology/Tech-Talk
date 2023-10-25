const db = require("../models");
const User = db.users;
const Op = db.Sequelize.Op;
const WebhooksReceiver = db.webhooksReceiver;
const webhooksReceiver = require("./webhooksReceiver.controller");
const crypto = require('crypto');

function computeHmacSha256(data, secret) {
  const hmac = crypto.createHmac('sha256', secret);
  hmac.update(data);
  return hmac.digest('hex');
}

// Create and Save a new User
exports.create = (req, res) => {
  // Validate request
  if (!req.body.email) {
    res.status(400).send({
      message: "Content can not be empty!"
    });
    return;
  }

  // Create a User
  const user = {
    email: req.body.email,
    phone: req.body.phone,
    published: req.body.published ? req.body.published : false
  };

  const body = `Create user - ${JSON.stringify(user)}`;

  const secretKey = 'your-secret-key'; // Replace with your actual secret key
  const signature = computeHmacSha256(JSON.stringify(body), secretKey);

  // Save User in the database
  User.create(user)
    .then(async data => {
      const result = await WebhooksReceiver.findAll();
      result.map(item => {
        const itemFormat = item.toJSON();
        webhooksReceiver.sendWebhookMessage(itemFormat?.receiverUrl, itemFormat?.token, body, { signature: signature });
      });
      res.send(data);
    })
    .catch(err => {
      res.status(500).send({
        message:
          err.message || "Some error occurred while creating the User."
      });
    });
};

// Retrieve all User from the database.
exports.findAll = (req, res) => {
  const email = req.query.email;
  var condition = email ? { email: { [Op.like]: `%${email}%` } } : null;

  User.findAll({ where: condition })
    .then(data => {
      res.send(data);
    })
    .catch(err => {
      res.status(500).send({
        message:
          err.message || "Some error occurred while retrieving User."
      });
    });
};

// Find a single User with an id
exports.findOne = (req, res) => {
  const id = req.params.id;

  User.findByPk(id)
    .then(data => {
      if (data) {
        res.send(data);
      } else {
        res.status(404).send({
          message: `Cannot find User with id=${id}.`
        });
      }
    })
    .catch(err => {
      res.status(500).send({
        message: "Error retrieving User with id=" + id
      });
    });
};

// Update a User by the id in the request
exports.update = (req, res) => {
  const id = req.params.id;

  User.update(req.body, {
    where: { id: id }
  })
    .then(async num => {
      if (num == 1) {
        const result = await WebhooksReceiver.findAll();
        result.map(item => {
          const itemFormat = item.toJSON();
          webhooksReceiver.sendWebhookMessage(itemFormat?.receiverUrl, itemFormat?.token, `Edit user - ${JSON.stringify(user)}`);
        });
        res.send({
          message: "User was updated successfully."
        });
      } else {
        res.send({
          message: `Cannot update User with id=${id}. Maybe User was not found or req.body is empty!`
        });
      }
    })
    .catch(err => {
      res.status(500).send({
        message: "Error updating User with id=" + id
      });
    });
};

// Delete a User with the specified id in the request
exports.delete = (req, res) => {
  const id = req.params.id;

  const body = `Delete user - ${id}`;

  const secretKey = 'your-secret-key'; // Replace with your actual secret key
  const signature = computeHmacSha256(JSON.stringify(body), secretKey);

  User.destroy({
    where: { id: id }
  })
    .then(async num => {
      if (num == 1) {
        const result = await WebhooksReceiver.findAll();
        result.map(item => {
          const itemFormat = item.toJSON();
          webhooksReceiver.sendWebhookMessage(itemFormat?.receiverUrl, itemFormat?.token, body, { signature: signature });
        });
        res.send({
          message: "User was deleted successfully!"
        });
      } else {
        res.send({
          message: `Cannot delete User with id=${id}. Maybe User was not found!`
        });
      }
    })
    .catch(err => {
      res.status(500).send({
        message: "Could not delete User with id=" + id
      });
    });
};

// Delete all User from the database.
exports.deleteAll = (req, res) => {
  User.destroy({
    where: {},
    truncate: false
  })
    .then(nums => {
      res.send({ message: `${nums} User were deleted successfully!` });
    })
    .catch(err => {
      res.status(500).send({
        message:
          err.message || "Some error occurred while removing all User."
      });
    });
};

// find all published User
exports.findAllPublished = (req, res) => {
  User.findAll({ where: { published: true } })
    .then(data => {
      res.send(data);
    })
    .catch(err => {
      res.status(500).send({
        message:
          err.message || "Some error occurred while retrieving Users."
      });
    });
};
