using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;
using UnityEngine.UI;

public class Database : MonoBehaviour
{
    private const string fileName = "testDB.db";
    public static string connectionString;
    public static SqliteConnection connection;
    public static SqliteCommand command;
    public static SqliteDataReader reader;

    public void OpenDataBase(string name)
    {
        string filepath = "";
        // check if file exists in Application.persistentDataPath
        if (Application.platform == RuntimePlatform.Android)
            filepath = Application.persistentDataPath + "/" + name;
        else
            filepath = Application.streamingAssetsPath + "/" + name;
        if (!File.Exists(filepath))
        {
            // if it doesn't ->
            // open StreamingAssets directory and load the db -> 
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + name);
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDB.bytes);
        }

        //open db connection
        connectionString = "URI=file:" + filepath;
        connection = new SqliteConnection(connectionString);
        connection.Open();
    }

    public void CloseDataBase()
    {
        reader.Close(); // clean everything up
        reader = null;
        command.Dispose();
        command = null;
        connection.Close();
        connection = null;
    }

    public SqliteDataReader BasicQuery(string query)
    { // run a basic Sqlite query
        command = connection.CreateCommand(); // create empty command
        command.CommandText = query; // fill the command
        reader = command.ExecuteReader(); // execute command which returns a reader
        return reader; // return the reader
    }

    public void InsertInto(string query)
    {
        try
        {
            command = connection.CreateCommand(); // create empty command
            command.CommandText = query; // fill the command
            reader = command.ExecuteReader(); // execute command which returns a reader
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void DeleteQuery(string query)
    {
        try
        {
            command = connection.CreateCommand(); // create empty command
            command.CommandText = "pragma foreign_keys = on; " + query; // fill the command
            reader = command.ExecuteReader(); // execute command which returns a reader
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public SqliteDataReader SelectQuery(string query)
    { // run a basic Sqlite query
        command = connection.CreateCommand(); // create empty command
        command.CommandText = query; // fill the command
        reader = command.ExecuteReader(); // execute command which returns a reader
        return reader; // return the reader
    }

    public void UpdateQuery(string query)
    {
        try
        {
            command = connection.CreateCommand(); // create empty command
            command.CommandText = query; // fill the command
            reader = command.ExecuteReader(); // execute command which returns a reader
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public int GetId(string query)
    {
        int id = 0;
        OpenDataBase("testDB.db");
        reader = SelectQuery(query);
        while (reader.Read())
        {
            id = reader.GetInt32(0);
        }
        CloseDataBase();
        return id;
    }
}

    