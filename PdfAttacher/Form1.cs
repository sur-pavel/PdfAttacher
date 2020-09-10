using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfAttacher
{
    public partial class Form1 : Form
    {
        IrbisHandler irbisHandler;
        FileHandler fileHandler;
        Logging logging;

        public Form1()
        {
            InitializeComponent();
            PasswordTextBox.PasswordChar = '*';
            logging = new Logging();
            logging.CreateLogFile();
            irbisHandler = new IrbisHandler(logging);
            fileHandler = new FileHandler(logging);
            //LoginTextBox.Text = "СПА";
            //PasswordTextBox.Text = "1";
            //PathFromTextBox.Text = @"d:\Auto951For\";
            //PathToTextBox.Text = @"d:\Auto951Done\";
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            bool connected = irbisHandler.ConnectToServer(LoginTextBox.Text, PasswordTextBox.Text, DatabaseTextBox.Text);
            if (connected)
            {
                InfoLabel.Text = "Connected!";
            }
            else
            {
                InfoLabel.Text = "";
            }
        }

        private void AttachButton_Click(object sender, EventArgs e)
        {
            FileInfo[] fileInfos = fileHandler.GetOldFileNames(PathFromTextBox.Text);
            int filesLength = fileInfos.Length;
            int inc = 0;
            int founded = 0;
            foreach (FileInfo fileInfo in fileInfos)
            {
                string text = LogTextBox.Text;

                ChangeLogTextBox("File: " + fileInfo.Name);

                inc++;
                ProgressBar.Value = inc * 100 / filesLength;

                if (irbisHandler.SearchInIrbis(fileInfo))
                {
                    ChangeLogTextBox("Founded");
                    fileHandler.MoveFile(fileInfo, PathToTextBox.Text);
                    ChangeLogTextBox("Moved" + Environment.NewLine);
                    LogTextBox.Refresh();
                    founded++;
                }
                else
                {
                    ChangeLogTextBox("Not founded");
                    ChangeLogTextBox("Renamed" + Environment.NewLine);
                    fileHandler.RenameFile(fileInfo);
                }
                logging.WriteLine("\n\n-------------------------------\n");
            }
            ChangeLogTextBox("-------------------------------------------");
            ChangeLogTextBox("FOUNDED " + founded + " OF " + filesLength + " FILES (" + founded * 100 / filesLength + " %).");

        }


        private void PathFromButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                PathFromTextBox.Text = folderBrowserDialog.SelectedPath + @"\";
            }
        }

        private void PathToButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                PathToTextBox.Text = folderBrowserDialog.SelectedPath + @"\";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var t = Task.Run(() => irbisHandler.Disconnect());
            t.Wait();
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            bool disconnected = irbisHandler.Disconnect();
            if (disconnected)
            {
                InfoLabel.Text = "Disconnected!";
            }
            else
            {
                InfoLabel.Text = "";
            }
        }
        private void ChangeLogTextBox(string str)
        {
            LogTextBox.AppendText(str + Environment.NewLine);
            LogTextBox.Refresh();
        }
    }
}
