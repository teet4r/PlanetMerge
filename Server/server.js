const WebSocket = require('ws');
const db = require('./db');

const wss = new WebSocket.Server({ port: 8000 }, () => { 
    console.log('PlanetMerge Server Start');
});

wss.on('connection', function connection(ws) { 
    ws.on('message', async (data) => { 
        console.log(data.toString());

        const result = await db.query('SELECT * FROM users');
        ws.send(JSON.stringify(result));
    });
});

wss.on('listening', () => { 
    console.log('listening...');
});