using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Lucky;

public class InfoView : BaseSceneEaseInOut
{
    public Text info;

    private string message;
    public string Message
    {
        set
        {
            message = value;
            InvalidView();
        }
    }

    protected override void InitUI()
    {
        base.InitUI();
        Enter();
    }

    protected override void UpdateView()
    {
        base.UpdateView();
        if (!string.IsNullOrEmpty(message))
            info.text = message;
    }
}
