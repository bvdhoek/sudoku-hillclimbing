using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku {
    class Sudoku {
        public int totalHeuristic;
        int[] rowHeuristic, columnHeuristic;
        public int[,] puzzle;
        public bool[,] swappable;
        public bool isLocalOptimum = false;
        private Random random = new Random();

        public Sudoku() {
            rowHeuristic = new int[9];
            columnHeuristic = new int[9];
            puzzle = new int[9, 9];
            swappable = new bool[9, 9];
            ReadPuzzle();
            FillRandom();
            CalculateHeuristic();
        }

        private void ReadPuzzle()
        {
            for (int i = 0; i < 9; i++)
            {
                string numbers = Console.ReadLine();
                for (int j = 0; j < 9; j++)
                {
                    int number = numbers[j] - '0';
                    swappable[i, j] = number == 0;
                    puzzle[i, j] = number;
                }
            }
        }

        public void BestNeighbour() {
            int blockNumber = random.Next(9);
            List<Swap> swaps = AllSwaps(blockNumber);
            Swap bestSwap = null;
            int currentHeuristic = totalHeuristic;
            int bestHeuristicFound = 1000;
            foreach (Swap s in swaps)
            {
                Swap(s);
                if (totalHeuristic <= currentHeuristic && totalHeuristic <= bestHeuristicFound)
                {
                    bestSwap = s;
                    bestHeuristicFound = totalHeuristic;
                }
                Swap(s);
            }
            if (bestSwap != null)
            {
                isLocalOptimum = false;
                Swap(bestSwap);
            } else
            {
                isLocalOptimum = true;
            }
            
        }

        private List<Swap> AllSwaps(int blockNumber)
        {
            List<Swap> swaps = new List<Swap>();
            int xMin = 3 * (blockNumber % 3);
            int yMin = 3 * (blockNumber / 3);

            for (int y1 = yMin; y1 <= yMin + 2; y1++)
            {
                for (int x1 = xMin; x1 <= xMin + 2; x1++)
                {
                    for (int y2 = yMin; y2 <= yMin + 2; y2++)
                    {
                        for (int x2 = xMin; x2 <= xMin + 2; x2++)
                        {
                            if (((x2 > x1 && y2 >= y1) || y2 > y1) && swappable[x1, y1] && swappable[x2, y2])
                                swaps.Add(new Swap(x1, y1, x2, y2));
                        }
                    }
                }
            }
            return swaps;
        }

        public void RandomWalk(int iterations)
        {
            int x1, y1, x2, y2;
            for (int i = 0; i < iterations; i++) {
                do
                {
                    x1 = random.Next(9);
                    y1 = random.Next(9);
                } while (!swappable[x1, y1]);
                do
                {
                    x2 = (x1 / 3) * 3 + random.Next(3);
                    y2 = (y1 / 3) * 3 + random.Next(3);
                } while (!swappable[x2, y2] || (x2 == x1 && y2 == y1));
                Swap(new Swap(x1, y1, x2, y2));
            }
        }

        public void Swap(Swap s)
        {
            int temp = puzzle[s.x1, s.y1];
            puzzle[s.x1, s.y1] = puzzle[s.x2, s.y2];
            puzzle[s.x2, s.y2] = temp;
            UpdateHeuristic(s);
        }

        private void UpdateHeuristic(Swap s)
        {
            UpdateColumnHeuristic(s.x1);
            UpdateColumnHeuristic(s.x2);
            UpdateRowHeuristic(s.y1);
            UpdateRowHeuristic(s.y2);
        }

        private void UpdateColumnHeuristic(int column)
        {
            UInt16 values = 0;
            int heuristic = 0;
            for (int i = 0; i < 9; i++)
            {
                if (values == (values | 1 << puzzle[column, i]))
                {
                    heuristic++;
                }
                else
                {
                    values = (UInt16)(values | (1 << puzzle[column, i]));
                }
            }
            totalHeuristic += heuristic - columnHeuristic[column];
            columnHeuristic[column] = heuristic;
        }

        private void UpdateRowHeuristic(int row)
        {
            UInt16 values = 0;
            int heuristic = 0;
            for (int i = 0; i < 9; i++)
            {
                if (values == (values | 1 << puzzle[i, row]))
                {
                    heuristic++;
                }
                else
                {
                    values = (UInt16)(values | (1 << puzzle[i, row]));
                }
            }
            totalHeuristic += heuristic - rowHeuristic[row];
            rowHeuristic[row] = heuristic;
        }

        private void CalculateHeuristic()
        {
            for (int j = 0; j < 9; j++)
            {
                UpdateColumnHeuristic(j);
                UpdateRowHeuristic(j);
            }
            totalHeuristic = rowHeuristic.Sum(x => x) + columnHeuristic.Sum(x => x);
        }

        public void FillRandom() {
            int i, j;
            List<int>[] rowContains = new List<int>[9];
            List<int>[] columnContains = new List<int>[9];
            for (i = 0; i < 9; i++) {
                rowContains[i] = new List<int>();
                columnContains[i] = new List<int>();
            }
            for (int block = 0; block < 9; block++) {
                List<int> missing = Enumerable.Range(1, 9).ToList();
                for (int elem = 0; elem < 9; elem++) {
                    i = 3 * (block / 3) + (elem / 3);
                    j = 3 * (block % 3) + (elem % 3);
                    if (puzzle[i, j] != 0) {
                        missing.Remove(puzzle[i, j]);
                    }
                }
                for (int elem = 0; elem < 9; elem++) {
                    i = 3 * (block / 3) + (elem / 3);
                    j = 3 * (block % 3) + (elem % 3);
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
            Console.WriteLine(this.totalHeuristic);
            String space = " ";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((j + 1) % 3 == 0 && j != 8)
                    {
                        space = "|";
                    }
                    else
                    {
                        space = " ";
                    }
                    if (!swappable[i, j])
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    Console.Write(puzzle[i, j]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(space);
                }
                Console.Write("\r\n");
                if ((i + 1) % 3 == 0 && i != 8)
                {
                    for (int j = 1; j < 3; ++j)
                    {
                        Console.Write(new string('-', 2 * 3 - 1) + "+");
                    }
                    Console.Write(new string('-', 2 * 3 - 1) + "\n");
                }
            }
        }
    }
}
