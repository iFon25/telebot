using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleBot
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static ITelegramBotClient botClient;
        static TextBox console;
        private static string token;

        public MainWindow()
        {
            InitializeComponent();
            console = ConsoleBox;
            token = Properties.Settings.Default.Token;
            if (token == "")
            {
                console.Text += $"No token for bot. Open settings and enter your token";
            }
            else
            {
                StartBot();
            }
        }

        private void StartBot()
        {
            if (token == "") Properties.Settings.Default.Reload();
            if (botClient == null) StartClient(token);
            if (botClient != null)
            {
                var me = botClient.GetMeAsync().Result;
                console.Text +=
                    $"Hello, World! I am user {me.Id} and my name is {me.FirstName}.\n";

                botClient.OnMessage += Bot_OnMessage;
                botClient.StartReceiving();
                //Thread.Sleep(int.MaxValue);
            }
        }

        private  async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
               Dispatcher.Invoke(() => {
                   console.Text += $"Received a text message in chat {e.Message.Chat.Id}. Time: {e.Message.Date}\n" +
                                   $"UserName: @{e.Message.From.Username}\n" +
                                   $"Message: {e.Message.Text}";
               });
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "You said:\n" + e.Message.Text
                );
            }
        }

        private void StartClient(string key)
        {
            try
            {
                botClient = new TelegramBotClient(key, GetProxy("212.66.34.240", 39800));
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => {
                    console.Text += $"\nError:\n" + ex.Message;
                });
            }
        }

        private WebProxy GetProxy(string ip, int port)
        {
            return new WebProxy(ip, port);
        }

        private void StartBotClick(object sender, RoutedEventArgs e)
        {
            StartBot();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            var winSettings = new Windows.SettingsWindow();
            winSettings.Show();
        }
    }
}
