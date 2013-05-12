//内容--增加脚本文件AuxiliaryMoveModule用于控制机床辅助部件的移动，姓名--刘旋，时间--2013-4-15
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class AuxiliaryMoveModule : MonoBehaviour {
	ControlPanel Main;
	MoveControl MoveControl_script;
	
	public Transform Xp_part1;//X轴正向移动的第一个铁片
	public Transform Xp_part2;//X轴正向移动的第二个铁片
	public Transform Xp_part3;//X轴正向移动的第三个铁片
	public Transform Xp_part4;//X轴正向移动的第四个铁片
	
	public Transform Xn_part1;//X轴负向移动的第一个铁片
	public Transform Xn_part2;//X轴负向移动的第二个铁片
	public Transform Xn_part3;//X轴负向移动的第三个铁片
	public Transform Xn_part4;//X轴负向移动的第四个铁片
	
	public float Xp_zero1;//X轴正向移动的第一个铁片的起始z轴坐标
	public float Xp_zero2;//X轴正向移动的第二个铁片的起始z轴坐标
	public float Xp_zero3;//X轴正向移动的第三个铁片的起始z轴坐标
	public float Xp_zero4;//X轴正向移动的第四个铁片的起始z轴坐标
	public float Xn_zero1;//X轴负向移动的第一个铁片的起始z轴坐标
	public float Xn_zero2;//X轴负向移动的第二个铁片的起始z轴坐标
	public float Xn_zero3;//X轴负向移动的第三个铁片的起始z轴坐标
	public float Xn_zero4;//X轴负向移动的第四个铁片的起始z轴坐标
	
	public Transform Yn_part1;//Y轴负向移动的第一个铁片
	public Transform Yn_part2;//Y轴负向移动的第二个铁片
	public Transform Yn_part3;//Y轴负向移动的第三个铁片
	
	public float Yn_zero1;//Y轴负向移动的第一个铁片的起始x轴坐标
	public float Yn_zero2;//Y轴负向移动的第二个铁片的起始x轴坐标
	public float Yn_zero3;//Y轴负向移动的第三个铁片的起始x轴坐标
	
	public Transform Zn_part1;//Z轴负向移动的第一个铁片
	public Transform Zn_part2;//Z轴负向移动的第二个铁片
	public Transform Zn_part3;//Z轴负向移动的第三个铁片
	
	public float Zn_zero1;//Z轴负向移动的第一个铁片的起始y轴坐标
	public float Zn_zero2;//Z轴负向移动的第二个铁片的起始y轴坐标
	public float Zn_zero3;//Z轴负向移动的第三个铁片的起始y轴坐标
	
	public float X_distance;//X轴移动方向上，主轴与铁片的z轴距离
	public float Y_distance;//Y轴移动方向上，主轴与铁片的x轴距离
	public float Z_distance;//Z轴移动方向上，主轴与铁片的y轴距离
	
	
	

	// Use this for initialization
	void Start () {
		Main=GameObject.Find("MainScript").GetComponent<ControlPanel>();
		GameObject.Find("move_control").AddComponent("MoveControl");
		MoveControl_script = GameObject.Find("move_control").GetComponent<MoveControl>();
		
		Xp_part1=GameObject.Find("X01_Y").transform;
		Xp_part2=GameObject.Find("X02_Y").transform;
		Xp_part3=GameObject.Find("X03_Y").transform;
		Xp_part4=GameObject.Find("X04_Y").transform;
		Xn_part1=GameObject.Find("X01_Z").transform;
		Xn_part2=GameObject.Find("X02_Z").transform;
		Xn_part3=GameObject.Find("X03_Z").transform;
		Xn_part4=GameObject.Find("X04_Z").transform;
		
		Xp_zero1=Xp_part1.position.z;
		Xp_zero2=Xp_part2.position.z;
		Xp_zero3=Xp_part3.position.z;
		Xp_zero4=Xp_part4.position.z;
		
		Xn_zero1=Xn_part1.position.z;
		Xn_zero2=Xn_part2.position.z;
		Xn_zero3=Xn_part3.position.z;
		Xn_zero4=Xn_part4.position.z;
		
		Yn_part1=GameObject.Find("Y01").transform;
		Yn_part2=GameObject.Find("Y02").transform;
		Yn_part3=GameObject.Find("Y03").transform;
		
		Yn_zero1=Yn_part1.position.x;
		Yn_zero2=Yn_part2.position.x;
		Yn_zero3=Yn_part3.position.x;
		
		Zn_part1=GameObject.Find("Z01").transform;
		Zn_part2=GameObject.Find("Z02").transform;
		Zn_part3=GameObject.Find("Z03").transform;
		
		Zn_zero1=Zn_part1.position.y;
		Zn_zero2=Zn_part2.position.y;
		Zn_zero3=Zn_part3.position.y;
	
	}
	
	// Update is called once per frame
	void Update () {
		xMove();
		yMove();
		zMove();
	}
	void xMove()
	{
		//X轴正向，离主轴最近的铁片的移动
		X_distance=MoveControl_script.X_part.position.z-Xp_part4.position.z;
		if(X_distance>0.165f)
		{
			if(MoveControl_script.x_p)
			    Xp_part4.Translate(0,0,-MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
		if(Xp_part4.position.z>Xp_zero4)
		{
			if(MoveControl_script.x_n)
			    Xp_part4.Translate(0,0,MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
		//X轴正向，离主轴距离第二的铁片的移动
		X_distance=MoveControl_script.X_part.position.z-Xp_part3.position.z;
		if(X_distance>0.36f)
		{
			if(MoveControl_script.x_p)
			    Xp_part3.Translate(0,0,-MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
		if(Xp_part3.position.z>Xp_zero3)
		{
			if(MoveControl_script.x_n)
			    Xp_part3.Translate(0,0,MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
		//X轴正向，离主轴距离第三的铁片的移动
		X_distance=MoveControl_script.X_part.position.z-Xp_part2.position.z;
		if(X_distance>0.555f)
		{
			if(MoveControl_script.x_p)
			    Xp_part2.Translate(0,0,-MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
		if(Xp_part2.position.z>Xp_zero2)
		{
			if(MoveControl_script.x_n)
			    Xp_part2.Translate(0,0,MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
		//X轴正向，离主轴距离第四的铁片的移动
		X_distance=MoveControl_script.X_part.position.z-Xp_part1.position.z;
		if(X_distance>0.745f)
		{
			if(MoveControl_script.x_p)
			    Xp_part1.Translate(0,0,-MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
		if(Xp_part1.position.z>Xp_zero1)
		{
			if(MoveControl_script.x_n)
			    Xp_part1.Translate(0,0,MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
		//X轴负向，离主轴最近的铁片的移动
		X_distance=MoveControl_script.X_part.position.z-Xn_part4.position.z;
		if(X_distance<-1.35f)
		{
			if(MoveControl_script.x_n)
				Xn_part4.Translate(0,0,MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
		if(Xn_part4.position.z<Xn_zero4)
		{
			if(MoveControl_script.x_p)
				Xn_part4.Translate(0,0,-MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
		//X轴负向，离主轴距离第二的铁片的移动
		X_distance=MoveControl_script.X_part.position.z-Xn_part3.position.z;
		if(X_distance<-1.545f)
		{
			if(MoveControl_script.x_n)
				Xn_part3.Translate(0,0,MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
		if(Xn_part3.position.z<Xn_zero3)
		{
			if(MoveControl_script.x_p)
				Xn_part3.Translate(0,0,-MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}	
		//X轴负向，离主轴距离第三的铁片的移动
		X_distance=MoveControl_script.X_part.position.z-Xn_part2.position.z;
		if(X_distance<-1.74f)
		{
			if(MoveControl_script.x_n)
				Xn_part2.Translate(0,0,MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
		if(Xn_part2.position.z<Xn_zero2)
		{
			if(MoveControl_script.x_p)
				Xn_part2.Translate(0,0,-MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
		//X轴正向，离主轴距离第四的铁片的移动
		X_distance=MoveControl_script.X_part.position.z-Xn_part1.position.z;
		if(X_distance<-1.93f)
		{
			if(MoveControl_script.x_n)
				Xn_part1.Translate(0,0,MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
		if(Xn_part1.position.z<Xn_zero1)
		{
			if(MoveControl_script.x_p)
				Xn_part1.Translate(0,0,-MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate);
		}
	}
	void yMove()
	{
		//Y轴负向，离主轴最近的铁片的移动
		Y_distance=MoveControl_script.Y_part.position.x-Yn_part3.position.x;
		if(Y_distance>0.723f)
		{
			if(MoveControl_script.y_n)
				Yn_part3.Translate(-MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate,0,0);
		}
		if(Yn_part3.position.x>Yn_zero3)
		{
			if(MoveControl_script.y_p)
				Yn_part3.Translate(MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate,0,0);
		}
		//Y轴负向，离主轴距离第二的铁片的移动
		Y_distance=MoveControl_script.Y_part.position.x-Yn_part2.position.x;
		if(Y_distance>0.887f)
		{
			if(MoveControl_script.y_n)
				Yn_part2.Translate(-MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate,0,0);
		}
		if(Yn_part2.position.x>Yn_zero2)
		{
			if(MoveControl_script.y_p)
				Yn_part2.Translate(MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate,0,0);
		}
		//Y轴负向，离主轴距离第三的铁片的移动
		Y_distance=MoveControl_script.Y_part.position.x-Yn_part1.position.x;
		if(Y_distance>1.05f)
		{
			if(MoveControl_script.y_n)
				Yn_part1.Translate(-MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate,0,0);
		}
		if(Yn_part1.position.x>Yn_zero1)
		{
			if(MoveControl_script.y_p)
				Yn_part1.Translate(MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate,0,0);
		}
	}
	void zMove()
	{
		//Z轴负向，离主轴最近的铁片的移动
		Z_distance=MoveControl_script.Z_part.position.y-Zn_part3.position.y;
		if(Z_distance<-0.05f)
		{
			if(MoveControl_script.z_n)
			    Zn_part3.Translate(0,-MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate,0);	
		}
		if(Zn_part3.position.y<Zn_zero3)
		{
			if(MoveControl_script.z_p)
				Zn_part3.Translate(0,MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate,0);
		}
		//Z轴负向，离主轴距离第二的铁片的移动
		Z_distance=MoveControl_script.Z_part.position.y-Zn_part2.position.y;
		if(Z_distance<-0.03f)
		{
			if(MoveControl_script.z_n)
			    Zn_part2.Translate(0,-MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate,0);	
		}
		if(Zn_part2.position.y<Zn_zero2)
		{
			if(MoveControl_script.z_p)
				Zn_part2.Translate(0,MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate,0);
		}
		//Z轴负向，离主轴距离第三的铁片的移动
		Z_distance=MoveControl_script.Z_part.position.y-Zn_part1.position.y;
		if(Z_distance<-0.01f)
		{
			if(MoveControl_script.z_n)
			    Zn_part1.Translate(0,-MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate,0);	
		}
		if(Zn_part1.position.y<Zn_zero1)
		{
			if(MoveControl_script.z_p)
				Zn_part1.Translate(0,MoveControl_script.speed_to_move*Time.deltaTime*MoveControl_script.move_rate,0);
		}
	}
}
