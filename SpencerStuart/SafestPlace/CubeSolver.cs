using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpencerStuart.SafestPlace
{
    public class CubeSolver
    {
        private readonly Cube _cube;
        private int[] _segments;

        private readonly int _size;
        private readonly int _radiusCap;
        

        public CubeSolver(Cube cube)
        {
            if (cube == null)
            {
                throw  new ArgumentNullException(nameof(cube));
            }
            _cube = cube;
            _size = _cube.Size;
            _radiusCap = 3 * (_size - 1) * (_size - 1) + 1;            
        }

        //Solves task using BF approach
        public int SolveBruteForce(int threadsCount = 8)
        {            
            int[] from = new int[threadsCount];
            int[] to = new int[threadsCount];
            int[] results = new int[threadsCount];
            var tasks = new Task[threadsCount];
            int ptr = 0;
            int perThread = _size / threadsCount;
            //split cube on slices by X coordinate.
            //solve each part in different thread
            for (int i = 0; i < threadsCount; i++)
            {
                results[i] = 0;
                from[i] = ptr;
                ptr = (i == threadsCount - 1) ? _size : ptr + perThread;
                to[i] = ptr;
                tasks[i] = Task.Factory.StartNew((obj) =>
                {
                    int index = (int)obj;

                    for (int x = from[index]; x < to[index]; x++)
                    {
                        for (int y = 0; y < _size; y++)
                        {
                            for (int z = 0; z < _size; z++)
                            {
                                int pointResult = _radiusCap;
                                for (int p = 0; p < _cube.CurrentBombsCount; p++)
                                {
                                    int dist = _cube[p].getSquareDistance(x, y, z);
                                    if (dist < pointResult)
                                    {
                                        pointResult = dist;
                                    }
                                    if (pointResult < results[index])
                                    {
                                        break;
                                    }
                                }
                                if (pointResult > results[index])
                                {
                                    results[index] = pointResult;
                                }
                            }
                        }
                    }

                }, i);
            }
            //wait for all threads
            Task.WaitAll(tasks);
            //max value among all slices is result
            return results.Max();
        }

        //Solves task using more smarter than BF approach
        public int SolveSmart()
        {
            int result = 0;
            //contains intersections of blasts and ZColumn
            _segments = new int[2 * _cube.CurrentBombsCount];
            try
            {                
                //iterate all ZColumns
                for (int x = 0; x < _size; x++)
                {
                    for (int y = 0; y < _size; y++)
                    {
                        //if column is covered by blast - skip it
                        if (IsZColumnCoveredByBlast(x, y, result))
                        {
                            continue;
                        }
                        //else try BS approach to find radius  of blast which covers column
                        //range of BS: from already found radius to max possible
                        int left = result;
                        int right = _radiusCap;
                        while (right - left > 1)
                        {
                            int probeRadius = (left + right)/2;
                            bool all = IsZColumnCoveredByBlast(x, y, probeRadius);
                            if (all)
                            {
                                right = probeRadius;
                            }
                            else
                            {
                                left = probeRadius;
                            }

                        }
                        result = Math.Max(result, right);
                    }
                }
            }
            finally
            {
                _segments = null;
            }
            return result;
        }

        //Returns true if Z column with coordinates (x, y) would be covered by blast with radius
        private bool IsZColumnCoveredByBlast(int x, int y, int radius)
        {
            int segmentEndsCount = 0;
            
            for (int i = 0; i < _cube.CurrentBombsCount; i++)
            {
                Cube.Bomb bomb = _cube[i];
                int maxSquaredZ = radius - (x - bomb.X) * (x - bomb.X) - (y - bomb.Y) * (y - bomb.Y);
                //if bomb is inside cylinder (x,y, radius)
                if (maxSquaredZ >= 0)
                {
                    //first estimation of intersection of blast and ZColumn
                    int deltaZ = (int)Math.Floor(Math.Sqrt(maxSquaredZ));
                    //adjust
                    while ((deltaZ)*(deltaZ) <= maxSquaredZ)
                    {
                        deltaZ++;
                    }
                    //coordinates of blast intersection
                    //left is inside, right !!!!!outside - for
                    //(1, 2)_(3, 4) situation
                    //if right is not outside, intersection is missed
                    int minZ = Math.Max(bomb.Z - deltaZ + 1, 0);
                    int maxZ = Math.Min(bomb.Z + deltaZ /*- 1*/, _size /*- 1*/);

                    //preparation for Klee. Scale for possible start\finish overlap
                    _segments[segmentEndsCount++] = 2 * minZ;
                    _segments[segmentEndsCount++] = 2 * maxZ + 1;                    

                }
            }
            if (segmentEndsCount == 0)
            {
                //no segments - no covering
                return false;
            }

            //Klee
            Array.Sort(_segments, 0, segmentEndsCount);
            //Fast check if ends of ZColumn uncovered
            if ((_segments[segmentEndsCount - 1] != (2 * (_size /*- 1*/) + 1)) ||
                (_segments[0] != 0))
            {                
                return false;
            }
            int ovelapsCounter = 0;
            for (int i = 0; i < segmentEndsCount - 1; i++)
            {
                if (_segments[i] % 2 == 0)
                {
                    ovelapsCounter++;
                } else
                {
                    ovelapsCounter--;
                }

                if (ovelapsCounter == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }            
}
