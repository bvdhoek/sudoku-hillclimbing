using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku
{
    interface Climbable
    {
        IEnumerable<Climbable> Neighbors();
        void RandomWalk(int iterations);
        int HeuristicValue();
    }
}
