using UnityEngine;
using System.Collections;
using System;

public class Cam : MonoBehaviour
{
	float timer = 0;
	int second = 120;
	// Use this for initialization
	void Start ()
	{
		timer = 0;
		if (Main.cam) {
			Destroy (gameObject);
		} else {

			Main.cam = true;
			DontDestroyOnLoad (gameObject.transform);
			PlayerPrefs.DeleteKey ("basa_" + PlayerPrefs.GetInt ("lvl").ToString ());
		}

		AddSeconds ();
	}

	void  OnEnable ()
	{
		AddSeconds ();
	}

	void AddSeconds ()
	{
		if (PlayerPrefs.GetInt ("timer") > 0)
			SetLabel ();

		if (PlayerPrefs.HasKey ("DateTime")) {
			TimeSpan diff = DateTime.Now - DateTime.Parse (PlayerPrefs.GetString ("DateTime"));
			PlayerPrefs.DeleteKey ("DateTime");
			//PlayerPrefs.SetString ("DateTime", DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.FF"));
			int seconds = (int)diff.TotalSeconds;

			if (PlayerPrefs.HasKey ("Heart") && PlayerPrefs.GetInt ("Heart") == 0 && seconds > 0)
				PlayerPrefs.SetInt ("timer", PlayerPrefs.GetInt ("timer") + seconds);
		}
	}

	void FixedUpdate ()
	{
		if (Main.heart == 0) {
			timer += Time.deltaTime;

			if (timer > 1) {
				timer = 0;
				PlayerPrefs.SetInt ("timer", PlayerPrefs.GetInt ("timer") + 1);
				SetLabel ();
			}
		}
	}

	void  SetLabel ()
	{
		
		if (PlayerPrefs.GetInt ("timer") >= second * PlayerPrefs.GetInt ("Hearts")) {
			Main.MainGetGamObject ("HeartLabel").GetComponent<UILabel> ().text = "";
			PlayerPrefs.SetInt ("timer", 0);
			PlayerPrefs.SetInt ("Heart", 1);
			Main.heart = (float)((float)PlayerPrefs.GetInt ("Heart") / 10f);
			Main.HeartUp (Main.heart);
		} else		
			Main.MainGetGamObject ("HeartLabel").GetComponent<UILabel> ().text = ((int)(((second * PlayerPrefs.GetInt ("Hearts")) - 60) / 60 - PlayerPrefs.GetInt ("timer") / 60)).ToString ("00") + ":" + (60 - (PlayerPrefs.GetInt ("timer") % 60)).ToString ("00");

	}

	void  OnDisable ()
	{
		PlayerPrefs.SetString ("DateTime", DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.FF"));
	}
}
