using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpencerStuart.SafestPlace
{
    public static class CubeUtils
    {
        public static readonly char CubeFileSeparator = ' ';

        public static Cube GenerateRandomCube()
        {
            var cube = new Cube();

            var rnd = new Random();
            while (cube.CurrentBombsCount < cube.MaxBombsCount)
            {
                cube.AddBomb(rnd.Next(cube.Size), rnd.Next(cube.Size), rnd.Next(cube.Size));
            }

            return cube;
        }

        public static string CubeToString(Cube cube)
        {
            var sb = new StringBuilder(3 * cube.CurrentBombsCount * (Convert.ToInt32(Math.Ceiling(Math.Log10(cube.Size)))) +
                (Convert.ToInt32(Math.Ceiling(Math.Log10(cube.MaxBombsCount) + 1))));

            sb.Append(cube.MaxBombsCount);            
            for (int i = 0; i < cube.CurrentBombsCount; i++)
            {
                Cube.Bomb bomb = cube[i];
                sb.Append(CubeFileSeparator);
                sb.Append(bomb.X);
                sb.Append(CubeFileSeparator);
                sb.Append(bomb.Y);
                sb.Append(CubeFileSeparator);
                sb.Append(bomb.Z);
            }
            
            return sb.ToString();
        }

        public static Cube StringToCube(String str, int size)
        {
            if (String.IsNullOrEmpty(str))
            {
                throw new ArgumentException(nameof(str));
            }

            int[] tokens = str.TrimEnd().Split(' ').Select(int.Parse).ToArray();
            int maxBombsCount = tokens[0];
            if (tokens.Length != 3 * maxBombsCount + 1)
            {
                throw new Exception("Wrong file content");
            }

            var cube = new Cube(size, maxBombsCount);
            for (int ptr = 1; ptr < tokens.Length; ptr += 3)
            {
                cube.AddBomb(tokens[ptr], tokens[ptr + 1], tokens[ptr + 2]);
            }            
            return cube;
        }

        public static void GenerateCubesWithBruteForceSolution(int cubesCount, int size, int bombsCount, string cubesFileName, string resultsFileName)
        {
            using (var cubesWriter = new StreamWriter(cubesFileName))
            using (var resultsWriter = new StreamWriter(resultsFileName))
            {
                cubesWriter.WriteLine(cubesCount);
                for (int i = 0; i < cubesCount; i++)
                {
                    var cube = new Cube(size, bombsCount);                    
                    int distance = new CubeSolver(cube).SolveBruteForce();
                    cubesWriter.WriteLine(CubeToString(cube));
                    resultsWriter.WriteLine(distance);
                }
            }
        }

        public static int[] SolveCubesInFile(string cubesFileName)
        {
            return SolveCubesInFile(Cube.DefSize, cubesFileName);
        }

        public static int[] SolveCubesInFile(int cubeSize, string cubesFileName)
        {

            int[] result = null;
            Cube[] cubes;
            using (var reader = new StreamReader(cubesFileName))
            {
                int cubesCount = 0;
                if (!Int32.TryParse(reader.ReadLine(), out cubesCount))
                {
                    throw new Exception("Wrong file content");
                }
                cubes = new Cube[cubesCount];
                for (int i = 0; i < cubesCount; i++)
                {
                    if (reader.EndOfStream)
                    {
                        throw new Exception("Wrong file content");
                    }
                    cubes[i] = StringToCube(reader.ReadLine(), cubeSize);
                }                
            }
            result = new int[cubes.Length];
            for (int i = 0; i < cubes.Length; i++)
            {
                result[i] = new CubeSolver(cubes[i]).SolveSmart();
            }
            return result;
        }
    }
}
