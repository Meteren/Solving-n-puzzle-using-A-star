using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solving_n_puzzle_using_A_star
{
    class MyLabel : Label
    {
        ProgressBar progressBar;
        public MyLabel(Point location, string text,ProgressBar progressBar)
        {
            this.progressBar = progressBar;
            this.Location = location;
            this.Text = text;
            this.ForeColor = Color.Black;
            this.Name = "myLabel";
            this.Size = new Size(progressBar.Size.Width, progressBar.Size.Height);
        }
    }
}
