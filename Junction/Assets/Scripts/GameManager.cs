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

    // Use this for initialization
    void Start()
    {
        ownPositionTimer = ownPositionFrequency;
        SetTargetPlayer();
    }

    // Update is called once per frame
    void Update()
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

#if UNITY_ANDROID && !UNITY_EDITOR
        DebugText.text = MapCoordinates.ToMapCoordinates(GetPosition()).ToString();
#else
        DebugText.text = GetPosition();
#endif
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
