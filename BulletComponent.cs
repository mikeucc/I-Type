using Godot;
using System;
using static Godot.TextServer;

public partial class BulletComponent : Node2D
{

	public float DamagePerBullet = 1, BulletsPerShot = 1, FireRate = 1;
    private float speed;
    private Vector2 direction;

    private EnemyShip parent;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        direction = new Vector2(-1.0f, 0.0f);
        speed = 300.0f;
        parent = GetParent<EnemyShip>();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Position += direction * (speed * (float)delta);
    }

    public void LeftScreen()
    {
        this.QueueFree();
    }

    public void Hit(Node2D node)
    {
        parent.hitsApplied++;
        this.QueueFree();
    }


}
