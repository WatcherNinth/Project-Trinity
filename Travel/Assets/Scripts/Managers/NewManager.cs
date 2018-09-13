using UnityEngine;
using System.Collections;
using Lucky;

public class NewManager : BaseInstance<NewManager> {

	public IEnumerator Init()
    {
        NewMessage m = new NewMessage("abc", "fuck fuck", GameModel.Instance.Start.AddMinutes(10));
        TimeManager.instance.AddNews(m, PostNew);
        yield return null;
    }

    public void PostNew(NewMessage data)
    {
        MessageBus.Post(data);
    }
}
