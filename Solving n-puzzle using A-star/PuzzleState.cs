using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solving_n_puzzle_using_A_star
{
    class PuzzleState
    {
        public int[,] State { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int F => H + G;
        public PuzzleState? Parent { get; set;}

        public PuzzleState(int[,] state,int g,int h)
        {
            G = g;
            H = h;
            State = state;
            //MessageBox.Show("Puzzle state instance created");
        }

    }
    class PuzzleSolver
    {
        int[,] goalState;
        int[,] moves;
        int rowsOrColumns;

        public PuzzleSolver(int rowsOrColumns, int[,] goalState)
        {
            this.rowsOrColumns = rowsOrColumns;
            this.moves = new int[,] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };
            this.goalState = goalState;
            
            //MessageBox.Show("Puzzle solver instance created");
        }

        public List<int[,]> Solve(int[,] initialState)
        {
            var openList = new List<PuzzleState>();
            var closedList = new HashSet<String>();
            var currentState = new PuzzleState(initialState,0,ManhattanDistance(initialState));
            openList.Add(currentState);

            while (openList.Count > 0)
            {
                openList.Sort((a, b) => a.F.CompareTo(b.F));
                currentState = openList[0];
                closedList.Add(StateToString(currentState.State));
                openList.RemoveAt(0);
                //MessageBox.Show(openList.ToString());

                if(IsGoalState(currentState))
                {
                    return TracePath(currentState);
                    
                }

                var emptyTile = FindEmptyTile(currentState);
                
                for(int i = 0; i < 4; i++)
                {
                    var newEmptyTile = (emptyTile.Item1 + moves[i, 0], emptyTile.Item2 + moves[i,1]);
                    if (IsNewTileInsideBorder(newEmptyTile))
                    {
                        var newBoard = (int[,])currentState.State.Clone();
                        newBoard[emptyTile.Item1, emptyTile.Item2] = newBoard[newEmptyTile.Item1, newEmptyTile.Item2];
                        newBoard[newEmptyTile.Item1, newEmptyTile.Item2] = 0;
                        var newState = new PuzzleState(newBoard, currentState.G + 1, ManhattanDistance(newBoard));
                        newState.Parent = currentState;
                        
                        if (!closedList.Contains(StateToString(newBoard)))
                        {
                            openList.Add(newState);
                        }
                    }
                }
            }


            return null;
        }

        private int ManhattanDistance(int[,] state)
        {
            int distance = 0;
            for (int i = 0; i < rowsOrColumns; i++)
            {
                for (int j = 0; j < rowsOrColumns; j++)
                {
                    int value = state[i, j];
                    if (value != 0)
                    {
                        for (int ni = 0; ni < rowsOrColumns; ni++)
                        {
                            for (int nj = 0; nj < rowsOrColumns; nj++)
                            {
                                if (value == goalState[ni, nj])
                                {
                                    distance += Math.Abs(i - ni) + Math.Abs(j - nj);
                                }
                            }
                        }
                    }
                }
            }
            return distance;
        }

        public String StateToString(int[,] board)
        {
            string s = "";
            for (int i = 0; i < rowsOrColumns; i++)
            {
                for(int j = 0; j < rowsOrColumns; j++)
                {
                    s += board[i, j].ToString();
                }
            }
            return s;
        }

        public bool IsGoalState(PuzzleState board)
        {
            return StateToString(goalState) == StateToString(board.State);
        }

        private Tuple<int,int> FindEmptyTile(PuzzleState board)
        {
            for (int i = 0; i < rowsOrColumns; i++)
            {
                for (int j = 0; j < rowsOrColumns; j++)
                {
                    if (board.State[i,j] == 0)
                    {
                        return Tuple.Create(i, j);
                    }
                }
            }
            return null;
        }

        private bool IsNewTileInsideBorder((int,int) newTile)
        {
            return newTile.Item1 >= 0 && newTile.Item1 < rowsOrColumns && newTile.Item2 >= 0 && newTile.Item2 < rowsOrColumns;
        }

        private List<int[,]> TracePath(PuzzleState state)
        {
            List<int[,]> path = new List<int[,]>();
            while(state != null)
            {
                path.Insert(0, state.State);
                state = state.Parent!;
            }
            //MessageBox.Show(path.ToString());
            return path;
        }
    }
}
