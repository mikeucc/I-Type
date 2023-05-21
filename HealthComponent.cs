using Godot;
using System;
using System.Runtime.CompilerServices;


public partial class HealthComponent : Node2D
{

	[Signal]
	public delegate void HealthChangedEventHandler(HealthUpdate healthUpdate);
	[Signal]
	public delegate void DiedEventHandler();
	


	[Export]
	public float CurrentHealth
	{
		get => currentHealth;
		private set
		{
			GD.Print("Setting Health");
			var previousHealth = CurrentHealth;
			currentHealth = Mathf.Clamp(value, 0, MaxHealth);
			var healthUpdate = new HealthUpdate
			{
				pHealth = previousHealth,
				cHealth = currentHealth,
				MaxHealth = maxHealth,
				HealthPercent = CurrentHealthPercent,
				IsHeal = previousHealth <= currentHealth
			};
			
			EmitSignal(SignalName.HealthChanged,healthUpdate);
			if(!HasHealthRemaining && !hasDied)
			{
				hasDied = true;
				EmitSignal(SignalName.Died);
			}
		}
	}

	[Export]
	private bool supressDamageFloat;

	public bool HasHealthRemaining => !Mathf.IsEqualApprox(CurrentHealth, 0f);
	public float CurrentHealthPercent=> MaxHealth >0 ? CurrentHealth/MaxHealth : 0f;
	
	[Export]
	public float MaxHealth 
	{	get => maxHealth;

		private set
		{
			maxHealth= value;
			if(CurrentHealth>maxHealth)
			{
				CurrentHealth=maxHealth;

			}
		}
	
	}

	public bool IsDamaged => currentHealth < maxHealth;
	public bool hasDied;

	private float currentHealth = 10;
	private float maxHealth = 10;
	private string currentDamageFloat; 


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void Heal(float heal)
	{
		Damage(-heal, true);
	}


	public void SetMaxHealth(float health) 
	{
		MaxHealth= health;
	}

	public void InitializeHealth()
	{
		CurrentHealth = MaxHealth;
	}

	public void ApplyScaling(Curve curve, float progress)
	{
		CallDeferred(nameof(ApplyScalingInternal), curve, progress);
	}

	public void ApplyScalingInternal(Curve curve, float progress)
	{
		var curveValue=curve.Sample(progress);
		MaxHealth *= 1f + curveValue;
		CurrentHealth = MaxHealth;
	}

	public void Damage(float damage,bool forceHideDamage=false)
	{
		GD.Print("Dealing " + damage+" Damage");
		CurrentHealth -= damage;
		if(!forceHideDamage) 
		{
			// Do stuff
		}
	}


	public void EmitHealth(HealthUpdate hu)
	{
		GD.Print(hu.pHealth);
        GD.Print(hu.cHealth);
        GD.Print(hu.HealthPercent);
        GD.Print(hu.MaxHealth);
        GD.Print(hu.IsHeal);
    }

	public void EmitDeath()
	{
		GD.Print("I have Died");
	}


	public partial class HealthUpdate : RefCounted
	{
		public float pHealth;
		public float cHealth;
		public float HealthPercent;
		public float MaxHealth;
		public bool IsHeal;
	}
	
}

