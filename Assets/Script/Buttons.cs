using UnityEngine;
using System.Collections;

public class Buttons : Main
{
	
	public UISprite sound;
	void Start ()
	{
		
		if (PlayerPrefs.GetInt ("sound") == 1 && name == "sound") {
			Main.sound = false;
			sound.spriteName = "btn_sound_off";
			AudioListener.volume = 0;
		} 
		
	}

	void OnPress (bool isDown)
	{
	
		if (isDown == false) {


			if (!GameOwer) {
				if (name == "Open")
					Open ();

				if (name == "OpenAll")
					AllLetter ();

				if (name == "Letter")
					Letter ();


	

				if (name == "s0")
					MainGetGamObject ("word").GetComponent<word> ().OpenBtn (0);
				if (name == "s1")
					MainGetGamObject ("word").GetComponent<word> ().OpenBtn (1);
				if (name == "s2")
					MainGetGamObject ("word").GetComponent<word> ().OpenBtn (2);
				if (name == "s3")
					MainGetGamObject ("word").GetComponent<word> ().OpenBtn (3);
				if (name == "s4")
					MainGetGamObject ("word").GetComponent<word> ().OpenBtn (4);
				if (name == "s5")
					MainGetGamObject ("word").GetComponent<word> ().OpenBtn (5);

				if (name == "wb0" && PlayerPrefs.GetInt ("s0") == 1)
					MainGetGamObject ("word").GetComponent<word> ().CheckOpenWord (0);
				if (name == "wb1" && PlayerPrefs.GetInt ("s1") == 1)
					MainGetGamObject ("word").GetComponent<word> ().CheckOpenWord (1);
				if (name == "wb2" && PlayerPrefs.GetInt ("s2") == 1)
					MainGetGamObject ("word").GetComponent<word> ().CheckOpenWord (2);
				if (name == "wb3" && PlayerPrefs.GetInt ("s3") == 1)
					MainGetGamObject ("word").GetComponent<word> ().CheckOpenWord (3);
				if (name == "wb4" && PlayerPrefs.GetInt ("s4") == 1)
					MainGetGamObject ("word").GetComponent<word> ().CheckOpenWord (4);
				if (name == "wb5" && PlayerPrefs.GetInt ("s5") == 1)
					MainGetGamObject ("word").GetComponent<word> ().CheckOpenWord (5);

			}

			if (name == "h") {

				MainGetGamObject ("Help").SetActive (true);
				gameObject.SetActive (false);
			}

			if (name == "NextLevel")
				Application.LoadLevel (Application.loadedLevel);	
			
			if (name == "Menu") {
				Application.LoadLevel (Application.loadedLevel - 1);
			}

			if (name == "pack1")
				Main.onBuy ("pack1");
			if (name == "pack2")
				Main.onBuy ("pack2");
			if (name == "pack3")
				Main.onBuy ("pack3");
			if (name == "pack4")
				Main.onBuy ("pack4");
			if (name == "pack5")
				Main.onBuy ("pack5");

			if (name == "fb")
				Application.OpenURL ("https://www.facebook.com/SayrexGames");	
			
			
			if (name == "gc") {
				Main.onLeaderBoard ();	
				Invoke ("check", 1);
			}

			if (name == "Play")
				StartCoroutine ("_Start");


			if (name == "Achivment")
				Main.onShowAchivments ();

			if (name == "buy" || name == "Back") {
				if (Application.loadedLevel == 0)
					sound.gameObject.SetActive (true);

				Coin_Panel ();
			}

			if (name == "buyHeart") {
				if (PlayerPrefs.GetInt ("Coin") >= buyHeart) {
					PlayerPrefs.SetInt ("Coin", PlayerPrefs.GetInt ("Coin") - buyHeart);
					CoinUp ();
					PlayerPrefs.SetInt ("Heart", 10);
					heart = (float)((float)PlayerPrefs.GetInt ("Heart") / 10f);
					Heart_Panel ();
				} else
					Coin_Panel ();
			}


			if (name == "BackHeart")
				Heart_Panel ();



			if (name == "sound") {
				if (Main.sound) {
					
					PlayerPrefs.SetInt ("sound", 1);
					Main.sound = false;
					sound.spriteName = "btn_sound_off";
					AudioListener.volume = 0;
				} else {
					PlayerPrefs.SetInt ("sound", 0);
					Main.sound = true;
					sound.spriteName = "btn_sound_on";
					AudioListener.volume = 1;
				}
			}

		}
	}

	private AsyncOperation async;

	IEnumerator _Start ()
	{
		
		async = Application.LoadLevelAsync (1);	
		yield return async;		
	}

	void check ()
	{
		CheckScore ();
	}
}
