using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Многоугольники
{
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
