using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net;
using Assets.Script.InterNet;
using System;
using System.Threading;

/// <summary>
/// 客户端的设计理念，
///     每帧调用异步的接收和发送数据，将接收到的数据，解码后将数据发送给各个模块，让各个模块自己调用这个数据进行更新；
///     发送数据，使用异步方法，在发送的列表中取出数据进行发送。
/// </summary>
public class Client {
    public Socket clientSocket;
    private int port;
    private string ip;
    private List<byte> receiveDatacache;
    private SocketAsyncEventArgs receiveArgs;
    private bool isProcessRec = false;

    private bool isProcessSend = false;
    private SocketAsyncEventArgs sendArgs;
    private Queue<byte[]> sendDataCache;
    private bool isDisconnected = true;
    public Client(int port,string ip)
    {
        this.ip = ip;
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        this.port = port;
        receiveDatacache = new List<byte>();
        receiveArgs = new SocketAsyncEventArgs();
        receiveArgs.SetBuffer(new byte[1024], 0, 1024);
        receiveArgs.Completed += ReceiveArgs_Completed;
        sendArgs = new SocketAsyncEventArgs();
        sendArgs.Completed += SendArgs_Completed;
        sendDataCache = new Queue<byte[]>();
    }
    #region 开始连接服务器
    public void StartConnect()
    {
        try
        {
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            Debug.Log("连接服务器成功");
            isDisconnected = false;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }
    #endregion
    #region 发送消息
    /// <summary>
    /// 发送消息包
    /// </summary>
    /// <param name="opCode"></param>
    /// <param name="subCode"></param>
    /// <param name="value"></param>
    public void Send(int opCode, int subCode, object value)
    {
        SocketMessage msg = new SocketMessage(opCode,subCode,value);
        byte[] data = CodingTool.EncodingMessage(msg);
        byte[] packet = CodingTool.EncodingPacket(data);
        sendDataCache.Enqueue(packet);
        if (!isProcessSend)
        {
            ProcessingSend();
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msgID"></param>
    /// <param name="pbMsg"></param>
    public void Send(MSGID msgID, object pbMsg)
    {
        SocketMessage msg = new SocketMessage((int)msgID, 0, pbMsg);
        byte[] data = CodingTool.EncodingMessage(msg);
        byte[] packet = CodingTool.EncodingPacket(data);
        sendDataCache.Enqueue(packet);
        if (!isProcessSend)
        {
            ProcessingSend();
        }
    }

    /// <summary>
    /// 正在发送数据
    /// </summary>
    private void ProcessingSend()
    {
        isProcessSend = true;
        if (sendDataCache.Count == 0)
        {
            isProcessSend = false;
            return;
        }
        byte[] packet = sendDataCache.Dequeue();
        sendArgs.SetBuffer(packet, 0, packet.Length);
        bool r = clientSocket.SendAsync(sendArgs);
        if (!r)
        {
            SendCompleted();
        }
    }
    /// <summary>
    /// 当异步发送消息完成时
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SendArgs_Completed(object sender, SocketAsyncEventArgs e)
    {
        SendCompleted();
    }
    private void SendCompleted()
    {
        if (sendArgs.SocketError != SocketError.Success)
        {
            Debug.Log("发送失败");
        }
        else
        {
            ProcessingSend();
        }
    }
    #endregion
    #region 接收消息
    public void StartReceive()
    {
        try
        {
            if (!isDisconnected)
            {
                bool r = clientSocket.ReceiveAsync(receiveArgs);
                if (!r)
                {
                    ProcessReceivedData();
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }
    /// <summary>
    /// 当接受数据完成时
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ReceiveArgs_Completed(object sender, SocketAsyncEventArgs e)
    {
        ProcessReceivedData();
    }
    /// <summary>
    /// 接收到数据进行处理
    /// </summary>
    private void ProcessReceivedData()
    {
        if (receiveArgs.SocketError == SocketError.Success && receiveArgs.BytesTransferred > 0)
        {
            byte[] packet = new byte[receiveArgs.BytesTransferred];
            Buffer.BlockCopy(receiveArgs.Buffer, 0, packet, 0,packet.Length);
            ProcessReceivedPacket(packet);
            StartReceive();
        }
        else
        {
            Debug.Log("掉线了");
        }
    }
    /// <summary>
    /// 接收包，将包放入数据缓冲区，进行互斥解析包
    /// </summary>
    /// <param name="packet"></param>
    private void ProcessReceivedPacket(byte[] packet)
    {
        receiveDatacache.AddRange(packet);
        if (!isProcessRec)
        {
            ProcessingPacket();
        }
    }
    /// <summary>
    /// 将包进行解析，并将消息转交给消息管理中心
    /// </summary>
    private void ProcessingPacket()
    {
        isProcessRec = true;
        byte[] data = CodingTool.DecodingPacket(receiveDatacache);
        if (data == null)
        {
            isProcessRec = false;
            return;
        }
        SocketMessage msg = CodingTool.DecodingMessage(data);
        //当接受到心跳包则返回心跳包,不将数据发回上层了；
        //if (msg.opCode == (int)ApplicationProtocol.HEARTPACKET)
        //{
        //    Send((int)ApplicationProtocol.HEARTPACKET, (int)ApplicationProtocol.HEARTPACKETSUB, null);
        //    Debug.Log("发送心跳包成功");
        //}
        if(msg != null)
        {
            SocketMessageManger.Instance.HandOutPBMsg(this, (MSGID)msg.opCode, msg.value);
        }
        ProcessingPacket();
    }
    #endregion
    #region 断开连接
    public void Disconnected()
    {
        receiveDatacache.Clear();
        sendDataCache.Clear();
        isDisconnected = true;
        isProcessRec = false;
        isProcessSend = false;
        //发送断开连接消息
        //Send((int)ApplicationProtocol.OFFLINE,(int)ApplicationProtocol.OFFLINE_SUB,null);
        Thread.Sleep(100);
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }
    #endregion


}
