using Godot;
using System;

public partial class bomb_weapon : RigidBody2D
{
	private AnimatedSprite2D animatedSprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        GD.Print("Bomb Weapon Ready");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_timer_timeout()
	{
		this.animatedSprite.Play("bombExplode");
		
		
	}

	public void _on_animated_sprite_2d_animation_finished()
	{
		this.QueueFree();
	}
}
