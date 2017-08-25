using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace Exemplo
{
    /// <summary>
    /// Interaction logic for AsyncAwaitMetodo.xaml
    /// </summary>
    public partial class AsyncAwaitMetodo : Window
    {
        public AsyncAwaitMetodo()
        {
            InitializeComponent();
        }

        /*
        O operador await é sempre usado para aguardar a conclusão de uma instância de tarefas de uma maneira não bloqueante. Se você deseja criar um método assíncrono que você pode
        esperar com o operador await, o método deve retornar um objeto Task. Quando você converte um método síncrona para um método assíncrono, use as seguintes diretrizes:
        • Se o seu método síncrono retorna void (em outras palavras, ele não retorna um valor), o método assíncrono deve retornar um objeto Task.
        • Se o seu método síncrono tem um tipo de retorno de TResult, seu método assíncrono deve retornar um Task <TResult> objeto.

        Um método assíncrono pode retornar void, no entanto, este é normalmente usado apenas para manipuladores de eventos. Sempre que possível, você deve retornar um objeto de tarefas 
        para que os chamadores possam usar o operador await com seu método.
        */

        /*
        Método sem usar o Async e Await
        */
        /*
        private void btnLongOperation_Click(object sender, RoutedEventArgs e)
        {
            lblResult.Content = GetData();
        }

        private string GetData()
        {
            var task = Task.Run<string>(() =>
            {
                // Simula um tarefa de longa execusão
                Thread.Sleep(10000);
                return "Operation complete";
            });
            return task.Result;
        }
        */

        /*
        Para converter em um método assincrono aguardavel, você deve:
        • Adicionar o modificador async para a declaração do método.
        • mudar o tipo de retorno de string para Task<string>.
        • modificar a logica do método para usar o operador await com alguma operação de longe execusão.

        Note que você pode usar a palavra chave await somente em um método assincrono.
        */
        private async void btnLongOperation_Click(object sender, RoutedEventArgs e)
        {
            lblResult.Content = await GetData();
        }

        /*
        O método GetData retorna uma tarefa, então você pode usar o método como o operador await. por exemplo, você pode chamar o método em um manipular de evento para o evento click do 
        botão e usar o resultado para setar o valor de um label chamado lblResult.
        */
        private async Task<string> GetData()
        {
            var task = await Task.Run<string>(() =>
            {
                // Simula um tarefa de longa execusão
                Thread.Sleep(10000);
                return "Operation complete";
            });
            return task;
        }
    }
}
