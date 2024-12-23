const axios = require('axios');
var express = require('express');
var router = express.Router();

/* GET users listing. */
router.get('/', async function(req, res, next) {
  try {
    await axios.get('https://saigontechonolgy.com/srv1/api/users')
  } catch(err) {
  }

  res.send([]);
});

module.exports = router;
