﻿using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;
using System;

public class TicketParam
{
    public RoutineTicket rt;
    public TicketParam(RoutineTicket trt)
    {
        rt = trt;
    }
}

public class TicketsController : BaseInstance<TicketsController>
{
    
    public void BuyTickets(int id)
    {
        TicketsOperaton ticket_operation = new TicketsOperaton();
        int ticketid = ticket_operation.BuyTickets(id);
        if(ticketid==0)
        {
            Debug.Log("ticket id error");
            return;
        }
        RoutineTicket ticket=ticket_operation.GetTicketByTickedId(ticketid);
        Debug.Log("ticket " + ticket.GetRoutineStartNode() + " " + ticket.GetBeginTime() + " "+ticket.GetTicketId());
        TimeManager.instance.AddGo(new TicketParam(ticket));
    }

    public List<TrafficMessage> GetBuyTickets(DateTime dt)
    {
        Debug.Log(dt);
        TicketsOperaton ticket_operation = new TicketsOperaton();
        List<RoutineTicket> all_tickets = ticket_operation.GetUserTickets(new DateTime());

        SortedDictionary<DateTime, TrafficMessage> data = new SortedDictionary<DateTime, TrafficMessage>();

        foreach (RoutineTicket rt in all_tickets)
        {
            DateTime starttime = rt.GetBeginTime();
            DateTime stoptime = rt.GetEndTime();

            string start = rt.GetRoutineStartNode();
            string stop = rt.GetEndNode();

            string ticketname = rt.GetTicketName();
            string money = rt.GetMoney() + "";

            int id = rt.GetTicketId();

            TimeSpan ts = stoptime - starttime;
            string usetime = ts.Hours + ":" + ts.Minutes;
            Debug.Log("start time " + starttime);
            Debug.Log("get id " + id);
            Debug.Log(TicketsController.Instance.DeleteTickets(id));
            //data.Add(starttime, new TrafficMessage(starttime.ToString("hh:mm"), start, usetime, ticketname, stoptime.ToString("hh:mm"), stop, money, false, id));
            //TimeManager.instance.AddGo(new TicketParam(rt));
        }

        List<TrafficMessage> finaldata = new List<TrafficMessage>();

        foreach (KeyValuePair<DateTime, TrafficMessage> kvp in data)
        {
            finaldata.Add(kvp.Value);
            
        }

        return finaldata;
    }

    public bool DeleteTickets(int id)
    {
        TicketsOperaton ticket_operation = new TicketsOperaton();
        bool abc =ticket_operation.RefundTicket(id);
        TimeManager.instance.RemoveGo(id);
        return abc;
    }

    public List<TrafficMessage> Search(int type, string startlocation, string stoplocation, DateTime dt)
    {

        RoutineOperation operation = new RoutineOperation();
        List<Routine> tickets = operation.GetAllTicket(startlocation, stoplocation, type, dt);

        List<TrafficMessage> data = new List<TrafficMessage>();

        foreach (RoutineTicket rt in tickets)
        {
            DateTime starttime = rt.GetBeginTime();
            DateTime stoptime = rt.GetEndTime();

            string start = rt.GetRoutineStartNode();
            string stop = rt.GetEndNode();

            string ticketname = rt.GetTicketName();
            string money = rt.GetMoney() + "";

            int id = rt.GetRoutineId();

            TimeSpan ts = stoptime - starttime;
            string usetime = ts.Hours + ":" + ts.Minutes;

            data.Add(new TrafficMessage(starttime.ToString("hh:mm"), start, usetime, ticketname, stoptime.ToString("hh:mm"), stop, money, true, id));
        }
        return data;
    }

}
