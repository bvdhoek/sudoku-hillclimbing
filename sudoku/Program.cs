using System;
using sudoku;

namespace sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            int lines = int.Parse(Console.ReadLine());
            Sudoku sudoku = new Sudoku(lines);
            for (int i = 0; i < lines; i++)
            {
                string[] numbers = Console.ReadLine().Split();
                for (int j = 0; j < lines; j++)
                {
                    sudoku.puzzle[i, j] = int.Parse(numbers[j]);
                }
            }
            sudoku.RandomWalk(30);
        }
    }
}
