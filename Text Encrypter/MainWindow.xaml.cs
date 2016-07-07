using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Net;
using System.IO;
namespace Text_Encrypter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string CurrentVersion = "v0.2-alpha";
        public MainWindow()
        {
            InitializeComponent();
            lblVersion.Content = CurrentVersion;
            try { 
            if (File.ReadAllText("updatedrecently") == "true")
            {
                File.Copy("update.exe", "TextEncrypter.exe");
                File.WriteAllText("updatedrecently", "false");

            }
            }catch
            {

            }
            try
            {
                var webRequest = WebRequest.Create(@"https://raw.githubusercontent.com/bitsol/Text-Encrypter/master/latest.txt");

                var response = webRequest.GetResponse();
                var content = response.GetResponseStream();
                var reader = new StreamReader(content);

                string latestVersion = reader.ReadToEnd().Replace("\n", "");
                if (latestVersion != CurrentVersion)
                {
                    if (MessageBox.Show("An update to Text-Encrypter was found. Would you like to download it?", "Update", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {

                        string remoteUri = "https://github.com/bitsol/Text-Encrypter/releases/download/" + latestVersion + "/TextEncrypter.exe";
                        string fileName = "update.exe";
                        WebClient myWebClient = new WebClient();
                        string myStringWebResource = remoteUri;
                        myWebClient.DownloadFile(myStringWebResource, fileName);
                        File.WriteAllText("updatedrecently", "true");
                        Process.Start("update.exe");
                        Environment.Exit(0);

                    }
                }
            }catch { }
            try
            {
                txtPasscode.Text = File.ReadAllText(@"passcode.txt");
            }
            catch
            {
            }
            Update();
        }
        private void radioButtonEncrypt_Checked(object sender, RoutedEventArgs e)
        {
            this.Title = "Text Encrypter";
            txtInput.SpellCheck.IsEnabled = true;

            Switch();
        }
        private void Switch()
        {
            string temp = txtOutput.Text;
            txtOutput.Text = txtInput.Text;
            txtInput.Text = temp;
        }
        private void radioButtonDecrypt_Checked(object sender, RoutedEventArgs e)
        {
            this.Title = "Text Decrypter";
            txtInput.SpellCheck.IsEnabled = false;
            Switch();
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }
        private void Update()
        {
            try
            {
                Random rnd = new Random();
                if (!File.Exists(@"passcode.txt"))
                {
                    File.WriteAllText(@"passcode.txt", rnd.Next(1, 29999).ToString());
                    txtPasscode.Text = File.ReadAllText(@"passcode.txt");

                }

                int passcode = int.Parse(File.ReadAllText(@"passcode.txt"));
                if (txtPasscode.Text != "")
                {

                    passcode = int.Parse(txtPasscode.Text);
                    if (passcode > 30000)
                    {
                        throw new Exception("Passcode validation error!");
                    }

                    string end = "";
                    if (radioButtonEncrypt.IsChecked == true)
                    {

                        foreach (char c in txtInput.Text)
                        {
                            end += Convert.ToString(Convert.ToChar((c + passcode)));
                        }

                    }
                    else if (radioButtonDecrypt.IsChecked == true)
                    {
                        foreach (char c in txtInput.Text)
                        {
                            end += Convert.ToString(Convert.ToChar((c - passcode)));
                        }

                    }
                    txtOutput.Text = end;
                }else
                {
                    
                }
            }
            catch
            {
                txtOutput.Text = "An error occurred.";
                
            }
        }

        private void txtPasscode_TextChanged(object sender, TextChangedEventArgs e)
        {
            File.WriteAllText(@"passcode.txt", txtPasscode.Text);
            Update();
        }

        private void radioButtonEncrypt_Loaded(object sender, RoutedEventArgs e)
        {
            radioButtonEncrypt.IsChecked = true;
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtOutput.Text);
        }

     
        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            File.Delete(@"passcode.txt");
            Update();
        }

       
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
