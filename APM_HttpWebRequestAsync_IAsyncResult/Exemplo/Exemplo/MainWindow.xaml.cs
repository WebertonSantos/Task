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

        private async void btnCheckUrl_Click(object sender, RoutedEventArgs e)
        {
            // pega a url informada pelo usuário
            string url = TxtUrl.Text;

            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    // cria uma requisição HTTP
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                    // Envia a requisição e espera uma resposta
                    HttpWebResponse response =
                        await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request) as HttpWebResponse;

                    // mostra o código de status da resposta.
                    LblResult.Content = string.Format("The URL retornou o seguinte código de status {0}", response.StatusCode);
                }
                catch (Exception ex)
                {
                    LblResult.Content = ex.Message;
                }
            }
            else
            {
                LblResult.Content = string.Empty;
            }            
        }
    }
}
