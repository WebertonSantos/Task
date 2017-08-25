using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

namespace Exemplo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Quando você realiza operações assincrona com as palavras chaves async e await, você pode manipular execusão da mesma maneira que você manipula Exceções em código sincrono.
        // que é usando blocos try/catch.
        /*
        No exemplo anterior, o manipulador de eventos Click para um botão chama o método de forma assíncrona WebClient.DownloadStringTaskAsync usando o operador esperam. A URL que é 
        fornecido é inválido, assim que o método lança uma exceção WebException. Mesmo que a operação é assíncrona, o controle retorna para o método btnThrowError_Click quando a operação 
        assíncrona for concluída ea exceção é tratado corretamente. Isso funciona porque nos bastidores, a Biblioteca paralela de tarefas é recuperar a exceção assíncrona e rethrowing-lo 
        no segmento interface do usuário.
        */
        private async void btnThrowError_Click(object sender, RoutedEventArgs e)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string data = await client.DownloadStringTaskAsync("http://fourthcoffee/bogus");
                }
                catch (WebException ex)
                {
                    lblResult.Content = ex.Message;
                }
            }
        }
    }
}
