using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessGame
{
    public partial class Form1 : Form
    {
        public Image chessSprites;
        public int[,] board = new int[8, 8] //задаём начальное состояние доски
        {
            {15, 14, 13, 12, 11, 13, 14, 15},
            {16, 16, 16, 16, 16, 16, 16, 16},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {26, 26, 26, 26, 26, 26, 26, 26},
            {25, 24, 23, 22, 21, 23, 24, 25}
        };
        public Button[,] butts = new Button[8, 8]; //массив кнопок, отображающих фигуры
        public int currPlayer; //номер текущего игрока
        public bool isMoving = false; //переменнаа, которая показывает выбрана ли правильная фигура
        public Button prevButton; //переменная хранящяя предыдущую нажатую кнопку
        public Form1() // сама программа
        {
            InitializeComponent(); //подключение компонентов
            chessSprites = new Bitmap("C:\\programki\\ChessGAMEVS\\ChessGame\\chess.png"); //подключение спрайтов фигур

            Init(); //инициализация
        }
        public void Init()
        {
            CreateMap();
            currPlayer = 1;
        }
        public void CreateMap() //начальная расстановка фигур
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    butts[i, j] = new Button();
                    Button butt = new Button();
                    butt.Size = new Size(50, 50);
                    butt.Location = new Point(j * 50, i * 50);
                    switch (board[i, j] / 10)
                    {
                        case 1:
                            Image part = new Bitmap(50, 50);
                            Graphics g = Graphics.FromImage(part);
                            g.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0 + 150 * (board[i, j] % 10 - 1), 0, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part;
                            break;
                        case 2:
                            Image part2 = new Bitmap(50, 50);
                            Graphics g1 = Graphics.FromImage(part2);
                            g1.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0 + 150 * (board[i, j] % 10 - 1), 150, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part2;
                            break;
                    }
                    butt.BackColor = Color.White;
                    butt.Click += new EventHandler(OnFigurePress);
                    this.Controls.Add(butt);
                    butts[i, j] = butt;
                }
            }
        }

        public void OnFigurePress(object sender, EventArgs e) // обработчик нажатий на кнопки
        {
            if (prevButton != null)
                prevButton.BackColor = Color.White;
            
            Button pressedButton = sender as Button;
            if (board[pressedButton.Location.Y / 50, pressedButton.Location.X / 50] != 0 && board[pressedButton.Location.Y / 50, pressedButton.Location.X / 50]/10 == currPlayer)
            {
                CloseSteps();
                pressedButton.BackColor = Color.Red;
                DeactivateAllButtons();
                pressedButton.Enabled = true;
                ShowSteps(pressedButton.Location.Y / 50, pressedButton.Location.X / 50, board[pressedButton.Location.Y / 50, pressedButton.Location.X / 50]);
                if (isMoving)
                {
                    CloseSteps();
                    pressedButton.BackColor = Color.White;
                    ActivateAllButtons();
                    isMoving = false;
                }
                else
                    isMoving = true;
            }
            else           //ход фигурой
            {
                if (isMoving) 
                {
                    int temp = board[pressedButton.Location.Y / 50, pressedButton.Location.X / 50];
                    board[pressedButton.Location.Y / 50, pressedButton.Location.X / 50] = board[prevButton.Location.Y / 50, prevButton.Location.X / 50];
                    board[prevButton.Location.Y / 50, prevButton.Location.X / 50] = temp;
                    pressedButton.BackgroundImage = prevButton.BackgroundImage;
                    prevButton.BackgroundImage = null;
                    isMoving = false;
                    CloseSteps();
                    ActivateAllButtons();
                    switchPlayer();
                }
            }
            prevButton = pressedButton;
        }
        public void switchPlayer() //переключатель хода игрока
        {
            currPlayer = currPlayer == 2 ? 1 : 2;
        }

        public void DeactivateAllButtons() 
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].Enabled = false;
                }
            }
        }

        public void ActivateAllButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].Enabled = true;
                }
            }
        }

        public bool InBorder(int i, int j)
        {
            if (i >= 8 || i < 0 || j >= 8 || j < 0)
                return false;
            return true;
        }

        public void CloseSteps()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].BackColor = Color.White;
                }
            }
        }
        public void ShowSteps(int i, int j, int currFigure)
        {
            switch (currFigure%10)
            {
                case 6:
                    ShowPawnSteps(i, j);
                    break;
                case 5:
                    ShowVertHorizlSteps(i, j);
                    break;
                case 4:
                    ShowHorseSteps(i, j);
                    break;
                case 3:
                    ShowDiagonal(i, j);
                    break;
                case 2:
                    ShowDiagonal(i, j);
                    ShowVertHorizlSteps(i, j);
                    break;
                case 1:
                    ShowVertHorizlSteps(i, j, true);
                    ShowDiagonal(i, j, true);
                    break;

            }
        }

        public void ShowPawnSteps(int i, int j)
        {
            int dir = currPlayer == 1 ? 1 : -1;
            if (InBorder(i + 1 * dir, j))
            {
                if (board[i + 1 * dir, j] == 0)
                {
                    butts[i + 1 * dir, j].BackColor = Color.Yellow;
                    butts[i + 1 * dir, j].Enabled = true;
                }
            }
            if (InBorder(i + 2 * dir, j) && (i == 1 || i == 6))
            {
                if (board[i + 2 * dir, j] == 0)
                {
                    butts[i + 2 * dir, j].BackColor = Color.Yellow;
                    butts[i + 2 * dir, j].Enabled = true;
                }
            }
            if (InBorder(i + 1 * dir, j + 1))
            {
                if (board[i + 1 * dir, j + 1] != 0 && board[i + 1 * dir, j + 1] / 10 != currPlayer)
                {
                    butts[i + 1 * dir, j + 1].BackColor = Color.Yellow;
                    butts[i + 1 * dir, j + 1].Enabled = true;
                }
            }
            if (InBorder(i + 1 * dir, j - 1))
            {
                if (board[i + 1 * dir, j - 1] != 0 && board[i + 1 * dir, j - 1] / 10 != currPlayer)
                {
                    butts[i + 1 * dir, j - 1].BackColor = Color.Yellow;
                    butts[i + 1 * dir, j - 1].Enabled = true;

                }
            }
        }

        public void ShowVertHorizlSteps(int icur, int jcur, bool isOneStep = false)
        {
            for (int i = icur + 1; i < 8; i++) // проверка по горизонтали
            {
                if(InBorder(i,jcur))
                    if (!DeterminePath(i, jcur))
                        break;
                if (isOneStep)
                    break;
            }
            for (int i = icur - 1; i >= 0; i--)
            {
                if (InBorder(i, jcur))
                {
                    if (!DeterminePath(i, jcur))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int j = jcur + 1; j < 8; j++)
            {
                if (InBorder(icur, j))
                {
                    if (!DeterminePath(icur, j))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int j = jcur - 1; j >= 0; j--)
            {
                if (InBorder(icur, j))
                {
                    if (!DeterminePath(icur, j))
                        break;
                }
                if (isOneStep)
                    break;
            }
        }
        public void ShowDiagonal(int IcurrFigure, int JcurrFigure, bool isOneStep = false)
        {
            int j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (InBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (InBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
        }
        public void ShowHorseSteps(int i, int j)
        {
            if (InBorder(i - 2, j + 1))
            {
                DeterminePath(i - 2, j + 1);
            }
            if (InBorder(i - 2, j - 1))
            {
                DeterminePath(i - 2, j - 1);
            }
            if (InBorder(i + 2, j + 1))
            {
                DeterminePath(i + 2, j + 1);
            }
            if (InBorder(i + 2, j - 1))
            {
                DeterminePath(i + 2, j - 1);
            }
            if (InBorder(i - 1, j + 2))
            {
                DeterminePath(i - 1, j + 2);
            }
            if (InBorder(i + 1, j + 2))
            {
                DeterminePath(i + 1, j + 2);
            }
            if (InBorder(i - 1, j - 2))
            {
                DeterminePath(i - 1, j - 2);
            }
            if (InBorder(i + 1, j - 2))
            {
                DeterminePath(i + 1, j - 2);
            }
        }

        public bool DeterminePath(int i, int j)
        {
            if (board[i, j] == 0)
            {
                butts[i, j].BackColor = Color.Yellow;
                butts[i, j].Enabled = true;
            }
            else
            {
                if (board[i, j] / 10 != currPlayer)
                {
                    butts[i, j].BackColor = Color.Yellow;
                    butts[i, j].Enabled = true;
                }
                return false;
            }
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
