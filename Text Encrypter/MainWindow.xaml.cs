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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
namespace Text_Encrypter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            catch(Exception ex)
            {
                txtOutput.Text = "An error occurred:\n" + ex.ToString();
                
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
