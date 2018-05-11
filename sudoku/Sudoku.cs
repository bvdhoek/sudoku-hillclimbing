using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku {
    class Sudoku : Climbable {

        int heuristic;
        public int[,] puzzle;
        public bool[,] unswappable;
        private int n, blockSize;
        private Random random = new Random();

        public Sudoku(int n) {
            this.n = n;
            puzzle = new int[n, n];
            unswappable = new bool[n, n];
            blockSize = (int)Math.Sqrt(n);
        }

        public IEnumerable<Climbable> Neighbours() {
            int block = random.Next(n);
            for (int i = 1; i < n; i++) {
                if (!unswappable[block, i]) {
                    for (int j = 0; j < i; j++) {
                        if (!unswappable[block, j]) {
                            Swap(block, i, j);
                            yield return this;
                            Swap(block, i, j);
                        }
                    }
                }
            }
        }

        public int HeuristicValue() {
            return 0;
        }

        public void RandomWalk(int iterations) {
            for (int i = 0; i < iterations; i++) {
                int j = random.Next(n);
                Swap(random.Next(n), j, (random.Next(n - 1) + 1 + j) % n);
            }
        }

        private void Swap(int block, int i, int j) {
            int first = puzzle[block, i];
            puzzle[block, i] = puzzle[block, j];
            puzzle[block, j] = first;
        }

        public void FillRandom() {
            for(int block = 0; block < n; block++) {
                List<int> missing = Enumerable.Range(1, n).ToList();
                for(int elem = 0; elem < n; elem++) {
                    if(puzzle[block, elem] != 0) {
                        unswappable[block, elem] = true;
                        missing.Remove(puzzle[block, elem]);
                    }
                }
                for (int elem = 0; elem < n; elem++) {
                    if (puzzle[block, elem] == 0) {
                        int r = missing[random.Next(missing.Count)];
                        puzzle[block, elem] = r;
                        missing.Remove(r);
                    }
                }
            }
        }

        public void Print() {
            String space = " ";
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    if (j == 2 || j == 5) {
                        space = "|";
                    } else {
                        space = " ";
                    }
                    Console.Write(puzzle[(blockSize * (i / blockSize) + (j / blockSize)), (blockSize * (i % blockSize) + (j % blockSize))] + space);
                }
                Console.Write("\r\n");
                if (i == 2 || i == 5) {
                    Console.WriteLine("-----+-----+-----");
                }
            }
        }
    }
}
