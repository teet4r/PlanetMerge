const db = require('./db');
const fb = require('./firebase');
const User = require('./commonClass/User').class;

const users = {};

exports.load = async function(uid) {
    const fbUser = await fb.getUser(uid);
    if (!fbUser) {
        return;
    }

    const dbUser = (await db.query('SELECT * FROM users WHERE uid = ?;', [uid]))[0];
    if (!dbUser) {
        await db.query('INSERT INTO users(uid, email) VALUES(?, ?);', [uid, fbUser.email]);
        dbUser = new User(uid);
    }

    return users[uid] = dbUser;
}

exports.unload = async function(uid) {
    delete users[uid];
}

exports.getUser = function(uid) {
    return users[uid];
}

exports.logAllUsers = function() {
    console.log(users);
}