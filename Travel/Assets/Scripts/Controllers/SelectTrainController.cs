using UnityEngine;
using System.Collections;
using Lucky;
using System;
using System.Collections.Generic;

public class SelectTrainController : BaseInstance<SelectTrainController> {

    public SelectTrainView selectTrainView;

    public void SetView(SelectTrainView view)
    {
        selectTrainView = view;
    }

    public void Search(int type, string startlocation, string stoplocation, DateTime dt)
    {
        Debug.Log("type " + type);
        Debug.Log("start " + startlocation);
        Debug.Log("stop " + stoplocation);
        Debug.Log("datetime " + dt);

        RoutineOperation operation = new RoutineOperation();
        List<RoutineTicket> tickets = operation.GetAllTicket(startlocation, stoplocation, type, dt);

        List<TrafficMessage> data = new List<TrafficMessage>();

        foreach(RoutineTicket rt in tickets)
        {
            DateTime starttime = rt.GetBeginTime();
            DateTime stoptime = rt.GetEndTime();

            string start = rt.GetRoutineStartNode();
            string stop = rt.GetEndNode();

            string ticketname = rt.GetTicketName();
            string money = rt.GetMoney() + "";

            TimeSpan ts = stoptime - starttime;
            string usetime = ts.Hours + ":" + ts.Minutes;

            data.Add(new TrafficMessage(starttime.ToString("hh:mm"), start, "", ticketname, stoptime.ToString("hh:mm"), stop, money, true));
        }

        //data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007", true));
        //data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007", true));
        selectTrainView.SetResults(data);
    }
}
