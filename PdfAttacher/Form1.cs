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
        private const string NOT_FOUND_FOLDER = @"\not_found\";
        private const string FEW_RECORDS_FOLDER = @"\2_records\";
        private const string DOUBLET_FOLDER = @"\951_est\";
        public Form1()
        {
            InitializeComponent();
            PasswordTextBox.PasswordChar = '*';
            logging = new Logging();
            logging.CreateLogFile();
            irbisHandler = new IrbisHandler(logging);
            fileHandler = new FileHandler(logging);
            LoginTextBox.Text = "СПА";
            PasswordTextBox.Text = "1";
            DatabaseTextBox.Text = "MPDA";
            PathFromTextBox.Text = @"d:\Auto951For";
            PathToTextBox.Text = @"d:\Auto951Done";
            TroublePathTextBox.Text = @"d:\Auto951Probl";
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
            if (InfoLabel.Text.Equals("Connected!"))
            {
                fileHandler.CreateFolder(TroublePathTextBox.Text + NOT_FOUND_FOLDER);
                fileHandler.CreateFolder(TroublePathTextBox.Text + FEW_RECORDS_FOLDER);
                fileHandler.CreateFolder(TroublePathTextBox.Text + DOUBLET_FOLDER);
                FileInfo[] fileInfos = fileHandler.GetOldFileNames(PathFromTextBox.Text);
                int filesLength = fileInfos.Length;
                int inc = 0;
                int founded = 0;
                foreach (FileInfo fileInfo in fileInfos)
                {
                    string text = LogTextBox.Text;

                    ChangeLogTextBox("Файл: " + fileInfo.Name);

                    inc++;
                    ProgressBar.Value = inc * 100 / filesLength;

                    IrbisRequest irbisRequest = irbisHandler.SearchInIrbis(fileInfo);
                    if (irbisRequest.FileStatus == IrbisRequest.Status.Founded)
                    {
                        ChangeLogTextBox("Найдена запись" + Environment.NewLine);
                        fileHandler.MoveFile(fileInfo, PathToTextBox.Text + @"\");
                        founded++;
                    }
                    else if (irbisRequest.FileStatus == IrbisRequest.Status.NotFounded)
                    {
                        ChangeLogTextBox("Запись не найдена" + Environment.NewLine);
                        fileHandler.MoveFile(fileInfo, TroublePathTextBox.Text + NOT_FOUND_FOLDER);
                    }
                    else if (irbisRequest.FileStatus == IrbisRequest.Status.FewRecords)
                    {
                        ChangeLogTextBox("Найдено несколько записей" + Environment.NewLine);
                        fileHandler.MoveFile(fileInfo, TroublePathTextBox.Text + FEW_RECORDS_FOLDER);
                        founded++;
                    }
                    else if (irbisRequest.FileStatus == IrbisRequest.Status.Doublet)
                    {
                        ChangeLogTextBox("Поле 951^a заполнено" + Environment.NewLine);
                        fileHandler.MoveFile(fileInfo, TroublePathTextBox.Text + DOUBLET_FOLDER);
                        founded++;
                    }

                    logging.WriteLine("\n\n-------------------------------\n");
                }

                if (filesLength > 0)
                {
                    ChangeLogTextBox("-------------------------------------------");
                    ChangeLogTextBox("FOUNDED " + founded + " OF " + filesLength + " FILES (" + founded * 100 / filesLength + " %).");
                }
            }
            else
            {
                MessageBox.Show("Программа не подключена к Ирбису!");
            }
        }

       

        private void PathFromButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                PathFromTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void PathToButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                PathToTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void TroublPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                TroublePathTextBox.Text = folderBrowserDialog.SelectedPath;
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

        private void ProgressBar_Click(object sender, EventArgs e)
        {

        }


    }
}
