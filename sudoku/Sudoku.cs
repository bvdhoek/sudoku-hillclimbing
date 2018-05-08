using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku
{
    class Sudoku : Climbable
    {
        public struct Block
        {
            public int xMin, yMin, xMax, yMax;
            public Block(int xMin, int yMin, int xMax, int yMax)
            {
                this.xMin = xMin; this.xMax = xMax; this.yMin = yMin; this.yMax = yMax;
            }
        }

        public int[,] puzzle;
        private int n, blockSize;
        private Random random = new Random();

        public Sudoku(int n)
        {
            this.n = n;
            puzzle = new int[n, n];
            blockSize = (int)Math.Sqrt(n);
        }

        public IEnumerable<Climbable> Neighbours()
        {
            Block block = RandomBlock();
            for (int x1 = block.xMin; x1 <= block.xMax; x1++)
            {
                for (int y1 = block.yMin; y1 <= block.yMax; y1++)
                {
                    for (int x2 = block.xMin; x2 <= block.xMax; x1++)
                    {
                        for (int y2 = block.yMin; y2 <= block.yMax; y1++)
                        {
                            Swap(x1, y1, x2, y2);
                            yield return this;
                            Swap(x1, y1, x2, y2);
                        }
                    }
                }
            }
        }

        public int HeuristicValue()
        {
            return 0;
        }

        public void RandomWalk(int iterations)
        {
            for (int i = 0; i < iterations; i++)
                RandomSwap(RandomBlock());
        }

        private void Swap(int x1, int y1, int x2, int y2)
        {
            int first = puzzle[x1, y1];
            puzzle[x1, y1] = puzzle[x2, y2];
            puzzle[x2, y2] = first;
        }

        private void fillRandom()
        {

        }

        public Block RandomBlock()
        {
            int xMin = random.Next(0, 3) * 3;
            int yMin = random.Next(0, 3) * 3;
            return new Block(xMin, xMin + blockSize - 1, yMin, yMin + blockSize - 1);
        }

        public void RandomSwap(Block block)
        {
            int x1, y1, x2, y2;
            x1 = random.Next(block.xMin, block.xMax);
            y1 = random.Next(block.yMin, block.yMax);
            do
            {
                x2 = random.Next(block.xMin, block.xMax);
                y2 = random.Next(block.yMin, block.yMax);
            } while (x1 == x2 && y1 == y2);
            Swap(x1, y1, x2, y2);
        }

        public void Print()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(puzzle[i, j] + " ");
                }
                Console.Write("\r\n");
            }
        }
    }
}
