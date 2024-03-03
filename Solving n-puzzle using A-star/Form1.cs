using System;
using System.Windows.Forms;

namespace Solving_n_puzzle_using_A_star
{
    public partial class Form1 : Form
    {
        int[,]? initialState;
        int[,]? goalState;
        static int lenght;
        MyLabel? myLabel;
        MyProgressBar? myProgressBar;
        public Form1()
        {
            InitializeComponent();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            button1.Enabled = false;
            button2.Enabled = false;
            textBox2.Enabled = false;

        }

        static int getInvCount(int[] arr)
        {
            int inv_count = 0;
            for (int i = 0; i < lenght - 1; i++)
            {
                for (int j = i + 1; j < lenght; j++)
                {
                    
                    if (arr[j] != 0 && arr[i] != 0
                        && arr[i] > arr[j])
                        inv_count++;
                }
            }
            return inv_count;
        }

        static int findXPosition(int[,] puzzle)
        {
           
            for (int i = Convert.ToInt32(Math.Sqrt(lenght)) - 1; i >= 0; i--)
            {
                for (int j = Convert.ToInt32(Math.Sqrt(lenght)) - 1; j >= 0; j--)
                {
                    if (puzzle[i, j] == 0)
                        return Convert.ToInt32(Math.Sqrt(lenght)) - i;
                }
            }
            return -1;
        }

        
        static bool isSolvable(int[,] puzzle)
        {
            int[] arr = new int[lenght];
            int k = 0;
            for (int i = 0; i < Convert.ToInt32(Math.Sqrt(lenght)); i++)
            {
                for (int j = 0; j < Convert.ToInt32(Math.Sqrt(lenght)); j++)
                {
                    arr[k++] = puzzle[i, j];
                }
            }

           
            int invCount = getInvCount(arr);

            if (Convert.ToInt32(Math.Sqrt(lenght)) % 2 == 1)
                return invCount % 2 == 0;
            else 
            {
                int pos = findXPosition(puzzle);
                if (pos % 2 == 1)
                    return invCount % 2 == 0;
                else
                    return invCount % 2 == 1;
            }
        }

        private void solve_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            
            if (initialState != null && goalState != null)
            {
                if (isSolvable(initialState) && isSolvable(goalState))
                {
                  
                    myLabel = new MyLabel(
                    location: new Point(progressBar1.Location.X, progressBar1.Location.Y - 20),
                    text: "Steps are calculating...", progressBar: progressBar1);
                    this.Controls.Add(myLabel);
                    
                    Application.DoEvents();

                    PuzzleSolver solver = new PuzzleSolver(Convert.ToInt32(Math.Sqrt(lenght)),goalState);
                    var solution = solver.Solve(initialState);

                    myLabel.Text = "Printing is on progress...";

                    DataGridViewCell? parentCell = null;
                    dataGridView1.ColumnCount = (int)Math.Sqrt(lenght);

                    double barLevelIncrement = 100 / solution.Count;

                    for (int i = 0; i < (int)Math.Sqrt(lenght); i++)
                    {
                        dataGridView1.Rows.Add();
                    }
                    foreach (var step in solution)
                    {

                        for (int i = 0; i < Math.Sqrt(lenght); i++)
                        {
                            for (int j = 0; j < Math.Sqrt(lenght); j++)
                            {
                                dataGridView1.Rows[i].Cells[j].Value = step[i, j];
                                if ((int)(dataGridView1.Rows[i].Cells[j].Value) == 0)
                                {
                                    if (parentCell != null)
                                    {
                                        parentCell.Style.BackColor = Color.White;
                                    }
                                    dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Tomato;
                                    parentCell = dataGridView1.Rows[i].Cells[j];
                                }
                            }
                        }
                        progressBar1.Value += (int)barLevelIncrement;
                        if (solution.IndexOf(step) + 1 == solution.Count)
                        {
                            progressBar1.Value = 100;
                            myLabel.Text = "Successfully solved!!";
                            myLabel.ForeColor = Color.Green;
                            button2.Enabled = true;
                        }
                        System.Threading.Thread.Sleep(1500);
                        Application.DoEvents();

                    }
                }
                else
                {
                    MessageBox.Show("Puzzle is not solvable");
                    button2.Enabled = true;
                }
            }


        }

        private void initialState_KeyDown(object sender, KeyEventArgs e)
        {
            bool isBiggerThanLenght = false;
            bool isNull = false;
            bool isThereSame = false;
            List<int> linearList = new List<int>();
            if (e.KeyData == Keys.Enter)
            {
                GetInitialState();
                GetLenght();
                if(Math.Sqrt(lenght)%1 == 0)
                {
                    for(int i = 0; i<Math.Sqrt(lenght); i++)
                    {
                        for (int j = 0; j < Math.Sqrt(lenght); j++)
                        {
                            try
                            {
                                linearList.Add(initialState[i, j]);
                            }
                            catch(NullReferenceException)
                            {
                                MessageBox.Show("Initial puzzle state can't be null!!");
                                isNull = true;
                                break;
                            }                        
                            
                        }
                    }
                    while(linearList.Count != 0)
                    {
                        int first = linearList[0];
                        linearList.RemoveAt(0);
                        for(int j = 0; j < linearList.Count; j++)
                        {
                            if (linearList[j] == first)
                            {
                                MessageBox.Show("The same value can't be used in the puzzle!!");
                                isThereSame = true;
                                break;
                            }
                            if(first >= lenght)
                            {
                                MessageBox.Show("Values can't be equal or bigger than lenght!!");
                                isBiggerThanLenght = true;
                                break;
                            }
                           
                        }
                        if (isThereSame)
                        {
                            break;
                        }
                        if (isBiggerThanLenght)
                        {
                            break;
                        }
                    }
                    if (!isThereSame && !isNull && !isBiggerThanLenght)
                    {
                        textBox2.Enabled = true;
                        textBox1.Enabled = false;
                    }

                }
                else
                {
                    MessageBox.Show("Puzzle is not a perfect square.");
                }

            }
            
        }

        private void goalState_KeyDown(object sender, KeyEventArgs e)
        {
            bool isBiggerThanLenght = false;
            bool isNull = false;
            bool isThereSame = false;
            List<int> linearList = new List<int>();
            if (e.KeyData == Keys.Enter)
            {
                GetGoalState();
                if (textBox2.Text.Length != lenght)
                {
                    if (Math.Sqrt((textBox2.Text.Split("-")).Length) % 1 == 0)
                    {
                        for (int i = 0; i < Math.Sqrt(lenght); i++)
                        {
                            for (int j = 0; j < Math.Sqrt(lenght); j++)
                            {
                                try
                                {
                                    linearList.Add(goalState[i, j]);
                                }
                                catch (NullReferenceException)
                                {
                                    MessageBox.Show("Goal puzzle state can't be null!!");
                                    isNull = true;
                                    break;
                                }

                            }
                        }
                        while (linearList.Count != 0)
                        {
                            int first = linearList[0];
                            linearList.RemoveAt(0);
                            for (int j = 0; j < linearList.Count; j++)
                            {
                                if (linearList[j] == first)
                                {
                                    MessageBox.Show("The same value can't be used in the puzzle!!");
                                    isThereSame = true;
                                    break;
                                }
                                if (first >= lenght)
                                {
                                    MessageBox.Show("Values can't be equal or bigger than lenght!!");
                                    isBiggerThanLenght = true;
                                    break;
                                }
                            }
                            if (isThereSame)
                            {
                                break;
                            }
                            if (isBiggerThanLenght)
                            {
                                break;
                            }
                        }
                        if (!isThereSame && !isNull && !isBiggerThanLenght)
                        {
                            textBox2.Enabled = false;
                            button1.Enabled = true;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Puzzle is not a perfect square.");
                    }
                }
                else
                {
                    MessageBox.Show("Puzzle sizes are not matching!!");
                }
                
            }
        }

        private void GetInitialState()
        {

            int stp = 0;
            if (textBox1.Text != "")
            {

                string[] s = (textBox1.Text.ToString()).Split("-");
                int size = (int)Math.Sqrt(s.Length);
                initialState = new int[size, size];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {

                        initialState![i, j] = Convert.ToInt32(s[stp]);
                        stp++;

                    }
                }

            }
        }

        private void GetGoalState()
        {
            int stp = 0;
            if (textBox2.Text != "")
            {
                string[] s = (textBox2.Text.ToString()).Split("-");
                int size = (int)Math.Sqrt(s.Length);
                goalState = new int[size, size];
                
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        goalState![i, j] = Convert.ToInt32(s[stp]);
                        stp++;
                    }
                }

                

            }
        }
        private void GetLenght()
        {
            string[] s = textBox1.Text.Split("-");
            lenght = s.Length;
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            dataGridView1.Rows.Clear();
            progressBar1.Value = 0;
            textBox1.Enabled = true;
            this.Controls.Remove(myLabel);
            
        }
    }
}