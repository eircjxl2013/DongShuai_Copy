using UnityEngine;
using System.Collections;

public class DrawLineTest : MonoBehaviour {

	// Use this for initialization
	private Vector3[] linePoints=new Vector3[2];
	private Material theMat;
	//VectorLine line;
	bool rotatey=false;
	float nowtime=0;
	bool test=true;
	LineDrawer a;
	float rotateZ=0;
	bool isRotate=false;
	bool isInitializePos=false;
	float offsetscale=0.005f;
	Vector3 initialMousePos=new Vector3(0,0,0);
	
	void Start () {
		/*linePoints[0]=new Vector3(0,0,0);
		linePoints[1]=new Vector3(2,2,2);
		theMat = new Material(Shader.Find("Self-Illumin/VertexLit"));
	    line=new VectorLine("line",linePoints,theMat,2);
		Vector.DrawLine3D(line);
		
		Debug.Log("vector.drawLine");*/
		linePoints[0]=new Vector3(-1.0f,2.0f,1.0f);
		linePoints[1]=new Vector3(4.0f,2.0f,4.0f);
		a=new LineDrawer ();
		//a.DrawArcLine(new Vector3(2.828f,2,0),new Vector3(-2.828f,2,0),new Vector3(0,2,0),3.14f,2.828f,2,40,2,Color.yellow,null);
		a.DrawArcLine(new Vector3(2.828f,2,0),new Vector3(0,2,-2.828f),new Vector3(0,2,0),1.57f,2.828f,2,100,0.02f,Color.red,null);
		//a.DrawArcLine(new Vector3(2.828f,0,2),new Vector3(2f,2,2),new Vector3(0,0,2),0.785f,2.828f,1,40,16,Color.black,null);
		//a.DrawArcLine(new Vector3(2.828f,0,2),new Vector3(0,2.828f,2),new Vector3(0,0,2),1.57f,2.828f,1,80,2,Color.red,null);
		//a.DrawArcLine(new Vector3(2.828f,0,2),new Vector3(0,-2.828f,2),new Vector3(0,0,2),1.57f,2.828f,1,40,16,Color.black,null);
         a.DrawStraightLine(linePoints[0],linePoints[1],0.02f,Color.yellow,null);
		//a.DrawArcLine(new Vector3(2,2.828f,0),new Vector3(2,0,-2.828f),new Vector3(2,0,0),1.57f,2.828f,3,40,8,Color.black,null);
		//a.DrawArcLine(new Vector3(2,2.828f,0),new Vector3(2,0,-2.828f),new Vector3(2,0,0),4.71f,2.828f,3,40,16,Color.yellow,null);
		nowtime=Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	     
	   /*while(Time.time-nowtime>0.1)
		// while(true)
		{
		 /* a.arcLine.vectorObject.transform.Rotate(Vector3.up*(Random.value*360));
		  a.arcLine.vectorObject.transform.Rotate(Vector3.forward*(Random.value*360));
		  a.arcLine.vectorObject.transform.Rotate(Vector3.right*(Random.value*360));*/
			//Camera.main.transform.Rotate(Vector3.up*(Random.value*180));
			//Camera.main.transform.Rotate(Vector3.forward*(Random.value*180));
			//Camera.main.transform.Rotate(Vector3.right*(Random.value*180));
			//rotateZ-=0.1f;
			//Camera.main.transform.Rotate(Vector3.forward*rotateZ);
		 // nowtime=Time.time;
		if(Input.GetMouseButtonDown(0))
		{
			
			isRotate=true;
			if(!isInitializePos)
				
			{
				isInitializePos=false;
				initialMousePos=Input.mousePosition;
			}
			
		}
		
		if(Input.GetMouseButtonUp(0))
		{
			isRotate=false;
			isInitializePos=false;
		}
		if(isRotate)
		{
				Camera.main.transform.RotateAround(Vector3.zero,Vector3.up,(Input.mousePosition.x-initialMousePos.x)*offsetscale);
			    Camera.main.transform.RotateAround(Vector3.zero,Vector3.right,(-Input.mousePosition.y+initialMousePos.y)*offsetscale);
			}
		}
		/*if(Input.anyKeyDown)

        {
			string ar=Input.inputString;
			Debug.Log(ar);
	        if(ar.CompareTo("W")==0)
			{
				rotateZ+=5;
	        	Camera.main.transform.Rotate(Vector3.forward*rotateZ);
			}
				
			
                

        }*/
	    

		
	}

