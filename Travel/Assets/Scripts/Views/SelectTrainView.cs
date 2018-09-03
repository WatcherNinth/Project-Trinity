using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public enum TrafficType
{
    Train = 0,
    Plane = 1
}

public class SelectTrainView : BaseUI {


    public Text Src;
    public Text Dst;
    public Text GoDate;

    public Button back;
    public Button yesterday;
    public Button tomorrow;
    public Button BtnGoData;
    public BaseGrid content;

    public Toggle Train;
    public Image TrainImage;
    public Toggle Airplane;
    public Image AirImage;

    private TrafficType trafficType;
    private DateTime date;
    private string DateFormat = "M月d日";

    private void Awake()
    {
        trafficType = TrafficType.Train;
        base.Awake();
    }

    // Use this for initialization
    void Start () {
        InitButtonEvent();

        List<TrafficMessage> data = new List<TrafficMessage>();
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007", true));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007", true));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007", true));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G450", "08:10", "广州", "1007", true));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007", true));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "18:10", "广州", "1007", true));

        content.source = data.ToArray();

        InitUI();

    }

    private void InitUI()
    {
        date = BuyTicketsModel.Instance.date;
        GoDate.text = date.ToString(DateFormat);
        Src.text = BuyTicketsModel.Instance.startlocation;
        Dst.text = BuyTicketsModel.Instance.stoplocation;
        trafficType = BuyTicketsModel.Instance.type;
        SetToggle(trafficType);
    }

    public void InitButtonEvent()
    {
        back.onClick.AddListener( delegate()
        {
            mc.ShowView(ViewID.BuyTickets);
        });

        yesterday.onClick.AddListener(delegate ()
        {
            SetDate(date.AddDays(-1));
            Search();
        });

        tomorrow.onClick.AddListener(delegate ()
        {
            SetDate(date.AddDays(1));
            Search();
        });

        BtnGoData.onClick.AddListener(delegate ()
        {
            GameObject go = PopUpManager.Instance.AddUiLayerPopUp("Prefabs/Calendar");
            CalendarView cv = go.GetComponent<CalendarView>();
            cv.Date = date;
            cv.AddCallback(SetDate);
            PopUpManager.Instance.SetPopupPanelAutoClose(go);
        });

        Train.onValueChanged.AddListener(delegate (bool isOn)
        {
            if(isOn)
            {
                trafficType = TrafficType.Train;
                SetToggle(trafficType);
                Search();
            }
        });

        Airplane.onValueChanged.AddListener(delegate (bool isOn)
        {
            if(isOn)
            {
                trafficType = TrafficType.Plane;
                SetToggle(trafficType);
                Search();
            }
        });
    }

    private void Search()
    {
        List<TrafficMessage> data = new List<TrafficMessage>();
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007", true));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007", true));
        content.source = data.ToArray();
    }

    public void SetDate(DateTime tdate)
    {
        date = tdate;
        GoDate.text = date.ToString(DateFormat);
        BuyTicketsModel.Instance.date = date;
    }

    public void SetToggle(TrafficType type)
    {
        if(type == TrafficType.Plane)
        {
            TrainImage.gameObject.SetActive(false);
            AirImage.gameObject.SetActive(true);
        }
        else
        {
            TrainImage.gameObject.SetActive(true);
            AirImage.gameObject.SetActive(false);
        }
        BuyTicketsModel.Instance.type = type;
    }

    protected override void UpdateView()
    {
        base.UpdateView();
        InitUI();
    }

}
