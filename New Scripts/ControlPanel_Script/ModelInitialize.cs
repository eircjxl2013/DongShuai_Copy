using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelInitialize : MonoBehaviour {
	GameObject KD;
	GameObject OuterSkin1;
	GameObject X_axis1;
	GameObject Y_axis1;
	GameObject Z_axis1;
	GameObject TheRest;
	List<Transform> AllTransform = new List<Transform>();
	// Use this for initialization
	void Start () {
		KD = GameObject.Find("KD");
		foreach(Transform child in KD.transform)
			AllTransform.Add(child);
		OuterSkin1 = GameObject.Find("OuterSkin1");
		X_axis1 = GameObject.Find("X_axis1");
		Y_axis1 = GameObject.Find("Y_axis1");
		Z_axis1 = GameObject.Find("Z_axis1");
		TheRest = GameObject.Find("TheRest");
		OuterSkin1.transform.parent = KD.transform;
		X_axis1.transform.parent = KD.transform;
		Y_axis1.transform.parent = KD.transform;
		Z_axis1.transform.parent = KD.transform;
		TheRest.transform.parent = KD.transform;
		for(int i = 0; i < AllTransform.Count; i++)
		{
			if(AllTransform[i].name.StartsWith("Z axle"))
			{
				AllTransform[i].parent = Z_axis1.transform;
				AllTransform.RemoveAt(i);
				i--;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
