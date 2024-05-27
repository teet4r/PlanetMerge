const fs = require('fs');

exports.makeCsApi = function(api, apiName) {
    const className = `Api_${apiName}`;
    let requestClass = '';
    let responseClass = '';
    const requestParams = [];
    const requestParams2 = [];
    
    for (const [name, type] of Object.entries(api.Response)) {
        requestClass += `\t\tpublic ${type} ${name};\n`;
        requestParams.push(`${type} ${name}`);
        requestParams2.push(`\t\t\t\t${name} = ${name},`);
    }
    for (const [name, type] of Object.entries(api.Request)) {
        responseClass += `\t\tpublic ${type} ${name};\n`;
    }

    const csApi = 
`using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ${className}
{
    public class Request
    {
${requestClass}    }

    public class Response
    {
${responseClass}    }

    public static async UniTask<Response> Send(${requestParams.join(', ')})
    {
        var result = await WebSocketManager.Send<Request, Response>(
            "${apiName}",
            new Request()
            {
${requestParams2.join('\n')}
            }
        );

        return result;
    }
}
`
    fs.writeFile(`../Client/Assets/Scripts/Api/${className}.cs`, csApi, (err) => {
        if (!err) {
            console.log(`${className} is created`);
        }
    });
}