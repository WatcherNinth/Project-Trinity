using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Lucky;
using Mono.Data.Sqlite;
using System.IO;

public class AccidentTextOperation  {
    private BasicDataOperation operation = BasicDataOperation.Instance;
    private static string data_resource = "";

    public AccidentTextOperation()
    {
    }
    
    public List<AccidentText> GetRailAccidentText()
    {
        List<AccidentText> res = new List<AccidentText>();
        operation.InitConnection(data_resource);
        string sql = "select * from accident_text order by accident_text_id asc";
        SqliteDataReader reader = operation.ExecuteQuery(sql);

        while (reader.Read())
        {
            AccidentText t = new AccidentText();
            t.title = reader.GetString(reader.GetOrdinal("accident_title"));
            t.description = reader.GetString(reader.GetOrdinal("accident_description"));
            string type = reader.GetString(reader.GetOrdinal("accident_type"));
            AccidentType accident_type = AccidentType.rail;

            if (type == "Airport")
            {
                accident_type = AccidentType.airport;
            }
            t.type = accident_type;
            res.Add(t);
        }
        operation.CloseConnection();
        return res;
    }
}
