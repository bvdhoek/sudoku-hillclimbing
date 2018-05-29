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
        private int foundSame = 0;
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
            puzzle.RandomWalk(1);
            while (!puzzle.InLocalOptimum())
            {
                bestSolution = puzzle.BestNeighbour();
                neighboursSearched++;
                puzzle = bestSolution;
            }
            if (puzzle.HeuristicValue() == this.bestHeuristic)
            {
                foundSame++;
                Console.Write("\r" + this.bestHeuristic + ": " + this.foundSame.ToString("D5") + " ");
            } else if (puzzle.HeuristicValue() < this.bestHeuristic)
            {
                this.bestHeuristic = puzzle.HeuristicValue();
                foundSame = 0;
                Console.Write("\r" + this.bestHeuristic + ": " + this.foundSame.ToString("D5") + " ");
            }
        }
    }
}
