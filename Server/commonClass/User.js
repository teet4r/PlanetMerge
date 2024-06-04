class User {
    constructor(uid) {
        this.uid = uid;
        this.highestScore = 0;
    }
};

exports.class = User;
exports.metafile = {
    uid: 'string',
    highestScore: 'long',
};