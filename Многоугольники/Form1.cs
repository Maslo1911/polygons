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
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Refresh();
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
            switch (currentFigure)
            {
                case "круг":
                    figures.Add(new Circle (e.X, e.Y, 30));
                    break;
                case "квадрат":
                    figures.Add(new Square(e.X, e.Y, 30));
                    break;
                case "треугольник":
                    figures.Add(new Triangle(e.X, e.Y, 30));
                    break;
            }
            Refresh();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < figures.Count; i++)
            {
                figures[i].Draw(e);
            }
        }
    }
}
