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

        // este é o manipulador de evento para o botão
        private async void btnGetCoffees_Click(object sender, RoutedEventArgs e)
        {
            await GetCoffees(RemoveDuplicates);
        }

        // este é o método assincrono
        private async Task GetCoffees(Action<IEnumerable<string>> callback)
        {
            // Simula uma chamada para uma base de dados ou um webservice
            var coffees = await Task.Run(() =>
            {
                var coffeeList = new List<string>();
                coffeeList.Add("Caffe Americano");
                coffeeList.Add("Café ou Lait");
                coffeeList.Add("Café ou Lait");
                coffeeList.Add("Expresso Romano");
                coffeeList.Add("Latte");
                coffeeList.Add("Macchiato");
                coffeeList.Add("Macchiato");
                return coffeeList;
            });

            // Invoca o método de chamada de retorno assincrono
            await Task.Run(() => callback(coffees));
        }

        private void RemoveDuplicates(IEnumerable<string> coffees)
        {
            IEnumerable<string> uniqueCoffees = coffees.Distinct();
            Dispatcher.BeginInvoke(new Action(() =>
            {
                LstCoffees.ItemsSource = uniqueCoffees;
            }));
        }
    }
}
