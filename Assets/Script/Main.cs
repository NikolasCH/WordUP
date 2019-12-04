using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using SA.Android.Firebase.Analytics;

public class Main : MonoBehaviour
{
	public static string word = "угадай", language = "RU", temp = "";
	public static bool cam = false, sound = true, GameOwer = false;
	public static int baza_Length = 0, price = 5, OpenWord = 100, DeleteWord = 100, PriceOpenLetter = 10, buyHeart = 500, OpenLetter = 0, OpneAllLetter = 0, OpenWordLetter = 0;
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

	//обновляем индикатор указывающий когда игрок получит сердце
	public static void Fill_Heart (float from)
	{
		
		TweenFill tf = MainGetGamObject ("Add_Heart").GetComponent<TweenFill>();
		tf.ResetToBeginning();
		tf.from = from; //tf.gameObject.GetComponent<UISprite>().fillAmount;
		tf.to = ((float)PlayerPrefs.GetInt ("Add_Heart")/5f);
		tf.Play(true);
	}
	
	//Начинаем игру
	public static void Play ()
	{
		if(PlayerPrefs.GetInt ("lvl")>3)
			SX.GetComponent<SX_Ads>().smartBanneShow();

		Fill_Heart (((float)PlayerPrefs.GetInt ("Add_Heart")/5f));

		PlayerPrefs.SetInt ("OpenWord", 0);

		MainGetGamObject ("OpenCoin").GetComponent<UILabel> ().text = OpenWord.ToString ();

		MainGetGamObject ("bg").SetActive (false);
		MainGetGamObject ("shadow").GetComponent<UISprite> ().alpha = 0;

		LevelUp ();
		GameObject.Find ("coin_lbl").GetComponent <UILabel>().text = PlayerPrefs.GetInt ("Coin").ToString() ;
		heart = (float)((float)PlayerPrefs.GetInt ("Heart") / 10f);

		AN_FirebaseAnalytics.LogEvent("Start_Play");

		//Debug.Log (heart);
		baza_Length = 1500;
		HeartUp (heart);
		#if UNITY_EDITOR
		//Debug.Log (length ("0-3000"));
		#endif
		if (PlayerPrefs.GetInt ("lvl") > baza_Length) {
			Finish ();
		} else {

			word = basa (PlayerPrefs.GetInt ("lvl"), 6);
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
				w += l < (OpenWordLetter + PlayerPrefs.GetInt ("OpenLetter")) ? _word [l].ToString ().ToLower () : ( (_word.Length == l + 1 ? "[/sub]" : "") + _word [l].ToString ().ToLower ());

			MainGetGamObject ("w" + (i)).GetComponent <UILabel> ().text = w;
			if (PlayerPrefs.GetInt ("s" + (i)) == 0 && PlayerPrefs.GetInt ("wb" + (i)) == 0) {
				if ((_word.Length <= (OpenWordLetter + 1 + PlayerPrefs.GetInt ("OpenLetter")))) {
					MainGetGamObject ("s" + (i)).GetComponent <UISprite> ().spriteName = "open word 1";
					if (MainGetGamObject ("s" + (i)).GetComponent<BoxCollider> ())
						Destroy (MainGetGamObject ("s" + (i)).GetComponent<BoxCollider> ());
					MainGetGamObject ("p" + (i)).transform.localScale = Vector3.zero;
				} else
					MainGetGamObject ("p" + (i)).GetComponent <UILabel> ().text = "";// (_word.Length - (OpenWordLetter + 1 + PlayerPrefs.GetInt ("OpenLetter"))) > 0 && PlayerPrefs.GetInt ("OpenPrice") > 0 ? ((_word.Length - (OpenWordLetter + 1 + PlayerPrefs.GetInt ("OpenLetter"))) + price).ToString () : "0";
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

		AN_FirebaseAnalytics.LogEvent("Finished_Game");
		MainGetGamObject ("good").SetActive (true);
		MainGetGamObject ("pic_menu").SetActive (false);
	}	


	public static void Open ()
	{	
		if (PlayerPrefs.GetInt ("Coin") >= OpenWord) {
			PlayerPrefs.SetInt ("Coin", PlayerPrefs.GetInt ("Coin") - OpenWord);
			PlayerPrefs.SetInt ("OpenWord", 1);
			AN_FirebaseAnalytics.LogEvent("Next_word");
			CoinUp ();
			for (int i=0; i<6; i++) {
				if (check_word (basa (PlayerPrefs.GetInt ("lvl"), i))) {
					PlayerPrefs.SetInt ("wb" + (i), 1);
					Destroy (MainGetGamObject ("s" + (i)).GetComponent<BoxCollider> ());
					Destroy (MainGetGamObject ("wb" + (i)).GetComponent<BoxCollider> ());
					MainGetGamObject ("s" + (i)).GetComponent <UISprite> ().spriteName = "+ kristal";
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
		//Debug.Log ("Coin_Panel");
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
		//Debug.Log ("Heart_Panel");
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
		UILabel Txt = GameObject.Find ("coin_lbl").GetComponent <UILabel>();		
		float from = float.Parse(Txt.text);
		float to = (float)PlayerPrefs.GetInt("Coin");
		GameObject.Find ("cam").GetComponent<Cam>().TweenCoins( Txt, 1f, from, to);
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

		MainGetGamObject ("HeartLine").GetComponent <UISprite> ().fillAmount = _heart;
		MainGetGamObject ("Blod").GetComponent <UISprite> ().fillAmount = 1f - _heart;

	}
	
	public static void buy (string ProductId )
	{		
		PlayerPrefs.SetInt ("Ad", 1);	
		GameObject.Find ("Music").SendMessage ("onCoin");
		onAchivment ();
		AN_FirebaseAnalytics.LogEvent("Purchased");
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
		//Debug.Log ("onShowInterstitial");
		SX.GetComponent<SX_Ads> ().showWhenReadyBannerLoad ();
	}

	public static void onAchivment ()
	{
		SX_GameCenter GameCenter = SX.GetComponent <SX_GameCenter>();
		Debug.Log("onAchivment");
		if(GameCenter.SignedIn()){
			Debug.Log("Achivment_SignedIn");
			int lvl_now = PlayerPrefs.GetInt ("lvl");

			if (PlayerPrefs.GetInt ("good_") >= 10 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQBg")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQBg");
			else if (PlayerPrefs.GetInt ("good_") >= 25 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQGg")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQGg");
			else if (PlayerPrefs.GetInt ("good_") >= 50 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQGw")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQGw");
			else if (lvl_now >= 1 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQBQ")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQBQ");
			 else if (lvl_now >= 100 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQAA")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQAA");
			else if (lvl_now >= 200 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQAQ")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQAQ");
			else if (lvl_now >= 300 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQAg")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQAg");
			else if (lvl_now >= 400 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQAw")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQAw");
			else if (lvl_now >= 500 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQBA")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQBA");
			else if (lvl_now >= 600 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQCQ")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQCQ");
			else if (lvl_now >= 700 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQCg")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQCg");
			else if (lvl_now >= 800 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQCw")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQCw");
			else if (lvl_now >= 900 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQDA")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQDA");
			else if (lvl_now >= 1000 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQDQ")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQDQ");
			else if (lvl_now >= 2000 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQDg")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQDg");
			else if (lvl_now >= 3000 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQDw"))
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQDw");
			else if (lvl_now >= 4000 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQEA")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQEA");
			else if (lvl_now >= 5000 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQEQ")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQEQ");
			else if (lvl_now >= 6000 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQEg")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQEg");
			else if (lvl_now >= 7000 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQEw")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQEw");
			else if (lvl_now >= 8000 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQFA")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQFA");
			else if (lvl_now >= 9000 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQFQ"))
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQFQ");
			else if (lvl_now >= 10000 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQFg"))
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQFg");
			else if (PlayerPrefs.GetInt ("Coin") >= 1000 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQBw")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQBw");
			else if (PlayerPrefs.GetInt ("Coin") >= 2000 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQFw")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQFw");
			else if (PlayerPrefs.GetInt ("Coin") >= 3000 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQGA")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQGA");
			else if (PlayerPrefs.GetInt ("Coin") >= 5000 && !PlayerPrefs.HasKey ("CgkIuKLB4MEMEAIQGQ")) 
				GameCenter.unlockAchievement ("CgkIuKLB4MEMEAIQGQ");
			  

		}
	}

}
