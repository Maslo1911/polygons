using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Многоугольники
{
    public partial class Form1 : Form
    {
        List<Shape> figures = new List<Shape> { };
        string currentFigure = "";
        bool ifIsInside = false;
        Pen P = new Pen(Color.Green, 3);
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == comboBox1.Items[0])
            {
                currentFigure = comboBox1.Items[0].ToString();
            }
            else if (comboBox1.SelectedItem == comboBox1.Items[1])
            {
                currentFigure = comboBox1.Items[1].ToString();
            }
            else if (comboBox1.SelectedItem == comboBox1.Items[2])
            {
                currentFigure = comboBox1.Items[2].ToString();
            }
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ifIsInside = true;
                for (int i = 0; i < figures.Count; i++)
                {
                    if (figures[i].IsInside(e.X, e.Y))
                    {
                        figures.RemoveAt(i);
                        Invalidate();
                    }
                }
            }
            if (e.Button == MouseButtons.Left)
            {
                ifIsInside = false;
                for (int i = 0; i < figures.Count; i++)
                {
                    if (figures[i].IsInside(e.X, e.Y))
                    {
                        ifIsInside = true;
                        figures[i].dragged = true;
                        figures[i].dx = figures[i].x - e.X;
                        figures[i].dy = figures[i].y - e.Y;
                    }
                }
                if (!ifIsInside)
                {
                    switch (currentFigure)
                    {
                        case "круг":
                            figures.Add(new Circle(e.X, e.Y, 30, false));
                            break;
                        case "квадрат":
                            figures.Add(new Square(e.X, e.Y, 30, false));
                            break;
                        case "треугольник":
                            figures.Add(new Triangle(e.X, e.Y, 30, false));
                            break;
                    }
                }
            }
            Refresh();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            bool upper, lower;
            double k, b;
            for (int i = 0; i < figures.Count; i++)
            {
                figures[i].Draw(e);
            }
            for (int i = 0; i < figures.Count; i++)
            {
                for (int j = i + 1; j < figures.Count; j++)
                {
                    upper = false;
                    lower = false;
                    k = (double)(figures[i].y - figures[j].y) / (double)(figures[i].x - figures[j].x);
                    b = figures[i].y - (k * figures[i].x);
                    for (int m = 0; m < figures.Count; m++)
                    {
                        if (m!=i && m!=j)
                        {
                            if (figures[m].y < k * figures[m].x + b)
                            {
                                upper = true;
                            }
                            else
                            {
                                lower = true;
                            }
                        }
                    }
                    if (upper == false || lower == false)
                    {
                        e.Graphics.DrawLine(P, figures[i].x, figures[i].y, figures[j].x, figures[j].y);
                    }
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            bool doRefresh = false;
            for (int i = 0; i < figures.Count; i++)
            {
                if (figures[i].dragged)
                {
                    doRefresh = true;
                    figures[i].x = e.X;
                    figures[i].y = e.Y;
                    if (figures[i].IsInside(e.X, e.Y))
                    {
                        figures[i].x += figures[i].dx;
                        figures[i].y += figures[i].dy;
                    }
                }

            }
            if (doRefresh) Refresh();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < figures.Count; i++)
            {
                figures[i].dragged = false;
            }
        }
    }
}
