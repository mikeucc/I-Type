using Godot;
using System;

public partial class StatusReceiverComponent : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public float ApplyDamageTransformation(float damage)
	{
		//This will be extended so different damage types will modify the damage
		return damage;
	}
}
