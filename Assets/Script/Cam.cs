using UnityEngine;
using System.Collections;
using System;
using SA.Android.Firebase.Analytics;
using SA.CrossPlatform.UI;
public class Cam : MonoBehaviour
{
	float timer = 0;
	int second = 180;
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

		Ask_Rate();
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

	 //Плавный пересчет баланса
    public void TweenCoins (UILabel Obj, float time, float from, float to) 
	{
        StopCoroutine("TweenCoin");
        StartCoroutine(TweenCoin( Obj, time, from, to));
    }

    IEnumerator TweenCoin( UILabel Obj, float time, float from, float to) 
    {
        int l = 0;
        for (float t = 0; t < time; t += Time.deltaTime) {
            float nt = Mathf.Clamp01( t / time );
            nt = Mathf.Sin(nt * Mathf.PI * 0.5f);
            int n = (int)Mathf.Lerp( from, to, nt );
            if(l!=n){
                Obj.text = (n).ToString();
                l=n;
            }
            yield return 0;
        }
		Obj.text = to.ToString();
    }	


	   //Предложение поставить оценку приложению в магазине
    public void Ask_Rate()
    {
        if (PlayerPrefs.GetInt("StoreRate")<PlayerPrefs.GetInt ("lvl")){
            PlayerPrefs.SetInt("StoreRate", PlayerPrefs.GetInt("lvl")+100);
            UM_ReviewController.RequestReview();
        }
    }
}
