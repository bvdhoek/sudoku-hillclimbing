using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku
{
    class HillClimber
    {
        Climbable puzzle;
        bool inLocalOptimum = false;

        public HillClimber(Climbable puzzle)
        {
            this.puzzle = puzzle;
        }

        public void Climb()
        {
            Climbable bestSolution = puzzle;
            int bestHeuristic = puzzle.HeuristicValue();
            while (!inLocalOptimum)
            {
                foreach (Climbable neighbour in puzzle.Neighbours())
                {
                    int neighbourHeuristic = neighbour.HeuristicValue();
                    if (neighbourHeuristic < bestHeuristic)
                    {
                        bestHeuristic = neighbourHeuristic;
                        bestSolution = neighbour;
                    }
                }
                inLocalOptimum = bestSolution == puzzle;
                puzzle = bestSolution;
            }
        }
    }
}
