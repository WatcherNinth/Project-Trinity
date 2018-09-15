using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Lucky;
using Mono.Data.Sqlite;
using System.IO;

public class AccidentTextOperation  {
    private BasicDataOperation operation = BasicDataOperation.Instance;

#if UNITY_EDITOR
    //通过路径找到第三方数据库
    private static string data_resource = "data source = " + Application.dataPath + "/Plugins/Android/assets/" + "Travel";
    // 如果运行在Android设备中
#elif UNITY_ANDROID
    //将第三方数据库拷贝至Android可找到的地方
    private static string data_resource = "data source = " + Application.persistentDataPath + "/" + "Travel";

#endif

    public AccidentTextOperation()
    {

#if UNITY_ANDROID
        string appDBPath = Application.persistentDataPath + "/" + "Travel";

        if (!File.Exists(appDBPath))
        {
            //用www先从Unity中下载到数据库
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + "Travel");

            while (!loadDB.isDone) { }
            //拷贝至规定的地方
            Lucky.LuckyUtils.Log("init");
            File.WriteAllBytes(appDBPath, loadDB.bytes);
        }
#endif
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
