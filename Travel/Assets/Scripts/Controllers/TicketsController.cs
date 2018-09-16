using UnityEngine;
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

public class SearchParam
{
    public int type;
    public string startlocation;
    public string stoplocation;
    public DateTime dt;

    public SearchParam(int ttype, string tstartlocation, string tstoplocation, DateTime tdt)
    {
        type = ttype;
        startlocation = tstartlocation;
        stoplocation = tstoplocation;
        dt = tdt;
    }
}

public class TicketsController : BaseInstance<TicketsController>
{
    private bool isFirstLoad=false;
    
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
        Debug.Log("buy ticket id " + ticket.GetTicketId() + " routtine id" + ticket.GetRoutineId());
        
    }

    public MultiYield GetBuyTickets(DateTime dt)
    {
        return MultiThreadPool.AddNewMission(dt, TicketsController.Instance.GetBuyTickets);
    }

    public List<TrafficMessage> GetBuyTickets(System.Object tdt)
    {
        DateTime dt = (DateTime)tdt;
        Debug.Log("abc "+dt);
        TicketsOperaton ticket_operation = new TicketsOperaton();
        List<RoutineTicket> all_tickets = ticket_operation.GetUserTickets(dt);

        List<TrafficMessage> finaldata = new List<TrafficMessage>();

        foreach (RoutineTicket rt in all_tickets)
        {
            DateTime starttime = rt.GetActualBeginTime();
            DateTime stoptime = rt.GetAcutalEndTime();

            string start = rt.GetRoutineStartNode();
            string stop = rt.GetEndNode();

            string ticketname = rt.GetTicketName();
            string money = rt.GetMoney() + "";

            int id = rt.GetTicketId();

            TimeSpan ts = stoptime - starttime;
            string usetime="";
            if (ts.Hours < 10)
                usetime += "0";
            usetime += ts.Hours + ":";
            if (ts.Minutes < 10)
                usetime += "0";
            usetime += ts.Minutes;

            Debug.Log("start time " + starttime);
            Debug.Log("get ticked id " + id);
            Debug.Log("get routined id  " + rt.GetRoutineId());
            
            /*
            // 第一次开启App，将没有加载的数据放入TimeManager
            if (!isFirstLoad)
            {
                TimeManager.instance.AddGo(new TicketParam(rt));
                isFirstLoad = true;
            }
            */

            Debug.Log("start time " + starttime + " plan start time " + rt.GetBeginTime());
            Debug.Log("stop time " + stoptime + " plan stop time " + rt.GetEndTime());
            if ( (DateTime.Compare(starttime, rt.GetBeginTime())==0) && (DateTime.Compare(stoptime, rt.GetEndTime())==0) )
                finaldata.Add(new TrafficMessage(starttime.ToString("HH:mm"), start, usetime, ticketname, stoptime.ToString("HH:mm"), stop, money, false, false, id,(TrafficType)rt.Type()));
            else
                finaldata.Add(new TrafficMessage(starttime.ToString("HH:mm"), start, usetime, ticketname, stoptime.ToString("HH:mm"), stop, money, false, true, id,(TrafficType)rt.Type()));

        }
        
        Debug.Log("count " + finaldata.Count);

        return finaldata;
    }

    public bool DeleteTickets(int id)
    {
        Debug.Log("delete ticket id " + id);
        TicketsOperaton ticket_operation = new TicketsOperaton();

        RoutineTicket ticket = ticket_operation.GetTicketByTickedId(id);
        Debug.Log("delte routine id " + ticket.GetRoutineId());
        
        bool abc = ticket_operation.RefundTicket(id);
        TimeManager.instance.RemoveGo(ticket.GetRoutineId());
        return abc;
    }

    public MultiYield Search(int type, string startlocation, string stoplocation, DateTime dt)
    {
        return MultiThreadPool.AddNewMission(new SearchParam(type, startlocation, stoplocation, dt), Search);
    }

    public List<TrafficMessage> Search(System.Object tsearchParam)
    {
        SearchParam searchParam = tsearchParam as SearchParam;
        int type = searchParam.type;
        string startlocation = searchParam.startlocation;
        string stoplocation = searchParam.stoplocation;
        DateTime dt = searchParam.dt;
        RoutineOperation operation = new RoutineOperation();
        List<Routine> tickets = operation.GetAllTicket(startlocation, stoplocation, type, dt);

        List<TrafficMessage> data = new List<TrafficMessage>();

        foreach (RoutineTicket rt in tickets)
        {
            DateTime starttime = rt.GetActualBeginTime();

            if(DateTime.Compare(starttime,TimeManager.instance.NowTime)>0)
            {
                DateTime stoptime = rt.GetAcutalEndTime();

                string start = rt.GetRoutineStartNode();
                string stop = rt.GetEndNode();

                string ticketname = rt.GetTicketName();
                string money = rt.GetMoney() + "";

                int id = rt.GetRoutineId();

                TimeSpan ts = stoptime - starttime;

                string usetime = "";
                if (ts.Hours < 10)
                    usetime += "0";
                usetime += ts.Hours + ":";
                if (ts.Minutes < 10)
                    usetime += "0";
                usetime += ts.Minutes;

                if ((DateTime.Compare(starttime, rt.GetBeginTime()) == 0) && (DateTime.Compare(stoptime, rt.GetEndTime()) == 0))
                    data.Add(new TrafficMessage(starttime.ToString("HH:mm"), start, usetime, ticketname, stoptime.ToString("HH:mm"), stop, money, true, false, id, (TrafficType)rt.Type()));
                else
                    data.Add(new TrafficMessage(starttime.ToString("HH:mm"), start, usetime, ticketname, stoptime.ToString("HH:mm"), stop, money, true, true, id, (TrafficType)rt.Type()));
            }
        }
        return data;
    }

}
