using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;

public class PopMessageView : BaseSceneEaseInOut
{
    public Text Title;
    public Text Content;

    private string title="";
    private string content="";

    protected override void InitUI()
    {
        base.InitUI();
        Enter();
    }

    protected override void UpdateView()
    {
        base.UpdateView();
        SetData();
    }

    private void SetData()
    {
        Title.text = title;
        Content.text = content;
    }
    

    public void SetMessages(string ttitle, string tcontent)
    {
        title = ttitle;
        content = tcontent;
        InvalidView();
    }
}
