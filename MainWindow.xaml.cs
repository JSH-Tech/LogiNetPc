using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace LogiNetPc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DirectoryInfo winTemp;
        public DirectoryInfo appTemp;

        public MainWindow()
        {
            InitializeComponent();
            winTemp = new DirectoryInfo(@"C:\Windows\Temp");
            appTemp= new DirectoryInfo(System.IO.Path.GetTempPath());
            loadDate();
        }

        public long DirSize(DirectoryInfo dir)
        {
            
            try
            {
                return dir.GetFiles().Sum(f => f.Length) + dir.GetDirectories().Sum(di => DirSize(di));
            }
            catch { return 0; }
     
        }

        public void clearTempData(DirectoryInfo dir) {

            foreach (var item in dir.GetFiles())
            {
                try
                {
                    item.Delete();
                    Console.WriteLine(item.FullName);
                }
                catch (Exception ex) {

                    MessageBox.Show(ex.Message);
                
                }
            }

            foreach (var item in dir.GetDirectories())
            {
                try
                {
                    item.Delete(true);
                    Console.WriteLine(item.FullName);
                }
                catch(Exception ex) 
                {

                    MessageBox.Show(ex.Message);

                }
            }
        }

        private void Btn_MAJ_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Votre logiciel est a jour !","Mise a jour.",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void Btn_SiteWeb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://www.google.fr")
                {
                    UseShellExecute = true
                });
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
            finally
            {
                MessageBox.Show("Site web en conception", "Site web", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Btn_Analyser_Click(object sender, RoutedEventArgs e)
        {
            AnalyseFolder();
        }

        public void AnalyseFolder()
        {
            Console.WriteLine("Debut de l'analyse ... ");
            long totalSize = 0;

            try
            {
                totalSize += DirSize(winTemp) / 1_000_000;
                totalSize += DirSize(appTemp) / 1_000_000;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            valeurEspaceANettoyer.Content=totalSize + " Mb";
            titrePrincipal.Content = "Analyse effectué";
            DateDerniereNettoyage.Content = DateTime.Today;
            saveDate();
        }

        private void Btn_Nettoyer_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Nettoyage en cours...");
            Btn_Nettoyer.Content = "NETTOYAGE EN COURS...";

            //netoyage du presse pappier
            Clipboard.Clear();
            //autre netoyage
            try
            {
               clearTempData(winTemp);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            try
            {
               clearTempData(appTemp);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            Btn_Nettoyer.Content = "\n\nNETTOYAGE TERMINER";
            titrePrincipal.Content = "Nettoyage effectué";
            valeurEspaceANettoyer.Content = "0 Mb";
        }


        public void saveDate()
        {
            try
            {
                string date = DateTime.Today.ToString("yyyy-MM-dd");
                File.WriteAllText("date.txt", date);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving date: " + ex.Message);
            }
        }

        public void loadDate()
        {
            try
            {
                if (File.Exists("date.txt"))
                {
                    string date = File.ReadAllText("date.txt");
                    if (!string.IsNullOrEmpty(date))
                    {
                        DateDerniereNettoyage.Content = date;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading date: " + ex.Message);
            }
        }

    }
}
