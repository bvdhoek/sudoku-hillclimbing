using System;
using sudoku;

namespace sudoku {
    class Program {
        static void Main(string[] args) {
            while (Console.ReadLine() != "") {
                Sudoku sudoku = new Sudoku();
                HillClimber climber = new HillClimber(sudoku);
                Console.WriteLine("Calculating solution");
                bool foundSolution = false;
                while (!foundSolution) {
                    foundSolution = climber.Climb();
                }
                Console.WriteLine("Calculated solution");

                climber.puzzle.Print();
            }
        }
    }
}
