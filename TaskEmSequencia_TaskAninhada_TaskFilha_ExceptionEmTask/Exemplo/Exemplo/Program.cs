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
            TaskAninhadaComDependencia();
        }

        #region .: TaskEmSequencia :.

        /*
        Pode ser necessário colocar uma tarefa para ser execultada assim que outra tarefa termine (Continuação) ou colocar uma tarefa para ser executada quando uma outra tarefa resulte 
        em falha, necessitando realizar algum tipo de recuperação. Com esta abordagem podemos construir um encanamento de operação em segundo plano.
        Podemos encademais varias tarefas, podemos também passar dados da tarefa Pai para a filha e da filha para suas filhas.
        */
        private static void TaskEmSequencia()
        {
            // Cria uma tarefa que retorna uma string 
            Task<string> firsTask = new Task<string>(() => "Hello");

            // Cria uma tarefa de continuação.
            // O delegado assume o resultado da tarefa antecedente como uma argumento.
            Task<string> secondTask = firsTask.ContinueWith((antecendent) =>
            {
                return string.Format("{0}, world!", antecendent.Result);
            });

            // inicia a tarefa antecedent
            firsTask.Start();

            // usa o resultado da tarefa de continuação
            // quando eu uso o retorno da tarefa o sistema pera ate a tarefa ser finalizada, para disponibilizar o resoltado
            Console.WriteLine(secondTask.Result);
        }

        #endregion

        #region .: TaskAninhadaSemDependencia :.

        /*
        Temos também a possibilidade de encadear tarefas dentro de tarefa (Chamado de Tarefas Pai e Filhas), 
        como neste exemplo não foi definido Dependencia a tarefa pai vai terminar e a tarefa filha vai continuar 
        executando de forma assincrona.
        */
        private static void TaskAninhadaSemDependencia()
        {
            var outer = Task.Run(() =>
            {
                Console.WriteLine("Outer task starting");
                var inner = Task.Run(() =>
                {
                    Console.WriteLine("Nested task starting");
                    Thread.SpinWait(500000);
                    Console.WriteLine("Nested task completing");
                });
            });

            outer.Wait();
            Console.WriteLine("Outer task completing");
            /*
             * Outer task starting
             * Outer task completing
             * Nested task starting
             * Nested task Completing
             * NESTE CASO, DEVIDO AO ATRASO NA TAREFA FILHA, A TAREFA PAI CONCLUIU ANTES DA TAREFA FILHA.
            */
        }

        #endregion

        #region .: TaskAninhadaComDependencia :.

        /*
        Neste exemplo criamos uma dependencia entre a tarefa filha e a tarefa Pai, informando que a tarefa pai só vai concluir quando a tarefa filha finalizar.
        Se observarmos este exemplo é muito parecido com o anterior, a diferença é que definimos a opção AttachedToParent para fazer com que a tarefa pai espere a tarefa filha para 
        poder finalizar.
        */
        private static void TaskAninhadaComDependencia()
        {
            var parent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Parent task starting");
                var child = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Child task starting");
                    Thread.Sleep(5000);
                    Console.WriteLine("Child task completing");
                }, TaskCreationOptions.AttachedToParent);

                var child2 = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Child2 task starting");
                    Thread.Sleep(10000);
                    Console.WriteLine("Child2 task completing");
                });
            });
            parent.Wait();
            Console.WriteLine("Parent task completing");

            /*
             * Parent task starting
             * Child2 task starting
             * Child task starting
             * Child task completing
             * Parent task completing
             * Child2 task completing
             * COMO NESTE CASO ESTAMOS DEFININDO DEPENDENCIA A TAREFA PAI NÃO VAI TERMINAR ENQUANTO A TAREFA FILHA
             * NÃO FINALIZAR.
             * OBSERVE QUE A CHILD2 NÃO TEM DEPENDENCIA, COM ISSO ELE SÓ VAI ESPERAR PELA CHILD QUE POSSUI DEPENDENCIA
            */
        }

        #endregion

        #region .: AgrupandoTratandoExceptions :.

        /*
        Quando uma tarefa lança uma exceção, a exceção é propagada até o segmento que iniciou a tarefa. Como podemos ligar tarefas atraves de Continuação ou Tarefas Pais e Filhas, uma 
        tarefa inicial pode conter diversas exceções. Para garantir que todas as exceções lançadas seja propagada para o inicio a Library Parallel Task agrupa todas as Exceções em um 
        objeto AggregateException. Este objeto expõe todas as exceções que ocorreram por meio de uma coleção InnerExceptions.

        Para que possamos capturar as exceções lançadas pelas tarefas, temos que esperar que a tarefa seja concluida, fazemos isso utilizando o método Task.Wait como já visto.
        Este metódo tem que estar cinculado pela expressão try catch e então capturar uma exceção AggregateException no bloco catch correspondente.
        */
        private static void AgrupandoTratandoExceptions()
        {
            // cria uma fonte chave de cancelamento e obtem a chave de cancelamento
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            // cria e e inicia uma tafera de longa execusão
            var task11 = Task.Run(() => doWork(ct), ct);

            // cancela a tarefa
            cts.Cancel();

            // manipula a TaskCanceledException
            try
            {
                task11.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var inner in ae.InnerExceptions)
                {
                    if (inner is TaskCanceledException)
                    {
                        Console.WriteLine("The task was cancelled");
                        Console.ReadLine();
                    }
                    else
                    {
                        // Se for algum outro tipo de Exception, relança a excecão
                        throw;
                    }

                }
            }
        }

        // Método Executa a tarefa
        private static void doWork(CancellationToken token)
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.SpinWait(500000);
                // Lança um OperationCanceledException se o cancelamento foi solicitado.
                token.ThrowIfCancellationRequested();
            }

            // Se a tarefa não tenha sido cancelada, continua executando normalmente.
            // ...
        }

        #endregion
    }
}
