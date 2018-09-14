    using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using System;

public class TrafficMessage
{
    public string StartTime = "";
    public string StartLocation = "";
    public string Time = "";
    public string Number = "";
    public string EndTime = "";
    public string EndLocation = "";
    public string Money = "";
    public bool buy = true;
    public bool isDelay = false;
    public TrafficType trafficType = TrafficType.Train;

    public int id = 0;

    public TrafficMessage(string st,string sl,string t,string n,string et,string el,string m, bool b,bool d, int i, TrafficType type)
    {
        StartTime = st;
        StartLocation = sl;
        Time = t;
        Number = n;
        EndTime = et;
        EndLocation = el;
        Money = m;
        buy = b;
        isDelay = d;
        id = i;
        trafficType = type;
    }

    public TrafficMessage()
    {

    }
}

public class TrainItemView : ItemRender {

    public Text StartTime;
    public Text StartLocation;
    public Text Time;
    public Text Number;
    public Text EndTime;
    public Text EndLocation;
    public Text Money;
    public Image Delay;
    public Image Type;

    public Button btn;

    public int id;

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateView()
    {
        if(m_Data!=null)
        {
            TrafficMessage tdata = m_Data as TrafficMessage;
            SetData(tdata);
        }
        base.UpdateView();
    }

    public void SetData(TrafficMessage data)
    {
        StartTime.text = data.StartTime;
        StartLocation.text = data.StartLocation;
        Time.text = data.Time;
        Number.text = data.Number;
        EndTime.text = data.EndTime;
        EndLocation.text = data.EndLocation;
        Money.text = "￥"+data.Money;
        id = data.id;
        Delay.gameObject.SetActive(data.isDelay);

        if (!data.buy)
        {
            Type.gameObject.SetActive(true);
            if (data.trafficType == TrafficType.Plane)
                Type.sprite = SpriteManager.Instance.GetSprite(Sprites.ticket_airplane);
            else
                Type.sprite = SpriteManager.Instance.GetSprite(Sprites.ticket_train);
        }
        else
            Type.gameObject.SetActive(false);

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(Popup);
    }

    public void Popup()
    {
        TrafficMessage tdata = m_Data as TrafficMessage;
        if(tdata.buy)
        {
            if (Convert.ToSingle(tdata.Money) <= UserTicketsModel.Instance.money)
            {
                if (TimeManager.instance.isTicketBuy(tdata.id))
                {
                    InfoView.Show(new InfoMessage("你已经购买了这张车票", "消息"));
                }
                else
                {
                    GameObject go = PopUpManager.Instance.AddUiLayerPopUp(Prefabs.BuyTicketPopup);
                    BuyTicketPopupView btpv = go.GetComponent<BuyTicketPopupView>();
                    btpv.traffic = m_Data as TrafficMessage;
                    PopUpManager.Instance.SetPopupPanelAutoClose(go);
                }
            }
            else
            {
                InfoView.Show(new InfoMessage("金额不足", "消息"));
            }
        }
        else
        {
            GameObject go = PopUpManager.Instance.AddUiLayerPopUp(Prefabs.BuyTicketPopup);
            BuyTicketPopupView btpv = go.GetComponent<BuyTicketPopupView>();
            btpv.traffic = m_Data as TrafficMessage;
            PopUpManager.Instance.SetPopupPanelAutoClose(go);
        }
        
    }
}
