using System;
using AppKit;
using Foundation;
using DiscUtils.Gdrom;
using System.Threading;
using DiscUtils.Iso9660;
using System.Collections.Generic;

namespace GDIBuilderMac
{
    public partial class ViewController : NSViewController
    {
        private CddaTrackSource _tracks;
        private GDromBuilder _builder;
        private Thread _worker;
        private CancellationTokenSource _cancelTokenSource;
        private string _volumeIdentifier = "DREAMCAST";
        private string _systemIdentifier = string.Empty;
        private string _volumeSetIdentifier = string.Empty;
        private string _publisherIdentifier = string.Empty;
        private string _dataPreparerIdentifier = string.Empty;
        private string _applicationIdentifier = string.Empty;
        private bool _truncateData = false;

        public ViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            tblCdda.DataSource = _tracks;
            pbProgress.MinValue = 0;
            pbProgress.MaxValue = 100;
            btnCancel.Hidden = true;

            // Do any additional setup after loading the view.
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        // Shared initialization code
        void Initialize()
        {
            _tracks = new CddaTrackSource();
        }

        partial void btnAddCdda_Clicked(Foundation.NSObject sender)
        {
            NSOpenPanel openPanel = NSOpenPanel.OpenPanel;
            openPanel.CanChooseFiles = true;
            openPanel.CanChooseDirectories = false;
            openPanel.AllowedFileTypes = new[] { "raw" };
            openPanel.AllowsOtherFileTypes = false;
            openPanel.AllowsMultipleSelection = true;
            openPanel.BeginSheet(View.Window, ((nint result) => {
                if (result == (int)NSPanelButtonType.Ok)
                {
                    foreach (NSUrl url in openPanel.Urls)
                    {
                        if (url.Path?.EndsWith("track02.raw", StringComparison.OrdinalIgnoreCase) == true)
                        {
                            NSAlert dialog = NSAlert.WithMessage("A mistake was made", "OK", null, null,
                                "This tool is only for building the high density area of a GD-ROM. You selected Track 2, which is part of the PC readable portion of the disc. " +
                                "This file has been automatically ignored. Please read the instructions for more information.");
                            dialog.RunModal();
                        }
                        else
                        {
                            _tracks.Rows.Add(new CddaItem(url.Path));
                        }
                    }
                    tblCdda.ReloadData();
                }
            }));
        }

        partial void btnAdvanced_Clicked(Foundation.NSObject sender)
        {
            txtVolumeId.Cell.Title = _volumeIdentifier;
            txtSystemId.Cell.Title = _systemIdentifier;
            txtVolSetId.Cell.Title = _volumeSetIdentifier;
            txtPublisherId.Cell.Title = _publisherIdentifier;
            txtDataPreparer.Cell.Title = _dataPreparerIdentifier;
            txtApplicationId.Cell.Title = _applicationIdentifier;
            chkTruncateMode.State = (_truncateData) ? NSCellStateValue.On : NSCellStateValue.Off;

            NSApplication.SharedApplication.BeginSheet(winAdvanced, View.Window);
        }

        partial void btnBrowseData_Clicked(Foundation.NSObject sender)
        {
            NSOpenPanel openPanel = NSOpenPanel.OpenPanel;
            openPanel.CanChooseDirectories = true;
            openPanel.CanChooseFiles = false;
            openPanel.BeginSheet(View.Window, ((nint result) => {
                if (result == (int)NSPanelButtonType.Ok)
                {
                    txtData.Cell.Title = openPanel.Url.Path;
                }
            }));
        }

        partial void btnBrowseIpBin_Clicked(Foundation.NSObject sender)
        {
            NSOpenPanel openPanel = NSOpenPanel.OpenPanel;
            openPanel.CanChooseFiles = true;
            openPanel.CanChooseDirectories = false;
            openPanel.AllowedFileTypes = new[] { "bin" };
            openPanel.AllowsOtherFileTypes = false;
            openPanel.BeginSheet(View.Window, ((nint result) => {
                if (result == (int)NSPanelButtonType.Ok)
                {
                    txtIpBin.Cell.Title = openPanel.Url.Path;
                }
            }));
        }

        partial void btnBrowseOutput_Clicked(Foundation.NSObject sender)
        {
            NSOpenPanel openPanel = NSOpenPanel.OpenPanel;
            openPanel.CanChooseDirectories = true;
            openPanel.CanChooseFiles = false;
            openPanel.BeginSheet(View.Window, ((nint result) => {
                if (result == (int)NSPanelButtonType.Ok)
                {
                    txtOutput.Cell.Title = openPanel.Url.Path;
                }
            }));
        }

        partial void btnCreate_Clicked(Foundation.NSObject sender)
        {
            if (txtData.Cell.Title.Length > 0 && txtIpBin.Cell.Title.Length > 0 && txtOutput.Cell.Title.Length > 0)
            {
                List<string> cdTracks = new List<string>();
                foreach (CddaItem lvi in _tracks.Rows)
                {
                    cdTracks.Add(lvi.FilePath);
                }
                string checkMsg = GDromBuilder.CheckOutputExists(cdTracks, txtOutput.Cell.Title);
                if (checkMsg != null)
                {
                    NSAlert dialog = NSAlert.WithMessage("Warning", "No", "Yes", null, checkMsg);
                    if (dialog.RunModal() == 1)
                    {
                        return;
                    }
                }

                EnableOrDisableButtons(disable: true);
                string dataPath = txtData.Cell.Title;
                string ipBinPath = txtIpBin.Cell.Title;
                string outputPath = txtOutput.Cell.Title;
                _builder = new GDromBuilder(ipBinPath, cdTracks)
                {
                    VolumeIdentifier = _volumeIdentifier,
                    SystemIdentifier = _systemIdentifier,
                    VolumeSetIdentifier = _volumeSetIdentifier,
                    PublisherIdentifier = _publisherIdentifier,
                    DataPreparerIdentifier = _dataPreparerIdentifier,
                    ApplicationIdentifier = _applicationIdentifier,
                    TruncateData = (chkTruncateMode.State == NSCellStateValue.On),
                    RawMode = (chkRawMode.State == NSCellStateValue.On),
                    ReportProgress = UpdateProgress
                };
                _cancelTokenSource = new CancellationTokenSource();
                _worker = new Thread(() => DoDiscBuild(dataPath, outputPath));
                _worker.Start();
            }
            else
            {
                NSAlert.WithMessage("Error", "OK", null, null, "Not ready to build disc. Please provide more information above.").RunModal();
            }
        }

        partial void btnCancel_Clicked(Foundation.NSObject sender)
        {
            _cancelTokenSource?.Cancel();
        }

        partial void btnDown_Clicked(Foundation.NSObject sender)
        {
            if (tblCdda.SelectedRow >= 0 && tblCdda.SelectedRow < _tracks.Rows.Count - 1)
            {
                int idx = (int)tblCdda.SelectedRow;
                CddaItem item = _tracks.Rows[idx];
                _tracks.Rows.RemoveAt(idx);
                _tracks.Rows.Insert(idx + 1, item);
                tblCdda.ReloadData();
                tblCdda.SelectRow(idx + 1, false);
            }
        }

        partial void btnRemoveCdda_Clicked(Foundation.NSObject sender)
        {
            if (tblCdda.SelectedRow >= 0 && tblCdda.SelectedRow < _tracks.Rows.Count)
            {
                _tracks.Rows.RemoveAt((int)tblCdda.SelectedRow);
                tblCdda.ReloadData();
            }
        }

        partial void btnUp_Clicked(Foundation.NSObject sender)
        {
            if (tblCdda.SelectedRow >= 0)
            {
                int idx = (int)tblCdda.SelectedRow;
                CddaItem item = _tracks.Rows[idx];
                _tracks.Rows.RemoveAt(idx);
                _tracks.Rows.Insert(idx - 1, item);
                tblCdda.ReloadData();
                tblCdda.SelectRow(idx - 1, false);
            }
        }

        partial void btnAdvancedCancel_Clicked(Foundation.NSObject sender)
        {
            NSApplication.SharedApplication.EndSheet(winAdvanced);
            winAdvanced.OrderOut(winAdvanced);
        }

        partial void btnAdvancedOk_Clicked(Foundation.NSObject sender)
        {
            _volumeIdentifier = IsoUtilities.FixAString(txtVolumeId.Cell.Title);
            _systemIdentifier = IsoUtilities.FixAString(txtSystemId.Cell.Title);
            _volumeSetIdentifier = IsoUtilities.FixAString(txtVolSetId.Cell.Title);
            _publisherIdentifier = IsoUtilities.FixAString(txtPublisherId.Cell.Title);
            _dataPreparerIdentifier = IsoUtilities.FixAString(txtDataPreparer.Cell.Title);
            _applicationIdentifier = IsoUtilities.FixAString(txtApplicationId.Cell.Title);
            _truncateData = (chkTruncateMode.State == NSCellStateValue.On);

            NSApplication.SharedApplication.EndSheet(winAdvanced);
            winAdvanced.OrderOut(winAdvanced);
        }

        partial void btnFinishedOk_Clicked(Foundation.NSObject sender)
        {
            NSApplication.SharedApplication.EndSheet(winAdvanced);
            winAdvanced.OrderOut(winAdvanced);
            View.Window.Close();
        }

        private void EnableOrDisableButtons(bool disable)
        {
            btnAddCdda.Enabled = !disable;
            btnAdvanced.Enabled = !disable;
            btnBrowseData.Enabled = !disable;
            btnBrowseIpBin.Enabled = !disable;
            btnBrowseOutput.Enabled = !disable;
            btnCreate.Enabled = !disable;
            btnUp.Enabled = !disable;
            btnDown.Enabled = !disable;
            btnRemoveCdda.Enabled = !disable;
            chkRawMode.Enabled = !disable;
            
            btnCancel.Hidden = !disable;
            btnCancel.Enabled = disable;
        }

        private void DoDiscBuild(string dataDir, string outdir)
        {
            try
            {
                _builder.ImportFolder(dataDir, token: _cancelTokenSource.Token);
                List<DiscTrack> tracks = _builder.BuildGDROM(outdir, _cancelTokenSource.Token);
                if (_cancelTokenSource.IsCancellationRequested)
                {
                    _cancelTokenSource.Dispose();
                    _cancelTokenSource = null;
                    InvokeOnMainThread(new Action(() =>
                    {
                        EnableOrDisableButtons(disable: false);
                        pbProgress.DoubleValue = 0;
                    }));
                }
                else
                {
                    InvokeOnMainThread(new Action(() =>
                    {
                        string gdiPath = System.IO.Path.Combine(outdir, "disc.gdi");
                        if (System.IO.File.Exists(gdiPath))
                        {
                            _builder.UpdateGdiFile(tracks, gdiPath);
                        }
                        txtResult.Cell.Title = _builder.GetGDIText(tracks);
                        NSApplication.SharedApplication.BeginSheet(winFinished, View.Window);
                    }));
                }
            }
            catch (Exception ex)
            {
                InvokeOnMainThread(new Action(() => {
                    NSAlert.WithMessage("Error", "OK", null, null, "Failed to build disc.\n" + ex.Message).RunModal();
                    NSApplication.SharedApplication.Terminate(View.Window);
                }));
            }
            _worker = null;
        }

        private void UpdateProgress(int percent)
        {
            InvokeOnMainThread(new Action(() => { pbProgress.DoubleValue = percent; }));
        }
    }

    [Register("TableViewDataSource")]
    public class CddaTrackSource : NSTableViewDataSource
    {
        public List<CddaItem> Rows { get; set; }

        public CddaTrackSource()
        {
            Rows = new List<CddaItem>();
        }

        // This method will be called by the NSTableView control to learn the number of rows to display.
        [Export("numberOfRowsInTableView:")]
        public int NumberOfRowsInTableView(NSTableView table)
        {
            return Rows.Count;
        }

        // This method will be called by the control for each column and each row.
        [Export("tableView:objectValueForTableColumn:row:")]
        public NSObject ObjectValueForTableColumn(NSTableView table, NSTableColumn col, int row)
        {
            if (row < Rows.Count)
            {
                return new NSString(Rows[row].ToString());
            }
            return new NSString();
        }
    }

    public class CddaItem
    {
        public CddaItem(string path)
        {
            FilePath = path;
        }
        public string FilePath { get; set; }
        public override string ToString()
        {
            return System.IO.Path.GetFileName(FilePath);
        }
    }
}
