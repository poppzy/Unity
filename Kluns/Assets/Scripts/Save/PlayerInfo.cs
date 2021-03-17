using System;
using UnityEngine;

[Serializable]
public class MultiplePlayers
{
	public PlayerInfo[] players;
}

[Serializable]
public class PlayerInfo
{
	public string Name;
	public Vector3 Position;
}