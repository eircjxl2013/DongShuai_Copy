//内容--增加脚本文件ViewSeleModule，用于实现数控机床的正视图、俯视图和左视图
//姓名--刘旋，时间--2013-4-7

using UnityEngine;
using System.Collections;

public class ViewSelectModule : MonoBehaviour {
	GameObject mainCamera;//内容--获取相机物体，用于对相机进行设置，姓名--刘旋，时间--2013-4-7
	Vector3 current_position;//内容--获取相机当前位置坐标，姓名--刘旋，时间--2013-4-7
	Vector3 targetFront_position;//内容--正视图下，相机的位置，姓名--刘旋，时间--2013-4-7
	Vector3 targetTop_position;//内容--俯视图下，相机的位置，姓名--刘旋，时间--2013-4-7
	Vector3 targetLeft_position;//内容--左视图下，相机的位置，姓名--刘旋，时间--2013-4-7
	Vector3 current_angles;//内容--相机当前角度信息，姓名--刘旋，时间--2013-4-7
	Vector3 targetFront_angles;//内容--正视图下，相机的角度，姓名--刘旋，时间--2013-4-7
	Vector3 targetTop_angles;//内容--俯视图下，相机的角度，姓名--刘旋，时间--2013-4-7
	Vector3 targetLeft_angles;//内容--左视图下，相机的角度，姓名--刘旋，时间--2013-4-7
	bool front_flag=false;//内容--正视图显示标志，姓名--刘旋，时间--2013-4-7
	bool top_flag=false;//内容--俯视图显示标志，姓名--刘旋，时间--2013-4-7
	bool left_flag=false;//内容--左视图显示标志，姓名--刘旋，时间--2013-4-7
	float currentView;//内容--相机当前视角信息，姓名--刘旋，时间--2013-4-7
	float targetView;//内容--相机的最终角度信息，姓名--刘旋，时间--2013-4-7

	
	
	void Awake()
	{
		mainCamera=GameObject.Find("Main Camera");
		targetFront_position=new Vector3(-30f,1.2f,-1.1f);
		targetTop_position=new Vector3(0,30f,-1.1f);
		targetLeft_position=new Vector3(0,1.2f,30f);
		targetFront_angles=new Vector3(0,90f,0);
		targetTop_angles=new Vector3(90f,90f,0);
		targetLeft_angles=new Vector3(0,180f,0);
		targetView=6f;
		current_angles=targetFront_angles;//内容--若首次显示俯视图，为相机角度赋初值，姓名--刘旋，时间--2013-4-7
		
	}

	// Use this for initialization
	void Start () {
		
	
	}
	void OnGUI()
	{
		if(GUI.Button(new Rect(10,190,100,40),"Front View"))
		{
			front_flag=true;//内容--正视图下，正视图显示标志位真，姓名--刘旋，时间--2013-4-7
			top_flag=false;//内容--正视图下，俯视图显示标志位假，姓名--刘旋，时间--2013-4-7
			left_flag=false;//内容--正视图下，左视图显示标志位假，姓名--刘旋，时间--2013-4-7
			currentView=30f;//内容--正视图下，当前视角设为30，姓名--刘旋，时间--2013-4-7
		
			
		}
		if(GUI.Button(new Rect(10,240,100,40),"Top View"))
		{
			
			front_flag=false;
			top_flag=true;
			left_flag=false;
			currentView=150f;//内容--俯视图下，当前视角设为150，姓名--刘旋，时间--2013-4-7
			
		}
		if(GUI.Button(new Rect(10,290,100,40),"Left View"))
		{
			front_flag=false;
			top_flag=false;
			left_flag=true;
			currentView=30f;//内容--左视图下，当前视角设为30，姓名--刘旋，时间--2013-4-7
			
		}
	}
	// Update is called once per frame
	void Update () 
	{
		if (front_flag)
			Front();
		if (top_flag)
			Top();
		if (left_flag)
			Left();
	}
	void Front()
	{
		mainCamera.camera.transform.LookAt(new Vector3(0,1.2f,-1.1f));//内容--相机的z轴始终指向该点，姓名--刘旋，时间--2013-4-7
		current_position=mainCamera.transform.position;//内容--获取相机的当前位置信息，姓名--刘旋，时间--2013-4-7
		current_position=Vector3.Lerp(current_position,targetFront_position,Time.deltaTime*3f);//内容--在当前位置信息和最终位置信息间进行线性插值，姓名--刘旋，时间--2013-4-7
		mainCamera.transform.position=current_position;//内容--为相机位置赋值，姓名--刘旋，时间--2013-4-7
		current_angles=mainCamera.transform.eulerAngles;//内容--获取相机当前角度信息，姓名--刘旋，时间--2013-4-7
		current_angles=Vector3.Slerp(current_angles,targetFront_angles,Time.deltaTime*3f);//内容--在当前角度和最终角度间进行弧度插值，姓名--刘旋，时间--2013-4-7
		mainCamera.transform.eulerAngles=current_angles;//内容--为相机角度赋值，姓名--刘旋，时间--2013-4-7
		currentView=Mathf.LerpAngle(currentView,targetView,Time.deltaTime*3f);//内容--在当前视角和最终视角间进行线性插值，姓名--刘旋，时间--2013-4-7
		mainCamera.camera.fieldOfView=currentView;//内容--为相机视角赋值，姓名--刘旋，时间--2013-4-7
	}
	void Top()
	{
		mainCamera.camera.transform.LookAt(new Vector3(0,1.2f,-1.1f));
		current_position=mainCamera.transform.position;
		current_position=Vector3.Lerp(current_position,targetTop_position,Time.deltaTime*3f);
		mainCamera.transform.position=current_position;
		//current_angles=mainCamera.transform.eulerAngles;会引起震荡
		current_angles=Vector3.Slerp(current_angles,targetTop_angles,Time.deltaTime*3f);
		mainCamera.transform.eulerAngles=current_angles;
		currentView=Mathf.LerpAngle(currentView,targetView,Time.deltaTime*3f);
		mainCamera.camera.fieldOfView=currentView;
	}
	void Left()
	{
		mainCamera.camera.transform.LookAt(new Vector3(0,1.2f,-1.1f));
		current_position=mainCamera.transform.position;
		current_position=Vector3.Lerp(current_position,targetLeft_position,Time.deltaTime*3f);
		mainCamera.transform.position=current_position;
		current_angles=mainCamera.transform.eulerAngles;
		current_angles=Vector3.Slerp(current_angles,targetLeft_angles,Time.deltaTime*3f);
		mainCamera.transform.eulerAngles=current_angles;
		currentView=Mathf.LerpAngle(currentView,targetView,Time.deltaTime*3f);
		mainCamera.camera.fieldOfView=currentView;
	}
	
}
