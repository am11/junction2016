using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<PlayerData> players;
    public PlayerVisualizer LocalPlayer, TargetPlayer;
    private float ownPositionTimer;
    public float ownPositionFrequency;
    public Text DebugText;
    private float nextActionTime = 0.0f;
    public float period = 1f;
    public string playerId;
    public string targetId;
	public bool waitingResponse;
	public float HitDistance = 1.0f;
	public Image reloadImage;
	public Sprite Reloading, Reloaded;
	public float ReloadTimer;
	public float CurrentReloadTimer;
	public bool reloaded;
	public Image HitImage;
	public Sprite Hit, Miss, YouWereKilled;
	// Use this for initialization
	void Start()
    {
		if (PlayerPrefs.HasKey("JunctionPlayerID"))
		{
			playerId = PlayerPrefs.GetString("JunctionPlayerID");
		}
		else
		{
			playerId = System.Guid.NewGuid().ToString();
			PlayerPrefs.SetString("JunctionPlayerID", playerId);
		}
		Debug.Log("GameManger:Start, calling GetAllPositions");
		if (!waitingResponse)
		{
			Debug.Log("GameManger:Start, calling GetAllPositions, waitingResponse = false");

			StartCoroutine(GetAllPositions(MapCoordinates.ToMapCoordinates(GetPosition())));
		}
		Debug.Log("GameManger:Start, called getallpositions");

		ownPositionTimer = ownPositionFrequency;
		reloaded = true;
    }

    // Update is called once per frame
    void Update()
    {
		if (CurrentReloadTimer < ReloadTimer && !reloaded)
		{
			CurrentReloadTimer += Time.deltaTime;
		}
		else
		{
			Reload();
			CurrentReloadTimer = 0;
		}

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
		
        MapCoordinates coordinates = MapCoordinates.ToMapCoordinates(GetPosition());
		LocalPlayer.data.latitude = coordinates.Latitude;
		LocalPlayer.data.longitude = coordinates.Longitude;
		DebugText.text = coordinates.ToString();

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
			if (!waitingResponse && !string.IsNullOrEmpty(TargetPlayer.data.id))
			{
				StartCoroutine(GetOpponentPosition(coordinates));
			}
        }
		if (Input.GetMouseButtonDown(0))
		{
			Tapped();
		}
    }

    private IEnumerator GetOpponentPosition(MapCoordinates coordinates)
    {
		waitingResponse = true;
		string url = "https://ngjunction2016.azurewebsites.net/api/HttpTriggerJS1?playerId=" + playerId + "&latitude=" + coordinates.Latitude + "&longitude=" + coordinates.Longitude + "&accuracy=" + coordinates.Accuracy + "&opponentId=" + TargetPlayer.data.id;

		WWW www = new WWW(url);

        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
			PlayerData data = JsonUtility.FromJson<PlayerData>(www.text);
			TargetPlayer.data = data;
			Debug.Log("");
		}
		else
        {
            Debug.Log("WWW Error: " + www.error);
        }
		waitingResponse = false;
    }

    private IEnumerator GetAllPositions(MapCoordinates coordinates, bool attackedOpponent = false)
    {
		Debug.Log("GetAllPositionsStarted");
		waitingResponse = true;
        WWW www = new WWW("https://ngjunction2016.azurewebsites.net/api/HttpTriggerJS1?playerId=" + playerId + "&latitude=" + coordinates.Latitude + "&longitude=" + coordinates.Longitude + "&accuracy=" + coordinates.Accuracy + (attackedOpponent ? "&opponentAttackedId=" + TargetPlayer.data.id : ""));
		Debug.Log("request URL:" +www.url);
		yield return www;
		Debug.Log("Request Done!");
        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
			JsonData data = JsonUtility.FromJson<JsonData>(www.text);
			players.Clear();
			for (int i = 0; i < data.players.Length; i++)
			{
				players.Add(data.players[i]);
			}

			SetTargetPlayer();
			Debug.Log("");
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
		waitingResponse = false;
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
                TargetPlayer.data = players[Random.Range(0, players.Count)];
            } while (TargetPlayer.data.id == playerId);
        }
        TargetPlayer.Visible = true;
    }

    private IEnumerator BlinkPlayer()
    {
        LocalPlayer.UpdatePosition();
        LocalPlayer.Visible = true;
        yield return new WaitForSeconds(2);
        LocalPlayer.Visible = true;
    }

    public string GetPosition()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		string positionString = currentActivity.Call<string>("GetPositionString");
		
		return positionString;
#else
        return "60.16563;24.96732;1";
#endif
    }

	public bool Attacked()
	{
		float distanceToTarget = GetDistanceBetweenPlayers(LocalPlayer, TargetPlayer);
		Debug.Log("Distance to target: " + distanceToTarget);
		return (distanceToTarget <= HitDistance);
	}

	public void Tapped()
	{
		Debug.Log("Attack clicked");
		reloadImage.sprite = Reloading;
		reloaded = false;
		bool hit = Attacked();
		if (hit)
		{
			Debug.Log("Target killed");

			StartCoroutine(GetAllPositions(new MapCoordinates(LocalPlayer.data.latitude,LocalPlayer.data.longitude), true));
		}
		StartCoroutine(ShowShootResult(hit));

	}

	IEnumerator ShowShootResult(bool hit)
	{
		HitImage.gameObject.SetActive(true);
		HitImage.sprite = hit ? Hit : Miss;
		yield return new WaitForSeconds(2);
		HitImage.gameObject.SetActive(false);
	}

	public void Reload()
	{
		reloadImage.sprite = Reloaded;
		reloaded = true;
	}
}
