using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.GeneticAlgorithm
{
    public class GA_TSP
    {
        protected int cityCount;
        protected int populationSize;
        protected double mutationPercent;
        protected int matingPopulationSize;
        protected int favoredPopulationSize;
        protected int cutLength;
        protected int generation;
        protected Thread worker = null;
        protected bool started = false;
        protected City[] cities;
        protected Chromosome[] chromosomes;
        private string status = "";

        public GA_TSP()
        {
        }

        public void Initialization()
        {
            Random randObj = new Random();
            try
            {
                cityCount = 40;
                populationSize = 1000;
                mutationPercent = 0.05;
            }
            catch (Exception e)
            {
                cityCount = 100;
            }

            matingPopulationSize = populationSize / 2;
            favoredPopulationSize = matingPopulationSize / 2;
            cutLength = cityCount / 5;

            // create a random list of cities
            cities = new City[cityCount];

            for (int i = 0; i < cityCount; i++)
            {
                cities[i] =
                    new City((int)(randObj.NextDouble() * 30),
                        (int)(randObj.NextDouble() * 15));
            }

            // create the initial chromosomes
            chromosomes = new Chromosome[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                chromosomes[i] = new Chromosome(cities);

                chromosomes[i].assignCut(cutLength);

                chromosomes[i].assignMutation(mutationPercent);
            }

            Chromosome.sortChromosomes(chromosomes, populationSize);

            started = true;

            generation = 0;
        }

        public void TSPCompute()
        {
            double thisCost = 500.0;

            double oldCost = 0.0;

            double dcost = 500.0;

            int countSame = 0;

            Random randObj = new Random();

            while (countSame < 120)
            {
                generation++;

                int ioffset = matingPopulationSize;

                int mutated = 0;

                for (int i = 0; i < favoredPopulationSize; i++)
                {
                    Chromosome cmother = chromosomes[i];

                    int father =
                        (
                        int
                        )(randObj.NextDouble() * (double)matingPopulationSize);

                    Chromosome cfather = chromosomes[father];

                    mutated +=
                        cmother
                            .mate(cfather,
                            chromosomes[ioffset],
                            chromosomes[ioffset + 1]);

                    ioffset += 2;
                }

                for (int i = 0; i < matingPopulationSize; i++)
                {
                    chromosomes[i] = chromosomes[i + matingPopulationSize];

                    chromosomes[i].calculateCost(cities);
                }

                // Now sort the new population
                Chromosome.sortChromosomes(chromosomes, matingPopulationSize);

                double cost = chromosomes[0].getCost();

                dcost = Math.Abs(cost - thisCost);

                thisCost = cost;

                double mutationRate =
                    100.0 * (double)mutated / (double)matingPopulationSize;

                System
                    .Console
                    .WriteLine("Generation = " +
                    generation.ToString() +
                    " Cost = " +
                    thisCost.ToString() +
                    " Mutated Rate = " +
                    mutationRate.ToString() +
                    "%");

                if ((int)thisCost == (int)oldCost)
                {
                    countSame++;
                }
                else
                {
                    countSame = 0;

                    oldCost = thisCost;

                    //System.Console.WriteLine("oldCost = " + oldCost.ToString());
                }
            }

            for (int i = 0; i < cities.Length; i++)
            {
                chromosomes[i].PrintCity(i, cities);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //
            // TODO: Add code to start application here
            //
            GA_TSP tsp = new GA_TSP();

            tsp.Initialization();

            tsp.TSPCompute();
        }
    }

    public class City
    {
        public City()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        // The city's x coordinate.
        private int xcoord;

        // The city's y coordinate.
        private int ycoord;

        // x -- the city's x coordinate
        // y -- the city's y coordinate
        public City(int x, int y)
        {
            xcoord = x;

            ycoord = y;
        }

        public int getx()
        {
            return xcoord;
        }

        public int gety()
        {
            return ycoord;
        }

        // Returns the distance from the city to another city.
        public int proximity(City cother)
        {
            return proximity(cother.getx(), cother.gety());
        }

        // x -- the x coordinate
        // y -- the y coordinate
        // return The distance.
        public int proximity(int x, int y)
        {
            int xdiff = xcoord - x;

            int ydiff = ycoord - y;

            return (int)Math.Sqrt(xdiff * xdiff + ydiff * ydiff);
        }
    }
}
