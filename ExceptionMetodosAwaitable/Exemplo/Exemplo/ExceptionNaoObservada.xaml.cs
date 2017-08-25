using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace Exemplo
{
    /// <summary>
    /// Interaction logic for ExceptionNaoObservada.xaml
    /// </summary>
    public partial class ExceptionNaoObservada : Window
    {
        public ExceptionNaoObservada()
        {
            InitializeComponent();
        }

        private void Exception_Click(object sender, RoutedEventArgs e)
        {
            // Subscribe to the TaskScheduler.UnobservedTaskException event and define an event handler.
            TaskScheduler.UnobservedTaskException += (Object senderr, UnobservedTaskExceptionEventArgs ee) =>
            {
                foreach (Exception ex in ((AggregateException)ee.Exception).InnerExceptions)
                {
                    MessageBox.Show($"An exception occured: {ex.Message}");
                }

                // Set the exception status to Observed.
                ee.SetObserved();
            };

            // Launch a task that will throw an unobserved exception
            // by attempting to download an item from an invalid URL.
            Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadStringTaskAsync("http://fourthcoffee/bogus");
                }
            });

            // Give the task time to complete and then trigger garbage collection (for example purposes only).
            Thread.Sleep(5000);
            GC.WaitForPendingFinalizers();
            GC.Collect();
            MessageBox.Show("Execution complete");

            /*
Se você usar um depurador para o passo através deste código, você verá que o evento UnobservedTaskException é disparado quando o GC é executado.
No .NET Framework 4.5, o tempo de execução .NET ignora exceções de tarefas não observadas por padrão e permite sua aplicação para continuar executando. Isto contrasta com o 
comportamento padrão no .NET Framework 4.0, onde o tempo de execução .NET iria encerrar todos os processos que lançam exceções de tarefas não observados. Você pode reverter 
para a abordagem de encerramento do processo, adicionando um elemento ThrowUnobservedTaskExceptions ao seu arquivo de configuração do aplicativo.
O exemplo de código a seguir mostra como adicionar um elemento ThrowUnobservedTaskExceptions para um arquivo de configuração do aplicativo.
	<configuration>
	  .....
	  <runtime>
		  <ThrowUnobservedTaskExceptions enabled="true"/>
	  </runtime>
	</configuration>
	
	
Se você definir ThrowUnobservedTaskExceptions para true, o tempo de execução .NET irá encerrar todos os processos que contêm exceções tarefa não observados. A melhor prática 
recomendada é definir esse sinalizador para true durante o desenvolvimento de aplicativos e para remover o sinalizador antes de liberar seu código. 
            */
        }
    }
}
