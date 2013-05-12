using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ProgramModule : MonoBehaviour {
	ControlPanel Main;
	CooSystem CooSystem_script;
	public string[] ModeState=new string[24];//内容--定义string数组ModeState，用于存放模态代码，姓名--刘旋，时间2013-4-23
	public string[] temp_ModeState=new string[24];//内容--定义string数组temp-ModeState，用于存放新的模态代码，姓名--刘旋，时间2013-4-23
	public float ModeCursorH=0;//内容--模态的水平坐标，用于模态代码的显示位置和蓝色光标的显示位置，姓名--刘旋，时间2013-4-23
	public float ModeCursorV=0;//内容--模态的垂直坐标，用于模态代码的显示位置和蓝色光标的显示位置，姓名--刘旋，时间2013-4-23
	public bool[] light_flag=new bool[24];//内容--模态的状态，ModeState与temp-ModeState进行比较，模态代码有变动时，相应的模态的状态为真，姓名--刘旋，时间2013-4-23
	public List<int> lightup_list=new List<int>();//内容--用于存放有变动的模态的编号，姓名--刘旋，时间2013-4-23
	public int para_det;//内容--1表示当前段，0表示检测（包括绝对和相对），姓名--刘旋，时间2013-4-23
	// Use this for initialization
	void Start () {
		Main = gameObject.GetComponent<ControlPanel>();
		CooSystem_script = gameObject.GetComponent<CooSystem>();
		for(int i=0;i<24;i++)
			light_flag[i]=false;//内容--模态的状态，初始化为假，姓名--刘旋，时间2013-4-23
		//内容--模态代码的初始化，当前段中，第一列编号为0-11，第二列编号为12-23，姓名--刘旋，时间2013-4-23
		ModeState[0]="G03";ModeState[1]="G17";ModeState[2]="G90";ModeState[3]="G22";
		ModeState[4]="G94";ModeState[5]="G21";ModeState[6]="G41";ModeState[7]="G43";
		ModeState[8]="G80";ModeState[9]="G98";ModeState[10]="G50";ModeState[11]="G67";
		ModeState[12]="G97";ModeState[13]="G54";ModeState[14]="G64";ModeState[15]="G69";
		ModeState[16]="G15";ModeState[17]="G40.1";ModeState[18]="G25";ModeState[19]="G160";
		ModeState[20]="G13.1";ModeState[21]="G50.1";ModeState[22]="G54.2";ModeState[23]="G80.5";
		//内容--新模态代码的初始化，初始化为当前模态代码，姓名--刘旋，时间2013-4-23
		for(int i=0;i<24;i++)
			temp_ModeState[i]=ModeState[i];
	}
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(200f,100f,100f,60f),"ModeState"))
		{
			//内容--新模态代码中，设定几个与当前模态代码不同，用于验证程序的正确性，姓名--刘旋，时间2013-4-23
		    temp_ModeState[0]="G80";temp_ModeState[8]="G30";temp_ModeState[16]="G90";temp_ModeState[10]="G70";temp_ModeState[18]="G60";
			SetBlueCursorState();
		}
	}
	
	public void Program () 
	{
		//编辑窗口
		if(Main.ProgEDIT)
		{
			//程序
			if(Main.ProgEDITProg)
			{
				ProgEDITWindow();
			}
			//列表
			if(Main.ProgEDITList)
			{
				ProgEDITListWindow();
			}	
		}
		//自动运行
		if(Main.ProgAUTO)
		{
			ProgAUTOWindow();
		}
		//JOG或者REF
		if(Main.ProgJOG || Main.ProgREF)
		{
			ProgShared();
		}
		//内容--MDI模式，姓名--刘旋，时间--2013-4-22
		if(Main.ProgMDI)
		{
			ProgMDIWindow();
		}
		//内容--DNC模式，姓名--刘旋，时间--2013-4-22
		if(Main.ProgDNC)
		{
			ProgDNCWindow();
		}
		//内容--Handle模式，姓名--刘旋，时间--2013-4-22
		if(Main.ProgHAN)
		{
			ProgHANWindow();
		}
	}
	
	//编辑窗口
	void ProgEDITWindow () 
	{
		GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"程序", Main.sty_Title);
		GUI.Label(new Rect(45f/1000f*Main.width,70f/1000f*Main.height,465f/1000f*Main.width,25f/1000f*Main.height),"", Main.sty_EDITLabel);
		GUI.Label(new Rect(45f/1000f*Main.width,68f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"O", Main.sty_ProgEDITWindowO);
		GUI.Label(new Rect(70f/1000f*Main.width,70f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.ToolNumFormat(Main.ProgramNum), Main.sty_ProgEditProgNum);
		GUI.Label(new Rect(70f/1000f*Main.width,70f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.ToolNumFormat(Main.ProgramNum), Main.sty_ProgEditProgNum);
		GUI.Label(new Rect(375f/1000f*Main.width,70f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"（FG:编辑）", Main.sty_ProgEDITWindowFG);	
		GUI.Label(new Rect(45f/1000f*Main.width,97f/1000f*Main.height,465f/1000f*Main.width,243f/1000f*Main.height),"", Main.sty_EDITLabelWindow);
		GUI.Label(new Rect(513f/1000f*Main.width,97f/1000f*Main.height,23f/1000f*Main.width,23f/1000f*Main.height),"", Main.sty_EDITLabelBar_1);
		GUI.Label(new Rect(513f/1000f*Main.width,121f/1000f*Main.height,23f/1000f*Main.width,191f/1000f*Main.height),"", Main.sty_EDITLabelBar_2);
		GUI.Label(new Rect(513f/1000f*Main.width,313f/1000f*Main.height,23f/1000f*Main.width,23f/1000f*Main.height),"", Main.sty_EDITLabelBar_3);
		if(Main.Code01 != "")
			GUI.Label(new Rect(Main.ProgEDITCusorH, Main.ProgEDITCusorV/1000f*Main.height, Main.TextSize.x + 3f, 25f/1000f*Main.height),"", Main.sty_EDITCursor);
		GUI.Label(new Rect(46f/1000f*Main.width,100f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code01, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,125f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code02, Main.sty_Code);			
		GUI.Label(new Rect(46f/1000f*Main.width,150f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code03, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,175f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code04, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,200f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code05, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,225f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code06, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,250f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code07, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,275f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code08, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,300f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code09, Main.sty_Code);
		if(Main.ProgEDITFlip == 0)
		{
			Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_d;
			Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(78f/1000f*Main.width,421f/1000f*Main.height,70f/1000f*Main.width,25f/1000f*Main.height),"程 序", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(171f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"列 表", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(423f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"（操 作）", Main.sty_BottomChooseMenu);
		}
		else if(Main.ProgEDITFlip == 1)
		{
			Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
			GUI.Label(new Rect(72f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"BG编辑", Main.sty_BottomChooseMenu);//内容--将“后台”改为“BG”，姓名--刘旋，日期--2013-3-14
			GUI.Label(new Rect(171f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"O检索", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(261f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"检索↓", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(352f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"检索↑", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(450f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"返回", Main.sty_BottomChooseMenu);//内容--将“REWIND”改为“返回”，姓名--刘旋，日期--2013-3-14
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		}
		else if(Main.ProgEDITFlip == 2)
		{
			Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
			GUI.Label(new Rect(86f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"选择", Main.sty_BottomChooseMenu);//内容--将“F检索”改为“选择”，姓名--刘旋，日期--2013-3-14
			GUI.Label(new Rect(165f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"全选择", Main.sty_BottomChooseMenu);//内容--将“READ”改为“全选择”，姓名--刘旋，日期--2013-3-14
			//GUI.Label(new Rect(257f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"PUNCH", Main.sty_BottomChooseMenu);删除的内容，姓名--刘旋，日期--2013-3-14
			//GUI.Label(new Rect(340f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"DELETE", Main.sty_BottomChooseMenu);删除的内容，姓名--刘旋，日期--2013-3-14
			GUI.Label(new Rect(450f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"粘贴", Main.sty_BottomChooseMenu);//内容--将“EX-EDT”改为“粘贴”，姓名--刘旋，日期--2013-3-14
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		}
		else if(Main.ProgEDITFlip == 3)
		{
			Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
			//内容--以下是增加或修改的程序编辑界面下，程序菜单底部按钮的不同文字显示功能
			//姓名--刘旋
			//日期--2013-3-14
			
			GUI.Label(new Rect(86f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"取消", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(171f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"~最后", Main.sty_BottomChooseMenu);//内容--将“C-EXT”改为“~最后”，姓名--刘旋，日期--2013-3-14
			GUI.Label(new Rect(262f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"复制", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(352f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"切取", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(450f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"粘贴", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		
		}
		
		else if(Main.ProgEDITFlip == 4)
		{
			Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
			GUI.Label(new Rect(82f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"替换", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		
		}
		else if(Main.ProgEDITFlip == 5)
		{
			Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
			GUI.Label(new Rect(86f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"取消", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(261f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"复制", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(352f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"切取", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(450f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"粘贴", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
			
		}
		
		else if(Main.ProgEDITFlip == 6)
		{
			Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
			GUI.Label(new Rect(82f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"替换", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
			
		}
		else if(Main.ProgEDITFlip == 7)
		{
			Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
			GUI.Label(new Rect(66f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"BUF执行", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(158f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"指定PRG", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
			
		}//增加内容到此
		
		else if(Main.ProgEDITFlip==8)
		{
			Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
			GUI.Label(new Rect(86f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"之前", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(176f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"之后", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(267f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"跳跃", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(346f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"1-执行", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(441f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"全执行", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		}
		
		
		
		
	}
	
	//编辑界面程序列表
	void ProgEDITListWindow () 
	{
		GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"程序列表", Main.sty_Title);
		GUI.Label(new Rect(45f/1000f*Main.width,58f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height),"", Main.sty_EDITList);
		GUI.Label(new Rect(170f/1000f*Main.width,60f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height),"程序数", Main.sty_MostWords);
		GUI.Label(new Rect(370f/1000f*Main.width,60f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height),"内存(KBYTE)", Main.sty_MostWords);
		GUI.Label(new Rect(60f/1000f*Main.width,79f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height),"已用：", Main.sty_MostWords);	
		GUI.Label(new Rect(150f/1000f*Main.width,79f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.NumberFormat(Main.ProgUsedNum), Main.sty_ProgEDITListWindowNum);
		GUI.Label(new Rect(370f/1000f*Main.width,79f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.NumberFormat(Main.ProgUsedSpace), Main.sty_ProgEDITListWindowNum);
		GUI.Label(new Rect(60f/1000f*Main.width,99f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height),"空区：", Main.sty_MostWords);
		GUI.Label(new Rect(150f/1000f*Main.width,99f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.NumberFormat(Main.ProgUnusedNum), Main.sty_ProgEDITListWindowNum);
		GUI.Label(new Rect(370f/1000f*Main.width,99f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.NumberFormat(Main.ProgUnusedSpace), Main.sty_ProgEDITListWindowNum);
		GUI.Label(new Rect(45f/1000f*Main.width,127f/1000f*Main.height,490f/1000f*Main.width,213f/1000f*Main.height),"", Main.sty_EDITList);
		GUI.Label(new Rect(48f/1000f*Main.width,127f/1000f*Main.height,484f/1000f*Main.width,25f/1000f*Main.height),"", Main.sty_EDITLabel);
		GUI.Label(new Rect(48f/1000f*Main.width,127f/1000f*Main.height,484f/1000f*Main.width,25f/1000f*Main.height),"设备：CNC_MEM", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(68f/1000f*Main.width,153f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height),"O号码", Main.sty_MostWords);
		GUI.Label(new Rect(200f/1000f*Main.width,153f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height),"容量(KBYTE)", Main.sty_MostWords);
		GUI.Label(new Rect(400f/1000f*Main.width,153f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height),"更新时间", Main.sty_MostWords);
		
		
		//去掉黄色选择图标，姓名--刘旋，时间--2013-3-21
		//if(Main.CodeName01 != "")
			//GUI.Label(new Rect(48f/1000f*Main.width, Main.ProgEDITCusor/1000f*Main.height,484f/1000f*Main.width,21f/1000f*Main.height),"", Main.sty_EDITCursor);
		
		//内容--如果对程序进行选择，在被选程序前加@
		//姓名--刘旋
		//时间--2013-3-18
		if(Main.ProgEDITAt)
			GUI.Label(new Rect(48f/1000f*Main.width, (Main.ProgEDITCusor-5f)/1000f*Main.height,484f/1000f*Main.width,30f/1000f*Main.height),"@", Main.sty_ClockStyle);
			
		//增加内容到此
		
		
		GUI.Label(new Rect(68f/1000f*Main.width,174f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.CodeName[0], Main.sty_ClockStyle);
		if(Main.CodeName[0] != "")
			GUI.Label(new Rect(200f/1000f*Main.width,174f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.NumberFormat(Main.CodeSize[0]), Main.sty_ClockStyle);			
		GUI.Label(new Rect(330f/1000f*Main.width,174f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.UpdateDate[0], Main.sty_ClockStyle);
		
		GUI.Label(new Rect(68f/1000f*Main.width,194f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.CodeName[1], Main.sty_ClockStyle);
		if(Main.CodeName[1] != "")
			GUI.Label(new Rect(200f/1000f*Main.width,194f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.NumberFormat(Main.CodeSize[1]), Main.sty_ClockStyle);
		GUI.Label(new Rect(330f/1000f*Main.width,194f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.UpdateDate[1], Main.sty_ClockStyle);
		
		GUI.Label(new Rect(68f/1000f*Main.width,214f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.CodeName[2], Main.sty_ClockStyle);
		if(Main.CodeName[2] != "")
			GUI.Label(new Rect(200f/1000f*Main.width,214f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.NumberFormat(Main.CodeSize[2]), Main.sty_ClockStyle);
		GUI.Label(new Rect(330f/1000f*Main.width,214f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.UpdateDate[2], Main.sty_ClockStyle);
		
		GUI.Label(new Rect(68f/1000f*Main.width,234f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.CodeName[3], Main.sty_ClockStyle);
		if(Main.CodeName[3] != "")
			GUI.Label(new Rect(200f/1000f*Main.width,234f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.NumberFormat(Main.CodeSize[3]), Main.sty_ClockStyle);
		GUI.Label(new Rect(330f/1000f*Main.width,234f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.UpdateDate[3], Main.sty_ClockStyle);
		
		GUI.Label(new Rect(68f/1000f*Main.width,254f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.CodeName[4], Main.sty_ClockStyle);
		if(Main.CodeName[4] != "")
			GUI.Label(new Rect(200f/1000f*Main.width,254f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.NumberFormat(Main.CodeSize[4]), Main.sty_ClockStyle);
		GUI.Label(new Rect(330f/1000f*Main.width,254f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.UpdateDate[4], Main.sty_ClockStyle);
		
		GUI.Label(new Rect(68f/1000f*Main.width,274f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.CodeName[5], Main.sty_ClockStyle);
		if(Main.CodeName[5] != "")	
			GUI.Label(new Rect(200f/1000f*Main.width,274f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.NumberFormat(Main.CodeSize[5]), Main.sty_ClockStyle);
		GUI.Label(new Rect(330f/1000f*Main.width,274f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.UpdateDate[5], Main.sty_ClockStyle);
		
		GUI.Label(new Rect(68f/1000f*Main.width,294f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.CodeName[6], Main.sty_ClockStyle);
		if(Main.CodeName[6] != "")
			GUI.Label(new Rect(200f/1000f*Main.width,294f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.NumberFormat(Main.CodeSize[6]), Main.sty_ClockStyle);
		GUI.Label(new Rect(330f/1000f*Main.width,294f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.UpdateDate[6], Main.sty_ClockStyle);
		
		GUI.Label(new Rect(68f/1000f*Main.width,314f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.CodeName[7], Main.sty_ClockStyle);
		if(Main.CodeName[7] != "")
			GUI.Label(new Rect(200f/1000f*Main.width,314f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.NumberFormat(Main.CodeSize[7]), Main.sty_ClockStyle);
		GUI.Label(new Rect(330f/1000f*Main.width,314f/1000f*Main.height,490f/1000f*Main.width,65f/1000f*Main.height), Main.UpdateDate[7], Main.sty_ClockStyle);
		
		if(Main.ProgEDITFlip == 0)
		{
			Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_d;
			Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(78f/1000f*Main.width,420f/1000f*Main.height,70f/1000f*Main.width,25f/1000f*Main.height),"程 序", Main.sty_BottomChooseMenu);
			//内容--当程序超过一页时，显示“列表+”否则显示列表，姓名--刘旋，时间--2013-3-18
			if (Main.TotalListNum>8)
				GUI.Label(new Rect(171f/1000f*Main.width,421f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"列 表+", Main.sty_BottomChooseMenu);
			else //增加内容到此
			    GUI.Label(new Rect(171f/1000f*Main.width,421f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"列 表", Main.sty_BottomChooseMenu);
			
			
			GUI.Label(new Rect(423f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"（操 作）", Main.sty_BottomChooseMenu);
		}
		else if(Main.ProgEDITFlip == 1)
		{
			Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(62f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"后台编辑", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(171f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"O检索", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(261f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"检索↓", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(352f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"检索↑", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(430f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"REWIND", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		}
		else if(Main.ProgEDITFlip == 2)
		{
			Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
			GUI.Label(new Rect(80f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"F检索", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(171f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"READ", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(257f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"PUNCH", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(340f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"DELETE", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(432f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"EX-EDT", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		}
		else if(Main.ProgEDITFlip == 3)
		{
			Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
			GUI.Label(new Rect(165f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"C-EXT", Main.sty_BottomChooseMenu);
		}
	} 

	//AUTO模式下的程序界面
	void ProgAUTOWindow () 
	{
		//内容--修改AUTO模式下，程序界面的功能
		//姓名--刘旋，时间--2013-3-25
		if (Main.ProgAUTOFlip==0)
		{
			ProgramInterface();
			GUI.Label(new Rect(175f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"检测", Main.sty_BottomChooseMenu);
		}
		else if (Main.ProgAUTOFlip==1)
		{
			GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"程序", Main.sty_Title);
			GUI.Label(new Rect(45f/1000f*Main.width,90f/1000f*Main.height,490f/1000f*Main.width,245f/1000f*Main.height),"", Main.sty_ProgSharedWindow);
			
			if(Main.Code01 != "")
			    GUI.Label(new Rect(32f, Main.ProgEDITCusorV/1000f*Main.height, 480f/1000f*Main.width, 25f/1000f*Main.height),"", Main.sty_EDITCursor);
		    GUI.Label(new Rect(46f/1000f*Main.width,100f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code01, Main.sty_Code);
		    GUI.Label(new Rect(46f/1000f*Main.width,125f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code02, Main.sty_Code);			
		    GUI.Label(new Rect(46f/1000f*Main.width,150f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code03, Main.sty_Code);
		    GUI.Label(new Rect(46f/1000f*Main.width,175f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code04, Main.sty_Code);
		    GUI.Label(new Rect(46f/1000f*Main.width,200f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code05, Main.sty_Code);
		    GUI.Label(new Rect(46f/1000f*Main.width,225f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code06, Main.sty_Code);
		    GUI.Label(new Rect(46f/1000f*Main.width,250f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code07, Main.sty_Code);
		    GUI.Label(new Rect(46f/1000f*Main.width,275f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code08, Main.sty_Code);
		    GUI.Label(new Rect(46f/1000f*Main.width,300f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code09, Main.sty_Code);
			
			Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
			Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"", Main.sty_MostWords);
			GUI.Label(new Rect(75f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"BC编辑", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(170f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"O检索", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(262f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"N检索", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(451f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"返回", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		}
		else if (Main.ProgAUTOFlip==2)
		{
		    GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"程序（检查）", Main.sty_Title);
		    GUI.Label(new Rect(40f/1000f*Main.width,55f/1000f*Main.height,500f/1000f*Main.width,110f/1000f*Main.height),"", Main.sty_EDITList);
		
		    if(Main.Code01 != "")
			     GUI.Label(new Rect(46f/1000f*Main.width,60f/1000f*Main.height,484f/1000f*Main.width,26f/1000f*Main.height),"", Main.sty_EDITCursor);
		    GUI.Label(new Rect(46f/1000f*Main.width,60f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code01, Main.sty_Code);
		    GUI.Label(new Rect(46f/1000f*Main.width,85f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code02, Main.sty_Code);
		    GUI.Label(new Rect(46f/1000f*Main.width,110f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code03, Main.sty_Code);
		    GUI.Label(new Rect(46f/1000f*Main.width,135f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code04, Main.sty_Code);
	
		    GUI.Label(new Rect(40f/1000f*Main.width,165f/1000f*Main.height,150f/1000f*Main.width,113f/1000f*Main.height),"", Main.sty_EDITList);
		    GUI.Label(new Rect(191f/1000f*Main.width,165f/1000f*Main.height,145f/1000f*Main.width,113f/1000f*Main.height),"", Main.sty_EDITList);
		    GUI.Label(new Rect(70f/1000f*Main.width,165f/1000f*Main.height,100f/1000f*Main.width,300f/1000f*Main.height),"绝对坐标", Main.sty_PosSmallWord);
		    GUI.Label(new Rect(42f/1000f*Main.width,185f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_SmallXYZ);
		    GUI.Label(new Rect(50f/1000f*Main.width,185f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.x), Main.sty_SmallNum);
		    GUI.Label(new Rect(42f/1000f*Main.width,210f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_SmallXYZ);
		    GUI.Label(new Rect(50f/1000f*Main.width,210f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.y), Main.sty_SmallNum);
		    GUI.Label(new Rect(42f/1000f*Main.width,235f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_SmallXYZ);
		    GUI.Label(new Rect(50f/1000f*Main.width,235f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.z), Main.sty_SmallNum);
		    GUI.Label(new Rect(210f/1000f*Main.width,165f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"剩余移动量", Main.sty_PosSmallWord);
		    GUI.Label(new Rect(195f/1000f*Main.width,185f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"     0.000", Main.sty_SmallNum);
		    GUI.Label(new Rect(195f/1000f*Main.width,210f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"     0.000", Main.sty_SmallNum);
		    GUI.Label(new Rect(195f/1000f*Main.width,235f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"     0.000", Main.sty_SmallNum);
			para_det=2;
			BlueCursorState();//内容--蓝色光标显示，模态改变的位置上显示蓝色光标，姓名--刘旋，时间2013-4-23
			//内容--“检测”界面下，模态代码的显示，12个模态代码显示为4行3列，用ModeCursorH和ModeCursorV决定具体显示的坐标，姓名--刘旋，时间2013-4-23
			for(int i=0;i<12;i++)
		    {
			    ModeCursorH=(340f+(i/4)*60f)/1000f*Main.width;
				ModeCursorV=(165f+(i%4)*24f)/1000f*Main.height;
				GUI.Label(new Rect(ModeCursorH,ModeCursorV,500f/1000f*Main.width,300f/1000f*Main.height),ModeState[i], Main.sty_ModeCode);   
		    }			
		    GUI.Label(new Rect(340f/1000f*Main.width,262f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"H", Main.sty_MostWords);
		    GUI.Label(new Rect(420f/1000f*Main.width,262f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"M", Main.sty_MostWords);
		    GUI.Label(new Rect(40f/1000f*Main.width,280f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"T", Main.sty_MostWords);
		    GUI.Label(new Rect(340f/1000f*Main.width,280f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"D", Main.sty_MostWords);
		    GUI.Label(new Rect(40f/1000f*Main.width,301f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"F", Main.sty_MostWords);
		    GUI.Label(new Rect(210f/1000f*Main.width,301f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"S", Main.sty_MostWords);
		    GUI.Label(new Rect(40f/1000f*Main.width,322f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"实速度         MM/MIN", Main.sty_MostWords);
		    GUI.Label(new Rect(113f/1000f*Main.width,321f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.NumberFormat(Main.RunningSpeed), Main.sty_SmallNum);
		    GUI.Label(new Rect(310f/1000f*Main.width,322f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"SACT         /分", Main.sty_MostWords);
		    GUI.Label(new Rect(365f/1000f*Main.width,321f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.NumberFormat(Main.SACT), Main.sty_SmallNum);
		    Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_d;
		    Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;	
		    Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
		    Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
		    Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
		    GUI.Label(new Rect(85f/1000f*Main.width,421f/1000f*Main.height,70f/1000f*Main.width,25f/1000f*Main.height),"绝对", Main.sty_BottomChooseMenu);//内容--按下时高度值加1，姓名--刘旋，时间--2013-4-9
		    GUI.Label(new Rect(176f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"相对", Main.sty_BottomChooseMenu);
		    GUI.Label(new Rect(423f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"（操 作）", Main.sty_BottomChooseMenu);

		}
		else if (Main.ProgAUTOFlip==3)
		{
			CurrentParagraph();
			GUI.Label(new Rect(175f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"检测", Main.sty_BottomChooseMenu);
		}//修改内容到此
		else if(Main.ProgAUTOFlip==4)
		{
			GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"程序（检查）", Main.sty_Title);
		    GUI.Label(new Rect(40f/1000f*Main.width,55f/1000f*Main.height,500f/1000f*Main.width,110f/1000f*Main.height),"", Main.sty_EDITList);
		
		    if(Main.Code01 != "")
			     GUI.Label(new Rect(46f/1000f*Main.width,60f/1000f*Main.height,484f/1000f*Main.width,26f/1000f*Main.height),"", Main.sty_EDITCursor);
		    GUI.Label(new Rect(46f/1000f*Main.width,60f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code01, Main.sty_Code);
		    GUI.Label(new Rect(46f/1000f*Main.width,85f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code02, Main.sty_Code);
		    GUI.Label(new Rect(46f/1000f*Main.width,110f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code03, Main.sty_Code);
		    GUI.Label(new Rect(46f/1000f*Main.width,135f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code04, Main.sty_Code);
	
		    GUI.Label(new Rect(40f/1000f*Main.width,165f/1000f*Main.height,150f/1000f*Main.width,113f/1000f*Main.height),"", Main.sty_EDITList);
		    GUI.Label(new Rect(191f/1000f*Main.width,165f/1000f*Main.height,145f/1000f*Main.width,113f/1000f*Main.height),"", Main.sty_EDITList);
		    GUI.Label(new Rect(70f/1000f*Main.width,165f/1000f*Main.height,100f/1000f*Main.width,300f/1000f*Main.height),"相对坐标", Main.sty_PosSmallWord);
		    GUI.Label(new Rect(42f/1000f*Main.width,185f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_SmallXYZ);
		    GUI.Label(new Rect(50f/1000f*Main.width,185f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.relative_pos.x), Main.sty_SmallNum);
		    GUI.Label(new Rect(42f/1000f*Main.width,210f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_SmallXYZ);
		    GUI.Label(new Rect(50f/1000f*Main.width,210f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.relative_pos.y), Main.sty_SmallNum);
		    GUI.Label(new Rect(42f/1000f*Main.width,235f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_SmallXYZ);
		    GUI.Label(new Rect(50f/1000f*Main.width,235f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.relative_pos.z), Main.sty_SmallNum);
		    GUI.Label(new Rect(210f/1000f*Main.width,165f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"剩余移动量", Main.sty_PosSmallWord);
		    GUI.Label(new Rect(195f/1000f*Main.width,185f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"     0.000", Main.sty_SmallNum);
		    GUI.Label(new Rect(195f/1000f*Main.width,210f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"     0.000", Main.sty_SmallNum);
		    GUI.Label(new Rect(195f/1000f*Main.width,235f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"     0.000", Main.sty_SmallNum);
			para_det=2;
			BlueCursorState();//内容--蓝色光标的显示，在模态改变的位置上显示蓝色光标，姓名--刘旋，时间--2013-4-23
			//内容--“检测”界面下，模态代码的显示，12个模态代码显示为4行3列，用ModeCursorH和ModeCursorV决定具体显示的坐标，姓名--刘旋，时间2013-4-23
			for(int i=0;i<12;i++)
		    {
			    ModeCursorH=(340f+(i/4)*60f)/1000f*Main.width;
				ModeCursorV=(165f+(i%4)*24f)/1000f*Main.height;
				GUI.Label(new Rect(ModeCursorH,ModeCursorV,500f/1000f*Main.width,300f/1000f*Main.height),ModeState[i], Main.sty_ModeCode);   
		    }				
		    GUI.Label(new Rect(340f/1000f*Main.width,262f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"H", Main.sty_MostWords);
		    GUI.Label(new Rect(420f/1000f*Main.width,262f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"M", Main.sty_MostWords);
		    GUI.Label(new Rect(40f/1000f*Main.width,280f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"T", Main.sty_MostWords);
		    GUI.Label(new Rect(340f/1000f*Main.width,280f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"D", Main.sty_MostWords);
		    GUI.Label(new Rect(40f/1000f*Main.width,301f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"F", Main.sty_MostWords);
		    GUI.Label(new Rect(210f/1000f*Main.width,301f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"S", Main.sty_MostWords);
		    GUI.Label(new Rect(40f/1000f*Main.width,322f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"实速度         MM/MIN", Main.sty_MostWords);
		    GUI.Label(new Rect(113f/1000f*Main.width,321f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.NumberFormat(Main.RunningSpeed), Main.sty_SmallNum);
		    GUI.Label(new Rect(310f/1000f*Main.width,322f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"SACT         /分", Main.sty_MostWords);
		    GUI.Label(new Rect(365f/1000f*Main.width,321f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.NumberFormat(Main.SACT), Main.sty_SmallNum);
		    Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
		    Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_d;	
		    Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
		    Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
		    Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
			GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
		    GUI.Label(new Rect(85f/1000f*Main.width,420f/1000f*Main.height,70f/1000f*Main.width,25f/1000f*Main.height),"绝对", Main.sty_BottomChooseMenu);//内容--按下时高度值加1，姓名--刘旋，时间--2013-4-9
		    GUI.Label(new Rect(176f/1000f*Main.width,421f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"相对", Main.sty_BottomChooseMenu);
		    GUI.Label(new Rect(423f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"（操 作）", Main.sty_BottomChooseMenu);
		}
		else if(Main.ProgAUTOFlip==5)
		{
			NextParagraph();
			GUI.Label(new Rect(175f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"检测", Main.sty_BottomChooseMenu);
			GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		}
	}
	
	//显示Handle、Jog、Ref模式下的程序界面
	void ProgShared () 
	{
		if(Main.ProgSharedFlip==0)
		{
			ProgramInterface();
		}
		if(Main.ProgSharedFlip==1)
		{
			CurrentParagraph();
		}
		if(Main.ProgSharedFlip==2)
		{
			NextParagraph();
		}
	}
	
	void ProgMDIWindow()
	{
		if(Main.ProgMDIFlip==0)
		{
			ModeEditInterface();
			GUI.Label(new Rect(40f/1000f*Main.width,30f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"程序(MDI)", Main.sty_BottomAST);
			GUI.Label(new Rect(175f/1000f*Main.width,421f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"MDI", Main.sty_BottomChooseMenu);
		}
		if(Main.ProgMDIFlip==1)
		{
			ProgramInterface();
			GUI.Label(new Rect(175f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"MDI", Main.sty_BottomChooseMenu);
		}
		if(Main.ProgMDIFlip==2)
		{
			CurrentParagraph();
			GUI.Label(new Rect(175f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"MDI", Main.sty_BottomChooseMenu);
		}
		if(Main.ProgMDIFlip==3)
		{
			NextParagraph();
			GUI.Label(new Rect(175f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"MDI", Main.sty_BottomChooseMenu);
		}
	}
	
	void ProgDNCWindow()
	{
		if(Main.ProgDNCFlip==0)
		{
			ProgramInterface();
		}
		if(Main.ProgDNCFlip==1)
		{
			CurrentParagraph();
		}
		if(Main.ProgDNCFlip==2)
		{
			NextParagraph();
		}
	}
	
	void ProgHANWindow()
	{
		if(Main.ProgHANFlip==0)
		{
			ProgramInterface();
		}
		if(Main.ProgHANFlip==1)
		{
			CurrentParagraph();
		}
		if(Main.ProgHANFlip==2)
		{
			NextParagraph();
		}
	}
	
	void ModeEditInterface()
	{
		GUI.Label(new Rect(45f/1000f*Main.width,70f/1000f*Main.height,465f/1000f*Main.width,243f/1000f*Main.height),"", Main.sty_EDITLabelWindow);
		GUI.Label(new Rect(513f/1000f*Main.width,70f/1000f*Main.height,23f/1000f*Main.width,23f/1000f*Main.height),"", Main.sty_EDITLabelBar_1);
		GUI.Label(new Rect(513f/1000f*Main.width,94f/1000f*Main.height,23f/1000f*Main.width,191f/1000f*Main.height),"", Main.sty_EDITLabelBar_2);
		GUI.Label(new Rect(513f/1000f*Main.width,286f/1000f*Main.height,23f/1000f*Main.width,23f/1000f*Main.height),"", Main.sty_EDITLabelBar_3);
		GUI.Label(new Rect(40f/1000f*Main.width,322f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"实速度         MM/MIN", Main.sty_MostWords);
		GUI.Label(new Rect(113f/1000f*Main.width,321f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.NumberFormat(Main.RunningSpeed), Main.sty_SmallNum);
		GUI.Label(new Rect(310f/1000f*Main.width,322f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"SACT         /分", Main.sty_MostWords);
		GUI.Label(new Rect(365f/1000f*Main.width,321f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.NumberFormat(Main.SACT), Main.sty_SmallNum);
		if(Main.Code01 != "")
			GUI.Label(new Rect(Main.ProgEDITCusorH, Main.ProgEDITCusorV/1000f*Main.height-25f/1000f*Main.height, Main.TextSize.x + 3f, 25f/1000f*Main.height),"", Main.sty_EDITCursor);
		GUI.Label(new Rect(46f/1000f*Main.width,75f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code01, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,100f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code02, Main.sty_Code);			
		GUI.Label(new Rect(46f/1000f*Main.width,125f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code03, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,150f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code04, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,175f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code05, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,200f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code06, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,225f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code07, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,250f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code08, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,275f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code09, Main.sty_Code);
		Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_d;	
		Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
		
		GUI.Label(new Rect(83f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"程序", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(257f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"当前段", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(347f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"下一段", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(429f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"（操作）", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
	}
	
	void ProgramInterface()
	{
		GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"程序", Main.sty_Title);
		GUI.Label(new Rect(45f/1000f*Main.width,90f/1000f*Main.height,490f/1000f*Main.width,245f/1000f*Main.height),"", Main.sty_ProgSharedWindow);
		
		if(Main.Code01 != "")
			GUI.Label(new Rect(32f, Main.ProgEDITCusorV/1000f*Main.height, 480f/1000f*Main.width, 25f/1000f*Main.height),"", Main.sty_EDITCursor);
		GUI.Label(new Rect(46f/1000f*Main.width,100f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code01, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,125f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code02, Main.sty_Code);			
		GUI.Label(new Rect(46f/1000f*Main.width,150f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code03, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,175f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code04, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,200f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code05, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,225f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code06, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,250f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code07, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,275f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code08, Main.sty_Code);
		GUI.Label(new Rect(46f/1000f*Main.width,300f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.Code09, Main.sty_Code);
		Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_d;
		Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;	
		Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
		//GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
		GUI.Label(new Rect(83f/1000f*Main.width,421f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"程序", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(175f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(257f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"当前段", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(347f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"下一段", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(429f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"（操作）", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
	}
	
	void CurrentParagraph()
	{
		GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"程序", Main.sty_Title);
		GUI.Label(new Rect(40f/1000f*Main.width,60f/1000f*Main.height,249f/1000f*Main.width,285f/1000f*Main.height),"", Main.sty_EDITList);
		GUI.Label(new Rect(291f/1000f*Main.width,60f/1000f*Main.height,249f/1000f*Main.width,285f/1000f*Main.height),"", Main.sty_EDITList);
		GUI.Label(new Rect(40f/1000f*Main.width,60f/1000f*Main.height,249f/1000f*Main.width,25f/1000f*Main.height),"", Main.sty_TopLabel);
		GUI.Label(new Rect(291f/1000f*Main.width,60f/1000f*Main.height,249f/1000f*Main.width,25f/1000f*Main.height),"", Main.sty_TopLabel);
		GUI.Label(new Rect(130f/1000f*Main.width,58f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"当前段", Main.sty_Title);
		GUI.Label(new Rect(386f/1000f*Main.width,58f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"模态", Main.sty_Title);
		
		GUI.Label(new Rect(42f/1000f*Main.width,88f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), "G00", Main.sty_ModeCode);
		GUI.Label(new Rect(42f/1000f*Main.width,113f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), "G90", Main.sty_ModeCode);
		GUI.Label(new Rect(108f/1000f*Main.width,88f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_SmallXYZ);
		GUI.Label(new Rect(128f/1000f*Main.width,88f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.x), Main.sty_SmallNum);
		GUI.Label(new Rect(108f/1000f*Main.width,113f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_SmallXYZ);
		GUI.Label(new Rect(128f/1000f*Main.width,113f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.y), Main.sty_SmallNum);
		GUI.Label(new Rect(108f/1000f*Main.width,138f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_SmallXYZ);
		GUI.Label(new Rect(128f/1000f*Main.width,138f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.z), Main.sty_SmallNum);
		para_det=1;
		BlueCursorState();//内容--蓝色光标的显示，在模态发生改变的位置上显示蓝色光标，姓名--刘旋，时间--2013-4-23
		//内容--“当前段”界面下，模态代码的显示，24个模态代码显示为12行2列，用ModeCursorH和ModeCursorV决定具体显示的坐标，姓名--刘旋，时间2013-4-23
		for(int i=0;i<24;i++)
		{
			ModeCursorH=(292f+i/12*70f)/1000f*Main.width;
			ModeCursorV=(85f+i%12*21f)/1000f*Main.height;
			GUI.Label(new Rect(ModeCursorH,ModeCursorV,500f/1000f*Main.width,300f/1000f*Main.height),ModeState[i], Main.sty_ModeCode);
		}
		GUI.Label(new Rect(446f/1000f*Main.width,85f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), "F", Main.sty_Mode);
		GUI.Label(new Rect(446f/1000f*Main.width,106f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), "M", Main.sty_Mode);
		GUI.Label(new Rect(446f/1000f*Main.width,169f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), "H", Main.sty_Mode);
		GUI.Label(new Rect(446f/1000f*Main.width,211f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), "T", Main.sty_Mode);
		GUI.Label(new Rect(446f/1000f*Main.width,236f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), "S", Main.sty_Mode);
		GUI.Label(new Rect(496f/1000f*Main.width,169f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), "D", Main.sty_Mode);
		
		Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;	
		Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_d;
		Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
		//GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
		GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"", Main.sty_MostWords);
		GUI.Label(new Rect(83f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"程序", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(175f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(257f/1000f*Main.width,421f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"当前段", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(347f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"下一段", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(429f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"（操作）", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
	}
	
	void NextParagraph()
	{
		GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"程序", Main.sty_Title);
		GUI.Label(new Rect(40f/1000f*Main.width,60f/1000f*Main.height,249f/1000f*Main.width,285f/1000f*Main.height),"", Main.sty_EDITList);
		GUI.Label(new Rect(291f/1000f*Main.width,60f/1000f*Main.height,249f/1000f*Main.width,285f/1000f*Main.height),"", Main.sty_EDITList);
		GUI.Label(new Rect(40f/1000f*Main.width,60f/1000f*Main.height,249f/1000f*Main.width,25f/1000f*Main.height),"", Main.sty_TopLabel);
		GUI.Label(new Rect(291f/1000f*Main.width,60f/1000f*Main.height,249f/1000f*Main.width,25f/1000f*Main.height),"", Main.sty_TopLabel);
		GUI.Label(new Rect(130f/1000f*Main.width,58f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"当前段", Main.sty_Title);
		GUI.Label(new Rect(386f/1000f*Main.width,58f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"下一段", Main.sty_Title);
		
		Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;	
		Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_d;
		Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
		//GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"<", Main.sty_MostWords);
		GUI.Label(new Rect(44f/1000f*Main.width,423f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"", Main.sty_MostWords);
		GUI.Label(new Rect(83f/1000f*Main.width,420f/1000f*Main.height,100f/1000f*Main.width,25f/1000f*Main.height),"程序", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(175f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(257f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"当前段", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(347f/1000f*Main.width,421f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"下一段", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(429f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"（操作）", Main.sty_BottomChooseMenu);
	}
	
	//内容--定义函数，设定模态的状态，当前模态代码与新的模态代码进行比较，如果不同，相应的代码编号存放在lightup-list里，并将新的模态代码赋给当前模态代码
	//利用lightup-list将light-flag里相应的模态状态为设为真
	//姓名--刘旋，时间2013-4-23
	public void SetBlueCursorState()
	{
		for(int i=0;i<24;i++)
		{
			if(ModeState[i]!=temp_ModeState[i])//内容--新模态代码与当前模态代码进行比较，姓名--刘旋，时间2013-4-23
			{
				lightup_list.Add(i);
			    ModeState[i]=temp_ModeState[i];
			}
			light_flag[i]=false;//内容--light-flag设定前，初始化为假，姓名--刘旋，时间2013-4-23
		}
		for(int i=0;i<lightup_list.Count;i++)
			light_flag[lightup_list[i]]=true;
		lightup_list.Clear();//内容--lightup-list清空，姓名--刘旋，时间2013-4-23
	}
	//内容--定义函数，显示蓝色光标，1-表示当前段，2-表示检测，姓名--刘旋，时间--2013-4-23
	public void BlueCursorState()
	{
		switch (para_det)
		{
		case 1:
		    for(int i=0;i<24;i++)
		    {
			    ModeCursorH=(292f+i/12*70f)/1000f*Main.width;
			    ModeCursorV=(85f+i%12*21f)/1000f*Main.height;
				if(light_flag[i])
				    GUI.Label(new Rect(ModeCursorH,ModeCursorV,60f/1000f*Main.width,21f/1000f*Main.height),"", Main.sty_EDITLabel);
		    }
			break;
		case 2:
			for(int i=0;i<12;i++)
		    {
			    ModeCursorH=(340f+i/4*60f)/1000f*Main.width;
				ModeCursorV=(165f+i%4*24f)/1000f*Main.height;
				if(light_flag[i])
				    GUI.Label(new Rect(ModeCursorH,ModeCursorV,60f/1000f*Main.width,24f/1000f*Main.height),"", Main.sty_EDITLabel);   
		    }
			break;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
