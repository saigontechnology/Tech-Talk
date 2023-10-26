'use strict';
const {
  Model
} = require('sequelize');
module.exports = (sequelize, DataTypes) => {
  class WebhooksReceiver extends Model {
    /**
     * Helper method for defining associations.
     * This method is not a part of Sequelize lifecycle.
     * The `models/index` file will call this method automatically.
     */
    static associate(models) {
      // define association here
    }
  }
  WebhooksReceiver.init({
    receiverUrl: DataTypes.STRING,
    token: DataTypes.STRING
  }, {
    sequelize,
    modelName: 'WebhooksReceiver',
  });
  return WebhooksReceiver;
};