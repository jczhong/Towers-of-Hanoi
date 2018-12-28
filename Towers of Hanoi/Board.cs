using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Towers_of_Hanoi
{
    class Board
    {
        Disk[,] board; //condition says TWO dimentional array            
        ArrayList movements;
        Disk[] disks; //Array of disks

        private const int NUM_DISKS = 4;
        private const int NUM_PEGS = 3;

        private int[] tops = {291, 267, 243, 219};
        private int[] lefts = {50, 66, 82, 98};

        int lastPegNum;
        int lastLevel;

        int drawCount;

        bool completed = false;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Board()
        {
            board = new Disk[NUM_PEGS, NUM_DISKS];
            movements = new ArrayList();

            //Array of disk objects
            disks = new Disk[NUM_DISKS];
            disks[0] = null;
            disks[1] = null;
            disks[2] = null;
            disks[3] = null;

            //Storing disk object into board array(Two dimensional arrray) 
            board = new Disk[NUM_PEGS, NUM_DISKS]; //condition says TWO dimentional array  

            board[0, 3] = new Disk();
            board[0, 2] = new Disk();
            board[0, 1] = new Disk();
            board[0, 0] = new Disk();

            //Creating arraylist of movement 
            movements = new ArrayList();
        }

        /// <summary>
        /// Alterntative constructor
        /// @param three disk objects
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="d3"></param>
        public Board(Disk d1, Disk d2, Disk d3, Disk d4)
        {
            //Storing into disks array
            disks = new Disk[NUM_DISKS];
            disks[0] = d1;
            disks[1] = d2;
            disks[2] = d3;
            disks[3] = d4;

            //Storing disk object into board array(Two dimensional arrray) 
            board = new Disk[NUM_PEGS, NUM_DISKS]; //condition says TWO dimentional array  
            board[0, 0] = d1;
            board[0, 1] = d2;
            board[0, 2] = d3;
            board[0, 3] = d4;

            //Arraylist of movement.
            movements = new ArrayList();
        }

        /// <summary>
        /// Reset the board, position the Disks in the starting position
        /// </summary>
        public void reset()
        {
            for (int iP = 0; iP < NUM_PEGS; iP++)
            {
                //Remove all elements from board array
                for (int iD = 0; iD < NUM_DISKS; iD++)
                {
                    board[iP, iD] = null;

                    //Update disks array
                    disks[iD].setPegNum(0);
                    disks[iD].setLevel(iD);
                }
            }

            //Reallocate elements 
            board[0, 3] = disks[3]; //Peg 1/Level4 
            board[0, 2] = disks[2]; //Peg 1/Level3 
            board[0, 1] = disks[1]; //Peg 1/Level2
            board[0, 0] = disks[0]; //Peg 1/Level1 

            //Remove all elements from movement arraylist
            movements.Clear();
            completed = false;
        }

        /// <summary>
        /// Check if the disk can be moved
        /// @param disk object nedd to be checked
        /// </summary>
        /// <param name="aDisk"></param>
        public bool canStartMove(Disk aDisk)
        {
            int pegNum = aDisk.getPegNum();
            int level = aDisk.getLevel();
            if (level < NUM_DISKS)
            {
                if ((level == NUM_DISKS - 1) || (board[pegNum, level + 1] == null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //error
                return false;
            }
        }


        /// <summary>
        /// Check if the disk can be dropped on the peg
        /// @param disk object and peg
        /// </summary>
        /// <param name="aDisk"></param>
        /// <param name="aPeg"></param>
        public bool canDrop(Disk aDisk, int aPeg)
        {
            for (int i = 0; i < NUM_DISKS; i++)
            {
                if (board[aPeg, i] != null)
                {
                    continue;
                }
                else
                {
                    i -= 1;
                    if (i < 0)
                    {
                        return true;
                    }
                    else
                    {
                        Disk disk = board[aPeg, i];
                        return (disk.getDiameter() > aDisk.getDiameter());
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Move disk to the target peg
        /// @param disk object, target peg and disk level
        /// </summary>
        /// <param name="aDisk"></param>
        /// <param name="aPeg"></param>
        /// <param name="aLevel"></param>
        public void move(Disk aDisk, int aPeg, int aLevel)
        {
            lastPegNum = aDisk.getPegNum();
            lastLevel = aDisk.getLevel();
            board[aDisk.getPegNum(), aDisk.getLevel()] = null;
            aDisk.setPegNum(aPeg);
            aDisk.setLevel(aLevel);
            board[aPeg, aLevel] = aDisk;
            movements.Add(new DiskMove(Array.IndexOf(disks, aDisk), aPeg));

            if ((aPeg == 2) && (aLevel == 3))
            {
                completed = true;
            }
            else
            {
                completed = false;
            }
        }


        /// <summary>
        /// Return a string giving the moves so far, one move per line
        /// </summary>
        public string allMovesAsString()
        {
            string str = "";

            foreach (DiskMove move in movements)
            {
                str += move.commaText() + Environment.NewLine;
            }
            return str;
        }


        /// <summary>
        /// Display the current position of the disks
        /// </summary>
        public void Display()
        {
            for (int i = 0; i < disks.Length; i++)
            {
                Disk disk = disks[i];
                Label label = disk.getLabel();
                label.Left = lefts[i] + (disk.getPegNum() * 180);
                label.Top = tops[disk.getLevel()];
            }
        }

        /// <summary>
        /// Get a reference to the disk that matches a label
        /// @param label object
        /// </summary>
        /// <param name="aLabel"></param>
        public Disk FindDisk(Label aLabel)
        {
            foreach (Disk disk in disks)
            {
                if (disk.getLabel() == aLabel)
                {
                    return disk;
                }
            }
            return null;
        }


        /// <summary>
        /// Get new level number on the target peg
        /// @param peg
        /// </summary>
        /// <param name="pegNum"></param>
        public int newLevInPeg(int pegNum)
        {
            for (int i = 0; i < NUM_DISKS; i++)
            {
                if (board[pegNum, i] != null)
                {
                    continue;
                }
                else
                {
                    return i;
                }
            }
            return -1;
        }


        //public String getText(int cnt)
        //{
        //    return "1";    // Dummy return to avoid syntax error - must be changed
        //}


        //public void backToSelected(int ind)
        //{

        //}


        //public int getPegInd(int ind)
        //{
        //     return disks[ind].getPegNum();
        //}


        //public int getLevel(int ind)
        //{
        //    return disks[ind].getLevel();
        //}


        /// <summary>
        /// Undo the last move
        /// </summary>
        public void unDo()
        {
            DiskMove diskMove = (DiskMove)movements[movements.Count - 1];
            Disk disk = disks[diskMove.getDiskInd()];
            board[disk.getPegNum(), disk.getLevel()] = null;
            disk.setLevel(lastLevel);
            disk.setPegNum(lastPegNum);
            board[lastPegNum, lastLevel] = disk;
            movements.Remove(diskMove);
        }

        /// <summary>
        /// Get the number of moves
        /// </summary>
        public int getMoveCount()
        {
            return movements.Count;
        }

        /// <summary>
        /// Load saved moves data
        /// </summary>
        public void loadData()
        {
            try
            {
                using (StreamReader sr = new StreamReader(new FileStream(Application.UserAppDataPath + "\\appdata.txt", FileMode.OpenOrCreate)))
                {
                    while (sr.Peek() >= 0)
                    {
                        DiskMove diskMove = new DiskMove(sr.ReadLine());
                        movements.Add(diskMove);
                    }
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("Error: " + e.ToString());
            }

            if (movements.Count > 0)
            {
                foreach(DiskMove diskMove in movements)
                {
                    int diskInd = diskMove.getDiskInd();
                    int pegNum = diskMove.getPegInd();
                    int level = newLevInPeg(pegNum);
                    Disk disk = disks[diskInd];
                    board[disk.getPegNum(), disk.getLevel()] = null;
                    disk.setPegNum(pegNum);
                    disk.setLevel(level);
                    board[pegNum, level] = disk;

                    if ((pegNum == 2) && (level == 3))
                    {
                        completed = true;
                    }
                }
                Display();
            }
        }

        /// <summary>
        /// Save moves data
        /// </summary>
        public void saveData()
        {
            try
            {
                string path = Application.UserAppDataPath + "\\appdata.txt";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (DiskMove diskMove in movements)
                    {
                        sw.WriteLine(diskMove.commaText());
                    }
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("Error: " + e.ToString());
            }
        }

        /// <summary>
        /// Start animation
        /// </summary>
        public void startAnimation()
        {
            for (int iP = 0; iP < NUM_PEGS; iP++)
            {
                for (int iD = 0; iD < NUM_DISKS; iD++)
                {
                    board[iP, iD] = null;

                    disks[iD].setPegNum(0);
                    disks[iD].setLevel(iD);
                }
            }

            board[0, 3] = disks[3];
            board[0, 2] = disks[2];
            board[0, 1] = disks[1];
            board[0, 0] = disks[0];

            drawCount = 0;

            Display();
        }

        /// <summary>
        /// Draw graphics
        /// </summary>
        public bool drawAnimation()
        {
            if (drawCount < movements.Count)
            {
                DiskMove diskMove = (DiskMove)movements[drawCount++];
                int diskInd = diskMove.getDiskInd();
                int pegNum = diskMove.getPegInd();
                int level = newLevInPeg(pegNum);
                Disk disk = disks[diskInd];

                board[disk.getPegNum(), disk.getLevel()] = null;
                disk.setPegNum(pegNum);
                disk.setLevel(level);
                board[pegNum, level] = disk;
                Display();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if user complete the game
        /// </summary>
        public bool isCompleted()
        {
            return completed;
        }

        /// <summary>
        /// Check if user complete the game with minimum number of moves
        /// </summary>
        public bool withMinimunSteps()
        {
            return (movements.Count == 15);
        }
   }
}
