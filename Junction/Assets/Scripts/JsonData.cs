using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class JsonData
{
	public PlayerData[] players;
}
[Serializable]
public class PlayerData
{
	public string id;
	public float latitude;
	public float longitude;
	public double accuracy;
	public bool attacked;
	public bool amIAttacked;
	public long utcTimestamp;
}
