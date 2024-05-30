const userManager = require('../userManager');
const db = require('../db');
const User = require('../user');
const moment = require('moment');

exports.Request = {
    userId: 'string',
    email: 'string',
}

exports.Response = {
    success: 'bool',
}

exports.api = async function(userId, email) {
    const result = await db.query('SELECT * FROM users WHERE id = ?;', [userId]);
    const user = result[0];
    const utcNow = moment.utc().format('YYYY-MM-DD hh:mm:ss');

    if (!user) {
        await db.query(
            'INSERT INTO users(id, email) VALUES(?, ?);',
            [userId, email],
        );
    }
    
    await db.query(
        'INSERT INTO loginlogs VALUES(?, ?);',
        [userId, utcNow]
    );

    userManager.load(new User(userId));

    return {};
}