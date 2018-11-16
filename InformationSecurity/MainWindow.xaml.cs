using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        GOST G = new GOST(256);
        GOST G512 = new GOST(512);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var message = Encoding.UTF8.GetBytes(Input.Text);
            byte[] res = G.GetHash(message);
            byte[] res2 = G512.GetHash(message);
            string h256 = BitConverter.ToString(res);
            string h512 = BitConverter.ToString(res2);
            Output.Text = h256 + "\n" + h512;
        }
    }
}
