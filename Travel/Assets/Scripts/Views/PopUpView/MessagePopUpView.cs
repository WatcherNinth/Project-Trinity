using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lucky;

public class MessageObject
{
    public System.Object data;

    public MessageObject(System.Object tdata)
    {
        data = tdata;
    }
}

public class MessagePopUpView : BaseUI {

    public GameObject prefab;

    private Stack<MessageItem> queue = new Stack<MessageItem>();

    private static MessagePopUpView mpuv;
    public static MessagePopUpView instance
    {
        get { return mpuv; }
    }

    protected override void Awake()
    {
        base.Awake();
        mpuv = this;
    }

    protected override void UpdateView()
    {
        base.UpdateView();
    }

    protected override void RegisterMsg(bool isOn)
    {
        base.RegisterMsg(isOn);
        if(isOn)
        {
            MessageBus.Register<MessageObject>(AddNewMessage);
        }
        else
        {
            MessageBus.UnRegister<MessageObject>(AddNewMessage);
        }
    }

    public bool AddNewMessage(MessageObject itemMessage)
    {
        AudioManager.Instance.PlayMusic(Audios.PopupClip);
        Lucky.LuckyUtils.Log("add new message");
        if(queue.Count!=0)
        {
            MessageItem temp = queue.Peek();
            temp.StopCountDown();
        }
        GameObject showItem = Instantiate(prefab);
        MessageItem mi = showItem.GetComponent<MessageItem>();
        showItem.transform.SetParent(transform);
        mi.data = itemMessage.data;
        mi.Showing = true;
        mi.EnableBg();
        showItem.SetActive(true);
        LuckyUtils.MakeIndentity(showItem.transform);
        queue.Push(mi);
        return false;
    }

    public void DestroyMessage()
    {
        queue.Pop();
        if (queue.Count!=0)
        {
            MessageItem mi = queue.Peek();
            mi.StartCountDown();
        }
        
    }

}
