using UnityEngine;
using System.Collections;
using Lucky;

public class WeChatManager : BaseInstance<WeChatManager> {

	public IEnumerator Init()
    {
        WeChatMessage m = new WeChatMessage("abc", "hhhh", GameModel.Instance.Start.AddMinutes(10));
        TimeManager.instance.AddWeChat(m, PostWeChat);  
        yield return null;
    }

    public void PostWeChat(WeChatMessage data)
    {
        Debug.Log("send we chat " + data.name);
        MessageBus.Post(data);
    }

    
}
