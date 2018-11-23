using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using WPFFolderBrowser;

namespace InformationSecurity
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] encryptedRSA;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateKeys(object sender, RoutedEventArgs e)
        {
            try
            {
                //Generate keys
                RSACryptoServiceProvider rsaGenKeys = new RSACryptoServiceProvider();
                string privateXml = rsaGenKeys.ToXmlString(true);
                string publicXml = rsaGenKeys.ToXmlString(false);
                Public.Text = publicXml;
                Private.Text = privateXml;
                Error.Text = "";
            }
            catch { Error.Text = "Ошибка"; }
        }

        private void Encrypt(object sender, RoutedEventArgs e)
        {
            try
            {
                //Encode with public key
                byte[] toEncryptData = Encoding.ASCII.GetBytes(Input.Text);
                RSACryptoServiceProvider rsaPublic = new RSACryptoServiceProvider();
                rsaPublic.FromXmlString(Public.Text);
                encryptedRSA = rsaPublic.Encrypt(toEncryptData, false);
                string EncryptedResult = Encoding.Default.GetString(encryptedRSA);
                Output.Text = EncryptedResult;
                Error.Text = "";
            }
            catch { Error.Text = "Ошибка"; }
        }

        private void Decrypt(object sender, RoutedEventArgs e)
        {
            try
            {
                //Decode with private key
                var rsaPrivate = new RSACryptoServiceProvider();
                rsaPrivate.FromXmlString(Private.Text);
                byte[] decryptedRSA = rsaPrivate.Decrypt(encryptedRSA, false);
                string originalResult = Encoding.Default.GetString(decryptedRSA);
                Output.Text = originalResult;
                Error.Text = "";
            }
            catch { Error.Text = "Ошибка"; }
        }
    }
}
