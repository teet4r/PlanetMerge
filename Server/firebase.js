// npm install firebase-admin
const admin = require("firebase-admin");
const serviceAccount = require("./planetmerge-30b99-firebase-adminsdk-3kwx0-2d09fb1b99.json");

async function initialize() {
    admin.initializeApp({ credential: admin.credential.cert(serviceAccount) });
}

initialize();

exports.getUser = async function(uid) {
    let record;

    try {
        record = await admin.auth().getUser(uid);
    }
    catch (e) {
        return null;
    }

    return record;
}