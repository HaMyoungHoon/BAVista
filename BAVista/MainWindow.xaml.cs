﻿using System;
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

namespace BAVista
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            tbResult.TextChanged += TbResult_TextChanged;
        }

        private void TbResult_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbResult.ScrollToEnd();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            ((MainWindowViewModel)DataContext).SetMessageHook(this);
        }

        public void IHaveToCloseThis(object sender)
        {
            ((MainWindowViewModel)DataContext).IHaveToCloseThis(sender, this);
        }
        public void DestroyWPF()
        {
            ((MainWindowViewModel)DataContext).DestroyWPF(this);
        }
    }
}
