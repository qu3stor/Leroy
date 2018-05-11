using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Leroy
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

        private void parseBTN_Click(object sender, RoutedEventArgs e)
        {
            string url = linkTB.Text;
            var web = new HtmlWeb();
            var doc = web.LoadFromBrowser(url);

            var names = doc.DocumentNode.SelectNodes("//li[@class = 'catalog__item inner ng-scope']//p[@class = 'catalog__name']");
            var prices = doc.DocumentNode.SelectNodes("//li[@class = 'catalog__item inner ng-scope']//p[@class = 'catalog__price ng-scope ng-binding']");

            for (int i = 0; i <= names.Count - 1; i++)
            {
                string priceNumbers = RemoveNonNumeric(prices[i].InnerText);

                Good good = new Good
                {
                    GoodName = names[i].InnerText.Trim(),
                    GoodPrice = priceNumbers.Remove(priceNumbers.Length - 2)

                };

                goodsDGRID.Items.Add(good);

            }
        }
        

        public class Good
            
        {
           public string GoodName { get; set; }
           public string GoodPrice { get; set; }
        }

        static string RemoveNonNumeric(String str)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            str = rgx.Replace(str, "");
            // remove spaces
            str = Regex.Replace(str, @"\s+", "");
            return str;


        }

    }
}
