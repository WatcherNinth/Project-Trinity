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

    public static DateTime GetTodayTime(int seconds)
    {
        DateTime now = GameModel.Instance.Start;
        DateTime new_now = new DateTime(now.Year, now.Month, now.Day);
        int hour = seconds / 3600;
        int minutes = (seconds - hour * 3600) / 60;
        
        
        new_now  = new_now.Add(new TimeSpan(hour, minutes, 0));
        return new_now;
    }
    public static void SetTicketName(SqliteDataReader reader, RoutineTicket ticket)
    {
        Type type = reader.GetFieldType(reader.GetOrdinal("ticket_name"));
        if (type == typeof(int))
        {
            ticket.SetTicketName("" + reader.GetInt32(reader.GetOrdinal("ticket_name")));
        }
        if (type == typeof(string))
        {
            ticket.SetTicketName(reader.GetString(reader.GetOrdinal("ticket_name")));
        }
    }
       

    public int BuyTickets(int routine_id)
    {
        operation.InitConnection(data_resource);

        try
        {
            string sql = "insert into purchased_tickets(routine_id) values(" + routine_id + ")";
            Debug.Log(sql);
            SqliteDataReader reader = operation.ExecuteQuery(sql);
            if (reader.RecordsAffected == 1)
            {
                string get_ticket_id_sql = "select max(ticket_id) as insert_ticket_id from purchased_tickets";
                reader = operation.ExecuteQuery(get_ticket_id_sql);

                reader.Read();

                int inserted_ticket_id = reader.GetInt32(reader.GetOrdinal("insert_ticket_id"));

                operation.CloseConnection();
                return inserted_ticket_id;
            }
            else
            {
                operation.CloseConnection();
                return 0;
            }

        } finally
        {
            operation.CloseConnection();
        }
    
       
    }

    public bool RefundTicket(int ticket_id)
    {

        try
        {
            operation.InitConnection(data_resource);

            string sql = "delete from purchased_tickets where ticket_id = " + ticket_id;
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
        finally
        {
            operation.CloseConnection();
        }
       
    }

    public List<RoutineTicket> GetUserTickets(DateTime time)
    {
        UInt64 ts = RoutineOperation.GetSeconds(time);
        operation.InitConnection(data_resource);

        // string sql = "select routine.*, purchased_tickets.* from routine, purchased_tickets where purchased_tickets.routine_id = routine.routine_id ";
        string sql = "select routine.*, purchased_tickets.* from routine, purchased_tickets where purchased_tickets.routine_id = routine.routine_id";

        Debug.Log(sql);
        List<RoutineTicket> res = new List<RoutineTicket>();

        try
        {
            SqliteDataReader reader = operation.ExecuteQuery(sql);
            while (reader.Read())
            {
                RoutineTicket ticket = new RoutineTicket();
                long begin_time = reader.GetInt64(reader.GetOrdinal("start_time"));
                long end_time = reader.GetInt64(reader.GetOrdinal("end_time"));
                ticket.SetRoutineId(reader.GetInt32(reader.GetOrdinal("routine_id")));
                ticket.SetEndNode(reader.GetString(reader.GetOrdinal("end_node")));
                ticket.SetStartNode(reader.GetString(reader.GetOrdinal("start_node")));
                ticket.SetType(reader.GetInt32(reader.GetOrdinal("type")));
                ticket.SetBeginTime(GetTodayTime(reader.GetInt32(reader.GetOrdinal("start_time"))));
                ticket.SetEndTime(GetTodayTime(reader.GetInt32(reader.GetOrdinal("end_time"))));
                ticket.SetMoney((int)reader.GetFloat(reader.GetOrdinal("money")));
                SetTicketName(reader, ticket);
                ticket.SetTicketid(reader.GetInt32(reader.GetOrdinal("ticket_id")));
                res.Add(ticket);
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
        finally
        {
            operation.CloseConnection();
            Debug.Log(res.Count);
          
        }
        return res;

    }

    public RoutineTicket GetTicketByTickedId(int id)
    {
        operation.InitConnection(data_resource);

        string sql = "select routine.*, purchased_tickets.* from routine, purchased_tickets where purchased_tickets.routine_id = routine.routine_id and purchased_tickets.ticket_id = " + id;

        Debug.Log(sql);
        RoutineTicket ticket = new RoutineTicket();

        try
        {
            SqliteDataReader reader = operation.ExecuteQuery(sql);
   
            if (reader.HasRows)
            {
                reader.Read();

                long begin_time = reader.GetInt64(reader.GetOrdinal("start_time"));
                long end_time = reader.GetInt64(reader.GetOrdinal("end_time"));
                ticket.SetRoutineId(reader.GetInt32(reader.GetOrdinal("routine_id")));
                ticket.SetEndNode(reader.GetString(reader.GetOrdinal("end_node")));
                ticket.SetStartNode(reader.GetString(reader.GetOrdinal("start_node")));
                ticket.SetType(reader.GetInt32(reader.GetOrdinal("type")));
                ticket.SetBeginTime(GetTodayTime(reader.GetInt32(reader.GetOrdinal("start_time"))));
                ticket.SetEndTime(GetTodayTime(reader.GetInt32(reader.GetOrdinal("end_time"))));
                ticket.SetMoney((int)reader.GetFloat(reader.GetOrdinal("money")));
                SetTicketName(reader, ticket);
                ticket.SetTicketid(reader.GetInt32(reader.GetOrdinal("ticket_id")));
            }

        } catch(Exception e)
        {

        }
        finally
        {
            operation.CloseConnection();
        }      
        return ticket;
    }
   
    public List<int> DelayTickets(DateTime accident_happen_time, int city_id, int duration, AccidentType type)
    {
        string city_name = CityUtil.Instance.GetCityName(city_id);
        Debug.Log("city name " + city_name);
        Debug.Log("accdent happen time " + accident_happen_time.ToString());
        List<int> affected_routine_ids = new List<int>();

        if (type == AccidentType.rail)
        {

                try
                {
                    operation.InitConnection(data_resource);
                    string sql = "select * from routine where start_node like \"%" + city_name + "%\" and type =0 order by start_time";

                    Debug.Log("select all the start node sql " + sql);

                    SqliteDataReader reader = operation.ExecuteQuery(sql);
                    List<Routine> res = RoutineOperation.GetRoutinInfo(reader);
                    operation.CloseConnection();
                    Debug.Log("res result " + res.Count);

                    List<Routine> delay_routine = new List<Routine>();
                    UInt64 accident_happen_time_seconds = RoutineOperation.GetSeconds(accident_happen_time);
                    Debug.Log("accident_happen_time_seconds " + accident_happen_time_seconds);
               

                    foreach (Routine t in res)
                    {
               
                        UInt64 begin_time = RoutineOperation.GetSeconds(t.GetBeginTime());
                        Debug.Log("begin time " + begin_time);

                        UInt64 end_time = RoutineOperation.GetSeconds(t.GetEndTime());

                        if (begin_time >= accident_happen_time_seconds)
                        {
                        int routine_id = t.GetRoutineId();
                        UInt64 actual_begin_time = begin_time + (UInt32)duration * 60;
                        UInt64 actual_end_time = end_time + (UInt32)duration * 60;

                        sql = "update routine set actual_start_time = " + actual_begin_time + ", actual_end_time = " + actual_end_time  + ", event_happen_time = " + accident_happen_time_seconds
                            + " where routine_id = " + routine_id;

                        operation.InitConnection(data_resource);
                        reader = operation.ExecuteQuery(sql);//

                        if (reader.RecordsAffected == 1)
                        {
                            affected_routine_ids.Add(routine_id);
                            operation.CloseConnection();
                           
                        }
                    }
                }
            }
            finally
            {

                operation.CloseConnection();

            }
            return affected_routine_ids;

        }
  

        if (type == AccidentType.airport)
        {
            try
            {
                operation.InitConnection(data_resource);
                string sql = "select * from routine where (start_node like \"%" + city_name + "%\"  or end_node like \"%"  + city_name + "%\")" + " and type = 1 order by start_time";

                Debug.Log("select all the start node sql " + sql);

                SqliteDataReader reader = operation.ExecuteQuery(sql);
                List<Routine> res = RoutineOperation.GetRoutinInfo(reader);
                operation.CloseConnection();
                Debug.Log("res result " + res.Count);

                List<Routine> delay_routine = new List<Routine>();
                UInt64 accident_happen_time_seconds = RoutineOperation.GetSeconds(accident_happen_time);
                Debug.Log("accident_happen_time_seconds " + accident_happen_time_seconds);


                foreach (Routine t in res)
                {

                    UInt64 begin_time = RoutineOperation.GetSeconds(t.GetBeginTime());
                    Debug.Log("begin time " + begin_time);

                    UInt64 end_time = RoutineOperation.GetSeconds(t.GetEndTime());

                    if (begin_time >= accident_happen_time_seconds)
                    {
                        int routine_id = t.GetRoutineId();
                        UInt64 actual_begin_time = begin_time + (UInt32)duration * 60;
                        UInt64 actual_end_time = end_time + (UInt32)duration * 60;

                        sql = "update routine set actual_start_time = " + actual_begin_time + ", actual_end_time = " + actual_end_time + ", event_happen_time = " + accident_happen_time_seconds
                            + " where routine_id = " + routine_id;

                        operation.InitConnection(data_resource);
                        reader = operation.ExecuteQuery(sql);//

                        if (reader.RecordsAffected == 1)
                        {
                            affected_routine_ids.Add(routine_id);
                            operation.CloseConnection();

                        }
                    }
                }
            }
            finally
            {

                operation.CloseConnection();

            }
            return affected_routine_ids;
        }
        return affected_routine_ids;
    }
}

