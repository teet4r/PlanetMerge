const fs = require('fs');

exports.makeCsApi = function(api, apiName) {
    const className = `Api_${apiName}`;
    let requestClass = '';
    let responseClass = '';
    const requestParams = [];
    const requestParams2 = [];
    const additionalUsings = api.AdditionalUsings;
    let additionalUsingsText = '';
    
    for (const [name, type] of Object.entries(api.Request)) {
        requestClass += `\t\tpublic ${type} ${name};\n`;
        requestParams.push(`${type} ${name}`);
        requestParams2.push(`\t\t\t\t${name} = ${name},`);
    }
    for (const [name, type] of Object.entries(api.Response)) {
        responseClass += `\t\tpublic ${type} ${name};\n`;
    }
    if (additionalUsings) {
        for (const usings of additionalUsings) {
            additionalUsingsText += `${usings}\n`;
        }
    }

    const csApi = 
`using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
${additionalUsingsText}
public static class ${className}
{
    [System.Serializable]
    public class Request
    {
${requestClass}    }

    [System.Serializable]
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
}`

    fs.writeFile(`../Client/Assets/Scripts/Api/${className}.cs`, csApi, (err) => {
        if (!err) {
            console.log(`${className} is created`);
        }
    });
}