module.exports = app => {
  const webhookReceiver = require("../controllers/webhooksReceiver.controller");

  var router = require("express").Router();

  // Create a new webhookReceiver
  router.post("/", webhookReceiver.create);

  // Retrieve all webhookReceiver
  router.get("/", webhookReceiver.findAll);

  // Retrieve all published webhookReceiver
  router.get("/published", webhookReceiver.findAllPublished);

  // Retrieve a single webhookReceiver with id
  router.get("/:id", webhookReceiver.findOne);

  // Update a webhookReceiver with id
  router.put("/:id", webhookReceiver.update);

  // Delete a webhookReceiver with id
  router.delete("/:id", webhookReceiver.delete);

  // Delete all webhookReceiver
  router.delete("/", webhookReceiver.deleteAll);

  app.use('/api/webhooks-receiver', router);
};
