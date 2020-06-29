using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace newtetris
{
    public partial class Form1 : Form
    {
        const int BASE_X = 12;
        const int BASE_Y = 12;

        PictureBox[,] boxs = new PictureBox[20, 10];

        int[,] remember_block = new int[20, 10];

        Timer timer = new Timer();

        Random rnd = new Random();

        int x = 0;
        int y = 0;

        int current_block = 0;
        int next_block = 0;

        //int[,] block_l = new int[4, 4]
        //{
        //    {0,0,0,0},
        //    {0,1,0,0},
        //    {0,1,1,1},
        //    {0,0,0,0}
        //};

        int[,,] block_n = new int[7, 4, 4]
        {
                {
                    {0, 0, 0, 0},
                    {0, 1, 0, 0},
                    {0, 1, 1, 1},
                    {0, 0, 0, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 1, 1, 0},
                    {0, 1, 1, 0},
                    {0, 0, 0, 0},
                },
                {
                    {0, 0, 1, 0},
                    {0, 0, 1, 0},
                    {0, 0, 1, 0},
                    {0, 0, 1, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 1, 0, 0},
                    {0, 1, 1, 0},
                    {0, 0, 1, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 0, 1, 0},
                    {0, 1, 1, 0},
                    {0, 1, 0, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 1, 1, 0},
                    {0, 0, 1, 0},
                    {0, 0, 1, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 0, 1, 0},
                    {0, 1, 1, 0},
                    {0, 0, 1, 0},
                }
         };
        Color[] block_color = new Color[8]
        {
            Color.White,
            Color.Red,
            Color.Orange,
            Color.Gold,
            Color.Green,
            Color.Blue,
            Color.Purple,
            Color.Black
        };
        Color[,] saved_color = new Color[20, 10];

        private void resetBlock()
        {
            block_n = new int[7, 4, 4] {
                  {
                    {0, 0, 0, 0},
                    {0, 1, 0, 0},
                    {0, 1, 1, 1},
                    {0, 0, 0, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 1, 1, 0},
                    {0, 1, 1, 0},
                    {0, 0, 0, 0},
                },
                {
                    {0, 0, 1, 0},
                    {0, 0, 1, 0},
                    {0, 0, 1, 0},
                    {0, 0, 1, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 1, 0, 0},
                    {0, 1, 1, 0},
                    {0, 0, 1, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 0, 1, 0},
                    {0, 1, 1, 0},
                    {0, 1, 0, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 1, 1, 0},
                    {0, 0, 1, 0},
                    {0, 0, 1, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 0, 1, 0},
                    {0, 1, 1, 0},
                    {0, 0, 1, 0},
                }
        };
    }

        public Form1()
        {
            InitializeComponent();

            for (int j = 0; j < 20; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    PictureBox box = new PictureBox();
                    box = new PictureBox();
                    box.Location = new Point((BASE_X + 2) + (i * 22), (BASE_Y + 2) + (j * 22));
                    box.Size = new Size(20, 20);

                    box.BackColor = Color.White;
                    saved_color[j, i] = Color.White;

                    Controls.Add(box);

                    box.BringToFront();

                    boxs[j, i] = box;
                }
            }

            for(int j=0;j<4;j++)
            {
                for(int i=0;i<4;i++)
                {
                    if (block_n[current_block, j, i] == 0)
                    {
                        boxs[y + j, x + i].BackColor = Color.White;
                    }
                    else
                    {
                        boxs[y + j, x + i].BackColor = block_color[current_block+1];
                    }
                    
                }
            }

            next_block = rnd.Next(7);
            drawNextBlock();

            //-----------------------Timer
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_event);
            timer.Start();
        }


        private void timer_event(object sender, EventArgs args)
        {
            move_block(0, 1);
            if(!move_block(0,1))
            {
                for(int j=0;j<4;j++)
                {
                    for(int i=0;i<4;i++)
                    {
                        if(block_n[current_block,j,i] == 1)
                        {
                            remember_block[y + j, x + i] = 1;
                            saved_color[y + j, x + i] = block_color[current_block + 1];
                        }
                    }
                }
                //--------------------Block 설치 이후
                for (int j = 0; j < 20; j++)
                {
                    int block_cnt = 0;

                    for (int i = 0; i < 10; i++)
                    {
                        if (remember_block[j, i] == 1) block_cnt++;
                    }

                    if (block_cnt == 10)
                    {
                        for (int level = j; level>0;level--)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                remember_block[level, i] = remember_block[level - 1, i];
                                saved_color[level, i] = saved_color[level - 1, i];
                            }
                        }
                    }
                }

                redraw_background();

                current_block = next_block;
                next_block = rnd.Next(7);
                drawNextBlock();

                x = y = 0;
                resetBlock();
            }
        }

        private int[,] rotate_block()
        {
            int[,] arr = new int[4, 4];
            for(int j=0; j<4;j++)
            {
                for(int i =0;i<4;i++)
                {
                    //열 - i , 행 - j
                    arr[j, i] = block_n[current_block,3 - i, j];
                }
            }
            return arr;
        }

        private bool overlap_check(int x, int y)
        {
            return overlap_check(x, y, false);
        }

        private bool overlap_check(int x,int y,bool is_rotate)
        {
            int[,] arr = new int[4, 4];
            for(int j=0;j<4;j++)
            {
                for(int i=0;i<4;i++)
                {
                    arr[j, i] = block_n[current_block, j, i];
                }
            }
            
            if (is_rotate) arr = rotate_block();

            for(int j=0;j<4;j++)
            {
                for(int i=0;i<4;i++)
                {
                    if (arr[j, i] == 1)
                    {
                        if (x + i >= 10 || x + i < 0)
                            return true;
                        if (y + j >= 20 || x + j < 0)
                            return true;
                        if (remember_block[y + j, x + i] == 1) 
                            return true;
                    }
                    
                }
            }
            return false;
        }

        private bool move_block(int x_amount, int y_amount)
        {
            if (overlap_check(x + x_amount, y + y_amount)) return false;

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (block_n[current_block,j, i] == 1)
                    {
                        boxs[y + j, x + i].BackColor = Color.White;
                    }
                }
            }

            x += x_amount;
            y += y_amount;

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (block_n[current_block,j, i] == 1)
                    {
                        boxs[y + j, x + i].BackColor = block_color[current_block+1];
                    }
                    
                }
            }
            return true;

        }

        private void redraw_background()
        {
            for(int j =0;j<20;j++)
            {
                for(int i=0;i<10;i++)
                {
                    boxs[j, i].BackColor = saved_color[j,i];
                }
            }     
        }

        private void drawNextBlock()
        {
            Bitmap map = new Bitmap(90, 90);
            for(int j=0;j<90;j++)
            {
                for(int i =0;i<90;i++)
                {
                    map.SetPixel(i, j, Color.Gray);
                }
            }

            int x = 2;
            int y = 2;

            for(int j=0;j<4;j++)
            {
                for(int i=0;i<4;i++)
                {
                    for(int h = 0;h<20;h++)
                    {
                        for(int w=0;w<20;w++)
                        {
                            if (block_n[next_block, j, i] == 1)
                                map.SetPixel(x + w, y + h, block_color[next_block + 1]);
                            else
                                map.SetPixel(x + w, y + h, block_color[0]);
                        }
                    }
                    x += 22;
                }
                x = 2;
                y += 22;
            }
            pictureBox2.Image = map;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {

                move_block(-1, 0);
            }

            else if (e.KeyCode == Keys.S)
            {
                move_block(0, 1);
            }

            else if (e.KeyCode == Keys.D)
            {
                move_block(1, 0);
            }

            else if (e.KeyCode == Keys.W)
            {
                move_block(0, -1);
            }

            else if (e.KeyCode == Keys.R)
            {
                if (!overlap_check(x, y, true))
                {
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (block_n[current_block,j, i] == 1)
                            {
                                boxs[y + j, x + i].BackColor = Color.White;
                            }

                        }
                    }
                    int[,] arr = rotate_block();

                    for(int j=0;j<4;j++)
                    {
                        for(int i=0;i<4;i++)
                        {
                            block_n[current_block, j, i] = arr[j, i];
                        }
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 4; i++)
                        {

                            if (block_n[current_block,j, i] == 1)
                                boxs[y + j, x + i].BackColor = block_color[current_block+1];
                        }
                    }
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
