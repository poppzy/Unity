using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MultiplePlayers
{
	public PlayerInfo[] players;
}

[Serializable]
public class PlayerInfo
{
	public string displayName;
	public string userID;
	public string status;
}

[Serializable]
public class UserInfo
{
	public string name;
	public List<string> activeGames;
	public int victories;
}

[Serializable]
public class GameInfo
{
	public string status;
	public string player1;
	public string player2;

	public string displayName;
	public string gameID;
	public List<PlayerInfo> players;
}