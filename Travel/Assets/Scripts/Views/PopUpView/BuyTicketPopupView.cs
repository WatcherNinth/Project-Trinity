using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using System;

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

    private void BuyTickets()
    {
        Debug.Log("buy ticket popup ticked id" + trafficMessage.id);
        float money = Convert.ToSingle(trafficMessage.Money);
        TicketsController.Instance.BuyTickets(trafficMessage.id);
        MessageBus.Post(new UseMoney(-money));
        Dispose();
    }

    private void DeleteTickets()
    {
        float money = Convert.ToSingle(trafficMessage.Money);
        TicketsController.Instance.DeleteTickets(trafficMessage.id);
        
        MessageBus.Post(new UseMoney(money));
        MessageBus.Post(new DeleteTicketsMsg());
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
        {
            Title.text = "购票";
            yes.onClick.AddListener(BuyTickets);
        }
        else
        {
            Title.text = "退票";
            yes.onClick.AddListener(DeleteTickets);
        }
        
        no.onClick.AddListener(Dispose);
    }


}
