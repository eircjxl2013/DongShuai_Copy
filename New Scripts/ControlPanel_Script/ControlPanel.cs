using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ControlPanel : MonoBehaviour { 
	
	#region Defined script variable
	PositionModule Position_Script;
	SoftkeyModule Softkey_Script;
	ProgramModule Program_Script;
	OffsetSettingModule Offset_Script;
	MDIInputModule MDIInput_Script;
	MDIFunctionModule MDIFunction_Script;
	MDIEditModule MDIEdit_Script;
	ModeSelectModule ModeSelect_Script;
	FeedrateModule Feedrate_Script;
	MachineFunctionModule MachineFunction_Script;
	AuxiliaryFunctionModule AuxiliaryFunction_Script;
	MoveControl MoveControl_script;
	SpindleControl SpindleControl_script;
	CooSystem CooSystem_script;
	CompileNC CompileNC_script;
	AuxiliaryMoveModule AuxiliaryMove_Script;
	SystemModule System_Script;//添加脚本SystemModule和MessageModule，姓名--刘旋，时间--2013-4-24
	MessageModule Message_Script;
	#endregion
	
	#region Defined variable
	public float width = 670;
	public float height = 650;
	public bool RapidMoveFlag = false;
	//内容--定义布尔变量，用于指示JOG模式下F0，25%，50%，100%四个按钮的功能
	//姓名--刘旋，时间--2013-4-8
	public bool F0_flag=false;
	public bool F25_flag=false;
	public bool F50_flag=false;//默认50%按钮时按下状态
	public bool F100_flag=false;//增加内容到此 2013-4-8
	//内容--定义整形变量SlowSpeedMode，用于指示慢常速下的按键状态，SlowSpeedMode=0，表示F0按下，SlowSpeedMode=1，表示25%按下
	//SlowSpeedMode=2，表示50%按下，SlowSpeedMode=3，表示100%按下，姓名--刘旋，时间--2013-4-8
	public int RapidSpeedMode=2;//增加内容到此  2013-4-8
	public Rect PanelWindowRect = new Rect(-300, 20, 670, 650);   
	public float timeV = 0;
	
	public Texture2D t2d_alarm;
	public Texture2D t2d_zero;
	public Texture2D t2d_toolnum;
	
	
	public Texture2D t2d_Emergency;
	public Texture2D t2d_em_u;
	public Texture2D t2d_em_d;
	
	public Texture2D t2d_Protect;
	public Texture2D t2d_lock;
	public Texture2D t2d_unlock;
	
	public Texture2D t2d_ModeSelect;
	public Texture2D t2d_ModeSelectEDIT;
	public Texture2D t2d_ModeSelectDNC;
	public Texture2D t2d_ModeSelectAUTO;
	public Texture2D t2d_ModeSelectMDI;
	public Texture2D t2d_ModeSelectHANDLE;
	public Texture2D t2d_ModeSelectJOG;
	public Texture2D t2d_ModeSelectREF;
	public int mode_type = 0;
	
	public Texture2D t2d_NCPower_on_u;
	public Texture2D t2d_NCPower_on_d;
	public Texture2D t2d_NCPower_off_u;
	public Texture2D t2d_NCPower_off_d;
	
	public Texture2D t2d_feedrate;
	public Texture2D t2d_FeedRate_0;
	public Texture2D t2d_FeedRate_10;
	public Texture2D t2d_FeedRate_20;
	public Texture2D t2d_FeedRate_30;
	public Texture2D t2d_FeedRate_40;
	public Texture2D t2d_FeedRate_50;
	public Texture2D t2d_FeedRate_60;
	public Texture2D t2d_FeedRate_70;
	public Texture2D t2d_FeedRate_80;
	public Texture2D t2d_FeedRate_90;
	public Texture2D t2d_FeedRate_100;
	public Texture2D t2d_FeedRate_110;
	public Texture2D t2d_FeedRate_120;
	public Texture2D t2d_FeedRate_130;
	public Texture2D t2d_FeedRate_140;
	public Texture2D t2d_FeedRate_150;
	public int feedrate_type = 0;
	
	public Texture2D t2d_spCW_off_d;
	public Texture2D t2d_spCW_off_u;
	public Texture2D t2d_spCW_on_d;
	public Texture2D t2d_spCW_on_u;
	
	public Texture2D t2d_spCCW_on_u;
	public Texture2D t2d_spCCW_on_d;
	public Texture2D t2d_spCCW_off_u;
	public Texture2D t2d_spCCW_off_d;
	
	public Texture2D t2d_spStop_on_u;
	public Texture2D t2d_spStop_on_d;
	public Texture2D t2d_spStop_off_u;
	public Texture2D t2d_spStop_off_d;
	
	public Texture2D t2d_rapid_on_u;
	public Texture2D t2d_rapid_on_d;
	public Texture2D t2d_rapid_off_u;
	public Texture2D t2d_rapid_off_d;
	
	public Texture2D t2d_BottomButton_u;
	public Texture2D t2d_BottomButton_d;
	
	//内容--定义变量，用于实现JOG模式下F0、25%、50%、100%四个按钮的显示
	//姓名--刘旋，时间--2013-4-8
	public Texture2D t2d_f0_on_u;
	public Texture2D t2d_f0_on_d;
	public Texture2D t2d_f0_off_u;
	public Texture2D t2d_f0_off_d;
	
	public Texture2D t2d_f25_on_u;
	public Texture2D t2d_f25_on_d;
	public Texture2D t2d_f25_off_u;
	public Texture2D t2d_f25_off_d;
	
	public Texture2D t2d_f50_on_u;
	public Texture2D t2d_f50_on_d;
	public Texture2D t2d_f50_off_u;
	public Texture2D t2d_f50_off_d;
	
	public Texture2D t2d_f100_on_u;
	public Texture2D t2d_f100_on_d;
	public Texture2D t2d_f100_off_u;
	public Texture2D t2d_f100_off_d;//增加内容到此 2013-4-8
	
	//label字体style
	public GUIStyle sty_ProgEDITWindowO; 
	public GUIStyle sty_Title;
	public GUIStyle sty_TitleLetter;
	public GUIStyle sty_BigXYZ;
	public GUIStyle sty_SmallNum;
	public GUIStyle sty_ProgModeName;
	public GUIStyle sty_Star;
	public GUIStyle sty_Alarm;
	public GUIStyle sty_BottomChooseMenu;
	public GUIStyle sty_ProgEditProgNum;
	public GUIStyle sty_PosSmallWord;
	public GUIStyle sty_SmallXYZ;
	public GUIStyle sty_ScreenCover;
	public GUIStyle sty_ProgEDITWindowFG;
	public GUIStyle sty_BottomAST;
	public GUIStyle sty_MostWords;
	public GUIStyle sty_Code;
	public GUIStyle sty_ProgEDITListWindowNum;
	public GUIStyle sty_Cursor;
	public GUIStyle sty_Warning;
	//内容--定义sty-Mode，用于显示模态
	//姓名--刘旋，时间--2013-3-29
	public GUIStyle sty_Mode;
	public GUIStyle sty_ModeCode;
	
	//button按钮style
	public GUIStyle sty_NCPowerOn;
	public GUIStyle sty_NCPowerOff;
	
	public GUIStyle sty_ButtonCW;
	public GUIStyle sty_ButtonCCW;
	public GUIStyle sty_ButtonSTOP;
	
	public GUIStyle sty_ButtonRapid;
	public GUIStyle sty_ButtonYP;
	public GUIStyle sty_ButtonYN;
	public GUIStyle sty_ButtonZP;
	public GUIStyle sty_ButtonZN;
	public GUIStyle sty_ButtonXP;
	public GUIStyle sty_ButtonXN;
	public GUIStyle sty_Button4P;
	public GUIStyle sty_Button4N;
	
	//内容--定义sty_ButtonF0、sty_Button25、sty_Button50、sty_Button100，用于实现JOG模式下F0、25%、50%、100%四个按钮的显示
	//姓名--刘旋，时间--2013-4-8
	public GUIStyle sty_ButtonF0;
	public GUIStyle sty_ButtonF25;
	public GUIStyle sty_ButtonF50;
	public GUIStyle sty_ButtonF100;//增加内容到此 2013-4-8
	
	public GUIStyle sty_ButtonEmpty;
	
	public GUIStyle sty_ScreenBackGround;
	public GUIStyle sty_TopLabel;
	
	public GUIStyle sty_BottomButtonSmallest;
	public GUIStyle sty_BottomButton_1;
	public GUIStyle sty_BottomButton_2;
	public GUIStyle sty_BottomButton_3;
	public GUIStyle sty_BottomButton_4;
	public GUIStyle sty_BottomButton_5;
	
	public GUIStyle sty_BottomLabel_1;
	public GUIStyle sty_BottomLabel_2;
	public GUIStyle sty_BottomLabel_3;
	public GUIStyle sty_BottomLabel_4;
	
	public GUIStyle sty_ClockStyle;
	
	public GUIStyle sty_EDITLabel;
	public GUIStyle sty_EDITLabelWindow;
	public GUIStyle sty_ProgSharedWindow;
	public GUIStyle sty_EDITLabelBar_1;
	public GUIStyle sty_EDITLabelBar_2;
	public GUIStyle sty_EDITLabelBar_3;
	
	public GUIStyle sty_EDITCursor;
	public GUIStyle sty_EDITTextField;
	public GUIStyle sty_EDITList;
	public GUIStyle sty_InputTextField;
	public GUIStyle sty_OffSet_Coo;
	public GUIStyle sty_SettingsBG;
	
	public GUIStyle sty_EditListTop;
	
	//内容--定义Message界面字体
	public GUIStyle sty_MessAlarm;
	public GUIStyle sty_MessRecordID;
	public GUIStyle sty_MessRecordTime;
	public GUIStyle sty_MessRecordInfo;
	//内容--定义System界面字体
	public GUIStyle sty_SysID;
	public GUIStyle sty_SysInfo;
	
	//内容--定义布尔变量，控制System、Message的显示，姓名--刘旋，时间--2013-4-24
	public bool SystemMenu=false;
	public bool MessageMenu=false;
	public bool PosMenu = true;
	public bool RelativeCoo = false;
	public bool AbsoluteCoo = true;
	public bool GeneralCoo = false;
	public bool ScreenPower = false;
	public bool ScreenCover = false;
	public string MenuDisplay = "编辑";
	public int ProgramNum = 0;
	public int AutoProgName = -1;
	public int LineNum = 0;
	public int SpindleSpeed = 0;
	public int ToolNo = 0;
	public int PartsNum = 987;
	public int RunningTimeH = 57;
	public int RunningTimeM = 60;
	public int CycleTimeH = 24;
	public int CycleTimeM = 60;
	public int CycleTimeS = 60;
	public int RunningSpeed = 0;
	public int SACT = 20;
	public bool ALMBlink = false;
	public bool CursorBlink = false;
	public float BlinkTime = 0;
	public bool ProgMenu = false;
	public bool ProgEDIT = true;
	public bool ProgEDITProg = true;
	public bool ProgEDITList = false;
	public int ProgEDITFlip = 0;
	//内容--定义变量ProgAUTOFlip，用于控制AUTO模式下屏幕的显示
	//姓名--刘旋，时间--2013-3-25
	public int ProgAUTOFlip=0;
	public int ProgSharedFlip = 0;
	public int ProgSharedView = 0;
	public int ProgUsedNum = 0;
	public int ProgUnusedNum = 400;
	public int ProgUsedSpace = 0;
	//内容--内存总容量为512K，姓名--刘旋，时间--2013-3-18
	public int ProgUnusedSpace = 512;//将419430400修改为512
	//内容--定义变量ProgMDIFlip，用于控制MDI模式下，程序菜单屏幕的显示，姓名--刘旋，时间--2013-4-22
	public int ProgMDIFlip=0;
	//内容--定义变量ProgDNCFlip，用于控制DNC模式下，程序菜单屏幕的显示，姓名--刘旋，时间--2013-4-22
	public int ProgDNCFlip=0;
	//内容--定义变量ProgHANFlip，用于控制Handle模式下，程序菜单屏幕的显示，姓名--刘旋，时间--2013-4-22
	public int ProgHANFlip=0;
	//内容--定义变量MessageFlip，用于Message模式的显示，姓名--刘旋，时间--2013-4-24
	public int MessageFlip=0;
	//内容--定义变量SystemFlip，用于System模式的显示，姓名--刘旋，时间--2013-4-24
	public int SystemFlip=0;
	public float ProgEDITCusor = 0;
	public float ProgEDITCusorV = 0;
	public float ProgEDITCusorH = 0;
	public float ProgEDITCusorPos = 0;
	public bool ProgDNC = false;
	public bool ProgAUTO = false;
	public bool ProgMDI = false;
	public bool ProgHAN = false;
	public bool ProgJOG = false;
	public bool ProgREF = false;
	public bool EmergencyCtrl = false;
	public bool EmergencyBlink = false;
	public bool ProgProtect = true;
	public bool ProgProtectWarn = false;
	public Vector2 TextSize = new Vector2(0,0);
	public bool SettingMenu = false;
	public bool OffSetTool = true;
	public bool OffSetSetting = false;
	public bool OffSetCoo = false;
	public bool OffSetOne = true;
	public bool OffSetTwo = false;
	public bool OffCooFirstPage = true;
	
	public List<string> FileNameList = new List<string>();
	public List<int> FileSizeList = new List<int>();
	public List<string> FileDateList = new List<string>();
	public List<string> CodeForAll = new List<string>();
	public List<List<string>> TempCodeList = new List<List<string>>();
	
	public bool[] MoreThanOneArray = new bool[9];
	public int[] RealNumArray = new int[9];
	public string[] TempCodeArray = new string[9];
	
	public int StartRow = 0;
	public int EndRow = 0;
	public int TotalCodeNum = 0;
	public int RealCodeNum = 1;
	public int HorizontalNum = 1;
	public int VerticalNum = 1;
	//The total amount of satisfatory NC files in the Gcode directory.
	public int TotalListNum = 0;
	//The location of current file in the NC file list.
	public int RealListNum = 1;
	public string Code01 = "";
	public string Code02 = "";
	public string Code03 = "";
	public string Code04 = "";
	public string Code05 = "";
	public string Code06 = "";
	public string Code07 = "";
	public string Code08 = "";
	public string Code09 = "";
	public string[] CodeName = new string[8];
	public int[] CodeSize = new int[8];
	public string[] UpdateDate = new string[8];
	public string current_filename = "";
	//The location of current file in the NC file list. Co-operate with arqument RealFileList.
	public int current_filenum = 0;
	
	public GUIText EDITText;
	public GUIText CursorText;
	
	public bool ShiftFlag = false;
	public string InputText = "";
	public string TempInputText = "";
	public Vector2 InputTextSize = new Vector2(0,0);
	public string OffSetTemp = "";
	
	public float coo_setting_cursor_x = 131f;
	public float coo_setting_cursor_y = 120f;
	public int coo_setting_1 = 1;
	public int coo_setting_2 = 1;
	
	public bool power_notification = false;
	Rect power_notifi_window = new Rect(Screen.width / 2f, Screen.height / 2f, 200f, 100f); 
	public float move_rate = 1f;
	
	//设定界面修改---张振华---03.11
	public GUIStyle sty_OffSet_Coo_mini;
	public GUIStyle sty_OffSet_Coo_mid;
	public float argu_setting_cursor_y = 61.5f;
	public float argu_setting_cursor_w = 16f;
	public int argu_setting = 1;
	//设定界面修改---张振华---03.11
	
	//位置界面功能完善---宋荣 ---03.09
	public bool operationBottomScrInitial=false;//position模式下按下操作键的初始界面显示标志
	public bool operationBottomScrExecute=false;//position模式下按下执行界面显示标志
	public bool partsNumBlink=false;//零件数闪烁标志
	public bool runtimeIsBlink=false;//运行时间闪烁标志
	public bool posOperationMode=false;//position下按下操作键,用来屏蔽第一、二、三个按钮的操作。
	public int statusBeforeOperation=-1;
	//位置界面功能完善---宋荣 ---03.09
	
	//内容--姓名--日期
	//变量
	//内容--定义布尔变量ProgEDITAt，用于选择程序时，前加@
	//姓名--刘旋
	//日期--2013-3-18
	public bool ProgEDITAt=false;
	public int at_position = -1;
	public int at_page_number = -1;
	
	//刀偏界面完善---张振华---03.30
	public GUIStyle sty_MostWords_ToolOffSet;        //刀偏界面字体
	public int ToolOffSetPage_num = 0;
	public int number = 0;
	public int tool_setting = 1 ; //值为1-32，代表刀偏界面的每一个空格
	public float tool_setting_cursor_y = 81.5f; //刀偏界面光标水平方向的值
	public float tool_setting_cursor_w = 91.5f; //刀偏界面光标垂直方向的值
	//刀偏界面完善---张振华---03.30
	
	float left = -300f;
	bool show_off = false;
	#endregion
	
	void Awake () 
	{
		gameObject.AddComponent("PositionModule");
		Position_Script = gameObject.GetComponent<PositionModule>();
		gameObject.AddComponent("SystemModule");//添加脚本，姓名--刘旋，时间--2013-4-24
		System_Script=gameObject.GetComponent<SystemModule>();
		gameObject.AddComponent("MessageModule");
		Message_Script=gameObject.GetComponent<MessageModule>();
		gameObject.AddComponent("SoftkeyModule");
		Softkey_Script = gameObject.GetComponent<SoftkeyModule>();
		gameObject.AddComponent("ProgramModule");
		Program_Script = gameObject.GetComponent<ProgramModule>();
		gameObject.AddComponent("OffsetSettingModule");
		Offset_Script = gameObject.GetComponent<OffsetSettingModule>();
		gameObject.AddComponent("MDIInputModule");
		MDIInput_Script = gameObject.GetComponent<MDIInputModule>();
		gameObject.AddComponent("MDIFunctionModule");
		MDIFunction_Script = gameObject.GetComponent<MDIFunctionModule>();
		gameObject.AddComponent("MDIEditModule");
		MDIEdit_Script = gameObject.GetComponent<MDIEditModule>();
		gameObject.AddComponent("ModeSelectModule");
		ModeSelect_Script = gameObject.GetComponent<ModeSelectModule>();
		gameObject.AddComponent("FeedrateModule");
		Feedrate_Script = gameObject.GetComponent<FeedrateModule>();
		gameObject.AddComponent("MachineFunctionModule");
		MachineFunction_Script = gameObject.GetComponent<MachineFunctionModule>();
		gameObject.AddComponent("AuxiliaryFunctionModule");
		AuxiliaryFunction_Script = gameObject.GetComponent<AuxiliaryFunctionModule>();
		gameObject.AddComponent("AuxiliaryMoveModule");
		AuxiliaryMove_Script = gameObject.GetComponent<AuxiliaryMoveModule>();
		GameObject.Find("move_control").AddComponent("MoveControl");
		MoveControl_script = GameObject.Find("move_control").GetComponent<MoveControl>();
		GameObject.Find("spindle_control").AddComponent("SpindleControl");
		SpindleControl_script = GameObject.Find("spindle_control").GetComponent<SpindleControl>();
		gameObject.AddComponent("CooSystem");
		gameObject.AddComponent("AutoMove");
		gameObject.AddComponent("CompileNC");
		CooSystem_script = gameObject.GetComponent<CooSystem>();
		CompileNC_script = gameObject.GetComponent<CompileNC>();

		t2d_lock = (Texture2D)Resources.Load("Texture_Panel/Button/lock");
		t2d_unlock = (Texture2D)Resources.Load("Texture_Panel/Button/unlock");
		t2d_alarm = (Texture2D)Resources.Load("Texture_Panel/Button/alarm");
		t2d_zero = (Texture2D)Resources.Load("Texture_Panel/Button/zero");
		t2d_toolnum = (Texture2D)Resources.Load("Texture_Panel/Button/toolnum");	
		t2d_em_u = (Texture2D)Resources.Load("Texture_Panel/Button/em_u");
		t2d_em_d = (Texture2D)Resources.Load("Texture_Panel/Button/em_d");
		t2d_Protect = t2d_lock;
		t2d_Emergency = t2d_em_u;
		
		t2d_ModeSelectEDIT = (Texture2D)Resources.Load("Texture_Panel/Button/mode_edit");
		t2d_ModeSelectDNC = (Texture2D)Resources.Load("Texture_Panel/Button/mode_dnc");
		t2d_ModeSelectAUTO = (Texture2D)Resources.Load("Texture_Panel/Button/mode_auto");
		t2d_ModeSelectMDI = (Texture2D)Resources.Load("Texture_Panel/Button/mode_mdi");
		t2d_ModeSelectHANDLE = (Texture2D)Resources.Load("Texture_Panel/Button/mode_handle");
		t2d_ModeSelectJOG = (Texture2D)Resources.Load("Texture_Panel/Button/mode_jog");
		t2d_ModeSelectREF = (Texture2D)Resources.Load("Texture_Panel/Button/mode_ref");
		if(PlayerPrefs.HasKey("ModeSelect"))
			mode_type = PlayerPrefs.GetInt("ModeSelect");
		else
		{
			PlayerPrefs.SetInt("ModeSelect", 1);
			mode_type = 1;
		}
		switch(mode_type)
		{
		case 1:
			t2d_ModeSelect = t2d_ModeSelectEDIT;
			MenuDisplay = "编辑";
			ProgEDIT = true;
			ProgDNC = false;
			ProgAUTO = false;
			ProgMDI = false;
			ProgHAN = false;
			ProgJOG = false;
			ProgREF = false;
			break;
		case 2:
			t2d_ModeSelect = t2d_ModeSelectDNC;
			MenuDisplay = "DNC";
			ProgEDIT = false;
			ProgDNC = true;
			ProgAUTO = false;
			ProgMDI = false;
			ProgHAN = false;
			ProgJOG = false;
			ProgREF = false;
			break;
		case 3:
			t2d_ModeSelect = t2d_ModeSelectAUTO;
			MenuDisplay = "MEM";
			ProgEDIT = false;
			ProgDNC = false;
			ProgAUTO = true;
			ProgMDI = false;
			ProgHAN = false;
			ProgJOG = false;
			ProgREF = false;
			break;
		case 4:
			t2d_ModeSelect = t2d_ModeSelectMDI;
			MenuDisplay = "MDI";
			ProgEDIT = false;
			ProgDNC = false;
			ProgAUTO = false;
			ProgMDI = true;
			ProgHAN = false;
			ProgJOG = false;
			ProgREF = false;
			break;
		case 5:
			t2d_ModeSelect = t2d_ModeSelectHANDLE;
			MenuDisplay = "HAN";
			ProgEDIT = false;
			ProgDNC = false;
			ProgAUTO = false;
			ProgMDI = false;
			ProgHAN = true;
			ProgJOG = false;
			ProgREF = false;
			break;
		case 6:
			t2d_ModeSelect = t2d_ModeSelectJOG;
			MenuDisplay = "JOG";
			MoveControl_script.speed_to_move = 0.16667F;//内容--JOG模式下，慢常速为10m/min=(10/60)m/s,因此spee-to-move=10/60,姓名--刘旋，时间--2013-4-8
			ProgEDIT = false;
			ProgDNC = false;
			ProgAUTO = false;
			ProgMDI = false;
			ProgHAN = false;
			ProgJOG = true;
			ProgREF = false;
			break;
		case 7:
			t2d_ModeSelect = t2d_ModeSelectREF;
			MenuDisplay = "REF";
			MoveControl_script.speed_to_move = 0.6F;//内容--归零操作的实际速度为5m/min=(5/60)m/s，而实际速度RunningSpeed=speed—to-move*move-rate，因此speed-to-move应设为5/60,姓名--刘旋，时间--2013-4-8
			ProgEDIT = false;
			ProgDNC = false;
			ProgAUTO = false;
			ProgMDI = false;
			ProgHAN = false;
			ProgJOG = false;
			ProgREF = true;
			break;	
		}
		
		t2d_FeedRate_0 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate1");
		t2d_FeedRate_10 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate2");
		t2d_FeedRate_20 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate3");
		t2d_FeedRate_30 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate4");
		t2d_FeedRate_40 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate5");
		t2d_FeedRate_50 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate6");
		t2d_FeedRate_60 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate7");
		t2d_FeedRate_70 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate8");
		t2d_FeedRate_80 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate9");
		t2d_FeedRate_90 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate10");
		t2d_FeedRate_100 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate11");
		t2d_FeedRate_110 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate12");
		t2d_FeedRate_120 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate13");
		t2d_FeedRate_130 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate14");
		t2d_FeedRate_140 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate15");
		t2d_FeedRate_150 = (Texture2D)Resources.Load("Texture_Panel/Button/feedrate16");
		if(PlayerPrefs.HasKey("FeedrateSelect"))
			feedrate_type = PlayerPrefs.GetInt("FeedrateSelect");
		else
		{
			PlayerPrefs.SetInt("FeedrateSelect", 11);
			feedrate_type = 11;
		}
		switch(feedrate_type) 
		{
		case 1:
			t2d_feedrate = t2d_FeedRate_0;
			move_rate = 0f;
			break;
		case 2:
			t2d_feedrate = t2d_FeedRate_10;
			move_rate = 0.1f;
			break;
		case 3:
			t2d_feedrate = t2d_FeedRate_20;
			move_rate = 0.2f;
			break;
		case 4:
			t2d_feedrate = t2d_FeedRate_30;
			move_rate = 0.3f;
			break;
		case 5:
			t2d_feedrate = t2d_FeedRate_40;
			move_rate = 0.4f;
			break;
		case 6:
			t2d_feedrate = t2d_FeedRate_50;
			move_rate = 0.5f;
			break;
		case 7:
			t2d_feedrate = t2d_FeedRate_60;
			move_rate = 0.6f;
			break;
		case 8:
			t2d_feedrate = t2d_FeedRate_70;
			move_rate = 0.7f;
			break;
		case 9:
			t2d_feedrate = t2d_FeedRate_80;
			move_rate = 0.8f;
			break;
		case 10:
			t2d_feedrate = t2d_FeedRate_90;
			move_rate = 0.9f;
			break;
		case 11:
			t2d_feedrate = t2d_FeedRate_100;
			move_rate = 1.0f;
			break;
		case 12:
			t2d_feedrate = t2d_FeedRate_110;
			move_rate = 1.1f;
			break;
		case 13:
			t2d_feedrate = t2d_FeedRate_120;
			move_rate = 1.2f;
			break;
		case 14:
			t2d_feedrate = t2d_FeedRate_130;
			move_rate = 1.3f;
			break;
		case 15:
			t2d_feedrate = t2d_FeedRate_140;
			move_rate = 1.4f;
			break;
		case 16:
			t2d_feedrate = t2d_FeedRate_150;
			move_rate = 1.5f;
			break;
		}
		MoveControl_script.move_rate = move_rate;
		
		t2d_NCPower_on_u = (Texture2D)Resources.Load("Texture_Panel/Button/NCPower_on_u");
		t2d_NCPower_on_d = (Texture2D)Resources.Load("Texture_Panel/Button/NCPower_on_d");
		t2d_NCPower_off_u = (Texture2D)Resources.Load("Texture_Panel/Button/NCPower_off_u");
		t2d_NCPower_off_d = (Texture2D)Resources.Load("Texture_Panel/Button/NCPower_off_d");
		sty_NCPowerOn.normal.background = t2d_NCPower_on_u;
		sty_NCPowerOn.active.background = t2d_NCPower_on_d;
		sty_NCPowerOff.normal.background = t2d_NCPower_off_u;
		sty_NCPowerOff.active.background = t2d_NCPower_off_d;
		
		t2d_spCW_off_d = (Texture2D)Resources.Load("Texture_Panel/Button/spCW_off_d");
		t2d_spCW_off_u = (Texture2D)Resources.Load("Texture_Panel/Button/spCW_off_u");
		t2d_spCW_on_d = (Texture2D)Resources.Load("Texture_Panel/Button/spCW_on_d");
		t2d_spCW_on_u = (Texture2D)Resources.Load("Texture_Panel/Button/spCW_on_u");
		sty_ButtonCW.normal.background = t2d_spCW_off_u;
		sty_ButtonCW.active.background = t2d_spCW_off_d;
		
		
		t2d_spCCW_on_u = (Texture2D)Resources.Load("Texture_Panel/Button/spCCW_on_u");
		t2d_spCCW_on_d = (Texture2D)Resources.Load("Texture_Panel/Button/spCCW_on_d");
		t2d_spCCW_off_u = (Texture2D)Resources.Load("Texture_Panel/Button/spCCW_off_u");
		t2d_spCCW_off_d = (Texture2D)Resources.Load("Texture_Panel/Button/spCCW_off_d");
		sty_ButtonCCW.normal.background = t2d_spCCW_off_u;
		sty_ButtonCCW.active.background = t2d_spCCW_off_d;
		
		t2d_spStop_on_u = (Texture2D)Resources.Load("Texture_Panel/Button/spStop_on_u");
		t2d_spStop_on_d = (Texture2D)Resources.Load("Texture_Panel/Button/spStop_on_d");
		t2d_spStop_off_u = (Texture2D)Resources.Load("Texture_Panel/Button/spStop_off_u");
		t2d_spStop_off_d = (Texture2D)Resources.Load("Texture_Panel/Button/spStop_off_d");
		sty_ButtonSTOP.normal.background = t2d_spStop_on_u;
		sty_ButtonSTOP.active.background = t2d_spStop_on_d;
		
		t2d_rapid_on_u = (Texture2D)Resources.Load("Texture_Panel/Button/rapid_on_u");
		t2d_rapid_on_d = (Texture2D)Resources.Load("Texture_Panel/Button/rapid_on_d");
		t2d_rapid_off_u = (Texture2D)Resources.Load("Texture_Panel/Button/rapid_off_u");
		t2d_rapid_off_d = (Texture2D)Resources.Load("Texture_Panel/Button/rapid_off_d");
		sty_ButtonRapid.normal.background = t2d_rapid_off_u;
		sty_ButtonRapid.active.background = t2d_rapid_off_d;
		
		//内容--为变量赋值，用于实现JOG模式下F0，25%、50%、100%四个按钮的显示
		//姓名--刘旋，时间--2013-4-8
		t2d_f0_on_u=(Texture2D)Resources.Load("Texture_Panel/Button/f0_on_u");
		t2d_f0_on_d=(Texture2D)Resources.Load("Texture_Panel/Button/f0_on_d");
		t2d_f0_off_u=(Texture2D)Resources.Load("Texture_Panel/Button/f0_off_u");
		t2d_f0_off_d=(Texture2D)Resources.Load("Texture_Panel/Button/f0_off_d");
		sty_ButtonF0.normal.background=t2d_f0_off_u;
		sty_ButtonF0.active.background=t2d_f0_off_d;
		
		t2d_f25_on_u=(Texture2D)Resources.Load("Texture_Panel/Button/f25_on_u");
		t2d_f25_on_d=(Texture2D)Resources.Load("Texture_Panel/Button/f25_on_d");
		t2d_f25_off_u=(Texture2D)Resources.Load("Texture_Panel/Button/f25_off_u");
		t2d_f25_off_d=(Texture2D)Resources.Load("Texture_Panel/Button/f25_off_d");
		sty_ButtonF25.normal.background=t2d_f25_off_u;
		sty_ButtonF25.active.background=t2d_f25_off_d;
		
		t2d_f50_on_u=(Texture2D)Resources.Load("Texture_Panel/Button/f50_on_u");
		t2d_f50_on_d=(Texture2D)Resources.Load("Texture_Panel/Button/f50_on_d");
		t2d_f50_off_u=(Texture2D)Resources.Load("Texture_Panel/Button/f50_off_u");
		t2d_f50_off_d=(Texture2D)Resources.Load("Texture_Panel/Button/f50_off_d");
		sty_ButtonF50.normal.background=t2d_f50_off_u;
		sty_ButtonF50.active.background=t2d_f50_off_d;
		
		t2d_f100_on_u=(Texture2D)Resources.Load("Texture_Panel/Button/f100_on_u");
		t2d_f100_on_d=(Texture2D)Resources.Load("Texture_Panel/Button/f100_on_d");
		t2d_f100_off_u=(Texture2D)Resources.Load("Texture_Panel/Button/f100_off_u");
		t2d_f100_off_d=(Texture2D)Resources.Load("Texture_Panel/Button/f100_off_d");
		sty_ButtonF100.normal.background=t2d_f100_off_u;
		sty_ButtonF100.active.background=t2d_f100_off_d;//增加内容到此  2013-4-8
		
		sty_ButtonYN.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/yminus_off_u");
		sty_ButtonYN.active.background = (Texture2D)Resources.Load("Texture_Panel/Button/yminus_on_d");
		
		sty_ButtonYP.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/yplus_off_u");
		sty_ButtonYP.active.background = (Texture2D)Resources.Load("Texture_Panel/Button/yplus_on_d");
		
		sty_ButtonZP.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/zplus_off_u");
		sty_ButtonZP.active.background = (Texture2D)Resources.Load("Texture_Panel/Button/zplus_on_d");
		
		sty_ButtonZN.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/zminus_off_u");
		sty_ButtonZN.active.background = (Texture2D)Resources.Load("Texture_Panel/Button/zminus_on_d");
		
		sty_ButtonXP.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/xplus_off_u");
		sty_ButtonXP.active.background = (Texture2D)Resources.Load("Texture_Panel/Button/xplus_on_d");
		
		sty_ButtonXN.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/xminus_off_u");
		sty_ButtonXN.active.background = (Texture2D)Resources.Load("Texture_Panel/Button/xminus_on_d");
		
		sty_Button4P.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/4plus_off_u");
		sty_Button4P.active.background = (Texture2D)Resources.Load("Texture_Panel/Button/4plus_on_d");
		
		sty_Button4N.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/4minus_off_u");
		sty_Button4N.active.background = (Texture2D)Resources.Load("Texture_Panel/Button/4minus_on_d");
		
		sty_ProgEDITWindowO.font = (Font)Resources.Load("font/STSONG");
		sty_ProgEDITWindowO.fontSize = 16;
		sty_ProgEDITWindowO.normal.textColor = Color.white;
		
		sty_Title.font = (Font)Resources.Load("font/digifaw");
		sty_Title.fontSize = 15;
		
		sty_TitleLetter.font = (Font)Resources.Load("font/STSONG");
		sty_TitleLetter.fontSize = 17;
		
		sty_BigXYZ.font = (Font)Resources.Load("font/LCD");
		sty_BigXYZ.fontSize = 45;
		
		sty_SmallNum.font = (Font)Resources.Load("font/monoMMM_5");
		sty_SmallNum.fontSize = 14;
		
		sty_ProgModeName.font = (Font)Resources.Load("font/times");
		sty_ProgModeName.fontSize = 14;
		
		sty_Star.fontSize = 22;
		
		sty_Alarm.font = (Font)Resources.Load("font/times");
		sty_Alarm.fontSize = 12;
		sty_Alarm.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/red");
		sty_Alarm.normal.textColor = Color.white;
		
		sty_BottomChooseMenu.font = (Font)Resources.Load("font/monoMMM_5");
		//sty_BottomChooseMenu.font = (Font)Resources.Load("font/times");
		sty_BottomChooseMenu.fontSize = 14;
		
		sty_ProgEditProgNum.font = (Font)Resources.Load("font/monoMMM_5");
		sty_ProgEditProgNum.fontSize = 15;
		sty_ProgEditProgNum.normal.textColor = Color.white;
		
		sty_PosSmallWord.font = (Font)Resources.Load("font/simfang");
		sty_PosSmallWord.fontSize = 15;
		
		sty_SmallXYZ.font = (Font)Resources.Load("font/times");
		sty_SmallXYZ.fontSize = 17;
		
		sty_ScreenCover.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/StartScreen");
		
		sty_ProgEDITWindowFG.font = (Font)Resources.Load("font/simfang");
		sty_ProgEDITWindowFG.fontSize = 15;
		sty_ProgEDITWindowFG.normal.textColor = Color.white;
		
		sty_BottomAST.font = (Font)Resources.Load("font/times");
		sty_BottomAST.fontSize = 15;
		//sty_BottomAST.normal.textColor = Color.cyan;
		
		sty_MostWords.font = (Font)Resources.Load("font/simfang");
		sty_MostWords.fontSize = 15;
		//sty_MostWords.normal.textColor = Color.cyan;
		
		//sty_Code.font = (Font)Resources.Load("font/dutch");
		sty_Code.fontSize = 17;
		sty_Code.fontStyle = FontStyle.Bold;
		
		sty_ModeCode.fontSize=15;
		sty_ModeCode.fontStyle=FontStyle.Bold;
		
		//内容--sty-Mode赋值为蓝色
		//姓名--刘旋，时间--2013-3-29
		sty_Mode.fontSize=15;
		sty_Mode.fontStyle=FontStyle.Bold;
		sty_Mode.normal.textColor=Color.blue;
		
		sty_ProgEDITListWindowNum.font = (Font)Resources.Load("font/monoMMM_5");
		sty_ProgEDITListWindowNum.fontSize = 13;
		
		sty_Cursor.font = (Font)Resources.Load("font/times");
		sty_Cursor.fontSize = 15;
		
		sty_Warning.fontSize = 14;
		sty_Warning.normal.textColor = Color.red;
		
		sty_ScreenBackGround.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/ScreenBackground");
		
		sty_TopLabel.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/toplabel");
		
		sty_MessAlarm.font=(Font)Resources.Load("font/simfang");
		sty_MessAlarm.normal.textColor=Color.red;
		sty_MessAlarm.fontSize=13;
		
		sty_MessRecordID.font=(Font)Resources.Load("font/simfang");
		sty_MessRecordID.normal.textColor=Color.blue;
		sty_MessRecordID.fontSize=13;
		
		sty_MessRecordTime.font=(Font)Resources.Load("font/simfang");
		sty_MessRecordTime.fontSize=14;
		
		sty_MessRecordInfo.font=(Font)Resources.Load("font/simfang");
		sty_MessRecordInfo.fontSize=15;
		
		sty_SysID.font=(Font)Resources.Load("font/monoMMM_5");
		sty_SysID.fontSize=13;
		
		sty_SysInfo.font=(Font)Resources.Load("font/simfang");
		sty_SysInfo.fontSize=15;
		sty_SysInfo.normal.textColor=Color.blue;
		
		t2d_BottomButton_u = (Texture2D)Resources.Load("Texture_Panel/Button/bottombutton_u");
		t2d_BottomButton_d = (Texture2D)Resources.Load("Texture_Panel/Button/bottombutton_d");
		
		sty_BottomButtonSmallest.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/bottombutton_smallest");
		sty_BottomButton_1.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/bottombutton_d");
		sty_BottomButton_2.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/bottombutton_u");
		sty_BottomButton_3.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/bottombutton_u");
		sty_BottomButton_4.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/bottombutton_u");
		sty_BottomButton_5.normal.background = (Texture2D)Resources.Load("Texture_Panel/Button/bottombutton_u");
		
		sty_BottomLabel_1.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/bottomLabel01");
		sty_BottomLabel_2.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/bottomLabel02");
		sty_BottomLabel_3.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/bottomLabel03");
		sty_BottomLabel_4.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/bottomLabel04");
		
		sty_ClockStyle.fontSize = 14;
		
		sty_EDITLabel.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/EDITLabel");
		sty_EDITLabelWindow.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/EditWindow");
		sty_EDITLabelBar_1.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/EditBar01");
		sty_EDITLabelBar_2.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/EditBar02");
		sty_EDITLabelBar_3.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/EditBar03");
		sty_ProgSharedWindow.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/ProgSharedWindow");
		
		sty_EDITCursor.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/EditCursor");
		
		sty_EDITTextField.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/EditCursor");
		sty_EDITTextField.normal.textColor = Color.yellow;
		sty_EDITTextField.fontSize = 17;
		sty_EDITTextField.fontStyle = FontStyle.Bold;
		
		sty_EDITList.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/EditList");
		
		sty_InputTextField.font = (Font)Resources.Load("font/times");
		sty_InputTextField.fontSize = 15;
		
		sty_OffSet_Coo.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/offset_coo");
		width = 670F;
		height = 650F;
		left = -700f;
		PanelWindowRect = new Rect(left, 30f, width, height);
		EDITText.enabled = false;
		EDITText.font = sty_Code.font;
		EDITText.fontSize = sty_Code.fontSize;
		//EDITText.fontStyle=FontStyle.Bold;
		EDITText.text = "";
		ProgEDITCusorPos = 57f;
		CursorText.enabled = false;
		CursorText.font = sty_Cursor.font;
		CursorText.fontSize = sty_Cursor.fontSize;
		
		coo_setting_cursor_x = 131f;
	    coo_setting_cursor_y = 120f;
		coo_setting_1 = 1;
		coo_setting_2 = 1;
		
		sty_SettingsBG.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/SettingsBG");
		
		//设定界面修改---陈振华---03.11
		sty_OffSet_Coo_mini.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/offset_coo_mini");
		sty_OffSet_Coo_mid.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/offset_coo_mid");
		//设定界面修改---陈振华---03.11
		
		sty_EditListTop.normal.background = (Texture2D)Resources.Load("Texture_Panel/Label/EditListTop");
		OffSetTool = true;
		OffSetSetting = false;
		OffSetCoo = false;
		
		//刀偏界面完善---张振华---03.30
		sty_MostWords_ToolOffSet.font = (Font)Resources.Load("font/simfang");
		sty_MostWords_ToolOffSet.fontSize = 13;
		ToolOffSetPage_num = 0;    //页面数
		number = 0;                            //序号
		tool_setting = 1;                     //黄色背景序号
	    tool_setting_cursor_y = 81.5f;
	    tool_setting_cursor_w = 91.5f;
		//刀偏界面完善---张振华---03.30
	}
	
	void OnGUI()
	{ 
		//GUI.depth = 1;
		if(show_off == false)
		{
			PanelWindowRect.x = left;
			PanelWindowRect.width = width;
			PanelWindowRect.height = height;
		}
		PanelWindowRect = GUI.Window(0, PanelWindowRect, PanelWindow, "Control Panel");   
		
		if(power_notification)
		{
			power_notifi_window = GUI.Window(1, power_notifi_window, PowerState, "");
			GUI.BringWindowToFront(1);
		}
	}    
	
	void FixedUpdate ()
	{
		width = Mathf.Lerp(0.1f, 670f, 1.5f*Time.time);
		height = Mathf.Lerp(0.1f, 650f, 1.5f*Time.time);
		left = Mathf.Lerp(-300f, 300f, 1.5f*Time.time);
		timeV += Time.deltaTime;	
		if(timeV > 1)
			show_off = true;
	}
	
	void PowerState (int windowID)
	{
		GUI.color = Color.black;
		GUI.Label(new Rect(40f, 25f, 400f, 40f), "请打开NC系统电源！");
		GUI.color = Color.white;
		if(GUI.Button(new Rect(70f, 60f, 50f, 20f), "确定"))
			power_notification = false;
		GUI.DragWindow();  
	}
	
	void PanelWindow(int windowID) 
	{  
		//屏幕
		GUI.Box(new Rect(40f/1000f*width,30f/1000f*height,500f/1000f*width,420f/1000f*height),"");
		//NC系统电源，屏幕是否显示
		if(ScreenPower)
		{
			ScreenNormallyOn();
			//位置界面
			if(PosMenu)
		    {
				//Position模块显示
				Position_Script.Position();
		    }
			
			//程序界面
			if(ProgMenu)
			{
				//Program模块显示
				Program_Script.Program();
			}
			
			//设置界面
			if(SettingMenu)
			{
				//Offset模块显示
				Offset_Script.Offset();
			}
			
			//System界面，姓名--刘旋，时间--2013-4-24
			if(SystemMenu)
			{
				//System模块显示
				System_Script.System();
			}
			
			//Message界面，姓名--刘旋，时间--2013-4-24
			if(MessageMenu)
			{
				//Message模块显示
				Message_Script.Message();
			}
			
			//屏幕 基本固定区域
			ScreenBottom();
			
			//打印编辑区域
			ScreenPrintArea();	
			SpeedModule();//内容--增加行数SpeedModule用于控制时速度，姓名--刘旋，时间--2013-4-16
		}
		
		//以上部分为屏幕显示区域，所有有关屏幕GUI效果的变化都通过上述函数增添和编辑
		
		//*********************************************************************************************************************
		
		
		//屏幕启动和关闭界面
		if(ScreenCover)
			GUI.Box(new Rect(40f/1000f*width,30f/1000f*height,500f/1000f*width,420f/1000f*height),"",sty_ScreenCover);
		
		//MDI面板输入区	
		MDIInput_Script.MDIInput();
		
		//MDI功能按键区
			//MDI功能选择按键
		MDIFunction_Script.Function();
			//MDI文字编辑按键
		MDIEdit_Script.Edit();
		
		//屏幕下方功能软键-------------------------------------------------------------------------------------------------------------
		Softkey_Script.Softkey();

		//NC系统电源按钮
		//(1)
		GUI.Box(new Rect(40f/1000f*width,520f/1000f*height,150f/1000f*width,250f/1000f*height),"");
		
		NCPowerButton();
		
		//(2)
		GUI.Box(new Rect(195f/1000f*width,520f/1000f*height,770f/1000f*width,100f/1000f*height),"");
		
		GUI.DrawTexture(new Rect(240f/1000f*width,535f/1000f*height,49f/1000f*width,75f/1000f*height), t2d_Protect, ScaleMode.ScaleAndCrop, true, 0.653f);

		//程序保护锁---------------------------------------------------------------------------------------------------------------------------------------------------------
		if (GUI.Button(new Rect(240f/1000f*width, 535f/1000f*height, 49f/1000f*width, 75f/1000f*height), "", sty_ButtonEmpty))
		{
			if(ProgProtect)
			{
				t2d_Protect = t2d_unlock;
				ProgProtect = false;
				ProgProtectWarn = false;
			}
			else
			{
				t2d_Protect = t2d_lock;
				ProgProtect = true;
			}	
		}
		
		GUI.DrawTexture(new Rect(360f/1000f*width,535f/1000f*height,150f/1000f*width,75f/1000f*height), t2d_alarm, ScaleMode.ScaleAndCrop, true, 2.0f);
		
		GUI.DrawTexture(new Rect(580f/1000f*width,535f/1000f*height,195f/1000f*width,75f/1000f*height), t2d_zero, ScaleMode.ScaleAndCrop, true, 2.6f);
		
		GUI.DrawTexture(new Rect(850f/1000f*width,535f/1000f*height,75f/1000f*width,75f/1000f*height), t2d_toolnum, ScaleMode.ScaleAndCrop, true, 1.0f);
		
		
		//(3)
		GUI.Box(new Rect(195f/1000f*width,625f/1000f*height,770f/1000f*width,145f/1000f*height),"");
		
		//模式选择旋钮
		ModeSelect_Script.ModeSelectButton();
		
		//手动进给速率旋钮
		Feedrate_Script.FeedrateSelect();
	
		//机床功能按键
		MachineFunction_Script.MachineFunction();

		
		//(4)
		GUI.Box(new Rect(40f/1000f*width,775f/1000f*height,925f/1000f*width,200f/1000f*height),"");
		
		GUI.color = Color.white;
		
		GUI.DrawTexture(new Rect(100f/1000f*width,790f/1000f*height,110f/1000f*width,110f/1000f*height), t2d_Emergency, ScaleMode.ScaleAndCrop, true, 1f);
		
		//紧急停止按钮   待完善
		if (GUI.Button(new Rect(80f/1000f*width, 790f/1000f*height, 150f/1000f*width, 110f/1000f*height), "", sty_ButtonEmpty))            
		{
			if(ScreenPower)
			{
				if(EmergencyCtrl)
				{
					t2d_Emergency = t2d_em_u;
				}
				else
				{
					t2d_Emergency = t2d_em_d;
				}
				EmergencyCtrl = !EmergencyCtrl;
			}
		}
		
		//循环启动 待完善
		GUI.color = Color.green;
		GUI.contentColor = Color.green;
		if (GUI.Button(new Rect(80f/1000f*width, 920f/1000f*height, 70f/1000f*width, 40f/1000f*height), "I"))            
		{
			if(ScreenPower)
			{
				
			}
			else
			{
				power_notification = true;
			}
		}
		//进给保持按钮  待完善
		GUI.color = Color.yellow;
		GUI.contentColor = Color.yellow;
		if (GUI.Button(new Rect(160f/1000f*width, 920f/1000f*height, 70f/1000f*width, 40f/1000f*height), "O"))            
		{
			if(ScreenPower)
			{
				
			}
			else
			{
				power_notification = true;
			}
		}
		GUI.color = Color.white;
		GUI.contentColor = Color.white;
		
		// 机床辅助功能按键
		AuxiliaryFunction_Script.AuxiliaryFunction();
		
		// 手动进给按钮 待完善
		ManualOperationButton();
		
		// 主轴旋转控制按钮  待完善
		SpindleControl();

		GUI.DragWindow();    
	}
	
	void SpeedModule()//内容--增加行数SpeedModule用于控制时速度，姓名--刘旋，时间--2013-4-16
	{
		if(MoveControl_script.x_p||MoveControl_script.x_n||MoveControl_script.y_p||MoveControl_script.y_n||MoveControl_script.z_p||MoveControl_script.z_n)
			RunningSpeed=Convert.ToInt32(MoveControl_script.speed_to_move*MoveControl_script.move_rate*1000*60);
		else 
			RunningSpeed=0;	
	}
	
	void ScreenNormallyOn() {
		
		GUI.Box(new Rect(40f/1000f*width,30f/1000f*height,500f/1000f*width,420f/1000f*height),"", sty_ScreenBackGround);
		GUI.Label(new Rect(40f/1000f*width,30f/1000f*height,500f/1000f*width,25f/1000f*height),"", sty_TopLabel);
		GUI.Label(new Rect(40f/1000f*width,422f/1000f*height,20f/1000f*width,25f/1000f*height),"", sty_BottomButtonSmallest);
		GUI.Label(new Rect(65f/1000f*width,422f/1000f*height,86f/1000f*width,25f/1000f*height),"", sty_BottomButton_1);
		GUI.Label(new Rect(156f/1000f*width,422f/1000f*height,86f/1000f*width,25f/1000f*height),"", sty_BottomButton_2);
		GUI.Label(new Rect(247f/1000f*width,422f/1000f*height,86f/1000f*width,25f/1000f*height),"", sty_BottomButton_3);
		GUI.Label(new Rect(338f/1000f*width,422f/1000f*height,86f/1000f*width,25f/1000f*height),"", sty_BottomButton_4);
		GUI.Label(new Rect(429f/1000f*width,422f/1000f*height,86f/1000f*width,25f/1000f*height),"", sty_BottomButton_5);
		GUI.Label(new Rect(520f/1000f*width,422f/1000f*height,20f/1000f*width,25f/1000f*height),"", sty_BottomButtonSmallest);
	}
	
	void ScreenBottom ()
	{
		GUI.Label(new Rect(340f/1000f*width,26f/1000f*height,500f/1000f*width,300f/1000f*height),"O           N", sty_TitleLetter);	
		GUI.Label(new Rect(362f/1000f*width,30f/1000f*height,500f/1000f*width,300f/1000f*height),ToolNumFormat(ProgramNum), sty_SmallNum);	
		GUI.Label(new Rect(449f/1000f*width,30f/1000f*height,500f/1000f*width,300f/1000f*height),LineNumFormat(LineNum), sty_SmallNum);
		GUI.Label(new Rect(40f/1000f*width,345f/1000f*height,500f/1000f*width,25f/1000f*height),"", sty_TopLabel);
		GUI.Label(new Rect(43f/1000f*width,345f/1000f*height,500f/1000f*width,300f/1000f*height),"A", sty_BottomAST);
		GUI.Label(new Rect(64f/1000f*width,346f/1000f*height,500f/1000f*width,300f/1000f*height),"> ", sty_Cursor);
		GUI.Label(new Rect(350f/1000f*width,373f/1000f*height,500f/1000f*width,300f/1000f*height),"S              T        ", sty_BottomAST);
		GUI.Label(new Rect(360f/1000f*width,371f/1000f*height,500f/1000f*width,300f/1000f*height),NumberFormat(SpindleSpeed), sty_SmallNum);
		GUI.Label(new Rect(463f/1000f*width,371f/1000f*height,500f/1000f*width,300f/1000f*height),ToolNumFormat(ToolNo), sty_SmallNum);
		GUI.Label(new Rect(40f/1000f*width,395f/1000f*height,230f/1000f*width,25f/1000f*height),"", sty_BottomLabel_1);
		GUI.Label(new Rect(272f/1000f*width,395f/1000f*height,45f/1000f*width,25f/1000f*height),"", sty_BottomLabel_2);
		GUI.Label(new Rect(319f/1000f*width,395f/1000f*height,94f/1000f*width,25f/1000f*height),"", sty_BottomLabel_3);
		GUI.Label(new Rect(415f/1000f*width,395f/1000f*height,125f/1000f*width,25f/1000f*height),"", sty_BottomLabel_4);
		GUI.Label(new Rect(53f/1000f*width,397f/1000f*height,500f/1000f*width,300f/1000f*height), MenuDisplay, sty_ProgModeName); 
		GUI.Label(new Rect(111f/1000f*width,395f/1000f*height,500f/1000f*width,300f/1000f*height),"****", sty_Star);
	} 
	
	void ScreenPrintArea () 
	{
		if(CursorBlink)
			GUI.Label(new Rect(ProgEDITCusorPos,346f/1000f*height,500f/1000f*width,300f/1000f*height),"_", sty_Cursor);
		
		if(InputText != "")
			GUI.Label(new Rect(57f,346f/1000f*height,500f/1000f*width,300f/1000f*height),InputText, sty_Cursor);
		
		if(ProgProtectWarn)
			GUI.Label(new Rect(33f,372f/1000f*height,450f/1000f*width,300f/1000f*height),"WRITE PROTECT", sty_Warning);
		
		if(EmergencyCtrl == false)
			GUI.Label(new Rect(171f/1000f*width,395f/1000f*height,500f/1000f*width,300f/1000f*height),"*** ***", sty_Star);
		 
		GUI.Label(new Rect(323f/1000f*width,395f/1000f*height,300f/1000f*width,300f/1000f*height),""+System.DateTime.Now.ToString("HH:mm:ss"),sty_ClockStyle);
		//ALM  blink
		if(Time.time - BlinkTime > 0.5f)
		{
			ALMBlink = !ALMBlink;
			CursorBlink = !CursorBlink;
			EmergencyBlink = !EmergencyBlink;
			BlinkTime = Time.time;
		}
		
		if(ALMBlink)
			GUI.Label(new Rect(273f/1000f*width,395f/1000f*height,42f/1000f*width,22f/1000f*height),"ALM", sty_Alarm);
			
		if(EmergencyCtrl)
		{
			if(EmergencyBlink)
				GUI.Label(new Rect(171f/1000f*width,395f/1000f*height,95f/1000f*width,22f/1000f*height),"  -- EMG --", sty_Alarm);
		}
	}


	
	void NCPowerButton () {
		
		//NC系统开
		if (GUI.Button(new Rect(75f/1000f*width, 570f/1000f*height, 80f/1000f*width, 40f/1000f*height), "", sty_NCPowerOn))            
		{
			if(ScreenPower == false)
			{
				sty_NCPowerOn.normal.background = t2d_NCPower_on_d;
				ScreenCover = true;
				power_notification = false;
			    StartCoroutine(ScreenCoverSet());
				F_operationButton();
			}
			ScreenPower = true;
		}
		
		//NC系统关
		if (GUI.Button(new Rect(75f/1000f*width, 660f/1000f*height, 80f/1000f*width, 40f/1000f*height), "", sty_NCPowerOff))            
		{
			if(ScreenPower)
			{
				sty_NCPowerOn.normal.background = t2d_NCPower_on_u;
				ScreenCover = true;
			    StartCoroutine(ScreenCoverSet());
				F_operationButton();
			}
			ScreenPower = false;
		}
	}
	
	void F_operationButton()//内容--控制电源关闭和开启时F0，F25,F50,F100按钮的状态，姓名--刘旋，时间--2013-4-12
	{
		if(ScreenPower==false)
		{
			if(PlayerPrefs.HasKey("F_SpeedMode"))
			    RapidSpeedMode = PlayerPrefs.GetInt("F_SpeedMode");
		    else
		    {
			    PlayerPrefs.SetInt("F_SpeedMode", 2);
			    RapidSpeedMode = 2;
		    }
			switch(RapidSpeedMode)
			{
			case 0:
				F0_flag=true;
				F25_flag=false;
				F50_flag=false;
				F100_flag=false;
				sty_ButtonF0.active.background=t2d_f0_on_d;
				sty_ButtonF0.normal.background=t2d_f0_on_u; 
				sty_ButtonF25.active.background=t2d_f25_off_d;
				sty_ButtonF25.normal.background=t2d_f25_off_u;
				sty_ButtonF50.active.background=t2d_f50_off_d;
				sty_ButtonF50.normal.background=t2d_f50_off_u;
				sty_ButtonF100.active.background=t2d_f100_off_d;
				sty_ButtonF100.normal.background=t2d_f100_off_u;
				break;
			case 1:
				F0_flag=false;
				F25_flag=true;
				F50_flag=false;
				F100_flag=false;
				sty_ButtonF25.active.background=t2d_f25_on_d;
				sty_ButtonF25.normal.background=t2d_f25_on_u;						    
				sty_ButtonF0.active.background=t2d_f0_off_d;
				sty_ButtonF0.normal.background=t2d_f0_off_u;
				sty_ButtonF50.active.background=t2d_f50_off_d;
				sty_ButtonF50.normal.background=t2d_f50_off_u;
				sty_ButtonF100.active.background=t2d_f100_off_d;
				sty_ButtonF100.normal.background=t2d_f100_off_u;
				break;
			case 2:
				F0_flag=false;
				F25_flag=false;
				F50_flag=true;
				F100_flag=false;
				sty_ButtonF50.active.background=t2d_f50_on_d;
				sty_ButtonF50.normal.background=t2d_f50_on_u;
				sty_ButtonF0.active.background=t2d_f0_off_d;
				sty_ButtonF0.normal.background=t2d_f0_off_u;
				sty_ButtonF25.active.background=t2d_f25_off_d;
				sty_ButtonF25.normal.background=t2d_f25_off_u;
				sty_ButtonF100.active.background=t2d_f100_off_d;
				sty_ButtonF100.normal.background=t2d_f100_off_u;
				break;
			case 3:
				F0_flag=false;
				F25_flag=false;
				F50_flag=false;
				F100_flag=true;
				sty_ButtonF100.active.background=t2d_f100_on_d;
				sty_ButtonF100.normal.background=t2d_f100_on_u;						    
				sty_ButtonF0.active.background=t2d_f0_off_d;
				sty_ButtonF0.normal.background=t2d_f0_off_u;
				sty_ButtonF25.active.background=t2d_f25_off_d;
				sty_ButtonF25.normal.background=t2d_f25_off_u;
				sty_ButtonF50.active.background=t2d_f50_off_d;
				sty_ButtonF50.normal.background=t2d_f50_off_u;
				break;
			}
		
		}
		else
		{
			sty_ButtonF0.active.background=t2d_f0_off_d;
			sty_ButtonF0.normal.background=t2d_f0_off_u; 
			sty_ButtonF25.active.background=t2d_f25_off_d;
			sty_ButtonF25.normal.background=t2d_f25_off_u;
			sty_ButtonF50.active.background=t2d_f50_off_d;
			sty_ButtonF50.normal.background=t2d_f50_off_u;
			sty_ButtonF100.active.background=t2d_f100_off_d;
			sty_ButtonF100.normal.background=t2d_f100_off_u;
		}
	}
	
	void ManualOperationButton () {
		
		if (GUI.Button(new Rect(540f/1000f*width, 850f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_ButtonRapid))            
		{
			if(ScreenPower)
			{
				if(RapidMoveFlag)
				{
					RapidMoveFlag = false;
					MoveControl_script.speed_to_move = 0.16667F;//内容--JOG模式下，慢常速为10m/min=(10/60)m/s,因此spee-to-move=10/60,姓名--刘旋，时间--2013-4-8
					MoveControl_script.move_rate = move_rate;
					sty_ButtonRapid.normal.background = t2d_rapid_off_u;
					sty_ButtonRapid.active.background = t2d_rapid_off_d;
				}
				else
				{
					RapidMoveFlag = true;
					MoveControl_script.speed_to_move = 0.6F;//内容--JOG模式下，快常速为36m/min=(36/60)m/s,因此spee-to-move=36/60,姓名--刘旋，时间--2013-4-8
					MoveControl_script.move_rate = 1f;//内容--JOG模式下，实际进给速率倍率的修改，不恒为1，与进给面板数值保持一致，姓名--刘旋，时间--2013-4-8
					sty_ButtonRapid.normal.background = t2d_rapid_on_u;
					sty_ButtonRapid.active.background = t2d_rapid_on_d;
				}
			}
			else
			{
				power_notification = true;
				GUI.BringWindowToBack(0);
			}
		}
		
		if (GUI.Button(new Rect(480f/1000f*width, 790f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_Button4P))            
		{
			if(ScreenPower)
			{
					
			}
			else
			{
				power_notification = true;
			}
		}
		
		if (GUI.Button(new Rect(480f/1000f*width, 910f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_Button4N))            
		{
			if(ScreenPower)
			{
					
			}
			else
			{
				power_notification = true;
			}
		}
		
		if(GUI.RepeatButton(new Rect(540f/1000f*width, 790f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_ButtonYN))
		{
			if(ScreenPower)
			{
				if(ProgJOG)
				{
					if(RapidMoveFlag)
					{
						MoveControl_script.speed_to_move = 0.6F;//内容--JOG模式下，快常速为36m/min=(36/60)m/s,因此spee-to-move=36/60,姓名--刘旋，时间--2013-4-8
						MoveControl_script.move_rate = 1f;//内容--JOG模式下，实际进给速率倍率的修改，恒为1，姓名--刘旋，时间--2013-4-11
						switch(RapidSpeedMode)//内容--快常速四种状态作用于实际速度，姓名--刘旋，时间--2013-4-9
							{
						    case 0://模式0（即;F0状态）
									MoveControl_script.speed_to_move=0*MoveControl_script.speed_to_move;//F0模式下，为停止，即：实际速度为0
								    break;
							case 1://模式0（即;F25状态）
								    MoveControl_script.speed_to_move=0.25f*MoveControl_script.speed_to_move;//F25模式下，实际速度为快常速的25%
								    break;
							case 2://模式0（即;F50状态）
									MoveControl_script.speed_to_move=0.5f*MoveControl_script.speed_to_move;//F50模式下，实际速度为快常速的50%
								    break;
							case 3://模式0（即;F100状态）
								    MoveControl_script.speed_to_move=1f*MoveControl_script.speed_to_move;//F100模式下，实际速度等于快常速
								    break;	
									
							}//增加内容到此  2013-4-9
					}
					else
					{
						MoveControl_script.speed_to_move = 0.16667F;//内容--JOG模式下，慢常速为10m/min=(10/60)m/s,因此spee-to-move=10/60,姓名--刘旋，时间--2013-4-8				
						MoveControl_script.move_rate = move_rate;
					}
					if(MoveControl_script.Y_part.position.x - MoveControl_script.MachineZero.x >= 0.5f)
						MoveControl_script.y_n = false;
					else
						MoveControl_script.y_n = true;
				}
			}
			else
			{
				power_notification = true;
			}
		}
		else
			MoveControl_script.y_n = false;
		
		if(GUI.RepeatButton(new Rect(600f/1000f*width, 850f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_ButtonXN))
		{
			if(ScreenPower)
			{
				if(ProgJOG)
				{
					if(RapidMoveFlag)
					{
						MoveControl_script.speed_to_move = 0.6F;//内容--JOG模式下，快常速为36m/min=(36/60)m/s,因此spee-to-move=36/60,姓名--刘旋，时间--2013-4-8
						MoveControl_script.move_rate = 1f;//内容--JOG模式下，实际进给速率倍率的修改，恒为1，姓名--刘旋，时间--2013-4-11
						switch(RapidSpeedMode)//内容--快常速四种状态作用于实际速度，姓名--刘旋，时间--2013-4-9
							{
						    case 0://模式0（即;F0状态）
									MoveControl_script.speed_to_move=0*MoveControl_script.speed_to_move;//F0模式下，为停止，即：实际速度为0
								    break;
							case 1://模式0（即;F25状态）
								    MoveControl_script.speed_to_move=0.25f*MoveControl_script.speed_to_move;//F25模式下，实际速度为快常速的25%
								    break;
							case 2://模式0（即;F50状态）
									MoveControl_script.speed_to_move=0.5f*MoveControl_script.speed_to_move;//F50模式下，实际速度为快常速的50%
								    break;
							case 3://模式0（即;F100状态）
								    MoveControl_script.speed_to_move=1f*MoveControl_script.speed_to_move;//F100模式下，实际速度等于快常速
								    break;	
									
							}//增加内容到此  2013-4-9
					}
					else
					{
						MoveControl_script.speed_to_move = 0.16667F;//内容--JOG模式下，慢常速为10m/min=(10/60)m/s,因此spee-to-move=10/60,姓名--刘旋，时间--2013-4-8
						MoveControl_script.move_rate = move_rate;
					}
					if(MoveControl_script.MachineZero.z - MoveControl_script.X_part.position.z >= 0.8f)
						MoveControl_script.x_n = false;
					else
						MoveControl_script.x_n = true;
				}
			}
			else
			{
				power_notification = true;
			}
		}
		else
			MoveControl_script.x_n = false;
		
		if(GUI.RepeatButton(new Rect(600f/1000f*width, 910f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_ButtonZN))
		{
			if(ScreenPower)
			{
				if(ProgJOG)
				{
					if(RapidMoveFlag)
					{
						MoveControl_script.speed_to_move = 0.6F;//内容--JOG模式下，快常速为36m/min=(36/60)m/s,因此spee-to-move=36/60,姓名--刘旋，时间--2013-4-8
						MoveControl_script.move_rate = 1f;//内容--JOG模式下，实际进给速率倍率的修改，恒为1，姓名--刘旋，时间--2013-4-11
						switch(RapidSpeedMode)//内容--快常速四种状态作用于实际速度，姓名--刘旋，时间--2013-4-9
							{
						    case 0://模式0（即;F0状态）
									MoveControl_script.speed_to_move=0*MoveControl_script.speed_to_move;//F0模式下，为停止，即：实际速度为0
								    break;
							case 1://模式0（即;F25状态）
								    MoveControl_script.speed_to_move=0.25f*MoveControl_script.speed_to_move;//F25模式下，实际速度为快常速的25%
								    break;
							case 2://模式0（即;F50状态）
									MoveControl_script.speed_to_move=0.5f*MoveControl_script.speed_to_move;//F50模式下，实际速度为快常速的50%
								    break;
							case 3://模式0（即;F100状态）
								    MoveControl_script.speed_to_move=1f*MoveControl_script.speed_to_move;//F100模式下，实际速度等于快常速
								    break;	
									
							}//增加内容到此  2013-4-9
					}
					else
					{
						MoveControl_script.speed_to_move = 0.16667F;//内容--JOG模式下，慢常速为10m/min=(10/60)m/s,因此spee-to-move=10/60,姓名--刘旋，时间--2013-4-8
						MoveControl_script.move_rate = move_rate;
					}
					if(MoveControl_script.MachineZero.y - MoveControl_script.Z_part.position.y >= 0.51f)
						MoveControl_script.z_n = false;
					else
						MoveControl_script.z_n = true;
				}
			}
			else
			{
				power_notification = true;
			}
		}
		else
			MoveControl_script.z_n = false;
		
		if(ProgREF)
		{
			if(GUI.Button(new Rect(600f/1000f*width, 790f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_ButtonZP))
			{
				if(ScreenPower)
				{
					MoveControl_script.move_rate = 1f;//内容--归零模式下，实际进给速率倍率的修改，恒为1,姓名--刘旋，时间--2013-4-11
					MoveControl_script.speed_to_move = 0.6F;//内容--归零操作的实际速度为36m/min=0.6m/s，而实际速度RunningSpeed=speed—to-move*move-rate，因此speed-to-move应设为0.6,姓名--刘旋，时间--2013-4-8
					MoveControl_script.z_p = true;
				}
				else
					power_notification = true;
			}
			
			if(GUI.Button(new Rect(480f/1000f*width, 850f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_ButtonXP))
			{
				if(ScreenPower)
				{
					MoveControl_script.move_rate = 1f;//内容--归零模式下，实际进给速率倍率的修改，恒为1,姓名--刘旋，时间--2013-4-11
					MoveControl_script.speed_to_move = 0.6F;//内容--归零操作的实际速度为36m/min=0.6m/s，而实际速度RunningSpeed=speed—to-move*move-rate，因此speed-to-move应设为0.6,姓名--刘旋，时间--2013-4-8
					MoveControl_script.x_p = true;
				}
				else
					power_notification = true;
			}
			
			if(GUI.RepeatButton(new Rect(540f/1000f*width, 910f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_ButtonYP))
			{
				if(ScreenPower)
				{
					MoveControl_script.move_rate = 1f;//内容--归零模式下，实际进给速率倍率的修改，恒为1,姓名--刘旋，时间--2013-4-11
					MoveControl_script.speed_to_move = 0.6F;//内容--归零操作的实际速度为36m/min=0.6m/s，而实际速度RunningSpeed=speed—to-move*move-rate，因此speed-to-move应设为0.6,姓名--刘旋，时间--2013-4-8
					MoveControl_script.y_p = true;
				}
				else
					power_notification = true;
			}
		}
		else
		{
			if(GUI.RepeatButton(new Rect(600f/1000f*width, 790f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_ButtonZP))
			{
				if(ScreenPower)
				{
					if(ProgJOG)
					{
						if(RapidMoveFlag)
						{
							MoveControl_script.speed_to_move = 0.6F;//内容--JOG模式下，快常速为36m/min=(36/60)m/s,因此spee-to-move=36/60,姓名--刘旋，时间--2013-4-8
							MoveControl_script.move_rate = 1f;//内容--JOG模式下，实际进给速率倍率的修改，恒为1，姓名--刘旋，时间--2013-4-11
							switch(RapidSpeedMode)//内容--快常速四种状态作用于实际速度，姓名--刘旋，时间--2013-4-9
								{
							    case 0://模式0（即;F0状态）
										MoveControl_script.speed_to_move=0*MoveControl_script.speed_to_move;//F0模式下，为停止，即：实际速度为0
									    break;
								case 1://模式0（即;F25状态）
									    MoveControl_script.speed_to_move=0.25f*MoveControl_script.speed_to_move;//F25模式下，实际速度为快常速的25%
									    break;
								case 2://模式0（即;F50状态）
										MoveControl_script.speed_to_move=0.5f*MoveControl_script.speed_to_move;//F50模式下，实际速度为快常速的50%
									    break;
								case 3://模式0（即;F100状态）
									    MoveControl_script.speed_to_move=1f*MoveControl_script.speed_to_move;//F100模式下，实际速度等于快常速
									    break;	
										
								}//增加内容到此  2013-4-9
						}
						else
						{
							MoveControl_script.speed_to_move = 0.16667F;//内容--JOG模式下，慢常速为10m/min=(10/60)m/s,因此spee-to-move=10/60,姓名--刘旋，时间--2013-4-8
							MoveControl_script.move_rate = move_rate;
						}
						if(MoveControl_script.MachineZero.y - MoveControl_script.Z_part.position.y <= 0)
							MoveControl_script.z_p = false;
						else
							MoveControl_script.z_p = true;
					}
				}
				else
				{
					power_notification = true;
				}
			}
			else
				MoveControl_script.z_p = false;
			
			if(GUI.RepeatButton(new Rect(480f/1000f*width, 850f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_ButtonXP))
			{
				if(ScreenPower)
				{
					if(ProgJOG)
					{
						if(RapidMoveFlag)
						{
							MoveControl_script.speed_to_move = 0.6F;//内容--JOG模式下，快常速为36m/min=(36/60)m/s,因此spee-to-move=36/60,姓名--刘旋，时间--2013-4-8
							MoveControl_script.move_rate = 1f;//内容--JOG模式下，实际进给速率倍率的修改，恒为1，姓名--刘旋，时间--2013-4-11
							switch(RapidSpeedMode)//内容--快常速四种状态作用于实际速度，姓名--刘旋，时间--2013-4-9
								{
							    case 0://模式0（即;F0状态）
										MoveControl_script.speed_to_move=0*MoveControl_script.speed_to_move;//F0模式下，为停止，即：实际速度为0
									    break;
								case 1://模式0（即;F25状态）
									    MoveControl_script.speed_to_move=0.25f*MoveControl_script.speed_to_move;//F25模式下，实际速度为快常速的25%
									    break;
								case 2://模式0（即;F50状态）
										MoveControl_script.speed_to_move=0.5f*MoveControl_script.speed_to_move;//F50模式下，实际速度为快常速的50%
									    break;
								case 3://模式0（即;F100状态）
									    MoveControl_script.speed_to_move=1f*MoveControl_script.speed_to_move;//F100模式下，实际速度等于快常速
									    break;	
										
								}//增加内容到此  2013-4-9
						}
						else
						{
							MoveControl_script.speed_to_move = 0.16667F;//内容--JOG模式下，慢常速为10m/min=(10/60)m/s,因此spee-to-move=10/60,姓名--刘旋，时间--2013-4-8
							MoveControl_script.move_rate = move_rate;
						}
						if(MoveControl_script.MachineZero.z - MoveControl_script.X_part.position.z <= 0)
						{
							MoveControl_script.x_p = false;
							//MoveControl_script.move_flag=false;
						}
						else
						{
						   MoveControl_script.x_p = true;
							//MoveControl_script.move_flag=true;
						}
					}
				}
				else
				{
					power_notification = true;
				}
			}
			else
				MoveControl_script.x_p = false;
			
			if(GUI.RepeatButton(new Rect(540f/1000f*width, 910f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_ButtonYP))
			{
				if(ScreenPower)
				{
					if(ProgJOG)
					{
						if(RapidMoveFlag)
						{
							MoveControl_script.speed_to_move = 0.6F;//内容--JOG模式下，快常速为36m/min=(36/60)m/s,因此spee-to-move=36/60,姓名--刘旋，时间--2013-4-8
							MoveControl_script.move_rate = 1f;//内容--JOG模式下，实际进给速率倍率的修改，恒为1，姓名--刘旋，时间--2013-4-11
							switch(RapidSpeedMode)//内容--快常速四种状态作用于实际速度，姓名--刘旋，时间--2013-4-9
								{
							    case 0://模式0（即;F0状态）
										MoveControl_script.speed_to_move=0*MoveControl_script.speed_to_move;//F0模式下，为停止，即：实际速度为0
									    break;
								case 1://模式0（即;F25状态）
									    MoveControl_script.speed_to_move=0.25f*MoveControl_script.speed_to_move;//F25模式下，实际速度为快常速的25%
									    break;
								case 2://模式0（即;F50状态）
										MoveControl_script.speed_to_move=0.5f*MoveControl_script.speed_to_move;//F50模式下，实际速度为快常速的50%
									    break;
								case 3://模式0（即;F100状态）
									    MoveControl_script.speed_to_move=1f*MoveControl_script.speed_to_move;//F100模式下，实际速度等于快常速
									    break;	
										
								}//增加内容到此  2013-4-9
						}
						else
						{
							MoveControl_script.speed_to_move = 0.16667F;//内容--JOG模式下，慢常速为10m/min=(10/60)m/s,因此spee-to-move=10/60,姓名--刘旋，时间--2013-4-8						
							MoveControl_script.move_rate = move_rate;
						}
						if(MoveControl_script.Y_part.position.x - MoveControl_script.MachineZero.x <= 0)
							MoveControl_script.y_p = false;
						else
							MoveControl_script.y_p = true;
					}
				}
				else
				{
					power_notification = true;
				}
			}
			else
				MoveControl_script.y_p = false;
			
			
		}
	}
	
	void SpindleControl () {
		
		if (GUI.Button(new Rect(680f/1000f*width, 790f/1000f*height, 50f/1000f*width, 50f/1000f*height), "",sty_ButtonF0))//内容--修改F0按钮的图片显示，姓名--刘旋，时间2013-4-8            
		{
			if(ScreenPower)
			{
				if(ProgJOG)//内容--JOG模式下，实现F0按钮的功能，姓名--刘旋，时间--2013-4-9
				{
					if(RapidMoveFlag)//内容，在快速模式下，F0的功能，姓名--刘旋，时间--2013-4-11
					{
						F0_flag=true;
						F25_flag=false;
						F50_flag=false;
						F100_flag=false;
						sty_ButtonF0.active.background=t2d_f0_on_d;
						sty_ButtonF0.normal.background=t2d_f0_on_u; 
						sty_ButtonF25.active.background=t2d_f25_off_d;
						sty_ButtonF25.normal.background=t2d_f25_off_u;
						sty_ButtonF50.active.background=t2d_f50_off_d;
						sty_ButtonF50.normal.background=t2d_f50_off_u;
						sty_ButtonF100.active.background=t2d_f100_off_d;
						sty_ButtonF100.normal.background=t2d_f100_off_u;
						RapidSpeedMode=0;//快常速模式状态为0
						PlayerPrefs.SetInt("F_SpeedMode", 0);    
					
					}
					else//内容--慢速模式下，F0按钮的功能。姓名--刘旋，时间--2013-4-9
					{
						
				    }
			    }//增加内容到此  2013-4-9
			}
		}
		
		if (GUI.Button(new Rect(750f/1000f*width, 790f/1000f*height, 50f/1000f*width, 50f/1000f*height), "",sty_ButtonF25))//内容--修改25%按钮的图片显示，姓名--刘旋，时间2013-4-8            
		{
			if(ScreenPower)
			{
				if(ProgJOG)//内容--JOG模式下，实现F25按钮的功能，姓名--刘旋，时间--2013-4-9
				{
					if(RapidMoveFlag)//内容，在快速模式下，F25的功能，姓名--刘旋，时间--2013-4-11
					{
						F0_flag=false;
					    F25_flag=true;
						F50_flag=false;
						F100_flag=false;
						sty_ButtonF25.active.background=t2d_f25_on_d;
						sty_ButtonF25.normal.background=t2d_f25_on_u;						    
						sty_ButtonF0.active.background=t2d_f0_off_d;
						sty_ButtonF0.normal.background=t2d_f0_off_u;
						sty_ButtonF50.active.background=t2d_f50_off_d;
						sty_ButtonF50.normal.background=t2d_f50_off_u;
						sty_ButtonF100.active.background=t2d_f100_off_d;
						sty_ButtonF100.normal.background=t2d_f100_off_u;
						RapidSpeedMode=1;//快常速模式状态为1
						PlayerPrefs.SetInt("F_SpeedMode", 1);
					
					}
					else//内容--慢速模式下，F25按钮的功能。姓名--刘旋，时间--2013-4-9
					{
						
				    }
			    }//增加内容到此  2013-4-9
			}
		}
		
		if (GUI.Button(new Rect(820f/1000f*width, 790f/1000f*height, 50f/1000f*width, 50f/1000f*height), "",sty_ButtonF50))//内容--修改50%按钮的图片显示，姓名--刘旋，时间2013-4-8            
		{
			if(ScreenPower)
			{
				if(ProgJOG)//内容--JOG模式下，实现F50按钮的功能，姓名--刘旋，时间--2013-4-9
				{
					if(RapidMoveFlag)//内容，在快速模式下，F50的功能，姓名--刘旋，时间--2013-4-11
					{
						F0_flag=false;
						F25_flag=false;
						F50_flag=true;
						F100_flag=false;
						sty_ButtonF50.active.background=t2d_f50_on_d;
						sty_ButtonF50.normal.background=t2d_f50_on_u;
						sty_ButtonF0.active.background=t2d_f0_off_d;
						sty_ButtonF0.normal.background=t2d_f0_off_u;
						sty_ButtonF25.active.background=t2d_f25_off_d;
						sty_ButtonF25.normal.background=t2d_f25_off_u;
						sty_ButtonF100.active.background=t2d_f100_off_d;
						sty_ButtonF100.normal.background=t2d_f100_off_u;
						RapidSpeedMode=2;//快常速模式状态为2
						PlayerPrefs.SetInt("F_SpeedMode", 2);
				
						
					}
					else//内容--慢速模式下，F50按钮的功能。姓名--刘旋，时间--2013-4-9
					{
				    }
			    }//增加内容到此  2013-4-9
			}
		}
		
		if (GUI.Button(new Rect(890f/1000f*width, 790f/1000f*height, 50f/1000f*width, 50f/1000f*height), "",sty_ButtonF100))//内容--修改100%按钮的图片显示，姓名--刘旋，时间2013-4-8            
		{
			if(ScreenPower)
			{
				if(ProgJOG)//内容--JOG模式下，实现F100按钮的功能，姓名--刘旋，时间--2013-4-9
				{
					if(RapidMoveFlag)//内容，在快速模式下，F100的功能，姓名--刘旋，时间--2013-4-11
					{
						F0_flag=false;
						F25_flag=false;
						F50_flag=false;
						F100_flag=true;
						sty_ButtonF100.active.background=t2d_f100_on_d;
						sty_ButtonF100.normal.background=t2d_f100_on_u;						    
						sty_ButtonF0.active.background=t2d_f0_off_d;
						sty_ButtonF0.normal.background=t2d_f0_off_u;
						sty_ButtonF25.active.background=t2d_f25_off_d;
						sty_ButtonF25.normal.background=t2d_f25_off_u;
						sty_ButtonF50.active.background=t2d_f50_off_d;
						sty_ButtonF50.normal.background=t2d_f50_off_u;	
						RapidSpeedMode=3;//快常速模式状态为3
						PlayerPrefs.SetInt("F_SpeedMode", 3);
						
					}
					else//内容--慢速模式下，F100按钮的功能。姓名--刘旋，时间--2013-4-9
					{
				    }
			    }//增加内容到此  2013-4-9
			}
		}
		
		if(GUI.Button(new Rect(680f/1000f*width, 850f/1000f*height, 50f/1000f*width, 50f/1000f*height), ""))
		{
			if(ScreenPower)
			{
				
			}
		}
		
		if (GUI.Button(new Rect(750f/1000f*width, 850f/1000f*height, 50f/1000f*width, 50f/1000f*height), "DOWN"))            
		{
			if(ScreenPower)
			{
				
			}
		}
		
		if (GUI.Button(new Rect(820f/1000f*width, 850f/1000f*height, 50f/1000f*width, 50f/1000f*height), "100%"))
		{
			if(ScreenPower)
			{
				
			}
		}
		
		if (GUI.Button(new Rect(890f/1000f*width, 850f/1000f*height, 50f/1000f*width, 50f/1000f*height), "UP"))
		{
			if(ScreenPower)
			{
				
			}
		}
		
		
		
		if (GUI.Button(new Rect(680f/1000f*width, 910f/1000f*height, 50f/1000f*width, 50f/1000f*height), "ORIENT"))            
		{
			if(ScreenPower)
			{
				
			}
		}
		
		if (GUI.Button(new Rect(750f/1000f*width, 910f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_ButtonCW))            
		{
			if(ScreenPower)
			{
				if(SpindleControl_script.rotate_flag == false)
				{
					SpindleControl_script.audio_flag = false;
					SpindleControl_script.SpindleSoundOff();
				}
				
				if(SpindleControl_script.cw_rotate_flag == false)
				{
					SpindleControl_script.audio_flag = false;
					SpindleControl_script.SpindleSoundOff();
				}
			    SpindleControl_script.rotate_flag = true;
				SpindleControl_script.cw_rotate_flag = true;
				
				sty_ButtonCW.normal.background = t2d_spCW_on_u;
				sty_ButtonCW.active.background = t2d_spCW_on_d;
				
				sty_ButtonCCW.normal.background = t2d_spCCW_off_u;
				sty_ButtonCCW.active.background = t2d_spCCW_off_d;
				
				sty_ButtonSTOP.normal.background = t2d_spStop_off_u;
				sty_ButtonSTOP.active.background = t2d_spStop_off_d;
			}
			else
			{
				power_notification = true;
			}
		}
		
		if (GUI.Button(new Rect(820f/1000f*width, 910f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_ButtonSTOP))            
		{
			if(ScreenPower)
			{
				SpindleControl_script.rotate_flag = false;
				SpindleControl_script.audio_flag = false;
				SpindleControl_script.SpindleSoundOff();
				
				sty_ButtonCW.normal.background = t2d_spCW_off_u;
				sty_ButtonCW.active.background = t2d_spCW_off_d;
		
				sty_ButtonCCW.normal.background = t2d_spCCW_off_u;
				sty_ButtonCCW.active.background = t2d_spCCW_off_d;
				
				sty_ButtonSTOP.normal.background = t2d_spStop_on_u;
				sty_ButtonSTOP.active.background = t2d_spStop_on_d;
			}
			else
			{
				power_notification = true;
			}
		}
		
		if (GUI.Button(new Rect(890f/1000f*width, 910f/1000f*height, 50f/1000f*width, 50f/1000f*height), "", sty_ButtonCCW))            
		{
			if(ScreenPower)
			{
				if(SpindleControl_script.rotate_flag == false)
				{
					SpindleControl_script.audio_flag = false;
					SpindleControl_script.SpindleSoundOff();
				}
				if(SpindleControl_script.cw_rotate_flag)
				{
					SpindleControl_script.audio_flag = false;
					SpindleControl_script.SpindleSoundOff();
				}
				SpindleControl_script.rotate_flag = true;
				SpindleControl_script.cw_rotate_flag = false;
				
				sty_ButtonCCW.normal.background = t2d_spCCW_on_u;
				sty_ButtonCCW.active.background = t2d_spCCW_on_d;
				
				sty_ButtonCW.normal.background = t2d_spCW_off_u;
				sty_ButtonCW.active.background = t2d_spCW_off_d;
				
				sty_ButtonSTOP.normal.background = t2d_spStop_off_u;
				sty_ButtonSTOP.active.background = t2d_spStop_off_d;
			}
			else
			{
				power_notification = true;
			}
		}
	}
	
	
	
	string TimeTest (float TimeValue)
	{	
		if(TimeValue <= 0)
	        return "00:00:00";
		string LastTime = "";
		int hour = Mathf.FloorToInt(TimeValue/3600%24);
		if(hour/10 >=1)
			LastTime+="" + hour;
        else
			LastTime +="0" + hour;
        int minute = Mathf.FloorToInt(TimeValue/60%60);
		if(minute/10 >=1)
			LastTime+=":" + minute;
        else
			LastTime +=":0" + minute;
        int second = Mathf.FloorToInt(TimeValue%60);
		if(second/10 >=1)
			LastTime+=":" + second;
		else
			LastTime +=":0" + second;
        return LastTime;	
	}
	
	
	void Update () {
		
	}
	
	//格式化显示数字
	public string CooStringGet (float StrValue) 
	{
		int intNum = 0;	
		string DisplayStr = "";
		intNum = (int)StrValue;
		//整数部分转化为string
		if(intNum < 0)
		{
			if(intNum > -100 && intNum <= -10)
				DisplayStr = "   " + intNum + ".";
			else if(intNum > -10)
				DisplayStr = "    " + intNum + ".";
			else if(intNum <= -100 && intNum > -1000)
				DisplayStr = "  "+intNum + ".";
			else
				DisplayStr = " "+intNum + ".";
		}
		else if(intNum == 0)
		{
			if(StrValue < 0)
				DisplayStr = "    -0.";
			else		
			    DisplayStr = "     0" + ".";
		}
		else
		{
			if(intNum < 100 && intNum >= 10)
				DisplayStr = "    " + intNum + ".";
			else if(intNum < 10)
				DisplayStr = "     " + intNum + ".";
			else if(intNum < 1000 && intNum >= 100)	
				DisplayStr = "   " + intNum + ".";
			else	
				DisplayStr = "  " + intNum + ".";
		}
		//小数部分转化为string
		intNum = (int)(Math.Abs(StrValue*10) % 10);
		DisplayStr += intNum;
		intNum = (int)(Math.Abs(StrValue*100) % 10);
		DisplayStr += intNum;
		intNum = (int)(Math.Abs(StrValue*1000) % 10);
		DisplayStr += intNum;
		return DisplayStr;	
	}
	
	public string NumberFormat (int C_Num) 
	{
		string C_Str = "";
		if(C_Num >= 100000)
			C_Str = "" + C_Num;
		else if(C_Num < 100000 && C_Num >= 10000)
			C_Str = " " + C_Num;
		else if(C_Num < 10000 && C_Num >= 1000)
			C_Str = "  " + C_Num;
		else if(C_Num < 1000 && C_Num >= 100)
			C_Str = "   " + C_Num;		
		else if(C_Num < 100 && C_Num >= 10)	
			C_Str = "    " + C_Num;
		else	
			C_Str = "     " + C_Num;	
		return C_Str;	
	}
	
	public string ToolNumFormat (int T_Num)
	{
		string T_Str = "";
		if(T_Num >= 1000)
			T_Str = "" + T_Num;
		else if(T_Num < 1000 && T_Num >= 100)	
			T_Str = "0" + T_Num;
		else if(T_Num < 100 && T_Num >= 10)
			T_Str = "00" + T_Num;
		else	
			T_Str = "000" + T_Num;
		return T_Str;	
	}
	
	string LineNumFormat (int T_Num)
	{
		string T_Str = "";
		if(T_Num >= 10000)	
			T_Str = "" + T_Num;
		else if(T_Num < 10000 && T_Num >= 1000)	
			T_Str = "0" + T_Num;
		else if(T_Num < 1000 && T_Num >= 100)	
			T_Str = "00" + T_Num;
		else if(T_Num < 100 && T_Num >= 10)
			T_Str = "000" + T_Num;
		else
			T_Str = "0000" + T_Num;	
		return T_Str;	
	}

	IEnumerator ScreenCoverSet () 
	{
		yield return new WaitForSeconds(0.3f);
		ScreenCover = false;	
	}
	
	//设定界面修改---陈振华---03.11
	//使设定输入右对齐，顺序号停止参数
	public string ArguStringGet(string StrValue)
	{
		string DisplayStr = "";
		if(StrValue.Length == 1)
		DisplayStr = "       "+StrValue;
		else if(StrValue.Length == 2)
		DisplayStr = "      "+StrValue;
		else if(StrValue.Length == 3)
		DisplayStr = "     "+StrValue;
		else if(StrValue.Length == 4)
		DisplayStr = "    "+StrValue;
		else if(StrValue.Length == 5)
		DisplayStr = "   "+StrValue;
		else if(StrValue.Length == 6)
		DisplayStr = "  "+StrValue;
		else if(StrValue.Length == 7)
		DisplayStr = " "+StrValue;
		return DisplayStr;	
	}
	
	//使设定输入右对齐，IO参数
	public string ArguStringGet_IO(string StrValue)
	{
		string DisplayStr = "";
		if(StrValue.Length == 1)
			DisplayStr = " "+StrValue;
		else if(StrValue.Length == 2)
			DisplayStr = StrValue;
		return DisplayStr;	
	}
	
	//刀偏界面加入---陈振华---03.30
	//格式化显示数字,刀偏界面
	public string ToolStringGet (float StrValue) 
	{
		int intNum = 0;	
		string DisplayStr = "";
		intNum = (int)StrValue;
		
		if(intNum < 0)
		{
			if(intNum > -100 && intNum <= -10)
				DisplayStr = "  " + intNum + ".";
			else if(intNum > -10)
				DisplayStr = "   " + intNum + ".";
			else if(intNum <= -100 && intNum > -1000)
				DisplayStr = " "+intNum + ".";
			else
				DisplayStr = ""+intNum + ".";
		}
		else if(intNum == 0)
		{
			if(StrValue < 0)
				DisplayStr = "   -0.";
			else		
			    DisplayStr = "    0" + ".";
		}
		else
		{
			if(intNum < 100 && intNum >= 10)
				DisplayStr = "   " + intNum + ".";
			else if(intNum < 10)
				DisplayStr = "    " + intNum + ".";
			else if(intNum < 1000 && intNum >= 100)	
				DisplayStr = "  " + intNum + ".";
			else	
				DisplayStr = " " + intNum + ".";
		}
		
		intNum = (int)(Math.Abs(StrValue*10) % 10);
		DisplayStr += intNum;
		intNum = (int)(Math.Abs(StrValue*100) % 10);
		DisplayStr += intNum;
		intNum = (int)(Math.Abs(StrValue*1000) % 10);
		DisplayStr += intNum;
		return DisplayStr;	
	}
	
	//刀偏界面加入---陈振华---03.30
	//使刀偏界面的序号为3位
	public string Tool_numberGet(int StrValue)
	{
		 string StringValue = StrValue.ToString();
		 string DisplayStr = "";
		 if(StringValue.Length == 1)
			DisplayStr = "00"+StringValue;
		 if(StringValue.Length == 2)
			DisplayStr = "0"+StringValue;
		 if(StringValue.Length == 3)
			DisplayStr = StringValue;
		  return DisplayStr;
	}

	string SingleCodeFormat (string OriCode) 
	{
		string AimCode = "";
		List<string> TempCodeSubList = new List<string>();
		if(OriCode == "%")
		{
			AimCode = "%";
			return AimCode;
		}
		else if(OriCode == "")
		{
			return ";";
		}
		else
		{
			char[] TempCharArray;
			bool SpaceFlag = false;
			TempCharArray = OriCode.ToCharArray();
			for(int k = 0; k < TempCharArray.Length; k++)
			{
				if(TempCharArray[k] >= 'A' && TempCharArray[k] <= 'Z')
				{
					if(SpaceFlag)
					{
						TempCodeSubList.Add(AimCode);
						AimCode = ""+TempCharArray[k];
						SpaceFlag = false;
					}
					else
					{
						AimCode += TempCharArray[k];
						SpaceFlag = false;
					}	
				}
				else
				{
					AimCode += TempCharArray[k];	
					SpaceFlag = true;
				}	
				if(k == TempCharArray.Length - 1)
				{
					TempCodeSubList.Add(AimCode);
					AimCode = "";
					SpaceFlag = false;
					TempCodeSubList.Add(";");
				}
			}
			AimCode = "";	
			for(int i = 0; i < TempCodeSubList.Count; i++)
			{
				EDITText.text = AimCode + TempCodeSubList[i];
				TextSize = sty_EDITTextField.CalcSize(new GUIContent(EDITText.text));
				if(TextSize.x > 320f)
				{
					break;
				}
				else
				{
					AimCode	= AimCode + TempCodeSubList[i] + "  ";
				}
			}
		}	
		return AimCode.TrimEnd();
	}
	
	
}
