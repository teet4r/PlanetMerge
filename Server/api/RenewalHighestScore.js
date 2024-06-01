const userManager = require('../userManager');
const db = require('../db');

exports.Request = {
    uid: 'string',
    score: 'long',
}

exports.Response = {
    highestScore: 'long',
    success: 'bool',
}

exports.api = async function(uid, score) {
    const user = await userManager.getUser(uid);
    if (!user) {
        return { success: false };
    }

    const highestScore = Math.max(0, score);

    await db.query('UPDATE users SET highestScore = ? WHERE uid = ?', [highestScore, uid]);

    user.highestScore = highestScore;

    return {
        highestScore: highestScore,
        success: true,
    };
}