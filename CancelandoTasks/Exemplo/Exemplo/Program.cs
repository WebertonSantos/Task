using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exemplo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Esta abordagem de cancelamento é util quando não precisamos saber se o método foi
            // execultado por completo.
            //CancelarTaskSemException();

            CancelarTaskComException();
        }

        #region .: ConcelarTaskSemException :.

        private static void CancelarTaskSemException()
        {
            // Cria uma fonte de chave de cancelamento e obtem uma chave de cancelamento
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            // Cria e inicia uma Task
            var task = Task.Run(() => DoWork(ct), ct);

            Thread.Sleep(5000);

            // quando fazemos isso ele cancela todas as tasks que possui o token gerado por este Source
            cts.Cancel();

            task.Wait();

            // Cada Tarefa expõe uma propriedade de estado que podemos usar para saber se ela foi executada ate a sua conclusão. Porém o status retornado é igual para quando é solicitado o seu
            // cancelamento conforme mostrado anteriormente ou quando a tarefa executou por completo, sendo assim esta abordagem de cancelamento é util quando não precisamos saber se o método foi
            // execultado por completo.
            if (task.IsCompleted)
            {
                Console.WriteLine("Task executada por completo.");
            }
        }

        private static void DoWork(CancellationToken token)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(2000);

                if (token.IsCancellationRequested)
                {
                    // Organizar e finalizar
                    // ....
                    return;
                }

                // Se a tarefa não tenha sido cancelada, continua executando normalmente.
                // ...
            }
        }
        #endregion

        #region .: CancelarTaskComException :.

        private static void CancelarTaskComException()
        {
            // Cria uma fonte de chave de cancelamento e obtem uma chave de cancelamento
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            // Cria e inicia uma Task
            var task = Task.Run(() => DoWorkException(ct), ct);

            Thread.Sleep(5000);

            // quando fazemos isso ele cancela todas as tasks que possui o token gerado por este Source
            cts.Cancel();

            try
            {
                task.Wait();
            }

            // A task utiliza a Exception AggregateException que contem uma lista de Exception, é realizado
            // desta forma pois a Task pode lançar mais de uma exception
            catch (AggregateException ae)
            {
                // Percorre a lista de exception que possa ter sido lançada pela task
                foreach (var inner in ae.InnerExceptions)
                {
                    // Verifica se a exception foi um lançada é de cancelamento
                    if (inner is TaskCanceledException)
                    {
                        Console.WriteLine("The task was cancelled");
                        Console.ReadLine();
                    }
                    else
                    {
                        // Se for algum outro tipo de Exception, relança a excessão
                        throw;
                    }
                }
            }

            // Cada Tarefa expõe uma propriedade de estado que podemos usar para saber se ela foi executada ate a sua conclusão.
            if (task.IsCompleted)
            {
                Console.WriteLine("Task executada por completo.");
            }
        }

        private static void DoWorkException(CancellationToken token)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(2000);

                // Lança uma OperationCanceledException se o cancelamento foi requisitado.
                token.ThrowIfCancellationRequested();

                // Se a tarefa não tenha sido cancelada, continua executando normalmente.
                // ...
            }
        }

        #endregion
    }
}
