using UnityEngine;
using System.Collections;
using Lucky;

public class WeChatManager : BaseInstance<WeChatManager> {

	public IEnumerator Init()
    {
        //TimeManager.instance.   
        yield return null;
    }

    public void PostWeChat(WeChatMessage data)
    {
        MessageBus.Post(data);
    }

    
}
