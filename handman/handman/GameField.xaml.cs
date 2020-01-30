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
using System.Windows.Shapes;

namespace handman
{
    /// <summary>
    /// Logika interakcji dla klasy GameField.xaml
    /// </summary>
    public partial class GameField : Window
    {
        
        public GameField()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private int wrongLetter = 0;
        private string haslo = "";
        private string kopiaHaslo = "";
        string komunikat;
        private string[] words;

        private void attempt(object sender, EventArgs e)
        {
            wrongLetter++;
            if (wrongLetter > 11)
            {
                Console.WriteLine('Przegrałeś!');
            }
        }
        private void loadwords()
        {
            char[] delimiterChars = { ',' };
            string[] readText = File.ReadAllLines('hasla.csv');
            words = new string[readText.Length];
            int index = 0;
            foreach (string s in readText)
            {
                string[] line = s.Split(delimiterChars);

                words[index++] = line[1];
            }
            int end = 0;
        }
        private void randomChoice()
        {
            wrongLetter = 0;
            int rollInx = (new Random()).Next(words.Length);
            haslo = words[rollInx];
            kopiaHaslo = "";
            for (int index = 0; index < kopiaHaslo.Length; index++)
            {
                kopiaHaslo += "_"
            }
            printCopy();

        }
        private void printCopy()
        {
            for (int index = 0; index < kopiaHaslo.Length; index++)
            {
                lblShowWord.Text += kopiaHaslo.Substring(index, 1) += "_";
                lblShowWord.Text += " ";
            }
        }
        private void overRideCody(char guess)
        {

        }

    }
}
}
