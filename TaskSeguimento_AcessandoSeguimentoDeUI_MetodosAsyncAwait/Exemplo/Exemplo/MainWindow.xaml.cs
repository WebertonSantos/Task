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

        /*
        Na .NET Framework, cada segmento é associado a um objeto Dispatcher. O dispatcher é responsável por manter uma fila de itens de trabalho para o segmento. Quando você trabalha em vários 
        seguimentos, por exemplo, executando tarefas assíncronas, você pode usar o objeto Dispatcher para invocar a lógica em um segmento específico. Você geralmente precisa fazer isso quando 
        você usa operações assíncronas em aplicações gráficas. Por exemplo, se um usuário clicar em um botão em um aplicativo do Windows® Presentation Foundation (WPF), o manipulador de 
        eventos clique é executado no segmento interface do usuário. Se o manipulador de eventos começa uma tarefa assíncrona, essa tarefa é executado no segmento de segundo plano. Como 
        resultado, a lógica tarefa já não tem acesso aos controles na interface do usuário, porque estes são todos de propriedade da UI thread. Para atualizar a interface do usuário, a 
        lógica de tarefa deve usar o método Dispatcher.BeginInvoke de fila a lógica atualização sobre o segmento. 
        */
        private void btnBotao_Click(object sender, RoutedEventArgs e)
        {
            /*
            Se você fosse executar o código anterior, você teria uma exceção InvalidOperationException com a mensagem "O segmento de chamada não podem acessar este objeto porque um 
            segmento diferente o possui." Isso ocorre porque o método SetTime está sendo executado em uma seguimento de fundo, mas o lblTime rótulo foi criado pelo segmento UI. Para atualizar o 
            conteúdo do rótulo lblTime, você deve executar o método SetTime no segmento interface do usuário. Para fazer isso, você pode recuperar o objeto Dispatcher que está associado com 
            o objeto lblTime e depois chamar o método Dispatcher.BeginInvoke para invocar o método SetTime no segmento interface do usuário.
            */
            /*
            Task.Run(() => 
            {
                string currentTime = DateTime.Now.ToLongTimeString();
                SetTime(currentTime);
            });
            */

            // Para acessar o segmento da interface do usuário temos que fazer da seguinte forma
            Task.Run(() => 
            {
                string currentTime = DateTime.Now.ToLongTimeString();
                lblTime.Dispatcher.BeginInvoke(new Action(() => SetTime(currentTime)));
            });
        }

        private void SetTime(string time)
        {
            lblTime.Content = time;
        }
    }
}
