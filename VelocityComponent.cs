using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class VelocityComponent : Node
{

	[Export]
	private float maxSpeed = 100;
	[Export] 
	private float accelerationCoefficient = 10;
	[Export]
	private bool debugMode;

	public Vector2 Velocity { get; set; }
	public Vector2? VelocityOverride { get; set; }
	public float SpeedMultiplier { get; set; } = 1f;
	public float SpeedPercentModifier => speedPercentModifiers.Values.Sum();
	public float AccelerationCoefficientMultiplier { get; set; } = 1f;

	public float AccelerationCoefficient => accelerationCoefficient;
	public float SpeedPercent => Mathf.Min(Velocity.Length() / (CalculatedMaxSpeed > 0f ? CalculatedMaxSpeed : 1f), 1f);
	public float CalculatedMaxSpeed => maxSpeed * (1f + SpeedPercentModifier) * SpeedMultiplier;

	private Dictionary<string, float> speedPercentModifiers = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AccelerateToVelocity(Vector2 velocity)
	{
		Velocity = Velocity.Lerp(velocity, 0.5f);
        
    }

	public void AccelerateInDirection(Vector2 direction)
	{
		AccelerateToVelocity(direction*CalculatedMaxSpeed);
	}

	public Vector2 GetMaxVelocity(Vector2 direction)
	{
		return CalculatedMaxSpeed * direction;
	}

    public void MaximizeVelocity(Vector2 direction)
    {
        Velocity=GetMaxVelocity(direction);
    }

	public void Decelerate()
	{
		AccelerateToVelocity(Vector2.Zero);
	}

	public void Move(CharacterBody2D characterBody)
	{
		characterBody.Velocity = this.Velocity;
		characterBody.MoveAndSlide();
		

	}

	public void SetMaxSpeed(float newSpeed)
	{
		maxSpeed= newSpeed;
	}

	public void AddSpeedPercentModifier(string name,float change) 
	{
		speedPercentModifiers.TryGetValue(name, out var currentValue);
		currentValue += change;
		speedPercentModifiers[name] = currentValue;
    }

	public void SetSpeedPercentModifier(string name,float val)
	{
		speedPercentModifiers[name] = val;
	}

	public float GetSpeedPercentModifier(string name)
	{
        speedPercentModifiers.TryGetValue(name, out var currentValue);
		return currentValue;
    }




}
