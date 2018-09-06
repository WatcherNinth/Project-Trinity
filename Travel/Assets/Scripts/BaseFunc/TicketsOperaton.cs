using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Lucky;
using Mono.Data.Sqlite;
using System.IO;

public class TicketsOperaton
{
    private BasicDataOperation operation = BasicDataOperation.Instance;

#if UNITY_EDITOR
    //通过路径找到第三方数据库
    private static string data_resource = "data source = " + Application.dataPath + "/Plugins/Android/assets/" + "Travel";
    // 如果运行在Android设备中
#elif UNITY_ANDROID
    //将第三方数据库拷贝至Android可找到的地方
    private static string data_resource = "data source = " + Application.persistentDataPath + "/" + "Travel";

#endif

    public TicketsOperaton()
    {

#if UNITY_ANDROID
        string appDBPath = Application.persistentDataPath + "/" + "Travel";

        if (!File.Exists(appDBPath))
        {
            //用www先从Unity中下载到数据库
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + "Travel");

            while (!loadDB.isDone) { }
            //拷贝至规定的地方
            Debug.Log("init");
            File.WriteAllBytes(appDBPath, loadDB.bytes);
        }
#endif

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

    public List<RoutineTicket> GetUserTickets(DateTime time)
    {
        long ts = RoutineOperation.GetTimeStamp(time);
        operation.InitConnection(data_resource);

        string sql = "select routine.*, purchased_tickets.* from routine, purchased_tickets where purchased_tickets.routine_id = routine.routine_id ";
        // string sql = "select routine.*, purchased_tickets.* from routine, purchased_tickets where purchased_tickets.routine_id = routine.routine_id where routine.start_time > " + ts;

        Debug.Log(sql);
        SqliteDataReader reader = operation.ExecuteQuery(sql);
        List<RoutineTicket> res = RoutineOperation.GetRoutinInfo(reader);
        operation.CloseConnection();
        Debug.Log(res.Count);
        return res;
    }
}

