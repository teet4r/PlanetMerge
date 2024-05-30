// npm install firebase-admin
const admin = require("firebase-admin");
const serviceAccount = require("./planetmerge-30b99-firebase-adminsdk-3kwx0-2d09fb1b99.json");

// Initialize Firebase
exports.initialize = async function() {
    admin.initializeApp({ credential: admin.credential.cert(serviceAccount) });
    const users = await admin.app().auth().listUsers();
    console.log(users);
}