using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;

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

    public int id = 0;

    public TrafficMessage(string st,string sl,string t,string n,string et,string el,string m, bool b, int i)
    {
        StartTime = st;
        StartLocation = sl;
        Time = t;
        Number = n;
        EndTime = et;
        EndLocation = el;
        Money = m;
        buy = b;
        id = i;
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

    public Button btn;

    public int id;

    private void Start()
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
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(Popup);
    }

    public void Popup()
    {
        GameObject go = PopUpManager.Instance.AddUiLayerPopUp("Prefabs/BuyTicketPopup");
        BuyTicketPopupView btpv = go.GetComponent<BuyTicketPopupView>();
        btpv.traffic = m_Data as TrafficMessage;
        PopUpManager.Instance.SetPopupPanelAutoClose(go);
    }
}
