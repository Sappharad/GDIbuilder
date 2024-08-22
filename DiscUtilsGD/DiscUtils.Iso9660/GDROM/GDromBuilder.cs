using System;
using System.IO;
using System.Text;
using System.Threading;
using DiscUtils.Streams;
using DiscUtils.Iso9660;
using File = System.IO.File;
using System.Collections.Generic;

namespace DiscUtils.Gdrom
{
    public class GDromBuilder
    {
        #region Constants
        const int DATA_SECTOR_SIZE = 2048;
        const int RAW_SECTOR_SIZE = 2352;
        const int GD_START_LBA = 45000;
        const int GD_END_LBA = 549150;
        #endregion
        #region Properties
        public string VolumeIdentifier
        {
            get => _builder.VolumeIdentifier;
            set => _builder.VolumeIdentifier = value;
        }
        public string SystemIdentifier
        {
            get => _builder.SystemIdentifier; 
            set => _builder.SystemIdentifier = value;
        }
        public string VolumeSetIdentifier
        {
            get => _builder.VolumeSetIdentifier; 
            set => _builder.VolumeSetIdentifier = value;
        }

        public string PublisherIdentifier
        {
            get => _builder.PublisherIdentifier; 
            set => _builder.PublisherIdentifier = value;
        }

        public string DataPreparerIdentifier
        {
            get => _builder.DataPreparerIdentifier; 
            set => _builder.DataPreparerIdentifier = value;
        }

        public string ApplicationIdentifier
        {
            get => _builder.ApplicationIdentifier; 
            set => _builder.ApplicationIdentifier = value;
        }
        private int _lastProgress;
        public delegate void OnReportProgress(int percent);
        public OnReportProgress ReportProgress { get; set; }
        public string Track03Path { get; set; }
        public string LastTrackPath { get; set; }
        public bool RawMode { get; set; }
        public bool TruncateData { get; set; }
        #endregion
        #region Private Fields
        private byte[] _ipbinData = new byte[0x8000];
        private readonly string _bootBin;
        private BuildFileInfo _bootBinFile = null;
        private List<string> _cddaPaths;
        private readonly CDBuilder _builder = new CDBuilder();
        #endregion

        private GDromBuilder()
        {
            VolumeIdentifier = "DREAMCAST";
            SystemIdentifier = string.Empty;
            VolumeSetIdentifier = string.Empty;
            PublisherIdentifier = string.Empty;
            DataPreparerIdentifier = string.Empty;
            ApplicationIdentifier = string.Empty;
        }

        public GDromBuilder(string ipBinPath, List<string> cddaPaths) : this()
        {
            using (FileStream ipfs = new FileStream(ipBinPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                LoadIPbin(ipfs);
            }
            _bootBin = GetBootBin();
            _cddaPaths = cddaPaths;
        }
        public GDromBuilder(Stream ipBinStream, List<string> cddaPaths) : this()
        {
            LoadIPbin(ipBinStream);
            _bootBin = GetBootBin();
            _cddaPaths = cddaPaths;
        }
        public GDromBuilder(byte[] ipBin, List<string> cddaPaths) : this()
        {
            _ipbinData = ipBin;
            _bootBin = GetBootBin();
            _cddaPaths = cddaPaths;
        }


        public List<DiscTrack> BuildGDROM(CancellationToken? token = null)
        {
            _builder.UseJoliet = false; //A stupid default, mkisofs won't do this by default.
            _builder.LBAoffset = GD_START_LBA;
            _builder.EndSector = GD_END_LBA;

            List<DiscTrack> retval = new List<DiscTrack>();
            if (_cddaPaths != null && _cddaPaths.Count > 0)
            {
                retval = ReadCDDA(_cddaPaths);
            }
            if (_bootBinFile != null)
            {
                _builder.MoveToEnd(_bootBinFile);
                long sectorSize = RoundUp(_bootBinFile.GetDataSize(Encoding.ASCII), DATA_SECTOR_SIZE);
                _builder.LastFileStartSector = (uint)(GD_END_LBA - 150 - (sectorSize / DATA_SECTOR_SIZE));
            }
            else
            {
                throw new InvalidOperationException($"The binary that the supplied IP.BIN expects to startup, {_bootBin}, was not added to the disc!");
            }
            if (token?.IsCancellationRequested == true) return null;
            
            using (BuiltStream isoStream = (BuiltStream)_builder.Build())
            {
                _lastProgress = 0;
                if (retval.Count > 0 || (TruncateData && !string.IsNullOrEmpty(LastTrackPath)))
                {
                    if (RawMode)
                    {
                        ExportMultiTrackRaw(isoStream, retval, token);
                    }
                    else
                    {
                        ExportMultiTrack(isoStream, retval, token);
                    }
                }
                else
                {
                    if (RawMode)
                    {
                        ExportSingleTrackRaw(isoStream, retval, token);
                    }
                    else
                    {
                        ExportSingleTrack(isoStream, retval, token);
                    }
                }
            }
            return retval;
        }

        public List<DiscTrack> BuildGDROM(string outDir, CancellationToken? token = null)
        {
            Track03Path = Path.Combine(outDir, "track03.bin");
            LastTrackPath = Path.Combine(outDir, GetLastTrackName(_cddaPaths?.Count ?? 0));
            return BuildGDROM(token);
        }

        private void ExportSingleTrack(BuiltStream isoStream, List<DiscTrack> tracks, CancellationToken? token)
        {
            long currentBytes = 0;
            long totalBytes = isoStream.Length;
            int skip = 0;

            DiscTrack track3 = new DiscTrack();
            track3.FileName = Path.GetFileName(Track03Path);
            track3.LBA = GD_START_LBA;
            track3.Type = 4;
            track3.FileSize = (GD_END_LBA - GD_START_LBA) * DATA_SECTOR_SIZE;
            tracks.Add(track3);
            UpdateIPBIN(_ipbinData, tracks);
            using (FileStream destStream = new FileStream(Track03Path, FileMode.Create, FileAccess.Write))
            {
                destStream.Write(_ipbinData, 0, _ipbinData.Length);
                isoStream.Seek(_ipbinData.Length, SeekOrigin.Begin);
                currentBytes += _ipbinData.Length;

                byte[] buffer = new byte[64 * 1024];
                int numRead = isoStream.Read(buffer, 0, buffer.Length);
                while (numRead != 0)
                {
                    destStream.Write(buffer, 0, numRead);
                    numRead = isoStream.Read(buffer, 0, buffer.Length);
                    currentBytes += numRead;
                    skip++;
                    if (skip >= 10)
                    {
                        if (token?.IsCancellationRequested == true) return;
                        skip = 0;
                        int percent = (int)((currentBytes * 100) / totalBytes);
                        if (percent > _lastProgress)
                        {
                            _lastProgress = percent;
                            ReportProgress?.Invoke(_lastProgress);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Separate raw logic to maintain performance of the 2048 version
        /// </summary>
        private void ExportSingleTrackRaw(BuiltStream isoStream, List<DiscTrack> tracks, CancellationToken? token)
        {
            long currentBytes = 0;
            long totalBytes = isoStream.Length;
            int skip = 0;

            DiscTrack track3 = new DiscTrack();
            track3.FileName = Path.GetFileName(Track03Path);
            track3.LBA = GD_START_LBA;
            track3.Type = 4;
            track3.FileSize = (GD_END_LBA - GD_START_LBA) * DATA_SECTOR_SIZE;
            tracks.Add(track3);
            UpdateIPBIN(_ipbinData, tracks);
            using (FileStream destStream = new FileStream(Track03Path, FileMode.Create, FileAccess.Write))
            {
                int currentLBA = GD_START_LBA;
                byte[] buffer = new byte[DATA_SECTOR_SIZE];
                byte[] resultSector;
                for (int i = 0; i < _ipbinData.Length; i += buffer.Length)
                {
                    Array.Copy(_ipbinData, i, buffer, 0, buffer.Length);
                    resultSector = IsoUtilities.ConvertSectorToRawMode1(buffer, currentLBA++);
                    destStream.Write(resultSector, 0, resultSector.Length);
                    currentBytes += 2048;
                }
                isoStream.Seek(_ipbinData.Length, SeekOrigin.Begin);

                int numRead = isoStream.Read(buffer, 0, buffer.Length);
                while (numRead != 0)
                {
                    while (numRead != 0 && numRead < buffer.Length)
                    {
                        //We need all 2048 bytes for a complete sector!
                        int localRead = isoStream.Read(buffer, numRead, buffer.Length - numRead);
                        numRead += localRead;
                        if (localRead == 0)
                        {
                            for (int i = numRead; i < buffer.Length; i++)
                            {
                                buffer[i] = 0;
                            }
                            break; //Prevent infinite loop
                        }
                    }
                    resultSector = IsoUtilities.ConvertSectorToRawMode1(buffer, currentLBA++);
                    destStream.Write(resultSector, 0, resultSector.Length);
                    numRead = isoStream.Read(buffer, 0, buffer.Length);
                    currentBytes += numRead;
                    skip++;
                    if (skip >= 10)
                    {
                        if (token?.IsCancellationRequested == true) return;
                        skip = 0;
                        int percent = (int)((currentBytes * 100) / totalBytes);
                        if (percent > _lastProgress)
                        {
                            _lastProgress = percent;
                            ReportProgress?.Invoke(_lastProgress);
                        }
                    }
                }
            }
        }
        
        private void ExportMultiTrack(BuiltStream isoStream, List<DiscTrack> tracks, CancellationToken? token)
        {
            //There is a 150 sector gap before and after the CDDA
            long lastHeaderEnd = 0;
            long firstFileStart = 0;
            foreach (BuilderExtent extent in isoStream.BuilderExtents)
            {
                if (extent is FileExtent)
                {
                    firstFileStart = extent.Start;
                    break;
                }
                else
                {
                    lastHeaderEnd = extent.Start + RoundUp(extent.Length, DATA_SECTOR_SIZE);
                }
            }
            lastHeaderEnd = lastHeaderEnd / DATA_SECTOR_SIZE;
            firstFileStart = firstFileStart / DATA_SECTOR_SIZE;
            int trackEnd = (int)(firstFileStart - 150);
            for (int i = tracks.Count - 1; i >= 0; i--)
            {
                trackEnd = trackEnd - (int)(RoundUp(tracks[i].FileSize, RAW_SECTOR_SIZE) / RAW_SECTOR_SIZE);
                //Track end is now the beginning of this track and the end of the previous
                tracks[i].LBA = (uint)(trackEnd + GD_START_LBA);
            }
            trackEnd = trackEnd - 150;
            if (trackEnd < lastHeaderEnd)
            {
                throw new Exception("Not enough room to fit all of the CDDA after we added the data.");
            }
            if (TruncateData)
            {
                trackEnd = (int)lastHeaderEnd;
            }
            DiscTrack track3 = new DiscTrack();
            track3.FileName = Path.GetFileName(Track03Path);
            track3.LBA = GD_START_LBA;
            track3.Type = 4;
            track3.FileSize = trackEnd * DATA_SECTOR_SIZE;
            tracks.Insert(0, track3);
            DiscTrack lastTrack = new DiscTrack();
            lastTrack.FileName = GetLastTrackName(tracks.Count - 1);
            lastTrack.FileSize = (GD_END_LBA - GD_START_LBA - firstFileStart) * DATA_SECTOR_SIZE;
            lastTrack.LBA = (uint)(GD_START_LBA + firstFileStart);
            lastTrack.Type = 4;
            tracks.Add(lastTrack);
            UpdateIPBIN(_ipbinData, tracks);

            long currentBytes = 0;
            long totalBytes = isoStream.Length;
            int skip = 0;

            using (FileStream destStream = new FileStream(Track03Path, FileMode.Create, FileAccess.Write))
            {
                destStream.Write(_ipbinData, 0, _ipbinData.Length);
                isoStream.Seek(_ipbinData.Length, SeekOrigin.Begin);
                long bytesWritten = (long)_ipbinData.Length;

                byte[] buffer = new byte[DATA_SECTOR_SIZE];
                int numRead = isoStream.Read(buffer, 0, buffer.Length);
                while (numRead != 0 && bytesWritten < track3.FileSize)
                {
                    destStream.Write(buffer, 0, numRead);
                    numRead = isoStream.Read(buffer, 0, buffer.Length);
                    bytesWritten += numRead;

                    currentBytes += numRead;
                    skip++;
                    if (skip >= 50)
                    {
                        if (token?.IsCancellationRequested == true) return;
                        skip = 0;
                        int percent = (int)((currentBytes * 100) / totalBytes);
                        if (percent > _lastProgress)
                        {
                            _lastProgress = percent;
                            ReportProgress?.Invoke(_lastProgress);
                        }
                    }
                }
            }
            using (FileStream destStream = new FileStream(LastTrackPath, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[64 * 1024];
                currentBytes = firstFileStart * DATA_SECTOR_SIZE;
                isoStream.Seek(currentBytes, SeekOrigin.Begin);
                int numRead = isoStream.Read(buffer, 0, buffer.Length);
                while (numRead != 0)
                {
                    destStream.Write(buffer, 0, numRead);
                    numRead = isoStream.Read(buffer, 0, buffer.Length);

                    currentBytes += numRead;
                    skip++;
                    if (skip >= 10)
                    {
                        if (token?.IsCancellationRequested == true) return;
                        skip = 0;
                        int percent = (int)((currentBytes * 100) / totalBytes);
                        if (percent > _lastProgress)
                        {
                            _lastProgress = percent;
                            ReportProgress?.Invoke(_lastProgress);
                        }
                    }
                }
            }
        }

        private void ExportMultiTrackRaw(BuiltStream isoStream, List<DiscTrack> tracks, CancellationToken? token)
        {
            //There is a 150 sector gap before and after the CDDA
            long lastHeaderEnd = 0;
            long firstFileStart = 0;
            foreach (BuilderExtent extent in isoStream.BuilderExtents)
            {
                if (extent is FileExtent)
                {
                    firstFileStart = extent.Start;
                    break;
                }
                else
                {
                    lastHeaderEnd = extent.Start + RoundUp(extent.Length, DATA_SECTOR_SIZE);
                }
            }
            lastHeaderEnd = lastHeaderEnd / DATA_SECTOR_SIZE;
            firstFileStart = firstFileStart / DATA_SECTOR_SIZE;
            int trackEnd = (int)(firstFileStart - 150);
            for (int i = tracks.Count - 1; i >= 0; i--)
            {
                trackEnd = trackEnd - (int)(RoundUp(tracks[i].FileSize, RAW_SECTOR_SIZE) / RAW_SECTOR_SIZE);
                //Track end is now the beginning of this track and the end of the previous
                tracks[i].LBA = (uint)(trackEnd + GD_START_LBA);
            }
            trackEnd = trackEnd - 150;
            if (trackEnd < lastHeaderEnd)
            {
                throw new Exception("Not enough room to fit all of the CDDA after we added the data.");
            }
            if (TruncateData)
            {
                trackEnd = (int)lastHeaderEnd;
            }
            DiscTrack track3 = new DiscTrack();
            track3.FileName = Path.GetFileName(Track03Path);
            track3.LBA = GD_START_LBA;
            track3.Type = 4;
            track3.FileSize = trackEnd * DATA_SECTOR_SIZE;
            tracks.Insert(0, track3);
            DiscTrack lastTrack = new DiscTrack();
            lastTrack.FileName = GetLastTrackName(tracks.Count - 1);
            lastTrack.FileSize = (GD_END_LBA - GD_START_LBA - firstFileStart) * DATA_SECTOR_SIZE;
            lastTrack.LBA = (uint)(GD_START_LBA + firstFileStart);
            lastTrack.Type = 4;
            tracks.Add(lastTrack);
            UpdateIPBIN(_ipbinData, tracks);

            long currentBytes = 0;
            long totalBytes = isoStream.Length;
            int skip = 0;
            int currentLBA = GD_START_LBA;

            using (FileStream destStream = new FileStream(Track03Path, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[DATA_SECTOR_SIZE];
                byte[] resultSector;
                for (int i = 0; i < _ipbinData.Length; i += buffer.Length)
                {
                    Array.Copy(_ipbinData, i, buffer, 0, buffer.Length);
                    resultSector = IsoUtilities.ConvertSectorToRawMode1(buffer, currentLBA++);
                    destStream.Write(resultSector, 0, resultSector.Length);
                    currentBytes += DATA_SECTOR_SIZE;
                }
                isoStream.Seek(_ipbinData.Length, SeekOrigin.Begin);
                long bytesWritten = (long)_ipbinData.Length;

                int numRead = isoStream.Read(buffer, 0, buffer.Length);
                while (numRead != 0 && bytesWritten < track3.FileSize)
                {
                    while (numRead != 0 && numRead < buffer.Length)
                    {
                        //We need all 2048 bytes for a complete sector!
                        int localRead = isoStream.Read(buffer, numRead, buffer.Length - numRead);
                        numRead += localRead;
                        if (localRead == 0)
                        {
                            for (int i = numRead; i < buffer.Length; i++)
                            {
                                buffer[i] = 0;
                            }
                            break; //Prevent infinite loop
                        }
                    }
                    resultSector = IsoUtilities.ConvertSectorToRawMode1(buffer, currentLBA++);
                    destStream.Write(resultSector, 0, resultSector.Length);
                    numRead = isoStream.Read(buffer, 0, buffer.Length);
                    bytesWritten += numRead;
                    currentBytes += numRead;
                    skip++;
                    if (skip >= 50)
                    {
                        if (token?.IsCancellationRequested == true) return;
                        skip = 0;
                        int percent = (int)((currentBytes * 100) / totalBytes);
                        if (percent > _lastProgress)
                        {
                            _lastProgress = percent;
                            ReportProgress?.Invoke(_lastProgress);
                        }
                    }
                }
            }
            currentLBA = (int)lastTrack.LBA;
            using (FileStream destStream = new FileStream(LastTrackPath, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[DATA_SECTOR_SIZE];
                byte[] resultSector;
                currentBytes = firstFileStart * DATA_SECTOR_SIZE;
                isoStream.Seek(currentBytes, SeekOrigin.Begin);
                int numRead = isoStream.Read(buffer, 0, buffer.Length);
                while (numRead != 0)
                {
                    while (numRead != 0 && numRead < buffer.Length)
                    {
                        //We need all 2048 bytes for a complete sector!
                        int localRead = isoStream.Read(buffer, numRead, buffer.Length - numRead);
                        numRead += localRead;
                        if (localRead == 0)
                        {
                            for (int i = numRead; i < buffer.Length; i++)
                            {
                                buffer[i] = 0;
                            }
                            break; //Prevent infinite loop
                        }
                    }
                    resultSector = IsoUtilities.ConvertSectorToRawMode1(buffer, currentLBA++);
                    destStream.Write(resultSector, 0, resultSector.Length);
                    numRead = isoStream.Read(buffer, 0, buffer.Length);

                    currentBytes += numRead;
                    skip++;
                    if (skip >= 10)
                    {
                        if (token?.IsCancellationRequested == true) return;
                        skip = 0;
                        int percent = (int)((currentBytes * 100) / totalBytes);
                        if (percent > _lastProgress)
                        {
                            _lastProgress = percent;
                            ReportProgress?.Invoke(_lastProgress);
                        }
                    }
                }
            }
        }
        
        private void LoadIPbin(Stream ipfs)
        {
            if (ipfs.Length != _ipbinData.Length)
            {
                throw new Exception("IP.BIN is the wrong size. Possibly the wrong file? Cannot continue.");
            }
            ipfs.Seek(0, SeekOrigin.Begin);
            if (ipfs.Read(_ipbinData, 0, _ipbinData.Length) < _ipbinData.Length)
            {
                throw new Exception("Unable to read the entire IP.BIN file. Cannot continue.");
            }
        }

        private void UpdateIPBIN(byte[] ipbinData, List<DiscTrack> tracks)
        {
            //Tracks 03 to 99, 1 and 2 were in the low density area
            for (int t = 0; t < 97; t++)
            {
                uint dcLBA = 0xFFFFFF;
                byte dcType = 0xFF;
                if (t < tracks.Count)
                {
                    DiscTrack track = tracks[t];
                    dcLBA = track.LBA + 150;
                    dcType = (byte)((track.Type << 4) | 0x1);
                }
                int offset = 0x104 + (t * 4);
                ipbinData[offset++] = (byte)(dcLBA & 0xFF);
                ipbinData[offset++] = (byte)((dcLBA >> 8) & 0xFF);
                ipbinData[offset++] = (byte)((dcLBA >> 16) & 0xFF);
                ipbinData[offset] = dcType;
            }
        }

        public static string CheckOutputExists(List<string> cdda, string output)
        {
            List<string> filesToCheck = new List<string>();
            filesToCheck.Add("track03.bin");
            if (cdda != null && cdda.Count > 0)
            {
                filesToCheck.Add(GetLastTrackName(cdda.Count));
            }
            StringBuilder sb = new StringBuilder();
            int fc = 0;
            foreach (string file in filesToCheck)
            {
                if (File.Exists(Path.Combine(output, file)))
                {
                    sb.Append(file + ", ");
                    fc++;
                }
            }
            if (fc >= 2)
            {
                return "The files " + sb.ToString(0, sb.Length - 2) + " already exist. They will be overwritten. Are you sure?";
            }
            else if (fc == 1)
            {
                return "The file " + sb.ToString(0, sb.Length - 2) + " already exists. It will be overwritten. Are you sure?";
            }
            return null;
        }

        public string GetGDIText(List<DiscTrack> tracks)
        {
            StringBuilder sb = new StringBuilder();
            int tn = 3;
            foreach (DiscTrack track in tracks)
            {
                sb.Append(tn + " " + track.LBA + " " + track.Type + " ");
                if (track.Type == 0 || RawMode)
                {
                    sb.Append("2352 ");
                }
                else
                {
                    sb.Append("2048 ");
                }
                sb.AppendLine(track.FileName + " 0");
                tn++;
            }
            return sb.ToString();
        }

        public void UpdateGdiFile(List<DiscTrack> tracks, string gdiPath)
        {
            StringBuilder sb = new StringBuilder();
            if (File.Exists(gdiPath))
            {
                string[] file = File.ReadAllLines(gdiPath);
                int i = 0;
                sb.AppendLine((tracks.Count + 2).ToString());
                if (file.Length > 0 && file[0].Length <= 3)
                {
                    i++;
                }
                for (; i < file.Length; i++)
                {
                    if (file[i].StartsWith("3"))
                    {
                        break;
                    }
                    else
                    {
                        sb.AppendLine(file[i]);
                    }
                }
            }
            sb.Append(GetGDIText(tracks));
            File.WriteAllText(gdiPath, sb.ToString());
        }

        private static string GetLastTrackName(int cddaTracks)
        {
            return "track" + (cddaTracks + 4).ToString("00") + ".bin";
        }

        private List<DiscTrack> ReadCDDA(List<string> paths)
        {
            List<DiscTrack> retval = new List<DiscTrack>();
            foreach (string path in paths)
            {
                FileInfo fi = new FileInfo(path);
                if (!fi.Exists)
                {
                    throw new FileNotFoundException("CDDA track " + fi.Name + " could not be accessed.");
                }
                DiscTrack track = new DiscTrack();
                track.FileName = fi.Name;
                track.Type = 0;
                track.FileSize = fi.Length;
                retval.Add(track);
            }
            return retval;
        }

        private string GetBootBin()
        {            
            byte[] name = new byte[16];
            Array.Copy(_ipbinData, 0x60, name, 0, name.Length);
            string actualFilename = Encoding.ASCII.GetString(name).Trim();
            return BuildFileInfo.MakeShortFileName(actualFilename, null);
        }

        public void ImportFolder(string path, string intoDiscPath = "", bool allowOverwrite = false, CancellationToken? token = null)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if (intoDiscPath == Path.DirectorySeparatorChar.ToString())
            {
                intoDiscPath = string.Empty;
            }
            PopulateFromFolder(di, di.FullName, intoDiscPath, allowOverwrite, token);
        }

        public BuildDirectoryInfo CreateDirectory(string atDiscPath)
        {
            return _builder.AddDirectory(atDiscPath);
        }

        public BuildFileInfo ImportFile(string path, string atDiscPath = "", bool allowOverwrite = false)
        {
            FileInfo fi = new FileInfo(path);
            if (atDiscPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                atDiscPath += fi.Name;
            }
            BuildFileInfo fileInfo = _builder.AddFile(atDiscPath, fi.FullName, allowOverwrite);
            if (IsBootBin(fileInfo))
            {
                _bootBinFile = fileInfo;
            }
            return fileInfo;
        }

        public BuildFileInfo ImportFile(Stream fileData, string atDiscPath, bool allowOverwrite = false)
        {
            BuildFileInfo fileInfo = _builder.AddFile(atDiscPath, fileData, allowOverwrite);
            if (IsBootBin(fileInfo))
            {
                _bootBinFile = fileInfo;
            }
            return fileInfo;
        }

        private void PopulateFromFolder(DirectoryInfo di, string basePath, string intoDiscPath, bool allowOverwrite, CancellationToken? token = null)
        {
            FileInfo[] folderFiles = di.GetFiles();
            //Add directory first, so we can set the creation time correctly.
            string localDirPath = intoDiscPath + di.FullName.Substring(basePath.Length);
            if (localDirPath.Length > 1)
            {
                //Add directory first, so it has the correct creation time.
                BuildDirectoryInfo dir = _builder.AddDirectory(localDirPath);
                if (di.CreationTimeUtc < dir.CreationTime)
                {
                    //Oldest creation date wins. Applicable if this is a newly added directory, or if we are combining two copies of the same folder.
                    dir.CreationTime = di.CreationTimeUtc;
                }
            }
            foreach (FileInfo file in folderFiles)
            {
                string filePath = intoDiscPath + file.FullName.Substring(basePath.Length);
                var fileEntry = _builder.AddFile(filePath, file.FullName, allowOverwrite);
                if (IsBootBin(fileEntry))
                {
                    _bootBinFile = fileEntry;
                }
            }
            if (token?.IsCancellationRequested == true)
            {
                return;
            }

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                PopulateFromFolder(dir, basePath, intoDiscPath, allowOverwrite, token);
            }
        }
        
        private long RoundUp(long value, long unit)
        {
            return ((value + (unit - 1)) / unit) * unit;
        }

        private bool IsBootBin(BuildFileInfo bfi)
        {
            return bfi.Parent.HierarchyDepth == 0 && bfi.Name.Equals(_bootBin, StringComparison.OrdinalIgnoreCase);
        }
    }

    public class DiscTrack
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public uint LBA { get; set; }
        public byte Type { get; set; } //4 is Data, 0 is audio
    }
}
