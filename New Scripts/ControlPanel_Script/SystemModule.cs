//内容--添加脚本，用于System模式的显示，姓名--刘旋，时间--2013-4-24
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
public class SystemModule : MonoBehaviour {
	ControlPanel Main;
	float CursorH=0;
	float CursorV=0;
	// Use this for initialization
	void Start () {
		Main = gameObject.GetComponent<ControlPanel>();
	
	}
	public void System()
	{
		if(Main.SystemMenu)
			SystemWindow();
	}
	void SystemWindow()
	{
		if(Main.SystemFlip==0)
		{
		    GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"参 数", Main.sty_Title);
			GUI.Label(new Rect(41f/1000f*Main.width,56f/1000f*Main.height,499f/1000f*Main.width,288f/1000f*Main.height),"", Main.sty_ProgSharedWindow);
			
			GUI.Label(new Rect(50f/1000f*Main.width,65f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"设定", Main.sty_Title);
			GUI.Label(new Rect(45f/1000f*Main.width,90f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"0000", Main.sty_SysID);
			
			GUI.Label(new Rect(45f/1000f*Main.width,140f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"0001", Main.sty_SysID);
			
			GUI.Label(new Rect(45f/1000f*Main.width,190f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"0002", Main.sty_SysID);
			GUI.Label(new Rect(45f/1000f*Main.width,240f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"0010", Main.sty_SysID);
			
			for(int i=0;i<32;i++)
			{
				CursorH=(110f+i%8*45f)/1000f*Main.width;
				CursorV=(110f+i/8*50f)/1000f*Main.height;
				GUI.Label(new Rect(CursorH,CursorV,19f/1000f*Main.width,25f/1000f*Main.height),"", Main.sty_OffSet_Coo);
				if(i!=6)
				GUI.Label(new Rect(CursorH+3f/1000f*Main.width,CursorV+1f/1000f*Main.width,19f/1000f*Main.width,25f/1000f*Main.height),"0", Main.sty_SysID);
			}
			GUI.Label(new Rect(113/1000f*Main.width,113f/1000f*Main.height,14f/1000f*Main.width,20f/1000f*Main.height),"", Main.sty_EDITCursor);
			GUI.Label(new Rect(383/1000f*Main.width,111f/1000f*Main.height,19f/1000f*Main.width,25f/1000f*Main.height),"1", Main.sty_SysID);
			GUI.Label(new Rect(113/1000f*Main.width,111f/1000f*Main.height,19f/1000f*Main.width,25f/1000f*Main.height),"0", Main.sty_SysID);	
			GUI.Label(new Rect(192/1000f*Main.width,90f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"SEQ", Main.sty_SysInfo);	
			GUI.Label(new Rect(327/1000f*Main.width,90f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"INI ISO TVC", Main.sty_SysInfo);
			GUI.Label(new Rect(372/1000f*Main.width,140f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"FCV", Main.sty_SysInfo);
			GUI.Label(new Rect(102/1000f*Main.width,190f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"SJZ", Main.sty_SysInfo);
			GUI.Label(new Rect(327/1000f*Main.width,240f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"PEC PRM PZS", Main.sty_SysInfo);
			
			
			Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_d;
		    Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;	
		    Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
		    Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
		    Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
			GUI.Label(new Rect(75f/1000f*Main.width,421f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"号搜索", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(172f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"ON:1", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(256f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"OFF:0", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(351f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+输入", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(450f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"输入", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
