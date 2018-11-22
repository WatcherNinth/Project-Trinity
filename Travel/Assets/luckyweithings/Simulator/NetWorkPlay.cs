using MiniGameClientProto;
using MyNetwork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class NetWorkPlay : MonoBehaviour {

#if UNITY_EDITOR

    private static NetWorkPlay _instance;
    
    public static NetWorkPlay instance
    {
        get
        {
            return _instance;
        }
    }

    private DateTime firstTime;
    private FileStream fs;
    NetworkStream stream;

    private bool m_terminateFlag = false;
    private System.Object m_terminateFlagMutex;

    const uint MaxPacketSize = 1024 * 512;

    const short PACKAGE_HEADER_SIZE = 2;

    private byte[] m_recBuf;
    private int m_recBufOffset;
    private bool destroy = false;

    private void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start () {
        Debug.Log("lucky start play network");
        firstTime = DateTime.Now;
        fs = FileManager.GetFileReadStream((FileManager.GetFilePath("Network.rec")));
        if (fs == null)
            return;

        ThreadStart ts = new ThreadStart(Listen);
        Thread t = new Thread(ts);
        t.Start();
	}

    private void OnDestroy()
    {
        destroy = true;
        SetTerminateFlag();
    }

    private void Read()
    {
        while(!IsTerminateFlagSet())
        {

            if(stream.CanRead)
            {
                byte[] buffer = new byte[2 * MaxPacketSize];
                int len = stream.Read(buffer, 0, buffer.Length);
                Debug.Log("lucky read read "+len);
            }
            else
            {
                Thread.Sleep(16);
            }
        }
        
    }

    public void Listen()
    {
        Debug.Log("lucky start listen");
        m_recBuf = new byte[2 * MaxPacketSize];
        m_recBufOffset = 0;

        TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
        //开启监听
        listener.Start();
        //接收客户端链接
        TcpClient remoteClient = listener.AcceptTcpClient();

        //获取流
        //while(!destroy)
        //{
            Debug.Log("lucky show");
            stream = remoteClient.GetStream();

            //ThreadStart ts = new ThreadStart(Read);
            //Thread t = new Thread(ts);
            //t.Start();

            while (!IsTerminateFlagSet())
            {
                ReadFromStream();
                ScanPackets();
            }
        //}
        

        fs.Close();
    }

    public string ReturnIp()
    {
        Debug.Log("lucky get ip");
        return "127.0.0.1";
    }

    protected void ReadFromStream()
    {
        if (fs.CanRead)
        {
            try
            {
                //This method reads data into the buffer parameter and returns the number of bytes successfully read. 
                //If no data is available for reading, the Read method blocks until data becomes available or the connection is closed. 
                //The Read operation reads as much data as is available, up to the number of bytes specified by the size parameter. 
                //If the remote host shuts down the connection, and all available data has been received, the Read method completes immediately and return zero bytes.
                int length = fs.Read(m_recBuf, m_recBufOffset, m_recBuf.Length - m_recBufOffset);
                m_recBufOffset += length;

                //超过一分钟，服务器没有收到client的包，对方关闭了连接，read 返回0
                //关闭连接后，这个receive也就没有用了, 新的连接会用new receiver, 所以offset不用清
                if (length == 0)
                {
                    Debug.Log("lucky length 0");
                    SetTerminateFlag();
                }
            }
            catch (System.Exception ex)
            {
                SetTerminateFlag();
            }
        }
        else
        {
            Thread.Sleep(16);
        }
    }

    protected void ScanPackets()
    {
        while (m_recBufOffset > PACKAGE_HEADER_SIZE && !IsTerminateFlagSet())
        {
            ushort pkgSize = 0;
            if (BitConverter.IsLittleEndian)
            {
                pkgSize = BitConverter.ToUInt16(new byte[] { m_recBuf[1], m_recBuf[0] }, 0);
            }
            else
            {
                pkgSize = BitConverter.ToUInt16(new byte[] { m_recBuf[0], m_recBuf[1] }, 0);
            }
            Debug.Log("lucky pkg Size " + pkgSize+" "+m_recBufOffset);
            if (pkgSize <= m_recBufOffset)
            {
                byte[] _buffer = new byte[pkgSize - PACKAGE_HEADER_SIZE];
                Array.Copy(m_recBuf, PACKAGE_HEADER_SIZE, _buffer, 0, _buffer.Length);

                NetMsg tPackage = new NetMsg();
                
                ProtoUtil.BytesToJceStruct(_buffer, tPackage);

                DateTime next = firstTime.AddMilliseconds(tPackage.offset);

                Debug.Log("lucky tpackage time " + tPackage.offset);
                double sleeptime = (next - DateTime.Now).TotalMilliseconds;

                byte[] sendData = tPackage.buffer.ToArray();

                if (sleeptime<0)
                {
                    stream.Write(sendData, 0, sendData.Length);
                }
                else
                {
                    Thread.Sleep((int)sleeptime);
                    stream.Write(sendData, 0, sendData.Length);
                }

                if (m_recBufOffset > pkgSize)
                {
                    for (int i = pkgSize, j = 0; i < m_recBufOffset; i++, j++)
                    {
                        m_recBuf[j] = m_recBuf[i];
                    }
                    m_recBufOffset -= pkgSize;
                }
                else
                    m_recBufOffset = 0;
            }
            else
            {
                break;
            }
        }
    }

    public void SetTerminateFlag()
    {
        //lock (m_terminateFlagMutex)
        {
            m_terminateFlag = true;
        }
    }

    public bool IsTerminateFlagSet()
    {
        //lock (m_terminateFlagMutex)
        {
            return m_terminateFlag;
        }
    }

#endif

}
