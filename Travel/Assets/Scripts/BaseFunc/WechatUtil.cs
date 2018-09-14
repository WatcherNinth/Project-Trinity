using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Lucky;
using Mono.Data.Sqlite;

public class WechatContent
{
    public List<WechatDialog> data;
}


[Serializable]
public class WechatDialog
{
    public string name;
    public string time;
    public List<string> content;

}
public class WechatUtil : BaseInstance<WechatUtil>
{
    private string content = "";

    public WechatUtil()
    {
        TextAsset text = Resources.Load<TextAsset>("Data/wechat");
        content = text.text;

    }

    public WechatContent GetAllWechatContent()
    {
        WechatContent res = JsonUtility.FromJson<WechatContent>(content);
        return res;
    }

}

