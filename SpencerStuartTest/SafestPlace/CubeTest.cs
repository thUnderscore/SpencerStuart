using System;
using System.Runtime.Remoting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpencerStuart.SafestPlace;

namespace SpencerStuartTest.SafestPlace
{
    [TestClass]
    public class CubeUtilsTest
    {
        [TestMethod]
        public void AddBombTest()
        {
            Cube cube = new Cube();

            Assert.AreEqual(Cube.DefBombsCount, cube.MaxBombsCount);
            Assert.AreEqual(Cube.DefSize, cube.Size);
            Assert.AreEqual(0, cube.CurrentBombsCount);
            cube.AddBomb(1, 2, 3);
            Assert.AreEqual(1, cube.CurrentBombsCount);
            Cube.Bomb bomb = cube[0];
            Assert.AreEqual(1, bomb.X);
            Assert.AreEqual(2, bomb.Y);
            Assert.AreEqual(3, bomb.Z);

            cube = CubeUtils.GenerateRandomCube();
            Assert.AreEqual(Cube.DefBombsCount, cube.MaxBombsCount);
            Assert.AreEqual(Cube.DefBombsCount, cube.CurrentBombsCount);
            cube.AddBomb(1, 2, 3);
            Assert.AreEqual(Cube.DefBombsCount, cube.CurrentBombsCount);
        }
    }
}
