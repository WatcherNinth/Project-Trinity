using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Lucky;

public class MessagePanelView : BaseUI {

    public GameObject WeChat;
    public GameObject Accident;

    public NewsMessageView nview;
    public WeChatMessageView wview;

    public Color Blue;

    private Toggle WeChatToggle;
    private Toggle AccidentToogle;

    private Image WeChatImage;
    private Image AccidentImage;

    protected override void Awake()
    {
        base.Awake();
        WeChatToggle = WeChat.GetComponent<Toggle>();
        AccidentToogle = Accident.GetComponent<Toggle>();

        WeChatImage = WeChat.GetComponent<Image>();
        AccidentImage = Accident.GetComponent<Image>();
    }

    private void OnEnable()
    {
        InvalidView();
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
        InitEvent();
        WeChatImage.color = Blue;
        AccidentImage.color = Color.white;
        Debug.Log("new list "+ MessageModel.Instance.NewsList.Count);
        nview.gameObject.SetActive(false);
        wview.gameObject.SetActive(true);
        nview.SetMessages(MessageModel.Instance.NewsList);
        wview.SetMessages(MessageModel.Instance.WeChatList);


    }

    protected override void UpdateView()
    {
        base.UpdateView();
        nview.SetMessages(MessageModel.Instance.NewsList);
        wview.SetMessages(MessageModel.Instance.WeChatList);
    }

    private void InitEvent()
    {
        WeChatToggle.onValueChanged.AddListener(delegate(bool isOn)
        {
            if(isOn)
            {
                WeChatImage.color = Blue;
                AccidentImage.color = Color.white;
                nview.gameObject.SetActive(false);
                wview.gameObject.SetActive(true);
            }
        });

        AccidentToogle.onValueChanged.AddListener(delegate(bool isOn) 
        {
            if(isOn)
            {
                AccidentImage.color = Blue;
                WeChatImage.color = Color.white;
                wview.gameObject.SetActive(false);
                nview.gameObject.SetActive(true);
            }
        });
    }


}
