using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpencerStuart.RecursiveAdding;

namespace SpencerStuartTest.RecursiveAdding
{
    [TestClass]
    public class RecursiveAddingUtilsTest
    {
        [TestMethod]
        public void EmptyArrays()
        {
            var result = RecursiveAddingUtils.AddRecursive(new byte[0], new byte[0]);
            Assert.IsNotNull(result, "Array is null");
            Assert.IsTrue(result.Length == 0, "Array is not empty");
        }

        [TestMethod]
        public void OverflowArrays()
        {
            checkArraysEqual(RecursiveAddingUtils.AddRecursive(new byte[] { 1, 1, 255 }, new byte[] { 1, 1, 255 }),
                new byte[] { 2, 2, 254 });
            checkArraysEqual(RecursiveAddingUtils.AddRecursive(new byte[] { 1, 1, 255 }, new byte[] { 0, 0, 1 }),
                new byte[] { 1, 1, 0 });
        }

        [TestMethod]
        public void PredefinedArrays()
        {
            checkArraysEqual(RecursiveAddingUtils.AddRecursive(new byte[] {1, 1, 1}, new byte[] { 1, 1, 1 }),
                new byte[] { 2, 2, 2 });

            checkArraysEqual(RecursiveAddingUtils.AddRecursive(new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 3 }),
                new byte[] { 2, 4, 6 });

            checkArraysEqual(RecursiveAddingUtils.AddRecursive(new byte[] { 1, 2, 3 }, new byte[] { 3, 2, 1 }),
                new byte[] { 4, 4, 4 });

            checkArraysEqual(RecursiveAddingUtils.AddRecursive(new byte[] { 1, 2, 3, 4 }, new byte[] { 3, 2, 1, 4}),
                new byte[] { 4, 4, 4 , 8});

        }

        [TestMethod]
        public void BigRandomArrays()
        {
            int size = 26513; //prime number

            byte[] f = new byte[size];
            byte[] s = new byte[size];
            byte[] result = new byte[size];
            var rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                rnd.NextBytes(f);
                rnd.NextBytes(s);
                for (int j = 0; j < size; j++)
                {
                    result[j] = (byte)(f[j] + s[j]);
                }
                checkArraysEqual(RecursiveAddingUtils.AddRecursive(f, s),
                    result);
            }            
        }

        private void checkArraysEqual(byte[] result, byte[] expected)
        {
            Assert.IsNotNull(result, "Array is null");
            CollectionAssert.AreEqual(expected, result, "Arrays not equal");
        }
    }
}
