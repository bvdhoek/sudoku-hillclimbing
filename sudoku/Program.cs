using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using sudoku;

namespace sudoku {
    class Program {
        static void Main(string[] args) {
            Application oXL;
            _Workbook oWB;
            _Worksheet oSheet;
            Range oRng;

            oXL = new Application();
            oXL.Visible = true;

            //Get a new workbook.
            oWB = (_Workbook)(oXL.Workbooks.Add(Missing.Value));

            Stopwatch s = new Stopwatch();
            for (int S = 1; S <= 10; S++)
            {
                for (int N = 10; N <= 100; N += 10)
                {
                    oSheet = oXL.Worksheets.Add();
                    oSheet.Name = "S = " + S + ", N = " + N;
                    for (int j = 1; j <= 10; j++)
                    {
                        string[] puzzle = File.ReadAllLines("puzzle" + j + ".txt");
                        oSheet.Cells[1, j * 2 - 1] = "puzzle " + j;
                        oSheet.Cells[1, j * 2] = "states";
                        for (int i = 0; i < 20; i++)
                        {
                            s.Reset();
                            Sudoku sudoku = new Sudoku(puzzle);
                            HillClimber climber = new HillClimber(sudoku, S, N);
                            bool foundSolution = false;
                            s.Start();
                            while (!foundSolution)
                            {
                                foundSolution = climber.Climb();
                            }
                            s.Stop();
                            oSheet.Cells[i + 2, (j * 2) - 1] = s.ElapsedMilliseconds;
                            oSheet.Cells[i + 2, j * 2] = sudoku.neighboursSearched;
                            Console.WriteLine("Calculated solution in " + s.ElapsedMilliseconds + " ms");
                        }
                    }
                }
            }
        }
    }
}
