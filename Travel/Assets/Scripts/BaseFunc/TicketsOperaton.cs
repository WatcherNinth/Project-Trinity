using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Lucky;
using Mono.Data.Sqlite;

public class TicketsOperaton
{
    private BasicDataOperation operation = BasicDataOperation.Instance;
    private static string data_resource = "data source=" + Application.dataPath + "/Travel";

    public TicketsOperaton()
    {
       
    }

    public bool BuyTickets(int routine_id)
    {
        operation.InitConnection(data_resource);

        string sql = "insert into purchased_tickets(routine_id) values(" + routine_id + ")";
        Debug.Log(sql);
        SqliteDataReader reader = operation.ExecuteQuery(sql);
        if (reader.RecordsAffected == 1)
        {
            operation.CloseConnection();
            return true;
        }
        else
        {
            operation.CloseConnection();
            return false;
        }
    }

    public bool RefundTicket(int ticket_id)
    {

        operation.InitConnection(data_resource);

        string sql = "delete from purchased_tickets where routine_id = " + ticket_id; 
        Debug.Log(sql);
        SqliteDataReader reader = operation.ExecuteQuery(sql);
        if (reader.RecordsAffected == 1)
        {
            operation.CloseConnection();
            return true;
        }
        else
        {
            operation.CloseConnection();
            return false;
        }
    } 

}
