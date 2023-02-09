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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public delegate void RadiusDelegate(object sender, RadiusEventArgs e);
        public event RadiusDelegate RadiusChanged;

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (RadiusChanged != null)
                RadiusChanged(sender, new RadiusEventArgs(trackBar1.Value));
        }
    }
}
