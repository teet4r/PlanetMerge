const moment = require('moment/moment');
const db = require('./db');
const User = require('./user');

const users = {};

exports.login = async function(userId) {
    let user = users[userId];

    if (user)
        return;
    
    user = await db.query('SELECT * FROM users WHERE id = ?', [userId]);

    if (!user) {
        await db.query('INSERT INTO users(id) VALUES(?)', [userId]);
        user = new User(userId);
    }

    await db.query(
        'UPDATE loginlogs SET time = ? WHERE userId = ?',
        [moment.utc().format('YYYY-MM-DD hh:mm:ss'), userId]
    );

    users[userId] = user;
}

exports.get = async function(userId) {
    return users[userId];
}