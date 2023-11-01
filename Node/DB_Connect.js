require('dotenv').config();

const mysql = require('mysql');

//MySQL 연결 설정

const connection = mysql.createConnection({
    host: process.env.DB_HOST,
    port: process.env.DB_PORT,
    user: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_NAME,
});

//MYSQL 연결
connection.connect((err) =>{
    if(err)
    {
        console.error("MYSQL 연결 오류 : " + err.stack);
        return;
    }

    console.log("연결 되었습니다. 연결 ID : " + connection.threadID)
}); 

connection.query('SELECT * FROM userid', (err, result, field) => {
    if(err) throw err;

    const dataArray = result;

    console.log('데이터 배열 : ' , dataArray);
});

connection.end((err) => {
    if(err) {
        console.error('MYSQL 연결 종료 오류 : ' + err.stack);
        return;
    }
    console.log("MYSQL 연결이 성공적으로 종료되었습니다.");
});