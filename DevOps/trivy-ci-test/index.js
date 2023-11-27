const express = require('express');
const bodyParser = require('body-parser');

const app = express();
const port = 3000;

// Add a middleware to make it handle json
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());
app.get('/', (req, res) => {
    res.json("Hello world");
});


app.listen(port, () => console.log(`App listening on port ${port}!`))