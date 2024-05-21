const WebSocket = require('ws');
const db = require('./db');

const wss = new WebSocket.Server({ port: 8000 }, () => { 
    console.log('PlanetMerge Server Start');
});

wss.on('connection', function connection(ws) { 
    ws.on('message', async (data) => {
        const dataStr = data.toString();
        console.log(dataStr);

        const strs = dataStr.split(' ');
        if (strs.length == 1)
            await db.query('INSERT INTO users VALUES(?)', [strs[0]]);
        else
            await db.query('INSERT INTO userscores VALUES(?, ?)', [strs[0], strs[1]]);

        // ws.send(JSON.stringify(result));
    });
});

wss.on('listening', () => { 
    console.log('listening...');
});