﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Lucky;

public class InfoMessage
{
    public string infos;

    public InfoMessage(string tinfo, string ttitle)
    {
        infos = tinfo;
    }
}

public class InfoView : BaseSceneEaseInOut
{
    public Text info;
    public Button btn;

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
        }

    }

    private void onClick()
    {
        Dispose();
    }

    public static void Show(InfoMessage message)
    {
        GameObject go = PopUpManager.Instance.AddUiLayerPopUp(Prefabs.InfoPanel);
        InfoView iv = go.GetComponent<InfoView>();
        iv.Message = message;
        PopUpManager.Instance.SetPopupPanelAutoClose(go);
    }
}