module.exports = app => {
  const webhooksLogs = require("../controllers/webhooksLogs.controller.js");

  var router = require("express").Router();

  // Create a new webhookLogs
  router.post("/webhooks-logs", webhooksLogs.create);

  // Retrieve all webhooksLogs
  router.get("/webhooks-logs", webhooksLogs.findAll);

  // Delete all webhooksLogs
  router.delete("/webhooks-logs", webhooksLogs.deleteAll);

  app.use('/api/client', router);
};
