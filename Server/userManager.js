const db = require('./db');
const fb = require('./firebase');
const User = require('./user');

const users = {};

exports.load = async function(uid) {
    const fbUser = await fb.getUser(uid);
    if (!fbUser) {
        return;
    }

    const email = fbUser.email;

    const dbUser = (await db.query('SELECT * FROM users WHERE uid = ?;', [uid]))[0];
    if (!dbUser) {
        await db.query('INSERT INTO users(uid, email) VALUES(?, ?);', [uid, email]);
        dbUser = new User(uid, email);
    }

    return users[uid] = dbUser;
}

exports.getUser = function(uid) {
    return users[uid];
}