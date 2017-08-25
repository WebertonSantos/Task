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
    /// Interaction logic for AssyncAwait.xaml
    /// </summary>
    public partial class AssyncAwait : Window
    {
        public AssyncAwait()
        {
            InitializeComponent();
        }

        /*
        Neste exemplo, a instrução final no bloco de manipulação de evento ficará bloqueada até que o resultado da tarefa esteja disponivel. Isso significa que interface com o usuário 
        congelará efetivamento, e o usuário não será capaz de redimensionar a janela, minimizar a janela, e assim por diante. Para ativar a interface do usuário 
        a permanecer ágil, você pode converter o manipulador de eventos em um método assíncrono.
        */
        /*
        private void btnLongOperation_Click(object sender, RoutedEventArgs e)
        {
            // Neste exemplo a interface do usário será congelada até que o seguimento seja finalizado
            lblResult.Content = "Commencing long-running operation";
            Task<string> task = Task.Run<string>(() =>
            {
                // isso representa uma operação de longa execusão
                Thread.Sleep(10000);
                return "Operation complete";
            });

            // Este bloco de instrução aguarda até que o resultado da tarefa esteja disponível
            lblResult.Content = task.Result;
        }
        */


        /*
        Este exemplo inclue duas mudanças principais em relação ao exemplo anterior.	
        • A declaração de método inclue agora a palavra chave async.
        • A declaração do bloco foi substituida por um operador await.

        Observe que quando você usa o operador await, você não aguarda o resultado da tarefa você aguarda a tarefa em si. Quando o .NET em tempo de execusão executa um método assincrono,
        ele efetivamente ignora a declaração await ate o resultado da tarefa estar disponivel. O metodo retorna e o seguimento esta livre para fazer outro trabalho. Quando o resultado 
        da tarefa se torna disponivel, o tempo de execusão retorna para o método e execulta a declaração aguarde.
        */
        private async void btnLongOperation_Click(object sender, RoutedEventArgs e)
        {
            // Neste exemplo a interface do usário será congelada até que o seguimento seja finalizado
            lblResult.Content = "Commencing long-running operation";
            Task<string> task = Task.Run<string>(() =>
            {
                // isso representa uma operação de longa execusão
                Thread.Sleep(10000);
                return "Operation complete";
            });

            // Este bloco de instrução aguarda até que o resultado da tarefa esteja disponível
            // Entretanto, o método completa e o seguimento é livre para fazer outro trabalho.
            lblResult.Content = await task;
        }
    }
}
