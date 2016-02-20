using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Model
{
    /// <summary>
    /// The world class. Used to create a world object in which data is stored and the state is kept.
    /// </summary>
    public class world
    {
        public readonly int HEIGHT;
        public readonly int WIDTH;
        private Dictionary<int, cubes> foodCubes;
        private Dictionary<int, cubes> playerCubes;

        /// <summary>
        /// How many updates the server should attempt to execute per second
        /// </summary>
        public readonly int bps;

        /// <summary>
        /// How fast a cube can move
        /// </summary>
        public readonly double topSpeed;

        /// <summary>
        /// How slow a cube can move
        /// </summary>
        public readonly double lowSpeed;

        /// <summary>
        /// How fast a cube loses mass
        /// </summary>
        public readonly double attrition;

        /// <summary>
        /// Mass of food
        /// </summary>
        public readonly double foodMass;

        /// <summary>
        /// How much mass the player cube starts with
        /// </summary>
        public readonly double playerMass;

        /// <summary>
        /// How much food can be in the world
        /// </summary>
        public readonly int maxFood;

        /// <summary>
        /// How much mass the player needs to split
        /// </summary>
        public readonly double minSplitMass;

        /// <summary>
        /// How far a split cube travels
        /// </summary>
        public readonly double maxSplitDist;

        /// <summary>
        /// How many times the player can split (10-20 ideal)
        /// </summary>
        public readonly int maxSplit;

        /// <summary>
        /// Percentage of cube coverage to eat
        /// </summary>
        public readonly double absorb;

        //Temporary.  I'm assuming this is the data structure to handle splits
        private Dictionary<cubes, int> splitCubes;


        /// <summary>
        /// World Constructor.  Sets Heigt and width and creates an empty dictionary
        /// </summary>
        public world()
        {
            HEIGHT = 1000;
            WIDTH = 1000;
            foodCubes = new Dictionary<int, cubes>();        //Only one data structure for all cubes, could potentially split into two dictionaries, one for food and one for players
            playerCubes = new Dictionary<int, cubes>();
            // Readonly values
            bps = 25;
            topSpeed = 5;
            lowSpeed = 1;
            attrition = 200;
            foodMass = 1;
            playerMass = 1000;
            maxFood = 5000;
        }

        /// <summary>
        /// Used to return an IEnumerable of all current cubes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<cubes> GetFoodCubes()
        {
            foreach (cubes c in foodCubes.Values) yield return c;
        }


        /// <summary>
        /// Used to return an IEnumerable of all current cubes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<cubes> GetPlayerCubes()
        {
            foreach (cubes c in playerCubes.Values) yield return c;
        }

        /// <summary>
        /// Returns the number of players and cubes.
        /// </summary>
        /// <returns></returns>
        public int cubeCount()
        {
            return foodCubes.Count + playerCubes.Count;
        }

        /// <summary>
        /// Returns the number of players
        /// </summary>
        /// <returns></returns>
        public int playerCount()
        {
            return playerCubes.Count;
        }

        /// <summary>
        /// Addes a cube to the dictionary
        /// </summary>
        /// <param name="cube"></param>
        public void add(cubes cube)
        {
            lock (this)
            {
                if (cube.food)
                {
                    foodCubes[cube.uid] = cube;
                }
                else
                {
                    playerCubes[cube.uid] = cube;
                }
            }
        }

        /// <summary>
        /// Removes a cube from the dictionary.  Takes in a cube and removes by uid
        /// </summary>
        /// <param name="cube"></param>
        public void remove(cubes cube)
        {
            lock (this)
            {
                if (cube.food)
                {
                    foodCubes.Remove(cube.uid);
                }
                else
                {
                    playerCubes.Remove(cube.uid);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public cubes getPlayer(string name)
        {
            //int num = 0;
            //foreach(cubes c in GetCubes())
            //{
            //    if (c.Name == name)
            //    {
            //        num = c.uid;
            //        break;
            //    }
            //    else
            //    {
            //        //Not needed? If not player then move on to next cube? what happens if cube not in dictionary?
            //        num = c.uid;
            //    }
            //}

            //return foodCubes[num];
            int num = 0;

            cubes Arb = new cubes(500, 500, 219030, 834290, 834290, false, "Jose", playerMass);

            foreach (cubes c in GetPlayerCubes())
            {


                if (c.Name == name)
                {
                    num = c.uid;
                    Arb = playerCubes[num];
                }
            }

            return Arb;
        }

    }
}