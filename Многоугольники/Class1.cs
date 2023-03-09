using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Многоугольники
{
    [Serializable]
    abstract class Shape
    {
        protected static Color c;
        protected static int r;
        public int x, y;
        [NonSerialized]
        public bool dragged;
        [NonSerialized]
        public int dx = 0;
        [NonSerialized]
        public int dy = 0;
        [NonSerialized]
        protected Point[] curvePoints;
        [NonSerialized]
        public bool drawLine;
        static Shape()
        {
            c = Color.Black;
            r = 25;
        }
        public Shape(int x, int y, int R, bool dragged)
        {
            this.x = x;
            this.y = y;
            r = R;
            this.dragged = dragged;
            drawLine = false;
        }
        public abstract void Draw(PaintEventArgs e);
        public abstract bool IsInside(int x, int y);
        public static Color Color
        {
            get { return c; }
            set { c = value; }
        }
        public static int Radius
        {
            get { return r; }
            set { r = value; }
        }
    }
    [Serializable]
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
    [Serializable]
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
    [Serializable]
    class Triangle : Shape
    {
        public Triangle(int x, int y, int R, bool dragged) : base(x, y, R, dragged) { }
        public override void Draw(PaintEventArgs e)
        {
            Brush B = new SolidBrush(c);
            curvePoints = new Point[3];
            curvePoints[0] = new Point(x, y - r);
            curvePoints[1] = new Point(x + (int)(r * Math.Sqrt(3) / 2), y + r / 2);
            curvePoints[2] = new Point(x - (int)(r * Math.Sqrt(3) / 2), y + r / 2);
            e.Graphics.FillPolygon(B, curvePoints);
        }
        public override bool IsInside(int x, int y)
        {
            double d1 = Math.Sqrt(Math.Pow(Math.Abs(curvePoints[0].X - x), 2) + Math.Pow(Math.Abs(curvePoints[0].Y - y), 2));
            double d2 = Math.Sqrt(Math.Pow(Math.Abs(curvePoints[1].X - x), 2) + Math.Pow(Math.Abs(curvePoints[1].Y - y), 2));
            double d3 = Math.Sqrt(Math.Pow(Math.Abs(curvePoints[2].X - x), 2) + Math.Pow(Math.Abs(curvePoints[2].Y - y), 2));
            double ab = Math.Sqrt(Math.Pow(Math.Abs(curvePoints[0].X - curvePoints[1].X), 2) + Math.Pow(Math.Abs(curvePoints[0].Y - curvePoints[1].Y), 2));
            double bc = Math.Sqrt(Math.Pow(Math.Abs(curvePoints[1].X - curvePoints[2].X), 2) + Math.Pow(Math.Abs(curvePoints[1].Y - curvePoints[2].Y), 2));
            double ac = Math.Sqrt(Math.Pow(Math.Abs(curvePoints[0].X - curvePoints[2].X), 2) + Math.Pow(Math.Abs(curvePoints[0].Y - curvePoints[2].Y), 2));
            double p = (ab + bc + ac) / 2;
            double p1 = (ab + d1 + d2) / 2;
            double p2 = (bc + d3 + d2) / 2;
            double p3 = (ac + d1 + d3) / 2;
            double s = Math.Sqrt(p * (p - ab) * (p - bc) * (p - ac));
            double s1 = Math.Sqrt(p1 * (p1 - ab) * (p1 - d1) * (p1 - d2));
            double s2 = Math.Sqrt(p2 * (p2 - bc) * (p2 - d2) * (p2 - d3));
            double s3 = Math.Sqrt(p3 * (p3 - ac) * (p3 - d1) * (p3 - d3));
            return Math.Abs(s - s1 - s2 - s3) <= 1;
        }
    }
    public class RadiusEventArgs
    {
        protected int radius;
        public RadiusEventArgs(int radius)
        {
            this.radius = radius;
        }
        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        }
    }
}
