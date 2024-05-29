const fs = require('fs');
const WebSocket = require('ws');
const { makeCsApi } = require('./makeApi');

// load methods --------------------
const methods = {};
const fileList = fs.readdirSync('./api');

for (const file of fileList) {
    const api = require(`./api/${file}`);
    const apiName = file.substring(0, file.length - 3);

    makeCsApi(api, apiName);
    for (const [name, property] of Object.entries(api)) {
        if (name == 'api') {    
            methods[apiName] = property;
        }
    }
}

// const express = require('express');
// const app = express();
// const port = 8000;

// app.use(express.urlencoded({ extended: true }));

// app.post('/', (req, res) => {
//     console.log(req.body);
//     res.send('Hello World!');
// });

// app.listen(port, () => {
//   console.log(`PlanetMerge Server running on port ${port}...`);
// });

// connect server --------------------
const wss = new WebSocket.Server({ port: 8000 }, () => { 
    console.log('PlanetMerge Server Start');
});

wss.on('connection', function connection(ws) {
    ws.on('message', async (reqPacket) => {
        reqPacket = JSON.parse(reqPacket);

        const apiName = reqPacket.apiName;
        const data = JSON.parse(reqPacket.data);

        const args = [];
        for (const key in data) {
            args.push(data[key]);
        }

        const result = await methods[apiName](...args);
        const resPacket = {
            apiName: apiName,
            data: JSON.stringify(result),
        }
        
        ws.send(JSON.stringify(resPacket));
    });
});

wss.on('listening', () => { 
    console.log('listening...');
});