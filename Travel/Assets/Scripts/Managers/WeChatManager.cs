﻿using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;
using System;

public class WeChatManager : BaseInstance<WeChatManager> {

    List<WechatDialog> diag = WechatUtil.Instance.GetAllWechatContent().data;
    List<WeChatMessage> WechatList = new List<WeChatMessage>();

    public void WechatContentProcessor()
    {
        WeChatMessage message;
        DateTime time;
        Debug.Log("Diag count: "+diag.Count);
        foreach(WechatDialog item in diag)
        {
            string[] timetemp = item.time.Split(':');
            time = new DateTime(DateTime.Now.Year, 2, 4, int.Parse(timetemp[0]), int.Parse(timetemp[1]), 0);
            Debug.Log("Processing name: "+item.name+" Time "+time.ToShortTimeString());
            for(int i = 0; i < item.content.Count; i++)
            {
                message = new WeChatMessage(item.name, item.content[i], time.AddSeconds(i));
                WechatList.Add(message);
            }
            
        }
    }
    void PushWechatList()
    {
        foreach(WeChatMessage item in WechatList)
        {
            TimeManager.instance.AddWeChat(item, PostWeChat);
        }
    }


    public IEnumerator Init()
    {
        WechatContentProcessor();
        PushWechatList();
        yield return null;
    }

    public void PostWeChat(WeChatMessage data)
    {
        Debug.Log("send we chat " + data.name);
        MessageBus.Post(data);
    }

    
}
