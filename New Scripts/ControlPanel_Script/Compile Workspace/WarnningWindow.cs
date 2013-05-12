using UnityEngine;
using System.Collections;

public class WarnningWindow : MonoBehaviour {
	private Rect win_rect = new Rect(-500f, Screen.height - 250f, 300f, 250f);
	private float left = -500f; 
	private bool come_forth = false;
	private bool motion_start = false;
	private float time_value = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	void OnGUI () 
	{
		win_rect.x = left;
		win_rect = GUI.Window(11, win_rect, Warnning, "Warnning Window");
		
		if(GUI.Button(new Rect(100f, Screen.height - 290f, 100f, 30f), "触发"))
		{
			motion_start = true;
		}
	}
	
	void Warnning(int WindowID)
	{
		
	}
	
	void FixedUpdate ()
	{
		if(motion_start)
		{
			//出来
			if(come_forth)
			{
				time_value += Time.fixedDeltaTime;
				left = Mathf.Lerp(0, -500f, 2*time_value);
				if(2*time_value > 1f)
				{
					time_value = 0; 
					come_forth = !come_forth;
					motion_start = false;
				}
			}
			//进去
			else
			{
				time_value += Time.fixedDeltaTime;
				left = Mathf.Lerp(-500f, 0, 2*time_value);
				if(2*time_value > 1f)
				{
					time_value = 0; 
					come_forth = !come_forth;
					motion_start = false;
				}
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
