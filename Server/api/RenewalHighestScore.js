const userManager = require('../userManager');
const db = require('../db');

exports.Request = {
    userId: 'string',
    score: 'long',
}

exports.Response = {
    highestScore: 'long',
}

exports.api = async function(userId, score) {
    const user = await userManager.get(userId);

    if (!user) {
        return {};
    }

    const highestScore = Math.max(0, score);

    await db.query('UPDATE users SET highestScore = ? WHERE id = ?', [highestScore, userId]);

    user.highestScore = highestScore;

    return {
        highestScore
    };
}