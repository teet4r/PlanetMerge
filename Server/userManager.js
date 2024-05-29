const moment = require('moment/moment');
const db = require('./db');
const User = require('./user');

const users = {};

exports.load = async function(user) {
    users[user.id] = user;
}

exports.get = async function(user) {
    return users[user.id];
}