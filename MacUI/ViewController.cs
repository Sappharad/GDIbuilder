using System;
using AppKit;
using Foundation;
using DiscUtils.Gdrom;
using System.Threading;
using System.Collections.Generic;

namespace GDIBuilderMac
{
    public partial class ViewController : NSViewController
    {
        private CddaTrackSource _tracks;
        private GDromBuilder _builder;
        private Thread _worker;

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
            _builder = new GDromBuilder();
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
                        _tracks.Rows.Add(new CddaItem(url.Path));
                    }
                    tblCdda.ReloadData();
                }
            }));
        }

        partial void btnAdvanced_Clicked(Foundation.NSObject sender)
        {
            txtVolumeId.Cell.Title = _builder.VolumeIdentifier;
            txtSystemId.Cell.Title = _builder.SystemIdentifier;
            txtVolSetId.Cell.Title = _builder.VolumeSetIdentifier;
            txtPublisherId.Cell.Title = _builder.PublisherIdentifier;
            txtDataPreparer.Cell.Title = _builder.DataPreparerIdentifier;
            txtApplicationId.Cell.Title = _builder.ApplicationIdentifier;
            chkTruncateMode.State = (_builder.TruncateData) ? NSCellStateValue.On : NSCellStateValue.Off;

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
                string checkMsg = _builder.CheckOutputExists(cdTracks, txtOutput.Cell.Title);
                if (checkMsg != null)
                {
                    NSAlert dialog = NSAlert.WithMessage("Warning", "No", "Yes", null, checkMsg);
                    if (dialog.RunModal() == 1)
                    {
                        return;
                    }
                }
                DisableButtons();
                _builder.RawMode = (chkRawMode.State == NSCellStateValue.On);
                _builder.ReportProgress = UpdateProgress;
                string dataPath = txtData.Cell.Title;
                string ipBinPath = txtIpBin.Cell.Title;
                string outputPath = txtOutput.Cell.Title;
                _worker = new Thread(() => DoDiscBuild(dataPath, ipBinPath, cdTracks, outputPath));
                _worker.Start();
            }
            else
            {
                NSAlert.WithMessage("Error", "OK", null, null, "Not ready to build disc. Please provide more information above.").RunModal();
            }
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
            _builder.VolumeIdentifier = txtVolumeId.Cell.Title;
            _builder.SystemIdentifier = txtSystemId.Cell.Title;
            _builder.VolumeSetIdentifier = txtVolSetId.Cell.Title;
            _builder.PublisherIdentifier = txtPublisherId.Cell.Title;
            _builder.DataPreparerIdentifier = txtDataPreparer.Cell.Title;
            _builder.ApplicationIdentifier = txtApplicationId.Cell.Title;
            _builder.TruncateData = (chkTruncateMode.State == NSCellStateValue.On);

            NSApplication.SharedApplication.EndSheet(winAdvanced);
            winAdvanced.OrderOut(winAdvanced);
        }

        partial void btnFinishedOk_Clicked(Foundation.NSObject sender)
        {
            NSApplication.SharedApplication.EndSheet(winAdvanced);
            winAdvanced.OrderOut(winAdvanced);
            View.Window.Close();
        }

        private void DisableButtons()
        {
            btnAddCdda.Enabled = false;
            btnAdvanced.Enabled = false;
            btnBrowseData.Enabled = false;
            btnBrowseIpBin.Enabled = false;
            btnBrowseOutput.Enabled = false;
            btnCreate.Enabled = false;
            btnUp.Enabled = false;
            btnDown.Enabled = false;
            btnRemoveCdda.Enabled = false;
            chkRawMode.Enabled = false;
        }

        private void DoDiscBuild(string dataDir, string ipBin, List<string> trackList, string outdir)
        {
            try
            {
                List<DiscTrack> tracks = _builder.BuildGDROM(dataDir, ipBin, trackList, outdir);
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
