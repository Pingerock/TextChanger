using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Media;

namespace Text_Schizophrenia
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string str_file_name;
        string word_for_change;
        int countdown;
        string[] words;

        //Browse a text file
        public void FileDialog_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text|*.txt|All|*.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                str_file_name = openFileDialog.FileName;
                textBox1.Text = str_file_name;
            }
        }

        //Fills an array of strings with words from chosen text file before
        public void split_text()
        {
            string text = "";
            using (StreamReader fs = new StreamReader(str_file_name))
            {
                while (true)
                {
                    string temp = fs.ReadLine();
                    if (temp == null)
                    {
                        break;
                    }
                    text += temp;
                }
            }
            words = text.Split(' ');
        }

        //Choose a random word from the words array and replaces them with a replacement word
        //until the countodwn reaches zero
        public void Mix_it_up(int countdown, string word_for_change) {
            Random rnd = new Random();
            for (int x = countdown; x >= 0; x--)
            {
                int pointer;
                do
                {
                    pointer = rnd.Next(1, words.Length - 2);
                    if ((words[pointer] != word_for_change) &&
                        (words[pointer-1] != word_for_change) &&
                        (words[pointer+1] != word_for_change))
                    {
                        break;
                    }
                } while (words[pointer] != word_for_change);
                words[pointer] = word_for_change;
            }
        }

        //Creates a text file with replaced words and saves it in the desktop folder
        public void Build_a_text_file(string[] words, int countdown)
        {
            string fileName = Path.GetFileName(str_file_name);
            fileName = fileName.Remove(fileName.Length - 4);
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fileName + "_Changed.txt";
            FileInfo result_text_file = new FileInfo(filePath);
            if (!result_text_file.Exists)
            {
                using (StreamWriter fs = result_text_file.CreateText())
                {
                    for(int x = 0; x < words.Length; x++)
                    {
                        fs.Write(words[x] + " ");
                    }
                }
            } else
            {
                result_text_file.Delete();
                using (StreamWriter fs = result_text_file.CreateText())
                {
                    for (int x = 0; x < words.Length; x++)
                    {
                        fs.Write(words[x] + " ");
                    }
                }
            }
        }

        //Writes silly messages in the richTextBox while changing the text
        public void Start_button_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText("\nCollecting data.\n");
            richTextBox1.Refresh();
            countdown = Convert.ToInt32(Math.Round(numericUpDown1.Value, 0));
            if (countdown == 0)
            {
                MessageBox.Show("The number of replacements is zero.\n");
                this.Close();
            }
            //These Sleep methods are just for a joke so people could read all messages in the richTextBox
            Thread.Sleep(2000);
            richTextBox1.Refresh();
            split_text();
            richTextBox1.AppendText("Preparing data for processing.\n");
            Thread.Sleep(1000);
            richTextBox1.Refresh();
            if (words.Length < countdown) 
            {
                MessageBox.Show("The number of replacements cannot be greater than\nor equal to the number of words in the file.");
                this.Close();
            }
            word_for_change = textBox2.Text;
            Mix_it_up(countdown, word_for_change);
            richTextBox1.AppendText("Changing words.\n");
            Thread.Sleep(2000);
            richTextBox1.Refresh();
            Build_a_text_file(words, countdown);
            richTextBox1.AppendText("Collecting the text in a new file.\n");
            Thread.Sleep(2000);
            richTextBox1.Refresh();
            SystemSounds.Exclamation.Play();
            richTextBox1.AppendText("All done! :)");
        }

        private void About_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Text changer \n" +
                            "\n" +
                            "Replaces random words in the text with the word you enter\n" + 
                            "and displays them in a separate text file in the desktop folder.\n" +
                            "\n" +
                            "Made by Pingerock. \n" +
                            "2021", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
