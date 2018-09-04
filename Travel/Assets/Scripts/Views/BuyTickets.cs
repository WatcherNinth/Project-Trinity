﻿using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class BuyTickets : BaseUI {

    public Button back;
    public GameObject Train;
    public GameObject AirPlane;

    public Button StartButton;
    public Button StopButton;
    public Text StartText;
    public Text StopText;

    public Button Switch;

    public Button DateChosen;
    public Text DateText;

    public Toggle HighWaysToggle;
    public Button Search;

    public BaseGrid grid;

    public Color Blue = Color.blue;

    private Toggle TrainToggle;
    private Toggle AirPlaneToggle;

    private Image TrainImage;
    private Image AirPlaneImage;

    private TrafficType type;

    private string DateFormat = "M月d日";

    private void Awake()
    {
        type = TrafficType.Train;
        base.Awake();
    }

    private void Start()
    {
        TrainToggle = Train.GetComponent<Toggle>();
        AirPlaneToggle = AirPlane.GetComponent<Toggle>();

        TrainImage = Train.GetComponent<Image>();
        AirPlaneImage = AirPlane.GetComponent<Image>();

        InitText();
        InitEvent();

        StartCoroutine(ShowTickets());
    }

    protected override void UpdateView()
    {
        base.UpdateView();
        InitText();
    }

    private IEnumerator ShowTickets()
    {
        yield return null;
        List<TrafficMessage> data = new List<TrafficMessage>();
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007", false));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007", false));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007", false));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G450", "08:10", "广州", "1007", false));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007", false));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "18:10", "广州", "1007", false));

        grid.source = data.ToArray();
    }

    private void InitText()
    {
        StartText.text = BuyTicketsModel.Instance.startlocation;
        StopText.text = BuyTicketsModel.Instance.stoplocation;
        DateText.text = BuyTicketsModel.Instance.date.ToString(DateFormat);

        SetToggle(BuyTicketsModel.Instance.type);
    }

    private void InitEvent()
    {
        back.onClick.AddListener(delegate ()
        {
            mc.ShowView(ViewID.Maps);
        });

        TrainToggle.onValueChanged.AddListener(delegate(bool isOn) 
        {
            if(isOn)
            {
                SetToggle(TrafficType.Train);
            }
        });

        AirPlaneToggle.onValueChanged.AddListener(delegate (bool isOn)
        {
            if (isOn)
            {
                SetToggle(TrafficType.Plane);
            }
        });

        StartButton.onClick.AddListener(delegate()
        {
            GameObject go = PopUpManager.Instance.AddUiLayerPopUp("Prefabs/LocationPanel");
            LocationView lv = go.GetComponent<LocationView>();
            lv.SetCallback(delegate(string city)
            {
                SetStartText(city);
                lv.Dispose();
            }, StartText.text, StopText.text);
            PopUpManager.Instance.SetPopupPanelAutoClose(go);
        });

        StopButton.onClick.AddListener(delegate ()
        {
            GameObject go = PopUpManager.Instance.AddUiLayerPopUp("Prefabs/LocationPanel");
            LocationView lv = go.GetComponent<LocationView>();
            lv.SetCallback(delegate (string city)
            {
                SetStopText(city);
                lv.Dispose();
            }, StopText.text, StartText.text);
            PopUpManager.Instance.SetPopupPanelAutoClose(go);
        });

        Switch.onClick.AddListener(delegate ()
        {
            string temp = StartText.text;
            SetStartText(StopText.text);
            SetStopText(temp);
        });

        DateChosen.onClick.AddListener(delegate ()
        {
            GameObject go = PopUpManager.Instance.AddUiLayerPopUp("Prefabs/Calendar");
            CalendarView cv = go.GetComponent<CalendarView>();
            cv.Date = BuyTicketsModel.Instance.date;
            cv.AddCallback(SetDate);
            
            PopUpManager.Instance.SetPopupPanelAutoClose(go);
        });

        Search.onClick.AddListener(delegate ()
        {
            Searching();
        });
    }

    private void Searching()
    {
        if (mc != null)
            mc.ShowView(ViewID.SelectTrain);
    }

    public void SetStartText(string s)
    {
        StartText.text = s;
        BuyTicketsModel.Instance.startlocation = s;
    }

    public void SetStopText(string s)
    {
        StopText.text = s;
        BuyTicketsModel.Instance.stoplocation = s;
    }

    public void SetDate(DateTime date)
    {
        DateText.text = date.ToString(DateFormat);
        BuyTicketsModel.Instance.date = date;
    }

    public void SetToggle(TrafficType type)
    {
        if (type == TrafficType.Plane)
        {
            AirPlaneImage.color = Blue;
            TrainImage.color = Color.white;
        }
        else
        {
            TrainImage.color = Blue;
            AirPlaneImage.color = Color.white;
        }
        BuyTicketsModel.Instance.type = type;
    }

}
