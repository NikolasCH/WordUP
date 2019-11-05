using UnityEngine;
using System.Collections;

public class Language: MonoBehaviour {

	public UILabel obj_0;
	public UILabel obj_13;
	public UILabel obj_14;
	public UILabel obj_15;
	public UILabel obj_16;
	public UILabel obj_17;
	public UILabel obj_18;
	public UILabel obj_19;
	public UILabel obj_20;
	public UILabel obj_21;
	public UILabel obj_22;
	public UILabel obj_23;
	public UILabel obj_24;
	public UILabel obj_25;
	public UILabel obj_26;
	public UILabel obj_27;
	public UILabel obj_28;
	public UILabel obj_29;
	public UILabel obj_30;
	public UILabel obj_31;
	

	public static string _Language = "RU";

	public string[] lang;

	public static  string[] RU = new string[]{

		"Покупая любой пакет \n вы отключите рекламу !", //0

		"+ 1000",	 //1
		"+ 2200",	//2
		"+ 3600",		//3
		"+ 5200",	//4
		"+ 7000",		//5

		"33 руб.",		//6
		"66 руб.",		//7
		"99 руб.",	//8
		"129 руб.",	//9
		"169 руб.",	//10


		"Закрыть",	//11

		"ИГРАТЬ",	//12

		"Закрыть",	//13


		"Чем могу помочь?",	//14


		"Показать слово",	//15
		"Показать букву",	//16
		"Удалить букву",	//17
		"Купить кристаллы", //18	
		"Отлично!" //19	
	};

	public static  string[] US = new string[]{
		
		"If you want to remove ads \n you need buy any coins", //0
		
		"+ 1000",	 //1
		"+ 2200",	//2
		"+ 3600",		//3
		"+ 5200",	//4
		"+ 7000",		//5

		
		"$0.99",		//6
		"$1.99",		//7
		"$2.99",	//8
		"$3.99",	//9
		"$4.99",	//10
		
		
		"Close",	//11
		
		"Play",	//12
		
		"Close",	//13
		
		
		"Can I help you?",	//14
		
		
		"Show the word",	//15
		"Remove one letter",	//16
		"Delete one letter",	//17
		"Buy coins", //18	
		"EXCELLENT!" //19	
	};


	void Start(){

		//_Language = Application.systemLanguage.ToString();

		//Main.language = _Language;

		lang=RU;

		if(Main.language=="RU")lang=RU;
		if(Main.language=="US")lang=US;



		if(obj_0)obj_0.text = lang[0];
		if(obj_13)obj_13.text = lang[1];
		if(obj_14)obj_14.text = lang[2];
		if(obj_15)obj_15.text = lang[3];
		if(obj_16)obj_16.text = lang[4];
		if(obj_17)obj_17.text = lang[5];
		if(obj_18)obj_18.text = lang[6];
		if(obj_19)obj_19.text = lang[7];
		if(obj_20)obj_20.text = lang[8];
		if(obj_21)obj_21.text = lang[9];
		if(obj_22)obj_22.text = lang[10];
		if(obj_23)obj_23.text = lang[11];
		if(obj_24)obj_24.text = lang[12];
		if(obj_25)obj_25.text = lang[13];
		if(obj_26)obj_26.text = lang[14];
		if(obj_27)obj_27.text = lang[15];
		if(obj_28)obj_28.text = lang[16];
		if(obj_29)obj_29.text = lang[17];
		if(obj_30)obj_30.text = lang[18];
		if(obj_31)obj_31.text = lang[19];

	}

}
