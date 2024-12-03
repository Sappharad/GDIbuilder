using System;
using DiscUtils;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using DiscUtils.Gdrom;
using System.Threading;
using DiscUtils.Streams;
using System.Collections.Generic;

namespace GDIBuilder2
{
    public class ExtractProgressDialog : Dialog
    {
        private volatile int _currentFileIndex;
        private Thread _worker;
        private readonly GDReader _disc;
        private volatile List<ExtractionEntry> _paths;
        private readonly string _destination;
        
        #region Controls
        private Label lblExtracting = new Label() { Text = "" };
        private ProgressBar pbProgress = new ProgressBar() { Value = 0 };
        private Button btnCancel = new Button() { Text = "Cancel" };
        private UITimer tmrProgress = new UITimer();
        #endregion
        
        public ExtractProgressDialog(GDReader disc, List<string> sourcePaths, string destinationFolder)
        {
            InitializeComponent();
            _disc = disc;
            _paths = PrepareFileList(sourcePaths);
            _destination = destinationFolder;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            pbProgress.MaxValue = _paths.Count;
            if (_paths.Count > 0)
            {
                lblExtracting.Text = _paths[0].Path;
            }
            _worker = new Thread(PerformExtraction);
            _worker.Start();
            tmrProgress.Start();
        }

        private List<ExtractionEntry> PrepareFileList(List<string> paths)
        {
            List<ExtractionEntry> result = new List<ExtractionEntry>();
            foreach (string path in paths)
            {
                DiscFileSystemInfo item = _disc.GetFileSystemInfo(path);
                if (item.Exists)
                {
                    if (item.Attributes.HasFlag(FileAttributes.Directory))
                    {
                        string startFrom = path;
                        if (startFrom.Length > 1 && path[path.Length - 1] == '\\') startFrom = startFrom.Substring(0, startFrom.Length - 1);
                        if (startFrom.Length > 0) startFrom = startFrom.Substring(0, startFrom.LastIndexOf('\\'));
                        if (path.Length > 0)
                        {
                            result.Add(new ExtractionEntry(path, path.Substring(startFrom.Length + 1)) { IsDirectory = true });
                        }
                        else
                        {
                            result.Add(new ExtractionEntry(path, "") { IsDirectory = true }); //Root directory
                        }
                        string[] subdirs = _disc.GetDirectories(path, "*", SearchOption.AllDirectories);
                        foreach (string subdir in subdirs)
                        {
                            result.Add(new ExtractionEntry(subdir, subdir.Substring(startFrom.Length + 1)) { IsDirectory = true });
                        }
                        //We put the directories in the list first so that the directories can be created, and so empty directories can also be extracted.
                        string[] files = _disc.GetFiles(path, "*.*", SearchOption.AllDirectories);
                        foreach (string file in files)
                        {
                            result.Add(file.Length > path.Length
                                ? new ExtractionEntry(file, file.Substring(startFrom.Length + 1))
                                : new ExtractionEntry(file));
                        }
                    }
                    else
                    {
                        result.Add(new ExtractionEntry(path));
                    }
                }
            }
            return result;
        }
        
        #region Component Init
        private void InitializeComponent()
        {
            DisplayMode = DialogDisplayMode.Attached;
            Title = "Extracting...";
            MinimumSize = new Size(400, 100);
            Padding = new Padding(4, 3, 4, 3);
            btnCancel.Click += BtnCancelOnClick;
            tmrProgress.Interval = 0.1;
            tmrProgress.Elapsed += TmrProgressOnElapsed;
            
            DynamicLayout statusRow = new DynamicLayout();
            statusRow.BeginHorizontal();
            statusRow.Add(new Label { Text = "Extracting:", TextAlignment = TextAlignment.Right });
            statusRow.Add(lblExtracting, true);
            statusRow.EndHorizontal();
            statusRow.Spacing = new Size(5, 5);
            
            DynamicLayout bottomRow = new DynamicLayout();
            bottomRow.BeginHorizontal();
            bottomRow.Add(pbProgress, true);
            bottomRow.Add(btnCancel);
            bottomRow.EndHorizontal();
            bottomRow.Spacing = new Size(5, 5);
            
            DynamicLayout completeLayout = new DynamicLayout() { Padding = 6 };
            completeLayout.Add(statusRow, true, true);
            completeLayout.Add(bottomRow, true, false);
            completeLayout.Spacing = new Size(0, 4); 
            Content = completeLayout;
        }

        private void TmrProgressOnElapsed(object sender, EventArgs e)
        {
            int currentFile = _currentFileIndex;
            if (pbProgress.Value != currentFile)
            {
                pbProgress.Value = currentFile;
                if (currentFile < _paths.Count)
                {
                    lblExtracting.Text = _paths[currentFile].Path;
                }
            }
        }

        #endregion
        
        private void BtnCancelOnClick(object sender, EventArgs e)
        {
            _worker?.Abort();
            Close();
        }

        private void PerformExtraction()
        {
            _currentFileIndex = 0;
            foreach (ExtractionEntry path in _paths)
            {
                string platformPath = path.ExtractAs;
                if (platformPath.Length > 0 && platformPath[0] == '\\') platformPath = platformPath.Substring(1);
                if (Path.DirectorySeparatorChar != '\\')
                {
                    platformPath = platformPath.Replace('\\', Path.DirectorySeparatorChar);
                }
                string destinationPath = Path.Combine(_destination, platformPath);
                if (path.IsDirectory)
                {
                    if (!Directory.Exists(destinationPath))
                    {
                        Directory.CreateDirectory(destinationPath);
                        Directory.SetCreationTimeUtc(destinationPath, _disc.GetCreationTimeUtc(path.Path));
                        Directory.SetLastWriteTimeUtc(destinationPath, _disc.GetLastWriteTimeUtc(path.Path));
                    }
                }
                else
                {
                    using (FileStream fs = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                    {
                        using (SparseStream readFile = _disc.OpenFile(path.Path, FileMode.Open, FileAccess.Read))
                        {
                            readFile.CopyTo(fs);
                        }
                    }

                    File.SetCreationTimeUtc(destinationPath, _disc.GetCreationTimeUtc(path.Path));
                    File.SetLastWriteTimeUtc(destinationPath, _disc.GetLastWriteTimeUtc(path.Path));
                }

                Interlocked.Increment(ref _currentFileIndex);
            }
            Application.Instance.Invoke(new Action(() =>
            {
                tmrProgress.Stop();
                pbProgress.Value = pbProgress.MaxValue;
                MessageBox.Show("Done. The file(s) have been saved to the specified folder.", "Extraction Complete", MessageBoxButtons.OK);
                Close();
            }));
        }
        
    }

    class ExtractionEntry
    {
        public ExtractionEntry(string path)
        {
            Path = path;
            ExtractAs = path;
        }
        public ExtractionEntry(string path, string extractAs)
        {
            Path = path;
            ExtractAs = extractAs;
        }
        public string Path { get; set; }
        public string ExtractAs { get; set; }
        public bool IsDirectory { get; set; }
    }
}