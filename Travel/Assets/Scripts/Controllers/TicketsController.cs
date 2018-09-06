using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;
using System;

public class TicketsController : BaseInstance<TicketsController>
{
    
    public void BuyTickets(int id)
    {
        Debug.Log("buy "+id);
        TicketsOperaton ticket_operation = new TicketsOperaton();
        ticket_operation.BuyTickets(id);
    }

    public List<TrafficMessage> GetBuyTickets(DateTime dt)
    {
        TicketsOperaton ticket_operation = new TicketsOperaton();
        List<RoutineTicket> all_tickets = ticket_operation.GetUserTickets(new DateTime());

        List<TrafficMessage> data = new List<TrafficMessage>();

        foreach (RoutineTicket rt in all_tickets)
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

            data.Add(new TrafficMessage(starttime.ToString("hh:mm"), start, usetime, ticketname, stoptime.ToString("hh:mm"), stop, money, false, id));
        }

        return data;
    }

    public void DeleteTickets(int id)
    {
        Debug.Log("delete " + id);
        TicketsOperaton ticket_operation = new TicketsOperaton();
        ticket_operation.RefundTicket(id);
    }

    public List<TrafficMessage> Search(int type, string startlocation, string stoplocation, DateTime dt)
    {

        RoutineOperation operation = new RoutineOperation();
        List<RoutineTicket> tickets = operation.GetAllTicket(startlocation, stoplocation, type, dt);

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
