using Eto.Drawing;
using Eto.Forms;

namespace GDIBuilder2
{
    public class BuildResultDialog : Dialog
    {
        #region Controls
        private Label lblIntro = new Label { Text = "GD-ROM build complete. Here is the new track info for the GDI file:" };
        private TextBox txtResult = new TextBox();
        private Button btnOK = new Button { Text = "OK" };
        private Label lblOutro = new Label { Text = "If disc.gdi exists in the output folder, this was updated for you automatically." };
        #endregion
        
        public BuildResultDialog(string text)
        {
            txtResult.Text = text;
            InitializeComponent();
        }

        #region Component Init
        private void InitializeComponent()
        {
            DisplayMode = DialogDisplayMode.Attached;
            Title = "Finished";
            MinimumSize = new Size(430, 200);
            Padding = new Padding(4, 3, 4, 3);
            btnOK.Click += (sender, e) =>
            {
                Close();
            };
            DefaultButton = btnOK;
            
            DynamicLayout completeLayout = new DynamicLayout() { Padding = 6 };
            completeLayout.Add(lblIntro);
            completeLayout.Add(txtResult, true, true);
            completeLayout.Add(lblOutro);
            completeLayout.Add(new StackLayout(null, btnOK)
                { Orientation = Orientation.Horizontal, Spacing = 5, Padding = 6 });
            Content = completeLayout;
        }
        #endregion
    }
}