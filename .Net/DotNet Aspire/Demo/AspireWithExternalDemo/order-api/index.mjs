import { logMessage } from './instrumentation.mjs'
import fetch from 'node-fetch';
import { env } from 'node:process';
import bodyParser from 'body-parser';
import { createServer } from 'node:http';
import { createTerminus, HealthCheckError } from '@godaddy/terminus';
import express from 'express';

logMessage('Node js app is starting...')

const app = express();
const environment = process.env.NODE_ENV || 'development';
const port = env.PORT ?? 3001;

const productApi = env['services__product__https__0'] ?? env['services__product__http__0'];

console.log(`environment: ${environment}`);
console.log(`productApi: ${productApi}`);

app.use(bodyParser.json());

let orders = [];

app.set('views', './views');
app.set('view engine', 'pug');

const server = createServer(app);

async function healthCheck() {
    const errors = [];
    const productApiHealthAddress = `${productApi}/health`;
    console.log(`Fetching ${productApiHealthAddress}`);
    try {
        var response = await fetch(productApiHealthAddress);
        if (!response.ok) {
            console.log(`Failed fetching ${productApiHealthAddress}. ${response.status}`);
            throw new HealthCheckError(`Fetching ${productApiHealthAddress} failed with HTTP status: ${response.status}`);
        }
    } catch (error) {
        console.log(`Failed fetching ${productApiHealthAddress}. ${error}`);
        throw new HealthCheckError(`Fetching ${productApiHealthAddress} failed with HTTP status: ${error}`);
    }
}

createTerminus(server, {
    signal: 'SIGINT',
    healthChecks: {
        '/health': healthCheck,
        '/alive': () => { }
    },
    onSignal: async () => {
        console.log('server is starting cleanup');
        console.log('closing Redis connection');
        await cache.disconnect();
    },
    onShutdown: () => console.log('cleanup finished, server is shutting down')
});

app.get('/', async (req, res) => {
    logMessage('fetching products...')
    let response = await fetch(`${productApi}/product`);
    let products = await response.json();
    logMessage('fetching products end')
    res.render('index', { products: products });
});

// Create an order
app.post('/orders', (req, res) => {
    const { product_id, ordered_at, quantity, note } = req.body;
    const order = new Order(product_id, ordered_at, quantity, note);
    orders.push(order);
    res.status(201).send(order);
});

// Get all orders
app.get('/orders', (req, res) => {
    logMessage('get orders')
    res.send(orders);
});

// Get an order by ID
app.get('/orders/:id', (req, res) => {
    const order = orders.find(o => o.id === req.params.id);
    if (!order) {
        return res.status(404).send({ error: 'Order not found' });
    }
    res.send(order);
});

// Update an order by ID
app.put('/orders/:id', (req, res) => {
    const order = orders.find(o => o.id === req.params.id);
    if (!order) {
        return res.status(404).send({ error: 'Order not found' });
    }
    const { product_id, ordered_at, quantity, note } = req.body;
    order.product_id = product_id;
    order.ordered_at = ordered_at;
    order.quantity = quantity;
    order.note = note;
    res.send(order);
});

// Delete an order by ID
app.delete('/orders/:id', (req, res) => {
    const orderIndex = orders.findIndex(o => o.id === req.params.id);
    if (orderIndex === -1) {
        return res.status(404).send({ error: 'Order not found' });
    }
    orders.splice(orderIndex, 1);
    res.status(204).send();
});

server.listen(port, () => {
    console.log(`Order API is listening ${port}`);
});