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
    public Text Tips;

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
    protected override void Start () {
        base.Start();
        InitButtonEvent();
        StartCoroutine(Init());
    }

    public IEnumerator Init()
    {
        InitUI();
        yield return null;
        Search();
    }

    protected override void InitUI()
    {
        base.InitUI();
        Tips.gameObject.SetActive(false);
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
            GameObject go = PopUpManager.Instance.AddUiLayerPopUp(Prefabs.Calendar);
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
        Tips.gameObject.SetActive(false);
        SetResults(TicketsController.Instance.Search((int)trafficType, Src.text, Dst.text, date));
    }

    public void SetResults(List<TrafficMessage> result)
    {
        content.source = result.ToArray();
        if (result.Count==0)
        {
            if(trafficType ==  TrafficType.Plane)
            {
                Tips.text = "暂无航班";
            }
            else
            {
                Tips.text = "暂无列车";
            }
            Tips.gameObject.SetActive(true);
        }

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
        StartCoroutine(Init());
    }

}
