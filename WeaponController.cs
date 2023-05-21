using Godot;
using System;

public partial class WeaponController : Node
{
    //public GameObject shot;
    //public Transform shotSpawn;
    [Export]
    public Vector2 fireRate=new(0,10);

    public float delay;



    private int nbShots;
    private int nbShotsOnTarget;

    private float accuracy; // Accuracy between 0 and 1

    private RandomNumberGenerator rng = new RandomNumberGenerator();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        float myFireRate = rng.RandfRange(fireRate.X, fireRate.Y);
        nbShots = 0;
        nbShotsOnTarget = 0;
        
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}



    /// <summary>
    /// Fire a projectile.
    /// </summary>
    public void Fire()
    {
       nbShots++;

           
        
    }

    public void OnTargetShot()
    {
        nbShotsOnTarget++;

        accuracy = nbShotsOnTarget / (float)nbShots;
        //Debug.Log("nbShotsOnTarget = " + nbShotsOnTarget + " | nbShots = " + nbShots + "\nShot on target, new accuracy = " + accuracy);
    }

    /// <summary>
    /// Accessor for the accuracy attribute.
    /// </summary>
    /// <returns>The value of accuracy.</returns>
    public float GetAccuracy()
    {
        return accuracy;
    }

    /// <summary>
    /// Accessor for the nbShotsOnTarget attribute.
    /// </summary>
    /// <returns>The value of nbShotsOnTarget</returns>
    public int GetNbShotsOnTarget()
    {
        return nbShotsOnTarget;
    }
}
