using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku
{
    class HillClimber
    {
        public Sudoku puzzle;
        public int bestHeuristic = 1000;
        public int neighboursSearched = 0;

        public HillClimber(Sudoku puzzle)
        {
            this.puzzle = puzzle;
        }

        public void Climb()
        {
            puzzle.isLocalOptimum = false;
            Sudoku bestSolution = puzzle;
            int bestHeuristic = puzzle.HeuristicValue();
            puzzle.RandomWalk(5);
            while (!puzzle.InLocalOptimum())
            {
                bestSolution = puzzle.BestNeighbour();
                neighboursSearched++;
                puzzle = bestSolution;
            }
            this.bestHeuristic = Math.Min(this.bestHeuristic, puzzle.HeuristicValue());
        }
    }
}
