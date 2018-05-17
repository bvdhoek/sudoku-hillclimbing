using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku {
    class Sudoku : Climbable {

        int totalHeuristic;
        int[] rowHeuristic, columnHeuristic;
        public int[,] puzzle;       //(Row, Column) based
        public bool[,] unswappable; //(Block, Elem) based
        private int n, blockSize;
        private Random random = new Random();

        public Sudoku(int n) {
            this.n = n;
            rowHeuristic = new int[n];
            columnHeuristic = new int[n];
            puzzle = new int[n, n];
            unswappable = new bool[n, n];
            blockSize = (int)Math.Sqrt(n);
        }

        public Sudoku Copy() {
            Sudoku copy = new Sudoku(n);
            copy.totalHeuristic = totalHeuristic;
            copy.rowHeuristic = rowHeuristic.Clone() as int[];
            copy.columnHeuristic = columnHeuristic.Clone() as int[];
            copy.puzzle = puzzle.Clone() as int[,];
            copy.unswappable = unswappable.Clone() as bool[,];
            return copy;
        }

        public IEnumerable<Climbable> Neighbours() {
            int block = random.Next(n);
            for (int elem1 = 1; elem1 < n; elem1++) {
                if (!unswappable[block, elem1]) {
                    for (int elem2 = 0; elem2 < elem1; elem2++) {
                        if (!unswappable[block, elem2]) {
                            Swap(block, elem1, elem2);
                            yield return this;
                            Swap(block, elem1, elem2);
                        }
                    }
                }
            }
        }

        public int HeuristicValue() {
            return totalHeuristic;
        }

        public void RandomWalk(int iterations) {
            for (int i = 0; i < iterations; i++) {
                int j = random.Next(n);
                Swap(random.Next(n), j, (random.Next(n - 1) + 1 + j) % n);
            }
        }

        private void Swap(int block, int elem1, int elem2) {
            int i1 = blockSize * (block / blockSize) + (elem1 / blockSize);
            int j1 = blockSize * (block % blockSize) + (elem1 % blockSize);
            int i2 = blockSize * (block / blockSize) + (elem2 / blockSize);
            int j2 = blockSize * (block % blockSize) + (elem2 % blockSize);

            int first = puzzle[i1, j1];
            puzzle[i1, j1] = puzzle[i2, j2];
            puzzle[i2, j2] = first;
            
            List<int> missing;
            if (i1 != i2) {
                missing = Enumerable.Range(1, n).ToList();
                for (int x = 0; x < n; x++) {
                    if (missing.Contains(puzzle[i1, x])) {
                        missing.Remove(puzzle[i1, x]);
                    }
                }
                totalHeuristic += missing.Count - rowHeuristic[i1];
                rowHeuristic[i1] = missing.Count;

                missing = Enumerable.Range(1, n).ToList();
                for (int x = 0; x < n; x++) {
                    if (missing.Contains(puzzle[i2, x])) {
                        missing.Remove(puzzle[i2, x]);
                    }
                }
                totalHeuristic += missing.Count - rowHeuristic[i2];
                rowHeuristic[i2] = missing.Count;
            }
            if (j1 != j2) {
                missing = Enumerable.Range(1, n).ToList();
                for (int x = 0; x < n; x++) {
                    if (missing.Contains(puzzle[x, j1])) {
                        missing.Remove(puzzle[x, j1]);
                    }
                }
                totalHeuristic += missing.Count - columnHeuristic[j1];
                columnHeuristic[j1] = missing.Count;

                missing = Enumerable.Range(1, n).ToList();
                for (int x = 0; x < n; x++) {
                    if (missing.Contains(puzzle[x, j2])) {
                        missing.Remove(puzzle[x, j2]);
                    }
                }
                totalHeuristic += missing.Count - columnHeuristic[i2];
                columnHeuristic[i2] = missing.Count;
            }
        }

        public void FillRandom() {
            int i, j;
            List<int>[] rowContains = new List<int>[n];
            List<int>[] columnContains = new List<int>[n];
            for (i = 0; i < n; i++) {
                rowContains[i] = new List<int>();
                columnContains[i] = new List<int>();
            }
            for (int block = 0; block < n; block++) {
                List<int> missing = Enumerable.Range(1, n).ToList();
                for (int elem = 0; elem < n; elem++) {
                    i = blockSize * (block / blockSize) + (elem / blockSize);
                    j = blockSize * (block % blockSize) + (elem % blockSize);
                    if (puzzle[i, j] != 0) {
                        unswappable[block, elem] = true;
                        missing.Remove(puzzle[i, j]);
                    }
                }
                for (int elem = 0; elem < n; elem++) {
                    i = blockSize * (block / blockSize) + (elem / blockSize);
                    j = blockSize * (block % blockSize) + (elem % blockSize);
                    if (puzzle[i, j] == 0) {
                        int r = missing[random.Next(missing.Count)];
                        puzzle[i, j] = r;
                        missing.Remove(r);
                    }
                    if (rowContains[i].Contains(puzzle[i, j])) {
                        rowHeuristic[i]++;
                    } else {
                        rowContains[i].Add(puzzle[i, j]);
                    }
                    if (columnContains[j].Contains(puzzle[i, j])) {
                        columnHeuristic[j]++;
                    } else {
                        columnContains[j].Add(puzzle[i, j]);
                    }
                }
            }
            totalHeuristic = rowHeuristic.Sum() + columnHeuristic.Sum();
        }

        public void Print() {
            String space = " ";
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    if ((j + 1) % blockSize == 0 && j != n - 1) {
                        space = "|";
                    } else {
                        space = " ";
                    }
                    Console.Write(puzzle[i, j] + space);
                }
                Console.Write("\r\n");
                if ((i + 1) % blockSize == 0 && i != n - 1) {
                    for (int j = 1; j < blockSize; ++j) {
                        Console.Write(new string('-', 2 * blockSize - 1) + "+");
                    }
                    Console.Write(new string('-', 2 * blockSize - 1) + "\n");
                }
            }
        }
    }
}
