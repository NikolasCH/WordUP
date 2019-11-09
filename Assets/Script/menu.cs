using UnityEngine;
using System.Collections;

public class menu : Main
{
	// Use this for initialization
	void Start ()
	{

		//PlayerPrefs.DeleteAll();
		SX = GameObject.Find ("SX");

		if (!PlayerPrefs.HasKey ("Install"))
			install ();
		
		Main.heart = (float)((float)PlayerPrefs.GetInt ("Heart") / 10f);
		Main.HeartUp (Main.heart);

		LevelUp ();
		CoinUp ();
	}

	public static void install ()
	{	
		PlayerPrefs.SetInt ("Install", 1);
		PlayerPrefs.SetInt ("lvl", 1);
		for (int i=0; i<6; i++)
			PlayerPrefs.SetInt ("s" + (i), 0);
		for (int i=0; i<6; i++)
			PlayerPrefs.SetString ("w" + (i), basa (PlayerPrefs.GetInt ("lvl"), i));
		for (int i=0; i<6; i++)
			PlayerPrefs.SetInt ("wb" + (i), 0);
		
		PlayerPrefs.SetInt ("Heart", 10);
		PlayerPrefs.SetInt ("Hearts", 0);
		PlayerPrefs.SetInt ("OpenLetter", 0);
		PlayerPrefs.SetInt ("OpenPrice", 0);
		PlayerPrefs.SetInt ("Coin", 500);
		PlayerPrefs.SetInt ("HIGHSCORE", 0);	
		PlayerPrefs.SetInt ("Ad", 0);
	}
}
