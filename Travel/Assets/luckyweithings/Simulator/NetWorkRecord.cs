using MiniGameClientProto;
using MyNetwork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetWorkRecord : MonoBehaviour {

    private static NetWorkRecord _instance;

	public static NetWorkRecord instance
    {
        get
        {
            return _instance;
        }
    }

    private FileStream fs;
    private DateTime firstTime;
    private const short PACKAGE_HEADER_SIZE = 2;

    private void Awake()
    {
        Debug.Log("luckyhigh Network Record start");
        _instance = this;
    }

    private void Start()
    {
        firstTime = DateTime.Now;
        fs = FileManager.GetFileWriteStream((FileManager.GetFilePath("Network.rec")));
    }

    private void OnDestroy()
    {
        if (fs != null)
            fs.Close();
    }

    public void WriteBuffer(byte[] buffer, int offset, int length)
    {
        Debug.Log("luckyhigh start to write");
        if (fs == null)
            return;

        Debug.Log("luckyhigh fs is no null");

        int temp = offset - length;
        byte[] tempBuf = new byte[length];
        Array.Copy(buffer, temp, tempBuf, 0, length);
        double TimeOffset = (DateTime.Now-firstTime).TotalMilliseconds;

        NetMsg net = new NetMsg();
        net.offset = TimeOffset;
        net.buffer = new List<byte>(tempBuf);

        byte[] data=ProtoUtil.JceStructToBytes(net);

        byte[] _len = BitConverter.GetBytes((ushort)(data.Length + PACKAGE_HEADER_SIZE));

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(_len);
        }

        byte[] _bufferWithLen = new byte[_len.Length + data.Length];
        _len.CopyTo(_bufferWithLen, 0);
        data.CopyTo(_bufferWithLen, _len.Length);
        
        Debug.Log("luckyhigh offset " + TimeOffset+" data Len "+data.Length+" total len "+_bufferWithLen.Length);

        fs.Write(_bufferWithLen, 0, (int)_bufferWithLen.Length);
        fs.Flush();
    }
}
