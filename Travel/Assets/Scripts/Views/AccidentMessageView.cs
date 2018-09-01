using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;

public class AccidentMessageView : BaseUI {

    public BaseGrid baseGrid;
    private List<WeChatMessage> messages;

    protected override void UpdateView()
    {
        base.UpdateView();
        if (messages != null)
            baseGrid.source = messages.ToArray();
    }

    public void SetMessages(List<WeChatMessage> data)
    {
        messages = data;
        InvalidView();
    }
}
