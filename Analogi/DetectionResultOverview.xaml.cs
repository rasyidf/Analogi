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
using System.Windows.Shapes;

namespace Analogi
{
    /// <summary>
    /// Interaction logic for DetectionResultOverview.xaml
    /// </summary>
    public partial class DetectionResultOverview : Window
    {
        public DetectionResultOverview()
        {
            InitializeComponent();

            if (lvReasons.ItemsSource != null)
            {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvReasons.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("TargetFile");
            view.GroupDescriptions.Add(groupDescription);
            

            }
        }
    }
}