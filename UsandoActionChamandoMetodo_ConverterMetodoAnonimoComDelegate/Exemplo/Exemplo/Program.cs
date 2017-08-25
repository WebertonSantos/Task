using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemplo
{
    class Program
    {
        static void Main(string[] args)
        {
            UsandoAction();
            UsandoDelegate();
        }

        private static void UsandoAction()
        {
            // Utilizando a classe Action, convertemos um método em um delegate, porém não é 
            // recomendado se este método não seré executado por mais de um processo.
            // Você precisa de um método para usar Action Delegate, Verificar se realmente precisa de um
            // método, se for usar o código apenas uma vez é preferivel usar o método Anonimo
            // Para Usar Action o método não pode ter parâmetros e não pode retornar valores
            Task task = new Task(new Action(Falar));

            // É necessário mandar a task iniciar.
            task.Start();

            Console.ReadKey();
        }

        private static void Falar()
        {
            Console.WriteLine($"The time now is {DateTime.Now}");
        }

        private static void UsandoDelegate()
        {
            // Criando um delegate anonimo, neste caso não criamos um método para a tarefa, simplismente,
            // atribuimos as responsabilidades dentro de um delegate anonimo, para isso precisamos usar 
            // a palavra reservada "delegate"
            Task task = new Task(delegate
            {
                Console.WriteLine($"The time now is {DateTime.Now}");
            });

            // É necessário mandar a task iniciar.
            task.Start();

            Console.ReadKey();
        }
    }
}
