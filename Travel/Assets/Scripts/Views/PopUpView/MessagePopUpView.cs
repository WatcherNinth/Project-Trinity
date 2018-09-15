using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lucky;

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
            MessageBus.Register<ItemMessage>(AddNewMessage);
        }
        else
        {
            MessageBus.UnRegister<ItemMessage>(AddNewMessage);
        }
    }

    public bool AddNewMessage(ItemMessage itemMessage)
    {
        Debug.Log("add new message");
        if(queue.Count!=0)
        {
            MessageItem temp = queue.Peek();
            temp.StopCountDown();
        }
        GameObject showItem = Instantiate(prefab);
        MessageItem mi = showItem.GetComponent<MessageItem>();
        showItem.transform.SetParent(transform);
        mi.data = itemMessage;
        showItem.SetActive(true);
        LuckyUtils.MakeIndentity(showItem.transform);
        queue.Push(mi);
        mi.ShowItem();
        return false;
    }

    public void DestroyMessage()
    {
        MessageItem mi = queue.Pop();
        mi = queue.Peek();
        mi.StartCountDown();
    }

}
