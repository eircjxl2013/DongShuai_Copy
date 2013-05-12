using UnityEngine;
using System.Collections;

public class LineDrawer : MonoBehaviour {

	// Use this for initialization
	public VectorLine straightLine;
	public VectorLine arcLine;
	void Start () {
	     
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// 在空间任意两点画圆弧
	/// </summary>
	/// <param name='startPoint'>
	/// 圆弧的起点坐标
	/// </param>
	/// <param name='endPoint'>
	/// 圆弧的终点坐标
	/// </param>
	/// <param name='origin'>
	/// 圆弧的圆心坐标
	/// </param>
	/// <param name='angle'>
	/// 圆弧的弧度
	/// </param>
	/// <param name='rads'>
	/// 圆弧的半径
	/// </param>
	/// <param name='planeFlag'>
	/// 其值为1，2，3分别表示圆弧平行于xoy,xoz,yoz平面
	/// </param>
	/// <param name='segments'>
	/// 插补线段的数目，控制圆弧的精度
	/// </param>
	/// <param name='lineWidth'>
	/// 圆弧的线宽
	/// </param>
	/// <param name='lineColor'>
	/// 圆弧的颜色
	/// </param>
	/// <param name='mat'>
	/// 圆弧的材质
	/// </param>
	public void DrawArcLine(Vector3 startPoint,Vector3 endPoint,Vector3 origin,float angle,float rads,int planeFlag,int segments,float lineWidth,Color lineColor,Material mat)
	//startPoint为圆弧起始点坐标，endPoint为圆弧终点坐标，origin为圆弧圆心坐标，angle为圆弧的弧度，rads为半径，planeFlag为1表示圆弧平行于xoy平面，为2平行于xoz,为3平行于yoz
		//segments表示插补的直线段数目，lineWidth为圆弧的线宽，lineColor为圆弧的颜色，mat为圆弧的材质。
	{
		Vector3[] linePoints=new Vector3[(segments-1)*2+2];
		linePoints[0]=startPoint;
		float intervalAngle=angle/segments;
		float currAngle=intervalAngle;
		int posIndex=0;
		for(int i=1;i<=segments-1;i++)
		{
			Vector3 nextPoint=CalculateNextPoint(startPoint,endPoint,origin,rads,planeFlag,currAngle,angle);
			if (posIndex==0)
				linePoints[posIndex]=startPoint;
			else
			    linePoints[posIndex]=linePoints[posIndex-1];
			Debug.Log(linePoints[posIndex]);
			linePoints[posIndex+1]=nextPoint;
			posIndex+=2;
			currAngle+=intervalAngle;
			
			Debug.Log(nextPoint);
		}
		Debug.Log(currAngle);
		linePoints[posIndex]=linePoints[posIndex-1];
		linePoints[posIndex+1]=endPoint;
		arcLine=new VectorLine("arc",linePoints,lineColor,mat,lineWidth);
		Vector.DrawLine3D(arcLine);
	}
	
	public Vector3 CalculateNextPoint(Vector3 stP,Vector3 endP,Vector3 orgn,float r,int planeFlag,float angle,float arcAngle)
	{
		//float distance=Mathf.Pow (stP.x-endP.x,2)+Mathf.Pow (stP.y-endP.y,2);
		//Vector3 nexPos;
		float c=r*r;
		float d=Mathf.Pow((Mathf.Sin(angle/2))*2*r,2);
		if(planeFlag==1)//平行于xoy平面画弧线
		{
			float x=0;float y=0;
			float m=c-d+stP.x*stP.x+stP.y*stP.y-orgn.x*orgn.x-orgn.y*orgn.y;
			float p=m/(2*(stP.x-orgn.x));
			float q=(stP.y-orgn.y)/(stP.x-orgn.x);
			float j=orgn.x-p;
			float t=2*(j*q-orgn.y);
			float s=j*j-c+orgn.y*orgn.y;
			float temp=t*t-4*s*(q*q+1);
			if(temp<0)
			{
				Debug.LogError("没有符合要求的解fgdfgf！");
				return new Vector3(0,0,0);
			}
			if(temp==0)
			{
				y=(-t)/(2*(1+q*q));
				x=p-q*y;
				return new Vector3(x,y,stP.z);
			}
			else{
				float y1=(-t+Mathf.Pow(temp,0.5f))/(2*(1+q*q));
			    float y2=(-t-Mathf.Pow(temp,0.5f))/(2*(1+q*q));
			    float x1=p-q*y1;
				float x2=p-q*y2;
				/*float absstx=Mathf.Abs(stP.x);
				float absedx=Mathf.Abs(endP.x);
				float maxX=(absstx>absedx)?absstx:absedx;
				float minX=(absstx<absedx)?absstx:absedx;
				float abssty=Mathf.Abs(stP.y);
				float absedy=Mathf.Abs(endP.y);
				float maxY=(abssty>absedy)?abssty:absedy;
				float minY=(abssty<absedy)?abssty:absedy;
				float absx1=Mathf.Abs(x1);
				float absx2=Mathf.Abs(x2);
				float absy1=Mathf.Abs(y1);
				float absy2=Mathf.Abs(y2);*/
				float distance1End=Mathf.Pow(x1-endP.x,2)+Mathf.Pow(y1-endP.y,2);
				float distance2End=Mathf.Pow(x2-endP.x,2)+Mathf.Pow(y2-endP.y,2);
				if(arcAngle<3.1416)
				{
				   if(distance1End<distance2End)
				   {
					return new Vector3(x1,y1,stP.z);
				    }
				   else if(distance2End<distance1End)
				    {
					return new Vector3(x2,y2,stP.z);
				    }
				   else{
					Debug.LogError("没有符合要求的解！");
					return new Vector3(0,0,0);
				   }
				}
				else{
					if(angle<3.1416)
					{
						if(distance1End>distance2End)
				        {
					      return new Vector3(x1,y1,stP.z);
				        }
				        else if(distance2End>distance1End)
				        {
					       return new Vector3(x2,y2,stP.z);
				         }
				         else{
					        Debug.LogError("没有符合要求的解！");
					        return new Vector3(0,0,0);
				          }
					}
					else if(angle>3.1416)
					{
						if(distance1End<distance2End)
				       {
					      return new Vector3(x1,y1,stP.z);
				        }
				        else if(distance2End<distance1End)
				        {
					       return new Vector3(x2,y2,stP.z);
				         }
				        else{
					       Debug.LogError("没有符合要求的解！");
					       return new Vector3(0,0,0);
				         }
					}
				}
				
			}
			
		}
		if(planeFlag==2)//平行于xoz平面画弧线
		{
			float x=0,z=0;
			float m=c-d+stP.x*stP.x+stP.z*stP.z-orgn.x*orgn.x-orgn.z*orgn.z;
			float p=m/(2*(stP.x-orgn.x));
			float q=(stP.z-orgn.z)/(stP.x-orgn.x);
			float j=orgn.x-p;
			float t=2*(j*q-orgn.z);
			float s=j*j-c+orgn.z*orgn.z;
			float temp=t*t-4*s*(q*q+1);
			if(temp<0)
			{
				Debug.LogError("没有符合要求的解！hhhhhhhhh");
				return new Vector3(0,0,0);
			}
			if(temp==0)
			{
				z=(-t)/(2*(1+q*q));
				x=p-q*z;
				return new Vector3(x,stP.y,z);
			}
			else{
				float z1=(-t+Mathf.Pow(temp,0.5f))/(2*(1+q*q));
			    float z2=(-t-Mathf.Pow(temp,0.5f))/(2*(1+q*q));
			    float x1=p-q*z1;
				float x2=p-q*z2;
				float distance1End=Mathf.Pow(x1-endP.x,2)+Mathf.Pow(z1-endP.z,2);
				float distance2End=Mathf.Pow(x2-endP.x,2)+Mathf.Pow(z2-endP.z,2);
				if(arcAngle<3.1416)
				{
				   if(distance1End<=distance2End)
				   {
					return new Vector3(x1,stP.y,z1);
				    }
				   else if(distance2End<distance1End)
				    {
					return new Vector3(x2,stP.y,z2);
				    }
				   else{
					Debug.LogError("没有符合要求的解！");
					return new Vector3(0,0,0);
				   }
				}
				else{
					if(angle<3.1416)
					{
						if(distance1End>distance2End)
				        {
					     return new Vector3(x1,stP.y,z1);
				        }
				        else if(distance2End>distance1End)
				        {
					       return new Vector3(x2,stP.y,z2);
				         }
				         else{
					        Debug.LogError("没有符合要求的解！");
					        return new Vector3(0,0,0);
				          }
					}
					else if(angle>3.1416)
					{
						if(distance1End<distance2End)
				       {
					      return new Vector3(x1,stP.y,z1);
				        }
				        else if(distance2End<distance1End)
				        {
					       return new Vector3(x2,stP.y,z2);
				         }
				        else{
					       Debug.LogError("没有符合要求的解！");
					       return new Vector3(0,0,0);
				         }
					}
				}
			}
			
		}
		if(planeFlag==3)//平行于yoz平面画弧线
		{
			float y=0,z=0;
			float m=c-d+stP.y*stP.y+stP.z*stP.z-orgn.y*orgn.y-orgn.z*orgn.z;
			float p=m/(2*(stP.y-orgn.y));
			float q=(stP.z-orgn.z)/(stP.y-orgn.y);
			float j=orgn.y-p;
			float t=2*(j*q-orgn.z);
			float s=j*j-c+orgn.z*orgn.z;
			float temp=t*t-4*s*(q*q+1);
			if(temp<0)
			{
				Debug.LogError("没有符合要求的解！");
				return new Vector3(0,0,0);
			}
			if(temp==0)
			{
				z=(-t)/(2*(1+q*q));;
				y= p-q*z;
				return new Vector3(stP.x,y,z);
			}
			else{
				float z1=(-t+Mathf.Pow(temp,0.5f))/(2*(1+q*q));
			    float z2=(-t-Mathf.Pow(temp,0.5f))/(2*(1+q*q));
			    float y1=p-q*z1;
				float y2=p-q*z2;
				float distance1End=Mathf.Pow(y1-endP.y,2)+Mathf.Pow(z1-endP.z,2);
				float distance2End=Mathf.Pow(y2-endP.y,2)+Mathf.Pow(z2-endP.z,2);
				if(arcAngle<3.1416)
				{
				   if(distance1End<distance2End)
				   {
					return new Vector3(stP.x,y1,z1);
				    }
				   else if(distance2End<distance1End)
				    {
					return new Vector3(stP.x,y2,z2);
				    }
				   else{
					Debug.LogError("没有符合要求的解！");
					return new Vector3(0,0,0);
				   }
				}
				else{
					if(angle<3.1416)
					{
						if(distance1End>distance2End)
				        {
					     return new Vector3(stP.x,y1,z1);
				        }
				        else if(distance2End>distance1End)
				        {
					       return new Vector3(stP.x,y2,z2);
				         }
				         else{
					        Debug.LogError("没有符合要求的解！");
					        return new Vector3(0,0,0);
				          }
					}
					else if(angle>3.1416)
					{
						if(distance1End<distance2End)
				       {
					      return new Vector3(stP.x,y1,z1);
				        }
				        else if(distance2End<distance1End)
				        {
					       return new Vector3(stP.x,y2,z2);
				         }
				        else{
					       Debug.LogError("没有符合要求的解！");
					       return new Vector3(0,0,0);
				         }
					}
				}
				
			}
			
		}
		Debug.LogError("请选择旋转平面");
		return new Vector3(0,0,0);
	}
	/// <summary>
	/// 空间两点间画直线
	/// </summary>
	/// <param name='startPoint'>
	/// 直线的起点坐标
	/// </param>
	/// <param name='endPoint'>
	/// 直线的终点坐标
	/// </param>
	/// <param name='lineWidth'>
	/// 直线的宽度
	/// </param>
	/// <param name='lineColor'>
	/// 直线的颜色
	/// </param>
	/// <param name='mat'>
	/// 直线的材质
	/// </param>
	
	public void DrawStraightLine(Vector3 startPoint,Vector3 endPoint,float lineWidth,Color lineColor,Material mat)
	{
		
		Vector3[] line=new Vector3[2];
		line[0]=startPoint;
		line[1]=endPoint;
		//line[2]=endPoint;
		//line[3]=new Vector3(5,5,5);
		straightLine=new VectorLine("line",line,lineColor,mat,lineWidth);
		Vector.DrawLine3D(straightLine);
	}
	
	
}
