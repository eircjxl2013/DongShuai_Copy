using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 编译数据存储结构
/// </summary>
public class DataStore
{
	public int slash_value;
	public string immediate_execution;
	public int motion_type;
	public float x_value;
	public float y_value;
	public float z_value;
	public bool[] xyz_state;
	public float s_value;
	public float f_value;
	public float i_value;
	public float j_value;
	public float k_value;
	public bool[] ijk_state;
	public float r_value;
	public int tool_number;
	public float x_remaining_movement;
	public float y_remaining_movement;
	public float z_remaining_movement;
	
	public List<string> G_code;
	public List<int> modal_index;
	public List<string> modal_string;
	
	public DataStore()
	{
		slash_value = 0;
		immediate_execution = "";
		motion_type = -1;
		x_value = 0;
		y_value = 0;
		z_value = 0;
		xyz_state = new bool[3]{false, false, false};
		s_value = 0;
		f_value = 0;
		i_value = 0;
		j_value = 0;
		k_value = 0;
		ijk_state = new bool[4]{false, false, false, false};
		r_value = 0;
		tool_number = 0;
		x_remaining_movement = 0;
		y_remaining_movement = 0;
		z_remaining_movement = 0;
		G_code = new List<string>();
		modal_index = new List<int>();
		modal_string = new List<string>();
	}
	
	public void Clear()
	{
		slash_value = 0;
		immediate_execution = "";
		motion_type = -1;
		x_value = 0;
		y_value = 0;
		z_value = 0;
		xyz_state[0] = false;
		xyz_state[1] = false;
		xyz_state[2] = false;
		s_value = 0;
		f_value = 0;
		i_value = 0;
		j_value = 0;
		k_value = 0;
		ijk_state[0] = false;
		ijk_state[1] = false;
		ijk_state[2] = false;
		//代表R是否有赋值
		ijk_state[3] = false;
		r_value = 0;
		tool_number = 0;
		x_remaining_movement = 0;
		y_remaining_movement = 0;
		z_remaining_movement = 0;
		G_code.Clear();
		modal_index.Clear();
		modal_string.Clear();
	}
	
	public bool IsEmpty()
	{
		if(slash_value != 0 || motion_type != -1 || xyz_state[0] || xyz_state[1] || xyz_state[2])
			return false;
		else if(s_value != 0 || f_value != 0 || ijk_state[0] || ijk_state[1] || ijk_state[2] || ijk_state[3])
			return false;
		else if(tool_number != 0 || x_remaining_movement != 0 || y_remaining_movement != 0 || z_remaining_movement != 0)
			return false;
		else if(immediate_execution != "" || G_code.Count != 0 || modal_index.Count != 0 || modal_string.Count != 0)
			return false;
		else
			return true;
	}
	
	public void ImmediateAdd(char char_str)
	{
		immediate_execution += char_str;
	}
	
	public bool HasMotion()
	{
		if(motion_type != -1 || xyz_state[0] || xyz_state[1] || xyz_state[2])
			return true;
		else if(ijk_state[0] || ijk_state[1] || ijk_state[2] || ijk_state[3])
			return true;
		else
			return false;
	}
	
	public int MotionTypeIndex(string motion_str)
	{
		switch(motion_str)
		{
		case "G00":
			return (int)MotionType.DryRunning;
		case "G01":
			return (int)MotionType.Line;
		case "G02":
			return (int)MotionType.Circular02;
		case "G03":
			return (int)MotionType.Circular03;
		default:
			return -1;
		}
	}
	
	public Vector3 AbsolutePosition(Vector3 display_pos)
	{
		if(xyz_state[0])
			display_pos.x = x_value;
		if(xyz_state[1])
			display_pos.y = y_value;
		if(xyz_state[2])
			display_pos.z = z_value;
		return display_pos;
	}
	
	public int CircleArguJudge(ref string error_string, ModalCode_Fanuc_M modal_state, ref Vector3 ijk_coo, ref Vector3 xyz_coo, ref float rValue)
	{
		//终点坐标在原地
		if(xyz_state[0] == false && xyz_state[1] == false && xyz_state[2] == false)
		{
			//若R有赋值，返回0停在原地
			if(ijk_state[3])
				return 0;
			//若R和IJK都没有赋值，返回0停在原地
			else if(ijk_state[0] == false && ijk_state[1] == false && ijk_state[2] == false)
				return 0;
			else
			{
				//三种平面下的判断及圆心增量坐标获取，如果返回1则为整圆情况
				switch(modal_state.Modal_Code[3])
				{
				case "G17":
					if(ijk_state[2])
					{
						error_string = "平面选择与输入坐标不符";
						return -1;
					}
					else
					{
						ijk_coo = new Vector3(i_value, j_value, 0);
						return 1;
					}
				case "G18":
					if(ijk_state[1])
					{
						error_string = "平面选择与输入坐标不符";
						return -1;
					}
					else
					{
						ijk_coo = new Vector3(i_value, 0, k_value);
						return 1;
					}
				case "G19":
					if(ijk_state[0])
					{
						error_string = "平面选择与输入坐标不符";
						return -1;
					}
					else
					{
						ijk_coo = new Vector3(0, j_value, k_value);
						return 1;
					}
				default:
					error_string = "平面选择错误，不存在这样的平面";
					return -1;
				}
			}
		}
		//有终点坐标
		else
		{
			//三种平面下的判断及圆心增量坐标和Display坐标（增量或最终值），如果返回2则为IJK，返回3则为R
			switch(modal_state.Modal_Code[3])
			{
				case "G17":
				if(xyz_state[2])
				{
					error_string = "平面选择与输入坐标不符";
					return -1;
				}
				else
				{
					xyz_coo = new Vector3(x_value, y_value, 0);
					//有R在
					if(ijk_state[3])
					{
						rValue = r_value;
						return 3;
					}
					//没有R
					else
					{
						if(ijk_state[2])
						{
							error_string = "平面选择与输入坐标不符";
							return -1;
						}
						else
						{
							ijk_coo = new Vector3(i_value, j_value, 0);
							return 2;
						}
					}
				}
			case "G18":
				if(xyz_state[1])
				{
					error_string = "平面选择与输入坐标不符";
					return -1;
				}
				else
				{
					xyz_coo = new Vector3(x_value, 0, z_value);
					//有R在
					if(ijk_state[3])
					{
						rValue = r_value;
						return 3;
					}
					//没有R
					else
					{
						if(ijk_state[1])
						{
							error_string = "平面选择与输入坐标不符";
							return -1;
						}
						else
						{
							ijk_coo = new Vector3(i_value, 0, k_value);
							return 2;
						}
					}
				}
			case "G19":
				if(xyz_state[0])
				{
					error_string = "平面选择与输入坐标不符";
					return -1;
				}
				else
				{
					xyz_coo = new Vector3(0, y_value, z_value);
					//有R在
					if(ijk_state[3])
					{
						rValue = r_value;
						return 3;
					}
					//没有R
					else
					{
						if(ijk_state[0])
						{
							error_string = "平面选择与输入坐标不符";
							return -1;
						}
						else
						{
							ijk_coo = new Vector3(0, j_value, k_value);
							return 2;
						}
					}
				}
			default:
				error_string = "平面选择错误，不存在这样的平面";
				return -1;
			}
		}
	}
	
	public override string ToString ()
	{
		string G_str = "";
		if(G_code.Count > 0)
		{
			for(int i = 0; i < G_code.Count; i++)
				G_str += G_code[i] + "; ";
		}
		else
			G_str = "null";
		string Modal_str = "";
		if(modal_string.Count > 0)
		{
			for(int i = 0; i < modal_string.Count; i++)
				Modal_str += modal_string[i] + "; ";
		}
		else
			Modal_str = "null";
		return "Slash: " + slash_value + "; Immediate execution: " + immediate_execution + "; Motion: " + motion_type + "; X: " + x_value.ToString() +
			"; Y: " + y_value.ToString() + "; Z: " + z_value.ToString() + "; S: " + s_value.ToString() + "; F: " + f_value.ToString() + "; I: " + 
				i_value.ToString() + "; J: " + j_value.ToString() + "; K: " + k_value.ToString() + "; R: " + r_value.ToString() + "; T: " + tool_number.ToString() + 
				"; X_remain: " + x_remaining_movement.ToString() + "; Y_remain: " + y_remaining_movement.ToString() + "; Z_remain: " + 
				z_remaining_movement.ToString() + "; Modal_str: " + Modal_str + "; G_str: " + G_str;
	}
	
	
}

/*
public interface IModal
{
	string[] Modal_Code{get;}
	int ModalIndex(string aim_code);
	bool SetModalCode(string aim_code, int index);
}
 */
/// <summary>
/// 模态代码数据结构，待修改，改进扩展机制
/// </summary>
public class ModalCode_Fanuc_M
{
	private string[] _modal_code;
	public string[] Modal_Code
	{
		get{return _modal_code;}
	}
	public bool Slash;
	public float Feedrate;
	private Dictionary<string, int> code_location;
	/// <summary>
	/// 普通初始化 <see cref="ModalCode_Fanuc_M"/> class.
	/// </summary>
	public ModalCode_Fanuc_M()
	{
		_modal_code = new string[]{"G00", "G94", "G80", "G17", "G21", "G98", "G90", "G40", "G50", "G22", 
			"G49", "G67", "G97", "G54", "G64", "G69", "G15", "G40.1", "G25", "G160", "G13.1", "G50.1", 
			"G54.2", "G80.5"};	
		Slash = false;
		Feedrate = 0;
		code_location = new Dictionary<string, int>();
		LocationInitialize();
	}
	/// <summary>
	/// 实现复制的初始化 <see cref="ModalCode_Fanuc_M"/> class.
	/// </summary>
	/// <param name='aim_class'>
	/// 复制的目标对象
	/// </param>
	public ModalCode_Fanuc_M (ModalCode_Fanuc_M aim_class)
	{
		_modal_code = new string[]{"G00", "G94", "G80", "G17", "G21", "G98", "G90", "G40", "G50", "G22", 
			"G49", "G67", "G97", "G54", "G64", "G69", "G15", "G40.1", "G25", "G160", "G13.1", "G50.1", 
			"G54.2", "G80.5"};	
		Slash = aim_class.Slash;
		Feedrate = aim_class.Feedrate;
		code_location = new Dictionary<string, int>();
		LocationInitialize();
		for(int i = 0; i < aim_class.Modal_Code.Length; i++)
		{
			_modal_code[i] = aim_class.Modal_Code[i];
		}
	}
	
	private void LocationInitialize()
	{
		string[] group01_01 = new string[]{"G0", "G00", "G1", "G01", "G2", "G02", "G3", "G03", "G33", "G75", 
			"G77", "G78", "G79"};
		string[] group05_02 = new string[]{"G93", "G94", "G95"};
		string[] group09_03 = new string[]{"G73", "G74", "G76", "G80", "G81", "G82", "G83", "G84", "G84.2", 
			"G84.3", "G85", "G86", "G87", "G88", "G89"};
		string[] group02_04 = new string[]{"G17", "G18", "G19"};
		string[] group06_05 = new string[]{"G20", "G21"};
		string[] group10_06 = new string[]{"G98", "G99"};
		string[] group03_07 = new string[]{"G90", "G91"};
		string[] group07_08 = new string[]{"G40", "G41", "G42"};
		string[] group11_09 = new string[]{"G50", "G51"};
		string[] group04_10 = new string[]{"G22", "G23"};
		string[] group08_11 = new string[]{"G43", "G44", "G49"};
		string[] group12_12 = new string[]{"G66", "G67"};
		string[] group13_13 = new string[]{"G96", "G97"};
		string[] group14_14 = new string[]{"G54", "G54.1", "G55", "G56", "G57", "G58", "G59"};
		string[] group15_15 = new string[]{"G61", "G62", "G63", "G64"};
		string[] group16_16 = new string[]{"G68", "G69"};
		string[] group17_17 = new string[]{"G15", "G16"};
		string[] group18_18 = new string[]{"G40.1", "G41.1", "G42.1"};
		string[] group25_19 = new string[]{"G25"};
		string[] group20_20 = new string[]{"G160", "G161"};
		string[] group131_21 = new string[]{"G13.1"};
		string[] group22_22 = new string[]{"G50.1", "G51.1"};
		string[] group542_23 = new string[]{"G54.2"};
		string[] group805_24 = new string[]{"G80.5"};
		List<string[]> location_group = new List<string[]>();
		location_group.Add(group01_01);
		location_group.Add(group05_02);
		location_group.Add(group09_03);
		location_group.Add(group02_04);
		location_group.Add(group06_05);
		location_group.Add(group10_06);
		location_group.Add(group03_07);
		location_group.Add(group07_08);
		location_group.Add(group11_09);
		location_group.Add(group04_10);
		location_group.Add(group08_11);
		location_group.Add(group12_12);
		location_group.Add(group13_13);
		location_group.Add(group14_14);
		location_group.Add(group15_15);
		location_group.Add(group16_16);
		location_group.Add(group17_17);
		location_group.Add(group18_18);
		location_group.Add(group25_19);
		location_group.Add(group20_20);
		location_group.Add(group131_21);
		location_group.Add(group22_22);
		location_group.Add(group542_23);
		location_group.Add(group805_24);
		for(int i = 0; i < location_group.Count; i++)
		{
			for(int j = 0; j < location_group[i].Length; j++)
			{
				code_location.Add(location_group[i][j], i);
			}
		}
	}
	
	public int ModalIndex(string aim_code)
	{
		if(code_location.ContainsKey(aim_code))
			return code_location[aim_code];
		else
			return -1;
	}
	
	public bool SetModalCode(string aim_code, int index)
	{
		if(index > 23 || index < 0)
			return false;
		else
		{
			_modal_code[index] = aim_code;
			return true;
		}
	}	
	
	public int UnitCheck()
	{
		if(Modal_Code[4] == "G21")
			return (int)CheckInformation.MetricSystem;
		else
			return (int)CheckInformation.BritishSystem;
	}
	
	public int PlaneCheck()
	{
		if(Modal_Code[3] == "G17")
			return (int)CheckInformation.XYPlane;
		else if(Modal_Code[3] == "G18")
			return (int)CheckInformation.ZXPlane;
		else
			return (int)CheckInformation.YZPlane;
	}
	
	public int RadiusCheck()
	{
		if(Modal_Code[7] == "G40")
			return (int)CheckInformation.RadiusCancel;
		else if(Modal_Code[7] == "G41")
			return (int)CheckInformation.RadiusLeft;
		else
			return (int)CheckInformation.RadiusRight;
	}
	
	public int LengthCheck()
	{
		if(Modal_Code[10] == "G49")
			return (int)CheckInformation.LengthCancel;
		else if(Modal_Code[10] == "G43")
			return (int)CheckInformation.LengthPositive;
		else
			return (int)CheckInformation.LengthNegative;
	}
	
	public int ScalingCheck()
	{
		if(Modal_Code[8] == "G50")
			return (int)CheckInformation.ScalingCancel;
		else
			return (int)CheckInformation.Scaling;
	}
	
	public int FixedCycleCheck()
	{
		if(Modal_Code[2] == "G80")
			return (int)CheckInformation.FixedCycelCancel;
		else
			 return -1;
	}
	
	public int AbsoluteCooCheck()
	{
		if(Modal_Code[6] == "G90")
			return (int)CheckInformation.AbsouteCoo;
		else
			return (int)CheckInformation.IncrementalCoo;
	}
	
	public Vector3 LocalCoordinate()
	{
		if(Modal_Code[13] == "G54.1")
			return Vector3.zero;
		else if(Modal_Code[13] == "G54")
			return LoadCoordinate.System("G54");
		else if(Modal_Code[13] == "G55")
			return LoadCoordinate.System("G55");
		else if(Modal_Code[13] == "G56")
			return LoadCoordinate.System("G56");
		else if(Modal_Code[13] == "G57")
			return LoadCoordinate.System("G57");
		else if(Modal_Code[13] == "G58")
			return LoadCoordinate.System("G58");
		else
			return LoadCoordinate.System("G59");
	}
}

/// <summary>
/// 静态加载工件坐标系
/// </summary>
public class LoadCoordinate
{
	public static Vector3  System(string coo_name)
	{
		Vector3 aim_vec = new Vector3(0, 0, 0);
		string filepath = Application.dataPath + SystemArguments.CoordinatePath + coo_name + ".txt";
		if(File.Exists(filepath))
		{
			FileStream coo_filestream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
			StreamReader coo_streamreader = new StreamReader(coo_filestream);
			string line_str = coo_streamreader.ReadLine();
			if(line_str != null)
			{
				string[] coo_strarray = line_str.Split(',');
				try
				{
					aim_vec.x = (float)Convert.ToDouble(coo_strarray[0]);
					aim_vec.y = (float)Convert.ToDouble(coo_strarray[1]);
					aim_vec.z = (float)Convert.ToDouble(coo_strarray[2]);
				}
				catch
				{
					Debug.LogError(coo_name + "工件坐标系文件格式不正确！位置: " + filepath);
					aim_vec = Vector3.zero;
				}
			}	
		}
		return aim_vec;
	}
}

public class MotionInfo
{
	public Vector3 DisplayStart;
	public Vector3 VirtualStart;
	public Vector3 DisplayTarget;
	public Vector3 VirtualTarget;
	public Vector3 Direction;
	public float Velocity;
	public float Rotate_Speed;
	public float Time_Value;
	public Vector3 Center_Point;
	public float Rotate_Degree;
	public int Motion_Type;
	public string Immediate_Motion;
	public int Slash;
	public int Tool_Number;
	public List<string> G_Display;
	public float[]  Remaining_Movement;
	
	public MotionInfo()
	{
		DisplayStart = new Vector3(0, 0, 0);
		VirtualStart = new Vector3(0, 0, 0);
		DisplayTarget = new Vector3(0, 0, 0);
		VirtualTarget = new Vector3(0, 0, 0);
		Direction = new Vector3(0, 0, 0);
		Velocity = 0;
		Rotate_Speed = 0;
		Time_Value = 0;
		Center_Point = new Vector3(0, 0, 0);
		Rotate_Degree = 0;
		Motion_Type = -1;
		Immediate_Motion = "";
		Slash = 0;
		Tool_Number = 0;
		G_Display = new List<string>();
		Remaining_Movement = new float[3]{0, 0, 0};
	}
	
	public void SetStartPosition(Vector3 display_pos, Vector3 virtual_pos)
	{
		DisplayStart = display_pos;
		VirtualStart = virtual_pos;
	}
	
	public void SetTargetPosition(Vector3 display_pos, Vector3 virtual_pos)
	{
		DisplayTarget = display_pos;
		VirtualTarget = virtual_pos;
	}
	
	public void SetCenterPoint(Vector3 vec)
	{
		Center_Point.x = vec.x;
		Center_Point.y = vec.y;
		Center_Point.z = vec.z;
	}
	
	public void G_Copy(List<string> str_list)
	{
		G_Display.Clear();
		string str_each = "";
		for(int i = 0; i < str_list.Count; i++)
		{
			str_each = str_list[i];
			G_Display.Add(str_each);
		}
	}
	
	public bool NotEmpty()
	{
		if(DisplayStart != Vector3.zero || DisplayTarget != Vector3.zero || VirtualStart != Vector3.zero || VirtualTarget != Vector3.zero || Direction != Vector3.zero)
			return true;
		else if(Velocity != 0 || Rotate_Speed != 0 || Time_Value != 0 || Rotate_Degree != 0 || Motion_Type != -1)
			return true;
		else if(Immediate_Motion != "" || Slash != 0 || Tool_Number != 0 || G_Display.Count != 0)
			return true;
		else
			return false;
	}
	
	public void SetRemainingMovement()
	{
		Remaining_Movement[0] = Mathf.Abs(Direction.x);
		Remaining_Movement[1] = Mathf.Abs(Direction.y);
		Remaining_Movement[2] = Mathf.Abs(Direction.z);
	}
}

