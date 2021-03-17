using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Text;
using System.Net;
using Firebase.Extensions;
using Firebase.Database;
using Firebase.Auth;

[Serializable]
public enum Options
{
    Rock,
	Scissors,
	Paper,
}

/*[Serializable]
public class PlayerInfo
{
    public string name;
	public Options userInput;
	public string opponent;
}*/



public class SaveManager : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		LoadData();
	}

	public void SaveData(int userInput)
    {
		Options selected = (Options)userInput;

		var player = new PlayerInfo();
		/*player.name = "Oscar";
		player.userInput = selected;*/
		string jsonString = JsonUtility.ToJson(player);
		SaveToFirebase(jsonString);
	}

	//Collects game data, turns it to json and calls save function
	/*public void SaveData()
	{
		//Get player info
		var players = FindObjectsOfType<PlayerMovement>();

		//Create holder object that contains multiple players

		//turn class into json 
		*//*string jsonString = JsonUtility.ToJson(multiplePlayers);*//*

		//Save the json String (can be done in different ways.
		//PlayerPrefs.SetString("json", jsonString);
		//SaveToFile("CarGameSaveFile", jsonString);
		//SaveOnline("CarGameSaveFile", jsonString);
		*//*SaveToFirebase(jsonString);*//*
	}*/

	//Saves to firebase
	private void SaveToFirebase(string data)
	{
		var db = FirebaseDatabase.DefaultInstance;
		var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
		db.RootReference.Child("users").Child(userId).SetRawJsonValueAsync(data);
		//This is done without callback or check that it worked. This is a base example.
	}

	//Selects a way to load data and runs the load state function with that data.
	public void LoadData()
	{
		//Load from firebase, this will call load state once its done
		LoadFromFirebase();

		#region OldSaveExamples       
		//load from player prefs
		//string jsonString = PlayerPrefs.GetString("json");

		//load from file
		//string jsonString2 = Load("CarGameSaveFile");

		//load from server
		//string jsonString = LoadOnline("CarGameSaveFile");

		//Compare if local save is the same as server
		//if (jsonString != jsonString2)
		//{
		//	Debug.LogError("Not the same!!!!");
		//}

		//load using json data.
		//LoadState(jsonString);
		#endregion
	}

	//The function that acctually updates stuff in our game
	private void LoadState(string jsonString)
	{
		/*//Convert our save data to a class
		var multiplePlayers = JsonUtility.FromJson<MultiplePlayers>(jsonString);
		//Find all players in the scene
		var players = FindObjectsOfType<PlayerMovement>();

		//Update Players.
		for (int i = 0; i < players.Length; i++)
		{
			players[i].name = multiplePlayers.players[i].Name;
			players[i].transform.position = multiplePlayers.players[i].Position;
		}

		//Tell our nametag manager to update our nametags.
		FindObjectOfType<NameTagManager>().UpdateNameTags();*/
	}

	//Load data from firebase
	private void LoadFromFirebase()
	{
		var db = FirebaseDatabase.DefaultInstance;
		var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
		var dataTask = db.RootReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
		{
			if (task.Exception != null)
			{
				Debug.LogError(task.Exception);
			}

			DataSnapshot snap = task.Result;
			//Update the game with our loaded data
			LoadState(snap.GetRawJsonValue());
		});
	}
}


