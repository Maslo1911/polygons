using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Многоугольники
{
    public partial class Form1 : Form
    {
        List<Shape> figures = new List<Shape> { };
        string currentFigure = "";
        bool ifIsInside = false;
        Pen P = new Pen(Color.Green, 3);
        bool isJarvis, flag;
        int dx, dy;
        Form2 f2;
        string fileName;
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
                            figures.Add(new Circle(e.X, e.Y, Shape.Radius, false));
                            break;
                        case "квадрат":
                            figures.Add(new Square(e.X, e.Y, Shape.Radius, false));
                            break;
                        case "треугольник":
                            figures.Add(new Triangle(e.X, e.Y, Shape.Radius, false));
                            break;
                    }
                }             
                Refresh();
                RemoveInsade();               
            }
        }
        private double Cos(double x1, double x2, double y1, double y2)
        {
            return (x1 * x2 + y1 * y2) / (Math.Sqrt(Math.Pow(x1, 2) + Math.Pow(y1, 2)) * Math.Sqrt(Math.Pow(x2, 2) + Math.Pow(y2, 2)));
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
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
                    if (Cos(0, i.x - figures[firstShape].x, -20000, i.y - figures[firstShape].y) < minCos)
                    {
                        minCos = Cos(0, i.x - figures[firstShape].x, -20000, i.y - figures[firstShape].y);
                        idx = figures.IndexOf(i);
                    }
                }
                figures[firstShape].drawLine = true;
                figures[idx].drawLine = true;
                if (figures.Count > 0)
                    e.Graphics.DrawLine(P, figures[firstShape].x, figures[firstShape].y, figures[idx].x, figures[idx].y);
                double x1 = figures[firstShape].x - figures[idx].x;
                double y1 = figures[firstShape].y - figures[idx].y;
                int currentShape = idx;
                do
                {
                    minCos = 1;
                    foreach (Shape i in figures)
                    {
                        if (i == figures[currentShape] || i == figures[idx]) continue;

                        if (Cos(x1, i.x - figures[currentShape].x, y1, i.y - figures[currentShape].y) < minCos)
                        {
                            minCos = Cos(x1, i.x - figures[currentShape].x, y1, i.y - figures[currentShape].y);
                            idx = figures.IndexOf(i);
                        }
                    }
                    figures[currentShape].drawLine = true;
                    figures[idx].drawLine = true;
                    e.Graphics.DrawLine(P, figures[currentShape].x, figures[currentShape].y, figures[idx].x, figures[idx].y);
                    x1 = figures[currentShape].x - figures[idx].x;
                    y1 = figures[currentShape].y - figures[idx].y;
                    currentShape = idx;
                } while (idx != firstShape);
            }
            for (int i = 0; i < figures.Count; i++)
            {
                figures[i].Draw(e);
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
                    figures[i].x += figures[i].dx;
                    figures[i].y += figures[i].dy;

                }
            }
            if (flag)
            {
                foreach (Shape i in figures)
                {
                    i.x += e.X - dx;
                    i.y += e.Y - dy;
                }
                dx += e.X - dx;
                dy += e.Y - dy;
                Refresh();               
            }
            if (doRefresh) Refresh();
        }

        private void RemoveInsade()
        {
            if (figures.Count > 3)
            {
                for (int i = 0; i < figures.Count; i++)
                {
                    if (!figures[i].drawLine)
                    {
                        dx = figures[i].x;
                        dy = figures[i].y;
                        figures.RemoveAt(i);
                        i--;
                        flag = true;
                    }
                    else flag = false;
                }
            }
            Refresh();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < figures.Count; i++)
            {
                figures[i].dragged = false;;
            }
            RemoveInsade();
            flag = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            if (circleToolStripMenuItem.Checked)
                currentFigure = "круг";
            if (standardToolStripMenuItem.Checked)
                isJarvis = false;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (figures.Count != 0)
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                MessageBoxIcon icon = MessageBoxIcon.Warning;
                DialogResult result = MessageBox.Show("Сохранить изменеия?", "Сохранить или нет", buttons, icon);
                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                    figures.RemoveRange(0, figures.Count);
                }
                else if (result == DialogResult.No)
                    figures.RemoveRange(0, figures.Count);
            }
            Refresh();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = dialog.FileName;
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream fs = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read);
                    figures = (List<Shape>)bf.Deserialize(fs);
                    Shape.Color = (Color)bf.Deserialize(fs);
                    Shape.Radius = (int)bf.Deserialize(fs);
                    fs.Close();
                }
            }
            Refresh();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileName != null)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                bf.Serialize(fs, figures);
                bf.Serialize(fs, Shape.Color);
                bf.Serialize(fs, Shape.Radius);
                fs.Close();
            }
            else 
                saveAsToolStripMenuItem_Click(sender, e);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = dialog.FileName;
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream fs = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write);
                    bf.Serialize(fs, figures);
                    bf.Serialize(fs, Shape.Color);
                    bf.Serialize(fs, Shape.Radius);
                    fs.Close();
                }
            }
        }

        private void figurestoolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (circleToolStripMenuItem.Selected)
            {
                circleToolStripMenuItem.Checked = true;
                squareToolStripMenuItem.Checked = false;
                triangleToolStripMenuItem.Checked = false;
                currentFigure = "круг";
            }
            if (squareToolStripMenuItem.Selected)
            {
                squareToolStripMenuItem.Checked = true;
                circleToolStripMenuItem.Checked = false;
                triangleToolStripMenuItem.Checked = false;
                currentFigure = "квадрат";
            }
            if (triangleToolStripMenuItem.Selected)
            {
                triangleToolStripMenuItem.Checked = true;
                circleToolStripMenuItem.Checked = false;
                squareToolStripMenuItem.Checked = false;
                currentFigure = "треугольник";
            }
        }

        private void algorithmsToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (circleToolStripMenuItem.Selected)
            {
                standardToolStripMenuItem.Checked = true;
                jarvisToolStripMenuItem.Checked = false;
                isJarvis = false;
            }
            if (squareToolStripMenuItem.Selected)
            {
                jarvisToolStripMenuItem.Checked = true;
                standardToolStripMenuItem.Checked = false;
                isJarvis = true;
            }
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult clr = colorDialog1.ShowDialog();
            if (clr == DialogResult.OK)
            {
                Shape.Color = colorDialog1.Color;
            }
            Refresh();
            MessageBox.Show("Цвет изменён");
        }

        private void radiusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (f2 == null || f2.IsDisposed)
            {
                f2 = new Form2(Shape.Radius);               
                f2.RadiusChanged += new Form2.RadiusDelegate(UpdateRadius);
                f2.Show();
            }
            else if (f2.WindowState == FormWindowState.Minimized)
            {
                f2.WindowState = FormWindowState.Normal;
            }
            else if (ActiveForm != f2)
            {
                f2.Activate();
            }
        }

        private void UpdateRadius(object sender, RadiusEventArgs e)
        {
            Shape.Radius = e.Radius;
            Refresh();
        }
    }
}
