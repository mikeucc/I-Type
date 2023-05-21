using Godot;
using GodotUtilities;
using GodotUtilities.Logic;
using System;

[Scene]
public partial class PathFindComponent : Node2D
{

    [Node]
    public NavigationAgent2D Nav2dAgent { get; set; }
    [Node]
    private Timer intervalTimer;

    [Export]
    private VelocityComponent velocityComponent;
    [Export]
    private bool debugDrawEnabled=true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Nav2dAgent = GetNode<NavigationAgent2D>("../Nav2DAgent");
        intervalTimer = GetNode<Timer>("../IntervalTimer");
        intervalTimer.Stop();


        Nav2dAgent.Connect("velocity_computed", new Callable(this, nameof(OnVelocityComputed)));

        


    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            //WireNodes(); // this is a generated method
            
            
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        QueueRedraw();
        Nav2dAgent.SetVelocity(this.velocityComponent.Velocity);
    }


    public override void _Draw()
    {
        if (debugDrawEnabled)
        {
            for (int i = 0; i < Nav2dAgent.GetCurrentNavigationPath().Length; i++)
            {
                var point = Nav2dAgent.GetCurrentNavigationPath()[i];
                DrawCircle(ToLocal(point), 3f, Colors.Orange);
                if (i > 0)
                {
                    var previousPoint = Nav2dAgent.GetCurrentNavigationPath()[i - 1];
                    DrawLine(ToLocal(previousPoint), ToLocal(point), Colors.Orange, 2f);
                }
            }
        }
    }

    public void SetTargetPosition(Vector2 targetPosition)
    {
        /*
        if (!intervalTimer.IsStopped())
        {
            GD.Print("No position set");
            return;
        }
        */

        intervalTimer.Call("start");
        Nav2dAgent.TargetPosition = targetPosition;
    }

    public void ForceTargetPosition(Vector2 targetPosition)
    {
        Nav2dAgent.TargetPosition=targetPosition;
        intervalTimer.Call("start");
    }

    public void FollowPath()
    {
        if (Nav2dAgent.IsNavigationFinished())
        {
            velocityComponent.Decelerate();
            return;
        }

        var direction = (Nav2dAgent.GetNextPathPosition() - GlobalPosition).Normalized();
        velocityComponent.AccelerateInDirection(direction);
        Nav2dAgent.SetVelocity(velocityComponent.Velocity);

    }

    private void OnVelocityComputed(Vector2 velocity)
    {
        var newDirection=velocity.Normalized();
        var currentDirection=velocityComponent.Velocity.Normalized();
        var halfway = newDirection.Lerp(currentDirection, 1f - Mathf.Exp(velocityComponent.AccelerationCoefficient));
        GD.Print("OnVelocity Computed");
    }
}
