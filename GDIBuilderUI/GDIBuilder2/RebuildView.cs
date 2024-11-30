using System;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using DiscUtils.Gdrom;
using System.Threading;
using DiscUtils.Iso9660;
using System.Collections.Generic;

namespace GDIBuilder2
{
    public class RebuildView : Form
    {
        private Thread _worker;
        private GDReader _reader;
        private GDromBuilder _builder;
        private CancellationTokenSource _cancelTokenSource;
        private string _volumeIdentifier = "DREAMCAST";
        private string _systemIdentifier = string.Empty;
        private string _volumeSetIdentifier = string.Empty;
        private string _publisherIdentifier = string.Empty;
        private string _dataPreparerIdentifier = string.Empty;
        private string _applicationIdentifier = string.Empty;
        private bool _truncateData = false;
        
        #region Controls
        private TextBox txtGdiPath = new TextBox() { ReadOnly = true };
        private Button btnPickGdi = new Button() { Text = "Browse..." };
        private TextBox txtDataFolder = new TextBox() { ReadOnly = true };
        private Button btnPickDataFolder = new Button() { Text = "Browse..." };
        private TextBox txtOutdir = new TextBox() { ReadOnly = true };
        private Button btnSelOutput = new Button() { Text = "Browse..." };
        private Button btnAdvanced = new Button() { Text = "Advanced Options..." };
        private CheckBox chkRawMode = new CheckBox() { Text = "Output raw sectors (2352 mode)" };
        private Button btnMake  = new Button() { Text = "Rebuild GD-ROM" };
        private Button btnCancel = new Button() { Text = "Cancel" };
        private ProgressBar pbProgress = new ProgressBar() { Value = 0 };
        #endregion
        
        
        public RebuildView()
        {
            InitializeComponent();
            Closed += (sender, args) => Application.Instance.Quit();
            var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();
            Menu = new MenuBar { QuitItem = quitCommand };
        }

        #region Component Init
        private void InitializeComponent()
        {
            Title = "Rebuild Patched GD-ROM";
            MinimumSize = new Size(540, 220);
            Padding = new Padding(4, 3, 4, 3);
            btnCancel.Enabled = false;
            btnPickDataFolder.Click += btnSelectData_Click;
            btnPickGdi.Click += btnPickGdi_Click;
            btnSelOutput.Click += btnSelOutput_Click;
            btnMake.Click += btnMake_Click;
            btnAdvanced.Click += btnAdvanced_Click;
            btnCancel.Click += btnCancel_Click;
            TableLayout topTable = new TableLayout(2, 5);
            topTable.Padding = new Padding(5, 5, 5, 5);
            topTable.Spacing = new Size(6, 6);
            topTable.SetColumnScale(1, true);
            topTable.Add(new Label { Text = "Original GDI:", TextAlignment = TextAlignment.Right }, 0, 0);
            DynamicLayout gdiRight = new DynamicLayout();
            gdiRight.BeginHorizontal();
            gdiRight.Add(txtGdiPath, true);
            gdiRight.Add(btnPickGdi);
            gdiRight.EndHorizontal();
            gdiRight.Spacing = new Size(5, 5);
            topTable.Add(gdiRight, 1, 1);
            topTable.Add(new Label { Text = "Modified files:", TextAlignment = TextAlignment.Right }, 0, 1);
            DynamicLayout dataRight = new DynamicLayout();
            dataRight.BeginHorizontal();
            dataRight.Add(txtDataFolder, true);
            dataRight.Add(btnPickDataFolder);
            dataRight.EndHorizontal();
            dataRight.Spacing = new Size(5, 5);
            topTable.Add(dataRight, 1, 0);
            topTable.Add(null, 0, 2);
            topTable.SetRowScale(2, true);
            topTable.Add(new Label { Text = "Output dir:", TextAlignment = TextAlignment.Right }, 0, 3);
            DynamicLayout outputRight = new DynamicLayout();
            outputRight.BeginHorizontal();
            outputRight.Add(txtOutdir, true);
            outputRight.Add(btnSelOutput);
            outputRight.EndHorizontal();
            outputRight.Spacing = new Size(5, 5);
            topTable.Add(outputRight, 1, 3);
            DynamicLayout advancedRow = new DynamicLayout();
            advancedRow.BeginHorizontal();
            advancedRow.Add(btnAdvanced);
            advancedRow.Add(chkRawMode);
            advancedRow.EndHorizontal();
            advancedRow.Spacing = new Size(5, 5);
            topTable.Add(advancedRow, 1, 4);
            DynamicLayout completeLayout = new DynamicLayout() { Padding = 6 };
            completeLayout.Add(topTable, true, true);
            completeLayout.Add(new StackLayout(null, btnCancel, btnMake)
                { Orientation = Orientation.Horizontal, Spacing = 5, Padding = 6 });
            completeLayout.Add(pbProgress);
            Content = completeLayout;
        }
        #endregion
        
        private void btnSelectData_Click(object sender, EventArgs e)
        {
            using (SelectFolderDialog fbd = new SelectFolderDialog())
            {
                if (fbd.ShowDialog(this) == DialogResult.Ok)
                {
                    txtDataFolder.Text = fbd.Directory;
                }
            }
        }

        private void btnPickGdi_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filters.Add(new FileFilter("GD-ROM Image (*.gdi)", new[] { "*.gdi" }));
                if (ofd.ShowDialog(this) == DialogResult.Ok)
                {
                    txtGdiPath.Text = ofd.FileName;
                }
            }
        }

        private void btnSelOutput_Click(object sender, EventArgs e)
        {
            using (SelectFolderDialog fbd = new SelectFolderDialog())
            {
                if (fbd.ShowDialog(this) == DialogResult.Ok)
                {
                    txtOutdir.Text = fbd.Directory;
                }
            }
        }

        private void EnableOrDisableButtons(bool disable)
        {
            btnAdvanced.Enabled = !disable;
            btnPickDataFolder.Enabled = !disable;
            btnPickGdi.Enabled = !disable;
            btnSelOutput.Enabled = !disable;
            btnMake.Enabled = !disable;
            chkRawMode.Enabled = !disable;

            btnCancel.Enabled = disable;
        }

        private void btnMake_Click(object sender, EventArgs e)
        {
            if (txtDataFolder.Text.Length > 0 && btnPickGdi.Text.Length > 0 && txtOutdir.Text.Length > 0)
            {
                string gdiDirectory = Path.GetDirectoryName(txtGdiPath.Text);
                if (!File.Exists(txtGdiPath.Text))
                {
                    MessageBox.Show("The .gdi file is missing?!?! Cannot continue.", "GDI Disappeared", MessageBoxButtons.OK, MessageBoxType.Error);
                    return;
                }
                if (Path.GetDirectoryName(txtGdiPath.Text).Equals(Path.GetDirectoryName(txtOutdir.Text)))
                {
                    MessageBox.Show("Cannot rebuild GDI into the same folder as the input. That would overwrite the disc we are reading files from!",
                        "Data loss attempted", MessageBoxButtons.OK, MessageBoxType.Error);
                    return;
                }
                
                List<string> cdTracks = new List<string>();
                string[] gdiLines = File.ReadAllText(btnPickGdi.Text).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (gdiLines.Length > 3 && int.TryParse(gdiLines[0], out int numTracks) && numTracks > 3)
                {
                    for (int i = 4; i < numTracks && i < gdiLines.Length; i++)
                    {
                        string track = ParseGdiTrackPath(gdiLines[i], gdiDirectory);
                        if (!File.Exists(track))
                        {
                            Console.WriteLine($"ERROR: Cannot find the CDDA track {Path.GetFileName(track)} referenced in the original .gdi file");
                            return;
                        }
                        cdTracks.Add(track);
                    }
                }
                
                string checkMsg = GDromBuilder.CheckOutputExists(cdTracks, txtOutdir.Text);
                if (checkMsg != null)
                {
                    if (MessageBox.Show(checkMsg, "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
                EnableOrDisableButtons(disable: true);
                _reader = GDReader.FromGDItext(gdiLines, gdiDirectory);
                _builder = new GDromBuilder(_reader.ReadIPBin(), cdTracks)
                {
                    VolumeIdentifier = _volumeIdentifier,
                    SystemIdentifier = _systemIdentifier,
                    VolumeSetIdentifier = _volumeSetIdentifier,
                    PublisherIdentifier = _publisherIdentifier,
                    DataPreparerIdentifier = _dataPreparerIdentifier,
                    ApplicationIdentifier = _applicationIdentifier,
                    TruncateData = _truncateData,
                    RawMode = chkRawMode.Checked == true,
                    ReportProgress = UpdateProgress
                };
                _cancelTokenSource = new CancellationTokenSource();
                _worker = new Thread(() => DoDiscBuild(gdiLines, gdiDirectory, cdTracks, txtDataFolder.Text, txtOutdir.Text));
                _worker.Start();
            }
            else
            {
                MessageBox.Show("Not ready to build disc. Please provide more information above.", "Error", MessageBoxButtons.OK);
            }
        }
        
        private static string ParseGdiTrackPath(string trackInfo, string sourceDir)
        {
            string[] pieces = trackInfo.Split(' ');
            if (pieces.Length == 6)
            {
                return Path.Combine(sourceDir, pieces[4]);
            }
            return null;
        }

        private void DoDiscBuild(string[] gdiLines, string gdiDirectory, List<string> cdTracks, string dataDir, string outdir)
        {
            try
            {
                _builder.ImportReader(_reader);
                _builder.ImportFolder(dataDir, "", true, token: _cancelTokenSource.Token);
                
                if (!Directory.Exists(outdir))
                {
                    Directory.CreateDirectory(outdir);
                }
                //Copy the PC tracks first
                for (int i = 1; i <= 2; i++)
                {
                    string srcPath = ParseGdiTrackPath(gdiLines[i], gdiDirectory);
                    if (File.Exists(srcPath))
                    {
                        File.Copy(srcPath, Path.Combine(outdir, Path.GetFileName(srcPath)), true);
                    }
                }
                //Next copy the CDDA
                foreach (string track in cdTracks)
                {
                    //We have already checked that all of these exist
                    File.Copy(track, Path.Combine(outdir, Path.GetFileName(track)), true);
                }
                //Next, save the data track(s)
                List<DiscTrack> tracks = _builder.BuildGDROM(outdir, _cancelTokenSource.Token);
                if (_cancelTokenSource.IsCancellationRequested)
                {
                    Application.Instance.Invoke(new Action(() =>
                    {
                        EnableOrDisableButtons(disable: false);
                        pbProgress.Value = 0;
                    }));
                }
                else
                {
                    Application.Instance.Invoke(new Action(() =>
                    {
                        string gdiPath = System.IO.Path.Combine(outdir, "disc.gdi");
                        _builder.WriteGdiFile(gdiLines, tracks, gdiPath);
                        MessageBox.Show("The GDI was generated successfully.", "Done!", MessageBoxButtons.OK, MessageBoxType.Information);
                        Close();
                    }));
                }
            }
            catch (Exception ex)
            {
                Application.Instance.Invoke(new Action(() =>
                {
                    MessageBox.Show("Failed to build disc.\n" + ex.Message, "Error", MessageBoxButtons.OK);
                    Close();
                }));
            }
            finally
            {
                _cancelTokenSource.Dispose();
                _cancelTokenSource = null;
            }
            _worker = null;
        }

        private void UpdateProgress(int percent)
        {
            Application.Instance.Invoke(new Action(() => { pbProgress.Value = percent; }));
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            using (BuildAdvancedDialog adv = new BuildAdvancedDialog())
            {
                adv.VolumeIdentifier = _volumeIdentifier;
                adv.SystemIdentifier = _systemIdentifier;
                adv.VolumeSetIdentifier = _volumeSetIdentifier;
                adv.PublisherIdentifier = _publisherIdentifier;
                adv.DataPreparerIdentifier = _dataPreparerIdentifier;
                adv.ApplicationIdentifier = _applicationIdentifier;
                adv.TruncateMode = _truncateData;
                adv.ShowModal(this);
                if (adv.DialogResult == DialogResult.Ok)
                {
                    _volumeIdentifier = IsoUtilities.FixAString(adv.VolumeIdentifier);
                    _systemIdentifier = IsoUtilities.FixAString(adv.SystemIdentifier);
                    _volumeSetIdentifier = IsoUtilities.FixAString(adv.VolumeSetIdentifier);
                    _publisherIdentifier = IsoUtilities.FixAString(adv.PublisherIdentifier);
                    _dataPreparerIdentifier = IsoUtilities.FixAString(adv.DataPreparerIdentifier);
                    _applicationIdentifier = IsoUtilities.FixAString(adv.ApplicationIdentifier);
                    _truncateData = adv.TruncateMode;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _cancelTokenSource?.Cancel();
        }
    }
}