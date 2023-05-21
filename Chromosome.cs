using Godot;
using System;
using GeneticSharp;
using System.Linq;

public partial class Chromosome : Node
{
    // Called when the node enters the scene tree for the first time.

    // Genes
    [Export]
    private double xVel, yVel, fireRate, fireArc, fireJitter, detectionRange, detectionArc;

    //Fitness
    [Export]
    public float timeSurvived, didSurvive, hitsTaken, hitsApplied;

    public GeneticAlgorithm ga;

    public PackedScene enemyShipScene;

    private EnemyShip[] enemyShips=new EnemyShip[10];


    public override void _Ready()
	{
        enemyShipScene = (PackedScene)ResourceLoader.Load("res://EnemyShip.tscn");

        var fpChromosome = new FloatingPointChromosomeGodot(
            new double[] { 0, 0, 0, 0, 0, 0, 0 },
            new double[] { xVel, yVel, fireRate, fireArc, fireJitter, detectionRange, detectionArc },
            new int[] { 8, 8, 8, 8, 8, 8, 8 },
            new int[] { 0, 0, 0, 0, 0, 0, 0 });
        var pop = new PopulationGodot(10, 10, fpChromosome);
        var selection = new EliteSelection();
        var mutation = new FlipBitMutation();
        var crossover = new UniformCrossover(0.5f);
        var termination = new GenerationNumberTermination(1);

        var fitness = new FuncFitness((c) =>
        {
        var fc = c as FloatingPointChromosomeGodot;

            /*try
            {
                GD.Print("Hits applied by ship:" + enemyShips[fc.associatedEnemy].hitsApplied);
            }
            catch
            {
                GD.Print("enemy ship array empty");
            }
            */


            if (enemyShips[fc.associatedEnemy] is null)
                return 0;

            GD.Print(enemyShips[fc.associatedEnemy]);

            GD.Print(enemyShips[fc.associatedEnemy].hitsApplied);

            return timeSurvived + didSurvive - hitsTaken + hitsApplied;
        });




        ga = new GeneticAlgorithm(
            pop, fitness, selection, crossover, mutation);
        ga.Termination = termination;

        
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


    

    public void Mutate()
    {
        xVel = 0.0f;
        yVel = 0.0f;
        fireRate = 0.0f;
        fireArc = 0.0f;
        fireJitter = 0.0f;
        detectionRange = 0.0f;
        detectionArc = 0.0f;

    }

    public float[] ProduceOffSpring()
    {
        return new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
    }



    public void evolvePopulation()
    {
        
        foreach(Node ships in this.GetChildren())
        {
            if(ships is EnemyShip)
            ships.QueueFree();
        }

        
        ga.Start();

        var bestFitness = ga.BestChromosome.Fitness.Value;

        //enemyShips = null;

        int i = 0;

        foreach (FloatingPointChromosomeGodot fpg in ga.Population.CurrentGeneration.Chromosomes)
        {
            EnemyShip enemy = (EnemyShip)enemyShipScene.Instantiate();
            AddChild(enemy);

            

            enemy.Position = new Vector2((float)GD.RandRange(600, 1600), (float)GD.RandRange(200, 800));

            

            var values = fpg.ToFloatingPoints();

            enemy.xVel = values[0];
            enemy.yVel = values[1];
            enemy.fireRate = values[2];
            enemy.fireArc = values[3];
            enemy.fireJitter = values[4];
            enemy.detectionRange = values[5];
            enemy.detectionArc = values[6];
            enemy.enemyRef = i;

            enemy.setDetectionRange();
            enemy.StartFireTimer();

            i++;
 

        }

        foreach (Node ships in this.GetChildren())
        {
            if (ships is EnemyShip)
            {
                EnemyShip ship = (EnemyShip)ships;
                enemyShips[ship.enemyRef] = ship;
            }
        }




    }

}
