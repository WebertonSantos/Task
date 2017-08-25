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
            ParallelFor();
        }

        #region .: ParallelInvoke :.

        // O Método Parallel.Invoke nós possibilita executar um conjunto fixo de tarefas em paralelo. Usamos Expressões lambda para invocar. Não precisa criar as tarefas explicitamente,
        // elas são criadas implicitamente através dos delegates fornecidos para o método Parallel.Invoke
        private static void ParallelInvoke()
        {
            Parallel.Invoke(() => MethodForFirstTask(),
                            () => MethodForSecondTask(),
                            () => MethodForThirdTask());
        }

        private static void MethodForFirstTask() { }

        private static void MethodForSecondTask() { }

        private static void MethodForThirdTask() { }

        #endregion

        #region .: ParallelFor :.

        // A classe Parallel também fornece métodos que você pode usar para executar loop "for" e "foreach" iterações em paralelo, Se for necessário comparar valores sequenciais não
        // podemos usar esta abordagem, pois a interação é executada em seguimentos separados.Devemos usar apenas quando cada interação representa uma operação independente, aonde uma
        // interação não depende da outra.Executando a interação em paralelo aproveitamos melhor o poder do multi processamento.
        private static void ParallelFor()
        {
            int from = 0;
            int to = 500000;
            int capacity = 500000;
            double[] array = new double[capacity];

            var inicioFor = DateTime.Now.Millisecond;

            // este é uma implementação sequencial
            for (int index = 0; index < 500000; index++)
            {
                array[index] = Math.Sqrt(index);
            }

            var tempoFor = DateTime.Now.Millisecond - inicioFor;
            Console.WriteLine($"Tempo usando For {tempoFor}");


            var inicioParallel = DateTime.Now.Millisecond;

            // este é o equivalente implementação paralelo
            Parallel.For(from, to, index =>
            {
                array[index] = Math.Sqrt(index);
            });

            var tempoParallel = DateTime.Now.Millisecond - inicioParallel;
            Console.WriteLine($"Tempo usando Parallel {tempoParallel}");

            Console.ReadKey();
        }

        #endregion

        #region .: ParallelForeach :.

        private static void ParallelForeach()
        {
            var coffeeList = new List<Coffee>();
            coffeeList.Add(new Coffee());
            coffeeList.Add(new Coffee());
            coffeeList.Add(new Coffee());
            coffeeList.Add(new Coffee());
            coffeeList.Add(new Coffee());
            coffeeList.Add(new Coffee());
            coffeeList.Add(new Coffee());
            coffeeList.Add(new Coffee());

            // Esta é uma implementação sequencial
            foreach (var coffee in coffeeList)
            {
                CheckAvailability(coffee);
            }

            // Esta é a implementação paralela equivalente
            Parallel.ForEach(coffeeList, coffee => CheckAvailability(coffee));
        }

        private static void CheckAvailability(Coffee coffee) { }
        #endregion

        #region .: LinqParallel :.

        // LINQ paralelos é uma implementação de Language Integrated Query (LINQ) que suporta operações paralelas. Na maioria dos casos, A Syntax PLINQ é identica a syntax regular LINQ.
        // Quando você escreve uma expressão LINQ, você pode "optar em" PLINQ chamando o método de expressão AsParallel em sua fonte de dados IEnumerable.
        private static void LinqParallel()
        {
            var coffeeList = new List<Coffee>();
            coffeeList.Add(new Coffee());
            coffeeList.Add(new Coffee());
            coffeeList.Add(new Coffee());
            coffeeList.Add(new Coffee());
            coffeeList.Add(new Coffee());
            coffeeList.Add(new Coffee());
            coffeeList.Add(new Coffee());
            coffeeList.Add(new Coffee());

            var strongCoffees = from coffee in coffeeList.AsParallel()
                                where coffee.Strength > 3
                                select coffee;
        }

        #endregion
    }
}
