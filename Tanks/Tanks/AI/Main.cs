﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Tanks.AI
{
    public partial class Main : Form
    {
        Play gameAI = new Play();
        
        public Main()
        {
            InitializeComponent();
            Thread x = new Thread(gameAI.startGame);
            x.Start();
        }

        private void start_btn_Click(object sender, EventArgs e)
        {
            Program.connection.sendData("JOIN#");
            Program.state.gameState = 1;
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.connection.sendData("SHOOT#");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.connection.sendData("UP#");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.connection.sendData("DOWN#");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Program.connection.sendData("RIGHT#");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Program.connection.sendData("LEFT#");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Program.state.my_tank_data == null) return;
            label1.Text = Program.state.my_tank_data.x.ToString() + " : " + Program.state.my_tank_data.y.ToString() + " : " + Program.state.my_tank_data.direction.ToString();

            label4.Text = "Player " + Program.state.my_tank;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            gameAI.move.x = int.Parse(x_txt.Text);
            gameAI.move.y = int.Parse(y_txt.Text);
            gameAI.move.goToSquare();

        }

 
    }
}
