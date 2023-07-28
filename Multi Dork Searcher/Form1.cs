using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Leaf.xNet;
using System.Text.RegularExpressions;
using System.Threading;
using HtmlAgilityPack;

namespace Multi_Dork_Searcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void google_search(string dork,string pages)
        {
            new Thread(() =>
            {
                for(int i = 0; i < Int32.Parse(pages); i++)
                {
                    try
                    {
                        HttpRequest request = new HttpRequest();
                        var dro = Uri.EscapeDataString(dork);
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36";
                        var response = request.Get($"https://www.google.com/search?q={dro}&start={i}0");

                        string pattern = @"https?://(www\.)?[^""\s<>']+";
                        MatchCollection matches = Regex.Matches(response.ToString(), pattern);
                        List<string> links = new List<string>();

                        var htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.LoadHtml(response.ToString());

                        var resultNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='yuRUbf']//a[@href]");
                        if (resultNodes != null)
                        {
                            foreach (var node in resultNodes)
                            {
                                string link = node.GetAttributeValue("href", "");
                                string title = Regex.Replace(node.InnerText, "<.*?>", string.Empty); // Remove HTML tags
                                var temp = link.Replace("://", "");
                                var domain = link.Split('/')[2];
                                domain = link.Split(':')[0] + "://" + domain;
                                //MessageBox.Show(link);
                                if (!links.Contains(domain) && !domain.Contains("google.") && !domain.Contains("googleusercontent"))
                                {

                                    links.Add(domain);
                                    richTextBox1.Invoke(new MethodInvoker(delegate
                                    {
                                        checkBox1.Invoke(new MethodInvoker(delegate
                                        {
                                            if (checkBox1.Checked)
                                            {
                                                richTextBox1.Text += link + "\n";
                                            }
                                            else
                                            {
                                                richTextBox1.Text += domain + "\n";
                                            }
                                        }));

                                        // MessageBox.Show(link);
                                    }));
                                }
                            }
                        }
                        

                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                MessageBox.Show("Finished!","Notice",MessageBoxButtons.OK,MessageBoxIcon.Information);
            })
            { IsBackground = true }.Start();
        }

        public void bing_search(string dork,string pages)
        {
            new Thread(() =>
            {
                for (int i = 0; i < Int32.Parse(pages); i++)
                {
                    try
                    {
                        HttpRequest request = new HttpRequest();
                        var dro = Uri.EscapeDataString(dork);
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36";
                        var response = request.Get($"https://www.bing.com/search?q={dro}&first={pages}0");

                        string pattern = @"https?://(www\.)?[^""\s<>']+";
                        MatchCollection matches = Regex.Matches(response.ToString(), pattern);
                        List<string> links = new List<string>();
                        var htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.LoadHtml(response.ToString());

                        var resultNodes = htmlDocument.DocumentNode.SelectNodes("//li[@class='b_algo']//h2//a[@href]");
                        if (resultNodes != null)
                        {
                            foreach (var node in resultNodes)
                            {
                                string link = node.GetAttributeValue("href", "");
                                string title = node.InnerText;
                                //MessageBox.Show(link);
                                var temp = link.Replace("://", "");
                                var domain = link.Split('/')[2];
                                domain = link.Split(':')[0] + "://" + domain;
                                if (!links.Contains(domain) && !domain.Contains("bing.") && !domain.Contains("bing"))
                                {

                                    links.Add(domain);
                                    richTextBox1.Invoke(new MethodInvoker(delegate
                                    {
                                        checkBox1.Invoke(new MethodInvoker(delegate
                                        {
                                            if (checkBox1.Checked)
                                            {
                                                richTextBox1.Text += link + "\n";
                                            }
                                            else
                                            {
                                                richTextBox1.Text += domain + "\n";
                                            }
                                        }));

                                        // MessageBox.Show(link);
                                    }));
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                MessageBox.Show("Finished!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            })
            { IsBackground = true }.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Google")
            {
                google_search(textBox1.Text, numericUpDown1.Value.ToString());
            }else if(comboBox1.Text == "Bing")
            {
                bing_search(textBox1.Text, numericUpDown1.Value.ToString());
            }
        }
    }
}
