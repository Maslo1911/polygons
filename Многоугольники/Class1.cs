using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Многоугольники
{
    abstract class Shape
    {
        protected static Color c;
        protected static int r;
        public int x, y;
        static Shape()
        {
            c = Color.Black;
            r = 20;
        }
        public Shape(int x, int y, int R)
        {
            this.x = x;
            this.y = y;
            r = R;
        }
        public abstract void Draw(PaintEventArgs e);
    }
    class Circle : Shape
    {
        public Circle(int x, int y, int R) : base(x, y, R) { }
        public override void Draw(PaintEventArgs e)
        {
            Brush B = new SolidBrush(c);
            e.Graphics.FillEllipse(B, x - r, y - r, 2 * r, 2 * r);
        }
    }
    class Square : Shape
    {
        public Square(int x, int y, int R) : base(x, y, R) { }
        public override void Draw(PaintEventArgs e)
        {
            Brush B = new SolidBrush(c);
            int d = (int)(r * Math.Sqrt(2) / 2);
            e.Graphics.FillRectangle(B, x - d, y - d, 2*d, 2*d);
        }
    }
    class Triangle : Shape
    {
        public Triangle(int x, int y, int R) : base(x, y, R) { }
        public override void Draw(PaintEventArgs e)
        {
            Brush B = new SolidBrush(c);
            Point[] curvePoints = new Point[3];
            curvePoints[0] = new Point(x, y - r);
            curvePoints[1] = new Point(x + (int)(r * Math.Sqrt(3) / 2), y + r / 2);
            curvePoints[2] = new Point(x - (int)(r * Math.Sqrt(3) / 2), y + r / 2);
            e.Graphics.FillPolygon(B, curvePoints);
        }
    }
}
