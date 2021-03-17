using Firebase.Auth;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirebaseLobby : MonoBehaviour
{
	//Editor Connections
	public TMP_InputField displayName;
	public Transform myGamesListHolder;
	public Transform publicGamesListHolder;
	public GameObject buttonPrefab;

	//Local variables
	string userID;
	UserInfo userInfo;
	FirebaseDatabase db;

	private void Start()
	{
		userID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
		db = FirebaseDatabase.DefaultInstance;
		userInfo = ActiveUser.Instance.userInfo;

        displayName.text = userInfo.name;
        UpdateGameList();
	}

	private void UpdateGameList()
	{
		//clear/remove the old list, If we have one.
		foreach (Transform child in myGamesListHolder)
			GameObject.Destroy(child.gameObject);

		//create new list, load each of the users active games
		foreach (var gameID in userInfo.activeGames)
		{
			StartCoroutine(FirebaseManager.Instance.LoadData("games/" + gameID, LoadGameInfo));
		}
	}

	//Create button for the games, and add onclick events with the corresponding game info.
	public void LoadGameInfo(string json)
	{
		Debug.Log(json);

		var gameInfo = JsonUtility.FromJson<GameInfo>(json);

		var newButton = Instantiate(buttonPrefab, myGamesListHolder).GetComponent<Button>();
		newButton.GetComponentInChildren<TextMeshProUGUI>().text = gameInfo.displayName;
		newButton.onClick.AddListener(() => ActiveGame.Instance.StartGame(gameInfo));
	}

	//Update the user with the new name (from lobby input field)
	public void SetName()
	{
		userInfo.name = displayName.text;
		StartCoroutine(FirebaseManager.Instance.SaveData("users/" + userID, JsonUtility.ToJson(userInfo)));
	}

	public void CreateGame()
	{
		//Check so we have a name and not to many games.
		if (displayName.text == "" || userInfo.activeGames.Count > 4)
			return;

		//Create a new game and start filling out the info.
		var newGameInfo = new GameInfo();
		newGameInfo.displayName = userInfo.name + "'s game";

		//Create Player info for the game.
		var playerInfo = new PlayerInfo();
		playerInfo.displayName = displayName.text;

		newGameInfo.players = new List<PlayerInfo>();
		newGameInfo.players.Add(playerInfo);

		//get a unique ID for the game
		string key = db.RootReference.Child("games/").Push().Key;
		string path = "games/" + key;

		newGameInfo.gameID = key;
		string data = JsonUtility.ToJson(newGameInfo);

		//Save our new game
		StartCoroutine(FirebaseManager.Instance.SaveData(path, data));

		//add the key to our active games
		GameCreated(key);
	}

	public void GameCreated(string gameKey)
	{
		if (userInfo.activeGames == null)
		{
			//If we dont have any active games, create the list.
			userInfo.activeGames = new List<string>();
		}
		userInfo.activeGames.Add(gameKey);

		string jsonData = JsonUtility.ToJson(userInfo);

		//save our user with our new game
		StartCoroutine(FirebaseManager.Instance.SaveData("users/" + userID, jsonData, UpdateGameList));
	}

	public void ListGames()
	{
		Debug.Log("Listing Games");

		//Remove the old list
		foreach (Transform child in publicGamesListHolder)
			GameObject.Destroy(child.gameObject);

		//Load games and create a new list
		StartCoroutine(FirebaseManager.Instance.LoadDataMultiple("games/", ShowGames));
	}

	public void ShowGames(string json)
	{
		var gameInfo = JsonUtility.FromJson<GameInfo>(json);

		if (userInfo.activeGames.Contains(gameInfo.gameID) || gameInfo.players.Count > 1)
		{
			//Don't list our own games or full games.
			return;
		}

		//TODO: optimize away double code (Load game info)
		var newButton = Instantiate(buttonPrefab, publicGamesListHolder).GetComponent<Button>();
		newButton.GetComponentInChildren<TextMeshProUGUI>().text = gameInfo.displayName;
		newButton.onClick.AddListener(() => JoinGame(gameInfo));
	}

	public void JoinGame(GameInfo gameInfo)
	{
		//Debug.Log("joining game: " + gameInfo.gameID);

		userInfo.activeGames.Add(gameInfo.gameID);

		string jsonData = JsonUtility.ToJson(userInfo);

		//save our user with our new game
		StartCoroutine(FirebaseManager.Instance.SaveData("users/" + userID, jsonData));

		var playerInfo = new PlayerInfo();
		playerInfo.displayName = displayName.text;
		gameInfo.players.Add(playerInfo);

		//Create new game name
		gameInfo.displayName = gameInfo.players[0].displayName + " vs " + playerInfo.displayName;

		jsonData = JsonUtility.ToJson(gameInfo);

		//Update the game
		StartCoroutine(FirebaseManager.Instance.SaveData("games/" + gameInfo.gameID, jsonData));
	}
}