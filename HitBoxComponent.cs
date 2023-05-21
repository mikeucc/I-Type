using Godot;
using System;

public partial class HitBoxComponent : Area2D
{
	[Export]
	public float damage = 1;
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Hit Box does not require a mask
		//this.CollisionMask = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
