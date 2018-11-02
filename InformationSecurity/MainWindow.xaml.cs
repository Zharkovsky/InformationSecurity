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
        private int[] rowPermutation;
        private int[] colPermutation;
        private int row, col;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckKeys(Input.Text, out row, out col)) return;
            string[,] matr = new string[row, col];
            rowPermutation = new int[row];
            colPermutation = new int[col];
            int index = 0;
            RowKey.Text.Split(' ').ToList().ForEach(_ => rowPermutation[index++] = Convert.ToInt32(_));
            index = 0;
            ColumnKey.Text.Split(' ').ToList().ForEach(_ => colPermutation[index++] = Convert.ToInt32(_));
            var result = Encode(Input.Text, matr);
            index = 1;
            Output.Text = "";
            if (!string.IsNullOrWhiteSpace(Directory.Text))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(Directory.Text);
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
            }
            result.ToList().ForEach(_ =>
            {
                Output.Text += $"Блок {index}: {_} \n";
                if(!string.IsNullOrWhiteSpace(Directory.Text))
                {
                    var array = Encoding.UTF8.GetBytes(_);
                    using (var file = File.Create(Path.Combine(Directory.Text, index + ".txt")))
                    {
                        file.Write(array, 0, array.Length);
                    }
                }
                index++;
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!CheckKeys(Input.Text, out row, out col)) return;
            string[,] matr = new string[row, col];
            rowPermutation = new int[row];
            colPermutation = new int[col];
            int index = 0;
            RowKey.Text.Split(' ').ToList().ForEach(_ => rowPermutation[index++] = Convert.ToInt32(_));
            index = 0;
            ColumnKey.Text.Split(' ').ToList().ForEach(_ => colPermutation[index++] = Convert.ToInt32(_));
            Input.Text = Decode(Input.Text, matr);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Input.Text = Encoding.UTF8.GetString(File.ReadAllBytes(openFileDialog.FileName));
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var fbd = new WPFFolderBrowserDialog();
            if ((fbd.ShowDialog() ?? false) && !string.IsNullOrWhiteSpace(fbd.FileName))
            {
                Directory.Text = fbd.FileName;
            }
        }

        private bool CheckKeys(string inputString, out int row, out int col)
        {
            bool result = true;
            var set = new HashSet<int>();
            try
            {
                RowKey.Text.Split(' ').ToList().ForEach(_ =>
                {
                    set.Add(Convert.ToInt32(_));
                });
                int rows = set.Count;
                set.ToList().ForEach(_ => result = result && 0 < _ && _ <= rows);
                set.Clear();
                ColumnKey.Text.Split(' ').ToList().ForEach(_ =>
                {
                    if (set.Contains(Convert.ToInt32(_))) result = false;
                    set.Add(Convert.ToInt32(_));
                });
                int cols = set.Count;
                set.ToList().ForEach(_ => result = result && 0 < _ && _ <= cols);

                row = RowKey.Text.Split(' ').Length;
                col = cols;

                result = result && (rows * cols == Convert.ToInt32(NumBlocks.Text) && row * col >= inputString.Length);
            }
            catch (Exception ex)
            {
                row = col = 0;
                result = false;
            }
            if (!result)
            {
                Error.Text = "Ошибка ключа. Проверьте ключ";
                Output.Text = "";
            }
            else
                Error.Text = "";
            return result;
        }

        private string[] Encode(string str, string[,] matr)
        {
            var codeStr = new string[Convert.ToInt32(NumBlocks.Text)];
            int strIter = 0, K;

            //write
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    if (strIter < str.Length)
                    { //если еще не всю строку переписали
                        matr[r, c] = (str[strIter]).ToString();
                        strIter++;
                    }
                    else
                    {
                        matr[r, c] = " ";
                    }
                }
            }

            //read
            for (int c = 0; c < col; c++)
            {
                for (int r = 0; r < row; r++)
                {
                    K = col * (rowPermutation[r] - 1) + colPermutation[c];
                    codeStr[K - 1] += matr[r, c];
                }
            }

            return codeStr;
        }

        

        private string Decode(string str, string[,] matr)
        {
            string decodeStr = "";
            var codeStr = new string[Convert.ToInt32(NumBlocks.Text)];
            int i = 0, strIter = 0, K;

            var directory = Directory.Text;
            if (string.IsNullOrWhiteSpace(directory)) return null;

            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                codeStr[i++] = File.ReadAllText(file.FullName);
            }
            

            //read
            for (int c = 0; c < col; c++)
            {
                for (int r = 0; r < row; r++)
                {
                    K = col * (rowPermutation[r] - 1) + colPermutation[c];
                    matr[r, c] = codeStr[K - 1].FirstOrDefault().ToString();
                    if(!string.IsNullOrWhiteSpace(codeStr[K-1]))
                        codeStr[K - 1] = codeStr[K - 1].Remove(0, 1);
                }
            }

            //write
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                   decodeStr += matr[r, c];
                }
            }

            return decodeStr;
        }

    }
}
