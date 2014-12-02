using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GDIbuilder
{
    public partial class ResultDialog : Form
    {
        public ResultDialog()
        {
            InitializeComponent();
        }
        public ResultDialog(string text) : this()
        {
            txtResult.Text = text;
        }
    }
}
