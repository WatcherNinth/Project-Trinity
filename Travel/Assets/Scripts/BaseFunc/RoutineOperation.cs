﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Lucky;
using Mono.Data.Sqlite;

public class Routine
{
    private string start_node;
    private string end_node;
    private int type;
    private int routine_id;
    private DateTime begin_time;
    private DateTime end_time;
    // 车次信息
    private string ticket_name;

    public string GetRoutineStartNode()
    {
        return start_node;
    }
   
    public void SetStartNode(string node)
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
    public int Type()
    {
        return type;
    }

    public void SetType(int t)
    {
        type = t;
    }

    public string GetTicketName()
    {
        return ticket_name;
    }

    public void SetTicketName(string ticket_name)
    {
        this.ticket_name = ticket_name;
    }
    public void SetBeginTime(DateTime time)
    {
        begin_time = time;
    }
    public void SetEndTime(DateTime time)
    {
        end_time = time;
    }

    public DateTime GetBeginTime()
    {
        return begin_time;
    }
    public DateTime GetEndTime()
    {
        return end_time;
    }
}

public class RoutineTicket : Routine {
  

    DateTime actual_begin_time;
    DateTime actual_end_time;

    int money;
    int ticket_id;

  
    public void SetMoney(int money)
    {
        this.money = money;
    }

    public int GetMoney()
    {
        return money;
    }

    public int GetTicketId()
    {
        return ticket_id;
    }

    public void SetTicketid(int ticket_id)
    {
        this.ticket_id = ticket_id;
    }
    public void SetActualBeginTime(DateTime time)
    {
        this.actual_begin_time = time;
    }
    public void SetActualEndTime(DateTime time)
    {
        this.actual_end_time = time;
    }
    public DateTime GetActualBeginTime()
    {
        return this.actual_begin_time;
    }
    public DateTime GetAcutalEndTime()
    {
        return this.actual_end_time;
    }
    
}

public class RoutineOperation {
    private List<RoutineTicket> routines = new List<RoutineTicket>();
    private BasicDataOperation operation = BasicDataOperation.Instance;

#if UNITY_EDITOR
    //通过路径找到第三方数据库
    private static string data_resource =  "data source = " + Application.dataPath + "/Plugins/Android/assets/" + "Travel";

    // 如果运行在Android设备中
#elif UNITY_ANDROID
		//将第三方数据库拷贝至Android可找到的地方
    private static string data_resource = "data source = " + Application.persistentDataPath + "/" + "Travel";
#endif

    // private static string data_resource = "data source=" + Application.dataPath +"/Travel";

    public static UInt64 GetTimeStamp(DateTime dt)
    {
        DateTime dateStart = new DateTime();
        Debug.Log("seconds " + (dt - dateStart).TotalSeconds);
        UInt64 timeStamp = Convert.ToUInt64((dt - dateStart).TotalSeconds);
        Debug.Log("timestamp " + timeStamp);
        return timeStamp;
    }

    public static DateTime GetTime(string timeStamp, bool bflag = true)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime;
        if (bflag == true)
        {
            lTime = long.Parse(timeStamp + "0000000");
        }
        else
        {
            lTime = long.Parse(timeStamp + "0000");
        }

        TimeSpan toNow = new TimeSpan(lTime); return dtStart.Add(toNow);
    }

    public static List<Routine> GetRoutinInfo(SqliteDataReader reader)
    {
        List<Routine> res = new List<Routine>();
        while (reader.Read())
        {
            RoutineTicket ticket = new RoutineTicket();
            long begin_time = reader.GetInt64(reader.GetOrdinal("start_time"));
            long end_time = reader.GetInt64(reader.GetOrdinal("end_time"));
            ticket.SetRoutineId(reader.GetInt32(reader.GetOrdinal("routine_id")));
            ticket.SetEndNode(reader.GetString(reader.GetOrdinal("end_node")));
            ticket.SetStartNode(reader.GetString(reader.GetOrdinal("start_node")));
            ticket.SetType(reader.GetInt32(reader.GetOrdinal("type")));
            int start_time = reader.GetInt32(reader.GetOrdinal("start_time"));

            ticket.SetBeginTime(TicketsOperaton.GetTodayTime(start_time));

            ticket.SetEndTime(TicketsOperaton.GetTodayTime(reader.GetInt32(reader.GetOrdinal("end_time"))));
            ticket.SetMoney((int)reader.GetFloat(reader.GetOrdinal("money")));
            Debug.Log(ticket.GetRoutineId());


            ticket.SetTicketName(reader.GetOrdinal("ticket_name").ToString());
            res.Add(ticket);
        }
        return res;
    }

    public RoutineOperation()
    {
      
    }


    // 0表示火车，1表示飞机
    public bool InsertTicket(string start_node, string end_node, int ticket_type, DateTime begin_time, DateTime end_time, int money, string ticket_name)
    {
        operation.InitConnection(data_resource);
        if (start_node == "" || end_node == "" || (ticket_type != 0 && ticket_type != 1))
        {
            return false;
        }

        UInt64 begin_time_ts = GetTimeStamp(begin_time);
        UInt64 end_time_ts = GetTimeStamp(end_time);

        string sql = "insert into routine (start_node, end_node, start_time, end_time, type, money, ticket_name) values(\""
            + start_node + "\",\"" + end_node + "\"," + begin_time_ts + "," + end_time_ts + ", " + ticket_type + ", " + money + "," + "\"" +ticket_name +"\")";
        Debug.Log(sql);
        SqliteDataReader reader = operation.ExecuteQuery(sql);
        if (reader.RecordsAffected == 1)
        {
            operation.CloseConnection();
            return true;
        } else
        {
            operation.CloseConnection();
            return false;
        }
    }

    public List<Routine> GetAllTicket(string start_node, string end_node, int ticket_type, DateTime time)
    {

        operation.InitConnection(data_resource);

        string sql = "select * from routine where start_node like \"%" + start_node + "%\" and "
            + "end_node like \"%" + end_node + "%\"" + " and type = " + ticket_type;

        Debug.Log(sql);
        SqliteDataReader reader = operation.ExecuteQuery(sql);
        List<Routine> res = GetRoutinInfo(reader);

        operation.CloseConnection();
        Debug.Log(res.Count);
        return res;
    }

    public bool DelayTickets()
    {
        return true;
    }

}
