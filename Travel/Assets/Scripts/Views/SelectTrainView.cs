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

    // Use this for initialization
    void Start () {
        InitButtonEvent();

        List<TrafficMessage> data = new List<TrafficMessage>();
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007"));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007"));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007"));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007"));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007"));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007"));

        content.source = data.ToArray();

        date = GameModel.Instance.Start;
        GoDate.text = date.ToString(DateFormat);


    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void InitButtonEvent()
    {
        back.onClick.AddListener( delegate()
        {
            
        });

        yesterday.onClick.AddListener(delegate ()
        {
            date = date.AddDays(-1);
            GoDate.text = date.ToString(DateFormat);
            Search();
        });

        tomorrow.onClick.AddListener(delegate ()
        {
            date = date.AddDays(1);
            GoDate.text = date.ToString(DateFormat);
            Search();
        });

        BtnGoData.onClick.AddListener(delegate ()
        {
            GameObject go = PopUpManager.Instance.AddUiLayerPopUp("Prefabs/Calendar");
            go.GetComponent<CalendarView>().Date = date;
            PopUpManager.Instance.SetPopupPanelAutoClose(go);
        });

        Train.onValueChanged.AddListener(delegate (bool isOn)
        {
            if(isOn)
            {
                trafficType = TrafficType.Train;
                TrainImage.gameObject.SetActive(true);
                AirImage.gameObject.SetActive(false);
                Search();
            }
        });

        Airplane.onValueChanged.AddListener(delegate (bool isOn)
        {
            if(isOn)
            {
                trafficType = TrafficType.Plane;
                TrainImage.gameObject.SetActive(false);
                AirImage.gameObject.SetActive(true);
                Search();
            }
        });
    }

    private void Search()
    {
        List<TrafficMessage> data = new List<TrafficMessage>();
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007"));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007"));
        content.source = data.ToArray();
    }
}
