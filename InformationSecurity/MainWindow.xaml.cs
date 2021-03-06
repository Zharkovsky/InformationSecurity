﻿using Microsoft.Win32;
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
        int N = 0;

        char[] characters = new char[] { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
                                                'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С',
                                                'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ',
                                                'Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7',
                                                '8', '9', '0' };

        public MainWindow()
        {
            InitializeComponent();
            N = characters.Length;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!Checked(Input.Text)) return;
            Output.Text = Encode(Input.Text, Key.Text);
        }

        private bool Checked(string text)
        {
            var result = true;

            text.ToUpper().ToList().ForEach(_ => result = result && characters.Contains(_));
            Key.Text.ToUpper().ToList().ForEach(_ => result = result && characters.Contains(_));
            if (!result)
            {
                Message.Text = "Символ ключа или входного текста не входит в алфавит";
            }
            else Message.Text = "";
            return result;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!Checked(Output.Text)) return;
            Input.Text = Decode(Output.Text, Key.Text);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Input.Text = Encoding.UTF8.GetString(File.ReadAllBytes(openFileDialog.FileName));
            }
        }

        private string Encode(string input, string keyword)
        {
            input = input.ToUpper();
            keyword = keyword.ToUpper();

            string result = "";

            int keyword_index = 0;

            foreach (char symbol in input)
            {
                if ((keyword_index) == keyword.Length)
                    keyword_index = 0;

                int c = (Array.IndexOf(characters, symbol) +
                    Array.IndexOf(characters, keyword[keyword_index])) % N;

                result += characters[c];

                keyword_index++;

                
            }

            return result;
        }
        
        //расшифровать
        private string Decode(string input, string keyword)
        {
            input = input.ToUpper();
            keyword = keyword.ToUpper();

            string result = "";

            int keyword_index = 0;

            foreach (char symbol in input)
            {
                if ((keyword_index) == keyword.Length)
                    keyword_index = 0;

                int p = (Array.IndexOf(characters, symbol) + N -
                    Array.IndexOf(characters, keyword[keyword_index])) % N;

                result += characters[p];

                keyword_index++;
            }

            return result;
        }

    }
}
