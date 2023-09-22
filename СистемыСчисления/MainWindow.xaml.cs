using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace СистемыСчисления
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       string[] Symbol = { "a", "b", "c", "d", "e", "f" }; // массив для обозначения систем больше десятичной
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Request_TextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^a-f0-9]");
            e.Handled = reg.IsMatch(e.Text);
        }

        //методы для переключения значений систем //
        void SwitchMethodMinus(Label SwitchMethodLabel)
        {
            _ = Convert.ToInt32(SwitchMethodLabel.Content) > 2 ?
                SwitchMethodLabel.Content = Convert.ToInt32(SwitchMethodLabel.Content) - 1 :
                SwitchMethodLabel.Content = 2;
        }
        void SwitchMethodPlus(Label SwitchMethodLabel)
        {
            _ = Convert.ToInt32(SwitchMethodLabel.Content) < 16 ?
                SwitchMethodLabel.Content = Convert.ToInt32(SwitchMethodLabel.Content) + 1 :
                SwitchMethodLabel.Content = 16;
        }

        // переключение значений систем //
        private void LabelSwitch_Click(object sender, MouseButtonEventArgs e)
        {
            List<Label> controls = new List<Label>{ SwitchNumber0, SwitchNumber1, SwitchNumber2, SwitchNumber3 };
            int Index = controls.IndexOf((Label)sender);
            switch (Index)
            {
                case 0:
                    Label SwitchMethodLabel = System1;
                    SwitchMethodMinus(SwitchMethodLabel);
                    break;
                case 1:
                    SwitchMethodLabel = System1;
                    SwitchMethodPlus(SwitchMethodLabel);
                    break;
                case 2:
                    SwitchMethodLabel = System2;
                    SwitchMethodMinus(SwitchMethodLabel);
                    break;
                case 3:
                    SwitchMethodLabel = System2;
                    SwitchMethodPlus(SwitchMethodLabel);
                    break;
            }
        }

        private void Request_TextChange(object sender, TextChangedEventArgs e)
        {
            int Sys1_int = Convert.ToInt32(System1.Content);
            int Sys2_int = Convert.ToInt32(System2.Content);

            bool StopResult = false; // булевая, отвечающая за выдачу / не выдачу результата
            
            bool NotSymbol = true; // булевая для проверки наличия символа в запросе
            int[] requestInt = new int[request.Text.Length];

            for (int i = 0; i < requestInt.Length; i++) // цикл для получения значения символа / числа
            {
                for (int j = 0; j < Symbol.Length; j++)
                {
                    if (request.Text[i].ToString() == Symbol[j]) 
                    {
                        requestInt[i] = 11 + j;
                        NotSymbol = false;
                    }
                }

                if (NotSymbol)
                    requestInt[i] = request.Text[i] - 47;
            }

            for (int i = 0; i < requestInt.Length; i++) // цикл для проверки на допустимость числа в системе
            {
                if (requestInt[i] > Sys1_int)
                {
                    WarningText.Content = "Символ не может быть больше системы, которую хотят перевести!";
                    StopResult = true;
                }
                else
                {
                    result.Content = "";
                    WarningText.Content = "";
                } 
            } 

            // перевод из системы в систему //

            if (Sys1_int >= Sys2_int)
                SystemsTranslation(StopResult, Sys1_int, Sys2_int);
            else if (Sys1_int <= Sys2_int)
                SystemsTranslation(StopResult, Sys1_int, Sys2_int);
        }

        void SystemsTranslation(bool StopResult, int Sys1_int, int Sys2_int)
        {
            if (StopResult == false)
            {
                int res = 0; // переменная промежуточного результата
                string resEnd = ""; // переменная финального результата

                bool NotSymbol = true; // булевая для проверки наличия символа в запросе
                int[] requestInt = new int[request.Text.Length];
                for (int i = 0; i < requestInt.Length; i++) // цикл разбиения на символы и запись промежуточного результата
                {
                    for (int j = 0; j < Symbol.Length; j++)
                    {
                        if (request.Text[i].ToString() == Symbol[j]) // работа с переводом символов
                        {
                            NotSymbol = false;
                            requestInt[i] = 10 + j;
                        }
                    }

                    if (NotSymbol) // работа с переводом без символов
                    {
                        requestInt[i] = request.Text[i] - 48;
                        res += requestInt[i] * Convert.ToInt32(Math.Pow(Sys1_int, requestInt.Length - (i + 1)));
                    }
                    else
                    {
                        res += requestInt[i] * Convert.ToInt32(Math.Pow(Sys1_int, requestInt.Length - (i + 1)));
                        NotSymbol = true;
                    }
                }

                while (res != 0) //цикл формирования конечного результата
                {
                    if (Convert.ToInt32(res % Sys2_int) > 9)
                    {
                        int ost = res % Sys2_int;
                        resEnd += Symbol[ost - 10];
                        res /= Sys2_int;
                    }
                    else
                    {
                        resEnd += (res % Sys2_int).ToString();
                        res /= Sys2_int;
                    }
                }

                char[] resReverse = resEnd.ToCharArray(); // перевертывание финального результата
                resEnd = "";
                Array.Reverse(resReverse);
                for (int i = 0; i < resReverse.Length; i++)
                    resEnd += resReverse[i];

                result.Content = "Ответ:" + resEnd;
            }
            else
                result.Content = "Ответ: невозможно подсчитать!";
        }

    }
}