using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class DeleteTicketsMsg
{
}

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
    public Text ShowTips;

    public Button Search;

    public BaseGrid grid;

    public Sprite Chosen;

    private Toggle TrainToggle;
    private Toggle AirPlaneToggle;

    private Image TrainImage;
    private Image AirPlaneImage;

    private TrafficType type;

    private string DateFormat = "M月d日";

    protected override void Awake()
    {
        type = TrafficType.Train;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        TrainToggle = Train.GetComponent<Toggle>();
        AirPlaneToggle = AirPlane.GetComponent<Toggle>();

        TrainImage = Train.GetComponent<Image>();
        AirPlaneImage = AirPlane.GetComponent<Image>();

        InitText();
        InitEvent();

        ShowTips.gameObject.SetActive(false);
        StartCoroutine(ShowTickets());
    }

    protected override void UpdateView()
    {
        base.UpdateView();
        InitText();
        StartCoroutine(ShowTickets());
    }

    private IEnumerator ShowTickets()
    {
        yield return null;
        MultiYield my = TicketsController.Instance.GetBuyTickets(TimeManager.instance.NowTime);
        yield return my;
        if (my.result != null)
        {
            if(my.result.Count==0)
            {
                ShowTips.gameObject.SetActive(true);
            }
            else
            {
                ShowTips.gameObject.SetActive(false);
            }
            grid.source = my.result.ToArray();

        }
    }

    private void InitText()
    {
        StartText.text = BuyTicketsModel.Instance.startlocation;
        StopText.text = BuyTicketsModel.Instance.stoplocation;
        if(DateText!=null)
            DateText.text = BuyTicketsModel.Instance.date.ToString(DateFormat);

        SetToggle(BuyTicketsModel.Instance.type);
    }

    private void InitEvent()
    {
        back.onClick.AddListener(delegate ()
        {
            mc.ShowView(ViewID.Maps);
            TimeManager.instance.StartTimeManager();
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
            GameObject go = PopUpManager.Instance.AddUiLayerPopUp(Prefabs.LocationPanel);
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
            GameObject go = PopUpManager.Instance.AddUiLayerPopUp(Prefabs.LocationPanel);
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

        if (DateChosen != null)
        {
            DateChosen.onClick.AddListener(delegate ()
            {
                GameObject go = PopUpManager.Instance.AddUiLayerPopUp(Prefabs.Calendar);
                CalendarView cv = go.GetComponent<CalendarView>();
                cv.Date = BuyTicketsModel.Instance.date;
                cv.AddCallback(SetDate);

                PopUpManager.Instance.SetPopupPanelAutoClose(go);
            });
        }
        

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
        if (DateText != null)
            DateText.text = date.ToString(DateFormat);
        BuyTicketsModel.Instance.date = date;
    }

    public void SetToggle(TrafficType type)
    {
        if (type == TrafficType.Plane)
        {
            AirPlaneImage.enabled = true;
            TrainImage.enabled = false;
        }
        else
        {
            TrainImage.enabled = true;
            AirPlaneImage.enabled = false;
        }
        BuyTicketsModel.Instance.type = type;
    }

    protected override void RegisterMsg(bool isOn)
    {
        base.RegisterMsg(isOn);
        if (isOn)
        {
            MessageBus.Register<DeleteTicketsMsg>(onGetNewFrash);
        }
        else
        {
            MessageBus.UnRegister<DeleteTicketsMsg>(onGetNewFrash);
        }
    }

    private bool onGetNewFrash(DeleteTicketsMsg msg)
    {
        InvalidView();
        return false;
    }

}
