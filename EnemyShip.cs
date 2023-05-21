using Godot;
using System;
using System.Linq.Expressions;
using GodotUtilities.Logic;
using GodotUtilities;

public partial class EnemyShip : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

    // Genes
    [Export]
    public double xVel, yVel, fireRate, fireArc, fireJitter, detectionRange, detectionArc;

    //Fitness
    [Export]
    public float timeSurvived, didSurvive, hitsTaken, hitsApplied;


    private PathFindComponent pathFindComponent;
    private VelocityComponent velocityComponent;

    private DelegateStateMachine stateMachine = new();


    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public PackedScene enemyBullet;

    public Timer fireTimer;

    public PlayerShip player;

    [Export]
    public int enemyRef;

    

    public override void _Ready()
    {
        enemyBullet = (PackedScene)ResourceLoader.Load("res://bullet_component.tscn");
        fireTimer = GetNode<Timer>("Timer");
        hitsApplied= 0;
        try
        {
            player = GetNode<PlayerShip>("../../PlayerShip");
        }
        catch 
        {
            GD.Print("Player has already been destroyed");
        }

        pathFindComponent=GetNode<PathFindComponent>("PathFindComponent");
        velocityComponent = GetNode<VelocityComponent>("VelocityComponent");

        

        


        stateMachine.AddStates(StateNormal);
        stateMachine.SetInitialState(StateNormal);

    }

    
    public override void _PhysicsProcess(double delta)
	{


        stateMachine.Update();        
		


    }

    public void setDetectionRange()
    {
        GetNode<Area2D>("DetectionArea").Scale = new Vector2((float)detectionRange, (float)detectionRange);
    }


    public void StartFireTimer()
    {
        fireTimer.Start(fireRate);
    }

    public void Fire()
    {
        BulletComponent firedProjectile = (BulletComponent)enemyBullet.Instantiate();
        AddChild(firedProjectile);
    }

    private void StateNormal()
    {
        //velocityComponent.Velocity = new Vector2((float)xVel, (float)yVel);
        var target=player.GlobalPosition;
        pathFindComponent.SetTargetPosition(target);
        pathFindComponent.FollowPath();
        velocityComponent.Move(this);
    }


}
