using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using HtmlAgilityPack;

namespace Scraper
{
    public partial class Scraper : Form
    {
        string sourceFile;
        string savedir;
        string contactFile;
        Regex EmailRegex = new Regex(@"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
           + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
             + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
           + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})");
        Regex phoneRegex = new Regex(@"\(?([0-9]{3})\)?[-. ]([0-9]{3})[-. ]?([0-9]{4})");
        public Scraper()
        {
            InitializeComponent();
        }
        private void choose_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            OpenFileDialog1.Title = "Source File";
            OpenFileDialog1.FileName = "";
            OpenFileDialog1.Filter = "Excel File|*.xlsx;*.xls";
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = OpenFileDialog1.FileName;
            }
        }

        private void output_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Save";
            dialog.Filter = "Text Document(*.txt)|.txt";
            dialog.FileName = "doc";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dialog.FileName;
            }
        }

        private async void Execute_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null || textBox2.Text == null || textBox3.Text == null)
            {
                MessageBox.Show("Enter valid values");
                return;
            }
            sourceFile = textBox1.Text;
            savedir = textBox2.Text;
            contactFile = textBox3.Text;
            MessageBox.Show("The process may take some time depending on the number of URLs, availability of the website and connection speed.");
            Execute.Hide();
            choose.Hide();
            output.Hide();
            contact.Hide();
            label1.Show();
            Task task = new Task(work);
            task.Start();
            await task;
            MessageBox.Show("Done!");
            choose.Show();
            output.Show();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            label1.Hide();
            Execute.Show();
            contact.Show();
        }

        private void work()
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook xlWorkBook = xlApp.Workbooks.Open(sourceFile, 0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Worksheet xlWorkSheet = (Worksheet)xlWorkBook.Sheets[1];
            Range xlRange = xlWorkSheet.UsedRange;
            int rowCount = xlRange.Rows.Count;
            List<string> urls = new List<string>();
            for (int i = 3; i < rowCount; i++)
            {
                try
                {
                    Uri uri = new UriBuilder(xlRange.Cells[i, 3].Value2.ToString()).Uri;
                    if (!urls.Contains(uri.GetLeftPart(UriPartial.Authority)))
                    {
                        urls.Add(uri.GetLeftPart(UriPartial.Authority));
                    }
                }
                catch (Exception exc) { }
            }
            xlWorkSheet.Delete();
            xlRange.Delete();
            xlWorkBook.Close();
            xlApp.Quit();
            using (StreamWriter writer = new StreamWriter(savedir, true))
            {
                foreach (string url in urls)
                {
                    try
                    {
                        GetSource(url, writer);
                    }
                    catch { }
                }
            }
        }

        private void GetSource(string url, StreamWriter writer)
        {
            Uri uri = new UriBuilder(url).Uri;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                string data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                writer.Write("---------------" + uri.ToString() + "--------------" + Environment.NewLine);
                writer.Write(data);
                WriteDetails(uri, data);
                writer.Write(Environment.NewLine);
                ExtractAllAHrefTags(data, uri, writer);
                writer.Write("----------------------------------------------" + Environment.NewLine);
            }
        }

        private void ExtractAllAHrefTags(string doc, Uri uri, StreamWriter writer)
        {
            int contact = 0;
            int about = 0;
            HtmlAgilityPack.HtmlDocument htmlSnippet = new HtmlAgilityPack.HtmlDocument();
            htmlSnippet.LoadHtml(doc);
            foreach (HtmlNode link in htmlSnippet.DocumentNode.SelectNodes("//a[@href]"))
            {
                string att = link.Attributes["href"].Value.ToString();
                if (att.Contains("contact") || att.Contains("about"))
                {
                    if (contact == 1 && about == 1)
                        return;
                    if (att.Contains("contact") && contact == 0)
                    {
                        writeExtra(uri, att, writer, "contact");
                        contact = 1;
                    }
                    if (att.Contains("about") && about == 0)
                    {
                        writeExtra(uri, att, writer, "about");
                        about = 1;
                    }
                }
            }
        }

        private void writeExtra(Uri host, string att, StreamWriter writer, string extra)
        {
            if (extra.Equals("contact"))
            {
                writer.Write("*********************Contact Page*********************" + Environment.NewLine);
            }
            else
                writer.Write("*********************About Page*********************" + Environment.NewLine);
            if (att.StartsWith("http") == false)
            {
                string temp = host.ToString();
                if (temp[temp.Length - 1] != '/')
                {
                    temp += '/';
                }
                att = att.Insert(0, temp);
                if (att[temp.Length] == '/')
                    att = att.Remove(temp.Length, 1);
            }
            Uri uri = new UriBuilder(att).Uri;
            writer.Write("*********************" + uri.ToString() + "*********************" + Environment.NewLine);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                string data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                writer.Write(data);
                writer.Write(Environment.NewLine);
                writer.Write("************************************************" + Environment.NewLine);
                WriteDetails(uri, data);
            }
        }

        private void Scraper_Load(object sender, EventArgs e)
        {
            label1.Hide();
        }

        private void contact_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Save";
            dialog.Filter = "Text Document(*.txt)|.txt";
            dialog.FileName = "contact";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = dialog.FileName;
            }
        }
        private void WriteDetails(Uri uri, string data)
        {
            StreamWriter writer = new StreamWriter(contactFile, true);
            HtmlAgilityPack.HtmlDocument htmlSnippet = new HtmlAgilityPack.HtmlDocument();
            htmlSnippet.LoadHtml(data);
            writer.Write("---------------" + uri.ToString() + "--------------" + Environment.NewLine);
            Match match;
            writer.Write("********************Email Addresses**********************" + Environment.NewLine);
            foreach (HtmlNode paragraph in htmlSnippet.DocumentNode.SelectNodes("//p"))
            {
                for (match = EmailRegex.Match(@paragraph.InnerHtml); match.Success; match = match.NextMatch())
                {
                    writer.Write(match.Value + Environment.NewLine);
                }
            }
            writer.Write(Environment.NewLine + "********************Fax Numbers**********************" + Environment.NewLine);
            foreach (HtmlNode paragraph in htmlSnippet.DocumentNode.SelectNodes("//p"))
            {
                string par = paragraph.InnerHtml;
                if (par.IndexOf("Fax", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    par = par.Remove(0, par.IndexOf("Fax", StringComparison.OrdinalIgnoreCase));
                    match = phoneRegex.Match(par);
                    if (match.Success)
                    {
                        writer.Write(match.Value + Environment.NewLine);
                    }
                }
            }
            writer.Write("----------------------------------------------" + Environment.NewLine);
            writer.Close();
        }
    }
}