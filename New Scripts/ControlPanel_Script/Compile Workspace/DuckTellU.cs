using UnityEngine;
using System.Collections;
using System;

public class DuckTellU : MonoBehaviour {
	// Use this for initialization
	void Start () {
		RedheadDuck red_duck = new RedheadDuck();
		red_duck.PerformFly();
		red_duck.PerformQuack();
		/*
		FANUC_OI_M test_class = new FANUC_OI_M();
		Debug.Log(test_class.G_Check("G01", 8));
		Debug.Log( test_class.M_Check("M03", 10));
		Debug.Log(test_class.ErrorMessage);
		Debug.Log(test_class.G_Check("M500", 19));
		Debug.Log(test_class.ErrorMessage);
		*/
	}
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
