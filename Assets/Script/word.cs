using UnityEngine;
using System.Collections;
using SA.Android.Firebase.Analytics;
public class word : Main
{

	public UIAtlas _atlas;
	public Font _font;
	GameObject _goButton;
	GameObject center;
	public AnimationCurve anim;

	public GameObject[] Objects;
	float _heart = 0;


	void Start ()
	{
		GameOwer = false;
		OBJECTS = Objects;
		if (MainGetGamObject ("Music"))
			MainGetGamObject ("Music").SendMessage ("onNew");

		_heart = (float)((float)PlayerPrefs.GetInt ("Heart") / 10f);

		Play ();
	}

	void FixedUpdate ()
	{
		if (_heart != heart) {
			if (_heart < heart) {
				_heart += Time.deltaTime / 6;
				if (_heart >= heart)
					_heart = heart;
			} else {
				_heart -= Time.deltaTime / 6;
				if (_heart <= heart)
					_heart = heart;
			}
			HeartUp (_heart);
		}
	}



	public void OpenBtn (int i)
	{
		if (PlayerPrefs.GetInt ("OpenPrice") < 1 || (PlayerPrefs.GetInt ("Coin") >= int.Parse (MainGetGamObject ("p" + (i)).GetComponent <UILabel> ().text) && PlayerPrefs.GetInt ("s" + (i)) != 1)) {		

			if (PlayerPrefs.GetInt ("OpenPrice") > 0) {
				PlayerPrefs.SetInt ("Coin", PlayerPrefs.GetInt ("Coin") - int.Parse (MainGetGamObject ("p" + (i)).GetComponent <UILabel> ().text));
				CoinUp ();
			} else
				PlayerPrefs.SetInt ("OpenPrice", PlayerPrefs.GetInt ("OpenPrice") + 1);

			PlayerPrefs.SetInt ("s" + (i), 1);

			check_words ();

			if (MainGetGamObject ("Music"))
				MainGetGamObject ("Music").SendMessage ("onCoins");
		} else 		
			Coin_Panel ();
		
	}

	public void CheckOpenWord (int i)
	{
		if (PlayerPrefs.GetInt ("Heart") > 0 && PlayerPrefs.GetInt ("wb" + (i)) != 1) {	
			PlayerPrefs.SetInt ("wb" + (i), 1);

			if (!check_word (basa (PlayerPrefs.GetInt ("lvl"), i))) {
				bad_wait ();

				PlayerPrefs.SetInt ("Heart", PlayerPrefs.GetInt ("Heart") - 1);
				if (PlayerPrefs.GetInt ("Heart") == 0)
					PlayerPrefs.SetInt ("Hearts", PlayerPrefs.GetInt ("Hearts") + 1);
				MainGetGamObject ("Heart").GetComponent<TweenScale> ().ResetToBeginning ();
				MainGetGamObject ("Heart").GetComponent<TweenScale> ().enabled = true;
				heart = (float)((float)PlayerPrefs.GetInt ("Heart") / 10f);
				MainGetGamObject ("s" + (i)).GetComponent<TweenScale> ().ResetToBeginning ();
				MainGetGamObject ("s" + (i)).GetComponent<TweenScale> ().delay = 0;
				check_words ();

			} else {

				good_wait (MainGetGamObject ("w" + (i)));
				Destroy (MainGetGamObject ("s" + (i)).GetComponent<BoxCollider> ());
				Destroy (MainGetGamObject ("wb" + (i)).GetComponent<BoxCollider> ());
				MainGetGamObject ("wb" + (i)).GetComponent <UISprite> ().spriteName = "pravilnoe slovo";

				MainGetGamObject ("s" + (i)).GetComponent <UISprite> ().spriteName = "+ kristal";
				MainGetGamObject ("s" + (i)).GetComponent<TweenScale> ().ResetToBeginning ();
				MainGetGamObject ("s" + (i)).GetComponent<TweenScale> ().delay = 0;
				MainGetGamObject ("s" + (i)).GetComponent<TweenScale> ().enabled = true;
				MainGetGamObject ("w" + (i)).GetComponent <UILabel> ().text = basa (PlayerPrefs.GetInt ("lvl"), i);
				MainGetGamObject ("p" + (i)).transform.localScale = Vector3.zero;

			}


		} else {
			Heart_Panel ();
		}
	}
	

	public void good_wait (GameObject obj)
	{
		PlayerPrefs.SetInt ( "Add_Heart", PlayerPrefs.GetInt ("Add_Heart")+1);
		Fill_Heart(((float)PlayerPrefs.GetInt ("Add_Heart")-1f)/5f);//MainGetGamObject ("Add_Heart").GetComponent<UISprite>().fillAmount);
		GameOwer = true;
		GameObject tObj = MainGetGamObject ("CompleteWord");
		tObj.transform.parent = MainGetGamObject ("obj").transform;
		tObj.GetComponent <UILabel> ().text = word;	
		Destroy (obj.GetComponent <UILabel> ());	
		tObj.transform.position = obj.transform.position;
		tObj.GetComponent<TweenPosition> ().from = tObj.transform.localPosition;
		tObj.GetComponent<TweenPosition> ().enabled = true;
		MainGetGamObject ("pic_menu").AddComponent<TweenAlpha> ().to = 0;
		MainGetGamObject ("pic_menu").GetComponent<TweenAlpha> ().duration = 0.3f;
		MainGetGamObject ("pic_menu").GetComponent<TweenAlpha> ().delay = 0.5f;
		if (MainGetGamObject ("Music"))
			MainGetGamObject ("Music").SendMessage ("onWin");
		Invoke ("good", 1.3f);
	}

	public void bad_wait ()
	{

		AN_FirebaseAnalytics.LogEvent("Dad_word");
		if (MainGetGamObject ("Music"))
			MainGetGamObject ("Music").SendMessage ("onWrong");
	}

	public  void good ()
	{	
		MainGetGamObject ("bg").SetActive (true);
		AN_FirebaseAnalytics.LogEvent("Next_Level");

		int reward = 10;

		MainGetGamObject ("heart_img").GetComponent<UISprite>().enabled = true;

		if (PlayerPrefs.GetInt ("Heart") < 10){
			
			if(PlayerPrefs.GetInt ("Add_Heart")>=5){
				PlayerPrefs.SetInt ("Add_Heart",0);
				PlayerPrefs.SetInt ("Heart", PlayerPrefs.GetInt ("Heart") + 1);
				MainGetGamObject ("heart_img").GetComponent<UISprite>().spriteName ="+jizn";
			}else
				MainGetGamObject ("heart_img").GetComponent<UISprite>().enabled = false;

			MainGetGamObject ("coin_b").GetComponent<UILabel>().text ="+"+ reward;
		}else{
			MainGetGamObject ("heart_img").GetComponent<UISprite>().spriteName ="_1";
			reward *= PlayerPrefs.GetInt ("OpenWord")==1 ? 2 : 3;
			MainGetGamObject ("coin_b").GetComponent<UILabel>().text ="+"+ reward;
		}


		MainGetGamObject ("Heart").GetComponent<TweenScale> ().ResetToBeginning ();
		MainGetGamObject ("Heart").GetComponent<TweenScale> ().enabled = true;
		heart = (float)((float)PlayerPrefs.GetInt ("Heart") / 10f);

		int tips = 0;	
		for (int i=0; i<6; i++)
			if (PlayerPrefs.GetInt ("wb" + (i)) > 0)
				tips++;

		if (tips == 1)
			PlayerPrefs.SetInt ("good_", PlayerPrefs.GetInt ("good_") + 1);
		else
			PlayerPrefs.SetInt ("good_", 0);

		Debug.Log(PlayerPrefs.GetInt ("good_"));


		PlayerPrefs.SetInt ("OpenPrice", 0);
		PlayerPrefs.SetInt ("OpenLetter", 0);
		PlayerPrefs.SetInt ("Hearts", 0);
		PlayerPrefs.SetInt ("timer", 0);
		PlayerPrefs.SetInt ("lvl", PlayerPrefs.GetInt ("lvl") + 1);
		PlayerPrefs.SetInt ("Coin", PlayerPrefs.GetInt ("Coin") + reward);

		for (int i=0; i<6; i++)
			PlayerPrefs.SetInt ("s" + (i), 0);
		for (int i=0; i<6; i++)
			PlayerPrefs.SetString ("w" + (i), basa (PlayerPrefs.GetInt ("lvl"), i));
		for (int i=0; i<6; i++)
			PlayerPrefs.SetInt ("wb" + (i), 0);


		LevelUp ();
		CoinUp ();
		




		MainGetGamObject ("CompleteWord").GetComponent <UILabel> ().text = word;		

		postScoreToLeaderBoard (PlayerPrefs.GetInt ("lvl"));		
		onAchivment ();
	}
}


