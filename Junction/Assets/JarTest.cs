using UnityEngine;
using System.Collections;

public class JarTest : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		//AndroidJavaClass jc = new AndroidJavaClass("io.proximi.proximiiolibrary");
		//AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("ProximiioFactory");
		//AndroidJavaObject proxiimio = jo.Call<AndroidJavaObject>("getProximiio",this,null);
		callShareApp();

	}
	string subject = "WORD-O-MAZE";
	string body = "PLAY THIS AWESOME. GET IT ON THE PLAYSTORE";

	public void callShareApp()
	{
		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		currentActivity.Call("shareText", subject, body);
	}
	// Update is called once per frame
	void Update()
	{

	}
}
