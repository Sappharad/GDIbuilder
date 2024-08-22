using System;
using DiscUtils.Gdrom;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using DiscUtils.Iso9660;

namespace GDIbuilder
{
    public partial class GDIBuilderForm : Form
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

        public GDIBuilderForm()
        {
            InitializeComponent();
        }

        private void btnSelectData_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtData.Text = fbd.SelectedPath;
            }
        }

        private void btnSelectIP_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "IP.BIN (*.bin)|*.bin";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtIpBin.Text = ofd.FileName;
            }
        }

        private void btnSelCdda_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Raw CDDA (*.raw)|*.raw";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    if (file.EndsWith("track02.raw", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        MessageBox.Show(this, "This tool is only for building the high density area of a GD-ROM. " +
                            "You selected Track 2, which is part of the PC readable portion of the disc. " +
                            "This file has been automatically ignored. Please read the instructions for more information.",
                            "A mistake was made", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        lstCdda.Items.Add(new CddaItem { FilePath = file });
                    }
                }
            }
        }

        private void btnSelOutput_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtOutdir.Text = fbd.SelectedPath;
            }
        }

        private void EnableOrDisableButtons(bool disable)
        {
            btnAddCdda.Enabled = !disable;
            btnAdvanced.Enabled = !disable;
            btnSelectData.Enabled = !disable;
            btnSelectIP.Enabled = !disable;
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
            if (txtData.Text.Length > 0 && txtIpBin.Text.Length > 0 && txtOutdir.Text.Length > 0)
            {
                List<string> cdTracks = new List<string>();
                foreach (CddaItem lvi in lstCdda.Items)
                {
                    cdTracks.Add(lvi.FilePath);
                }
                string checkMsg = GDromBuilder.CheckOutputExists(cdTracks, txtOutdir.Text);
                if (checkMsg != null)
                {
                    if (MessageBox.Show(checkMsg, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
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
                    RawMode = chkRawMode.Checked,
                    ReportProgress = UpdateProgress
                };
                _cancelTokenSource = new CancellationTokenSource();
                _worker = new Thread(() => DoDiscBuild(txtData.Text, txtOutdir.Text));
                _worker.Start();
            }
            else
            {
                MessageBox.Show("Not ready to build disc. Please provide more information above.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    Invoke(new Action(() =>
                    {
                        EnableOrDisableButtons(disable: false);
                        pbProgress.Value = 0;
                    }));
                }
                else
                {
                    Invoke(new Action(() =>
                    {
                        string gdiPath = System.IO.Path.Combine(outdir, "disc.gdi");
                        if (System.IO.File.Exists(gdiPath))
                        {
                            _builder.UpdateGdiFile(tracks, gdiPath);
                        }
                        ResultDialog rd = new ResultDialog(_builder.GetGDIText(tracks));
                        rd.ShowDialog();
                        Close();
                    }));
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() =>
                {
                    MessageBox.Show("Failed to build disc.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Invoke(new Action(() => { pbProgress.Value = percent; }));
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
                object item = lstCdda.Items[idx];
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
                object item = lstCdda.Items[idx];
                lstCdda.Items.RemoveAt(idx);
                lstCdda.Items.Insert(idx + 1, item);
                lstCdda.SelectedIndex = idx + 1;
            }
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            using AdvancedDialog adv = new AdvancedDialog();
            adv.VolumeIdentifier = _volumeIdentifier;
            adv.SystemIdentifier = _systemIdentifier;
            adv.VolumeSetIdentifier = _volumeSetIdentifier;
            adv.PublisherIdentifier = _publisherIdentifier;
            adv.DataPreparerIdentifier = _dataPreparerIdentifier;
            adv.ApplicationIdentifier = _applicationIdentifier;
            adv.TruncateMode = _truncateData;
            if (adv.ShowDialog() == DialogResult.OK)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _cancelTokenSource?.Cancel();
        }
    }

    public class CddaItem
    {
        public string FilePath { get; set; }
        public override string ToString()
        {
            return System.IO.Path.GetFileName(FilePath);
        }
    }
}
