using System;
using System.IO;
using DiscUtils.Gdrom;
using System.Collections.Generic;

namespace buildgdi
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                PrintUsage();
                return;
            }
            string data = GetSoloArgument("-data", args);
            string ipBin = GetSoloArgument("-ip", args);
            List<string> outPath = GetMultiArgument("-output", args);
            List<string> cdda = GetMultiArgument("-cdda", args);
            string gdiPath = GetSoloArgument("-gdi", args);
            string volume = GetSoloArgument("-V", args);
            bool rebuild = HasArgument("-rebuild", args);
            bool extract = HasArgument("-extract", args);
            bool truncate = HasArgument("-truncate", args);
            bool useIsoSectors = HasArgument("-iso", args);
            bool fileOutput = false;
            if (CheckArguments(extract, rebuild, data, ipBin, gdiPath, outPath, cdda, truncate, out fileOutput) == false)
            {
                return;
            }
            if (extract)
            {
                Console.WriteLine("Starting extraction");
                Extract(gdiPath, ipBin, outPath);
                Console.WriteLine("Done!");
            }
            else if (rebuild)
            {
                Rebuild(gdiPath, data, ipBin, cdda, outPath, volume, useIsoSectors, truncate);
            }
            else
            {
                BuildDisc(ipBin, cdda, useIsoSectors, truncate, data, volume, fileOutput, outPath, gdiPath);
            }
        }

        private static void Extract(string gdiPath, string ipBin, List<string> outPath)
        {
            using (GDReader reader = GDReader.FromGDIfile(gdiPath))
            {
                if (!string.IsNullOrEmpty(ipBin))
                {
                    if (string.IsNullOrEmpty(Path.GetExtension(ipBin)))
                    {
                        ipBin = Path.Combine(ipBin, "IP.BIN");
                    }
                    if (!Directory.Exists(Path.GetDirectoryName(ipBin)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(ipBin));
                    }
                    Console.WriteLine("Extracting IP.BIN");
                    using (Stream input = reader.ReadIPBin())
                    {
                        using (FileStream output = new FileStream(ipBin, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                        {
                            TransferStreams(input, output);
                        }
                    }
                }
                if (!Directory.Exists(outPath[0]))
                {
                    Directory.CreateDirectory(outPath[0]);
                }
                ExtractFolder(reader, "", outPath[0]);
            }
        }

        private static void ExtractFolder(GDReader reader, string folder, string toPath)
        {
            string[] files = reader.GetFiles(folder);
            foreach (var file in files)
            {
                string filePath = file;
                if (filePath.StartsWith("\\"))
                {
                    filePath = filePath.Substring(1); //I don't want the leading slash, it breaks things;
                }
                string destPath = Path.Combine(toPath, filePath);
                Console.WriteLine($"Extracting {filePath}");
                using (Stream input = reader.OpenFile(file, FileMode.Open, FileAccess.Read))
                {
                    using (FileStream output = new FileStream(destPath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        TransferStreams(input, output);
                    }
                }
                File.SetCreationTimeUtc(destPath, reader.GetCreationTimeUtc(file));
                File.SetLastWriteTimeUtc(destPath, reader.GetLastWriteTimeUtc(file));
            }
            string[] folders = reader.GetDirectories(folder);
            foreach (string subfolder in folders)
            {
                string folderPath = subfolder;
                if (folderPath.StartsWith("\\"))
                {
                    folderPath = folderPath.Substring(1);
                }
                string destPath = Path.Combine(toPath, folderPath);
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                    Directory.SetCreationTimeUtc(destPath, reader.GetCreationTimeUtc(subfolder));
                    Directory.SetLastWriteTimeUtc(destPath, reader.GetLastWriteTimeUtc(subfolder));
                }
                ExtractFolder(reader, folderPath, toPath);
            }
        }

        private static void TransferStreams(Stream input, Stream output)
        {
            byte[] buffer = new byte[8192];
            int bytesRead = 0;
            long bytesLeft = input.Length;
            do
            {
                bytesRead = input.Read(buffer, 0, (int)Math.Min(bytesLeft, buffer.Length));
                output.Write(buffer, 0, bytesRead);
                bytesLeft -= bytesRead;
            } while (bytesLeft > 0 && bytesRead > 0);
        }

        private static void Rebuild(string gdiPath, string data, string ipBin, List<string> cdda, List<string> outPath, 
            string volume, bool useIsoSectors, bool truncate)
        {
            string gdiDirectory = Path.GetDirectoryName(gdiPath);
            string[] gdiLines = File.ReadAllText(gdiPath).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (cdda.Count == 0 && gdiLines.Length > 3 && int.TryParse(gdiLines[0], out int numTracks) && numTracks > 3)
            {
                //Original had CDDA and user didn't override, let's copy them
                for (int i = 4; i < numTracks && i < gdiLines.Length; i++)
                {
                    string track = ParseGdiTrackPath(gdiLines[i], gdiDirectory);
                    if (!File.Exists(track))
                    {
                        Console.WriteLine($"ERROR: Cannot find the CDDA track {Path.GetFileName(track)} referenced in the original .gdi file");
                        return;
                    }
                    cdda.Add(track);
                }
            }
            
            using (GDReader reader = GDReader.FromGDItext(gdiLines, gdiDirectory))
            {
                GDromBuilder builder;
                if (!string.IsNullOrEmpty(ipBin) && File.Exists(ipBin))
                {
                    builder = new GDromBuilder(ipBin, cdda);
                }
                else
                {
                    builder = new GDromBuilder(reader.ReadIPBin(), cdda);
                }
                builder.ReportProgress += ProgressReport;
                builder.RawMode = !useIsoSectors;
                builder.TruncateData = truncate;
                if (volume != null)
                {
                    builder.VolumeIdentifier = volume;
                }
                Console.Write("Writing");

                builder.ImportReader(reader);
                builder.ImportFolder(data, "", true);
                if (!Directory.Exists(outPath[0]))
                {
                    Directory.CreateDirectory(outPath[0]);
                }
                //Copy the PC tracks first
                for (int i = 1; i <= 2; i++)
                {
                    string srcPath = ParseGdiTrackPath(gdiLines[i], gdiDirectory);
                    if (File.Exists(srcPath))
                    {
                        File.Copy(srcPath, Path.Combine(outPath[0], Path.GetFileName(srcPath)), true);
                    }
                }
                //Next copy the CDDA
                foreach (string track in cdda)
                {
                    //We have already checked that all of these exist
                    File.Copy(track, Path.Combine(outPath[0], Path.GetFileName(track)), true);
                }
                //Next, save the data track(s)
                List<DiscTrack> tracks = builder.BuildGDROM(outPath[0]);
                //Finally, save the .gdi file
                builder.WriteGdiFile(gdiLines, tracks, Path.Combine(outPath[0], "disc.gdi"));
                Console.WriteLine(" Done!");
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

        private static void BuildDisc(string ipBin, List<string> cdda, bool useIsoSectors, bool truncate, 
            string data, string volume, bool fileOutput, List<string> outPath, string gdiPath)
        {
            GDromBuilder builder = new GDromBuilder(ipBin, cdda);
            builder.ReportProgress += ProgressReport;
            builder.RawMode = !useIsoSectors;
            builder.TruncateData = truncate;
            builder.ImportFolder(data);
            if (volume != null)
            {
                builder.VolumeIdentifier = volume;
            }
            Console.Write("Writing");
            List<DiscTrack> tracks = null;
            if (fileOutput)
            {
                builder.Track03Path = Path.GetFullPath(outPath[0]);
                if (outPath.Count == 2 && (cdda.Count > 0 || builder.TruncateData))
                {
                    builder.LastTrackPath = Path.GetFullPath(outPath[1]);
                }
                tracks = builder.BuildGDROM();
            }
            else
            {
                tracks = builder.BuildGDROM(outPath[0]);
            }
            Console.WriteLine(" Done!");
            if (gdiPath != null)
            {
                builder.UpdateGdiFile(tracks, gdiPath);
            }
            else
            {
                Console.WriteLine(builder.GetGDIText(tracks));
            }
        }

        private static void ProgressReport(int amount)
        {
            if (amount % 10 == 0)
            {
                Console.Write('.');
            }
        }

        private static bool CheckArguments(bool extracting, bool rebuild, string data, string ipBin, string gdiPath,
            List<string> outPath, List<string> cdda, bool truncate, out bool fileOutput)
        {
            fileOutput = false;
            if (extracting || rebuild)
            {
                if (string.IsNullOrEmpty(gdiPath) || !File.Exists(gdiPath) || !Path.GetExtension(gdiPath).ToLower().Equals(".gdi"))
                {
                    Console.WriteLine($"A .gdi file is required in {(extracting ? "extract" : "rebuild")} mode");
                    return false;
                }
            }
            else
            {
                //User wants to build a new high density data track from scratch
                if (gdiPath != null && !File.Exists(gdiPath))
                {
                    Console.WriteLine("The .gdi file specified does not exist.");
                    return false;
                }
            }
            if (extracting)
            {
                if (outPath.Count != 1)
                {
                    Console.WriteLine("Only one output path is allowed for extraction");
                    return false;
                }
                else if (!string.IsNullOrEmpty(Path.GetExtension(outPath[0])))
                {
                    Console.WriteLine("Extraction output must be a folder, not a file!");
                    return false;
                }
                return true; //This is all we need to check for extraction
            }
            if (data == null || (!rebuild && ipBin == null) || outPath.Count == 0)
            {
                Console.WriteLine("The required fields have not been provided.");
                return false;
            }
            if (!Directory.Exists(data))
            {
                Console.WriteLine("The specified data directory does not exist!");
                return false;
            }
            if (ipBin != null && !File.Exists(ipBin))
            {
                //Rebuild mode will use the existing IP.BIN, unless you specify one to replace it
                Console.WriteLine("The specified IP.BIN file does not exist!");
                return false;
            }
            foreach (string track in cdda)
            {
                if (!File.Exists(track))
                {
                    Console.WriteLine("The CDDA track " + track + " does not exist!");
                    return false;
                }
            }
            if (rebuild)
            {
                if (outPath.Count == 1)
                {
                    string path = outPath[0];
                    if (path.EndsWith(Path.DirectorySeparatorChar.ToString()) || !Path.HasExtension(path))
                    {
                        if (Path.GetDirectoryName(gdiPath).Equals(Path.GetDirectoryName(path)))
                        {
                            Console.WriteLine("Cannot rebuild the same GDI as the input. That would overwrite the disc we are reading files from!");
                            return false;
                        }
                        fileOutput = false;
                    }
                    else
                    {
                        Console.WriteLine("Rebuild mode requires a directory to output the new GDI!");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Rebuild mode only accepts a single directory as the output path, which cannot be the same as the input.");
                    return false;
                }
            }
            else if (outPath.Count > 2)
            {
                Console.WriteLine("Too many output paths specified.");
                return false;
            }
            else if (outPath.Count == 2)
            {
                fileOutput = true;
                if (!Path.HasExtension(outPath[0]) || !Path.HasExtension(outPath[1]))
                {
                    Console.WriteLine("Output filenames are not valid!");
                    return false;
                }
            }
            else
            {
                string path = outPath[0];
                if (path.EndsWith(Path.DirectorySeparatorChar.ToString()) || !Path.HasExtension(path))
                {
                    fileOutput = false;
                }
                else
                {
                    fileOutput = true;
                }
                if (truncate && fileOutput)
                {
                    Console.WriteLine("Can't output a single data track in truncated data mode.");
                    Console.WriteLine("Please provide two different output tracks.");
                    return false;
                }
                if (cdda.Count > 0 && fileOutput)
                {
                    Console.WriteLine("Can't output a single track when CDDA is specified.");
                    return false;
                }
            }
            return true;
        }

        private static bool HasArgument(string argument, string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals(argument, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private static string GetSoloArgument(string argument, string[] args)
        {
            for (int i = 0; i < args.Length-1; i++)
            {
                if (args[i].Equals(argument, StringComparison.OrdinalIgnoreCase))
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        private static List<string> GetMultiArgument(string argument, string[] args)
        {
            List<string> retval = new List<string>();
            int start = -1;
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i].Equals(argument, StringComparison.OrdinalIgnoreCase))
                {
                    start = i + 1;
                    break;
                }
            }
            if (start > 0)
            {
                for (int i = start; i < args.Length; i++)
                {
                    if (args[i].StartsWith("-"))
                    {
                        break;
                    }
                    else
                    {
                        retval.Add(args[i]);
                    }
                }
            }
            return retval;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("BuildGDI - Command line GDIBuilder");
            Console.WriteLine("Usage: buildgdi -data dataFolder -ip IP.BIN -cdda track04.raw track05.raw -output folder -gdi disc.gdi");
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            Console.WriteLine("-data <folder> (Required) = Location of the files for the disc");
            Console.WriteLine("-ip <file> (Required) = Location of disc IP.BIN bootsector");
            Console.WriteLine("-cdda <files> (Optional) = List of RAW CDDA tracks on the disc");
            Console.WriteLine("-output <folder or file(s)> (Required) = Output location");
            Console.WriteLine("   If output is a folder, tracks with default filenames will be generated.");
            Console.WriteLine("   Otherwise, specify one filename for track03.bin on data only discs, ");
            Console.WriteLine("   or two files for discs with CDDA.");
            Console.WriteLine("-gdi <file> (Optional) = Path of the disc.gdi file for this disc");
            Console.WriteLine("   Existing GDI files will be updated with the new tracks.");
            Console.WriteLine("   If no GDI exists, only lines for tracks 3 and above will be written.");
            Console.WriteLine("-V <volume identifier> (Optional) = The volume name (Default is DREAMCAST)");
            Console.WriteLine("-iso (Optional) = Output 2048 byte disc sectors found in ISO9660 instead of 2352");
            Console.WriteLine("-truncate (Optional) = Do not pad generated data to the correct size");
            Console.WriteLine("-rebuild (Optional) = Build a new GDI using an existing one as a data source");
            Console.WriteLine("   Requires the -gdi, -data and -output arguments. Files will be copied from ");
            Console.WriteLine("   the original disc. Files in the -data folder will be added to the copied ");
            Console.WriteLine("   disc if they are new, or replace existing files in the same location.");
            Console.WriteLine("   This requires -output to be a folder. -ip is optional to replace the existing IP.BIN.");
            Console.WriteLine("-extract (Optional) =  Extracts a GDI file to a folder");
            Console.WriteLine("   Extraction requires the -gdi and -output arguments. -ip is optional to extract IP.BIN.");
        }
    }
}
