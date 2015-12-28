using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using static SpencerStuart.Stairs.Runner;

namespace SpencerStuartTest.Stairs
{
    [TestClass]
    public class RunnerTest
    {
        [TestMethod]
        public void WrongFlightsArray()
        {
            AreEqual(-1, GetStridesCount(null, MinStepsPerStride)) ;
            AreEqual(-1, GetStridesCount(new int[MinFlights - 1], MinStepsPerStride));
            AreEqual(-1, GetStridesCount(new int[MaxFlights + 1], MaxStepsPerStride));
        }

        [TestMethod]
        public void WrongFlight()
        {
            AreEqual(-1, GetStridesCount(new[] { MinStepsPerFlight - 1 }, MinStepsPerStride));
            AreEqual(-1, GetStridesCount(new[] { MaxStepsPerFlight + 1 }, MaxStepsPerStride));
        }

        [TestMethod]
        public void WrongStepsPerStride()
        {
            AreEqual(-1, GetStridesCount(new [] {5}, MinStepsPerStride - 1));
            AreEqual(-1, GetStridesCount(new[] { 5 }, MaxStepsPerStride + 1));
        }

        [TestMethod]
        public void CorrectData()
        {
            AreEqual(8, GetStridesCount(new[] { 15 }, 2));
            AreEqual(8, GetStridesCount(new[] { 16 }, 2));
            AreEqual(9, GetStridesCount(new[] { 17 }, 2));
            AreEqual(18, GetStridesCount(new[] { 15, 15 }, 2));
            AreEqual(18, GetStridesCount(new[] { 16, 16 }, 2));
            AreEqual(20, GetStridesCount(new[] { 17, 17 }, 2));
            AreEqual(44, GetStridesCount(new[] { 5, 11, 9, 13, 8, 30, 14 }, 3));
        }

    }
}
