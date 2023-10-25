const { DataTypes } = require('sequelize');

module.exports = (sequelize, Sequelize) => {
  const WebhooksReceiver = sequelize.define("webhooks-receiver", {
    id: {
      type: DataTypes.UUID,
      primaryKey: true,
      defaultValue: DataTypes.UUIDV4
    },
    receiverUrl: {
      type: Sequelize.STRING,
    },
    token: {
      type: Sequelize.STRING
    },
  });

  return WebhooksReceiver;
};
