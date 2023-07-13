using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ClientWpf.ViewModels;

namespace ClientWpf
{
    public partial class MainWindow : Window
    {
        private ClientViewModel clientViewModel;
        public MainWindow()
        {
            InitializeComponent();
            clientViewModel = new ClientViewModel();

            DataContext = clientViewModel;
        }

        private void ImagePanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                
                if(files != null && files.Length > 0)
                {
                    clientViewModel.Model.FileName = files[0];
                }
            }
        }
    }
}
