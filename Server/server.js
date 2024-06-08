const fs = require('fs');
const WebSocket = require('ws');
const { makeCsApi } = require('./makeCsApi');
const { makeCsEnum } = require('./makeCsEnum');
const { makeCsCommonClass } = require('./makeCsCommonClass');
const userManager = require('./userManager');

// update files ----------------------------------------
const methods = {};
const apiNameList = fs.readdirSync('./api');
const enumNameList = fs.readdirSync('./enum');
const classNameList = fs.readdirSync('./commonClass');

for (const apiFullName of apiNameList) {
    const api = require(`./api/${apiFullName}`);
    const apiName = apiFullName.substring(0, apiFullName.length - 3);

    makeCsApi(api, apiName);

    for (const [name, property] of Object.entries(api)) {
        if (name == 'api') {    
            methods[apiName] = property;
        }
    }
}

for (const enumFullName of enumNameList) {
    const enumFile = require(`./enum/${enumFullName}`);
    const enumName = enumFullName.substring(0, enumFullName.length - 3);

    makeCsEnum(enumFile, enumName);
}

for (const classFullName of classNameList) {
    const classFile = require(`./commonClass/${classFullName}`);
    const className = classFullName.substring(0, classFullName.length - 3);

    makeCsCommonClass(classFile, className);
}

// connect server ----------------------------------------
const clients = {};

const wss = new WebSocket.Server({ port: 8000, }, function() { 
    console.log('PlanetMerge Server Start');
});

wss.on('connection', function connection(ws) {
    console.log('클라이언트 연결됨!');

    ws.onclose = function() {
        // userManager.unload();
        console.log('클라이언트 닫힘!');
    };

    ws.on('message', async (data) => {
        data = data.toString().split('/');

        const apiName = data[0];
        const api = methods[apiName];
        if (!api) {
            return;
        }
        const request = JSON.parse(data[1]);
        const args = [];
        for (const key in request) {
            args.push(request[key]);
        }

        const result = await api(...args);

        ws.send(`${apiName}/${JSON.stringify(result)}`);
    });
});

wss.on('listening', () => { 
    console.log('listening...');
});

// setInterval(() => {
//     console.log(userManager.logAllUsers());
// }, 2000);