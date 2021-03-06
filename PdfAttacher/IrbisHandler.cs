﻿using ManagedClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfAttacher
{
    class IrbisHandler

    {
        ManagedClient64 client = new ManagedClient64();
        internal List<string> notFounded = new List<string>();
        internal int pdfPages = 0;
        private Logging logging;

        public IrbisHandler(Logging logging)
        {
            this.logging = logging;
        }

        internal string ConnectToServer(string login, string password, string database)
        {

            string message = "";
            try
            {
                client.ParseConnectionString("host=194.169.10.3;port=8888; user=" + login + ";password=" + password + ";");

                client.Connect();
                client.PushDatabase(database);

                message = "Подключено!";
                logging.WriteLine("Connected to IRBIS successfully.");
                message += " " + client.StageOfWork;
                logging.WriteLine("Stage:" + client.StageOfWork);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.ToString());
                logging.WriteLine("IRBIS ERROR!");
                logging.WriteLine(ex.StackTrace);
                logging.WriteLine(ex.ToString());
            }
            return message;
        }

        internal bool Disconnect()
        {
            bool disconnected = false;
            try
            {
                client.Disconnect();
                logging.WriteLine("Disconnected from IRBIS successfully.");
                disconnected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.ToString());
                logging.WriteLine("IRBIS ERROR!");
                logging.WriteLine(ex.StackTrace);
                logging.WriteLine(ex.ToString());
            }
            return disconnected;
        }

        internal IrbisRequest SearchInIrbis(FileInfo fileInfo)
        {
            logging.WriteLine("Filename: " + fileInfo.Name + "\n");
            IrbisRequest irbisRequest = new IrbisRequest();
            FillIrbisRequest(fileInfo, irbisRequest);
            try
            {

                if (!irbisRequest.Year.Equals("") && irbisRequest.TitleKeyWords.Length > 0)
                {
                    int[] foundRecordsMFN = new int[] { };
                    if (irbisRequest.VolNum.Equals(""))
                    {
                        foundRecordsMFN = SimpleSearch(client, irbisRequest.Author, irbisRequest.Title, irbisRequest.Year);
                    }
                    if (foundRecordsMFN.Length != 1)
                    {
                        foundRecordsMFN = SequentialSearch(client,
                            irbisRequest.Author, irbisRequest.TitleKeyWords,
                            irbisRequest.Year, irbisRequest.VolNum);
                    }
                    if (foundRecordsMFN.Length == 0)
                    {
                        logging.WriteLine("No founded records in Irbis");
                        irbisRequest.FileStatus = IrbisRequest.Status.NotFounded;
                    }
                    else
                    {
                        if (foundRecordsMFN.Length == 1)
                        {
                            irbisRequest.FileStatus = IrbisRequest.Status.OneRecord;
                            logging.WriteLine("1 record founded in Irbis");
                        }
                        else
                        {
                            irbisRequest.FileStatus = IrbisRequest.Status.FewRecords;
                            logging.WriteLine(foundRecordsMFN.Length + " records founded in irbis");
                        }

                        foreach (int mfn in foundRecordsMFN)
                        {
                            logging.WriteLine("\nMFN " + mfn);
                            IrbisRecord record = client.ReadRecord(mfn);
                            irbisRequest.FileNameFromRecord = GetFileNameFromRecord(record);
                            logging.WriteLine("File name from record: " + irbisRequest.FileNameFromRecord);
                            string[] fields951A = record.FMA("951", 'A');
                            bool field951exist = false;
                            foreach (string field in fields951A)
                            {
                                if (!field.Equals("") && !field.Contains("FULLTEXT"))
                                {
                                    field951exist = true;
                                    if (irbisRequest.FileStatus == IrbisRequest.Status.OneRecord)
                                    {
                                        irbisRequest.FileStatus = IrbisRequest.Status.Doublet;

                                    }
                                    break;
                                }                                
                            }
                            if (!field951exist)
                            {
                                WriteToRecord(record, fileInfo);
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.ToString());
                logging.WriteLine(ex.StackTrace);
                logging.WriteLine(ex.ToString());
            }
            return irbisRequest;
        }

       
        private void FillIrbisRequest(FileInfo fileInfo, IrbisRequest irbisRequest)
        {
            string reformatFileName = ReformatFileName(fileInfo);
            irbisRequest.Author = GetAuthor(reformatFileName);
            irbisRequest.Title = GetTitle(reformatFileName);
            irbisRequest.TitleKeyWords = GetTitleKeyWords(irbisRequest.Title);
            irbisRequest.Year = GetYear(reformatFileName);
            irbisRequest.VolNum = GetVolumeNumber(fileInfo);
        }

        private string GetFileNameFromRecord(IrbisRecord record)
        {

            string fileName;
            string lastName;
            string title;
            string year;
            string volume = GetSubField(record, 200, 'v');

            if (volume.Equals(""))
            {
                lastName = GetSubField(record, 700, 'a');

                title = GetSubField(record, 200, 'a');
                title = CheckTitleLength(title);

                year = GetSubField(record, 210, 'd');
            }
            else
            {
                lastName = GetSubField(record, 961, 'a');

                title = GetSubField(record, 461, 'c');

                year = GetSubField(record, 210, 'd');
                year = year.Equals("") ? GetSubField(record, 461, 'h') : year;
            }

            fileName = string.Format(
            "{0}_{1}_{2}_{3}",
             lastName, title, volume, year
            );


            return fileName;
        }

        private string GetVolumeNumber(FileInfo file)
        {
            string volNum = "";
            Match match = Regex.Match(file.Name, @"_[ЧТTВBКK](асть|ом|ып|ыпуск|н|нига)?\.? ?\d+(_|\s)"); /*russian and engish literals*/
            if (match.Success)
            {
                volNum = Regex.Match(match.Value, @"\d+").Value;
                logging.WriteLine("Volume Number: " + volNum);
            }
            return volNum;
        }
        internal string ReformatFileName(FileInfo file)
        {
            string fileName = file.ToString()
                .Replace(".pdf", "")
                .Replace(".", ". ")
                .Replace("--", " ")
                .Replace("-", " ");
            fileName = Regex.Replace(fileName, @"^\d+_|^\d\w_", "");

            logging.WriteLine("Reformated file name: " + fileName);

            return fileName;
        }

        private int[] SequentialSearch(ManagedClient64 client, string author, string[] titleKeyWords, string year, string volNum)
        {
            List<string> filteredKeyWords = new List<string>();

            if (volNum.Equals(""))
            {
                if (!author.Equals(""))
                {
                    filteredKeyWords.Add(string.Format("(v700:'{0}' or v702:'{0}')", author));
                }

                int size = titleKeyWords.Length;
                if (size > 10) size = 10;
                for (int i = 0; i < size; i++)
                {
                    string keyWord = titleKeyWords[i];

                    if (keyWord.Length > 1 && !keyWord.Contains("?"))
                    {
                        filteredKeyWords.Add(string.Format("v200:'{0}'", keyWord));
                    }
                }
                filteredKeyWords.Add("v900^b:'05'");
            }

            else
            {
                filteredKeyWords.Add(string.Format("v961:'{0}'", author));
                filteredKeyWords.Add(string.Format("v200^v:'{0}'", volNum));
                int size = titleKeyWords.Length;
                if (size > 10) size = 10;
                for (int i = 0; i < size; i++)
                {
                    string keyWord = titleKeyWords[i];

                    if (keyWord.Length > 1 && !keyWord.Contains("?"))
                    {
                        filteredKeyWords.Add(string.Format("v461:'{0}'", keyWord));
                    }
                }

                filteredKeyWords.Add("v900^b:'03'");
            }

            string searchYear = string.Format("\"G={0}\"", year);
            string searchTitle = string.Join(" and ", filteredKeyWords.ToArray());

            //searchTitle = "v200: 'Жизнь' and v200: 'митрополита'";

            logging.WriteLine("---Sequential Search\nTherm for search title: " + searchTitle);
            logging.WriteLine("Therm for search year: " + searchYear);


            int[] foundRecordsMFN = client.SequentialSearch(searchYear, searchTitle);
            logging.WriteLine("MFNs count = " + foundRecordsMFN.Length);
            if (foundRecordsMFN.Length < 1 && titleKeyWords.Length > 3)
            {
                Array.Resize(ref titleKeyWords, 3);
                foundRecordsMFN = SequentialSearch(client, author, titleKeyWords, year, volNum);
            }
            if (foundRecordsMFN.Length < 1 && titleKeyWords.Length > 2)
            {
                Array.Resize(ref titleKeyWords, 2);
                foundRecordsMFN = SequentialSearch(client, author, titleKeyWords, year, volNum);
            }
            return foundRecordsMFN;
        }

        private int[] SimpleSearch(ManagedClient64 client, string author, string title, string year)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("\"KA={0}$\"", author);
            builder.AppendFormat("*\"T={0}$\"", title);
            builder.AppendFormat("*\"G={0}$\"", year);

            string searchTherm = builder.ToString();
            logging.WriteLine("---Simple search\nSearch Therm: " + searchTherm);

            return client.Search(searchTherm);
        }



        private string GetAuthor(string fileName)
        {
            string author = fileName.Split('_')[0];
            author = CleanWord(author);
            author = author.Split(' ')[0];
            if (author != "") logging.WriteLine("Author: " + author);
            return author;
        }

        internal string GetTitle(string fileName)
        {
            string[] nameSplit = fileName.Split('_');
            string title = nameSplit.Length > 1 ? nameSplit[1] : "";
            if (title != "") logging.WriteLine("Title: " + title);
            return title;
        }

        internal string[] GetTitleKeyWords(string title)
        {
            string titleClean = CleanWord(title);
            IStemmer stemmer = new RussianStemmer();
            string[] titleKeyWords = titleClean.Split(' ');
            bool notRus = false;
            if (Regex.IsMatch(titleKeyWords[0], "[A-z]"))
            {
                notRus = true;
            }
            List<string> keyWords = new List<string>();

            foreach (string titleKeyWord in titleKeyWords)
            {
                string keyWord = stemmer.Stem(titleKeyWord);
                keyWord = ReduceIfMatch(keyWord);
                if (keyWord.Length > 2)
                {
                    if (notRus == true && !Regex.IsMatch(keyWord, "[A-z]"))
                    {
                        break;
                    }
                    keyWords.Add(keyWord);
                }
            }
            return keyWords.ToArray();
        }

        private string ReduceIfMatch(string keyWord)
        {
            if (Regex.IsMatch(keyWord.ToLower(), "^учеб[а-я]+")) keyWord = "учеб";
            return keyWord;
        }

        internal string GetYear(string fileName)
        {
            string year = "";
            Regex regex = new Regex(@"_\d{4}[^с]?");
            MatchCollection matches = regex.Matches(fileName);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    year = match.Value.Replace("_", "");
                }
            }
            if (year != "") logging.WriteLine("year: " + year + "\n");
            return year;
        }

        private string CleanWord(string word)
        {
            return Regex.Replace(word, "[-.?!)(,:«»\"\"]", "");
        }

        private void WriteToRecord(IrbisRecord record, FileInfo fileInfo)
        {
            string[] fields951A = record.FMA("951", 'A');
            logging.WriteLine("found " + fields951A.Length + " fields with tag 951");
            for (int i = 0; i < fields951A.Length ; i++)
            {
                logging.WriteLine("Field 951A occurrence " + i + " text: " + fields951A[i]);
                if (fields951A[i].Contains("FULLTEXT"))
                {                    
                    record.RemoveField("951", i);
                }
            }
            record.AddField("951", 'A', fileInfo.Name, 'H', "01a", '2', "8m");
            //record.AddField("907", 'C', "Auto951Opac", 'A', DateTime.Now.ToString("yyyyMMdd"), 'B', this.user);
            try
            {
                int mfn = record.Mfn;
                client.WriteRecord(record, false, true);
                record = client.ReadRecord(mfn);
                logging.WriteLine("Write to record with mfn " + mfn + " field 951: " + record.FM("951", 'A'));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.ToString());
                logging.WriteLine(ex.Message);
                logging.WriteLine(ex.StackTrace);
            }
        }


        private string GetSubField(IrbisRecord irbisRecord, int num, char code)
        {

            string field = num.ToInvariantString();
            string subField = irbisRecord
                .Fields
                .GetField(field)
                .GetSubField(code)
                .GetSubFieldText()
                .FirstOrDefault();
            return subField == null ? "" : subField;
        }

        private static string CheckTitleLength(string title)
        {
            string[] titleWords = title.Split(' ');
            int wordsInTitle = 15;
            if (titleWords.Length > wordsInTitle)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < wordsInTitle; i++)
                {
                    builder.Append(titleWords[i]);
                    if (i != wordsInTitle - 1) builder.Append(" ");
                }
                builder.Append("...");
                title = builder.ToString();
            }
            return title;
        }
    }
}
