using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Rev
{
    public partial class Reversi : Form
    {
        StartPage startPage = new StartPage();
        CBoard board;
        bool flag = false;
        bool test = true;
        int level = 0;

        private bool IsDrawHelp = true;

        public Reversi()
        {
            InitializeComponent();
            InitBoard();
        }

        private void InitBoard()
        {
            board = new CBoard();
            panel1.Width = CBoard.w * CBoard.RectWidth;
            panel1.Height = CBoard.w * CBoard.RectWidth;
            label3.Text = "2";
            label4.Text = "2";
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            board.Draw(e.Graphics, false, 0);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (board.GetEnableSteps(1).Count == 0)
            {
                flag = true;
                ShowRes();
                CompStep();               
            }
            if (flag) return;
            int x = e.X / CBoard.RectWidth;
            int y = e.Y / CBoard.RectWidth;

            if (board.AddFig(x, y, 1, true) > 0)
            {
                
                
                board.Draw(panel1, IsDrawHelp, -1);
                flag = true;
                ShowRes();
                CompStep();               
            }
            else
            {
                //MessageBox.Show("Error position!!!");
            }
        }

        private void ShowRes()
        {
            if (board.FigCount == 60 && test)
            {
                if (board.PlayersFig > board.CompFig)
                {
                    MessageBox.Show("Player wins");
                    test = false;
                }
                if (board.PlayersFig < board.CompFig)
                {
                    MessageBox.Show("Computer wins");
                    test = false;
                }
                if (board.PlayersFig == board.CompFig)
                {
                    MessageBox.Show("Draw");
                    test = false;
                }
            }
            if (board.PlayersFig == 0 && test)
            {
                MessageBox.Show("Computer wins");
                test = false;
            }
            if (board.CompFig == 0)
            {
                MessageBox.Show("Player wins");
                test = false;
            }
            if ((board.GetEnableSteps(1).Count == 0) && (board.GetEnableSteps(-1).Count == 0) && (board.FigCount != 60) && test)
            {
                if (board.PlayersFig > board.CompFig)
                {
                    MessageBox.Show("Player wins");
                    test = false;
                }
                if (board.PlayersFig < board.CompFig)
                {
                    MessageBox.Show("Computer wins");
                    test = false;
                }
                if (board.PlayersFig == board.CompFig)
                {
                    MessageBox.Show("Draw");
                    test = false;
                }
            }
            label3.Text = board.PlayersFig.ToString();
            label4.Text = board.CompFig.ToString();
            if (!test)
            {
                beginnerToolStripMenuItem.Enabled = false;
                mediumToolStripMenuItem.Enabled = false;
                expertToolStripMenuItem.Enabled = false;
            }
        }

        private void DoRandomStep()
        {
            List<int[]> l = board.GetEnableSteps(-1);
            Random rand = new Random();
            int temp = rand.Next(0, l.Count);
            board.AddFig(l[temp][0], l[temp][1], -1, true);
        }

        private void DoStep()
        {
            List<int[]> l = board.GetEnableSteps(-1);
            int j = -1;
            int max = 0;
            for (int i = 0; i < l.Count; i++)
            {
                int[] ar = l[i];
                if (max < ar[2])
                {
                    j = i;
                    max = ar[2];
                }
            }
            if (j != -1)
                board.AddFig(l[j][0], l[j][1], -1, true);
        }

        private void CompStep()
        {
            Thread.Sleep(500);
            if (board.GetEnableSteps(-1).Count == 0)
            {

                flag = false;
                board.Draw(panel1, IsDrawHelp, 1);
                ShowRes();
            }
            if (flag)
            {
                if (level == 0)
                {
                    DoRandomStep();
                }
                if (level == 1)
                {
                    DoStep();
                }
                if (level == 2)
                {
                    DoBestStep();
                }
                board.Draw(panel1, IsDrawHelp, 1);
                flag = false;
                ShowRes();
            }
        }

        private void DoBestStep()
        {
            List<int[]> l = board.GetEnableSteps(-1);
            int j = 0;
            int max = -Int32.MaxValue;
            for (int i = 0; i < l.Count; i++)
            {
                CBoard cp = board.Copy();
                cp.AddFig(l[i][0], l[i][1], -1, true);
                int res = CBoard.GetBestStep(1, 0, cp);
                if (max <= res)
                {
                    if (!IsBad(l,i))
                    {
                        j = i;
                        max = res;
                    }
                }
            }
            if (l.Count > j)
                board.AddFig(l[j][0], l[j][1], -1, true);
        }

        public bool IsBad(List<int[]> list, int i)
        {
            if ((list[i][0] == 1 && list[i][1] == 6) || (list[i][0] == 6 && list[i][1] == 1) || (list[i][0] == 6 && list[i][1] == 6) || (list[i][0] == 1 && list[i][1] == 1))
            {
                return true;
            }
            if ((list[i][0] == 0 && list[i][1] == 1) || (list[i][0] == 1 && list[i][1] == 0) || (list[i][0] == 0 && list[i][1] == 6) || (list[i][0] == 1 && list[i][1] == 7))
            {
                return true;
            }
            if ((list[i][0] == 6 && list[i][1] == 0) || (list[i][0] == 7 && list[i][1] == 1) || (list[i][0] == 6 && list[i][1] == 7) || (list[i][0] == 7 && list[i][1] == 6))
            {
                return true;
            }
            else return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            startPage.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InitBoard();
            test = true;
            board.Draw(panel1, IsDrawHelp, 1);
            beginnerToolStripMenuItem.Enabled = false;
            mediumToolStripMenuItem.Enabled = false;
            expertToolStripMenuItem.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InitBoard();
            test = true;
            board.Draw(panel1, IsDrawHelp, 1);
            beginnerToolStripMenuItem.Enabled = false;
            mediumToolStripMenuItem.Enabled = false;
            expertToolStripMenuItem.Enabled = false;
        }

        private void beginnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            level = 0;
        }

        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            level = 1;
        }

        private void expertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            level = 2;
        }
    }
}
