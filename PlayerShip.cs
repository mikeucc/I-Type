using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class PlayerShip : Godot.CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	public GpuParticles2D chargeParticle;
	public Line2D line;
	public AnimationPlayer chargePlayer;
	public PackedScene bombScene;

	public bool laserFired=false;
	public Timer laserTimer;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();



    public override void _Ready()
    {
		bombScene = (PackedScene)ResourceLoader.Load("res://bomb_weapon.tscn");
		chargeParticle = GetNode<GpuParticles2D>("ChargeWeapon");
		chargePlayer = GetNode<AnimationPlayer>("ChargeWeapon/ChargeAnimation");
		line=GetNode<Line2D>("ChargeWeapon/Line2D");
		line.Visible = false;

		laserTimer = GetNode<Timer>("ChargeWeapon/Line2D/LaserTimer");
		
		
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		

			// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept"))
		{
			GD.Print("Release Bomb");
			bomb_weapon bomb = (bomb_weapon)bombScene.Instantiate();
			AddChild(bomb);
				
		}

		if(Input.IsActionJustPressed("charge"))
		{
			chargeParticle.Emitting = true;
			chargePlayer.Play("chargeBeam");

			
		}

        if (Input.IsActionJustReleased("charge"))
        {
            chargeParticle.Emitting = false;
			laserFired= true;
			line.Visible = true;
            laserTimer.Start();


        }

		if (laserFired)
		{
			line.Position=new Vector2
			{
				X = (float)(line.Position.X+Speed*delta*10),
				Y = (float)(line.Position.Y)
			};
			
		}

		

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;

			


		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X,0, Speed);
			velocity.Y=  Mathf.MoveToward(Velocity.Y,0, Speed);
        }

		Velocity = velocity;
		
		MoveAndSlide();

		Position = new Vector2(Mathf.Clamp(Position.X, 100, 1600), (Mathf.Clamp(Position.Y, 100, 1000)));

    }


	public void LaserFinished()
	{
		laserFired = false;
		line.Visible = false;

        line.Position = new Vector2
        {
            X = 6,
            Y = 0
        };
    }

	public void Destroyed()
	{
		this.QueueFree();
	}
}
