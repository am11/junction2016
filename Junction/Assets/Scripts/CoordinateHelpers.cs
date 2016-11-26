using System;
using UnityEngine;

[Serializable]
public struct MapCoordinates
{
    public MapCoordinates(float latitude, float longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        Accuracy = -1;
    }

    public MapCoordinates(float latitude, float longitude, double accuracy)
    {
        Latitude = latitude;
        Longitude = longitude;
        Accuracy = accuracy;
    }

    public double Accuracy;
    public float Latitude;
    public float Longitude;

    public static MapCoordinates ToMapCoordinates(string raw)
    {
        string[] tuple = raw.Split(';');
        return new MapCoordinates(float.Parse(tuple[0]), float.Parse(tuple[1]), double.Parse(tuple[0]));
    }

    public override string ToString()
    {
        return "Latitude = " + Latitude + Environment.NewLine +
               "Longitude = " + Longitude + Environment.NewLine +
               "Accuracy = " + Accuracy;
    }
}

[Serializable]
public struct UnityCoordinates
{
    public float X;
    public float Y;
}

public static class CoordinateHelpers
{
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
