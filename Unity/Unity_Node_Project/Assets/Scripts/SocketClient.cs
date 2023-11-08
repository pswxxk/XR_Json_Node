using WebSocketSharp;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.UI;

public class MyData             //JSON 데이터를 위한 클래스
{
    public string clientID;
    public string message;
    public int requestType;
}

public class SocketClient : MonoBehaviour
{
    private WebSocket webSocket;
    private bool IsConnected = false;
    private int ConnectionAttempt = 0;
    private const int MaxConnectionAttempts = 3;

    public Text text;
    public InputField inputField;

    MyData sendData = new MyData { message = "메세지 보내기" };
    // Start is called before the first frame update
    void Start()
    {
        ConnectWebSock();
    }

    void ConnectWebSock()
    {
        webSocket = new WebSocket("ws://localhost:8000");       //8000포트에 연결 
        webSocket.OnOpen += OnWebSocketOpen;                    //함수 이벤트 등록
        webSocket.OnMessage += OnWebSocketMessage;              //함수 이벤트 등록 
        webSocket.OnClose += OnWebSocketClose;                  //함수 이벤트 등록 

        webSocket.ConnectAsync();
    }

    void OnWebSocketOpen(object sender , System.EventArgs e)
    {
        Debug.Log("WebSocket connected");
        IsConnected = true;
        ConnectionAttempt = 0;
    }

    void OnWebSocketMessage(object sender, MessageEventArgs e)
    {
        string jsonData = Encoding.Default.GetString(e.RawData);
        Debug.Log("Received JSON data : " + jsonData);
        text.text = jsonData;

        //JSON 데이터를 객체로 역직렬화 
        MyData receivedData = JsonConvert.DeserializeObject<MyData>(jsonData);

        if(receivedData != null && !string.IsNullOrEmpty(receivedData.clientID))
        {
            sendData.clientID = receivedData.clientID;
        } 
    }

    void OnWebSocketClose(object sender , CloseEventArgs e)
    {
        Debug.Log("WebSocket connection closed");
        IsConnected = false;

        if(ConnectionAttempt < MaxConnectionAttempts)
        { 
            ConnectionAttempt++;
            Debug.Log("Attempting to reconnect . Attempt : " + ConnectionAttempt);
        }
        else
        {
            Debug.Log("Failed to connect ager " + MaxConnectionAttempts + "attempts. ");
        }
    }

    void OnApplicationQuit()        //어플 종료되면 소캣 제거
    {
        DisconnectWebSocket();
    }

    void DisconnectWebSocket()
    {
        if(webSocket != null && IsConnected)
        {
            webSocket.Close();
            IsConnected = false;
        }
    }

    void Update()
    {
        if (webSocket == null || !IsConnected) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //JSON 데이터 생성
            sendData.requestType = 0;
            string jsonData = JsonConvert.SerializeObject(sendData);

            webSocket.Send(jsonData);
        }
    }

    public void SendMessage()
    {
        sendData.requestType = 0;
        sendData.message = inputField.text;
        string jsondata = JsonConvert.SerializeObject(sendData);

        webSocket.Send(jsondata);
    }


}
