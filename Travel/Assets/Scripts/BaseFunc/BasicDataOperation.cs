using UnityEngine;
using System.Collections;
using Lucky;
using Mono.Data.Sqlite;
using System;

public class BasicDataOperation : BaseInstance<BasicDataOperation> {
    private SqliteConnection db_connection;
    private SqliteCommand db_command;
    private SqliteDataReader data_reader;
    
    public void InitConnection(string connectionString)
    {
        try {
            Debug.Log("connection str " + connectionString);
            db_connection = new SqliteConnection(connectionString);
            Debug.Log("connection str  " + connectionString);

            db_connection.Open();
        } catch(Exception e) { 
            Debug.Log("connection str 1 " + connectionString);
            Debug.Log(e.Message);
        }
        
    }

    public SqliteDataReader ExecuteQuery(string queryString)
    {
        db_command = db_connection.CreateCommand();
        db_command.CommandText = queryString;
        data_reader = db_command.ExecuteReader();
        return data_reader;
    }

    public void CloseConnection()
    {
        if (db_command != null)
        {
            db_command.Cancel();
        }
        db_command = null;
        if (data_reader != null)
        {
            data_reader.Close();
        }
        data_reader = null;
        if (db_connection != null)
        {
            db_connection.Close();
        }
        db_connection = null;
    }

    public SqliteDataReader ReadFullTable(string tableName)
    {
        string queryString = "SELECT * FROM " + tableName;
        return ExecuteQuery(queryString);
    }


    public SqliteDataReader InsertValues(string tableName, string[] values)
    {
        // 获取数据表中字段数目
        int fieldCount = ReadFullTable(tableName).FieldCount;
        // 当插入的数据长度不等于字段数目时引发异常
        if (values.Length != fieldCount)
        {
            throw new SqliteException("values.Length!=fieldCount");
        }

        string queryString = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for (int i = 1; i < values.Length; i++)
        {
            queryString += ", " + values[i];
        }
        queryString += " )";
        return ExecuteQuery(queryString);
    }

    public SqliteDataReader DeleteValuesOR(string tableName, string[] colNames, string[] operations, string[] colValues)
    {
        //当字段名称和字段数值不对应时引发异常
        if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
        {
            throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
        }

        string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
        for (int i = 1; i < colValues.Length; i++)
        {
            queryString += "OR " + colNames[i] + operations[0] + colValues[i];
        }
        return ExecuteQuery(queryString);
    }

    public SqliteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
    {
        string queryString = "CREATE TABLE " + tableName + "( " + colNames[0] + " " + colTypes[0];
        for (int i = 1; i < colNames.Length; i++)
        {
            queryString += ", " + colNames[i] + " " + colTypes[i];
        }
        queryString += "  ) ";
        return ExecuteQuery(queryString);
    }

    public SqliteDataReader ReadTable(string tableName, string[] items, string[] colNames, string[] operations, string[] colValues)
    {
        string queryString = "SELECT " + items[0];
        for (int i = 1; i < items.Length; i++)
        {
            queryString += ", " + items[i];
        }
        queryString += " FROM " + tableName + " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
        for (int i = 0; i < colNames.Length; i++)
        {
            queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[0] + " ";
        }
        return ExecuteQuery(queryString);
    }
}
