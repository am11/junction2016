using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<PlayerVisualizer> players;
    public PlayerVisualizer LocalPlayer, TargetPlayer;
    private float ownPositionTimer;
    public float ownPositionFrequency;
    public Text DebugText;
    private float nextActionTime = 0.0f;
    public float period = 0.1f;
    public string playerId;
    public string targetId;

    // Use this for initialization
    void Start()
    {
        playerId = "???";
        targetId = "???";

        ownPositionTimer = ownPositionFrequency;
        SetTargetPlayer();
    }

    // Update is called once per frame
    IEnumerator Update()
    {
        TargetPlayer.UpdatePosition();
        if (ownPositionTimer <= 0)
        {
            ownPositionTimer = ownPositionFrequency;
            StartCoroutine(BlinkPlayer());
        }
        else
        {
            ownPositionTimer -= Time.deltaTime;
        }

#if UNITY_ANDROID || !UNITY_EDITOR
        MapCoordinates coordinates = MapCoordinates.ToMapCoordinates(GetPosition());

        DebugText.text = coordinates.ToString();

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            yield return GetOpponentPosition(coordinates);
        }
#else
        DebugText.text = GetPosition();
        yield break;
#endif
    }

    private IEnumerator GetOpponentPosition(MapCoordinates coordinates)
    {
        WWW www = new WWW("https://ngjunction2016.azurewebsites.net/api/HttpTriggerJS1?code=rVbhaoGvX5L/WNnJQEmMpkPiRNrFykb7aiDBEpF0qv7W4xAE9adbQQ==&playerId=" + playerId + "&latitude=" + coordinates.Latitude + "&longitude=" + coordinates.Longitude + "&accuracy=" + coordinates.Accuracy + "&opponentId=" + targetId);

        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    private IEnumerator GetAllPositions(MapCoordinates coordinates)
    {
        WWW www = new WWW("https://ngjunction2016.azurewebsites.net/api/HttpTriggerJS1?code=rVbhaoGvX5L/WNnJQEmMpkPiRNrFykb7aiDBEpF0qv7W4xAE9adbQQ==&playerId=" + playerId + "&latitude=" + coordinates.Latitude + "&longitude=" + coordinates.Longitude + "&accuracy=" + coordinates.Accuracy);

        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public float GetDistanceBetweenPlayers(PlayerVisualizer playerA, PlayerVisualizer playerB)
    {
        return (new Vector2(playerA.UnityCoords.X, playerA.UnityCoords.Y) - new Vector2(playerB.UnityCoords.X, playerB.UnityCoords.Y)).magnitude;
    }

    public void SetTargetPlayer()
    {
        if (TargetPlayer != null)
        {
            TargetPlayer.Visible = false;
        }
        if (players.Count > 1)
        {
            do
            {
                TargetPlayer = players[Random.Range(0, players.Count)];
            } while (TargetPlayer == LocalPlayer);
        }
        TargetPlayer.Visible = true;
    }

    private IEnumerator BlinkPlayer()
    {
        LocalPlayer.UpdatePosition();
        LocalPlayer.Visible = true;
        yield return new WaitForSeconds(2);
        LocalPlayer.Visible = false;
    }

    public string GetPosition()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		string positionString = currentActivity.Call<string>("GetPositionString");
		
		return positionString;
#else
        return "Editor";
#endif
    }
}
