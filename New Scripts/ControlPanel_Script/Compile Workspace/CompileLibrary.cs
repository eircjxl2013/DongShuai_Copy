using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public enum ResultType {Success, CompileError, MacroError}
public enum MotionType {DryRunning, Line, Circular02, Circular03}
public enum ImmediateMotionType {ToolChanging = 'a', Pause, AutoReturnRP, RadiusCompensationCancel, RadiusCompensationLeft, RadiusCompensationRight, 
	LengthCompensationCancel, LengthCompensationPositive, LengthCompensationNegative, G54, G54_1, G55, G56, G57, G58, G59, M00, M01, M02, M03, M04, 
M05, M06, M07, M08, M09, M30, M98, M99}
public enum CheckInformation {MetricSystem, BritishSystem, XYPlane, YZPlane, ZXPlane, RadiusCancel, RadiusLeft, RadiusRight, LengthCancel, LengthPositive, 
LengthNegative, Scaling, ScalingCancel, FixedCycelCancel, AbsouteCoo, IncrementalCoo, G54, G54_1, G55, G56, G57, G58, G59}

public interface IG_Code
{
	string ErrorMessage {get;}
	bool G_Check(string g_str, int row_index, ref DataStore step_compile_data, ref ModalCode_Fanuc_M modal_state);
}

public interface IM_Code
{
	string ErrorMessage {get;}
	bool M_Check(string m_str, int row_index, ref DataStore step_compile_data);
}

class G_FANUC_OI_M_T:IG_Code
{
	private string _errorMessage;
	public string ErrorMessage
	{
		get {return _errorMessage;}
	}
	private List<string> satisfactoryG_T = new List<string>() {"G0", "G00", "G1", "G01", "G2", "G02", "G3", "G03", "G4", "G04",  "G5.4", "G05.4", "G7.1", "G07.1", "G8", "G08",
		"G9", "G09", "G10", "G11", "G12.1", "G13.1", "G17", "G18", "G19", "G20", "G21", "G22", "G23", "G25", "G26", "G27", "G28", "G30", "G31", "G32", "G33", "G34", "G36", 
		"G37", "G39", "G40", "G41", "G42", "G50", "G50.3", "G50.2", "G51.2", "G50.4", "G50.5", "G50.6", "G51.4", "G51.5", "G51.6", "G52", "G53", "G54", "G55", "G56", "G57", 
		"G58", "G59", "G61", "G63", "G64", "G65", "G66", "G67", "G68", "G69", "G70", "G71", "G72", "G73", "G74", "G75", "G76", "G80", "G81", "G82", "G83", "G83.1", "G84", 
		"G84.2", "G85", "G87", "G88", "G89", "G90", "G92", "G94", "G91.1", "G96", "G97", "G96.1", "G96.2", "G96.3", "G96.4", "G98", "G99", };
	public G_FANUC_OI_M_T()
	{
		_errorMessage = "";
	}
	
	public bool G_Check(string g_str, int row_index, ref DataStore step_compile_data, ref ModalCode_Fanuc_M modal_state)
	{
		_errorMessage = "";
		if(satisfactoryG_T.IndexOf(g_str) == -1)
		{
			_errorMessage = "(Line:" + row_index + "): " + "系统中不存在该G指令: " + g_str;
			return false; 
		}
		else
			return true;
	}
}

class G_FANUC_OI_M_M:IG_Code
{
	private string _errorMessage;
	public string ErrorMessage
	{
		get {return _errorMessage;}
	}
	private List<string> satisfactoryG_M = new List<string>() {"G0", "G00", "G1", "G01", "G2", "G02", "G3", "G03", "G4", "G04", "G5.1", "G05.1",  "G5.4", "G05.4", "G7.1", "G07.1",
		"G9", "G09", "G10", "G11", "G15", "G16", "G17", "G18", "G19", "G20", "G21", "G22", "G23", "G27", "G28", "G29", "G30", "G31", "G33","G37", "G39", "G40", "G41", "G42",
		"G40.1", "G41.1", "G42.1", "G43", "G44", "G45", "G46", "G47", "G48", "G49", "G50", "G51", "G50.1", "G51.1", "G52", "G53", "G54", "G54.1", "G55", "G56", "G57", "G58", 
		"G59", "G60", "G61", "G62", "G63", "G64", "G65", "G66", "G67", "G68", "G69", "G73", "G74", "G75", "G76", "G77", "G78", "G79", "G80", "G80.4", "G81.4", "G81", "G82", 
		"G83", "G84", "G84.2", "G84.3", "G85", "G86", "G87", "G88", "G89", "G90", "G91", "G91.1", "G92", "G92.1", "G93", "G94", "G95", "G96", "G97", "G98", "G99", "G160", "161"};
	public G_FANUC_OI_M_M()
	{
		_errorMessage = "";
	}
	
	public bool G_Check(string g_str, int row_index, ref DataStore step_compile_data, ref ModalCode_Fanuc_M modal_state)
	{
		_errorMessage = "";
		if(satisfactoryG_M.IndexOf(g_str) == -1)
		{
			_errorMessage = "(Line:" + row_index + "): " + "系统中不存在该G指令: " + g_str;
			return false;
		}
		else
		{
			try
			{
				float g_value = (float)Convert.ToDouble(g_str.Trim('G'));
				if(g_value < 10f)
					g_str = "G0" + g_value.ToString();
			}
			catch
			{
				_errorMessage = "(Line:" + row_index + "): " + "系统中不存在该G指令: " + g_str;
				return false;
			}
			int modal_index = modal_state.ModalIndex(g_str);
			//立即执行G代码（非模态G代码）
			if(modal_index == -1)
			{
				switch(g_str)
				{
				case "G04":
					step_compile_data.ImmediateAdd((char)ImmediateMotionType.Pause);
					break;
				case "G28":
					step_compile_data.ImmediateAdd((char)ImmediateMotionType.AutoReturnRP);
					break;
				default:
					//Todo: 有很多未完成的功能
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持该G代码: " + g_str;
					return false;
				}
			}
			//模态代码
			else
			{//1 level
				switch(modal_index)
				{
				case 0:
					if(g_str == "G00")
						step_compile_data.motion_type = (int)MotionType.DryRunning;
					else if(g_str == "G01")
						step_compile_data.motion_type = (int)MotionType.Line;
					else if(g_str == "G02")
						step_compile_data.motion_type = (int)MotionType.Circular02;
					else
						step_compile_data.motion_type = (int)MotionType.Circular03;
					break;
				case 1:
					if(g_str != "G94")
					{
						_errorMessage = "(Line:" + row_index + "): " + "目前系统只支持G94(每分钟进给)，暂不支持" + g_str;
						return false;
					}
					break;
				case 2:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持09组G代码";
					return false;
				case 3:
					if(g_str != "G17")
					{
						_errorMessage = "(Line:" + row_index + "): " + "目前系统只支持G17(XY平面选择)，暂不支持" + g_str;
						return false;
					}
					break;
				case 4:
					if(g_str != "G21")
					{
						_errorMessage = "(Line:" + row_index + "): " + "目前系统只支持G21(公制输入)，暂不支持" + g_str;
						return false;
					}
					break;
				case 5:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持10组固定循环G代码";
					return false;
				case 6:
					//G90、G91 绝对、增量
					//Todo:
					break;
				case 7:
					//G40, G41, G42 刀具半径补偿
					//Todo: 坐标会发生一些变化
					if(g_str == "G40")
						step_compile_data.ImmediateAdd((char)ImmediateMotionType.RadiusCompensationCancel);
					else if(g_str == "G41")
						step_compile_data.ImmediateAdd((char)ImmediateMotionType.RadiusCompensationLeft);
					else
						step_compile_data.ImmediateAdd((char)ImmediateMotionType.RadiusCompensationRight);
					break;
				case 8:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持11组比例缩放G代码";
					return false;
				case 9:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持04组存储形成检测功能G代码";
					return false;
				case 10:
					//G43, G44, G49 刀具长度补偿
					//Todo: 坐标会发生一些变化
					if(g_str == "G43")
						step_compile_data.ImmediateAdd((char)ImmediateMotionType.LengthCompensationPositive);
					else if(g_str == "G44")
						step_compile_data.ImmediateAdd((char)ImmediateMotionType.LengthCompensationNegative);
					else
						step_compile_data.ImmediateAdd((char)ImmediateMotionType.LengthCompensationCancel);
					break;
				case 11:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持12组宏模态调用G代码";
					return false;
				case 12:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持13组周速恒定控制G代码";
					return false;
				case 13:
					//G54, G54.1, G55, G56, G57, G58, G59
					//Todo: 坐标会发生一些变化
					if(g_str == "G54.1")
					{
						_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持G54.1追加工件坐标系功能";
						return false;
					}
					else if(g_str == "G54")
						step_compile_data.ImmediateAdd((char)ImmediateMotionType.G54);
					else if(g_str == "G55")
						step_compile_data.ImmediateAdd((char)ImmediateMotionType.G55);
					else if(g_str == "G56")
						step_compile_data.ImmediateAdd((char)ImmediateMotionType.G56);
					else if(g_str == "G57")
						step_compile_data.ImmediateAdd((char)ImmediateMotionType.G57);
					else if(g_str == "G58")
						step_compile_data.ImmediateAdd((char)ImmediateMotionType.G58);
					else
						step_compile_data.ImmediateAdd((char)ImmediateMotionType.G59);
					break;
				case 14:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持15组G代码";
					return false;
				case 15:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持16组坐标旋转方式G代码";
					return false;
				case 16:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持17组极坐标指令G代码";
					return false;
				case 17:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持18组法线方向控制G代码";
					return false;
				case 18:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持G25代码";
					return false;
				case 19:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持20组横向进磨控制（磨床用）G代码";
					return false;
				case 20:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持G13.1代码";
					return false;
				case 21:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持22组可编程镜像G代码";
					return false;
				case 22:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持G54.2代码";
					return false;
				case 23:
					_errorMessage = "(Line:" + row_index + "): " + "目前系统暂不支持G80.5代码";
					return false;
				default:
					break;
				}
				//模态代码改变
				if(modal_state.Modal_Code[modal_index] != g_str)
				{
					step_compile_data.modal_index.Add(modal_index);
					step_compile_data.modal_string.Add(g_str);
					modal_state.SetModalCode(g_str, modal_index);
				}
				step_compile_data.G_code.Add(g_str);
			}//1 level
			
			return true;
		}
	}
}

class M_FANUC_OI_M:IM_Code
{
	private string _errorMessage;
	public string ErrorMessage
	{
		get {return _errorMessage;}
	}
	
	public M_FANUC_OI_M()
	{
		_errorMessage = "";
	}
	
	private List<string> satisfactoryM = new List<string>() {"M0", "M00", "M1", "M01", "M2", "M02", "M3", "M03", "M4", "M04", 
		"M5", "M05", "M6", "M06", "M7", "M07", "M8", "M08", "M9", "M09", "M19", "M30", "M98", "M98"};
	public bool M_Check(string m_str, int row_index, ref DataStore step_compile_data)
	{
		_errorMessage = "";
		if(satisfactoryM.IndexOf(m_str) == -1)
		{
			_errorMessage = "(Line:" + row_index + "): " + "系统中不存在该M指令: " + m_str;
			return false;
		}
		else
		{
			try
			{
				float m_value = (float)Convert.ToDouble(m_str.Trim('M'));
				if(m_value < 10f)
					m_str = "M0" + m_value.ToString();
			}
			catch
			{
				_errorMessage = "(Line:" + row_index + "): " + "系统中不存在该M指令: " + m_str;
				return false;
			}
			switch(m_str)
			{
			case "M00":
				step_compile_data.ImmediateAdd((char)ImmediateMotionType.M00);
				break;
			case "M01":
				step_compile_data.ImmediateAdd((char)ImmediateMotionType.M01);
				break;
			case "M02":
				step_compile_data.ImmediateAdd((char)ImmediateMotionType.M02);
				break;
			case "M03":
				step_compile_data.ImmediateAdd((char)ImmediateMotionType.M03);
				break;
			case "M04":
				step_compile_data.ImmediateAdd((char)ImmediateMotionType.M04);
				break;
			case "M05":
				step_compile_data.ImmediateAdd((char)ImmediateMotionType.M05);
				break;
			case "M06":
				step_compile_data.ImmediateAdd((char)ImmediateMotionType.M06);
				break;
			case "M08":
				step_compile_data.ImmediateAdd((char)ImmediateMotionType.M08);
				break;
			case "M09":
				step_compile_data.ImmediateAdd((char)ImmediateMotionType.M09);
				break;
			case "M30":
				step_compile_data.ImmediateAdd((char)ImmediateMotionType.M30);
				break;
			case "M99":
				step_compile_data.ImmediateAdd((char)ImmediateMotionType.M99);
				break;
			default:
				_errorMessage = "(Line:" + row_index + "): " + "系统暂不支持该M指令: " + m_str;
				return false;	
			}
			return true;
		}	
	}
}

/// <summary>
/// Lexical check class
/// </summary>
public abstract class LexicalCheck
{
	//接口，实现多态，以达成不同系统间的扩展；
	protected IG_Code g_code_check;
	protected IM_Code m_code_check;
	
	//Todo：模态信息保存的方式以后要更改，不能满足不同系统扩展的需求
	protected ModalCode_Fanuc_M ModalState;
	/*
	private enum FuncSelect {G_Check, M_Check, F_Check, I_Check, Slash_Check}
	private Dictionary<int, Func<string, int, bool>> CheckFunc;
	*/
	private string _errorMessage;
	protected string ErrorMessage
	{
		get {return _errorMessage;}
	}
	private List<string> _compileInfo;
	/// <summary>
	/// 错误信息汇集属性.
	/// </summary>
	/// <value>
	/// 错误信息List
	/// </value>
	public List<string> CompileInfo
	{
		get {return _compileInfo;}
	}
	public LexicalCheck()
	{
		_errorMessage = "";
		_compileInfo = new List<string>();
		ModalState = new ModalCode_Fanuc_M();
		/*
		CheckFunc = new Dictionary<int, Func<string, int, bool>>();
		CheckFunc.Add((int)FuncSelect.G_Check, G_Check);
		CheckFunc.Add((int)FuncSelect.M_Check, M_Check);
		CheckFunc.Add((int)FuncSelect.F_Check, F_Check);
		CheckFunc.Add((int)FuncSelect.I_Check, I_Check);
		CheckFunc.Add((int)FuncSelect.Slash_Check, Slash_Check);
		*/
	}
	/// <summary>
	/// 清空错误信息
	/// </summary>
	protected void ErrorClear()
	{
		_compileInfo.Clear();
	}
	
	/// <summary>
	/// 克隆当前系统中的模态信息
	/// </summary>
	/// <param name='current_modal'>
	/// 当前系统中的模态信息
	/// </param>
	public void ModalClone(ModalCode_Fanuc_M current_modal)
	{
		for(int i = 0; i < current_modal.Modal_Code.Length; i++)
		{
			ModalState.Modal_Code[i] = current_modal.Modal_Code[i];
		}
	}
	
	private bool G_Check(string code_str, int row_index, ref DataStore step_compile_data, ref ModalCode_Fanuc_M modal_state)
	{
		_errorMessage = "";
		bool temp_flag = this.g_code_check.G_Check(code_str, row_index, ref step_compile_data, ref modal_state);
		_errorMessage = this.g_code_check.ErrorMessage;
		return temp_flag;
	}

	private bool M_Check(string code_str, int row_index, ref DataStore step_compile_data)
	{
		_errorMessage = "";
		bool temp_flag = this.m_code_check.M_Check(code_str, row_index, ref step_compile_data);
		_errorMessage = this.m_code_check.ErrorMessage;
		return temp_flag;
	}
	
	/// <summary>
	/// A, B, C, I, J, K, U, V, W, X, Y, Z, R;  F
	/// </summary>
	private bool F_Check(string code_str, int row_index, ref  DataStore step_compile_data)
	{
		_errorMessage = "";
		string address_value = code_str[0].ToString().ToUpper();
		code_str = code_str.Remove(0, 1);
		float str_value = 0;
		try
		{
			str_value = (float)Convert.ToDouble(code_str);	
		}
		catch
		{
			_errorMessage = "(Line:" + row_index + "): " + address_value + "地址后值的格式错误";
			return false;
		}
		if(address_value == "F")
		{
			if(str_value < 0)
			{
				_errorMessage = "(Line:" + row_index + "): " + address_value + "地址后值不能为负数";
				return false;
			}
			step_compile_data.f_value = str_value;
			ModalState.Feedrate = str_value;
		}
		else
		{
			switch(address_value)
			{
			case "I":
				step_compile_data.i_value = str_value;
				step_compile_data.ijk_state[0] = true;
				break;
			case "J":
				step_compile_data.j_value = str_value;
				step_compile_data.ijk_state[1] = true;
				break;
			case "K":
				step_compile_data.k_value = str_value;
				step_compile_data.ijk_state[2] = true;
				break;
			case "X":
				step_compile_data.x_value = str_value;
				step_compile_data.xyz_state[0] = true;
				break;
			case "Y":
				step_compile_data.y_value = str_value;
				step_compile_data.xyz_state[1] = true;
				break;
			case "Z":
				step_compile_data.z_value = str_value;
				step_compile_data.xyz_state[2] = true;
				break;
			case "R":
				step_compile_data.r_value = str_value;
				step_compile_data.ijk_state[3] = true;
				break;
			default:
				_errorMessage = "(Line:" + row_index + "): " + "系统暂不支持该指令: " + code_str;
				return false;	
			}
		}
		return true;
	}
	
	/// <summary>
	/// D, H;  L, P;  N, Q;   O;  S;  T;  
	/// </summary>
	private bool I_Check(string code_str, int row_index, ref DataStore step_compile_data)
	{
		_errorMessage = "";
		string address_value = code_str[0].ToString().ToUpper();
		code_str = code_str.Remove(0, 1);
		int index_number = 0;
		try
		{
			index_number = Convert.ToInt32(code_str);
		}
		catch
		{
			_errorMessage = "(Line:" + row_index + "): " + address_value + "地址后值的格式错误";
			return false;
		}
		switch(address_value)
		{
		case "D":
		case "H":
			if(index_number < 0 || index_number > 400)
			{
				_errorMessage = "(Line:" + row_index + "): " + address_value + "地址后值为" + index_number + ", 超出规定范围(0~400)";
				return false;
			}
			//Todo: 刀具补偿
			break;
		case "L":
		case "P":
			if(index_number <= 0 || index_number > 99999999)
			{
				_errorMessage = "(Line:" + row_index + "): " + address_value + "地址后值为" + index_number + ", 超出规定范围(1~99999999)";
				return false;
			}
			//Todo: 程序循环相关
			_errorMessage = "(Line:" + row_index + "): " + "系统暂不支持循环操作";
			return false;
		case "N":
		case "Q":
			if(index_number <= 0 || index_number > 99999)
			{
				_errorMessage = "(Line:" + row_index + "): " + address_value + "地址后值为" + index_number + ", 超出规定范围(1~99999)";
				return false;
			}
			//Todo: 程序跳转相关
			break;
		case "O":
			if(index_number <= 0 || index_number > 9999)
			{
				_errorMessage = "(Line:" + row_index + "): " + address_value + "地址后值为" + index_number + ", 超出规定范围(1~9999)";
				return false;
			}
			break;
		case "S":
			if(index_number < 0 || index_number > 99999)
			{
				_errorMessage = "(Line:" + row_index + "): " + address_value + "地址后值为" + index_number + ", 超出规定范围(0~99999)";
				return false;
			}
			step_compile_data.s_value = (float)index_number;
			break;
		case "T":
			if(index_number < 0 || index_number > 99999999)
			{
				_errorMessage = "(Line:" + row_index + "): " + address_value + "地址后值为" + index_number + ", 超出规定范围(0~99999999)";
				return false;
			}
			step_compile_data.tool_number = index_number;
			break;
		default:
			break;
		}
		return true;
	}
	
	/// <summary>
	/// check
	/// </summary>
	private bool Slash_Check(string code_str, int row_index, ref  DataStore step_compile_data)
	{
		_errorMessage = "";
		string address_value = code_str[0].ToString().ToUpper();
		code_str = code_str.Remove(0, 1);
		int num = 1;
		try
		{
			if(code_str == "")
				num = 1;
			else
				num = Convert.ToInt32(code_str);
		}
		catch
		{
			_errorMessage = "(Line:" + row_index + "): " + address_value + "地址后值的格式错误";
			return false;
		}
		if(num <= 0 || num > 9)
		{
			_errorMessage = "(Line:" + row_index + "): " + address_value + "地址后值为" + num + ", 超出规定范围(1~9)";
			return false;
		}
		step_compile_data.slash_value = num;
		return true;
	}
	
	/// <summary>
	/// Code Check Entrance
	/// </summary>
	protected bool Code_Check(List<string> code_segment, int row_index, ref bool macro_flag, ref DataStore step_compile_data, ref Vector3 display_position, ref MotionInfo step_motion_data, ref Vector3 virtual_position)
	{
		bool temp_flag = true;
		bool return_flag = true;
		string Address = "";
		Vector3 program_position = new Vector3(0, 0, 0);
		macro_flag = false;
		Regex macro_Reg = new Regex(@"((#+)|(\[+)|(\]+)|(=+))", RegexOptions.IgnoreCase);
		MatchCollection macro_Col;
		for(int i = 0; i < code_segment.Count; i++)
		{
			// 检查是否有宏代码，如果有宏代码
			macro_Col = macro_Reg.Matches(code_segment[i]);
			//如果程序中含有宏代码，中断编译过程，返回宏代码错误，宏代码的编译暂不处理
			if(macro_Col.Count > 0)
			{
				temp_flag = false;
				macro_flag = true;
				break;
			}
			if(code_segment[i] != ";")
			{
				_errorMessage = "";
				Address = code_segment[i][0].ToString().ToUpper();
				switch(Address)
				{
				case "G":
					return_flag = G_Check(code_segment[i], row_index, ref step_compile_data, ref ModalState);
					break;
				case "M":
					return_flag = M_Check(code_segment[i], row_index, ref step_compile_data);
					break;
				/// A, B, C, I, J, K, U, V, W, X, Y, Z, R;  F
				case "A":
				case "B":
				case "C":
				case "I":
				case "J":
				case "K":
				case "U":
				case "V":
				case "W":
				case "X":
				case "Y":
				case "Z":
				case "R":
				case "F":
					return_flag = F_Check(code_segment[i], row_index, ref step_compile_data);
					break;
				case "/":
					return_flag = Slash_Check(code_segment[i], row_index, ref step_compile_data);
					break;
				/// D, H;  L, P;  N, Q;   O;  S;  T;  
				case "D":
				case "H":
				case "L":
				case "P":
				case "N":
				case "Q":
				case "O":
				case "S":
				case "T":
					return_flag = I_Check(code_segment[i], row_index, ref step_compile_data);
					break;
				default:
					_errorMessage = "(Line:" + row_index + "): " + Address + "地址不存在";
					return_flag = false;
					break;
				}
				
				if(return_flag == false)
				{
					if(temp_flag)
					{
						temp_flag = false;
					}
					_compileInfo.Add(ErrorMessage);
				}
			}
		}
		if(temp_flag)
		{//compile level
			//Todo: 分析此段代码，生成相关运动信息；
			//是否为空
			if(step_compile_data.IsEmpty())
				return true;
			else
			{//1 level
				//立即执行代码处理
				if(step_compile_data.immediate_execution != "")
				{//2 level
					step_motion_data.Immediate_Motion = step_compile_data.immediate_execution;
					for(int i = 0; i < step_compile_data.immediate_execution.Length; i++)
					{
						switch((char)step_compile_data.immediate_execution[i])
						{
							//Todo: 考虑下当前行只有T代码或者只有M06的情况
						case (char)ImmediateMotionType.ToolChanging:
							step_motion_data.Tool_Number = step_compile_data.tool_number;
							break;
						case (char)ImmediateMotionType.M03:
						case (char)ImmediateMotionType.M04:
							if(step_compile_data.s_value == 0)
							{
								_errorMessage = "(Line:" + row_index + "): " + "当前程序段中未指定主轴转速";
								_compileInfo.Add(ErrorMessage);
								return false;
							}
							step_motion_data.Rotate_Speed = step_compile_data.s_value;
							break;
						case (char)ImmediateMotionType.G54:
						case (char)ImmediateMotionType.G55:
						case (char)ImmediateMotionType.G56:
						case (char)ImmediateMotionType.G57:
						case (char)ImmediateMotionType.G58:
						case (char)ImmediateMotionType.G59:
							display_position = virtual_position - ModalState.LocalCoordinate();
							break;
						default:
							break;
						}
					}
				}//2 level
				
				//计算运动信息
				if(step_compile_data.HasMotion())
				{//3 level
					step_motion_data.Motion_Type = step_compile_data.MotionTypeIndex(ModalState.Modal_Code[0]);
					if(step_motion_data.Motion_Type == -1)
					{
						_errorMessage = "(Line:" + row_index + "): " + "未知运动方式错误！";
						_compileInfo.Add(ErrorMessage);
						return false;
					}
					switch(step_motion_data.Motion_Type)
					{
					case (int)MotionType.DryRunning:
						step_motion_data.SetStartPosition(display_position, virtual_position);
						//公制单位
						if(ModalState.UnitCheck() == (int)CheckInformation.MetricSystem)
						{
							//绝对坐标
							if(ModalState.AbsoluteCooCheck() == (int)CheckInformation.AbsouteCoo)
							{
								program_position = step_compile_data.AbsolutePosition(display_position);
								step_motion_data.Direction = program_position - display_position;
								step_motion_data.SetRemainingMovement();
								step_motion_data.Velocity = SystemArguments.RapidMoveSpeed;
								step_motion_data.Time_Value = step_motion_data.Direction.magnitude / SystemArguments.RapidMoveSpeed * 60;
								virtual_position += step_motion_data.Direction;
								step_motion_data.SetTargetPosition(program_position, virtual_position);
							}
							//增量坐标
							else
							{
								
							}
						}
						//英制单位
						else
						{
							
						}
						break;
					case (int)MotionType.Line:
						if(ModalState.Feedrate == 0)
						{
							_errorMessage = "(Line:" + row_index + "): " + "未指定进给速率！";
							_compileInfo.Add(ErrorMessage);
							return false;
						}
						step_motion_data.SetStartPosition(display_position, virtual_position);
						//公制单位
						if(ModalState.UnitCheck() == (int)CheckInformation.MetricSystem)
						{
							//绝对坐标
							if(ModalState.AbsoluteCooCheck() == (int)CheckInformation.AbsouteCoo)
							{
								program_position = step_compile_data.AbsolutePosition(display_position);
								step_motion_data.Direction = program_position - display_position;
								step_motion_data.SetRemainingMovement();
								step_motion_data.Velocity = ModalState.Feedrate;
								step_motion_data.Time_Value = step_motion_data.Direction.magnitude / step_motion_data.Velocity * 60;
								virtual_position += step_motion_data.Direction;
								step_motion_data.SetTargetPosition(program_position, virtual_position);
							}
							//增量坐标
							else
							{
								
							}
						}
						//英制单位
						else
						{
							
						}
						break;
					case (int)MotionType.Circular02:
						if(ModalState.Feedrate == 0)
						{
							_errorMessage = "(Line:" + row_index + "): " + "未指定进给速率！";
							_compileInfo.Add(ErrorMessage);
							return false;
						}
						step_motion_data.SetStartPosition(display_position, virtual_position);
						//公制单位
						if(ModalState.UnitCheck() == (int)CheckInformation.MetricSystem)
						{
							//绝对坐标
							Vector3 xyz_coo = new Vector3(0, 0, 0);
							Vector3 ijk_coo = new Vector3(0, 0, 0);
							float rValue = 0;
							if(ModalState.AbsoluteCooCheck() == (int)CheckInformation.AbsouteCoo)
							{
								switch(step_compile_data.CircleArguJudge(ref _errorMessage, ModalState, ref ijk_coo, ref xyz_coo, ref rValue))
								{
								case -1:
									_errorMessage = "(Line:" + row_index + "): " + _errorMessage;
									_compileInfo.Add(ErrorMessage);
									return false;
								case 0:
									step_motion_data.Motion_Type = -1;
									return true;
								case 1:
									step_motion_data.Center_Point = display_position + ijk_coo;
									step_motion_data.Rotate_Degree = 360f;
									
									break;
								}
								
								program_position = step_compile_data.AbsolutePosition(display_position);
								step_motion_data.Direction = program_position - display_position;
								step_motion_data.SetRemainingMovement();
								step_motion_data.Velocity = ModalState.Feedrate;
								step_motion_data.Time_Value = step_motion_data.Direction.magnitude / step_motion_data.Velocity * 60;
								virtual_position += step_motion_data.Direction;
								step_motion_data.SetTargetPosition(program_position, virtual_position);
							}
							//增量坐标
							else
							{
								
							}
						}
						//英制单位
						else
						{
							
						}
						break;
					case (int)MotionType.Circular03:
						
						break;
					default:
						_errorMessage = "(Line:" + row_index + "): " + "当前系统暂不支持Motion Type: " + step_compile_data.motion_type;
						_compileInfo.Add(ErrorMessage);
						return false;
					}
				}//3 level
				
			}//1 level
			
			
			if(step_compile_data.slash_value > 0)
			{
				if(ModalState.Slash)
					return true;
			}
			
				
			
			
			return true;
		}//compile level
		else
			return false;
	}
	
	
}

public abstract class CompileBase:LexicalCheck
{
	private bool _executeFlag;
	//编译后是否可以MEM执行判断属性；
	public bool ExecuteFlag
	{
		get {return _executeFlag;}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CompileBase"/> class.
	/// </summary>
	public CompileBase()
	{
		_executeFlag = false;
		
	}
	
	/// <summary>
	/// The main method of NC code compile
	/// </summary>
	/// <returns>
	/// compile result type
	/// </returns>
	/// <param name='source_code'>
	/// formative NC code
	/// </param>
	/// <param name='prog_name'>
	/// the name of current NC program
	/// </param>
	public int CompileEntrance(List<List<string>> source_code, string prog_name, ref List<DataStore> compile_data, Vector3 current_position, ref List<MotionInfo> motion_data, Vector3 virtual_position)
	{
		//初始信息
		bool temp_execute = true;
		bool macro_flag = false;
		ResultType type_var = ResultType.Success;
		_executeFlag = false;
		ErrorClear();
		compile_data.Clear();
		motion_data.Clear();
		DataStore step_compile_data = new DataStore();
		MotionInfo step_motion_data = new MotionInfo();

		for(int i = 0; i < source_code.Count; i++)
		{
			step_compile_data = new DataStore();
			step_motion_data = new MotionInfo();
			if(Code_Check(source_code[i], i + 1, ref macro_flag, ref step_compile_data, ref current_position, ref step_motion_data, ref virtual_position) == false)
			{
				if(macro_flag)
				{
					temp_execute = false;
					type_var = ResultType.MacroError;
					compile_data.Clear();
					break;
				}
				if(temp_execute)
				{
					temp_execute = false;
					type_var = ResultType.CompileError;
				}
			}
			if(temp_execute)
			{
				compile_data.Add(step_compile_data);
				motion_data.Add(step_motion_data);
			}
		}
		if(temp_execute)
			_executeFlag = true;
		else
		{
			_executeFlag = false;
			compile_data.Clear();
			motion_data.Clear();
		}
		int return_type = (int)type_var;
		return return_type;
	}
}

public class FANUC_OI_M:CompileBase
{
	public FANUC_OI_M()
	{
		this.g_code_check = new G_FANUC_OI_M_M();
		this.m_code_check = new M_FANUC_OI_M();
	}
}



