using Godot;
using System;

public partial class LaserBeam : RayCast2D
{
	public Line2D beam;

	public float cast_speed = 7000.0f;
	public float max_length = 1400.0f;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		beam = GetNode<Line2D>("Line2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		this.TargetPosition+=(this.TargetPosition +Vector2.Right * (this.cast_speed*((float)delta))).Clamp(Vector2.Zero,Vector2.Right*max_length*((float)delta));

		
		//beam.Points[1] = ToLocal(GetCollisionPoint());
		
    }
}
