//内容--添加脚本，用于Message模式的显示，姓名--刘旋，时间--2013-4-24
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class MessageModule : MonoBehaviour {
	ControlPanel Main;
	// Use this for initialization
	void Start () {
		Main = gameObject.GetComponent<ControlPanel>();
	
	}
	
	public void Message()
	{
		if(Main.MessageMenu)
			MessageWindow();
	}
	void MessageWindow()
	{
		if(Main.MessageFlip==0)
		{
			GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"报 警 信 息", Main.sty_Title);
			GUI.Label(new Rect(41f/1000f*Main.width,56f/1000f*Main.height,499f/1000f*Main.width,288f/1000f*Main.height),"", Main.sty_ProgSharedWindow);
			GUI.Label(new Rect(42f/1000f*Main.width,58f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"EX1016   MOTORS   OVERLOAD",Main.sty_MessAlarm);
			Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_d;
		    Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;	
		    Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;   
		    Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
		    Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(83f/1000f*Main.width,421f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"报警", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(175f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"信息", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(267f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"履历", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(347f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(429f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		}
		if(Main.MessageFlip==1)
		{
			GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"操 作 信 息", Main.sty_Title);
			GUI.Label(new Rect(41f/1000f*Main.width,56f/1000f*Main.height,499f/1000f*Main.width,288f/1000f*Main.height),"", Main.sty_ProgSharedWindow);
			GUI.Label(new Rect(513f/1000f*Main.width,63f/1000f*Main.height,23f/1000f*Main.width,23f/1000f*Main.height),"", Main.sty_EDITLabelBar_1);
		    GUI.Label(new Rect(513f/1000f*Main.width,87f/1000f*Main.height,23f/1000f*Main.width,225f/1000f*Main.height),"", Main.sty_EDITLabelBar_2);
		    GUI.Label(new Rect(513f/1000f*Main.width,313f/1000f*Main.height,23f/1000f*Main.width,23f/1000f*Main.height),"", Main.sty_EDITLabelBar_3);
			Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
		    Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_d;	
		    Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
		    Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
		    Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(83f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"报警", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(175f/1000f*Main.width,421f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"信息", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(267f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"履历", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(347f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(429f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		}
		if(Main.MessageFlip==2)
		{
			GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"报 警 履 历", Main.sty_Title);
			GUI.Label(new Rect(41f/1000f*Main.width,56f/1000f*Main.height,499f/1000f*Main.width,288f/1000f*Main.height),"", Main.sty_ProgSharedWindow);
			GUI.Label(new Rect(320f/1000f*Main.width,65f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"警 告 件 数：",Main.sty_MessRecordInfo);
			GUI.Label(new Rect(500f/1000f*Main.width,65f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"50",Main.sty_MessRecordTime);
			GUI.Label(new Rect(45f/1000f*Main.width,81f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"0001",Main.sty_MessRecordID);
			GUI.Label(new Rect(100f/1000f*Main.width,81f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"2005/02/25 13:05:15",Main.sty_MessRecordTime);
			GUI.Label(new Rect(45f/1000f*Main.width,107f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"EX1016",Main.sty_MessRecordTime);
			
			GUI.Label(new Rect(45f/1000f*Main.width,133f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"0002",Main.sty_MessRecordID);
			GUI.Label(new Rect(100f/1000f*Main.width,133f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"2005/02/25 12:57:57",Main.sty_MessRecordTime);
			GUI.Label(new Rect(45f/1000f*Main.width,159f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"OT0500",Main.sty_MessRecordTime);
			GUI.Label(new Rect(120f/1000f*Main.width,159f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"(Z)正向超程(软限位1)",Main.sty_MessRecordInfo);
			GUI.Label(new Rect(45f/1000f*Main.width,185f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"0003",Main.sty_MessRecordID);
			GUI.Label(new Rect(100f/1000f*Main.width,185f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"2005/02/25 12:57:53",Main.sty_MessRecordTime);
			GUI.Label(new Rect(45f/1000f*Main.width,211f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"OT0500",Main.sty_MessRecordTime);
			GUI.Label(new Rect(120f/1000f*Main.width,211f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"(Z)正向超程(软限位1)",Main.sty_MessRecordInfo);
			GUI.Label(new Rect(45f/1000f*Main.width,237f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"0004",Main.sty_MessRecordID);
			GUI.Label(new Rect(100f/1000f*Main.width,237f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"2005/02/25 12:44:11",Main.sty_MessRecordTime);
			GUI.Label(new Rect(45f/1000f*Main.width,263f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"SW0100",Main.sty_MessRecordTime);
			GUI.Label(new Rect(120f/1000f*Main.width,263f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"参数写入开关处于打开",Main.sty_MessRecordInfo);
			GUI.Label(new Rect(45f/1000f*Main.width,289f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"0005",Main.sty_MessRecordID);
			GUI.Label(new Rect(100f/1000f*Main.width,289f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"2005/02/24 09:08:43",Main.sty_MessRecordTime);
			GUI.Label(new Rect(45f/1000f*Main.width,315f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"SW0100",Main.sty_MessRecordTime);
			GUI.Label(new Rect(120f/1000f*Main.width,315f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"参数写入开关处于打开",Main.sty_MessRecordInfo);
			Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
		    Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;	
		    Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_d;
		    Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
		    Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(83f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"报警", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(175f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"信息", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(267f/1000f*Main.width,421f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"履历", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(347f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(429f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
