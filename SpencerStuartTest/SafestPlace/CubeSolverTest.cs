using System;
using System.IO;
using System.Linq;
using SpencerStuart.SafestPlace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpencerStuartTest.SafestPlace
{
    [TestClass]
    public class CubeSolverTest
    {
        [TestMethod]
        //Very slow besause of BF
        public void RandomCubeBFvsSmart()
        {
            Cube cube = CubeUtils.GenerateRandomCube();
            var solver = new CubeSolver(cube);
            int bfResult = solver.SolveBruteForce();
            int smartResult = solver.SolveSmart();
            Assert.AreEqual(smartResult, bfResult, CubeUtils.CubeToString(cube));
        }

        [TestMethod]
        [DeploymentItem(@"Resources\cubes.txt", "Resources")]
        [DeploymentItem(@"Resources\cubesResults.txt", "Resources")]
        public void PredefinedCubes()
        {
            int[] results = CubeUtils.SolveCubesInFile(@"Resources\cubes.txt");
            int[] expectedResults = File.ReadAllLines(@"Resources\cubesResults.txt").Select(int.Parse).ToArray();

            CollectionAssert.AreEqual(expectedResults, results);
        }
    }
}
