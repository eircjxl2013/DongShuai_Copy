using UnityEngine;
using System.Collections;

public class camera_sm : MonoBehaviour {

	// Use this for initialization
	Vector3 p,p1,axe;
	Vector3 old_p;
	public Vector3 center;
	Quaternion old_r;
	float old_field;
	Vector3 new1,old1;
//	public GameObject cen,empty;
	public bool locked;
	

	
	GameObject target;
	// Use this for initialization
	void Start () {
		
		old_p=this.transform.position;
		old_r=this.transform.rotation;
		old_field=camera.fieldOfView;
	
	}
	
	// Update is called once per frame
	void Update () {
			
		
		float s1=20;
		float s2=10;	
		
		if(Input.GetMouseButton(2)&&Input.GetMouseButton(1)==false)					
		{	
			axe.x=-Input.GetAxis("Mouse Y");
			axe.y=Input.GetAxis("Mouse X");
	
			
			new1=Input.mousePosition;						
		//	axe.x=e.delta.y;					
		//	axe.y=e.delta.x;					
			axe.z=0;
			
		//	p=axe;
			p=camera.transform.TransformDirection(axe);	
		//	p=camera.transform.InverseTransformDirection(p);
		//	p= camera.ScreenToWorldPoint (new Vector3(axe.x,axe.y,0));
			if(new1==old1)						
			{
					
			}				
			else
			{
		
				float dist1 = Vector3.Distance(camera.transform.position, new Vector3(0,0,0));
	
					
		//		Vector3 pp = camera.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height/2, dist1));
				
		//		float x=-camera.transform.position.y/(pp.y-camera.transform.position.y)*(pp.x-camera.transform.position.x)+camera.transform.position.x;
				
		//		float z=-camera.transform.position.y/(pp.y-camera.transform.position.y)*(pp.z-camera.transform.position.z)+camera.transform.position.z;
		//		Vector3 ss=new Vector3(x,0,z);
		//		float dist = Vector3.Distance(camera.transform.position, ss);
		//		pp = camera.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height/2, dist));
	
									
		//		camera.transform.RotateAround(pp, p, 10* 0.02F*s1);
				camera.transform.RotateAround(new Vector3(0,0,0), p, axe.magnitude*10*Time.deltaTime*s1);
		//		camera.transform.Rotate(p,10*Time.deltaTime*s1,Space.Self);
				//Debug.Log(p+" "+ 10*Time.deltaTime*s1);
		//		camera.transform.LookAt(center_obj.transform);
				
			}
					
			old1=new1;
				
		}
					
		if(Input.GetMouseButton(2)&&Input.GetMouseButton(1))
							
		{
					
			new1=Input.mousePosition;
								
			//axe.x=e.delta.x;
				axe.x=	Input.GetAxis("Mouse X");			
		//axe.y=-e.delta.y;
				axe.y=	Input.GetAxis("Mouse Y");			
			axe.z=0;
		
								
		//	p=-axe.normalized;
				p=-axe;			
								
			if(new1!=old1)					
				transform.Translate(axe.magnitude*p*Time.deltaTime*1.0F*s2,Space.Self);
		
								
			old1=new1;
		
	
		}
		//camera.fieldOfView
		
							
		if (Input.GetAxis("Mouse ScrollWheel") != 0) 						
		{
							
			if(camera.fieldOfView>1&&Input.GetAxis("Mouse ScrollWheel")<0)
			{
				if(camera.fieldOfView+Input.GetAxis("Mouse ScrollWheel")*90*Time.deltaTime>1)							
				camera.fieldOfView+=Input.GetAxis("Mouse ScrollWheel")*90*Time.deltaTime;
				else
					camera.fieldOfView=1;
			}
			
										
			if(Input.GetAxis("Mouse ScrollWheel")>0&&camera.fieldOfView<70)
			{
				if(camera.fieldOfView+Input.GetAxis("Mouse ScrollWheel")*90*Time.deltaTime<70)							
				camera.fieldOfView+=Input.GetAxis("Mouse ScrollWheel")*90*Time.deltaTime;
				else
					camera.fieldOfView=70;
			}
						
		}
		
				
		
	}
	void OnGUI() {
	
	}
	
	
	void recover()
	{
		this.transform.position=old_p;
		this.transform.rotation=old_r;
		camera.fieldOfView=old_field;

	}
}
