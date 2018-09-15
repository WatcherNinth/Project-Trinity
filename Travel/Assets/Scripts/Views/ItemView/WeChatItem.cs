using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using System;

public class WeChatMessage
{
    public string name;
    public string content;
    public DateTime date;
    public Action<WeChatMessage> callback;

    public WeChatMessage(string tname, string tcontent, DateTime tdate)
    {
        name = tname;
        content = tcontent;
        date = tdate;
        callback = null;
    }
}
