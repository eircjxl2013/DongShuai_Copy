using UnityEngine;
using System.Collections;
using System.Threading;
public class MyThread
{
	public int count;
	string thrdName;
	
	public MyThread(string nam)
	{
	     count = 0;
	  thrdName = nam;
	}
	public void run()
	{
	  Debug.Log("start run a thread");
	  do{
	     Thread.Sleep(500);
	     Debug.Log("in child thread count="+count);
	     count++;   
	   }while(count <20);
	   Debug.Log("end thread");
	}
}

public class ThreadTest : MonoBehaviour {
	
	MyThread mt = new MyThread("CHILE ");
	Thread newThrd;
	// Use this for initialization
	void Start () {
	  Debug.Log("start main"+Time.time);
	  
	  
	}
	
	void OnGUI ()
	{
		if(GUI.Button(new Rect(200, 200, 100, 30), "Start"))
		{
			newThrd = new Thread(new ThreadStart(mt.run));
			newThrd.Start();
			
		}
		
		if(GUI.Button(new Rect(200, 200, 100, 30), "Pause"))
		{
			
		}
	}
	
	// Update is called once per frame
	void Update () {
	      //Debug.Log(Time.time);
	}
}