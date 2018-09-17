using UnityEngine;
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
        int timeModifier;
        Lucky.LuckyUtils.Log("Diag count: "+diag.Count);
        foreach(WechatDialog item in diag)
        {
            string[] timetemp = item.time.Split(':');
            time = new DateTime(DateTime.Now.Year, 2, 4, int.Parse(timetemp[0]), int.Parse(timetemp[1]), 30);
            timeModifier = 0;
            Lucky.LuckyUtils.Log("Processing name: "+item.name+" Time "+time.ToShortTimeString());
            for(int i = 0; i < item.content.Count; i++)
            {
                message = new WeChatMessage(item.name, item.content[i], time.AddSeconds(timeModifier));
                timeModifier += item.content[i].Length / 2;
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
        yield return null;
        PushWechatList();
    }

    public void PostWeChat(WeChatMessage data)
    {
        Lucky.LuckyUtils.Log("send we chat " + data.name);
        MessageBus.Post(data);
    }

    
}
