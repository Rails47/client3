using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UdpClient client;
        const int PORT = 1234;

        public MainWindow()
        {
            InitializeComponent();
            client = new UdpClient();
            client.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));
            Listen();
        }

        private void Listen()
        {
            Thread listenThread = new Thread(() =>
            {
                while (true)
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, PORT);
                    byte[] data = client.Receive(ref remoteEP);
                    string message = Encoding.UTF8.GetString(data);
                    UpdateUI(message);
                }
            });
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void UpdateUI(string message)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                currentTimeTextBlock.Text = message;
            }), DispatcherPriority.Background);
        }
    }
}
