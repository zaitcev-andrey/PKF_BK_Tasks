using System.Windows;

using ClientWpf.ViewModels;

namespace ClientWpf
{
    public partial class MainWindow : Window
    {
        #region private Members
        private ClientViewModel clientViewModel;
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            clientViewModel = new ClientViewModel();

            DataContext = clientViewModel;
        }
        #endregion

        #region private Methods
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
        #endregion
    }
}
