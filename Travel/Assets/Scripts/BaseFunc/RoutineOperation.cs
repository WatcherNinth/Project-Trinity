using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Lucky;
using Mono.Data.Sqlite;

public class Routine
{
    string start_node;
    private string end_node;
    private bool type;
    private int routine_id;

    string GetRoutineStartNode()
    {
        return start_node;
    }

    void SetStartNode(string node)
    {
        start_node = node;
    }
    public string GetEndNode()
    {
        return end_node;
    }
    public void SetEndNode(string node)
    {
        end_node = node;
    }

    public int GetRoutineId()
    {
        return routine_id;
    }

    public void SetRoutineId(int id)
    {
        routine_id = id;
    }
    public bool Type()
    {
        return type;
    }

    public void SetType(bool t)
    {
        type = t;
    }
}

public class RoutineTicket : Routine {
    DateTime time;
}

public class RoutineOperation {
    private List<RoutineTicket> routines = new List<RoutineTicket>();
    private BasicDataOperation operation = BasicDataOperation.Instance;
    private static string data_resource = "data source=" + Application.dataPath +"/Travel";

    public RoutineOperation()
    {
        operation.InitConnection(data_resource);
    }


    public List<RoutineTicket> GetAllTicket(string start_node, string end_node, bool ticket_type, DateTime time)
    {
        int ticket_type_value = 0;
        if (ticket_type)
            ticket_type_value = 1;

        string sql = "select routine_id, start_node, end_node from routine where start_node = \"" + start_node + "\" and "
            + "end_node = \"" + end_node + "\"" + ", type = " + ticket_type_value;
        Debug.Log(sql);

        SqliteDataReader reader = operation.ExecuteQuery(sql);

        List<RoutineTicket> res = new List<RoutineTicket>();

        while (reader.Read())
        {
            RoutineTicket ticket = new RoutineTicket();
            ticket.SetRoutineId(reader.GetOrdinal("routine_id"));
            ticket.SetEndNode(reader.GetString(reader.GetOrdinal("end_node")));
            // ticket.SetStartNode(reader.GetString(reader.GetOrdinal("start_node")));
            // ticket.SetType(reader.GetBoolean(reader.GetOrdinal("type")));      
            res.Add(ticket);
        }
        Debug.Log(res.Count);
        return res;
    }

}
