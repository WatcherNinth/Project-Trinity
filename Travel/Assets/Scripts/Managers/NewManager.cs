using UnityEngine;
using System.Collections;
using Lucky;

public class NewManager : BaseInstance<NewManager> {

	public IEnumerator Init()
    {
        yield return null;
    }

    public void PostNew(WeChatMessage data)
    {
        MessageBus.Post(data);
    }
}
