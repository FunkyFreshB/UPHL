using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;
//using System;


public class Database : MonoBehaviour {

    // Use this for initialization
    void Start() {
        string connectionString = "Server=192.168.1.198;" +
          "Database=idb4home;" +
          "User ID=postgres;";
          //"Password=postgres;";

        NpgsqlConnection dbcon;
        dbcon = new NpgsqlConnection(connectionString);
        dbcon.Open();

        NpgsqlCommand dbcmd = dbcon.CreateCommand();

        string sql = "SELECT * " + "FROM resource";

        dbcmd.CommandText = sql;
        NpgsqlDataReader reader = dbcmd.ExecuteReader();
        string text = "";
        while (reader.Read()) {
            string FirstName = "";// (string)reader["resource_uuid"];
            string LastName = (string)reader["description"];
            text += " " + LastName;
        }
        this.GetComponent<TextMesh>().text = text;
    }
	
	// Update is called once per frame
	void Update () {
		
	}


}

//var connString = "Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase";

