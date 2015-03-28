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
    public partial class AdvancedDialog : Form
    {
        public AdvancedDialog()
        {
            InitializeComponent();
        }

        public string VolumeIdentifier { get { return txtVolume.Text; } set { txtVolume.Text = value; } }
        public string SystemIdentifier { get { return txtSystem.Text; } set { txtSystem.Text = value; } }
        public string VolumeSetIdentifier { get { return txtVolumeSet.Text; } set { txtVolumeSet.Text = value; } }
        public string PublisherIdentifier { get { return txtPublisher.Text; } set { txtPublisher.Text = value; } }
        public string DataPreparerIdentifier { get { return txtDataPrep.Text; } set { txtDataPrep.Text = value; } }
        public string ApplicationIdentifier { get { return txtApplication.Text; } set { txtApplication.Text = value; } }
        public bool TruncateMode { get { return chkTruncateMode.Checked; } set { chkTruncateMode.Checked = value; } }
    }
}
