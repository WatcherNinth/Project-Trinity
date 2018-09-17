using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Lucky;

public class InfoMessage
{
    public string infos;
    public string titles;

    public InfoMessage(string tinfo, string ttitle)
    {
        infos = tinfo;
        titles = ttitle;
    }
}

public class InfoView : BaseSceneEaseInOut
{
    public Text info;
    public Text title;
    public Button btn;
    public Image Bg;

    private InfoMessage message = null;
    public InfoMessage Message
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
        btn.onClick.AddListener(onClick);
    }

    protected override void UpdateView()
    {
        base.UpdateView();
        if (message != null )
        {
            info.text = message.infos;
            title.text = message.titles;
        }

    }

    private void onClick()
    {
        Dispose();
    }

    public void SetBg(string img)
    {
        Bg.sprite = SpriteManager.Instance.GetSprite(img);
    }

    public static void Show(InfoMessage message)
    {
        GameObject go = PopUpManager.Instance.AddPopLayerPopUp(Prefabs.InfoPanel);
        InfoView iv = go.GetComponent<InfoView>();
        iv.Message = message;
        PopUpManager.Instance.SetPopupPanelAutoClose(go);
    }
}
