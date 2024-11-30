using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading;
using DiscUtils.Gdrom;
using DiscUtils.Iso9660;
using Eto.Forms;
using Eto.Drawing;

namespace GDIBuilder2
{
    public class BuildView : Form
    {
        private Thread _worker;
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
        private TextBox txtDataFolder = new TextBox() { ReadOnly = true };
        private Button btnPickDataFolder = new Button() { Text = "Browse..." };
        private TextBox txtIpBin = new TextBox() { ReadOnly = true };
        private Button btnPickIpBin = new Button() { Text = "Browse..." };
        private Button btnMoveCddaUp = new Button() { Text = "Up" };
        private Button btnMoveCddaDown = new Button() { Text = "Down" };
        private ListBox lstCdda = new ListBox() { Height = 110 };
        private Button btnAddCdda = new Button() { Text = "Add..." };
        private Button btnRemoveCdda = new Button() { Text = "Remove" };
        private TextBox txtOutdir = new TextBox() { ReadOnly = true };
        private Button btnSelOutput = new Button() { Text = "Browse..." };
        private Button btnAdvanced = new Button() { Text = "Advanced Options..." };
        private CheckBox chkRawMode = new CheckBox() { Text = "Output raw sectors (2352 mode)" };
        private Button btnMake  = new Button() { Text = "Create GD-ROM" };
        private Button btnCancel = new Button() { Text = "Cancel" };
        private ProgressBar pbProgress = new ProgressBar() { Value = 0 };
        #endregion
        
        public BuildView()
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
            Title = "Build GDI Tracks";
            MinimumSize = new Size(540, 360);
            Padding = new Padding(4, 3, 4, 3);
            StackLayout cddaLeft = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Items =
                {
                    new Label { Text = "CDDA:" },
                    btnMoveCddaUp, btnMoveCddaDown
                },
                Spacing = 5,
                HorizontalContentAlignment = HorizontalAlignment.Right
            };
            StackLayout cddaButtons = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Items = { btnAddCdda, btnRemoveCdda },
                Spacing = 5
            };
            btnCancel.Enabled = false;
            btnPickDataFolder.Click += btnSelectData_Click;
            btnPickIpBin.Click += btnSelectIP_Click;
            btnAddCdda.Click += btnSelCdda_Click;
            btnRemoveCdda.Click += btnRemoveCdda_Click;
            btnMoveCddaUp.Click += btnMoveCddaUp_Click;
            btnMoveCddaDown.Click += btnMoveCddaDown_Click;
            btnSelOutput.Click += btnSelOutput_Click;
            btnMake.Click += btnMake_Click;
            btnAdvanced.Click += btnAdvanced_Click;
            btnCancel.Click += btnCancel_Click;
            TableLayout topTable = new TableLayout(2, 5);
            topTable.Padding = new Padding(5, 5, 5, 5);
            topTable.Spacing = new Size(6, 6);
            topTable.SetColumnScale(1, true);
            topTable.Add(new Label { Text = "Data files:", TextAlignment = TextAlignment.Right }, 0, 0);
            DynamicLayout dataRight = new DynamicLayout();
            dataRight.BeginHorizontal();
            dataRight.Add(txtDataFolder, true);
            dataRight.Add(btnPickDataFolder);
            dataRight.EndHorizontal();
            dataRight.Spacing = new Size(5, 5);
            topTable.Add(dataRight, 1, 0);
            topTable.Add(new Label { Text = "IP.BIN:", TextAlignment = TextAlignment.Right }, 0, 1);
            DynamicLayout ipBinRight = new DynamicLayout();
            ipBinRight.BeginHorizontal();
            ipBinRight.Add(txtIpBin, true);
            ipBinRight.Add(btnPickIpBin);
            ipBinRight.EndHorizontal();
            ipBinRight.Spacing = new Size(5, 5);
            topTable.Add(ipBinRight, 1, 1);
            topTable.Add(cddaLeft, 0, 2);
            DynamicLayout cddaRight = new DynamicLayout();
            cddaRight.BeginHorizontal();
            cddaRight.Add(lstCdda, true, true);
            cddaRight.Add(cddaButtons);
            cddaRight.EndHorizontal();
            cddaRight.Spacing = new Size(5, 5);
            topTable.Add(cddaRight, 1, 2);
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

        private void btnSelectIP_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filters.Add(new FileFilter("Binary Files (*.bin)", new[] { "*.bin" }));
                if (ofd.ShowDialog(this) == DialogResult.Ok)
                {
                    txtIpBin.Text = ofd.FileName;
                }
            }
        }

        private void btnSelCdda_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filters.Add(new FileFilter("RAW CDDA (*.raw)", new[] { "*.raw" }));
                ofd.MultiSelect = true;
                if (ofd.ShowDialog(this) == DialogResult.Ok)
                {
                    foreach (string file in ofd.Filenames)
                    {
                        if (file.EndsWith("track02.raw", StringComparison.OrdinalIgnoreCase) == true)
                        {
                            MessageBox.Show(this, "This tool is only for building the high density area of a GD-ROM. " +
                                                  "You selected Track 2, which is part of the PC readable portion of the disc. " +
                                                  "This file has been automatically ignored. Please read the instructions for more information.",
                                "A mistake was made", MessageBoxButtons.OK);
                        }
                        else
                        {
                            lstCdda.Items.Add(Path.GetFileName(file), file);
                        }
                    }
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
            btnAddCdda.Enabled = !disable;
            btnAdvanced.Enabled = !disable;
            btnPickDataFolder.Enabled = !disable;
            btnPickIpBin.Enabled = !disable;
            btnSelOutput.Enabled = !disable;
            btnMake.Enabled = !disable;
            btnMoveCddaUp.Enabled = !disable;
            btnMoveCddaDown.Enabled = !disable;
            btnRemoveCdda.Enabled = !disable;
            chkRawMode.Enabled = !disable;

            btnCancel.Enabled = disable;
        }

        private void btnMake_Click(object sender, EventArgs e)
        {
            if (txtDataFolder.Text.Length > 0 && txtIpBin.Text.Length > 0 && txtOutdir.Text.Length > 0)
            {
                List<string> cdTracks = new List<string>();
                foreach (IListItem lvi in lstCdda.Items)
                {
                    cdTracks.Add(lvi.Key);
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
                _builder = new GDromBuilder(txtIpBin.Text, cdTracks)
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
                _worker = new Thread(() => DoDiscBuild(txtDataFolder.Text, txtOutdir.Text));
                _worker.Start();
            }
            else
            {
                MessageBox.Show("Not ready to build disc. Please provide more information above.", "Error", MessageBoxButtons.OK);
            }
        }

        private void DoDiscBuild(string dataDir, string outdir)
        {
            try
            {
                _builder.ImportFolder(dataDir, token: _cancelTokenSource.Token);
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
                        if (System.IO.File.Exists(gdiPath))
                        {
                            _builder.UpdateGdiFile(tracks, gdiPath);
                        }
                        BuildResultDialog rd = new BuildResultDialog(_builder.GetGDIText(tracks));
                        rd.ShowModal(this);
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

        private void btnRemoveCdda_Click(object sender, EventArgs e)
        {
            if (lstCdda.SelectedIndex >= 0)
            {
                lstCdda.Items.RemoveAt(lstCdda.SelectedIndex);
            }
        }

        private void btnMoveCddaUp_Click(object sender, EventArgs e)
        {
            if (lstCdda.SelectedIndex > 0)
            {
                int idx = lstCdda.SelectedIndex;
                IListItem item = lstCdda.Items[idx];
                lstCdda.Items.RemoveAt(idx);
                lstCdda.Items.Insert(idx - 1, item);
                lstCdda.SelectedIndex = idx - 1;
            }
        }

        private void btnMoveCddaDown_Click(object sender, EventArgs e)
        {
            if (lstCdda.SelectedIndex >= 0 && lstCdda.SelectedIndex < lstCdda.Items.Count - 1)
            {
                int idx = lstCdda.SelectedIndex;
                IListItem item = lstCdda.Items[idx];
                lstCdda.Items.RemoveAt(idx);
                lstCdda.Items.Insert(idx + 1, item);
                lstCdda.SelectedIndex = idx + 1;
            }
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