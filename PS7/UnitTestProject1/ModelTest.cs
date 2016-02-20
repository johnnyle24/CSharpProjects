using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Collections.Generic;

namespace MT
{
    [TestClass]
    public class ModelTest
    {
        public double x = 10.2;
        public double y = 9.3;
        public int color = 65;
        public int id = 4357;
        public int tid = 4357;
        public bool food = true;
        public string name = "Kevin Durant";
        public double mass = 1000.00;

        /// <summary>
        /// This method tests the cubes.
        /// </summary>
        [TestMethod]
        public void cubeTest()
        {
            cubes cube = new cubes(x, y, color, id, tid, food, name, mass);

            Assert.AreEqual(10.2, cube.loc_x);
            Assert.AreEqual(9.3, cube.loc_y);
            Assert.AreEqual(true, cube.food);
            Assert.AreEqual(id, cube.uid);
            Assert.AreEqual(name, cube.Name);
            Assert.AreEqual(mass, cube.Mass);
            Assert.AreEqual(color, cube.argb_color);

        }

        /// <summary>
        /// This method tests the cubes length method.
        /// </summary>
        [TestMethod]
        public void cubeTest2()
        {
            cubes cube = new cubes(x, y, color, id, tid, food, name, mass);
            double g = 89.1250938133746;
            Assert.AreEqual(g, cube.getLength());



        }
        /// <summary>
        /// This method tests the cubes.
        /// </summary>
        [TestMethod]
        public void cubeTest3()
        {
            cubes cube = new cubes(x, y, color, id, tid, food, name, mass);

            Assert.AreEqual("1000,10.2,9.3", cube.ToString());

        }

        /// <summary>
        /// This method tests the world's count.
        /// </summary>
        [TestMethod]
        public void worldTest()
        {
            world World = new world();

            for (int i = 0; i > 1000; i++)
            {
                cubes cube = new cubes(i, i + 2, i + 3, i + 4, i + 4, false, "Kevin" + i, i * i);
                World.add(cube);
            }

            List<cubes> cubeList = new List<cubes>();

            foreach (cubes c in World.GetPlayerCubes())
            {
                cubeList.Add(c);
            }

            Assert.AreEqual(1000, World.cubeCount());
        }

        /// <summary>
        /// This method tests the world's remove method.
        /// </summary>
        [TestMethod]
        public void worldTest2()
        {
            world World = new world();

            for (int i = 0; i > 1000; i++)
            {
                cubes cube = new cubes(i, i + 2, i + 3, i + 4, i + 4, false, "Kevin" + i, i * i);
                World.add(cube);
            }

            List<cubes> cubeList = new List<cubes>();

            foreach (cubes c in World.GetPlayerCubes())
            {
                cubeList.Add(c);
            }

            foreach (cubes c in cubeList)
            {
                World.remove(c);
            }

            Assert.AreEqual(0, World.cubeCount());
        }

        /// <summary>
        /// This method tests the world's getPlayer method.
        /// </summary>
        [TestMethod]
        public void worldTest3()
        {
            world World = new world();

            for (int i = 0; i > 1000; i++)
            {
                cubes ncube = new cubes(i, i + 2, i + 3, i + 4, i+4, false, "Kevin" + i, i * i);
                World.add(ncube);
            }

            List<cubes> cubeList = new List<cubes>();

            foreach (cubes c in World.GetPlayerCubes())
            {
                cubeList.Add(c);
            }

            foreach (cubes c in cubeList)
            {
                World.remove(c);
            }

            cubes cube = new cubes(5, 7 + 2, 8 + 3, 35,35, false, "KevinDurant", 9001);

            Assert.AreEqual(35, World.getPlayer("KevinDurant").uid);
        }
    }
}
