using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoOrganizr
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

        private void ChoseSourceBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            tbSource.Text = dialog.SelectedPath;
            AddToLog($"Nouveau dossier source selectioné: {tbSource.Text}");
            int filesToBeTreated = Organizer.CountNumberOfJpgFiles(tbSource.Text);
            AddToLog($"Nombre de photos à trier: {filesToBeTreated}");
        }

        private void ChoseDestBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            tbDest.Text = dialog.SelectedPath;
        }

        private void AddToLog(string log)
        {
            tblog.AppendText(log + Environment.NewLine);
            tblog.ScrollToEnd();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSource.Text)) { return; }
            if (string.IsNullOrWhiteSpace(tbDest.Text)) { return; }

            AddToLog(Organizer.ReadExif(tbSource.Text, tbDest.Text));
        }
    }
}
