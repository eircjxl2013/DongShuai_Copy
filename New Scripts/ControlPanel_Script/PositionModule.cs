using UnityEngine;
using System.Collections;

public class PositionModule : MonoBehaviour {
	ControlPanel Main;
	CooSystem CooSystem_script;
	MoveControl MoveControl_script;
	//位置界面功能完善---宋荣 ---03.09
	MDIInputModule MDIInput_script;
	float lastTime=0;//闪烁时记录上次时间
	float xyzLastTime=0;
	bool displayFlag=true;
	bool xDisplayFlag=true;
	bool yDisplayFlag=true;
	bool zDisplayFlag=true;
	public bool xBlink=false;
	public bool yBlink=false;
	public bool zBlink=false;
	public Vector3 preSetAbsoluteCoo=new Vector3(0,0,0);
	public Vector3 preSetRelativeCoo=new Vector3(0,0,0);
	//位置界面功能完善---宋荣 ---03.09
	
	// Use this for initialization
	void Start () {
		Main = gameObject.GetComponent<ControlPanel>();
		CooSystem_script = gameObject.GetComponent<CooSystem>();
		MoveControl_script = GameObject.Find("move_control").GetComponent<MoveControl>();
		//位置界面功能完善---宋荣 ---03.09
		MDIInput_script=gameObject.GetComponent<MDIInputModule>();
		//位置界面功能完善---宋荣 ---03.09
	}
	
	public void Position () {
		//绝对坐标界面
		if(Main.AbsoluteCoo)	
		{
			PosAbsoluteCoo();
		 }
		//相对坐标界面
		if(Main.RelativeCoo)
		{
			PosRelativeCoo();
		}
		//综合界面
		if(Main.GeneralCoo)
		{
			PosGeneralCoo();
		}
		//宋荣
		if(Main.operationBottomScrInitial)
		{
			//Debug.Log("showoperationscreen");
			if(MDIInput_script.isXSelected)
			{
				xBlink=true;
				yBlink=false;
				zBlink=false;
				if(Time.time-xyzLastTime>0.5)
				{
					xDisplayFlag=!xDisplayFlag;
				    xyzLastTime=Time.time;
					Debug.Log("isXBlink");
				}
			}
			if(MDIInput_script.isYSelected)
			{
				yBlink=true;
				xBlink=false;
				zBlink=false;
				if(Time.time-xyzLastTime>0.5)
				{
					yDisplayFlag=!yDisplayFlag;
				    xyzLastTime=Time.time;
					Debug.Log("isYBlink");
				}
			}
			if(MDIInput_script.isZSelected)
			{
				zBlink=true;
				xBlink=false;
				yBlink=false;
				if(Time.time-xyzLastTime>0.5)
				{
					zDisplayFlag=!zDisplayFlag;
				    xyzLastTime=Time.time;
					Debug.Log("isZBlink");
				}
			}
			ShowOperationScreen();	
		}
		if(Main.operationBottomScrExecute)
		{
			ShowOperationScreen();
			if(Main.runtimeIsBlink||Main.partsNumBlink)
			{
				if(Time.time-lastTime>0.5)
				{
					displayFlag=!displayFlag;
				    lastTime=Time.time;
				}
			}
		}
		
		//位置界面下方公共区域显示控制
		PosBottomScreen();
		
		//宋荣
		//显示相对坐标或综合坐标下打印区域x或y或z
		PrintAreaXYZ();
		//宋荣
	}
	
	void PrintAreaXYZ()
	{
		if(Main.operationBottomScrInitial&&xBlink&&xDisplayFlag)
		{
		     GUI.Label(new Rect(100f/1000f*Main.width,345f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X",Main.sty_BottomAST);
			// Debug.Log("打印X");
		}
		if(Main.operationBottomScrInitial&&yBlink&&yDisplayFlag)
		{
			GUI.Label(new Rect(100,346f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y",Main.sty_BottomAST);
			//Debug.Log("打印Y");
		}
		if(Main.operationBottomScrInitial&&zBlink&&zDisplayFlag)
		{
			GUI.Label(new Rect(100,346f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z",Main.sty_BottomAST);
			//Debug.Log("打印Z");
		}
	}
	//宋荣
	
	//宋荣
	
	void ShowOperationScreen()
	{
		if(Main.operationBottomScrInitial)
		{
			if(Main.statusBeforeOperation==2||Main.statusBeforeOperation==3)
			{
				GUI.Label(new Rect(171f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"归 零", Main.sty_BottomChooseMenu);
			}
	    	GUI.Label(new Rect(78f/1000f*Main.width,421f/1000f*Main.height,70f/1000f*Main.width,25f/1000f*Main.height),"W预 置", Main.sty_BottomChooseMenu);
	    	GUI.Label(new Rect(345f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"件数:0", Main.sty_BottomChooseMenu);
         	GUI.Label(new Rect(430f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"时间:0", Main.sty_BottomChooseMenu);
	    	GUI.Label(new Rect(40f/1000f*Main.width,422f/1000f*Main.height,20f/1000f*Main.width,25f/1000f*Main.height),"<", Main.sty_BottomButtonSmallest);
		}
		else if(Main.operationBottomScrExecute)
		{
			GUI.Label(new Rect(430f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"执 行", Main.sty_BottomChooseMenu);
		}
		//GUI.Label(new Rect(40f/1000f*Main.width,422f/1000f*Main.height,20f/1000f*Main.width,25f/1000f*Main.height),"", Main.sty_BottomButtonSmallest);
		Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
	    Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
        Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
	    Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
	    Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
		if(Main.statusBeforeOperation==1)
		{
			GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"绝对坐标", Main.sty_Title);
	    	GUI.Label(new Rect(60f/1000f*Main.width,65f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_BigXYZ);
	    	GUI.Label(new Rect(140f/1000f*Main.width,65f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.x), Main.sty_BigXYZ);
	    	GUI.Label(new Rect(60f/1000f*Main.width,115f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_BigXYZ);
	    	GUI.Label(new Rect(140f/1000f*Main.width,115f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.y), Main.sty_BigXYZ);
	    	GUI.Label(new Rect(60f/1000f*Main.width,165f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_BigXYZ);
	    	GUI.Label(new Rect(140f/1000f*Main.width,165f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),Main.CooStringGet(CooSystem_script.absolute_pos.z), Main.sty_BigXYZ);
		}
		if(Main.statusBeforeOperation==2)
		{
			GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"相对坐标", Main.sty_Title);
		  //  GUI.Label(new Rect(40f/1000f*Main.width,422f/1000f*Main.height,20f/1000f*Main.width,25f/1000f*Main.height),"", Main.sty_BottomButtonSmallest);
			//宋荣
			if((Main.operationBottomScrInitial&&xBlink&&xDisplayFlag)||!MDIInput_script.isXSelected)
		    	GUI.Label(new Rect(60f/1000f*Main.width,65f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_BigXYZ);
			
		    GUI.Label(new Rect(140f/1000f*Main.width,65f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),  Main.CooStringGet(CooSystem_script.relative_pos.x), Main.sty_BigXYZ);
			if((Main.operationBottomScrInitial&&yBlink&&yDisplayFlag)||!MDIInput_script.isYSelected)
		    	GUI.Label(new Rect(60f/1000f*Main.width,115f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_BigXYZ);
	    	GUI.Label(new Rect(140f/1000f*Main.width,115f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.relative_pos.y), Main.sty_BigXYZ);
			if((Main.operationBottomScrInitial&&zBlink&&zDisplayFlag)||!MDIInput_script.isZSelected)
	    		GUI.Label(new Rect(60f/1000f*Main.width,165f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_BigXYZ);
			//宋荣
	    	GUI.Label(new Rect(140f/1000f*Main.width,165f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.relative_pos.z), Main.sty_BigXYZ);
		}
		if(Main.statusBeforeOperation==3)
		{
			GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"综合显示", Main.sty_Title);
		
	    	GUI.Label(new Rect(113f/1000f*Main.width,57f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"相对坐标", Main.sty_PosSmallWord);
			//宋荣
			if((Main.operationBottomScrInitial&&xBlink&&xDisplayFlag)||!MDIInput_script.isXSelected)
     			GUI.Label(new Rect(60f/1000f*Main.width,80f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_SmallXYZ);
	
	    	GUI.Label(new Rect(100f/1000f*Main.width,80f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.relative_pos.x), Main.sty_SmallNum);
			if((Main.operationBottomScrInitial&&yBlink&&yDisplayFlag)||!MDIInput_script.isYSelected)
		    	GUI.Label(new Rect(60f/1000f*Main.width,105f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_SmallXYZ);
		    GUI.Label(new Rect(100f/1000f*Main.width,105f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.relative_pos.y), Main.sty_SmallNum);
			if((Main.operationBottomScrInitial&&zBlink&&zDisplayFlag)||!MDIInput_script.isZSelected)
		    	GUI.Label(new Rect(60f/1000f*Main.width,130f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_SmallXYZ);
			//宋荣
		    GUI.Label(new Rect(100f/1000f*Main.width,130f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.relative_pos.z), Main.sty_SmallNum);

	    	GUI.Label(new Rect(375f/1000f*Main.width,57f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"绝对坐标", Main.sty_PosSmallWord);
	    	GUI.Label(new Rect(300f/1000f*Main.width,80f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_SmallXYZ);
	    	GUI.Label(new Rect(360f/1000f*Main.width,80f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.x), Main.sty_SmallNum);
	    	GUI.Label(new Rect(300f/1000f*Main.width,105f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_SmallXYZ);
	    	GUI.Label(new Rect(360f/1000f*Main.width,105f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.y), Main.sty_SmallNum);
	    	GUI.Label(new Rect(300f/1000f*Main.width,130f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_SmallXYZ);
	    	GUI.Label(new Rect(360f/1000f*Main.width,130f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.z), Main.sty_SmallNum);
			
	    	GUI.Label(new Rect(113f/1000f*Main.width,157f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"机械坐标", Main.sty_PosSmallWord);
	     	GUI.Label(new Rect(60f/1000f*Main.width,180f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_SmallXYZ);
	    	GUI.Label(new Rect(100f/1000f*Main.width,180f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(MoveControl_script.MachineCoo.x), Main.sty_SmallNum);
	    	GUI.Label(new Rect(60f/1000f*Main.width,205f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_SmallXYZ);
	    	GUI.Label(new Rect(100f/1000f*Main.width,205f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(MoveControl_script.MachineCoo.y), Main.sty_SmallNum);
	    	GUI.Label(new Rect(60f/1000f*Main.width,230f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_SmallXYZ);
	    	GUI.Label(new Rect(100f/1000f*Main.width,230f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(MoveControl_script.MachineCoo.z), Main.sty_SmallNum);
	
	    	GUI.Label(new Rect(370f/1000f*Main.width,157f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"剩余移动量", Main.sty_PosSmallWord);
	    	GUI.Label(new Rect(300f/1000f*Main.width,180f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_SmallXYZ);
	     	GUI.Label(new Rect(360f/1000f*Main.width,180f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"     0.000", Main.sty_SmallNum);
	     	GUI.Label(new Rect(300f/1000f*Main.width,205f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_SmallXYZ);
		    GUI.Label(new Rect(360f/1000f*Main.width,205f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"     0.000", Main.sty_SmallNum);
	    	GUI.Label(new Rect(300f/1000f*Main.width,230f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_SmallXYZ);
		    GUI.Label(new Rect(360f/1000f*Main.width,230f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"     0.000", Main.sty_SmallNum);
		}
	}
	//宋荣
	
	//绝对坐标界面显示控制
	void PosAbsoluteCoo() 
	{
		GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"绝对坐标", Main.sty_Title);
		GUI.Label(new Rect(40f/1000f*Main.width,422f/1000f*Main.height,20f/1000f*Main.width,25f/1000f*Main.height),"", Main.sty_BottomButtonSmallest);
		Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_d;
		Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
		GUI.Label(new Rect(78f/1000f*Main.width,421f/1000f*Main.height,70f/1000f*Main.width,25f/1000f*Main.height),"绝 对", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(171f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"相 对", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(261f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"综 合", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(352f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"手 轮", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(423f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"（操 作）", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		GUI.Label(new Rect(60f/1000f*Main.width,65f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_BigXYZ);
		GUI.Label(new Rect(140f/1000f*Main.width,65f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.x), Main.sty_BigXYZ);
		GUI.Label(new Rect(60f/1000f*Main.width,115f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_BigXYZ);
		GUI.Label(new Rect(140f/1000f*Main.width,115f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.y), Main.sty_BigXYZ);
		GUI.Label(new Rect(60f/1000f*Main.width,165f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_BigXYZ);
		GUI.Label(new Rect(140f/1000f*Main.width,165f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),Main.CooStringGet(CooSystem_script.absolute_pos.z), Main.sty_BigXYZ);
	}
	
	//相对坐标界面显示控制
	void PosRelativeCoo () 
	{
		GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"相对坐标", Main.sty_Title);
		GUI.Label(new Rect(40f/1000f*Main.width,422f/1000f*Main.height,20f/1000f*Main.width,25f/1000f*Main.height),"", Main.sty_BottomButtonSmallest);
		Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_d;
		Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
		GUI.Label(new Rect(78f/1000f*Main.width,420f/1000f*Main.height,70f/1000f*Main.width,25f/1000f*Main.height),"绝 对", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(171f/1000f*Main.width,421f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"相 对", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(261f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"综 合", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(352f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"手 轮", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(423f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"（操 作）", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		GUI.Label(new Rect(60f/1000f*Main.width,65f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_BigXYZ);
		GUI.Label(new Rect(140f/1000f*Main.width,65f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),  Main.CooStringGet(CooSystem_script.relative_pos.x), Main.sty_BigXYZ);
		GUI.Label(new Rect(60f/1000f*Main.width,115f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_BigXYZ);
		GUI.Label(new Rect(140f/1000f*Main.width,115f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.relative_pos.y), Main.sty_BigXYZ);
		GUI.Label(new Rect(60f/1000f*Main.width,165f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_BigXYZ);
		GUI.Label(new Rect(140f/1000f*Main.width,165f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.relative_pos.z), Main.sty_BigXYZ);
	}
	
	//综合界面显示控制
	void PosGeneralCoo() 
	{
		GUI.Label(new Rect(40f/1000f*Main.width,28f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"综合显示", Main.sty_Title);
		Main.sty_BottomButton_1.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_2.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_3.normal.background = Main.t2d_BottomButton_d;
		Main.sty_BottomButton_4.normal.background = Main.t2d_BottomButton_u;
		Main.sty_BottomButton_5.normal.background = Main.t2d_BottomButton_u;
		
		GUI.Label(new Rect(78f/1000f*Main.width,420f/1000f*Main.height,70f/1000f*Main.width,25f/1000f*Main.height),"绝 对", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(171f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"相 对", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(261f/1000f*Main.width,421f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"综 合", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(352f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"手 轮", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(423f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"（操 作）", Main.sty_BottomChooseMenu);
		GUI.Label(new Rect(523f/1000f*Main.width,420f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"+", Main.sty_MostWords);
		
		GUI.Label(new Rect(113f/1000f*Main.width,57f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"相对坐标", Main.sty_PosSmallWord);
		GUI.Label(new Rect(60f/1000f*Main.width,80f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_SmallXYZ);
		GUI.Label(new Rect(100f/1000f*Main.width,80f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.relative_pos.x), Main.sty_SmallNum);
		GUI.Label(new Rect(60f/1000f*Main.width,105f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_SmallXYZ);
		GUI.Label(new Rect(100f/1000f*Main.width,105f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.relative_pos.y), Main.sty_SmallNum);
		GUI.Label(new Rect(60f/1000f*Main.width,130f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_SmallXYZ);
		GUI.Label(new Rect(100f/1000f*Main.width,130f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.relative_pos.z), Main.sty_SmallNum);

		GUI.Label(new Rect(375f/1000f*Main.width,57f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"绝对坐标", Main.sty_PosSmallWord);
		GUI.Label(new Rect(300f/1000f*Main.width,80f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_SmallXYZ);
		GUI.Label(new Rect(360f/1000f*Main.width,80f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.x), Main.sty_SmallNum);
		GUI.Label(new Rect(300f/1000f*Main.width,105f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_SmallXYZ);
		GUI.Label(new Rect(360f/1000f*Main.width,105f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.y), Main.sty_SmallNum);
		GUI.Label(new Rect(300f/1000f*Main.width,130f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_SmallXYZ);
		GUI.Label(new Rect(360f/1000f*Main.width,130f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(CooSystem_script.absolute_pos.z), Main.sty_SmallNum);
			
		GUI.Label(new Rect(113f/1000f*Main.width,157f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"机械坐标", Main.sty_PosSmallWord);
		GUI.Label(new Rect(60f/1000f*Main.width,180f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_SmallXYZ);
		GUI.Label(new Rect(100f/1000f*Main.width,180f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(MoveControl_script.MachineCoo.x), Main.sty_SmallNum);
		GUI.Label(new Rect(60f/1000f*Main.width,205f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_SmallXYZ);
		GUI.Label(new Rect(100f/1000f*Main.width,205f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(MoveControl_script.MachineCoo.y), Main.sty_SmallNum);
		GUI.Label(new Rect(60f/1000f*Main.width,230f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_SmallXYZ);
		GUI.Label(new Rect(100f/1000f*Main.width,230f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.CooStringGet(MoveControl_script.MachineCoo.z), Main.sty_SmallNum);
				
				
		GUI.Label(new Rect(370f/1000f*Main.width,157f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"剩余移动量", Main.sty_PosSmallWord);
		GUI.Label(new Rect(300f/1000f*Main.width,180f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"X", Main.sty_SmallXYZ);
		GUI.Label(new Rect(360f/1000f*Main.width,180f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"     0.000", Main.sty_SmallNum);
		GUI.Label(new Rect(300f/1000f*Main.width,205f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Y", Main.sty_SmallXYZ);
		GUI.Label(new Rect(360f/1000f*Main.width,205f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"     0.000", Main.sty_SmallNum);
		GUI.Label(new Rect(300f/1000f*Main.width,230f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"Z", Main.sty_SmallXYZ);
		GUI.Label(new Rect(360f/1000f*Main.width,230f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"     0.000", Main.sty_SmallNum);
	}
	
	//位置界面下方公共区域显示控制
	void PosBottomScreen()
	{
		//宋荣
		//if(Main.partsNumBlink)
			//Debug.Log("parttimeblink is true");
	    if((Main.partsNumBlink&&displayFlag)||!Main.partsNumBlink)
			GUI.Label(new Rect(290f/1000f*Main.width,280f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"加工零件数", Main.sty_MostWords);
		//宋荣
		GUI.Label(new Rect(452f/1000f*Main.width,278f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.NumberFormat(Main.PartsNum), Main.sty_SmallNum);
		//宋荣
		//if(Main.runtimeIsBlink)
		//	Debug.Log("runtimeIsBlink is true");
		if((Main.runtimeIsBlink&&displayFlag)||!Main.runtimeIsBlink)
		   GUI.Label(new Rect(40f/1000f*Main.width,301f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"运行时间", Main.sty_MostWords);
	    //宋荣
		GUI.Label(new Rect(187f/1000f*Main.width,301f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"H", Main.sty_MostWords);
		GUI.Label(new Rect(250f/1000f*Main.width,301f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"M", Main.sty_MostWords);
		
		GUI.Label(new Rect(105f/1000f*Main.width,299f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.NumberFormat(Main.RunningTimeH), Main.sty_SmallNum);
		GUI.Label(new Rect(166f/1000f*Main.width,299f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.NumberFormat(Main.RunningTimeM), Main.sty_SmallNum);
		
		GUI.Label(new Rect(290f/1000f*Main.width,301f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"循环时间", Main.sty_MostWords);
		GUI.Label(new Rect(437f/1000f*Main.width,301f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"H", Main.sty_MostWords);
		GUI.Label(new Rect(480f/1000f*Main.width,301f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"M", Main.sty_MostWords);
		GUI.Label(new Rect(522f/1000f*Main.width,301f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"S", Main.sty_MostWords);
		
		GUI.Label(new Rect(357f/1000f*Main.width,299f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.NumberFormat(Main.CycleTimeH), Main.sty_SmallNum);
		GUI.Label(new Rect(398f/1000f*Main.width,299f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.NumberFormat(Main.CycleTimeM), Main.sty_SmallNum);
		GUI.Label(new Rect(440f/1000f*Main.width,299f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.NumberFormat(Main.CycleTimeS), Main.sty_SmallNum);
		
		GUI.Label(new Rect(40f/1000f*Main.width,322f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"实速度         MM/MIN", Main.sty_MostWords);
		GUI.Label(new Rect(133f/1000f*Main.width,321f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.NumberFormat(Main.RunningSpeed), Main.sty_SmallNum);
		GUI.Label(new Rect(310f/1000f*Main.width,322f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height),"SACT         /分", Main.sty_MostWords);
		GUI.Label(new Rect(380f/1000f*Main.width,321f/1000f*Main.height,500f/1000f*Main.width,300f/1000f*Main.height), Main.NumberFormat(Main.SACT), Main.sty_SmallNum);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
