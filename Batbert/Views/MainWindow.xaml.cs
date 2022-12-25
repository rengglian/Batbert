using Batbert.Interfaces;
using System;
using System.Windows;

namespace Batbert.Views

{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(DataContext is ICloseWindows vm)
            {
                vm.CloseWindow += () => 
                {
                    Close();
                };
            }
        }

        private void MainWindow_MouseLeftButtonDown(object sender, EventArgs e)
        {
            DragMove();
        }
    }
}
