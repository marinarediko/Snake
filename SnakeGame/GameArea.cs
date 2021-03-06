﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class GameArea : Form
    {        
        Area area = new Area();
        Snake snake = new Snake();
        Timer mainTimer = new Timer();
        Food food = new Food();
        private int score;

        public GameArea()
        {
            InitializeComponent();
            InitializeGame();
            InitializeTimer();
        }

        private void InitializeGame()
        {
            this.Height = 640;
            this.Width = 640;
            this.Controls.Add(area);
            area.Location = new Point(20, 20);
            area.Height = ClientRectangle.Height - 20 * 2;
            area.Width = ClientRectangle.Width - 20 * 2;

            this.Controls.Add(food);
            SetFoodLocation();
            score = 0;

            //adding snake body
            snake.Render(this);           
        }

        private void InitializeTimer()
        {
            mainTimer.Interval = 500;
            mainTimer.Tick += new EventHandler(MainTimer_Tick);
            mainTimer.Start();
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            if(snake.invalidMove())
                GameOver();
            snake.SnakeMove();
            SnakeFoodCollision();
            if(snake.BorderCollision(area))
                GameOver();
            snake.Render(this);
        }       

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            {
                snake.VerVelocity = -snake.Step;
                snake.HorVelocity = 0;
            }
            else if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                snake.VerVelocity = snake.Step;
                snake.HorVelocity = 0;
            }
            else if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                snake.HorVelocity = -snake.Step;
                snake.VerVelocity = 0;
            }
            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                snake.HorVelocity = snake.Step;
                snake.VerVelocity = 0;
            }
        }

        private void GameOver()
        {
            mainTimer.Stop();
            lblGameOver.Visible = true;
        }

        private void SetFoodLocation()
        {
            int jointCount;
            do
            {
                food.GetFoodLocation();
                jointCount = 0;
                foreach (var item in snake.snakePixels)
                {
                    if (item.Location != food.Location)
                        jointCount++;
                }
            }
            while (jointCount != snake.snakePixels.Count);
            food.BringToFront();
        }

        private void SnakeFoodCollision()
        {
            if (snake.snakePixels[0].Bounds.IntersectsWith(food.Bounds))
            {
                score += 10;
                SetFoodLocation();
                int left = snake.snakePixels[snake.snakePixels.Count - 1].Left;
                int top = snake.snakePixels[snake.snakePixels.Count - 1].Top;
                snake.AddPixel(left, top);
                snake.Render(this);
                //if (mainTimer.Interval >= 20)
                //    mainTimer.Interval -= 20;
            }
        }
    }
}
