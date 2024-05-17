const mysql = require('mysql2/promise');
const dbConfig = require('./dbConfig');

let pool;

async function initialize() {
    pool = await mysql.createPool({
        host: dbConfig.host,
        user: dbConfig.user,
        port: dbConfig.port,
        password: dbConfig.password,
        database: dbConfig.database,
    });
}

initialize();

async function query(sql, values = []) {
    const connection = await pool.getConnection();

    const [results, fields] = await connection.query(sql, values);

    connection.release();

    return results;
}

module.exports = { query };