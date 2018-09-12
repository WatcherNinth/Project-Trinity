using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;

public class NewsMessageView : BaseUI {

    public BaseGrid baseGrid;
    private List<NewMessage> messages;

    protected override void UpdateView()
    {
        base.UpdateView();
        if (messages != null)
            baseGrid.source = messages.ToArray();
    }

    public void SetMessages(List<NewMessage> data)
    {
        messages = data;
        InvalidView();
    }
}
