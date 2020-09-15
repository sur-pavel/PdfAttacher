using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfAttacher
{
    class FileHandler
    {

        internal List<string> producers = new List<string>();

        private Logging logging;

        public FileHandler(Logging logging)
        {
            this.logging = logging;
        }

        internal FileInfo[] GetOldFileNames(string pathFrom)
        {

            DirectoryInfo info = new DirectoryInfo(pathFrom);

            FileInfo[] files = info.GetFiles("*.pdf");
            if (files.Length == 0)
            {
                MessageBox.Show("В папке \"" + pathFrom + "\" нет pdf-файлов");
            }

            return files;
        }

        internal void MoveFile(FileInfo fileInfo, string pathTo)
        {
            try
            {

                File.Move(fileInfo.FullName, pathTo + fileInfo.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.ToString());
                logging.WriteLine(pathTo + fileInfo.Name);
                logging.WriteLine("EXCEPTION: " + ex);
            }
        }

        internal void CreateFolder(string directory)
        {
            try
            {
                Directory.CreateDirectory(directory);
                logging.WriteLine("Directory \"" + directory + "\" was created");
            }
            catch (Exception ex)
            {
                logging.WriteLine("EXCEPTION: " + ex);
            }
        }
    }
}
