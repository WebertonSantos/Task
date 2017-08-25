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
            AguardandoTaskSimples();
            AguardandoMultiplasTasks();
            AguardandoQualquerUmaTaskDaLista();
            RetornandoValoresTask();
        }

        private static void AguardandoTaskSimples()
        {
            // Uma Task com processo demorado
            var task = Task.Run(() => 
            {
                Thread.Sleep(5000);
            });

            // Espera a Task finalizar a execusão
            task.Wait();
        }

        private static void AguardandoMultiplasTasks()
        {
            // Cria uma lista de Tasks 
            var tasks = new Task[3]
            {
                Task.Run(() => { Thread.Sleep(5000); }),
                Task.Run(() => { Thread.Sleep(10000); }),
                Task.Run(() => { Thread.Sleep(15000); })
            };

            // Aguarda até que todas as Taks finalize para depois continuar a execusão
            Task.WaitAll(tasks);
        }

        private static void AguardandoQualquerUmaTaskDaLista()
        {
            // Cria uma lista de Tasks 
            var tasks = new Task[3]
            {
                Task.Run(() => { Thread.Sleep(5000); }),
                Task.Run(() => { Thread.Sleep(10000); }),
                Task.Run(() => { Thread.Sleep(15000); })
            };

            // Aguarda até que uma das taks da lista finalize para depois continuar a execusão
            Task.WaitAny(tasks);
        }

        private static void RetornandoValoresTask()
        {
            // Ao instanciar um objeto da classe Genérica Task<TResult> usamos o parametro de tipo para especificar o tipo de resultado que a tarefa retornará. A classe Task<TResult> expõe 
            // uma propiedade somente leitura chamada Result. Apos a tarefa tenha sua execusão finalizada, você pode usar a propriedade Result para recuperar o valor de retorno da tarefa. 
            // A propriedade result é o mesmo tipo que tipo de parâmetro da tarefa.
            Task<string> task = Task.Run(() =>
            {
                Thread.Sleep(5000);
                return DateTime.Now.DayOfWeek.ToString();
            });

            // Se você acessar a propriedade result antes da tarefa tenha finalizado a execusão, seu código esperará ate o resultado esta diponivél para processamento.
            Console.WriteLine(task.Result);
        }
    }
}
