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
        public bool dragged;
        public int dx = 0;
        public int dy = 0;
        static Shape()
        {
            c = Color.Black;
            r = 20;
        }
        public Shape(int x, int y, int R, bool dragged)
        {
            this.x = x;
            this.y = y;
            r = R;
            this.dragged = dragged;
        }
        public abstract void Draw(PaintEventArgs e);
        public abstract bool IsInside(int x, int y);
    }
    class Circle : Shape
    {
        public Circle(int x, int y, int R, bool dragged) : base(x, y, R, dragged) { }
        public override void Draw(PaintEventArgs e)
        {
            Brush B = new SolidBrush(c);
            e.Graphics.FillEllipse(B, x - r, y - r, 2 * r, 2 * r);
        }
        public override bool IsInside(int x, int y)
        {
            double d = Math.Sqrt(Math.Pow(x - this.x, 2) + Math.Pow(y - this.y, 2));
            return d <= r;
        }
    }
    class Square : Shape
    {
        public Square(int x, int y, int R, bool dragged) : base(x, y, R, dragged) { }
        public override void Draw(PaintEventArgs e)
        {
            Brush B = new SolidBrush(c);
            int d = (int)(r * Math.Sqrt(2) / 2);
            e.Graphics.FillRectangle(B, x - d, y - d, 2*d, 2*d);
        }
        public override bool IsInside(int x, int y)
        {
            double d = r * Math.Sqrt(2) / 2;
            return Math.Abs(this.x - x) <= d && Math.Abs(this.y - y) <= d;
        }
    }
    class Triangle : Shape
    {
        public Triangle(int x, int y, int R, bool dragged) : base(x, y, R, dragged) { }
        public override void Draw(PaintEventArgs e)
        {
            Brush B = new SolidBrush(c);
            Point[] curvePoints = new Point[3];
            curvePoints[0] = new Point(x, y - r);
            curvePoints[1] = new Point(x + (int)(r * Math.Sqrt(3) / 2), y + r / 2);
            curvePoints[2] = new Point(x - (int)(r * Math.Sqrt(3) / 2), y + r / 2);
            e.Graphics.FillPolygon(B, curvePoints);
        }
        public override bool IsInside(int x, int y)
        {
            return false;
        }
    }
}
