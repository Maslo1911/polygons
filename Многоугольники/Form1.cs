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
        bool isJarvis;
        public Form1()
        {
            InitializeComponent();

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
        private double Cos(double x1, double x2, double y1, double y2)
        {
            return (x1 * x2 + y1 * y2) / (Math.Sqrt(Math.Pow(x1, 2) + Math.Pow(x2, 2)) * Math.Sqrt(Math.Pow(y1, 2) + Math.Pow(y2, 2)));
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < figures.Count; i++)
            {
                figures[i].Draw(e);
            }
            if (!isJarvis)
            {
                foreach (Shape i in figures)
                    i.drawLine = false;
                int cntL, cntR;
                double k, b;
                for (int i = 0; i < figures.Count; i++)
                {
                    for (int j = i + 1; j < figures.Count; j++)
                    {
                        cntL = cntR = 0;
                        k = (double)(figures[i].y - figures[j].y) / (double)(figures[i].x - figures[j].x);
                        b = figures[i].y - (k * figures[i].x);
                        for (int m = 0; m < figures.Count; m++)
                        {
                            if (m != i && m != j)
                            {
                                if (figures[m].y < k * figures[m].x + b)
                                {
                                    cntL++;
                                }
                                else
                                {
                                    cntR++;
                                }
                            }
                        }
                        if (cntR * cntL == 0 && figures[i].x != figures[j].x)
                        {
                            figures[i].drawLine = true;
                            figures[j].drawLine = true;
                            e.Graphics.DrawLine(P, figures[i].x, figures[i].y, figures[j].x, figures[j].y);
                        }
                    }
                }
            }
            else
            {
                foreach (Shape i in figures)
                    i.drawLine = false;
                int firstShape = 0;
                foreach (Shape i in figures)
                {
                    if (i.x <= figures[firstShape].x)
                        firstShape = figures.IndexOf(i);
                }
                double minCos = 1;
                int idx = 0;
                foreach (Shape i in figures)
                {
                    if (i == figures[firstShape]) continue;
                    if (Cos(0, i.x - figures[firstShape].x, -2000, i.y - figures[firstShape].y) < minCos)
                    {
                        minCos = Cos(0, i.x - figures[firstShape].x, -2000, i.y - figures[firstShape].y);
                        idx = figures.IndexOf(i);
                    }
                }
                if (figures.Count > 0)
                    e.Graphics.DrawLine(P, figures[firstShape].x, figures[firstShape].y, figures[idx].x, figures[idx].y);
                double x1 = figures[firstShape].x - figures[idx].x;
                double y1 = figures[firstShape].y - figures[idx].y;
                int currentShape = idx;
                do
                {
                    minCos = 1;
                    for (int i = 0; i < figures.Count; i++)
                    {
                        if (figures[i] == figures[currentShape] || figures[i] == figures[idx]) continue;

                        if (Cos(x1, figures[i].x - figures[currentShape].x, y1, figures[i].y - figures[currentShape].y) < minCos)
                        {
                            minCos = Cos(x1, figures[i].x - figures[currentShape].x, y1, figures[i].y - figures[currentShape].y);
                            idx = i;
                        }
                    }
                    e.Graphics.DrawLine(P, figures[currentShape].x, figures[currentShape].y, figures[idx].x, figures[idx].y);
                    figures[currentShape].drawLine = true;
                    figures[idx].drawLine = true;
                    x1 = figures[currentShape].x - figures[idx].x;
                    y1 = figures[currentShape].y - figures[idx].y;
                    currentShape = idx;
                } while (idx != firstShape);
            }
            if (figures.Count > 3)
            {
                for (int i = 0; i < figures.Count; i++)
                {
                    if (!figures[i].drawLine)
                    {
                        figures.RemoveAt(i);
                        i--;
                        Invalidate();
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

        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            if (toolStripMenuItem2.Checked)
                currentFigure = "круг";
            if (toolStripMenuItem7.Checked)
                isJarvis = false;
        }

        private void toolStripMenuItem1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (toolStripMenuItem2.Selected)
            {
                toolStripMenuItem2.Checked = true;
                toolStripMenuItem3.Checked = false;
                toolStripMenuItem4.Checked = false;
                currentFigure = "круг";
            }
            if (toolStripMenuItem3.Selected)
            {
                toolStripMenuItem3.Checked = true;
                toolStripMenuItem2.Checked = false;
                toolStripMenuItem4.Checked = false;
                currentFigure = "квадрат";
            }
            if (toolStripMenuItem4.Selected)
            {
                toolStripMenuItem4.Checked = true;
                toolStripMenuItem2.Checked = false;
                toolStripMenuItem3.Checked = false;
                currentFigure = "треугольник";
            }
        }

        private void toolStripMenuItem6_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (toolStripMenuItem7.Selected)
            {
                toolStripMenuItem7.Checked = true;
                toolStripMenuItem8.Checked = false;
                isJarvis = false;
            }
            if (toolStripMenuItem8.Selected)
            {
                toolStripMenuItem8.Checked = true;
                toolStripMenuItem7.Checked = false;
                isJarvis = true;
            }
        }
    }
}
