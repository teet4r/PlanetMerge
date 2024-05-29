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
        multipleStatements: true,
    });
}

initialize();

exports.query = async function(sql, values = []) {
    return this.queries([sql], [values]);
}

exports.queries = async function(sqls = [], values = [[]]) {
    const connection = await pool.getConnection();
    let queries = '';

    for (let i = 0; i < sqls.length; ++i) {
        queries += mysql.format(sqls[i], values[i]); 
    }

    const [results, fields] = await connection.query(queries);

    connection.release();

    return results;
}