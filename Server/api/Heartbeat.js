const userManager = require('../userManager');

exports.Request = {
    uid: 'string',
}

exports.Response = {
    success: 'bool',
}

exports.api = async function(uid) {
    if (!userManager.getUser(uid)) {
        userManager.unload(uid);
        
        return { success: false };
    }

    return { success: true };
}