﻿using System;
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
            
            DatabaseTextBox.Text = "MPDA";
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            InfoLabel.Text = irbisHandler.ConnectToServer(LoginTextBox.Text, PasswordTextBox.Text, DatabaseTextBox.Text);            
        }

        private void AttachButton_Click(object sender, EventArgs e)
        {
            if (InfoLabel.Text.Equals("Подключено!"))
            {
                fileHandler.CreateFolder(TroublePathTextBox.Text + NOT_FOUND_FOLDER);
                fileHandler.CreateFolder(TroublePathTextBox.Text + FEW_RECORDS_FOLDER);
                fileHandler.CreateFolder(TroublePathTextBox.Text + DOUBLET_FOLDER);
                FileInfo[] fileInfos = fileHandler.GetOldFileNames(PathFromTextBox.Text);
                int filesLength = fileInfos.Length;
                int inc = 0;
                int founded = 0;
                string message;
                foreach (FileInfo fileInfo in fileInfos)
                {
                    string text = LogTextBox.Text;

                    WriteToLogTextBox("Файл: " + fileInfo.Name);

                    inc++;
                    ProgressBar.Value = inc * 100 / filesLength;

                    IrbisRequest irbisRequest = irbisHandler.SearchInIrbis(fileInfo);
                    
                    if (irbisRequest.FileStatus == IrbisRequest.Status.NotFounded)
                    {
                        WriteToLogTextBox("Запись не найдена" + Environment.NewLine);
                        fileHandler.MoveFile(fileInfo, TroublePathTextBox.Text + NOT_FOUND_FOLDER);
                    }
                    else if (irbisRequest.FileStatus == IrbisRequest.Status.OneRecord)
                    {
                        WriteToLogTextBox("Найдена запись" + Environment.NewLine);
                        fileHandler.MoveFile(fileInfo, PathToTextBox.Text + @"\");
                        founded++;
                    }
                    else if (irbisRequest.FileStatus == IrbisRequest.Status.FewRecords)
                    {
                        WriteToLogTextBox("Найдено несколько записей" + Environment.NewLine);
                        fileHandler.MoveFile(fileInfo, TroublePathTextBox.Text + FEW_RECORDS_FOLDER);
                        founded++;
                    }
                    else if (irbisRequest.FileStatus == IrbisRequest.Status.Doublet)
                    {
                        message = "Поле 951^a заполнено";
                        logging.WriteLine(message);
                        WriteToLogTextBox(message + Environment.NewLine);
                        fileHandler.MoveFile(fileInfo, TroublePathTextBox.Text + DOUBLET_FOLDER);
                        founded++;
                    }

                    logging.WriteLine("\n-------------------------------\n");
                }

                if (filesLength > 0)
                {
                    message = "-------------------------------------------" + Environment.NewLine + "Найдено " + founded + " из " + filesLength + " файлов (" + founded * 100 / filesLength + " %).";
                    WriteToLogTextBox(message);
                    logging.WriteLine(message);
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
            bool disПодключено = irbisHandler.Disconnect();
            if (disПодключено)
            {
                InfoLabel.Text = "Отключено!";
            }
            else
            {
                InfoLabel.Text = "";
            }
        }
        private void WriteToLogTextBox(string str)
        {
            LogTextBox.AppendText(str + Environment.NewLine);
            LogTextBox.Refresh();
        }

        private void ProgressBar_Click(object sender, EventArgs e)
        {

        }


    }
}
