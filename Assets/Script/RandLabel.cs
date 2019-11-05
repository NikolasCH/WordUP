using UnityEngine;
using System.Collections;

public class RandLabel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ArrayList t = new ArrayList{"Супер!","Отлично!","Невероятно!","Круто!","Прекрасно!"};
		UILabel lbl = (UILabel) gameObject.GetComponent("UILabel");
		lbl.text = t[Random.Range(0, t.Count-1)].ToString();	
		Debug.Log(t[Random.Range(0, t.Count-1)].ToString());
	}
}
