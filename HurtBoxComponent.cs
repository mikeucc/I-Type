using Godot;
using System;
using GodotUtilities;

public partial class HurtBoxComponent : Area2D
{
	
	public const string GROUP_ENEMY_HURTBOX = "enemy_hitbox";


	[Signal]
	public delegate void HitByBulletEventHandler(BulletHitContext bulletHitContext);
	[Signal]
	public delegate void HitByHitboxEventHandler(HitBoxComponent hitboxComponent);


	[Export]
	private HealthComponent healthComponent;
	[Export]
	private StatusReceiverComponent statusReceiverComponent;
	[Export]
	private PackedScene bulletImpactScene;
	[Export]
	private bool detectOnly=false;


    


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public bool CanAcceptBulletCollision()
	{
		return healthComponent?.HasHealthRemaining ?? true;
	}

	
	 

	public void HandleBulletCollision(BulletComponent bullet)
	{
		
		
		
		var damage = 0f;

		if(!detectOnly)
		{
			damage = bullet.DamagePerBullet;
			damage=DealDamageWithTransForms(damage);
		}

		EmitSignal(SignalName.HitByBullet,new BulletHitContext
		{ 
			BulletComponent= bullet,
			TransformedDamage=damage
		});
	}

	

	private float DealDamageWithTransForms(float damage)
	{
		var finalDamage =statusReceiverComponent.ApplyDamageTransformation(damage);
		healthComponent.Damage(finalDamage);
		return finalDamage;
	}

	private void OnAreaEntered(Area2D otherArea)
	{
              
        if (otherArea is HitBoxComponent hitboxComponent)
		{
			if (hitboxComponent.GetParent() is BulletComponent)
			{
				GD.Print("Bullet Hit");
				EmitSignal(SignalName.HitByBullet, new BulletHitContext
				{
					BulletComponent = hitboxComponent.GetParent<BulletComponent>(),
					TransformedDamage = DealDamageWithTransForms(hitboxComponent.GetParent<BulletComponent>().DamagePerBullet)
				});
                
            }

			else
			{

				if (!detectOnly)
				{
					GD.Print(hitboxComponent.damage);
					DealDamageWithTransForms(hitboxComponent.damage);
				}
				EmitSignal(SignalName.HitByHitbox, hitboxComponent);
			}
			
		}
		
        
    }


	public void EmitHitByHit(HitBoxComponent hbc)
	{
        //Placeholder for handeleing the emited signal
        GD.Print("Hit by another Hitbox");
	}

    public void EmitHitByBullet(BulletHitContext hbc)
    {
        //Placeholder for handeleing the emited signal
		GD.Print("Hit by a Bullet");
		hbc.BulletComponent.QueueFree();
    }


    public partial class BulletHitContext:RefCounted
	{
		public BulletComponent BulletComponent;
		public float TransformedDamage;
	}

	
}
