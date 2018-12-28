using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Towers_of_Hanoi
{
    public partial class MainForm : Form
    {
        private Board board;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            board = new Board(new Disk(lblDisk1, lblDisk4.BackColor, 0, 0), 
                              new Disk(lblDisk2, lblDisk3.BackColor, 1, 0),
                              new Disk(lblDisk3, lblDisk2.BackColor, 2, 0),
                              new Disk(lblDisk4, lblDisk1.BackColor, 3, 0));
            board.loadData();

            txtMoveCount.Text = board.getMoveCount().ToString();
            txtMoves.Text = board.allMovesAsString();
            if (board.isCompleted())
            {
                btnPlay.Enabled = true;
            }
        }

        /// <summary>
        /// lblDisk MouseDown event handler
        /// @param event sender object and event object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDisk_MouseDown(object sender, MouseEventArgs e)
        {
            Label lblDisk = (sender as Label);
            Disk disk = board.FindDisk(lblDisk);

            if (disk != null && board.canStartMove(disk))
            {
                DragDropEffects result = lblDisk.DoDragDrop(disk, DragDropEffects.All);

                if (result != DragDropEffects.None)
                {
                }
                else
                {
                }
            }
            else
            {
                MessageBox.Show("You can not move this disk!", "Error");
            }
        }

        /// <summary>
        /// lblPeg DragEnter event handler
        /// @param event sender object and event object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblPeg_DragEnter(object sender, DragEventArgs e)
        {
            Label lblPeg = (sender as Label);
            int pegNum = getPegNum(lblPeg);
            Disk disk = (Disk)e.Data.GetData(typeof(Disk));

            if ((pegNum >= 0) && (disk != null) && (pegNum != disk.getPegNum()))
            {
                if (board.canDrop(disk, pegNum))
                {
                    e.Effect = DragDropEffects.All;
                    return;
                }
            }
            e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// lblPeg DragDrop event handler
        /// @param event sender object and event object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblPeg_DragDrop(object sender, DragEventArgs e)
        {
            Label lblPeg = (sender as Label);
            int pegNum = getPegNum(lblPeg);
            Disk disk = (Disk)e.Data.GetData(typeof(Disk));

            int level = board.newLevInPeg(pegNum);
            board.move(disk, pegNum, level);
            board.Display();
            txtMoveCount.Text = board.getMoveCount().ToString();
            txtMoves.Text = board.allMovesAsString();

            btnReset.Enabled = true;
            btnUndo.Enabled = true;
            btnPlay.Enabled = true;
            if (board.isCompleted())
            {
                if (board.withMinimunSteps())
                {
                    MessageBox.Show("You have successfully completed the game with the minimum number of moves");
                }
                else
                {
                    MessageBox.Show("You have successfully completed the game but not with the minimum number of moves");
                }
            }
        }

        /// <summary>
        /// Get peg number
        /// @param label object of peg
        /// </summary>
        /// <param name="peg"></param>
        private int getPegNum(Label peg)
        {
            if (peg == lblPeg1)
            {
                return 0;
            }
            else if (peg == lblPeg2)
            {
                return 1;
            }
            else if (peg == lblPeg3)
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// btnReset Click event handler
        /// @param event sender object and event object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            board.reset();
            board.Display();
            txtMoveCount.Text = "0";
            txtMoves.Text = "";
            btnReset.Enabled = false;
            btnUndo.Enabled = false;
            btnPlay.Enabled = false;
        }

        /// <summary>
        /// btnUndo Click event handler
        /// @param event sender object and event object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUndo_Click(object sender, EventArgs e)
        {
            board.unDo();
            board.Display();
            txtMoveCount.Text = board.getMoveCount().ToString();
            txtMoves.Text = board.allMovesAsString();
            btnUndo.Enabled = false;
        }

        /// <summary>
        /// Main form close event handler
        /// @param event sender object and event object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            board.saveData();
        }

        /// <summary>
        /// btnPlay Click event handler
        /// @param event sender object and event object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlay_Click(object sender, EventArgs e)
        {
            timerPlay.Interval = 1000;
            timerPlay.Enabled = true;
            board.startAnimation();
            btnUndo.Enabled = false;
        }

        /// <summary>
        /// timerPlay Tick event handler
        /// @param event sender object and event object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerPlay_Tick(object sender, EventArgs e)
        {
            if (!board.drawAnimation())
            {
                timerPlay.Enabled = false;
                btnUndo.Enabled = true;
            }
        }
    }
}
