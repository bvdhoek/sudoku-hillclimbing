using System;
using sudoku;

namespace sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            Sudoku sudoku = new Sudoku(n);
            int blockSize = (int)Math.Sqrt(n);
            for (int i = 0; i < n; i++)
            {
                string[] numbers = Console.ReadLine().Split();
                for (int j = 0; j < n; j++)
                {
                    //int block = blockSize * (i / blockSize) + (j / blockSize);
                    //int elem = blockSize * (i % blockSize) + (j % blockSize);
                    sudoku.puzzle[i, j] = int.Parse(numbers[j]);
                }
            }
            sudoku.FillRandom();
            sudoku.Print();

            Console.WriteLine(sudoku.HeuristicValue());

            HillClimber climber = new HillClimber(sudoku);
            for (int i = 0; i < 10000000; i++)
            {
                climber.Climb();
                if (climber.bestHeuristic == 0) break;
                Console.WriteLine(climber.bestHeuristic);
            }
            climber.puzzle.Print();
            Console.WriteLine("neighbours searched {0}", climber.neighboursSearched);

            Console.ReadLine();
        }
    }
}
