using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using Windows.ApplicationModel;
using Windows.Storage;

namespace OCTranspo.Models
{
    class OCZipFile
    {
        public static async void UnZipOCTranspo()
        {
            String finalPath =  ApplicationData.Current.LocalFolder.Path + "/OCTranspo.sqlite";
            DateTime creationTime = File.GetLastWriteTime("Assets/OCTranspo.zip");
            if (File.Exists(finalPath) == false || checkDateTime(creationTime))
            {
                ExtractZipFile("Assets/OCTranspo.zip", "", ApplicationData.Current.LocalFolder.Path);
                bool isDatabaseExisting = false;
                try
                {
                    StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("OCTranspo.sqlite");
                    isDatabaseExisting = storageFile != null;
                }
                catch
                {
                    isDatabaseExisting = false;
                }

                if (isDatabaseExisting == false)
                {
                    StorageFile dbFile = await StorageFile.GetFileFromPathAsync(finalPath);
                    await dbFile.CopyAsync(ApplicationData.Current.LocalFolder);
                }
            }
        }

        private static Boolean checkDateTime(DateTime time)
        {
            Boolean result = false;
            String path = ApplicationData.Current.LocalFolder.Path + "/time.txt";
            if (File.Exists(path)) {

                TextReader reader = new StreamReader(path);
                String date = reader.ReadLine();
                reader.Close();
                DateTime timeFile = DateTime.Parse(date);
                int compared = timeFile.CompareTo(time);
                if (compared > 0)
                {
                    TextWriter tw = new StreamWriter(path);
                    tw.WriteLine(time.ToLongDateString() + " " + time.ToLongTimeString());
                    tw.Close();
                    result = true;
                }
            } else {
                TextWriter tw = new StreamWriter(path);
                tw.WriteLine(time.ToLongDateString() + " " + time.ToLongTimeString());
                tw.Close();
            }
            return result;
        }

        private static void ExtractZipFile(string archiveFilenameIn, string password, string outFolder)
        {
            ZipFile zf = null;
            try
            {
                FileStream fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                if (!String.IsNullOrEmpty(password))
                {
                    zf.Password = password;     // AES encrypted entries are handled automatically
                }
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;           // Ignore directories
                    }
                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    byte[] buffer = new byte[4096];     // 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    String fullZipToPath = Path.Combine(outFolder, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
        }
    }

}


