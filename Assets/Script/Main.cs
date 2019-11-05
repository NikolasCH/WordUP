using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using SA.Android.Firebase.Analytics;

public class Main : MonoBehaviour
{
	public static string word = "угадай", language = "RU", temp = "";
	public static bool cam = false, sound = true, GameOwer = false;
	public static int baza_Length = 0, price = 10, OpenWord = 500, DeleteWord = 100, PriceOpenLetter = 10, buyHeart = 1000, OpenLetter = 0, OpneAllLetter = 0, OpenWordLetter = 3;
	public static float heart = 10;
	public static GameObject[] OBJECTS;

	public static GameObject SX;

	public static GameObject MainGetGamObject (string name)
	{
		GameObject obj = null;
		if (OBJECTS != null)
			for (int i = 0; i < OBJECTS.Length; i++) {
				if (OBJECTS [i] != null && OBJECTS [i].name == name) {
					obj = OBJECTS [i];
					break;
				}
			}
		if (obj == null)
			obj = GameObject.Find (name);
		return obj;
	}

	public static string basa (int l, int s)
	{
		MatchCollection t;
		Regex f = new Regex ("(?<=\")([^\"]+)(?=\")");

		if (!PlayerPrefs.HasKey ("basa_" + l.ToString ())) {
			int q = l % 3000;
			if (q == 0)
				l--;
			TextAsset txt = (TextAsset)Resources.Load ((l - l % 3000).ToString () + "-" + (l - l % 3000 + 3000).ToString (), typeof(TextAsset));
			if (q == 0)
				l++;
			string data = txt.text;	
			Regex r = new Regex ("(?<={)([^}]{5,})(?=})");
			MatchCollection b = r.Matches (data);
			if (q == 0)
				q = 3000;
			t = f.Matches (b [q].ToString ());
			PlayerPrefs.DeleteKey ("basa_" + (l - 1).ToString ());
			PlayerPrefs.SetString ("basa_" + l.ToString (), b [q].ToString ());
		} else 
			t = f.Matches (PlayerPrefs.GetString ("basa_" + l.ToString ()));

		return t [s].ToString ();
	}
	
	public static int length (string w)
	{
		TextAsset txt = (TextAsset)Resources.Load (w, typeof(TextAsset));
		string data = txt.text;	
		Regex r = new Regex ("(?<={)([^}]{5,})(?=})");
		MatchCollection b = r.Matches (data);
		return b.Count - 1;
	}
			
	public static bool check_word (string w)
	{
		return w == basa (PlayerPrefs.GetInt ("lvl"), 6);
	}
	
	public static void Play ()
	{
		//MainGetGamObject ("DeleteCoin").GetComponent<UILabel> ().text = DeleteWord.ToString ();
		MainGetGamObject ("OpenCoin").GetComponent<UILabel> ().text = OpenWord.ToString ();

		MainGetGamObject ("bg").SetActive (false);
		MainGetGamObject ("shadow").GetComponent<UISprite> ().alpha = 0;

		LevelUp ();
		CoinUp ();
		heart = (float)((float)PlayerPrefs.GetInt ("Heart") / 10f);

		Debug.Log (heart);
		baza_Length = 1500;
		HeartUp (heart);
		#if UNITY_EDITOR
		Debug.Log (length ("0-3000"));// + "_" + length ("3000-6000") + "_" + length ("6000-9000") + "_" + length ("9000-12000") + "_" + length ("12000-15000") + "_" + length ("15000-18000"));
		#endif
		if (PlayerPrefs.GetInt ("lvl") > baza_Length) {
			Finish ();
		} else {

			word = basa (PlayerPrefs.GetInt ("lvl"), 6);
			Debug.Log (word);
//			btn_price = 10;
//			for (int i=0; i<6; i++)
//				if (PlayerPrefs.GetInt ("s" + (i)) > 0)
//					btn_price += price;

			check_words ();
		}
	}

	public static void check_words ()
	{

		OpenLetter = 0;
		OpneAllLetter = 0;
		temp = "";

		for (int i=0; i<6; i++) {	

			string w = "[b]";
			string _word = basa (PlayerPrefs.GetInt ("lvl"), i);

			for (int l=0; l<_word.Length; l++)
				w += l < (OpenWordLetter + PlayerPrefs.GetInt ("OpenLetter")) ? _word [l].ToString ().ToLower () : ((PlayerPrefs.GetInt ("s" + (i)) > 0 || PlayerPrefs.GetInt ("wb" + (i)) > 0 || _word.Length == l + 1) ? (_word.Length == l + 1 ? "[/sub]" : "") + _word [l].ToString ().ToLower () : GetString (_word));

			MainGetGamObject ("w" + (i)).GetComponent <UILabel> ().text = w;
			if (PlayerPrefs.GetInt ("s" + (i)) == 0 && PlayerPrefs.GetInt ("wb" + (i)) == 0) {
				if ((_word.Length <= (OpenWordLetter + 1 + PlayerPrefs.GetInt ("OpenLetter")))) {
					MainGetGamObject ("s" + (i)).GetComponent <UISprite> ().spriteName = "open word 1";
					if (MainGetGamObject ("s" + (i)).GetComponent<BoxCollider> ())
						Destroy (MainGetGamObject ("s" + (i)).GetComponent<BoxCollider> ());
					MainGetGamObject ("p" + (i)).transform.localScale = Vector3.zero;
				} else
					MainGetGamObject ("p" + (i)).GetComponent <UILabel> ().text = (_word.Length - (OpenWordLetter + 1 + PlayerPrefs.GetInt ("OpenLetter"))) > 0 && PlayerPrefs.GetInt ("OpenPrice") > 0 ? ((_word.Length - (OpenWordLetter + 1 + PlayerPrefs.GetInt ("OpenLetter"))) * price).ToString () : "0";
			} else {
				if (PlayerPrefs.GetInt ("wb" + (i)) == 1) {
					MainGetGamObject ("wb" + (i)).GetComponent <UISprite> ().alpha = 0.3f;
					MainGetGamObject ("w" + (i)).GetComponent <UILabel> ().color = new Color32 (7, 18, 50, 255);
					MainGetGamObject ("w" + (i)).GetComponent <UILabel> ().effectStyle = UILabel.Effect.None;
					MainGetGamObject ("s" + (i)).GetComponent <UISprite> ().spriteName = "heart-";
					MainGetGamObject ("s" + (i)).GetComponent<TweenScale> ().enabled = true;
					if (MainGetGamObject ("wb" + (i)).GetComponent<BoxCollider> ())
						Destroy (MainGetGamObject ("wb" + (i)).GetComponent<BoxCollider> ());
				} else
					MainGetGamObject ("s" + (i)).GetComponent <UISprite> ().spriteName = "open word 1";
			
				if (MainGetGamObject ("s" + (i)).GetComponent<BoxCollider> ())
					Destroy (MainGetGamObject ("s" + (i)).GetComponent<BoxCollider> ());

				MainGetGamObject ("p" + (i)).transform.localScale = Vector3.zero;
			}
		}

		MainGetGamObject ("LetterCoin").GetComponent<UILabel> ().text = (PriceOpenLetter * OpenLetter).ToString ();
		MainGetGamObject ("OpenAllCoin").GetComponent<UILabel> ().text = OpneAllLetter == 1 ? (PriceOpenLetter * OpenLetter).ToString () : ((PriceOpenLetter - 1) * OpneAllLetter).ToString ();
	}

	public static string GetString (string t)
	{
		if (temp != t) {
			temp = t;
			OpenLetter++;
		}
		OpneAllLetter++;
		return "[sub]*";
	}	


	public static void Finish ()
	{	
		MainGetGamObject ("good").SetActive (true);
		MainGetGamObject ("pic_menu").SetActive (false);
//		GameObject[] menus = GameObject.FindGameObjectsWithTag ("menu");
//		foreach (GameObject go in menus)
//			NGUITools.SetActive (go, false);
	}	


	public static void Open ()
	{	
		if (PlayerPrefs.GetInt ("Coin") >= OpenWord) {
			PlayerPrefs.SetInt ("Coin", PlayerPrefs.GetInt ("Coin") - OpenWord);
			PlayerPrefs.SetInt ("Heart", PlayerPrefs.GetInt ("Heart") + 1);
			CoinUp ();
			for (int i=0; i<6; i++) {
				if (check_word (basa (PlayerPrefs.GetInt ("lvl"), i))) {
					PlayerPrefs.SetInt ("wb" + (i), 1);
					Destroy (MainGetGamObject ("s" + (i)).GetComponent<BoxCollider> ());
					Destroy (MainGetGamObject ("wb" + (i)).GetComponent<BoxCollider> ());
					MainGetGamObject ("s" + (i)).GetComponent <UISprite> ().spriteName = "heart+";
					MainGetGamObject ("wb" + (i)).GetComponent <UISprite> ().spriteName = "pravilnoe slovo";
					MainGetGamObject ("w" + (i)).GetComponent <UILabel> ().text = basa (PlayerPrefs.GetInt ("lvl"), i);
					MainGetGamObject ("p" + (i)).transform.localScale = Vector3.zero;
					MainGetGamObject ("word").GetComponent<word> ().good_wait (MainGetGamObject ("w" + (i)));
					break;
				}
			}
		} else
			Coin_Panel ();
	}	
	public static void Coin_Panel ()
	{
		Debug.Log ("Coin_Panel");
		TweenPosition tp = MainGetGamObject ("Panel_Buy_Coin").AddComponent<TweenPosition> ();

		if (tp.gameObject.transform.localPosition.x == 0)
			MainGetGamObject ("shadow").GetComponent<UISprite> ().alpha = 0f;
		else {
			MainGetGamObject ("shadow").GetComponent<TweenAlpha> ().ResetToBeginning ();
			MainGetGamObject ("shadow").GetComponent<TweenAlpha> ().enabled = true;
		}

		tp.from = tp.gameObject.transform.localPosition;
		tp.to = new Vector3 (tp.gameObject.transform.localPosition.x == 0 ? 550 : 0, 0);
		tp.duration = 0.5f;

		if (MainGetGamObject ("Panel_Buy_Heart") && MainGetGamObject ("Panel_Buy_Heart").transform.localPosition.x == 0)
			Heart_Panel ();
	}
	public static void Heart_Panel ()
	{
		Debug.Log ("Heart_Panel");
		TweenPosition tp = MainGetGamObject ("Panel_Buy_Heart").AddComponent<TweenPosition> ();

		if (tp.gameObject.transform.localPosition.x == 0)
			MainGetGamObject ("shadow").GetComponent<UISprite> ().alpha = 0f;
		else {
			MainGetGamObject ("shadow").GetComponent<TweenAlpha> ().ResetToBeginning ();
			MainGetGamObject ("shadow").GetComponent<TweenAlpha> ().enabled = true;
		}
		tp.from = tp.gameObject.transform.localPosition;
		tp.to = new Vector3 (tp.gameObject.transform.localPosition.x == 0 ? 550 : 0, 0);
		tp.duration = 0.5f;


	}
	public static void Letter ()
	{	
		if ((PriceOpenLetter * OpenLetter) > 0 && PlayerPrefs.GetInt ("Coin") >= (PriceOpenLetter * OpenLetter) && PlayerPrefs.GetInt ("OpenLetter") < 10) {
			PlayerPrefs.SetInt ("Coin", PlayerPrefs.GetInt ("Coin") - (PriceOpenLetter * OpenLetter));
			CoinUp ();
			PlayerPrefs.SetInt ("OpenLetter", PlayerPrefs.GetInt ("OpenLetter") + 1);
			check_words ();
		} else
			Coin_Panel ();
	}
	public static void AllLetter ()
	{	
		if (((PriceOpenLetter - 1) * OpneAllLetter) > 0 && PlayerPrefs.GetInt ("Coin") >= ((PriceOpenLetter - 1) * OpneAllLetter) && PlayerPrefs.GetInt ("OpenLetter") < 10) {
			PlayerPrefs.SetInt ("Coin", PlayerPrefs.GetInt ("Coin") - (PriceOpenLetter * OpenLetter));
			CoinUp ();
			PlayerPrefs.SetInt ("OpenLetter", PlayerPrefs.GetInt ("OpenLetter") + 10);
			check_words ();
		} else
			Coin_Panel ();
	}
	public static void Delete ()
	{	
		bool d = false;
		for (int i=0; i<6; i++) {
			if (!check_word (basa (PlayerPrefs.GetInt ("lvl"), i)) && PlayerPrefs.GetInt ("wb" + (i)) != 1) {
				d = true;
				break;
			}
		}

		if (PlayerPrefs.GetInt ("Coin") >= DeleteWord && d) {
			PlayerPrefs.SetInt ("Coin", PlayerPrefs.GetInt ("Coin") - DeleteWord);
			CoinUp ();

			for (int i=0; i<6; i++) {
				if (!check_word (basa (PlayerPrefs.GetInt ("lvl"), i)) && PlayerPrefs.GetInt ("wb" + (i)) != 1) {
					PlayerPrefs.SetInt ("wb" + (i), 1);
					check_words ();
					MainGetGamObject ("word").GetComponent<word> ().bad_wait ();
					break;
				}
			}
		} else
			Coin_Panel ();
	}	
	
	public static void CoinAdd (int AddScore)
	{	
		PlayerPrefs.SetInt ("Coin", PlayerPrefs.GetInt ("Coin") + AddScore);
		CoinUp ();
	}	
	
	public static void CoinUp ()
	{	
		UILabel Txt = (UILabel)GameObject.Find ("coin_lbl").GetComponent ("UILabel");		
		Txt.text = PlayerPrefs.GetInt ("Coin").ToString ();
	}	
	
	public static void LevelUp ()
	{	
		UILabel Txt = (UILabel)GameObject.Find ("lvl_lbl").GetComponent ("UILabel");		
		Txt.text = PlayerPrefs.GetInt ("lvl").ToString ();
	}

	public static void UpdateLevel (int _lvl)
	{
		if (_lvl > PlayerPrefs.GetInt("lvl"))
			PlayerPrefs.SetInt ("lvl",_lvl);
	}
	
	public static void HeartUp (float _heart)
	{	

		MainGetGamObject ("HeartLine").GetComponent <UISprite> ().fillAmount = _heart;//(float)((float)PlayerPrefs.GetInt ("Heart") / 10f);	
		MainGetGamObject ("Blod").GetComponent <UISprite> ().fillAmount = 1f - _heart;//(float)((float)PlayerPrefs.GetInt ("Heart") / 10f);	

	}
	
	public static void buy (string ProductId )
	{		
		PlayerPrefs.SetInt ("Ad", 1);	
		GameObject.Find ("Music").SendMessage ("onCoin");
		onAchivment ();
	}


	public static void onBuy (string product)
	{
		AN_FirebaseAnalytics.LogEvent("try_purchase");
        SX.GetComponent <SX_InApp_Android>().Purchase(product);
	}



	public static void onLeaderBoard ()
	{
		SX.GetComponent <SX_GameCenter>().showLeaderBoards(null);
	}

	public static void onShowAchivments ()
	{
	    SX.GetComponent <SX_GameCenter>().showArchievements();
	}

	public static void postScoreToLeaderBoard (int _score)
	{
        SX.GetComponent <SX_GameCenter>().submitPlayerScore(null, _score);
	}

	public static void CheckScore ()
	{
		SX.GetComponent <SX_GameCenter>().loadPlayerScore(null);
	}

	public static void onShowInterstitial ()
	{
		Debug.Log ("onShowInterstitial");
		SX.GetComponent<SX_Ads> ().showWhenReadyNonRewarded ();
	}

	public static void onShowSmartBanner ()
	{
		Debug.Log ("onShowInterstitial");
		SX.GetComponent<SX_Ads> ().showWhenReadyBannerLoad ();
	}

	public static void onShow ()
	{

	}

	public static void onAchivment ()
	{
		SX_GameCenter GameCenter = SX.GetComponent <SX_GameCenter>();

		if (PlayerPrefs.GetInt ("good_") >= 10 && !PlayerPrefs.HasKey ("good_10")) {
			PlayerPrefs.SetInt ("good_10", 1);
			GameCenter.unlockAchievement ("good_10");
		} else if (PlayerPrefs.GetInt ("good_") >= 25 && !PlayerPrefs.HasKey ("good_25")) {
			PlayerPrefs.SetInt ("good_25", 1);
			GameCenter.unlockAchievement ("good_25");
		} else if (PlayerPrefs.GetInt ("good_") >= 50 && !PlayerPrefs.HasKey ("good_50")) {
			PlayerPrefs.SetInt ("good_50", 1);
			GameCenter.unlockAchievement ("good_50");
		} else if (PlayerPrefs.GetInt ("lvl") >= 1 && !PlayerPrefs.HasKey ("achievement_1")) {
			PlayerPrefs.SetInt ("achievement_1", 1);
			GameCenter.unlockAchievement ("achievement_1");
		} else if (PlayerPrefs.GetInt ("lvl") >= 100 && !PlayerPrefs.HasKey ("achievement_100")) {
			PlayerPrefs.SetInt ("achievement_100", 1);
			GameCenter.unlockAchievement ("achievement_100");
		} else if (PlayerPrefs.GetInt ("lvl") >= 200 && !PlayerPrefs.HasKey ("achievement_200")) {
			PlayerPrefs.SetInt ("achievement_200", 1);
			GameCenter.unlockAchievement ("achievement_200");
		} else if (PlayerPrefs.GetInt ("lvl") >= 300 && !PlayerPrefs.HasKey ("achievement_300")) {
			PlayerPrefs.SetInt ("achievement_300", 1);
			GameCenter.unlockAchievement ("achievement_300");
		} else if (PlayerPrefs.GetInt ("lvl") >= 400 && !PlayerPrefs.HasKey ("achievement_400")) {
			PlayerPrefs.SetInt ("achievement_400", 1);
			GameCenter.unlockAchievement ("achievement_400");
		} else if (PlayerPrefs.GetInt ("lvl") >= 500 && !PlayerPrefs.HasKey ("achievement_500")) {
			PlayerPrefs.SetInt ("achievement_500", 1);
			GameCenter.unlockAchievement ("achievement_500");
		} else if (PlayerPrefs.GetInt ("lvl") >= 600 && !PlayerPrefs.HasKey ("achievement_600")) {
			PlayerPrefs.SetInt ("achievement_600", 1);
			GameCenter.unlockAchievement ("achievement_600");
		} else if (PlayerPrefs.GetInt ("lvl") >= 700 && !PlayerPrefs.HasKey ("achievement_700")) {
			PlayerPrefs.SetInt ("achievement_700", 1);
			GameCenter.unlockAchievement ("achievement_700");
		} else if (PlayerPrefs.GetInt ("lvl") >= 800 && !PlayerPrefs.HasKey ("achievement_800")) {
			PlayerPrefs.SetInt ("achievement_800", 1);
			GameCenter.unlockAchievement ("achievement_800");
		} else if (PlayerPrefs.GetInt ("lvl") >= 900 && !PlayerPrefs.HasKey ("achievement_900")) {
			PlayerPrefs.SetInt ("achievement_900", 1);
			GameCenter.unlockAchievement ("achievement_900");
		} else if (PlayerPrefs.GetInt ("lvl") >= 1000 && !PlayerPrefs.HasKey ("achievement_1000")) {
			PlayerPrefs.SetInt ("achievement_1000", 1);
			GameCenter.unlockAchievement ("achievement_1000");
		} else if (PlayerPrefs.GetInt ("lvl") >= 2000 && !PlayerPrefs.HasKey ("achievement_2000")) {
			PlayerPrefs.SetInt ("achievement_2000", 1);
			GameCenter.unlockAchievement ("achievement_2000");
		} else if (PlayerPrefs.GetInt ("lvl") >= 3000 && !PlayerPrefs.HasKey ("achievement_3000")) {
			PlayerPrefs.SetInt ("achievement_3000", 1);
			GameCenter.unlockAchievement ("achievement_3000");
		} else if (PlayerPrefs.GetInt ("lvl") >= 4000 && !PlayerPrefs.HasKey ("achievement_4000")) {
			PlayerPrefs.SetInt ("achievement_4000", 1);
			GameCenter.unlockAchievement ("achievement_4000");
		} else if (PlayerPrefs.GetInt ("lvl") >= 5000 && !PlayerPrefs.HasKey ("achievement_5000")) {
			PlayerPrefs.SetInt ("achievement_5000", 1);
			GameCenter.unlockAchievement ("achievement_5000");
		} else if (PlayerPrefs.GetInt ("lvl") >= 6000 && !PlayerPrefs.HasKey ("achievement_6000")) {
			PlayerPrefs.SetInt ("achievement_6000", 1);
			GameCenter.unlockAchievement ("achievement_6000");
		} else if (PlayerPrefs.GetInt ("lvl") >= 7000 && !PlayerPrefs.HasKey ("achievement_7000")) {
			PlayerPrefs.SetInt ("achievement_7000", 1);
			GameCenter.unlockAchievement ("achievement_7000");
		} else if (PlayerPrefs.GetInt ("lvl") >= 8000 && !PlayerPrefs.HasKey ("achievement_8000")) {
			PlayerPrefs.SetInt ("achievement_8000", 1);
			GameCenter.unlockAchievement ("achievement_8000");
		} else if (PlayerPrefs.GetInt ("lvl") >= 9000 && !PlayerPrefs.HasKey ("achievement_9000")) {
			PlayerPrefs.SetInt ("achievement_9000", 1);
			GameCenter.unlockAchievement ("achievement_9000");
		} else if (PlayerPrefs.GetInt ("lvl") >= 10000 && !PlayerPrefs.HasKey ("achievement_10000")) {
			PlayerPrefs.SetInt ("achievement_10000", 1);
			GameCenter.unlockAchievement ("achievement_10000");
		} else if (PlayerPrefs.GetInt ("Coin") >= 1000 && !PlayerPrefs.HasKey ("coin_1000")) {
			PlayerPrefs.SetInt ("coin_1000", 1);
			GameCenter.unlockAchievement ("coin_1000");
		} else if (PlayerPrefs.GetInt ("Coin") >= 2000 && !PlayerPrefs.HasKey ("coin_2000")) {
			PlayerPrefs.SetInt ("coin_2000", 1);
			GameCenter.unlockAchievement ("coin_2000");
		} else if (PlayerPrefs.GetInt ("Coin") >= 3000 && !PlayerPrefs.HasKey ("coin_3000")) {
			PlayerPrefs.SetInt ("coin_3000", 1);
			GameCenter.unlockAchievement ("coin_3000");
		} else if (PlayerPrefs.GetInt ("Coin") >= 5000 && !PlayerPrefs.HasKey ("coin_5000")) {
			PlayerPrefs.SetInt ("coin_5000", 1);
			GameCenter.unlockAchievement ("coin_5000");
		}  
	}

}
