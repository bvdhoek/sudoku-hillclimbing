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

            HillClimber climber = new HillClimber(sudoku);
            Console.WriteLine("Calculating solution");
            while (true)
            {
                climber.Climb();
                if (climber.bestHeuristic == 0) break;
            }
            Console.WriteLine("Calculated solution");

            climber.puzzle.Print();
            Console.ReadLine();
        }
    }
}
