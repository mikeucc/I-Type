using Godot;
using System;
using System.Data;

public partial class charging_laser_beam : RayCast2D
{
	[Export]
	public float cast_speed = 7000.0f;
    [Export]
    public float max_length = 1400.0f;
    [Export]
    public float growth_time = 0.1f;

	public GpuParticles2D casting_partille, beam_particle, collision_particle;
	public Line2D line_2d;

	public Tween tween;

	public bool isCasting=false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		casting_partille = GetNode<GpuParticles2D>("CastingParticle2D");
        beam_particle= GetNode<GpuParticles2D>("BeamParticle2D");
        collision_particle = GetNode<GpuParticles2D>("CollisionParticle2D");
		line_2d = GetNode<Line2D>("Line2D");

		this.SetPhysicsProcess(false);
		line_2d.Points[1]=Vector2.Zero;
		
        tween = GetTree().CreateTween();
		tween.Stop();

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.TargetPosition = TargetPosition + Vector2.Right * cast_speed * ((float)delta);
		if (TargetPosition.X>max_length)
		{
			TargetPosition=new Vector2 { X=max_length,Y=TargetPosition.Y};
		}

		CastBeam();
		
	}

	public void SetIsCasting(bool firing)
	{
		this.isCasting = firing;

		if(isCasting)
		{
			TargetPosition = Vector2.Zero;
			line_2d.Points[1] = TargetPosition;
			Appear();
		}
		else
		{
			collision_particle.Emitting= false;
			Disappear();
		}

		this.SetPhysicsProcess(isCasting);
		beam_particle.Emitting = isCasting;
		casting_partille.Emitting = isCasting;
	}

	public void CastBeam()
	{
		Vector2 castPoint=this.TargetPosition;

		ForceRaycastUpdate();

		collision_particle.Emitting = this.IsColliding();

		if(this.IsColliding() )
		{
			castPoint = ToLocal(GetCollisionPoint());
			collision_particle.GlobalRotation = GetCollisionNormal().Angle();
			collision_particle.Position= castPoint;
		}

		line_2d.Points[1] = castPoint;
		beam_particle.Position = castPoint * 0.5f;
		ParticleProcessMaterial mat = (ParticleProcessMaterial)beam_particle.ProcessMaterial;
		mat.EmissionBoxExtents=new Vector3 { X=(castPoint.Length() * 0.5f), Y=mat.EmissionBoxExtents.Y , Z=0.0f};

		GD.Print(castPoint);

	}

	public void Appear()
	{
		if(tween.IsRunning())
		{
			tween.Stop();
		}

		tween.TweenProperty(line_2d, "width", 0, growth_time * 2);
		tween.Play();


	}

	public void Disappear()
	{
        if (tween.IsRunning())
        {
            tween.Stop();
        }

        tween.TweenProperty(line_2d, "width", line_2d.Width, growth_time * 2);
        tween.Play();
    }
}
