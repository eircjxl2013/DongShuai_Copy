using UnityEngine;
using System.Collections;
//For List
using System.Collections.Generic;
//For Regex --- Regular Expression:正则表达式
using System.Text.RegularExpressions;

public class RegexTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*
		Regex r = new Regex(@"([A-Z]+[^A-Z^\s]+)+");
		Match m = r.Match("WHILE90G02G0G11G43X0.22 G25Y0.1+");
		MatchCollection mc = r.Matches("WHILE90G02G0G11G43X0.22 G25Y0.1+");
		Debug.Log("Groups:  "+m.Groups.Count);
		Debug.Log("Matches:  "+ mc.Count);
		for(int i = 0; i < m.Groups.Count; i++)
		{
			Debug.Log(m.Groups[i].Value);
			for(int j = 0; j < m.Groups[i].Captures.Count; j++)
				Debug.Log(m.Groups[i].Captures[j].Value);
		}
		*/
		string test_str = "dsgdsfg(hello (中(你好)文) world)s()d()gf))((fgh)dgjh((dg((hello world2)";
		string pr = @"\([^\(\)]*(((?'Open'\()[^\(\)]*)+((?'-Open'\))[^\(\)]*)+)*(?(Open)(?!))\)";
		Regex r2 = new Regex(@"\([^\(\)]*(((?'Open'\()[^\(\)]*)+((?'-Open'\))[^\(\)]*)+)*(?(Open)(?!))\)");
		Regex r3 = new Regex(@"(\(+)|(\)+)");
		/// <summary>
		/// 正则表达式字符串匹配
		/// 解决“[ ]”运算符问题
		/// 涉及到了平衡组，用堆栈的思想进行判断
		/// 详情请见下列网站
		/// http://deerchao.net/tutorials/regex/regex.htm
		/// </summary>
		/// 
		Match m2 = r2.Match("dsgdsfg(hello (中(你好)文) world)sdgfdg(hello world2)");
		MatchCollection mc2 = r2.Matches("dsgdsfg(hello (中文) world)sdgfdg(hello world2)");
		
		MatchCollection match_coll = r2.Matches(test_str);
		for(int i = 0; i < match_coll.Count; i++)
		{
			Debug.Log("Matches: " + i + " --- " + match_coll[i].Value);
			Debug.Log(i + ": Old string: " + test_str);
			test_str = test_str.Replace(match_coll[i].Value.ToString(), "");
			//Regex.Replace(test_str, pr, "");
			Debug.Log(i + ": New string: " + test_str);
		}
		MatchCollection match_coll2 = r3.Matches(test_str);
		if(match_coll2.Count > 0)
		{
			Debug.Log("false!!!!!!!!!!!!!!!!!!!!");
			for(int i = 0; i < match_coll2.Count; i++)
			{
				Debug.Log("Matches: " + i + " --- " + match_coll2[i].Value);
				Debug.Log(i + ": Old string: " + test_str);
				test_str = test_str.Replace(match_coll2[i].Value.ToString(), "");
				Debug.Log(i + ": New string: " + test_str);
			}
		}
		else
			Debug.Log("true!!!!!!!!!!!!!!");
		
		Debug.Log("Groups:  "+m2.Groups.Count);
		Debug.Log("Matches:  "+ mc2.Count);
		Debug.Log(mc2[0].Value);
		Debug.Log(mc2[1].Value);
		for(int i = 0; i < m2.Groups.Count; i++)
		{
			Debug.Log(m2.Groups[i].Value);
			/*
			for(int j = 0; j < m2.Groups[i].Captures.Count; j++)
				Debug.Log(m2.Groups[i].Captures[j].Value);
				*/
		}
		
		//Debug.Log(CodeFormat("while#if#s3fdaififgd##s###f[g][][][[####hello]").Count);		
		
		//Debug.Log(CodeFormat("G10G30s768G12f199z723i+23j-234sgfadsg").Count);
		
		Debug.Log(CodeFormat("sgfadsg").Count);
	}
	
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
		{
			//To judge whether it is macroprogram or not
			Regex macro_Reg = new Regex(@"((#+)|(\[+)|(\]+)|(=+)|(GOTO)|(IF)|(WHILE)|(END))", RegexOptions.IgnoreCase);
			MatchCollection macro_Col = macro_Reg.Matches(sourceCode);
			if(macro_Col.Count > 0)
			{
				// It contains macroprogram
				/* For test
				for(int i = 0; i < macro_Col.Count; i++)
				{
					Debug.Log(i+1 + ": " + macro_Col[i].Value);		
				}
				*/
			}
			else
			{
				//It doest't contain macroprogram
				Regex format_normal_Reg = new Regex(@"([A-Z]+[^A-Z^\s]+)+", RegexOptions.IgnoreCase);
				Match format_normal_Mat = format_normal_Reg.Match(sourceCode);
				if(format_normal_Mat.Groups.Count > 1)
				{
					for(int i = 0; i < format_normal_Mat.Groups[1].Captures.Count; i++)
					{
						code_segment.Add(format_normal_Mat.Groups[1].Captures[i].Value);
						//Debug.Log(format_normal_Mat.Groups[1].Captures[i].Value);
					}
					code_segment.Add(";");
				}
				else
					Debug.Log("正常的NC代码还有未考虑到的情况或者代码格式完全不对！");
			}
		}
		return code_segment;
	}
	

	
	// Update is called once per frame
	void Update () {
		
		//Debug.Log("Wts wrong?");
	}
}
