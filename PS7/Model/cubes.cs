//Cody Ngo, Johhny Le  11/7/15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;

namespace Model
{
    /// <summary>
    /// 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class cubes
    {
        /// <summary>
        /// Unique id for the cubes
        /// </summary>
        [JsonProperty(PropertyName = "uid")]
        public int uid { get; set; }

        /// <summary>
        /// unique team id for splitting
        /// </summary>
        [JsonProperty(PropertyName = "Team id")]
        public int team_id { get; set; }

        /// <summary>
        /// Stores what color a cube is
        /// </summary>
        [JsonProperty(PropertyName = "argb_color")]
        public int argb_color { get; set; }

        /// <summary>
        /// x coordinates of cube
        /// </summary>
        [JsonProperty(PropertyName = "loc_x")]
        public double loc_x { get; set; }

        /// <summary>
        /// y coordinates of cube
        /// </summary>
        [JsonProperty(PropertyName = "loc_y")]
        public double loc_y { get; set; }

        /// <summary>
        /// Stores the mass of the cube
        /// </summary>
        [JsonProperty(PropertyName = "Mass")]
        public double Mass { get; set; }

        /// <summary>
        /// Name of player's cube
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// True if cube is food, false if cube is player
        /// </summary>
        [JsonProperty(PropertyName = "food")]
        public bool food { get; set; }

        /// <summary>
        /// Stores the length of one side of the cube
        /// </summary>
        public double length { get; set; }

        private double momentum;

        /// <summary>
        /// Constructor for player cube
        /// </summary>
        [JsonConstructor]
        public cubes(double x, double y, int color, int id, int team, bool _food, string _name, double _mass)
        {
            loc_x = x;
            loc_y = y;
            argb_color = color;
            uid = id;
            food = _food;
            Name = _name;
            Mass = _mass;
            team_id = team;
            //missing team id?
        }

        /// <summary>
        /// Returns length of cube
        /// </summary>
        /// <returns></returns>
        public double getLength()
        {
            length = Math.Sqrt(Mass);
            return length;
        }

        /// <summary>
        /// Nice to string method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Mass + "," + loc_x + "," + loc_y;        //What would you need this for
        }

        public void apply_momentum()
        {
            //TODO: I don't know
        }

    }
}