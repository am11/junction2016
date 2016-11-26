using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct MapCoordinates
{
	public MapCoordinates(float lat, float lon)
	{
		Latitude = lat;
		Longitude = lon;
	}
	public float Latitude;
	public float Longitude;
}

[Serializable]
public struct UnityCoordinates
{
	public float X;
	public float Y;
}

public static class CoordinateHelpers {
	public static MapCoordinates UnityOrigin = new MapCoordinates(60.165633f, 24.967318f);
	public static float AspectRatio = Mathf.Cos(Mathf.Deg2Rad * 60.165633f);
	public static float RadiusOfEarth = 6371f;
	static public UnityCoordinates MapCoordinatesToUnity(MapCoordinates coordinates)
	{
		UnityCoordinates result = new UnityCoordinates();
		MapCoordinates originCoordinates = new MapCoordinates(coordinates.Latitude - UnityOrigin.Latitude, coordinates.Longitude - UnityOrigin.Longitude);
		result.X = RadiusOfEarth * originCoordinates.Longitude * AspectRatio;
		result.Y = RadiusOfEarth * originCoordinates.Latitude;

		return result;
	}
	
}
