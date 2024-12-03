using Eto.Drawing;
using Eto.Forms;

namespace GDIBuilder2
{
    public class BuildAdvancedDialog : Dialog
    {
        #region Properties
        public string VolumeIdentifier { get { return txtVolume.Text; } set { txtVolume.Text = value; } }
        public string SystemIdentifier { get { return txtSystem.Text; } set { txtSystem.Text = value; } }
        public string VolumeSetIdentifier { get { return txtVolumeSet.Text; } set { txtVolumeSet.Text = value; } }
        public string PublisherIdentifier { get { return txtPublisher.Text; } set { txtPublisher.Text = value; } }
        public string DataPreparerIdentifier { get { return txtDataPrep.Text; } set { txtDataPrep.Text = value; } }
        public string ApplicationIdentifier { get { return txtApplication.Text; } set { txtApplication.Text = value; } }
        public bool TruncateMode { get { return chkTruncateMode.Checked == true; } set { chkTruncateMode.Checked = value; } }
        public DialogResult DialogResult { get; set; } = DialogResult.Cancel;
        #endregion
        
        #region Controls
        private Button btnOK = new Button { Text = "OK" };
        private Button btnCancel = new Button { Text = "Cancel" };
        private TextBox txtVolume = new TextBox();
        private TextBox txtSystem = new TextBox();
        private TextBox txtVolumeSet = new TextBox();
        private TextBox txtPublisher = new TextBox();
        private TextBox txtDataPrep = new TextBox();
        private TextBox txtApplication = new TextBox();
        private Label lblVolume = new Label() { Text = "Volume ID:" };
        private Label lblSystem = new Label() { Text = "System ID:" };
        private Label lblVolSet = new Label() { Text = "Volume Set ID:" };
        private Label lblPublisher = new Label() { Text = "Publisher ID:" };
        private Label lblDataPrep = new Label() { Text = "Data Preparer:" };
        private Label lblApplication = new Label() { Text = "Application ID:" };
        private CheckBox chkTruncateMode = new CheckBox() { Text = "Generate truncated track03.bin" };
        #endregion
        
        public BuildAdvancedDialog()
        {
            InitializeComponent();
        }

        #region Component Init
        private void InitializeComponent()
        {
            DisplayMode = DialogDisplayMode.Attached;
            Title = "Advanced";
            MinimumSize = new Size(560, 155);
            Padding = new Padding(4, 3, 4, 3);
            btnOK.Click += (sender, e) =>
            {
                DialogResult = DialogResult.Ok;
                Close();
            };
            btnCancel.Click += (sender, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
            DefaultButton = btnOK;
            AbortButton = btnCancel;
            
            DynamicLayout topTable = new DynamicLayout();
            topTable.Padding = new Padding(5, 5, 5, 5);
            topTable.Spacing = new Size(6, 6);
            topTable.BeginHorizontal();
            topTable.Add(lblVolume);
            txtVolume.Width = 200;
            topTable.Add(txtVolume);
            topTable.Add(lblPublisher);
            txtPublisher.Width = 200;
            topTable.Add(txtPublisher);
            topTable.EndHorizontal();
            topTable.BeginHorizontal();
            topTable.Add(lblSystem);
            topTable.Add(txtSystem);
            topTable.Add(lblDataPrep);
            topTable.Add(txtDataPrep);
            topTable.EndHorizontal();
            topTable.BeginHorizontal();
            topTable.Add(lblVolSet);
            topTable.Add(txtVolumeSet);
            topTable.Add(lblApplication);
            topTable.Add(txtApplication);
            topTable.EndHorizontal();
            
            DynamicLayout completeLayout = new DynamicLayout() { Padding = 6 };
            completeLayout.Add(topTable, true);
            completeLayout.AddCentered(chkTruncateMode);
            completeLayout.Add(null, false, true);
            completeLayout.Add(new StackLayout(null, btnCancel, btnOK)
                { Orientation = Orientation.Horizontal, Spacing = 5, Padding = 6 });
            Content = completeLayout;
        }
        #endregion
    }
}