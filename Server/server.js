const fs = require('fs');
const WebSocket = require('ws');
const { makeCsApi } = require('./makeApi');
const firebase = require('./firebase');

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

const waitingQ = new Set();

// connect server --------------------
const wss = new WebSocket.Server({ port: 8000 }, () => { 
    console.log('PlanetMerge Server Start');
});

wss.on('connection', function connection(ws) {
    ws.on('message', async (reqPacket) => {
        reqPacket = JSON.parse(reqPacket);

        const apiName = reqPacket.apiName;
        if (waitingQ.has(apiName)) {
            return;
        }
        waitingQ.add(apiName);
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
        waitingQ.delete(apiName);
    });
});

wss.on('listening', () => { 
    console.log('listening...');
});