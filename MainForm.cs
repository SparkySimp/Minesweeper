//#define __DEBUG_1

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTP_20230119_Minesweeper
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        readonly Size BUTTON_SIZE = new Size(50, 50);

        Point POS = new Point(5, 5);
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Create a 9x9 grid of MineButtonProps.
            MineButtonProps[,] props = (MineButtonProps[,])Array.CreateInstance(typeof(MineButtonProps), 9, 9);
            Random prng = new Random(DateTime.Now.Ticks.GetHashCode());

            int refx, refy;
            // Create mines.
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    props[i, j] = new MineButtonProps(isMine: prng.Next(0, 3) == 1);
                    refx = i;
                    refy = j;

                    // Find neighbours of a prop. (suspected working on edges)
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            if (refx + k >= 0 && refx + k < 9 && refy + l >= 0 && refy + l < 9)
                            {
                                props[i, j].Neighbours.Add(props[refx + k, refy + l]);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Button btn = props[i, j].GenerateButton(BUTTON_SIZE, POS);

                    btn.Click += MineButton_Click;
                    POS.X += BUTTON_SIZE.Width + 5;
                    this.Controls.Add(btn);
                }
                POS.X = 5;
                POS.Y += BUTTON_SIZE.Height + 5;
            }
        }

        private void MineButton_Click(object sender, EventArgs e)
        {
            
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
        }
    }
}
