using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;

public class BuyTicketPopupView : BaseSceneEaseInOut
{

    public Button yes;
    public Button no;

    public Text Title;
    public Text StartTime;
    public Text StartLocation;
    public Text Time;
    public Text Number;
    public Text EndTime;
    public Text EndLocation;

    private TrafficMessage trafficMessage;
    public TrafficMessage traffic
    {
        set
        {
            trafficMessage = value;
            InvalidView();
        }
    }

    protected override void InitUI()
    {
        base.InitUI();
        Enter();
    }

    private void InitButtonEvent()
    {
        yes.onClick.AddListener(BuyTickets);
        no.onClick.AddListener(Dispose);
    }

    private void BuyTickets()
    {
        Debug.Log("buy");
        Dispose();
    }

    protected override void UpdateView()
    {
        base.UpdateView();
        if (trafficMessage != null)
            SetData(trafficMessage);
    }

    private void SetData(TrafficMessage data)
    {
        StartTime.text = data.StartTime;
        StartLocation.text = data.StartLocation;
        Time.text = data.Time;
        Number.text = data.Number;
        EndTime.text = data.EndTime;
        EndLocation.text = data.EndLocation;
        if (data.buy)
            Title.text = "购票";
        else
            Title.text = "退票";
        InitButtonEvent();
    }


}
