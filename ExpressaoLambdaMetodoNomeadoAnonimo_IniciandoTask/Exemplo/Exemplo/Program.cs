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
            MetodoNomeado();
            MetodoAnonimo();
            IniciandoTask();
        }

        private static void MetodoNomeado()
        {
            // Estamos usando uma expressão Lambda para chamar um método nomeado, passando um string 
            // por parâmetro, observe que não preciso converter para delegate a expressão faz isso por si só.
            Task task = new Task(() => Falar("Weberton"));
            task.Start();
            Console.WriteLine("Escreva algo");

            Console.ReadKey();
        }

        private static void Falar(string nome)
        {
            Console.WriteLine($"Bom dia {nome}");
        }

        private static void MetodoAnonimo()
        {
            // Usando método anonimo, observe que não foi necessário informa o delegate, a expressão já faz isso
            Task task = new Task(() =>
            {
                Console.WriteLine("Bom dia Weberton");
            });

            task.Start();

            Console.ReadKey();
        }

        private static void IniciandoTask()
        {
            // A Library Parallel Task atribui um seguimento para a sua tarefa quando ela for iniciada, 
            // e este seguimento é separado para que não seja necessário esperar a sua execusão.
            // Usamos o método Start da Classe Task para colocar mais uma tarefa na Fila de Execusão.
            var task1 = new Task(() => { Console.WriteLine("Colocando a task na fila de execusão com Start"); });
            task1.Start();


            // Ao invés de instanciar um objeto da Classe Task podemos utilizar a Classe Estática 
            // TaskFactory que é exposta através da propriedade Factory da Classe Task. Com isso criamos e 
            // colocar na fila uma nova tarefa sem precisar de uma instancia, observe que não usamos a 
            // palavra new para criar a task.
            // O método Task.Factory.StartNew é altamente configurável e aceita uma ampla gama de parâmetro
            var task2 = Task.Factory.StartNew(() => { Console.WriteLine("Criando e já colocando a task na fila de execusão usando Factory"); });


            // Caso precisamos apenas enfileira um bloco de função com as opção de agendamento padrão
            // podemos usar o método estático Task.Run como um atalho para o método Task.Factory.StartNew.
            var task3 = Task.Run(() => { Console.WriteLine("Iniciando Task com Run"); });
        }
    }
}
