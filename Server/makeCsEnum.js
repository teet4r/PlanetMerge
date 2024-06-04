const fs = require('fs');

exports.makeCsEnum = function(enumFile, enumFileName) {
    let values = '';

    Object.freeze(enumFile);

    for (const [name, value] of Object.entries(enumFile[enumFileName])) {
        values += `\t${name} = ${value},\n`;
    }

    const enumFileText = 
`public enum ${enumFileName}
{
${values}}`

    fs.writeFile(`../Client/Assets/Scripts/Enums/${enumFileName}.cs`, enumFileText, (err) => {
        if (!err) {
            console.log(`${enumFileName} is created`);
        }
    });
}