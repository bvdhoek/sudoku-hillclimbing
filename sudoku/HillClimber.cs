using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku {
    class HillClimber {
        public Sudoku puzzle;
        public int bestHeuristic = 1000;
        private int foundSame = 0, iterations = 0;
        public int neighboursSearched = 0;

        public HillClimber(Sudoku puzzle) {
            this.puzzle = puzzle;
        }

        public bool Climb() {
            foundSame = 0;
            iterations++;
            while (!puzzle.isLocalOptimum || foundSame <= 50) {
                int h = puzzle.totalHeuristic;
                puzzle.BestNeighbour();
                neighboursSearched++;
                if (h == puzzle.totalHeuristic)
                    foundSame++;
                else {
                    foundSame = 0;
                }
                if (puzzle.totalHeuristic == 0)
                    return true;
            }
            puzzle.RandomWalk(2);
            return false;
        }
    }
}
