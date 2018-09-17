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
    
    public MultiYield BuyTickets(int id)
    {
        System.Object value = id;
        return MultiThreadPool.AddNewMission(value, BuyingTickets);
    }

    public List<TrafficMessage> BuyingTickets(System.Object value)
    {
        int id = (int)value;
        TicketsOperaton ticket_operation = new TicketsOperaton();
        int ticketid = ticket_operation.BuyTickets(id);
        if(ticketid==0)
        {
            Lucky.LuckyUtils.Log("ticket id error");
            return null;
        }
        RoutineTicket ticket=ticket_operation.GetTicketByTickedId(ticketid);
        Lucky.LuckyUtils.Log("ticket " + ticket.GetRoutineStartNode() + " " + ticket.GetBeginTime() + " "+ticket.GetTicketId());
        TimeManager.instance.AddGo(new TicketParam(ticket));
        Lucky.LuckyUtils.Log("buy ticket id " + ticket.GetTicketId() + " routtine id" + ticket.GetRoutineId());
        return new List<TrafficMessage>();
    }

    public MultiYield GetBuyTickets(DateTime dt)
    {
        Debug.Log("tttttttttttttt " + dt);
        return MultiThreadPool.AddNewMission(dt, GetingBuyTickets);
    }

    private List<TrafficMessage> GetingBuyTickets(System.Object tdt)
    {
        DateTime dt = (DateTime)tdt;
        Lucky.LuckyUtils.Log("abc "+dt);
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

            Lucky.LuckyUtils.Log("start time " + starttime);
            Lucky.LuckyUtils.Log("get ticked id " + id);
            Lucky.LuckyUtils.Log("get routined id  " + rt.GetRoutineId());
            
            /*
            // 第一次开启App，将没有加载的数据放入TimeManager
            if (!isFirstLoad)
            {
                TimeManager.instance.AddGo(new TicketParam(rt));
                isFirstLoad = true;
            }
            */

            Lucky.LuckyUtils.Log("start time " + starttime + " plan start time " + rt.GetBeginTime());
            Lucky.LuckyUtils.Log("stop time " + stoptime + " plan stop time " + rt.GetEndTime());
            if ( (DateTime.Compare(starttime, rt.GetBeginTime())==0) && (DateTime.Compare(stoptime, rt.GetEndTime())==0) )
                finaldata.Add(new TrafficMessage(starttime.ToString("HH:mm"), start, usetime, ticketname, stoptime.ToString("HH:mm"), stop, money, false, false, id,(TrafficType)rt.Type()));
            else
                finaldata.Add(new TrafficMessage(starttime.ToString("HH:mm"), start, usetime, ticketname, stoptime.ToString("HH:mm"), stop, money, false, true, id,(TrafficType)rt.Type()));

        }
        
        Lucky.LuckyUtils.Log("count " + finaldata.Count);

        return finaldata;
    }

    public MultiYield DeleteTickets(int id)
    {
        System.Object value = id;
        return MultiThreadPool.AddNewMission(value, DeleteTickets);
    }

    private List<TrafficMessage> DeleteTickets(System.Object value)
    {
        int id = (int)value;
        Lucky.LuckyUtils.Log("delete ticket id " + id);
        TicketsOperaton ticket_operation = new TicketsOperaton();

        RoutineTicket ticket = ticket_operation.GetTicketByTickedId(id);
        Lucky.LuckyUtils.Log("delte routine id " + ticket.GetRoutineId());
        
        bool abc = ticket_operation.RefundTicket(id);
        if(abc)
        {
            TimeManager.instance.RemoveGo(ticket.GetRoutineId());
            return new List<TrafficMessage>();
        }
        else
            return null;
    }

    public MultiYield Search(int type, string startlocation, string stoplocation, DateTime dt)
    {
        return MultiThreadPool.AddNewMission(new SearchParam(type, startlocation, stoplocation, dt), Searching);
    }

    private List<TrafficMessage> Searching(System.Object tsearchParam)
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
