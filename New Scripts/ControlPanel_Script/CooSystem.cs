using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class CooSystem : MonoBehaviour {
	
	ControlPanel Main;
	MoveControl MoveControl_script;
	public Vector3 absolute_pos = new Vector3(0,0,0);
	public Vector3 relative_pos = new Vector3(0,0,0);
	public int workpiece_flag = 1;
	
	public Vector3 G00_pos = new Vector3(0,0,0);
	public Vector3 G54_pos = new Vector3(0,0,0);
	public Vector3 G55_pos = new Vector3(0,0,0);
	public Vector3 G56_pos = new Vector3(0,0,0);
	public Vector3 G57_pos = new Vector3(0,0,0);
	public Vector3 G58_pos = new Vector3(0,0,0);
	public Vector3 G59_pos = new Vector3(0,0,0);
	public Vector3 workpiece_coo = new Vector3(0,0,0);
	
	//设定界面修改---陈振华---03.11
	public string parameter_writabel = "1";
	public string TV_check = "0";
	public string hole_code = "1";
	public string input_unit = "0";
	public string IO = "0";
	public string sequence_number = "0";
	public string paper_tape = "0";
	public string SN_stop1 = "0";
	public string SN_stop2 = "0";
	//设定界面修改---陈振华---03.11
	
	//刀偏界面加入---陈振华---03.30
	public float [] shape_H = new float[400];  //形状H
	public float [] wear_H  = new float[400];  //磨损H
	public float [] shape_D = new float[400];  //形状D
	public float [] wear_D  = new float[400];  //磨损D	
	public float [] write_tool_str = new float[400];
	//刀偏界面加入---陈振华---03.30
	
	void Awake () {
		
	}
	
	// Use this for initialization
	void Start () {
		Main = gameObject.GetComponent<ControlPanel>();
		MoveControl_script = GameObject.Find("move_control").GetComponent<MoveControl>();
		ReadCooFile();
		workpiece_coo = G54_pos;
		workpiece_flag = 1;
		
		//获得设置界面显示值
		if(PlayerPrefs.HasKey("parameter_writabel"))
			parameter_writabel = PlayerPrefs.GetString("parameter_writabel");
		else
		{
			PlayerPrefs.SetString("parameter_writabel", "1");
			parameter_writabel = "1";
		}
		
		if(PlayerPrefs.HasKey("TV_check"))
			TV_check = PlayerPrefs.GetString("TV_check");
		else
		{
			PlayerPrefs.SetString("TV_check", "0");
			TV_check = "0";
		}
		
		if(PlayerPrefs.HasKey("hole_code"))
			hole_code = PlayerPrefs.GetString("hole_code");
		else
		{
			PlayerPrefs.SetString("hole_code", "1");
			hole_code = "1";
		}
		
		if(PlayerPrefs.HasKey("input_unit"))
			input_unit = PlayerPrefs.GetString("input_unit");
		else
		{
			PlayerPrefs.SetString("input_unit", "0");
			input_unit = "0";
		}
		
		if(PlayerPrefs.HasKey("IO"))
			IO = PlayerPrefs.GetString("IO");
		else
		{
			PlayerPrefs.SetString("IO", "0");
			IO = "0";
		}
			
		if(PlayerPrefs.HasKey("sequence_number"))
			sequence_number = PlayerPrefs.GetString("sequence_number");
		else
		{
			PlayerPrefs.SetString("sequence_number", "0");
			sequence_number = "0";
		}
		
		if(PlayerPrefs.HasKey("paper_tape"))
			paper_tape = PlayerPrefs.GetString("paper_tape");
		else
		{
			PlayerPrefs.SetString("paper_tape", "0");
			paper_tape = "0";
		}
		
		if(PlayerPrefs.HasKey("SN_stop1"))
			SN_stop1 = PlayerPrefs.GetString("SN_stop1");
		else
		{
			PlayerPrefs.SetString("SN_stop1", "0");
			SN_stop1 = "0";
		}
		
		if(PlayerPrefs.HasKey("SN_stop2"))
			SN_stop2 = PlayerPrefs.GetString("SN_stop2");
		else
		{
			PlayerPrefs.SetString("SN_stop2", "0");
			SN_stop2 = "0";
		}
		//获得设置界面显示值
		
		ReadToolFile ();
	}
	
	//刀偏界面下移
	public void tool_down()
	{
		if(Main.tool_setting>=1&&Main.tool_setting<=28)
	   		Main.tool_setting  += 4;
		else if(Main.tool_setting == 29)
		{
			if(Main.ToolOffSetPage_num < 49)
			{
				Tool_pagedown();
				Main.tool_setting = 1;
			}
		}
		else if(Main.tool_setting == 30)
		{
			if(Main.ToolOffSetPage_num < 49)
			{
				Tool_pagedown();
				Main.tool_setting = 2;
			}
		}
		else if(Main.tool_setting == 31)
		{
			if(Main.ToolOffSetPage_num < 49)
			{
				Tool_pagedown();
				Main.tool_setting = 3;
			}
		}
		else if(Main.tool_setting == 32)
		{
			if(Main.ToolOffSetPage_num < 49)
			{
				Tool_pagedown();
				Main.tool_setting = 4;
			}
		}
		//Debug.Log(Main.tool_setting);
		ToolCursorPos();
	}
	
	//刀偏界面上移
	public void tool_up()
	{
		if(Main.tool_setting>=5 && Main.tool_setting<=32)
	  		Main.tool_setting  -= 4;
		else if(Main.tool_setting == 1)
		{
			if(Main.ToolOffSetPage_num > 0)
			{
				Tool_pageup();
				Main.tool_setting = 29;
			}
		}
	    else if(Main.tool_setting == 2)
		{
			if(Main.ToolOffSetPage_num > 0)
			{
				Tool_pageup();
				Main.tool_setting = 30;
			}
		}
		else if(Main.tool_setting == 3)
		{
			
			if(Main.ToolOffSetPage_num > 0)
			{
				Tool_pageup();
				Main.tool_setting = 31;
			}
		}
		else if(Main.tool_setting == 4)
		{
			if(Main.ToolOffSetPage_num > 0)
			{
				Tool_pageup();
				Main.tool_setting = 32;
			}
		}
		ToolCursorPos();
	}
	
	//刀偏界面右移
	public void tool_right()
	{
		if(Main.tool_setting == 32)
		{
			if(Main.ToolOffSetPage_num < 49)
			{
				Tool_pagedown();
				Main.tool_setting = 1;
			}
		}
		else
			Main.tool_setting++;
		ToolCursorPos();
	}
	//刀偏界面左移
	public void tool_left()
	{
		if(Main.tool_setting == 1)
		{
			if(Main.ToolOffSetPage_num > 0)
			{
				Tool_pageup();
				Main.tool_setting = 32;
			}
		}
		else
			Main.tool_setting--;
		ToolCursorPos();
	}
	
	//刀偏页面下翻
	public void Tool_pagedown()
	{
		Main.ToolOffSetPage_num++;	
	}
	
	//刀偏页面上翻
	public void Tool_pageup()
	{
		Main.ToolOffSetPage_num--;
	}
	
	//黄色背景图片
	public void ToolCursorPos()
	{
		if(Main.tool_setting>= 1 && Main.tool_setting <= 32)
		{
			if(Main.tool_setting % 4 == 1)
			{
				Main.tool_setting_cursor_w = 91.5f;
			}
			else if (Main.tool_setting % 4 == 2)
			{
				Main.tool_setting_cursor_w = 201.5f;
			}
			else if (Main.tool_setting % 4 == 3)
			{
				Main.tool_setting_cursor_w = 311.5f;
			}
			else if (Main.tool_setting % 4 == 0)
			{
				Main.tool_setting_cursor_w = 421.5f;
			}
			
			if((int)(Main.tool_setting / 4.1f) == 0)
				Main.tool_setting_cursor_y = 81.5f;	
			else if((int)(Main.tool_setting / 4.1f) == 1)
				Main.tool_setting_cursor_y = 106.5f;
			else if((int)(Main.tool_setting / 4.1f) == 2)
				Main.tool_setting_cursor_y = 132.5f;
			else if((int)(Main.tool_setting / 4.1f) == 3)
				Main.tool_setting_cursor_y = 156.5f;
			else if((int)(Main.tool_setting / 4.1f) == 4)
				Main.tool_setting_cursor_y =181.5f;
			else if((int)(Main.tool_setting / 4.1f )== 5)
				Main.tool_setting_cursor_y = 206.5f;
			else if((int)(Main.tool_setting / 4.1f) == 6)
				Main.tool_setting_cursor_y = 232.5f;
			else if((int)(Main.tool_setting / 4.1f) == 7)
				Main.tool_setting_cursor_y = 256.5f;
		}
		//Debug.Log((int)Main.tool_setting / 4.1f);
	}
	
	public void SearchToolNo(string num_str)
	{
		Regex isNum_Test = new Regex(@"\D+");
		if(isNum_Test.IsMatch(num_str))
		{
			Debug.Log("格式错误！请输入1-400");
			return;
		}
		int num = int.Parse(num_str);
		if(num > 400 || num <= 0)
		{
			Debug.Log("格式错误！请输入1-400");
			return;
		}
		//Todo: to be modified.
		if((num-1) % 8 == 0)
		{
			Main.ToolOffSetPage_num = ((num-1) / 8);
			Main.tool_setting = 1;	
			ToolCursorPos();
		}
		else if((num-2) % 8 == 0)
		{
			Main.ToolOffSetPage_num = ((num-2) / 8);
			Main.tool_setting = 5;	
			ToolCursorPos();
		}
		else if((num-3) % 8 == 0)
		{
			Main.ToolOffSetPage_num = ((num-3) / 8);
			Main.tool_setting = 9;	
			ToolCursorPos();
		}
		else if((num-4) % 8 == 0)
		{
			Main.ToolOffSetPage_num = ((num-4) / 8);
			Main.tool_setting = 13;	
			ToolCursorPos();
		}
		else if((num-5) % 8 == 0)
		{
			Main.ToolOffSetPage_num = ((num-5) / 8);
			Main.tool_setting = 17;	
			ToolCursorPos();
		}
		else if((num-6) % 8 == 0)
		{
			Main.ToolOffSetPage_num = ((num-6) / 8);
			Main.tool_setting = 21;	
			ToolCursorPos();
		}
		else if((num-7) % 8 == 0)
		{
			Main.ToolOffSetPage_num = ((num-7) / 8);
			Main.tool_setting = 25;	
			ToolCursorPos();
		}
		else if((num-8) % 8 == 0)
		{
			Main.ToolOffSetPage_num = ((num-8) / 8);
			Main.tool_setting = 29;	
			ToolCursorPos();
		}
	}
	
	//刀偏界面初始数据读取
	public void ReadToolFile () 
	{
		string line_str = "";
		StreamReader line_str_reader;
		FileStream tool_stream = new FileStream(Application.dataPath + SystemArguments.ToolParameterPath + "shape_H.txt", FileMode.OpenOrCreate, FileAccess.Read);
		line_str_reader = new StreamReader(tool_stream);
		line_str = line_str_reader.ReadLine();
		//Debug.Log(line_str);
		if(line_str == null)
		{
			for(int i = 0; i<400; i++)
				shape_H[i] = 0f;	
		}
		else
		{
			for(int i = 0; i<400; i++)
			{
				if(line_str == null)
					shape_H[i] = 0;
				else
					shape_H[i] = float.Parse(line_str);
				line_str = line_str_reader.ReadLine();
			}
		}
		line_str_reader.Close();
		tool_stream = new FileStream(Application.dataPath + SystemArguments.ToolParameterPath + "wear_H.txt", FileMode.OpenOrCreate, FileAccess.Read);
		line_str_reader = new StreamReader(tool_stream);
		line_str = line_str_reader.ReadLine();
		if(line_str == null)
		{
			for(int i = 0; i<400; i++)
				wear_H[i] = 0f;	
		}
		else
		{
			for(int i = 0; i<400; i++)
			{
				if(line_str == null)
					wear_H[i] = 0;
				else
					wear_H[i] = float.Parse(line_str);
				line_str = line_str_reader.ReadLine();
			}
		}
		line_str_reader.Close();
		tool_stream = new FileStream(Application.dataPath + SystemArguments.ToolParameterPath + "shape_D.txt", FileMode.OpenOrCreate, FileAccess.Read);
		line_str_reader = new StreamReader(tool_stream);
		line_str = line_str_reader.ReadLine();
		if(line_str == null)
		{
			for(int i = 0; i<400; i++)
				shape_D[i] = 0f;	
		}
		else
		{
			for(int i = 0; i<400; i++)
			{
				if(line_str == null)
					shape_D[i] = 0;
				else
					shape_D[i] =  float.Parse(line_str);
				line_str = line_str_reader.ReadLine();
			}
		}
		line_str_reader.Close();
		tool_stream = new FileStream(Application.dataPath + SystemArguments.ToolParameterPath + "wear_D.txt", FileMode.OpenOrCreate, FileAccess.Read);
		line_str_reader = new StreamReader(tool_stream);
		line_str = line_str_reader.ReadLine();
		if(line_str == null)
		{
			for(int i = 0; i<400; i++)
			wear_D[i] = 0f;
			
		}
		else
		{
			for(int i = 0; i<400; i++)
			{
				if(line_str == null)
					wear_D[i] = 0;
				else
					wear_D[i] =  float.Parse(line_str);
				line_str = line_str_reader.ReadLine();
			}
		}
		line_str_reader.Close();
	}
	
	//刀偏写入功能
	void WriteToolFile (string filename)
	{
		StreamWriter line_str_writer;
		FileStream tool_stream;
		FileInfo check_exist = new FileInfo(Application.dataPath + SystemArguments.ToolParameterPath + filename+".txt");
		if(check_exist.Exists)
			tool_stream = new FileStream(Application.dataPath + SystemArguments.ToolParameterPath + filename+".txt", FileMode.Truncate, FileAccess.Write);	
		else
			tool_stream = new FileStream(Application.dataPath + SystemArguments.ToolParameterPath + filename+".txt", FileMode.Create, FileAccess.Write);
		line_str_writer = new StreamWriter(tool_stream);
		switch(filename)
		{
		case "shape_H":
			for(int i=0; i<400; i++)
			{
				line_str_writer.WriteLine(shape_H[i]);
			}
			break;
		case "wear_H":
			for(int i=0; i<400; i++)
			{
				line_str_writer.WriteLine(wear_H[i]);
			}
			break;
		case "shape_D":
			for(int i=0; i<400; i++)
			{
				line_str_writer.WriteLine(shape_D[i]);
			}
			break;
		case "wear_D":
			for(int i=0; i<400; i++)
			{
				line_str_writer.WriteLine(wear_D[i]);
			}
			break;
		}
		line_str_writer.Close();
	}
	
	//刀偏c输入功能
	public void C_Input (string tool_value)
	{
		char[] tool_choose = tool_value.ToCharArray();
		float value_f = 0;
		if( tool_choose[0] == 'Z' || tool_choose[0] == 'z' )
		{
			if(Main.tool_setting == 1 || Main.tool_setting == 2|| Main.tool_setting == 5|| Main.tool_setting == 6|| Main.tool_setting == 9|| Main.tool_setting == 10|| Main.tool_setting == 13|| Main.tool_setting == 14|| Main.tool_setting == 17|| Main.tool_setting == 18|| Main.tool_setting == 21|| Main.tool_setting == 22|| Main.tool_setting == 25|| Main.tool_setting == 26|| Main.tool_setting == 29|| Main.tool_setting == 30)
			{
				value_f = relative_pos.z;
				Write_choose( value_f, 1);
			}
			else if(Main.tool_setting == 3 || Main.tool_setting == 4|| Main.tool_setting == 7|| Main.tool_setting == 8|| Main.tool_setting == 11|| Main.tool_setting == 12|| Main.tool_setting == 15|| Main.tool_setting == 16|| Main.tool_setting == 19|| Main.tool_setting == 20|| Main.tool_setting == 23|| Main.tool_setting == 24|| Main.tool_setting == 27|| Main.tool_setting == 28|| Main.tool_setting == 31|| Main.tool_setting == 32)
		    return;
		}
		else
		{
			Debug.Log("Format Error!!!");
			return;
		}	
	}
	
	//刀偏+输入功能
	public void Plus_Tool_Input (string input_value, bool plus_flag) 
	{
		char[] tool_choose = input_value.ToCharArray();
		string value_str = "";
		float value_f = 0;
		for(int i = 0; i < tool_choose.Length; i++)
		{
			if(tool_choose[i] != '.'  && tool_choose[i] != '+' && tool_choose[i] != '-')
			{
				if(tool_choose[i] < '0' || tool_choose[i] > '9')
				{
					Debug.Log("Format Error!!!");
					return;
				}
			}
			else if(tool_choose[i] == '+' || tool_choose[i] == '-')
			{
				if(i != 0)
				{
					Debug.Log("Format Error!!!");
					return;
				}
			}
			value_str += tool_choose[i];
		}
		if(value_str == "+" || value_str == "-")
		{
			Debug.Log("Format Error!!!");
			return;
		}
		value_f = float.Parse(value_str);
		if(plus_flag)
			Write_choose(value_f, 2);
		else
			Write_choose(value_f, 3);
	}
	
	//刀偏输入框选择
	void Write_choose (float value_f, int mode_flag) 
	{
		switch(Main.tool_setting)
		{
		case 1:
			if(mode_flag == 1)
				shape_H[Main.number ] = value_f; 
			else if(mode_flag == 2)
				shape_H[Main.number ] += value_f;
			else if(mode_flag == 3)
				shape_H[Main.number ] = value_f; 
			WriteToolFile("shape_H");
			break;
		case 2:
			if(mode_flag == 1)
				wear_H[Main.number ] = value_f; 
			else if(mode_flag == 2)
				wear_H[Main.number ] += value_f;
			else if(mode_flag == 3)
				wear_H[Main.number ] = value_f; 	
			WriteToolFile("wear_H");
			break;
		case 3:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				shape_D[Main.number ] += value_f;
			else if(mode_flag == 3)
				shape_D[Main.number ] = value_f;   
			WriteToolFile("shape_D");
			break;
		case 4:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				wear_D[Main.number ] += value_f;
			else if(mode_flag == 3)
				wear_D[Main.number ] = value_f; 
			WriteToolFile("wear_D");
			break;
		case 5:
			if(mode_flag == 1)
				shape_H[Main.number +1] = value_f; 
			else if(mode_flag == 2)
				shape_H[Main.number +1] += value_f;
			else if(mode_flag == 3)
				shape_H[Main.number +1] = value_f; 
			WriteToolFile("shape_H");
			break;
		case 6:
			if(mode_flag == 1)
				wear_H[Main.number +1] = value_f; 
			else if(mode_flag == 2)
				wear_H[Main.number +1] += value_f;
			else if(mode_flag == 3)
				wear_H[Main.number +1] = value_f; 
			WriteToolFile("wear_H");
			break;
		case 7:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				shape_D[Main.number +1] += value_f;
			else if(mode_flag == 3)
				shape_D[Main.number +1] = value_f; 
			WriteToolFile("shape_D");
			break;
		case 8:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				wear_D[Main.number +1] += value_f;
			else if(mode_flag == 3)
				wear_D[Main.number +1] = value_f; 
			WriteToolFile("wear_D");
			break;
		case 9:
			if(mode_flag == 1)
				shape_H[Main.number +2] = value_f; 
			else if(mode_flag == 2)
				shape_H[Main.number +2] += value_f;
			else if(mode_flag == 3)
				shape_H[Main.number +2] = value_f; 
			WriteToolFile("shape_H");
			break;
		case 10:
			if(mode_flag == 1)
				wear_H[Main.number +2] = value_f;
			else if(mode_flag == 2)
				wear_H[Main.number +2] += value_f;
			else if(mode_flag == 3)
				wear_H[Main.number +2] = value_f; 
			WriteToolFile("wear_H");
			break;
		case 11:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				shape_D[Main.number +2] += value_f;
			else if(mode_flag == 3)
				shape_D[Main.number +2] = value_f; 
				WriteToolFile("shape_D");
			break;
		case 12:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				wear_D[Main.number +2] += value_f;
			else if(mode_flag == 3)
				wear_D[Main.number +2] = value_f; 
			WriteToolFile("wear_D");
			break;
		case 13:
			if(mode_flag == 1)
				shape_H[Main.number +3] = value_f; 
			else if(mode_flag == 2)
				shape_H[Main.number +3] += value_f;
			else if(mode_flag == 3)
				shape_H[Main.number +3] = value_f; 
			WriteToolFile("shape_H");
			break;
		case 14:
			if(mode_flag == 1)
				wear_H[Main.number +3] = value_f;
			else if(mode_flag == 2)
				wear_H[Main.number +3] += value_f;
			else if(mode_flag == 3)
				wear_H[Main.number +3] = value_f; 
			WriteToolFile("wear_H");
			break;
		case 15:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				shape_D[Main.number +3] += value_f;
			else if(mode_flag == 3)
				shape_D[Main.number +3] = value_f; 
			WriteToolFile("shape_D");
			break;
		case 16:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				wear_D[Main.number +3] += value_f;
			else if(mode_flag == 3)
				wear_D[Main.number +3] = value_f; 
			WriteToolFile("wear_D");
			break;
		case 17:
			if(mode_flag == 1)
				shape_H[Main.number +4] = value_f; 
			else if(mode_flag == 2)
				shape_H[Main.number +4] += value_f;
			else if(mode_flag == 3)
				shape_H[Main.number +4] = value_f; 
			WriteToolFile("shape_H");
			break;
		case 18:
			if(mode_flag == 1)
				wear_H[Main.number +4] = value_f; 
			else if(mode_flag == 2)
				wear_H[Main.number +4] += value_f;
			else if(mode_flag == 3)
			 	wear_H[Main.number +4] = value_f; 
			WriteToolFile("wear_H");
			break;
		case 19:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				shape_D[Main.number +4] += value_f;
			else if(mode_flag == 3)
				shape_D[Main.number +4] = value_f; 
			WriteToolFile("shape_D");
			break;
		case 20:
			if(mode_flag == 1)	
				   return;
			else if(mode_flag == 2)
				wear_D[Main.number +4] += value_f;
			else if(mode_flag == 3)
				wear_D[Main.number +4] = value_f; 
			WriteToolFile("wear_D");
			break;
		case 21:
			if(mode_flag == 1)
				shape_H[Main.number +5] = value_f; 
			else if(mode_flag == 2)
				shape_H[Main.number +5] += value_f;
			else if(mode_flag == 3)
				shape_H[Main.number +5] = value_f; 
			WriteToolFile("shape_H");
			break;
		case 22:
			if(mode_flag == 1)
				wear_H[Main.number +5] = value_f; 
			else if(mode_flag == 2)
				wear_H[Main.number +5] += value_f;
			else if(mode_flag == 3)
				wear_H[Main.number +5] = value_f; 
			WriteToolFile("wear_H");
			break;
		case 23:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				shape_D[Main.number +5] += value_f;
			else if(mode_flag == 3)
				shape_D[Main.number +5] = value_f; 
			WriteToolFile("shape_D");
			break;
		case 24:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				wear_D[Main.number +5] += value_f;
			else if(mode_flag == 3)
				wear_D[Main.number +5] = value_f; 
			WriteToolFile("wear_D");
			break;
		case 25:
			if(mode_flag == 1)
				shape_H[Main.number +6] = value_f; 
			else if(mode_flag == 2)
				shape_H[Main.number +6] += value_f;
			else if(mode_flag == 3)
				shape_H[Main.number +6] = value_f; 
			WriteToolFile("shape_H");
			break;
		case 26:
			if(mode_flag == 1)
				wear_H[Main.number +6] = value_f; 
			else if(mode_flag == 2)
				wear_H[Main.number +6] += value_f;
			else if(mode_flag == 3)
				wear_H[Main.number +6] = value_f; 
			WriteToolFile("wear_H");
			break;
		case 27:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				shape_D[Main.number +6] += value_f;
			else if(mode_flag == 3)
				shape_D[Main.number +6] = value_f; 
			WriteToolFile("shape_D");
			break;
		case 28:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				wear_D[Main.number +6] += value_f;
			else if(mode_flag == 3)
				wear_D[Main.number +6] = value_f; 
			WriteToolFile("wear_D");
			break;
		case 29:
			if(mode_flag == 1)
				shape_H[Main.number +7] = value_f; 
			else if(mode_flag == 2)
				shape_H[Main.number +7] += value_f;
			else if(mode_flag == 3)
				shape_H[Main.number +7] = value_f; 
			WriteToolFile("shape_H");
			break;
		case 30:
			if(mode_flag == 1)
				wear_H[Main.number +7] = value_f; 
			else if(mode_flag == 2)
				wear_H[Main.number +7] += value_f;
			else if(mode_flag == 3)
				wear_H[Main.number +7] = value_f; 
			WriteToolFile("wear_H");
			break;
		case 31:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				shape_D[Main.number +7] += value_f;
			else if(mode_flag == 3)
				shape_D[Main.number +7] = value_f; 
			WriteToolFile("shape_D");
			break;
		case 32:
			if(mode_flag == 1)	
				return;
			else if(mode_flag == 2)
				wear_D[Main.number +7] += value_f;
			else if(mode_flag == 3)
				wear_D[Main.number +7] = value_f; 
			WriteToolFile("wear_D");
			break;
		default:
			break;
		}	
	}
	
	//设定界面下移
	public void argu_down()
	{
		switch(Main.argu_setting)
		{
		case 1:
			Main.argu_setting = 2;
			ArguCursorPos();
			break;
		case 2:
			Main.argu_setting = 3;
			ArguCursorPos();
			break;
		case 3:
			Main.argu_setting = 4;
			ArguCursorPos();
			break;
		case 4:
			Main.argu_setting = 5;
			ArguCursorPos();
			break;
		case 5:
			Main.argu_setting = 6;
			ArguCursorPos();
			break;
		case 6:
			Main.argu_setting = 7;
			ArguCursorPos();
			break;
		case 7:
			Main.argu_setting = 8;
			ArguCursorPos();
			break;
		case 8:
			Main.argu_setting = 9;
			ArguCursorPos();
			break;
		}
	}
	
		//设定界面上移
	public void argu_up()
	{
		switch(Main.argu_setting)
		{
		case 9:
			Main.argu_setting = 8;
			ArguCursorPos();
			break;
		case 8:
			Main.argu_setting = 7;
			ArguCursorPos();
			break;
		case 7:
			Main.argu_setting = 6;
			ArguCursorPos();
			break;
		case 6:
			Main.argu_setting = 5;
			ArguCursorPos();
			break;
		case 5:
			Main.argu_setting = 4;
			ArguCursorPos();
			break;
		case 4:
			Main.argu_setting = 3;
			ArguCursorPos();
			break;
		case 3:
			Main.argu_setting = 2;
			ArguCursorPos();
			break;
		case 2:
			Main.argu_setting = 1;
			ArguCursorPos();
			break;
		}
	}
	
	//黄色背景位置
	public void ArguCursorPos()
	{
		switch(Main.argu_setting)
		{
		case 1:
		    Main.argu_setting_cursor_y = 61.5f;
			Main.argu_setting_cursor_w = 16f;
			break;
		case 2:
			Main.argu_setting_cursor_y = 86.5f;
			Main.argu_setting_cursor_w = 16f;
			break;
		case 3:
			Main.argu_setting_cursor_y = 112f;
			Main.argu_setting_cursor_w = 16f;
			break;
		case 4:
			Main.argu_setting_cursor_y = 136.5f;
			Main.argu_setting_cursor_w = 16f;
			break;
		case 5:
			Main.argu_setting_cursor_y = 161.5f;
			Main.argu_setting_cursor_w = 36f;
			break;
		case 6:
			Main.argu_setting_cursor_y = 186.5f;
			Main.argu_setting_cursor_w = 16f;
			break;
		case 7:
			Main.argu_setting_cursor_y = 212f;
			Main.argu_setting_cursor_w = 16f;
			break;
		case 8:
			Main.argu_setting_cursor_y = 236.5f;
			Main.argu_setting_cursor_w = 116f;
			break;
		case 9:
			Main.argu_setting_cursor_y = 261.5f;
			Main.argu_setting_cursor_w = 116f;
			break;
		}
	}
	
	public void set_parameter(string input)
	{
		//Debug.Log(input);
		switch (Main.argu_setting)
		{
		case 1:
			if( input == "0" || input=="1")
			{
				PlayerPrefs.SetString("parameter_writabel", input);
			    parameter_writabel = input;
				//Debug.Log(parameter);
			}
			else
				Debug.Log("请输入0或1");
			break;
		case 2:
		    if( input == "0" || input=="1")
			{
				PlayerPrefs.SetString("TV_check", input);
				TV_check = input;
			}
			else
				Debug.Log("请输入0或1");
			break;
		case 3:
			if( input == "0" || input=="1")
			{
				PlayerPrefs.SetString("hole_code", input);
				hole_code = input;
			}
			else
				Debug.Log("请输入0或1");
			break;
		case 4:
			if( input == "0" || input=="1")
			{
				PlayerPrefs.SetString("input_unit", input);
				input_unit = input;
			}
			else
				Debug.Log("请输入0或1");
			break;
		case 5:
			if(input.Length > 2)
				Debug.Log("请输入0~35");
			else
			{
				if(input.Length == 2)
				{
					Regex num_Reg = new Regex(@"\d{2}");
					if(num_Reg.IsMatch(input))
					{
						int temp_value = Convert.ToInt32(input);
						if(temp_value > 35)
							Debug.Log("请输入0~35");
						else
						{
							PlayerPrefs.SetString("IO", input);
							IO = input;	
						}	
					}
					else
						Debug.Log("请输入0~35");
				}
				else
				{
					
					Regex num_Reg = new Regex(@"\d{1}");
					if(num_Reg.IsMatch(input))
					{
						int temp_value = Convert.ToInt32(input);
						if(temp_value > 35)
							Debug.Log("请输入0~35");
						else
						{
							PlayerPrefs.SetString("IO", input);
							IO = input;	
						}	
					}
					else
						Debug.Log("请输入0~35");
				}
			}
			break;
		case 6:
			if( input == "0" || input=="1")
			{
				PlayerPrefs.SetString("sequence_number", input);
				sequence_number= input;
			}
			else
				Debug.Log("请输入0或1");
			break;
		case 7:
			if( input == "0" || input=="1")
			{
				PlayerPrefs.SetString("paper_tape", input);
				paper_tape = input;
			}
			else
				Debug.Log("请输入0或1");
			break;
		case 8:
			PlayerPrefs.SetString("SN_stop1", input);
			SN_stop1 = input;
			break;
		case 9:
			PlayerPrefs.SetString("SN_stop2", input);
			SN_stop2 = input;
			break;
		default:
			Debug.Log("out of range");
			break;
		}	
	}
	
	//参数界面内容
	public void ReadCooFile () 
	{
		string line_str = "";
		string[] coo_str;
		StreamReader line_str_reader;
		FileStream coo_stream = new FileStream(Application.dataPath + SystemArguments.CoordinatePath + "G00.txt", FileMode.OpenOrCreate, FileAccess.Read);
		line_str_reader = new StreamReader(coo_stream);
		line_str = line_str_reader.ReadLine();
		if(line_str == null)
		{
			G00_pos.x = 0f;
			G00_pos.y = 0f;
			G00_pos.z = 0f;
		}
		else
		{
			coo_str = line_str.Split(',');
			G00_pos.x = float.Parse(coo_str[0]);
			G00_pos.y = float.Parse(coo_str[1]);
			G00_pos.z = float.Parse(coo_str[2]);
		}
		line_str_reader.Close();
		coo_stream = new FileStream(Application.dataPath + SystemArguments.CoordinatePath + "G54.txt", FileMode.OpenOrCreate, FileAccess.Read);
		line_str_reader = new StreamReader(coo_stream);
		line_str = line_str_reader.ReadLine();
		if(line_str == null)
		{
			G54_pos.x = 0f;
			G54_pos.y = 0f;
			G54_pos.z = 0f;
		}
		else
		{
			coo_str = line_str.Split(',');
			G54_pos.x = float.Parse(coo_str[0]);
			G54_pos.y = float.Parse(coo_str[1]);
			G54_pos.z = float.Parse(coo_str[2]);
		}
		line_str_reader.Close();
		coo_stream = new FileStream(Application.dataPath + SystemArguments.CoordinatePath + "G55.txt", FileMode.OpenOrCreate, FileAccess.Read);
		line_str_reader = new StreamReader(coo_stream);
		line_str = line_str_reader.ReadLine();
		if(line_str == null)
		{
			G55_pos.x = 0f;
			G55_pos.y = 0f;
			G55_pos.z = 0f;
		}
		else
		{
			coo_str = line_str.Split(',');
			G55_pos.x = float.Parse(coo_str[0]);
			G55_pos.y = float.Parse(coo_str[1]);
			G55_pos.z = float.Parse(coo_str[2]);
		}
		line_str_reader.Close();
		coo_stream = new FileStream(Application.dataPath + SystemArguments.CoordinatePath + "G56.txt", FileMode.OpenOrCreate, FileAccess.Read);
		line_str_reader = new StreamReader(coo_stream);
		line_str = line_str_reader.ReadLine();
		if(line_str == null)
		{
			G56_pos.x = 0f;
			G56_pos.y = 0f;
			G56_pos.z = 0f;
		}
		else
		{
			coo_str = line_str.Split(',');
			G56_pos.x = float.Parse(coo_str[0]);
			G56_pos.y = float.Parse(coo_str[1]);
			G56_pos.z = float.Parse(coo_str[2]);
		}
		line_str_reader.Close();
		coo_stream = new FileStream(Application.dataPath + SystemArguments.CoordinatePath + "G57.txt", FileMode.OpenOrCreate, FileAccess.Read);
		line_str_reader = new StreamReader(coo_stream);
		line_str = line_str_reader.ReadLine();
		if(line_str == null)
		{
			G57_pos.x = 0f;
			G57_pos.y = 0f;
			G57_pos.z = 0f;
		}
		else
		{
			coo_str = line_str.Split(',');
			G57_pos.x = float.Parse(coo_str[0]);
			G57_pos.y = float.Parse(coo_str[1]);
			G57_pos.z = float.Parse(coo_str[2]);
		}
		line_str_reader.Close();
		coo_stream = new FileStream(Application.dataPath + SystemArguments.CoordinatePath + "G58.txt", FileMode.OpenOrCreate, FileAccess.Read);
		line_str_reader = new StreamReader(coo_stream);
		line_str = line_str_reader.ReadLine();
		if(line_str == null)
		{
			G58_pos.x = 0f;
			G58_pos.y = 0f;
			G58_pos.z = 0f;
		}
		else
		{
			coo_str = line_str.Split(',');
			G58_pos.x = float.Parse(coo_str[0]);
			G58_pos.y = float.Parse(coo_str[1]);
			G58_pos.z = float.Parse(coo_str[2]);
		}
		line_str_reader.Close();
		coo_stream = new FileStream(Application.dataPath + SystemArguments.CoordinatePath + "G59.txt", FileMode.OpenOrCreate, FileAccess.Read);
		line_str_reader = new StreamReader(coo_stream);
		line_str = line_str_reader.ReadLine();
		if(line_str == null)
		{
			G59_pos.x = 0f;
			G59_pos.y = 0f;
			G59_pos.z = 0f;
		}
		else
		{
			coo_str = line_str.Split(',');
			G59_pos.x = float.Parse(coo_str[0]);
			G59_pos.y = float.Parse(coo_str[1]);
			G59_pos.z = float.Parse(coo_str[2]);
		}
		line_str_reader.Close();
	}
	
	void WriteCooFile (string filename, string write_str)
	{
		StreamWriter line_str_writer;
		FileStream coo_stream;
		FileInfo check_exist = new FileInfo(Application.dataPath +SystemArguments.CoordinatePath + filename+".txt");
		if(check_exist.Exists)
			coo_stream = new FileStream(Application.dataPath +SystemArguments.CoordinatePath + filename+".txt", FileMode.Truncate, FileAccess.Write);
		else
			coo_stream = new FileStream(Application.dataPath +SystemArguments.CoordinatePath + filename+".txt", FileMode.Create, FileAccess.Write);
		line_str_writer = new StreamWriter(coo_stream);
		line_str_writer.WriteLine(write_str);
		line_str_writer.Close();
	}
	

	public void Down () 
	{
		switch (Main.coo_setting_1)
		{
		case 1:
			if(Main.coo_setting_2 == 1)
				Main.coo_setting_2 = 2;
			else if(Main.coo_setting_2 == 2)
				Main.coo_setting_2 = 3;
			else
			{
				Main.coo_setting_2 = 1;
				Main.coo_setting_1 = 2;
			}
			CooCursorPos();
			break;
		case 2:
			if(Main.coo_setting_2 == 1)
				Main.coo_setting_2 = 2;
			else if(Main.coo_setting_2 == 2)
				Main.coo_setting_2 = 3;
			else
			{
				Main.coo_setting_2 = 1;
				Main.coo_setting_1 = 3;
			}
			CooCursorPos();
			break;
		case 3:
			if(Main.coo_setting_2 == 1)
				Main.coo_setting_2 = 2;
			else if(Main.coo_setting_2 == 2)
				Main.coo_setting_2 = 3;
			else
			{
				Main.coo_setting_2 = 1;
				Main.coo_setting_1 = 4;
			}
			CooCursorPos();
			break;
		case 4:
			if(Main.coo_setting_2 == 1)
				Main.coo_setting_2 = 2;
			else if(Main.coo_setting_2 == 2)
				Main.coo_setting_2 = 3;
			else
			{
				Main.coo_setting_2 = 1;
				Main.coo_setting_1 = 5;
				Main.OffCooFirstPage = false;
			}
			CooCursorPos();
			break;
		case 5:
			if(Main.coo_setting_2 == 1)
				Main.coo_setting_2 = 2;
			else if(Main.coo_setting_2 == 2)
				Main.coo_setting_2 = 3;
			else
			{
				Main.coo_setting_2 = 1;
				Main.coo_setting_1 = 6;
			}
			CooCursorPos();
			break;
		case 6:
			if(Main.coo_setting_2 == 1)
				Main.coo_setting_2 = 2;
			else if(Main.coo_setting_2 == 2)
				Main.coo_setting_2 = 3;
			else
			{
				Main.coo_setting_2 = 1;
				Main.coo_setting_1 = 7;
			}
			CooCursorPos();
			break;
		case 7:
			if(Main.coo_setting_2 == 1)
				Main.coo_setting_2 = 2;
			else if(Main.coo_setting_2 == 2)
				Main.coo_setting_2 = 3;
			CooCursorPos();
			break;
		}
	}
	
	public void Up () 
	{
		switch(Main.coo_setting_1)
		{
		case 1:
			if(Main.coo_setting_2 == 2)
				Main.coo_setting_2 = 1;
			else if(Main.coo_setting_2 == 3)
				Main.coo_setting_2 = 2;
			CooCursorPos();
			break;
		case 2:
			if(Main.coo_setting_2 == 2)
				Main.coo_setting_2 = 1;
			else if(Main.coo_setting_2 == 3)
				Main.coo_setting_2 = 2;
			else
			{
				Main.coo_setting_2 = 3;
				Main.coo_setting_1 = 1;
			}
			CooCursorPos();
			break;
		case 3:
			if(Main.coo_setting_2 == 2)
				Main.coo_setting_2 = 1;
			else if(Main.coo_setting_2 == 3)
				Main.coo_setting_2 = 2;
			else
			{
				Main.coo_setting_2 = 3;
				Main.coo_setting_1 = 2;
			}
			CooCursorPos();
			break;
		case 4:
			if(Main.coo_setting_2 == 2)
				Main.coo_setting_2 = 1;
			else if(Main.coo_setting_2 == 3)
				Main.coo_setting_2 = 2;
			else
			{
				Main.coo_setting_2 = 3;
				Main.coo_setting_1 = 3;
			}
			CooCursorPos();
			break;
		case 5:
			if(Main.coo_setting_2 == 2)
				Main.coo_setting_2 = 1;
			else if(Main.coo_setting_2 == 3)
				Main.coo_setting_2 = 2;
			else
			{
				Main.coo_setting_2 = 3;
				Main.coo_setting_1 = 4;
				Main.OffCooFirstPage = true;
			}
			CooCursorPos();
			break;
		case 6:
			if(Main.coo_setting_2 == 2)
				Main.coo_setting_2 = 1;
			else if(Main.coo_setting_2 == 3)
				Main.coo_setting_2 = 2;
			else
			{
				Main.coo_setting_2 = 3;
				Main.coo_setting_1 = 5;
			}
			CooCursorPos();
			break;
		case 7:
			if(Main.coo_setting_2 == 2)
				Main.coo_setting_2 = 1;
			else if(Main.coo_setting_2 == 3)
				Main.coo_setting_2 = 2;
			else
			{
				Main.coo_setting_2 = 3;
				Main.coo_setting_1 = 6;
			}
			CooCursorPos();
			break;
		}
	}
	
	public void Left () 
	{
		switch(Main.coo_setting_1)
		{
		case 1:
			if(Main.coo_setting_2 == 3 || Main.coo_setting_2 == 2)
			{
				Main.coo_setting_2--;
				Main.coo_setting_1 = 3;
				CooCursorPos();
			}
			break;
		case 2:
			if(Main.coo_setting_2 == 3 || Main.coo_setting_2 == 2)
			{
				Main.coo_setting_2--;
				Main.coo_setting_1 = 4;
			}
			else
			{
				Main.coo_setting_1 = 3;
				Main.coo_setting_2 = 3;
			}
			CooCursorPos();
			break;
		case 3:
			Main.coo_setting_1 = 1;
			CooCursorPos();
			break;
		case 4:
			Main.coo_setting_1 = 2;
			CooCursorPos();
			break;
		case 5:
			if(Main.coo_setting_2 == 3 || Main.coo_setting_2 == 2)
			{
				Main.coo_setting_2--;
				Main.coo_setting_1 = 7;
			}
			else
			{
				Main.coo_setting_1 = 4;
				Main.coo_setting_2 = 3;
				Main.OffCooFirstPage = true;
			}
			CooCursorPos();
			break;
		case 6:
			if(Main.coo_setting_2 == 3 || Main.coo_setting_2 == 2)
			{
				Main.coo_setting_2--;
			}
			else
			{
				Main.coo_setting_1 = 7;
				Main.coo_setting_2 = 3;
			}
			CooCursorPos();
			break;
		case 7:
			Main.coo_setting_1 = 5;
			CooCursorPos();
			break;
		}
	}
	
	public void Right () 
	{
		switch(Main.coo_setting_1)
		{
		case 1:
			Main.coo_setting_1 = 3;
			CooCursorPos();
			break;
		case 2:
			Main.coo_setting_1 = 4;
			CooCursorPos();
			break;
		case 3:
			if(Main.coo_setting_2 == 1 || Main.coo_setting_2 == 2)
			{
				Main.coo_setting_2++;
				Main.coo_setting_1 = 1;
			}
			else
			{
				Main.coo_setting_1 = 2;
				Main.coo_setting_2 = 1;
			}
			CooCursorPos();
			break;
		case 4:
			if(Main.coo_setting_2 == 1 || Main.coo_setting_2 == 2)
			{
				Main.coo_setting_2++;
				Main.coo_setting_1 = 2;
			}
			else
			{
				Main.coo_setting_1 = 5;
				Main.coo_setting_2 = 1;
				Main.OffCooFirstPage = false;
			}
			CooCursorPos();
			break;
		case 5:
			Main.coo_setting_1 = 7;
			CooCursorPos();
			break;
		case 6:
			if(Main.coo_setting_2 == 1 || Main.coo_setting_2 == 2)
			{
				Main.coo_setting_2++;
				CooCursorPos();
			}
			break;
		case 7:
			if(Main.coo_setting_2 == 1 || Main.coo_setting_2 == 2)
			{
				Main.coo_setting_2++;
				Main.coo_setting_1 = 5;
			}
			else
			{
				Main.coo_setting_1 = 6;
				Main.coo_setting_2 = 1;
			}
			CooCursorPos();
			break;
		}
	}
	
	public void PageUp ()
	{
		switch (Main.coo_setting_1)
		{
		case 5:
			Main.coo_setting_1 = 1;
			break;
		case 6:
			Main.coo_setting_1 = 2;
			break;
		case 7:
			Main.coo_setting_1 = 3;
			break;
		}
	}
	
	public void PageDown ()
	{
		switch (Main.coo_setting_1)
		{
		case 1:
			Main.coo_setting_1 = 5;
			break;
		case 2:
			Main.coo_setting_1 = 6;
			break;
		case 3:
			Main.coo_setting_1 = 7;
			break;
		case 4:
			Main.coo_setting_1 = 7;
			Main.coo_setting_2 = 3;
			CooCursorPos();
			break;
		}
	}
	
	public void CooCursorPos () 
	{
		switch(Main.coo_setting_1)
		{
		case 1:
		case 5:
			Main.coo_setting_cursor_x = 131f;
			if(Main.coo_setting_2 == 1)
				Main.coo_setting_cursor_y = 120f;
			else if(Main.coo_setting_2 == 2)
				Main.coo_setting_cursor_y = 150f;
			else
				Main.coo_setting_cursor_y = 180f;
			break;
		case 2:
		case 6:
			Main.coo_setting_cursor_x = 131f;
			if(Main.coo_setting_2 == 1)
				Main.coo_setting_cursor_y = 240f;
			else if(Main.coo_setting_2 == 2)
				Main.coo_setting_cursor_y = 270f;
			else
				Main.coo_setting_cursor_y = 300f;
			break;
		case 3:
		case 7:
			Main.coo_setting_cursor_x = 376f;
			if(Main.coo_setting_2 == 1)
				Main.coo_setting_cursor_y = 120f;
			else if(Main.coo_setting_2 == 2)
				Main.coo_setting_cursor_y = 150f;
			else
				Main.coo_setting_cursor_y = 180f;
			break;
		case 4:
			Main.coo_setting_cursor_x = 376f;
			if(Main.coo_setting_2 == 1)
				Main.coo_setting_cursor_y = 240f;
			else if(Main.coo_setting_2 == 2)
				Main.coo_setting_cursor_y = 270f;
			else
				Main.coo_setting_cursor_y = 300f;
			break;
		}
	}
	
	public void SearchNo (string num_str) 
	{
		string str_temp = num_str.TrimStart('0', ' ');
		switch (str_temp)
		{
		case "":
			Main.coo_setting_1 = 1;
			Main.coo_setting_2 = 1;
			CooCursorPos();
			Main.OffCooFirstPage = true;
			break;
		case "1":
			Main.coo_setting_1 = 2;
			Main.coo_setting_2 = 1;
			CooCursorPos();
			Main.OffCooFirstPage = true;
			break;
		case "2":
			Main.coo_setting_1 = 3;
			Main.coo_setting_2 = 1;
			CooCursorPos();
			Main.OffCooFirstPage = true;
			break;
		case "3":
			Main.coo_setting_1 = 4;
			Main.coo_setting_2 = 1;
			CooCursorPos();
			Main.OffCooFirstPage = true;
			break;
		case "4":
			Main.coo_setting_1 = 5;
			Main.coo_setting_2 = 1;
			CooCursorPos();
			Main.OffCooFirstPage = false;
			break;
		case "5":
			Main.coo_setting_1 = 6;
			Main.coo_setting_2 = 1;
			CooCursorPos();
			Main.OffCooFirstPage = false;
			break;
		case "6":
			Main.coo_setting_1 = 7;
			Main.coo_setting_2 = 1;
			CooCursorPos();
			Main.OffCooFirstPage = false;
			break;
		default:
			Debug.Log("格式不正确");
			break;
		}
	}
	
	public void Measure (string coo_value)
	{
		char[] coo_choose = coo_value.ToCharArray();
		string value_str = "";
		float value_f = 0;
		if(coo_choose.Length < 2 || (coo_choose[0] != 'X' && coo_choose[0] != 'Y'  && coo_choose[0] != 'Z' && coo_choose[0] != 'x' && coo_choose[0] != 'y' && coo_choose[0] != 'z' ))
		{
			Debug.Log("Format Error!!!");
			return;
		}
		else
		{
			for(int i = 1; i < coo_choose.Length; i++)
			{
				if(coo_choose[i] != '.'  && coo_choose[i] != '+' && coo_choose[i] != '-')
				{
					if(coo_choose[i] < '0' || coo_choose[i] > '9')
					{
						Debug.Log("Format Error!!!");
						return;
					}
				}
				else if(coo_choose[i] == '+' || coo_choose[i] == '-')
				{
					if(i != 1)
					{
						Debug.Log("Format Error!!!");
						return;
					}
				}
				value_str += coo_choose[i];
			}
			if(value_str == "+" || value_str == "-")
			{
				Debug.Log("Format Error!!!");
				return;
			}
			value_f = float.Parse(value_str);
			//Debug.Log(value_f);
			switch(coo_choose[0])
			{
			case 'X':
			case 'x':
				Measure_choose(1, value_f, 1);
				Main.coo_setting_2 = 1;
				break;
			case 'Y':
			case 'y':
				Measure_choose(2, value_f, 1);
				Main.coo_setting_2 = 2;
				break;
			case 'Z':
			case 'z':
				Measure_choose(3, value_f, 1);
				Main.coo_setting_2 = 3;
				break;
			default:
				Debug.Log("Format Error!!!");
				break;
			}
			CooCursorPos();
		}		
	}
	
	public void PlusInput (string input_value, bool plus_flag) 
	{
		char[] coo_choose = input_value.ToCharArray();
		string value_str = "";
		float value_f = 0;
		for(int i = 0; i < coo_choose.Length; i++)
		{
			if(coo_choose[i] != '.'  && coo_choose[i] != '+' && coo_choose[i] != '-')
			{
				if(coo_choose[i] < '0' || coo_choose[i] > '9')
				{
					Debug.Log("Format Error!!!");
					return;
				}
			}
			else if(coo_choose[i] == '+' || coo_choose[i] == '-')
			{
				if(i != 0)
				{
					Debug.Log("Format Error!!!");
					return;
				}
			}
			value_str += coo_choose[i];
		}
		if(value_str == "+" || value_str == "-")
		{
			Debug.Log("Format Error!!!");
			return;
		}
		value_f = float.Parse(value_str);
		if(plus_flag)
			Measure_choose(Main.coo_setting_2, value_f, 2);
		else
			Measure_choose(Main.coo_setting_2, value_f, 3);
	}
	
	void Measure_choose (int xyz_select, float value_f, int mode_flag) 
	{
		string write_str = "";
		switch(Main.coo_setting_1)
		{
		case 1:
			if(xyz_select == 1)
			{
				if(mode_flag == 1)
					G00_pos.x = MoveControl_script.MachineCoo.x - value_f;
				else if(mode_flag == 2)
					G00_pos.x += value_f; 
				else
					G00_pos.x = value_f; 
				write_str = G00_pos.x+","+G00_pos.y+","+G00_pos.z;
				WriteCooFile("G00", write_str);
			}
			else if(xyz_select == 2)
			{
				if(mode_flag == 1)
					G00_pos.y = MoveControl_script.MachineCoo.y - value_f;
				else if(mode_flag == 2)
					G00_pos.y += value_f; 
				else
					G00_pos.y = value_f; 
				write_str = G00_pos.x+","+G00_pos.y+","+G00_pos.z;
				WriteCooFile("G00", write_str);
			}
			else
			{
				if(mode_flag == 1)
					G00_pos.z = MoveControl_script.MachineCoo.z - value_f;
				else if(mode_flag == 2)
					G00_pos.z += value_f; 
				else
					G00_pos.z = value_f; 
				write_str = G00_pos.x+","+G00_pos.y+","+G00_pos.z;
				WriteCooFile("G00", write_str);
			}
			Main.OffCooFirstPage = true;
			Workpiece_Change();
			break;
		case 2:
			if(xyz_select == 1)
			{
				if(mode_flag == 1)
					G54_pos.x = MoveControl_script.MachineCoo.x - value_f;
				else if(mode_flag == 2)
					G54_pos.x += value_f; 
				else
					G54_pos.x = value_f; 
				write_str = G54_pos.x+","+G54_pos.y+","+G54_pos.z;
				WriteCooFile("G54", write_str);
			}
			else if(xyz_select == 2)
			{
				if(mode_flag == 1)
					G54_pos.y = MoveControl_script.MachineCoo.y - value_f;
				else if(mode_flag == 2)
					G54_pos.y += value_f; 
				else
					G54_pos.y = value_f; 
				write_str = G54_pos.x+","+G54_pos.y+","+G54_pos.z;
				WriteCooFile("G54", write_str);
			}
			else
			{
				if(mode_flag == 1)
					G54_pos.z = MoveControl_script.MachineCoo.z - value_f;
				else if(mode_flag == 2)
					G54_pos.z += value_f; 
				else
					G54_pos.z = value_f; 
				write_str = G54_pos.x+","+G54_pos.y+","+G54_pos.z;
				WriteCooFile("G54", write_str);
			}
			Main.OffCooFirstPage = true;
			Workpiece_Change();
			break;
		case 3:
			if(xyz_select == 1)
			{
				if(mode_flag == 1)
					G55_pos.x = MoveControl_script.MachineCoo.x - value_f;
				else if(mode_flag == 2)
					G55_pos.x += value_f; 
				else
					G55_pos.x = value_f; 
				write_str = G55_pos.x+","+G55_pos.y+","+G55_pos.z;
				WriteCooFile("G55", write_str);
			}
			else if(xyz_select == 2)
			{
				if(mode_flag == 1)
					G55_pos.y = MoveControl_script.MachineCoo.y - value_f;
				else if(mode_flag == 2)
					G55_pos.y += value_f; 
				else
					G55_pos.y = value_f; 
				write_str = G55_pos.x+","+G55_pos.y+","+G55_pos.z;
				WriteCooFile("G55", write_str);
			}
			else
			{
				if(mode_flag == 1)
					G55_pos.z = MoveControl_script.MachineCoo.z - value_f;
				else if(mode_flag == 2)
					G55_pos.z += value_f; 
				else
					G55_pos.z = value_f; 
				write_str = G55_pos.x+","+G55_pos.y+","+G55_pos.z;
				WriteCooFile("G55", write_str);
			}
			Main.OffCooFirstPage = true;
			Workpiece_Change();
			break;
		case 4:
			if(xyz_select == 1)
			{
				if(mode_flag == 1)
					G56_pos.x = MoveControl_script.MachineCoo.x - value_f;
				else if(mode_flag == 2)
					G56_pos.x += value_f; 
				else
					G56_pos.x = value_f; 
				write_str = G56_pos.x+","+G56_pos.y+","+G56_pos.z;
				WriteCooFile("G56", write_str);
			}
			else if(xyz_select == 2)
			{
				if(mode_flag == 1)
					G56_pos.y = MoveControl_script.MachineCoo.y - value_f;
				else if(mode_flag == 2)
					G56_pos.y += value_f; 
				else
					G56_pos.y = value_f;
				write_str = G56_pos.x+","+G56_pos.y+","+G56_pos.z;
				WriteCooFile("G56", write_str);
			}
			else
			{
				if(mode_flag == 1)
					G56_pos.z = MoveControl_script.MachineCoo.z - value_f;
				else if(mode_flag == 2)
					G56_pos.z += value_f; 
				else
					G56_pos.z = value_f;
				write_str = G56_pos.x+","+G56_pos.y+","+G56_pos.z;
				WriteCooFile("G56", write_str);
			}
			Main.OffCooFirstPage = true;
			Workpiece_Change();
			break;
		case 5:
			if(xyz_select == 1)
			{
				if(mode_flag == 1)
					G57_pos.x = MoveControl_script.MachineCoo.x - value_f;
				else if(mode_flag == 2)
					G57_pos.x += value_f; 
				else
					G57_pos.x = value_f;
				write_str = G57_pos.x+","+G57_pos.y+","+G57_pos.z;
				WriteCooFile("G57", write_str);
			}
			else if(xyz_select == 2)
			{
				if(mode_flag == 1)
					G57_pos.y = MoveControl_script.MachineCoo.y - value_f;
				else if(mode_flag == 2)
					G57_pos.y += value_f; 
				else
					G57_pos.y = value_f;
				write_str = G57_pos.x+","+G57_pos.y+","+G57_pos.z;
				WriteCooFile("G57", write_str);
			}
			else
			{
				if(mode_flag == 1)
					G57_pos.z = MoveControl_script.MachineCoo.z - value_f;
				else if(mode_flag == 2)
					G57_pos.z += value_f; 
				else
					G57_pos.z = value_f;
				write_str = G57_pos.x+","+G57_pos.y+","+G57_pos.z;
				WriteCooFile("G57", write_str);
			}
			Main.OffCooFirstPage = false;
			Workpiece_Change();
			break;
		case 6:
			if(xyz_select == 1)
			{
				if(mode_flag == 1)
					G58_pos.x = MoveControl_script.MachineCoo.x - value_f;
				else if(mode_flag == 2)
					G58_pos.x += value_f; 
				else
					G58_pos.x = value_f;
				write_str = G58_pos.x+","+G58_pos.y+","+G58_pos.z;
				WriteCooFile("G58", write_str);
			}
			else if(xyz_select == 2)
			{
				if(mode_flag == 1)
					G58_pos.y = MoveControl_script.MachineCoo.y - value_f;
				else if(mode_flag == 2)
					G58_pos.y += value_f; 
				else
					G58_pos.y = value_f;
				write_str = G58_pos.x+","+G58_pos.y+","+G58_pos.z;
				WriteCooFile("G58", write_str);
			}
			else
			{
				if(mode_flag == 1)
					G58_pos.z = MoveControl_script.MachineCoo.z - value_f;
				else if(mode_flag == 2)
					G58_pos.z += value_f; 
				else
					G58_pos.z = value_f;
				write_str = G58_pos.x+","+G58_pos.y+","+G58_pos.z;
				WriteCooFile("G58", write_str);
			}
			Main.OffCooFirstPage = false;
			Workpiece_Change();
			break;
		case 7:
			if(xyz_select == 1)
			{
				if(mode_flag == 1)
					G59_pos.x = MoveControl_script.MachineCoo.x - value_f;
				else if(mode_flag == 2)
					G59_pos.x += value_f; 
				else
					G59_pos.x = value_f;
				write_str = G59_pos.x+","+G59_pos.y+","+G59_pos.z;
				WriteCooFile("G59", write_str);
			}
			else if(xyz_select == 2)
			{
				if(mode_flag == 1)
					G59_pos.y = MoveControl_script.MachineCoo.y - value_f;
				else if(mode_flag == 2)
					G59_pos.y += value_f; 
				else
					G59_pos.y = value_f;
				write_str = G59_pos.x+","+G59_pos.y+","+G59_pos.z;
				WriteCooFile("G59", write_str);
			}
			else
			{
				if(mode_flag == 1)
					G59_pos.z = MoveControl_script.MachineCoo.z - value_f;
				else if(mode_flag == 2)
					G59_pos.z += value_f; 
				else
					G59_pos.z = value_f;
				write_str = G59_pos.x+","+G59_pos.y+","+G59_pos.z;
				WriteCooFile("G59", write_str);
			}
			Main.OffCooFirstPage = false;
			Workpiece_Change();
			break;
		default:
			break;
		}
	}
	
	
	
	// Update is called once per frame
	void Update () {
		absolute_pos = MoveControl_script.MachineCoo - G00_pos - workpiece_coo;
		//Debug.Log(workpiece_coo.x +","+workpiece_coo.y+","+workpiece_coo.z);
		relative_pos = MoveControl_script.MachineCoo - G00_pos - workpiece_coo;
	}
	
	
	public void Workpiece_Change () {
		switch (workpiece_flag)
		{
		case 1:
			workpiece_coo = G54_pos;
			break;
		case 2:
			workpiece_coo = G55_pos;
			Debug.Log("2222222222");
			break;
		case 3:
			workpiece_coo = G56_pos;
			break;
		case 4:
			workpiece_coo = G57_pos;
			break;
		case 5:
			workpiece_coo = G58_pos;
			break;
		case 6:
			workpiece_coo = G59_pos;
			break;
		default:
			Debug.Log("workpiece flag is wrong!!!");
			break;
		}
	}
}
