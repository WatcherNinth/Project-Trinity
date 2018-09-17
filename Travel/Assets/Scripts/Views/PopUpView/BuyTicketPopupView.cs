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
        StartCoroutine(BuyTickets(trafficMessage.id));
    }

    private IEnumerator BuyTickets(int id)
    {
        MultiYield my=TicketsController.Instance.BuyTickets(trafficMessage.id);
        yield return my;
        if(my.result!=null)
        {
            float money = Convert.ToSingle(trafficMessage.Money);
            MessageBus.Post(new UseMoney(-money));
            string traffic = "";
            if (trafficMessage.trafficType == TrafficType.Train)
                traffic = "列车";
            else
                traffic = "航班";
            string content = "尊敬的旅客，您已购买" + trafficMessage.StartTime + "出发的" + trafficMessage.Number + traffic + "，祝您旅途愉快";
            MessageBus.Post(new MessageObject(new ItemMessage("12306铁路管家", content)));
            
        }
        Dispose();
    }

    private void DeleteTickets()
    {
        StartCoroutine(DeleteTickets(trafficMessage.id));
    }

    private IEnumerator DeleteTickets(int id)
    {
        MultiYield my = TicketsController.Instance.DeleteTickets(trafficMessage.id);
        yield return my;
        if(my.result!=null)
        {
            float money = Convert.ToSingle(trafficMessage.Money);
            MessageBus.Post(new UseMoney(money));
            MessageBus.Post(new DeleteTicketsMsg());
            string traffic = "";
            if (trafficMessage.trafficType == TrafficType.Train)
                traffic = "列车";
            else
                traffic = "航班";
            string content = "尊敬的旅客，您已成功退订" + trafficMessage.StartTime + "出发的" + trafficMessage.Number + traffic;
            MessageBus.Post(new MessageObject(new ItemMessage("12306铁路管家", content)));
        }
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
