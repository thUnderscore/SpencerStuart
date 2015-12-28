using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpencerStuart.SafestPlace
{
    public class Cube
    {
        public struct Bomb
        {
            public int X;
            public int Y;
            public int Z;

            public Bomb(int x, int y, int z)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
            }

            public int getSquareDistance(int x, int y, int z)
            {
                return (x - this.X) * (x - this.X) + (y - this.Y) * (y - this.Y) + (z - this.Z) * (z - this.Z);
            }            
        }
      
        public static readonly int DefSize = 1001;
        public static readonly int DefBombsCount = 200;

        public readonly int Size;
        public readonly int MaxBombsCount;
        public int CurrentBombsCount;

        internal readonly Bomb[] Bombs;

        public Bomb this[int index] => Bombs[index];

        public Cube(): this(DefSize, DefBombsCount)
        {
            
        }

        public Cube(int size, int maxBombsCount)
        {
            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            if (maxBombsCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxBombsCount));
            }
            Size = size;
            MaxBombsCount = maxBombsCount;

            CurrentBombsCount = 0;
            Bombs = new Bomb[maxBombsCount];
        }

        public void AddBomb(int x, int y, int z)
        {            
            if (CurrentBombsCount < MaxBombsCount)
            {
                Bombs[CurrentBombsCount++] = new Bomb(x, y, z);
            }
            
        }
    }
}
