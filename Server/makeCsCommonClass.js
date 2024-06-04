const fs = require('fs');

exports.makeCsCommonClass = function(classFile, className) {
    const csClassName = `${className}Data`;
    let declares = '';

    for (const [name, type] of Object.entries(classFile.metafile)) {
        declares += `\tpublic ${type} ${name};\n`;
    }

    const classFileText = 
`[System.Serializable]
public class ${csClassName}
{
${declares}}`

    fs.writeFile(`../Client/Assets/Scripts/ClassData/${csClassName}.cs`, classFileText, (err) => {
        if (!err) {
            console.log(`${csClassName} is created`);
        }
    });
}