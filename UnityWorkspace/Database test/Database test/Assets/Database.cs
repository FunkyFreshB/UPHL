using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;
using System;


public class Database : MonoBehaviour {

    private bool active = false;
    private NpgsqlConnection dbcon;
    // Use this for initialization

    static void NotificationHandler(object sender, NpgsqlNotificationEventArgs e) {
        Debug.Log("yeah");
    }

    static void startListening() {
        string connectionString = "Server=192.168.1.198;" +
          "Database=idb4home;" +
          "User ID=postgres;" +
          "Password=postgres;" +
          "SyncNotification=false";
        NpgsqlConnection _notificationConnection = new NpgsqlConnection(connectionString);
        _notificationConnection.Open();
        String sql = "LISTEN a11d260747d24ae0a2b40feb14a9aa4a";

        using (NpgsqlCommand command = new NpgsqlCommand(sql, _notificationConnection)) {
            command.ExecuteNonQuery();
        }

        _notificationConnection.Notification += new NotificationEventHandler(NotificationHandler);//NotificationHandler;//new NotificationEventHandler(NotificationHandler);
    }

    void Start() {
        startListening();
        /*string connectionString = "Server=192.168.1.198;" +
          "Database=idb4home;" +
          "User ID=postgres;" +
          "Password=postgres;";
        
        //NpgsqlConnection dbcon;
        dbcon = new NpgsqlConnection(connectionString);
        dbcon.Notification += new NotificationEventHandler(NotificationHandler);//(o, e) => active = true; //Console.WriteLine("Received notification");
        dbcon.Open();

        
        String command = "LISTEN a0a261a690c144698aaeccf7ab5c7770";
        //NpgsqlCommand dbcmd = dbcon.CreateCommand();
        //dbcmd.CommandText = command;
        //dbcmd.ExecuteNonQuery();
        using (NpgsqlCommand sqlCommand = new NpgsqlCommand(command, dbcon)) {
            sqlCommand.ExecuteNonQuery();
        }
        */
        //string sql = "SELECT * " + "FROM resource";

        //string sql = "LISTEN a0a261a690c144698aaeccf7ab5c7770";

        //dbcmd.CommandText = sql;

        /*NpgsqlCommand dbcmd = dbcon.CreateCommand();
        string sql = "SELECT * " + "FROM resource";
        dbcmd.CommandText = sql;
        NpgsqlDataReader reader = dbcmd.ExecuteReader();
        string text = "";
        while (reader.Read()) {
            //string FirstName = "";// (string)reader["resource_uuid"];
            string LastName = (string)reader["description"];
            text += " " + LastName;
        }
        Debug.Log(text);
        Console.Out.WriteLine(text);
        //this.gameObject.name = text;
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;*/
    }
	
	// Update is called once per frame
	void Update () {
        //dbcon.Wait();
        if (active) {
            this.gameObject.SetActive(false);
            Debug.Log("HURUEKA!");
        }
    }


}

//var connString = "Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase";

