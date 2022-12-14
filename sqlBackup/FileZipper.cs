using SevenZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;


namespace DatabaseBackup
{
    public class FileZipper
    {
        public void Zip(string fileName,string folder,int MB)
        {
            try
            {
                string dll = System.Windows.Forms.Application.StartupPath + @"\SevenZipSharp.dll";
                if (Is64BitSystem)
                {
                    dll = dll.Replace("SevenZipSharp.dll", @"x64\7z.dll");
                } else
                {
                    dll = dll.Replace("SevenZipSharp.dll", @"x86\7z.dll");
                }
                string source = folder;
                string output = fileName.Replace(".bak", ".7z");
                if (File.Exists(dll))
                {
                    SevenZipExtractor.SetLibraryPath(dll);
                }
                SevenZipCompressor compressor = new SevenZipCompressor();
                compressor.IncludeEmptyDirectories = false;
                compressor.ArchiveFormat = OutArchiveFormat.SevenZip;
                compressor.CompressionMode = SevenZip.CompressionMode.Create;
                compressor.TempFolderPath = System.IO.Path.GetTempPath();
                compressor.FastCompression = true;
                compressor.VolumeSize = MB * 1024 * 1024;
                compressor.CompressDirectory(source, output);
            }
            catch(Exception e)
            {
                string a = e.Message;
            }
        }
        private bool Is64BitSystem
        {
            get
            {
                return Directory.Exists(Environment.ExpandEnvironmentVariables(@"%PROGRAMFILES(X86)%"));
            }
        }

    }


}
