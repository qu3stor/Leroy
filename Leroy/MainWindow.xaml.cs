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
            for (int j = 1; j <= Int32.Parse(countTB.Text); j++)
            {



                string url = linkTB.Text + "?page=" + j;
                var web = new HtmlWeb();
                var doc = web.LoadFromBrowser(url);

                var names = doc.DocumentNode.SelectNodes("//li[@class = 'catalog__item inner ng-scope']//p[@class = 'catalog__name']");
                var prices = doc.DocumentNode.SelectNodes("//li[@class = 'catalog__item inner ng-scope']//p[@class = 'catalog__price ng-scope ng-binding']");

                for (int i = 0; i <= names.Count - 1; i++)
                {
                    string priceNumbers = RemoveNonNumeric(prices[i].InnerText);

                    Plywood good = new Plywood
                    {
                        GoodName = names[i].InnerText.Trim(),
                        GoodPrice = Double.Parse(priceNumbers.Remove(priceNumbers.Length - 2)),
                        Height = ExtractDimensions(names[i].InnerText.Trim()).Split('-')[0],
                        Width = ExtractDimensions(names[i].InnerText.Trim()).Split('-')[1],
                        Depth = ExtractDimensions(names[i].InnerText.Trim()).Split('-')[2]
                       
                    };
                    good.Square = Double.Parse(good.Height) * Double.Parse(good.Width) / 1000000;
                    good.GoodPriceM2 = good.GoodPrice / good.Square;

                    goodsDGRID.Items.Add(good);


                }
            }
        }
        

        public abstract class Good
            
        {
           public string GoodName { get; set; }
           public double GoodPrice { get; set; }
        }

        public class Plywood : Good
        {
            public string Height { get; set; }
            public string Width { get; set; }
            public string Depth { get; set; }
            public double Square { get; set; }
            public double GoodPriceM2 { get; set; }
        }

        static string RemoveNonNumeric(String str)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            str = rgx.Replace(str, "");
            // remove spaces
            str = Regex.Replace(str, @"\s+", "");
            return str;


        }

        static string ExtractDimensions(String str)
        {
            //ебучмй шабон того, что ищем в тексте [цифры от 0 до 9] один или более раза, далее х англ или х рус и снова цифры и еще раз тоже самое
            Regex reg1 = new Regex(@"[0-9]+[xх][0-9]+[xх][0-9]+");
            // ищем совпадения в исходной строке
            Match m = reg1.Match(str);
            str = m.Value.Replace('x', '-');
            str = str.Replace('х', '-');

            return str;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            goodsDGRID.Items.Clear();
        }
    }
}
