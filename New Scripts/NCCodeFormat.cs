/*
 * 在原来的程序中，加载NC代码文本的那个模块很乱，后面我会进行相应的修改，
 * 本程序中已完成了一部分相应的功能，可以作为参考，即到时候我会改成这样的接口给你们；
 * 
 * 对代码格式化的函数在此脚本中我已经进行了相应的完善，初步可用；
 * 
 * 如果有哪部分不明白，可以QQ告诉我；
 * */

using UnityEngine;
using System;
using System.Collections;
//For List
using System.Collections.Generic;
//For Regex --- Regular Expression:正则表达式
using System.Text.RegularExpressions;
using System.IO;

public class NCCodeFormat : MonoBehaviour {
	
	int codenum = 0;
	string test_str = "";
	string text_field = "O10";
	List<string> codestr = new List<string>();
	List<List<string>> all_code_test = new List<List<string>>();
		
	//文件路径
	string document_path = "";

	void Awake () 
	{
		document_path = Application.dataPath + "/Resources/Gcode/";
	}
	
	// Use this for initialization
	void Start () {	
		//codestr = CodeFormat("G10G30s768G12f199z723i+23j-234");
		//codestr = CodeFormat("N30#[#10]=#4*COS[45*#[#23]]-#5*SIN[45]");
		//codestr = CodeFormat("N38IF[#1LT370]GOTO26");
		//codestr = CodeFormat("234hahaha");
		/*
		codestr = CodeFormat("#[234hahaha]g[sdafd]dsf");
		codenum = codestr.Count;
		Debug.Log(codenum);
		if(codenum > 0)
		{
			foreach(string str in codestr)
			{
				Debug.Log(str);
			}
		}
		*/
		/*
		Debug.Log(test_str.IndexOfAny("b".ToCharArray()));
		
		//CodeLoad("O");
		codestr = CodeLoad("O200");
		//CodeLoad("O12");
		//CodeLoad("O123");
		//CodeLoad("O1234");
		//CodeLoad("O12343");
		codenum = codestr.Count;
		Debug.Log(codenum);
		if(codenum > 0)
		{
			foreach(string str in codestr)
			{
				Debug.Log(str);
			}
		}
		*/	
	}
	
	void OnGUI ()
	{
		//To display
		test_str = GUI.TextArea(new Rect(10, 10,300, 500), test_str);
		
		GUI.Label(new Rect(10, 520, 100, 20), "请输入程序号：");
		
		text_field = GUI.TextField(new Rect(115, 520, 195, 20), text_field);
		
		//运行时，可重新输入不同的NC程序名字，反复启动
		if(GUI.Button(new Rect(10, 550, 300, 30), "启动"))
		{
			test_str = "";
			TestFunction(text_field);
		}
	}
	
	/// <summary>
	/// 测试用的函数，通过GUI中的“启动”按钮启动
	/// </summary>
	/// <param name='file_name'>
	/// NC 程序的程序名，O0001可以简写为O1
	/// </param>
	void TestFunction (string file_name)
	{
		all_code_test = AllCode(file_name);
		codenum = all_code_test.Count;
		if(codenum > 0)
		{
			string temp_str = "";
			for(int i = 0; i < codenum; i++)
			{
				temp_str = "";
				for(int j = 0; j < all_code_test[i].Count; j++)
				{
					temp_str = temp_str + " " + all_code_test[i][j];
				}
				temp_str = temp_str.TrimStart().TrimEnd();
				if(i == 0)
					test_str = temp_str ;
				else
					test_str = test_str + "\n" + temp_str ;
			}
		}
	}
	
	
	/// <summary>
	/// 获取一个完整NC文件的的NC代码，并进行格式化
	/// --1--先通过调用CodeLoad函数获取相应路径下的NC文件中的内容
	/// --2--将每条代码都进行格式化
	/// </summary>
	/// <returns>
	/// 格式化后的代码，类型为：List<List<string>>
	/// </returns>
	/// <param name='file_name'>
	/// NC 程序的程序名，O0010可以简写为O10
	/// </param>
	public List<List<string>> AllCode (string file_name) 
	{
		//Initialize the return value. 
		List<List<string>> all_code_list = new List<List<string>>();
		//Temporal code data. 
		List<string> all_code_temp = CodeLoad(file_name);
		//Temporal code segment data. 
		List<string> temp_str_list = new List<string>();
		//Format code segment.
		if(all_code_temp.Count > 0)
		{
			foreach(string code_str in all_code_temp)
			{
				temp_str_list = new List<string>();
				temp_str_list = CodeFormat(code_str);
				if(temp_str_list.Count > 0)
					all_code_list.Add(temp_str_list);
			}
		}
		return all_code_list;
	}
	
	/// <summary>
	/// 对输入的每一NC程序段进行格式化.
	/// 如果输入的程序段只有字符“%”，则此函数返回一个空的List<string>类型，code_segment.cout = 0
	/// 因为程序开头可能包含“%”，在编辑界面的时候该符号不显示，所以在初次导入NC代码的时候，如果该行代码code_segment.cout = 0，应直接去除
	/// 此外，初次导入时，如果NC代码中间夹杂一空行，同样处理
	/// </summary>
	/// <returns>
	/// 返回List<string>类型的格式化后的NC代码.
	/// </returns>
	/// <param name='sourceCode'>
	/// 传入的NC程序段,类型为string.
	/// </param>
	public List<string> CodeFormat(string sourceCode)
	{
		//Initialize code segment data struct
		List<string> code_segment = new List<string>();
		//Remove the "space" , "%" and ";" in the source NC code and make each letter uppercase letters
		sourceCode = sourceCode.ToUpper().Trim().Trim(';', '；', '%');
		//Remove the annotation of NC code like: (blah blah blah)
		/*
		 * 正则表达式字符串匹配
		 * 解决“()”匹配问题，可识别嵌套的(***(***))
		 * 涉及到了平衡组，用堆栈的思想进行判断
		 * 详情请见以下网址
		 * http://deerchao.net/tutorials/regex/regex.htm
		 * 注意： 是否需要考虑只有"("或者“)”的情况？这种情况下是否要报错？载入代码时可以不用考虑，编译代码时再考虑。
		*/
		Regex annotation_Reg = new Regex(@"\([^\(\)]*(((?'Open'\()[^\(\)]*)+((?'-Open'\))[^\(\)]*)+)*(?(Open)(?!))\)");
		MatchCollection annotation_Col = annotation_Reg.Matches(sourceCode);
		if(annotation_Col.Count > 0)
		{
			for(int i = 0; i < annotation_Col.Count; i++)
			{
				sourceCode = sourceCode.Replace(annotation_Col[i].Value.ToString(), "");
			}	
		}
		//If sourceCode is not null, format the source NC code then. Otherwise, return an empty List<string>() type.
		if(sourceCode != "")
		{//1 level
			//To judge whether it is macroprogram or not.
			Regex macro_Reg = new Regex(@"((#+)|(\[+)|(\]+)|(=+)|(/))", RegexOptions.IgnoreCase);
			MatchCollection macro_Col = macro_Reg.Matches(sourceCode);
			if(macro_Col.Count > 0)
			{//2 level
				// It contains macroprogram
				// 考虑有宏程序
				//暂定以下6种匹配方式
				//--1-- G40&&X-37.1 == ([A-Z]+[^A-Z^\s^#^=^\[^\]]+) 
				//--2-- G#40&&GOTO#1 == ([A-Z]+#\d+)
				//--3-- G[#[#40]] == ([A-Z]+\[[^\[\]]*(((?'Open'\[)[^\[\]]*)+((?'-Open'\])[^\[\]]*)+)*(?(Open)(?!))\])
				//--4-- #40 == (#\d+)
				//--5-- #[#40] == (#\[[^\[\]]*(((?'Open'\[)[^\[\]]*)+((?'-Open'\])[^\[\]]*)+)*(?(Open)(?!))\])
				//--6-- =\S+$ == (=\S+$) 即等号后面都当做一个字符数来处理
				string pattern_macro = @"([A-Z]+[^A-Z^\s^#^=^\[^\]]+)|([A-Z]+#\d+)|([A-Z]+\[[^\[\]]*(((?'Open'\[)[^\[\]]*)+((?'-Open'\])[^\[\]]*)+)*(?(Open)(?!))\])|(#\d+)|(#\[[^\[\]]*(((?'Open'\[)[^\[\]]*)+((?'-Open'\])[^\[\]]*)+)*(?(Open)(?!))\])|(=\S+$)|(/\d?)";
				Regex macro_test_Reg = new Regex(pattern_macro);
				MatchCollection macro_test_Col = macro_test_Reg.Matches(sourceCode);
				if(macro_test_Col.Count > 0)
				{
					int str_index = 0;
					string temp_str = "";
					for(int i = 0; i < macro_test_Col.Count; i++)
					{
						temp_str = macro_test_Col[i].Value;
						code_segment.Add(temp_str);
						str_index = sourceCode.IndexOfAny(temp_str.ToCharArray());
						sourceCode = sourceCode.Remove(str_index, temp_str.Length);
						//Debug.Log(i + " -- Source Code: " + sourceCode + " --- Match String: " + macro_test_Col[i].Value);		
						//sourceCode = sourceCode.Replace(macro_test_Col[i].Value, "");
						//Debug.Log(sourceCode);
					}
					//If the code segment cotains some disgusting codes, deal with it as follows.
					if(sourceCode.Length != 0)
					{
						code_segment.Add(sourceCode);
					}
					code_segment.Add(";");
				}
				else
				{
					if(sourceCode.Length != 0)
					{
						code_segment.Add(sourceCode);
						code_segment.Add(";");
					}
					else
						Debug.LogError("正常的NC代码还有未考虑到的情况或者代码格式完全不对！ Error caused by Eric.");
				}
			}//2 level
			else
			{//3 level
				//It doesn't contain macroprogram
				Regex format_normal_Reg = new Regex(@"([A-Z]+[^A-Z^\s]+)+", RegexOptions.IgnoreCase);
				Match format_normal_Mat = format_normal_Reg.Match(sourceCode);
				if(format_normal_Mat.Groups.Count > 1)
				{
					int str_index = 0;
					string temp_str = "";
					for(int i = 0; i < format_normal_Mat.Groups[1].Captures.Count; i++)
					{
						temp_str = format_normal_Mat.Groups[1].Captures[i].Value;
						code_segment.Add(temp_str);
						str_index = sourceCode.IndexOfAny(temp_str.ToCharArray());
						sourceCode = sourceCode.Remove(str_index, temp_str.Length);
					}
					//If the code segment cotains some disgusting codes, deal with it as follows.
					if(sourceCode.Length != 0)
					{
						code_segment.Add(sourceCode);
					}
					code_segment.Add(";");
				}
				//If the code segment cotains some disgusting codes, deal with it as follows.
				else
				{
					if(sourceCode.Length != 0)
					{
						code_segment.Add(sourceCode);
						code_segment.Add(";");
					}
					else
						Debug.LogError("正常的NC代码还有未考虑到的情况或者代码格式完全不对！ Error caused by Eric.");
				}
			}//3 level
		}//1 level
		return code_segment;
	}
	
	/// <summary>
	/// 加载相应路径下的NC文件，并以List<string>类型返回该文件中的所有NC代码
	/// --1--调用了NCFileList函数，获得当前目录下所有符合要求的文件的文件名
	/// --2--上一步操作在这里显得有点多余，其实是因为数控面板中的程序列表可以显示存储器中的文件信息，这是为改善那一步准备的
	/// </summary>
	/// <returns>
	/// 该NC程序文件中的所有NC代码，返回类型：List<string>
	/// </returns>
	/// <param name='filename'>
	/// NC程序的程序名
	/// </param>
	public List<string> CodeLoad (string filename)
	{
		List<string> original_code = new List<string>();
		bool success_open = true;
		List<string> file_name_list = NCFileList();
		if(file_name_list.Count > 0)
		{//1 level
			//Judge whether the input string is right or not.
			if(filename.StartsWith("O"))
			{//2 level
				string temp_name = filename.Trim('O');
				//Regular Expression: 判断输入的是否为数字字符，且大小在(0,10000)之间
				Regex name_Reg = new Regex(@"^\d{1,4}$");
				if(name_Reg.IsMatch(temp_name))
				{//3 level
					int name_num = Convert.ToInt32(temp_name);
					if(name_num > 0 && name_num < 10)
						temp_name = "O000" + name_num.ToString();
					else if(name_num >= 10 && name_num < 100 )
						temp_name = "O00" + name_num.ToString();
					else if(name_num >= 100 && name_num < 1000)
						temp_name = "O0" + name_num.ToString();
					else
						temp_name = "O" + name_num.ToString();
					if(file_name_list.IndexOf(temp_name) >= 0)
					{//4 level
						string file_path = "";
						FileInfo exist_check = new FileInfo(document_path + temp_name + ".txt");
						if(exist_check.Exists)
							file_path = document_path + temp_name + ".txt";
						else
						{
							exist_check = new FileInfo(document_path + temp_name + ".cnc");
							if(exist_check.Exists)
								file_path = document_path + temp_name + ".cnc";
							else
							{
								exist_check = new FileInfo(document_path + temp_name + ".nc");
								if(exist_check.Exists)
									file_path = document_path + temp_name + ".nc";
								else
									success_open = false;
							}
						}
						//Acquire original code.
						if(success_open)
						{
							FileStream code_file_stream = new FileStream(file_path, FileMode.Open, FileAccess.Read); 
							StreamReader code_SR = new StreamReader(code_file_stream);
							string s_Line = code_SR.ReadLine();
							while(s_Line != null)
							{
								original_code.Add(s_Line);
								s_Line = code_SR.ReadLine();
							}
							code_SR.Close();
						}
						else
							Debug.LogError("Unexpected error! Program: " + temp_name + " disappears.  Error caused by Eric.");
					}//4 level
					else
						Debug.LogWarning("Can't find Program: " + temp_name + " in current working directory!  Warning caused by Eric.");
				}//3 level
				else
					Debug.LogError("格式错误! Error caused by Eric.");
			}//2 level
			else
				Debug.LogError("格式错误! Error caused by Eric.");
		}//1 level
		else
			Debug.LogWarning("Can't find any file in current working directory. 	Warning caused by Eric.");
		return original_code;
	}
	
	/// <summary>
	/// 获取当前目录下符合要求的文件的文件名
	/// </summary>
	/// <returns>
	/// 文件名字列表，返回类型：List<string>
	/// </returns>
	public List<string> NCFileList ( )
	{
		List<string> file_name_list = new List<string>();
		//Judge whether the file directory is right or not
		if(Directory.Exists(document_path))
		{
			//Acquire all of the files's name under current directory.
			string[] tempFileList = Directory.GetFiles(document_path);
			if(tempFileList.Length > 0)
			{
				foreach(string fullname in tempFileList)
				{
					//Regular Expression: 判断文件路径中是否包含指定格式的字符串："O"+"4个数字"+"."+"2或3个字符结尾"
					Regex fullname_Reg = new Regex(@"O\d{4}.\w{2,3}$");
					if(fullname_Reg.IsMatch(fullname))
						file_name_list.Add(fullname_Reg.Match(fullname).Value.Substring(0,5));		
				}
			}
			else
				Debug.LogWarning("Can't find any file in current working directory. 	Warning caused by Eric.");
		}
		else
			Debug.LogError("The file directory doesn't exist. 	Error caused by Eric.");
		return file_name_list;
	}
}
