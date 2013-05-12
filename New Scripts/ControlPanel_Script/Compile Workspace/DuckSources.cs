using UnityEngine;
using System.Collections;

public interface IFlyBehavior
{
 	void Fly() ;
}

class FlyNoWay:IFlyBehavior
{
	public void Fly() 
	{
		Debug.Log("No wings!");
	}
}

class FlyWithWings:IFlyBehavior
{
	public void Fly()
	{
		Debug.Log("Fly with wings!");
	}
}

public interface IQuackBehavior
{
	void Quack();
}

class Quacks:IQuackBehavior
{
	public void Quack()
	{
		Debug.Log("Quacks!");
	}
}

class Squeak:IQuackBehavior
{
	public void Quack()
	{
		Debug.Log("Squeak!");
	}
}

class MuteQuack:IQuackBehavior
{
	public void Quack()
	{
		Debug.Log(" Mute Quack!");
	}
}

public abstract class Duck
{
	protected IFlyBehavior flybehavior;
    protected IQuackBehavior quackbehavior;

    public void Swim()
    {
        Debug.Log("Swim");
    }

    public void PerformFly()
    {
        this.flybehavior.Fly();
    }

    public void PerformQuack()
    {
        this.quackbehavior.Quack();
    }
	
	public abstract void Display();
}

public class RedheadDuck : Duck
{
    public RedheadDuck()
	{
		this.flybehavior = new FlyWithWings();
		this.quackbehavior = new MuteQuack();
	}
        
    public override void Display()
    {
        Debug.Log("I'm a RedheadDuck");
    }
}