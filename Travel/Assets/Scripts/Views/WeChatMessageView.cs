using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;

public class WeChatMessageView : BaseUI
{
    public BaseGrid baseGrid;
    private List<WeChatMessage> messages;

    protected override void UpdateView()
    {
        base.UpdateView();
        if (messages != null)
            baseGrid.source = messages.ToArray();
    }

    public void SetMessages(List<WeChatMessage> datas)
    {
        foreach (WeChatMessage data in datas)
        {
            data.callback = Callback;
        }
        messages = datas;
        messages.Reverse();
        InvalidView();
    }

    public void Callback(WeChatMessage newMessage)
    {
        Debug.Log("enter callback");
        InfoView.Show(new InfoMessage(newMessage.content, newMessage.name));
    }
}
