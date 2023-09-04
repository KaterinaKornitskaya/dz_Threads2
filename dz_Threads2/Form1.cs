using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace dz_Threads2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = "Введите путь к файлу";
            label2.Text = "Введите искомое слово";
        }

        struct MyStruct  // структура для передачи в делегат с параметром для потока
        {
            public string filePath;       // путь к файлу
            public string wordForSearch;  // искомое слово
            public MyStruct(string f, string w)
            {
                filePath = f;
                wordForSearch = w;
            }
        }

        public void SearchWord(object obj)
        {
            MyStruct myStruct = (MyStruct)obj;

            string filePath = myStruct.filePath;
            string wordForSearch = myStruct.wordForSearch;

            int count = 0;
            StringBuilder sb = new StringBuilder();  // создаем строку
            using (StreamReader reader = new StreamReader(filePath))  // считываем файл
            {
                // присоединяем к созданной строке текст из файла
                sb.Append(reader.ReadToEnd());
                // текст из файла с пимощью метода Split преобразуем в массив слов
                string[] s = sb.ToString().Split(new char[] { ' ', '.', ',', '!', '?', ':', ';', '\n'});
                foreach (var item in s)
                {
                    if (item == wordForSearch)
                        count++;
                }
            }
            labelResult.Text = count.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // создаем и инициализируем объект структуры
            MyStruct myStruct = new MyStruct(textBoxPath.Text, textBoxWord.Text);
            // создаем поток, передаем делегат, инициализированный методом SearchWord
            Thread thread = new Thread(new ParameterizedThreadStart(SearchWord));
            thread.IsBackground = true;
            // запускаем поток и в параметры передаем объект структуры
            thread.Start(myStruct);
        }
    }
}
