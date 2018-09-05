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
        return null;
    }

    public void DeleteTickets(int id)
    {
        Debug.Log("delete " + id);
        TicketsOperaton ticket_operation = new TicketsOperaton();
        ticket_operation.RefundTicket(id);
    }
	
}
