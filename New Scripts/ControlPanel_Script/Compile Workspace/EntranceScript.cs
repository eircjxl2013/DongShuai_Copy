using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntranceScript : MonoBehaviour {
	NCCodeFormat NCCodeFormat_Script;
	List<List<string>> SourceCode = new List<List<string>>();
	List<DataStore> compile_info_list = new List<DataStore>();
	List<MotionInfo> motion_info_list = new List<MotionInfo>();
	ModalCode_Fanuc_M current_modal = new ModalCode_Fanuc_M();
	Vector3 current_position = new Vector3(1, 1, 1);
	Vector3 virtual_position = new Vector3(0, 0, 0);
	//GameObject tryCreate;
	//ModalCode_Fanuc_M 
	string text_field = "O2";
	// Use this for initialization
	void Start () {
		/*
		tryCreate = (GameObject)Resources.Load("EmptyObject");
		Instantiate(tryCreate);
		tryCreate.transform.name = "tryCreate";
		*/
		gameObject.AddComponent("NCCodeFormat");
		NCCodeFormat_Script = gameObject.GetComponent<NCCodeFormat>();
	}

	void Load(string filename)
	{
		if(SourceCode != null)
		{
			SourceCode.Clear();
			SourceCode = null;
		}
		SourceCode = NCCodeFormat_Script.AllCode(filename);
		Debug.Log(SourceCode.Count);
	}
	
	void OnGUI ()
	{
		//To display
		//test_str = GUI.TextArea(new Rect(10, 10,300, 500), test_str);
		
		GUI.Label(new Rect(140, 80, 100, 20), "请输入程序号：");
		text_field = GUI.TextField(new Rect(245, 80, 195, 20), text_field);
		//运行时，可重新输入不同的NC程序名字，反复启动
		if(GUI.Button(new Rect(140, 110, 300, 30), "启动"))
		{
			Load(text_field);
		}
		
		if(GUI.Button(new Rect(140, 160, 300, 30), "检查"))
		{
			FANUC_OI_M check = new FANUC_OI_M();
			check.ModalClone(current_modal);
			Debug.Log(check.CompileEntrance(SourceCode, text_field, ref compile_info_list, current_position, ref motion_info_list, virtual_position));
			Debug.Log(check.ExecuteFlag);
			Debug.Log(check.CompileInfo.Count);
			Debug.Log(compile_info_list.Count);
			if(check.CompileInfo.Count > 0)
			foreach(string error_str in check.CompileInfo)
				Debug.Log(error_str);
			if(compile_info_list.Count > 0)
				foreach(DataStore compile_info in compile_info_list)
					Debug.Log(compile_info.ToString());
		}
		
		if(GUI.Button(new Rect(140, 200, 300, 30), "Clone"))
		{
			CloneTest();
		}
	}
	
	void CloneTest()
	{
		ModalCode_Fanuc_M original_class = new ModalCode_Fanuc_M();
		Debug.Log( "original1: "+original_class.Modal_Code[10]);
		original_class.Modal_Code[10] = "G1111";
		Debug.Log("original2: "+original_class.Modal_Code[10]);
		ModalCode_Fanuc_M copy1 = new ModalCode_Fanuc_M(original_class);
		Debug.Log("copy1: "+copy1.Modal_Code[10]);
		copy1.Modal_Code[10] = "copy";
		Debug.Log("compare: " + original_class.Modal_Code[10] + copy1.Modal_Code[10]);
	}
	
	void SimulateFunc(List<List<string>> source_code, string prog_name, ref List<DataStore> compile_data, Vector3 current_position, ref List<MotionInfo> motion_data)
	{
		FANUC_OI_M SimulatClass = new FANUC_OI_M();
		SimulatClass.ModalClone(current_modal);
		SimulatClass.CompileEntrance(source_code, prog_name, ref compile_data, current_position, ref motion_data, virtual_position);
		
	}
	
	void StartSimulate()
	{
		for(int i = 0; i < motion_info_list.Count; i++)
		{
			//光标换行
			//Debug.Log("Change to row: " + i);
			//跳过功能
			/***
			 * if（跳过按钮按下）
			 * {
			 * 		if（当前行有/）
			 * 			continue；
			 * }
			 * **/
			//执行需立即实现的功能
			if(motion_info_list[i].Immediate_Motion != "")
			{
				for(int j = 0; j < motion_info_list[i].Immediate_Motion.Length; j++)
				{
					Debug.Log("Immediate Function: " + motion_info_list[i].Immediate_Motion[j]);
					//+++在这里对应的参数要变化
				}
			}
			//常规运动
			if(motion_info_list[i].Motion_Type != -1)
			{
				Debug.Log("判断模态和参数是否要变化");
				switch(motion_info_list[i].Motion_Type)
				{
				case (int)MotionType.DryRunning:
					Debug.Log("G00");
					Debug.Log("剩余移动量哦亲，要跟时间连起来哦亲，还要注意当前的倍率哦亲");
					break;
				case (int)MotionType.Line:
					Debug.Log("G01");
					Debug.Log("剩余移动量哦亲，要跟时间连起来哦亲，还要注意当前的倍率哦亲");
					break;
				case (int)MotionType.Circular02:
					Debug.Log("G02");
					Debug.Log("剩余移动量哦亲，要跟时间连起来哦亲，还要注意当前的倍率哦亲");
					break;
				case (int)MotionType.Circular03:
					Debug.Log("G03");
					Debug.Log("剩余移动量哦亲，要跟时间连起来哦亲，还要注意当前的倍率哦亲");
					break;
					
				default:
					break;
				}
			}
			//switch(compile_info_list[i].immediate_execution)
		}
	}
}
