﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lucky;

public class MapsView : BaseUI {

    private MainContent mainContent;

    public Button BuyBtn;
    public Button GoBtn;

    public Text GoBtnText;

    private bool isPlay = false;

    private void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        mainContent = transform.parent.gameObject.GetComponent<MainContent>();
        BuyBtn.onClick.AddListener(OnClick);
        GoBtn.onClick.AddListener(OnGoClick);
        TimeManager.instance.SetMapsView(this);
	}

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void OnClick()
    {
        mainContent.ShowView(ViewID.BuyTickets);
    }

    private void OnGoClick()
    {
        if(!isPlay)
        {
            isPlay = true;
            TimeManager.instance.GoToNextStartTime();
            MapTrafficView.instance.SetAnimatorSpeed();
            GoBtnText.text = "休息";
        }
        else
        {
            isPlay = false;
            GoBtnText.text = "出发";
            TimeManager.instance.TimeSpeed = 1.0f;
        }
    }

    public void ChangeGoButton()
    {
        isPlay = false;
        GoBtnText.text = "出发";
    }



}
