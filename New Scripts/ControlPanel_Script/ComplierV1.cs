using UnityEngine;
using System.Collections;

public struct Person
{
    public string Name;
    public int Age;
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}

public class ComplierV1 : MonoBehaviour
{
     void Start()
    {
        // Create  struct instance and initialize by using "new".
        // Memory is allocated on thread stack.
        Person p1 = new Person("Alex", 9);
       Debug.Log("p1 Name = " + p1.Name + " Age = " + p1.Age);

        // Create  new struct object. Note that  struct can be initialized 
        // without using "new".
        Person p2 = p1;
		
        // Assign values to p2 members.
        p2.Name = "Spencer";
        //p2.Age = 7;
       	
        Debug.Log("p2 Name = " + p2.Name + " Age = " + p2.Age);

        // p1 values remain unchanged because p2 is  copy.
        Debug.Log("p1 Name = " + p1.Name + " Age = " + p1.Age);

    }
}

