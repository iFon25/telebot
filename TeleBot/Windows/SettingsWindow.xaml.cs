using System.Windows;
using TeleBot.Properties;

namespace TeleBot.Windows
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private static string Token = Settings.Default.Token;
        public SettingsWindow()
        {
            InitializeComponent();
            if (Token != "")
            {
                TokenBox.Text = Token;
            }

        }

        private void BtnSaveToken_Click(object sender, RoutedEventArgs e)
        {
            Token = TokenBox.Text;
            Settings.Default.Save();
        }
    }
}
