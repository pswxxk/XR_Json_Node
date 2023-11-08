const WebSocket = require('ws');

console.log(genKey(10));

const wss = new WebSocket.Server({ port:8000 } , () => {        //소캣을 포트 8000번에 시작 시킨다.
    console.log('서버 시작');
});

const userList = [];

wss.on('connection' , function connections(ws){                 //커넥션이 되었을 때
    ws.clientID = genKey(8);

    ws.on('message' , (data) => {
        const jsonData = JSON.parse(data);
        console.log('받은 데이터 : ' , jsonData);

        if(jsonData.requestType == 10)
        {
            ws.send(JSON.stringify(userList));
        }

        wss.clients.forEach((client)=>
        {
            client.send(data);
        })

    });

    ws.on('close', ()=> {
        const index = userList.indexOf(ws.clientID);
        if(index !== -1)
        {
            console.log('클라이언트가 해제됨 - ID : ' , ws.clientID);
            userList.splice(index, 1);         //배열에서 해당 클라이언트 제거
        }
    });
    //새로 연결된 클라이언트를 유저 리스트에 추가
    userList.push(ws.clientID);
    //클라이언트에게 임시 유저 이름 전송
    ws.send(JSON.stringify({clientID : ws.clientID}));
    //연결된 클라이언트에게 유저 이름 로그 출력
    console.log('클라이언트 연결 - ID :' , ws.clientID);
})

wss.on('listening' , () => {
    console.log('리스닝...');
});

function genKey(length) {
    let result = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';

    for (let i = 0; i < length; i++)
    {
        result += characters.charAt(Math.floor(Math.random() * characters.length));
    }
    return result;
}