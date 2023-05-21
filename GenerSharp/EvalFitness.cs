using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp;


    public class EvalFitness:IFitness
    {
        public EvalFitness() { }

        public double Evaluate(IChromosome chromosone)
        {
        var fc = chromosone as FloatingPointChromosome;    

            var values = fc.ToFloatingPoints();
            
            var xVel = values[0];
            var yVel = values[1];
            var fireRate = values[2];
            var fireArc = values[3];
            var fireJitter = values[4];
            var detectionRange = values[5];
            var detectionArc = values[6];


        return 0;
            //return timeSurvived + hitsApplied + (timeSurvived * 5) - hitsTaken;
        }
    }

