const { DataTypes } = require('sequelize');

module.exports = (sequelize, Sequelize) => {
  const WebhooksLogs = sequelize.define("webhooks-logs", {
    id: {
      type: DataTypes.UUID,
      primaryKey: true,
      defaultValue: DataTypes.UUIDV4
    },
    content: {
      type: Sequelize.STRING,
    },
  });

  return WebhooksLogs;
};
