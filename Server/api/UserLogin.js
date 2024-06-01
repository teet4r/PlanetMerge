const userManager = require('../userManager');
const db = require('../db');
const moment = require('moment');

exports.Request = {
    uid: 'string',
}

exports.Response = {
    success: 'bool',
}

exports.api = async function(uid) {
    const utcNow = moment.utc().format('YYYY-MM-DD hh:mm:ss');
    const user = await userManager.load(uid);
    if (!user) {
        return { success: false };
    }

    await db.query(
        'INSERT INTO loginlogs VALUES(?, ?);',
        [uid, utcNow]
    );

    return { success: true };
}