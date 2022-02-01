using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vibrodiagnostic
{
    public partial class Form1 : Form
    {
        public MyHookClass simpr;
        string logMessage = "~";
        public Form1()
        {
            InitializeComponent();
            simpr = new MyHookClass(this.Handle, this);
            //timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // кривенькая реализация лога (по-хорошему надо к логу обращаться прямо из класса симпра, через f.LogTextBox)

            logMessage = simpr.solver.answer;
            //if (simpr.solver.answer != logMessage || simpr.solver.answer == "~")
            //{
            //textBox2.Text += simpr.solver.answer;
            //}
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {
                simpr.solver.trend = true;
            }
            else
            {
                simpr.solver.trend = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.CheckState == CheckState.Checked)
            {
                simpr.solver.time_Less_24 = true;
            }
            else
            {
                simpr.solver.time_Less_24 = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.CheckState == CheckState.Checked)
            {
                simpr.solver.regr_out_of_range = true;
            }
            else
            {
                simpr.solver.regr_out_of_range = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.CheckState == CheckState.Checked)
            {
                simpr.solver.antiphase_vect = true;
            }
            else
            {
                simpr.solver.antiphase_vect = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.CheckState == CheckState.Checked)
            {
                simpr.solver.levels_1_comp = true;
            }
            else
            {
                simpr.solver.levels_1_comp = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.CheckState == CheckState.Checked)
            {
                simpr.solver.levels_2_comp = true;
            }
            else
            {
                simpr.solver.levels_2_comp = false;
            }
        }


        #region методы РИСОВАНИЯ ФИГУР
        public void DrawEllipse(int x, int y, int width, int height, Color color)
        {
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(color);
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            formGraphics.DrawEllipse(myPen, new Rectangle(x, y, width, height));
            myPen.Dispose();
            //formGraphics.Dispose();

            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(color);
            formGraphics.FillEllipse(myBrush, new Rectangle(x, y, width, height));
            myBrush.Dispose();
            formGraphics.Dispose();
        }
        public void DrawRectangle(int x, int y, int width, int height, Color color)
        {
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(color);
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            formGraphics.DrawRectangle(myPen, new Rectangle(x, y, width, height));
            myPen.Dispose();
            formGraphics.Dispose();
        }

        public void DrawRectangleFill(int x, int y, int width, int height, Color color)
        {
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(color);
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            formGraphics.DrawRectangle(myPen, new Rectangle(x, y, width, height));
            myPen.Dispose();
            //formGraphics.Dispose();

            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(color);
            formGraphics.FillEllipse(myBrush, new Rectangle(x, y, width, height));
            myBrush.Dispose();
            formGraphics.Dispose();
        }

        public void DrawCircle(int x, int y, int width, int height, Color color)
        {
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(color);
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            formGraphics.DrawEllipse(myPen, new Rectangle(x, y, width, height));
            myPen.Dispose();
            formGraphics.Dispose();
        }
        #endregion

        #region РИСОВАНИЕ МУФТ ВАЛОПРОВОДА
        public void DrawMuftRVD(Color color)
        {
            DrawRectangleFill(50, 500, 50, 50, color); //checked
        }

        //public void DrawMuftRSD(Color color)
        //{
        //    DrawRectangleFill(335, 401, 50, 50, color); //checked
        //}
        #endregion

        private void DrawString(float x, float y, string drawString)
        {
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            System.Drawing.Font drawFont = new System.Drawing.Font(
                "Arial", 16);
            System.Drawing.SolidBrush drawBrush = new
                System.Drawing.SolidBrush(System.Drawing.Color.Black);
            //float x = 735.0f;
            //float y = 120.0f;
            formGraphics.DrawString(drawString, drawFont, drawBrush, x, y);
            drawFont.Dispose();
            drawBrush.Dispose();
            formGraphics.Dispose();
        }

        #region Рисование процентов
        public void DrawRVDMuftpercent(string percentage)
        {
            DrawString(150.0f, 514.0f, percentage);
        }
        //public void DrawRSDMuftpercent(string percentage)
        //{
        //    DrawString(321.0f, 514.0f, percentage);
        //}

        #endregion

        private void Form1_Shown(object sender, EventArgs e)
        {
            DrawMuftRVD(Color.LightGray);

            //System.Drawing.Graphics formGraphics = this.CreateGraphics();
            //System.Drawing.Font drawFont = new System.Drawing.Font(
            //    "Arial", 16);
            //System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            //formGraphics.DrawString("Вероятность обрыва болтов", drawFont, drawBrush, 100.0f, 470.0f); //735 370

            ////drawBrush.Color = Color.Gold;
            ////formGraphics.DrawString("Подозрение на обрыв", drawFont, drawBrush, 265.0f, 470.0f); //900 370

            //drawFont.Dispose();
            //drawBrush.Dispose();
            //formGraphics.Dispose();
        }
    }
}
