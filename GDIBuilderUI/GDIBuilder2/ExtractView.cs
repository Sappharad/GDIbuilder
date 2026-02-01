using System;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using DiscUtils.Gdrom;
using System.Threading;
using DiscUtils.Iso9660;
using System.Collections.Generic;
using System.Reflection;

namespace GDIBuilder2
{
    public class ExtractView : Form
    {
        private GDReader _currentDisc;
        
        #region Controls
        private TextBox txtGdiPath = new TextBox() { ReadOnly = true };
        private Button btnPickGdi = new Button() { Text = "Open..." };
        private TreeGridView tgvDisc = new TreeGridView();
        private Bitmap bmpFile = null;
        private Bitmap bmpFolder = null;
        private Button btnSaveIpBin = new Button() { Text = "Extract IP.BIN..." };
        private Button btnSaveSelected = new Button() { Text = "Extract selection..." };
        private Button btnSaveAll = new Button() { Text = "Extract all files..." };
        #endregion
        
        public ExtractView()
        {
            InitializeComponent();
            Closed += (sender, args) => Application.Instance.Quit();
            var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();
            Menu = new MenuBar { QuitItem = quitCommand };
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose();
            _currentDisc?.Dispose();
        }

        #region Component Init
        private void InitializeComponent()
        {
            btnSaveIpBin.Enabled = false;
            btnSaveIpBin.Click += btnSaveIpBin_Click;
            btnSaveSelected.Enabled = false;
            btnSaveSelected.Click += btnSaveSelected_Click;
            btnSaveAll.Enabled = false;
            btnSaveAll.Click += btnSaveAll_Click;
            bmpFile = new Bitmap(GetType().Assembly.GetManifestResourceStream("GDIBuilder2.file.png"));
            bmpFolder = new Bitmap(GetType().Assembly.GetManifestResourceStream("GDIBuilder2.folder.png"));
            Title = "Browse and Extract GD-ROM";
            MinimumSize = new Size(540, 420);
            Padding = new Padding(4, 3, 4, 3);
            btnPickGdi.Click += btnPickGdi_Click;
            DynamicLayout gdiRow = new DynamicLayout();
            gdiRow.BeginHorizontal();
            gdiRow.Add(new Label { Text = "Disc Image:", TextAlignment = TextAlignment.Right });
            gdiRow.Add(txtGdiPath, true);
            gdiRow.Add(btnPickGdi);
            gdiRow.EndHorizontal();
            gdiRow.Spacing = new Size(5, 5);

            tgvDisc.RowHeight = 24;
            tgvDisc.AllowMultipleSelection = true;
            tgvDisc.Columns.Add(new GridColumn()
                {
                    HeaderText = "Filename", 
                    DataCell = new TextBoxCell(1)
                    {
                        Binding = Binding.Property((AdaptedPath p) => p.Name)
                    },
                    DisplayIndex = 1,
                    Expand = true
                });
            tgvDisc.Columns.Add(new GridColumn()
            {
                HeaderText = "",
                MinWidth = 32,
                DataCell = new ImageViewCell(0)
                {
                    Binding = Binding.Property((AdaptedPath p) => p.IsFolder ? bmpFolder as Image : bmpFile),
                },
                DisplayIndex = 0
            });
            tgvDisc.Columns.Add(new GridColumn()
            {
                HeaderText = "Size",
                MinWidth = 75,
                DataCell = new TextBoxCell(2)
                {
                    Binding = Binding.Property((AdaptedPath p) => p.SizeName)
                },
                DisplayIndex = 2
            });
            tgvDisc.Columns.Add(new GridColumn()
            {
                HeaderText = "Modified",
                MinWidth = 140,
                DataCell = new TextBoxCell(3)
                {
                    Binding = Binding.Property((AdaptedPath p) => p.Modified.ToString())
                },
                DisplayIndex = 3
            });
            
            DynamicLayout bottomRow = new DynamicLayout();
            bottomRow.BeginHorizontal();
            bottomRow.Add(null, true, false); //Add expanding blank space to right-justify the row 
            bottomRow.Add(btnSaveIpBin);
            bottomRow.Add(btnSaveSelected);
            bottomRow.Add(btnSaveAll);
            bottomRow.EndHorizontal();
            bottomRow.Spacing = new Size(5, 5);
            
            DynamicLayout completeLayout = new DynamicLayout() { Padding = 6 };
            completeLayout.Add(gdiRow, true, false);
            completeLayout.Add(tgvDisc, true, true);
            completeLayout.Add(bottomRow, true, false);
            completeLayout.Spacing = new Size(0, 4); 
            Content = completeLayout;
        }
        #endregion
        
        private void btnPickGdi_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filters.Add(new FileFilter("GD-ROM Image (*.gdi, *.cue)", new[] { "*.gdi", "*.cue" }));
                if (ofd.ShowDialog(this) == DialogResult.Ok)
                {
                    txtGdiPath.Text = ofd.FileName;
                    LoadDisc(ofd.FileName);
                }
            }
        }

        private void btnSaveIpBin_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filters.Add(new FileFilter("Binary Files (*.bin)", new[] { "*.bin" }));
                sfd.FileName = "IP.BIN";
                if (sfd.ShowDialog(this) == DialogResult.Ok)
                {
                    using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write))
                    {
                        using (Stream ip = _currentDisc.ReadIPBin())
                        {
                            ip.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }
        }

        private void btnSaveSelected_Click(object sender, EventArgs e)
        {
            if (tgvDisc.SelectedRow < 0)
            {
                return; //No selection
            }
            using (SelectFolderDialog fbd = new SelectFolderDialog())
            {
                fbd.Title = "Select destination folder";
                if (fbd.ShowDialog(this) == DialogResult.Ok)
                {
                    ExtractSelectionTo(fbd.Directory);
                }
            }
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            string destination = null;
            using (SelectFolderDialog fbd = new SelectFolderDialog())
            {
                fbd.Title = "Select destination folder";
                if (fbd.ShowDialog(this) == DialogResult.Ok)
                {
                    destination = fbd.Directory;
                }
            }

            if (destination != null)
            {
                List<string> pathsToExtract = new List<string> { "" };
                using (ExtractProgressDialog epd = new ExtractProgressDialog(_currentDisc, pathsToExtract, destination))
                {
                    epd.ShowModal(this);
                }
            }
        }

        private void LoadDisc(string path)
        {
            _currentDisc?.Dispose();
            _currentDisc = Path.GetExtension(path) == ".cue" ? GDReader.FromCueSheet(path) : GDReader.FromGDIfile(path);
            TreeGridItemCollection discRoot = new TreeGridItemCollection();
            foreach (var dirPath in _currentDisc.GetDirectories(""))
            {
                AdaptedPath folder = new AdaptedPath
                {
                    FullPath = dirPath,
                    Name = Path.GetFileName(dirPath),
                    IsFolder = true,
                    Modified = _currentDisc.GetCreationTimeUtc(dirPath).ToLocalTime()
                };
                AddFolder(dirPath, folder);
                discRoot.Add(folder);
            }

            var files = _currentDisc.GetFiles("");
            var uniqueFiles = new HashSet<string>(files);
            foreach (var filePath in uniqueFiles)
            {
                AdaptedPath file = new AdaptedPath
                {
                    FullPath = filePath,
                    Name = Path.GetFileName(filePath),
                    IsFolder = false,
                    Modified = _currentDisc.GetCreationTimeUtc(filePath).ToLocalTime(),
                    Size = _currentDisc.GetFileLength(filePath)
                };
                discRoot.Add(file);
            }
            tgvDisc.DataStore = discRoot;
            btnSaveIpBin.Enabled = true;
            btnSaveSelected.Enabled = true;
            btnSaveAll.Enabled = true;
        }

        private void AddFolder(string folderPath, AdaptedPath parent)
        {
            foreach (var dirPath in _currentDisc.GetDirectories(folderPath))
            {
                AdaptedPath folder = new AdaptedPath
                {
                    FullPath = dirPath,
                    Name = Path.GetFileName(dirPath),
                    IsFolder = true,
                    Modified = _currentDisc.GetCreationTime(dirPath),
                    Parent = parent
                };
                AddFolder(dirPath, folder);
                parent.Children.Add(folder);
            }
            var files = _currentDisc.GetFiles(folderPath);
            var uniqueFiles = new HashSet<string>(files);
            foreach (var filePath in uniqueFiles)
            {
                AdaptedPath file = new AdaptedPath
                {
                    FullPath = filePath,
                    Name = Path.GetFileName(filePath),
                    IsFolder = false,
                    Modified = _currentDisc.GetCreationTime(filePath),
                    Size = _currentDisc.GetFileLength(filePath),
                    Parent = parent
                };
                parent.Children.Add(file);
            }
        }

        private void ExtractSelectionTo(string destinationFolder)
        {
            HashSet<string> folders = new HashSet<string>();
            HashSet<string> files = new HashSet<string>();
            foreach (var row in tgvDisc.SelectedItems)
            {
                if (row is AdaptedPath gridItem)
                {
                    if (gridItem.IsFolder)
                    {
                        folders.Add(gridItem.FullPath);
                    }
                    else
                    {
                        files.Add(gridItem.FullPath);
                    }
                }
            }
            //First, omit files where the folder containing that file was also selected.
            foreach (string dropMe in GetDuplicates(folders, files))
            {
                files.Remove(dropMe);
            }
            //Then omit folders where the parent folder is also selected
            foreach (string dropMe in GetDuplicates(folders, folders))
            {
                folders.Remove(dropMe);
            }
            List<string> pathsToExtract = new List<string>();
            pathsToExtract.AddRange(folders);
            pathsToExtract.AddRange(files);
            using (ExtractProgressDialog epd = new ExtractProgressDialog(_currentDisc, pathsToExtract, destinationFolder))
            {
                epd.ShowModal(this);
            }
        }

        private List<string> GetDuplicates(HashSet<string> folders, HashSet<string> search)
        {
            List<string> toDrop = new List<string>();
            foreach (string file in search)
            {
                if (file.LastIndexOf('\\') > 0)
                {
                    string parent = file.Substring(0, file.LastIndexOf('\\'));
                    do
                    {
                        if (folders.Contains(parent))
                        {
                            toDrop.Add(file);
                            break;
                        }

                        if (parent.LastIndexOf('\\') <= 0) break;
                        parent = parent.Substring(0, parent.LastIndexOf('\\'));
                    } while (parent.Length > 1);
                }
            }
            return toDrop;
        }
    }

    public class AdaptedPath : TreeGridItem
    {
        public bool IsFolder { get; set; }
        public override bool Expandable => IsFolder && Children.Count > 0;
        public string FullPath { get; set; }
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                int finalSlash = value?.LastIndexOf("\\") ?? -1;
                _name = finalSlash >= 0 ? value?.Substring(finalSlash + 1) : value;
            }
        }

        public DateTime Modified { get; set; }
        public long Size { get; set; }

        public string SizeName
        {
            get
            {
                if (IsFolder) return "";
                if (Size > 1024 * 1024) return $"{Size / 1024.0 / 1024.0:#.##} MB";
                if (Size > 1024) return $"{Size / 1024.0:#.##} KB";
                return $"{Size} B";
            }
        }
    }
}