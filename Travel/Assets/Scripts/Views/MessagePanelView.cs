using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class MessagePanelView : MonoBehaviour {

    public GameObject WeChat;
    public GameObject Accident;

    public AccidentMessageView aview;
    public WeChatMessageView wview;

    public Color Blue;

    private Toggle WeChatToggle;
    private Toggle AccidentToogle;

    private Image WeChatImage;
    private Image AccidentImage;

    private void Awake()
    {
        WeChatToggle = WeChat.GetComponent<Toggle>();
        AccidentToogle = Accident.GetComponent<Toggle>();

        WeChatImage = WeChat.GetComponent<Image>();
        AccidentImage = Accident.GetComponent<Image>();

    }

    // Use this for initialization
    void Start () {
        InitEvent();
        WeChatImage.color = Blue;
        AccidentImage.color = Color.white;
        aview.gameObject.SetActive(false);
        wview.gameObject.SetActive(true);
    }

    private void InitEvent()
    {
        WeChatToggle.onValueChanged.AddListener(delegate(bool isOn)
        {
            if(isOn)
            {
                WeChatImage.color = Blue;
                AccidentImage.color = Color.white;
                aview.gameObject.SetActive(false);
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
                aview.gameObject.SetActive(true);
            }
        });

        List<WeChatMessage> list = new List<WeChatMessage>();
        list.Add(new WeChatMessage("lucky", "asdads", new DateTime(2018, 1, 2)));
        list.Add(new WeChatMessage("lucky", "asdads", new DateTime(2018, 1, 6)));
        list.Add(new WeChatMessage("lucky", "asdads", new DateTime(2018, 1, 5, 23, 58, 0)));
        list.Add(new WeChatMessage("lucky", "asdads", new DateTime(2018, 1, 5, 23, 38, 0)));
        list.Add(new WeChatMessage("lucky", "asdads", new DateTime(2018, 1, 5, 23, 28, 0)));
        list.Add(new WeChatMessage("lucky", "asdads", new DateTime(2018, 1, 5, 22, 28, 0)));
        wview.SetMessages(list);
    }
	
    
}
