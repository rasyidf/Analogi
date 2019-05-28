using Analogi.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Analogi
{
    /// <summary>
    /// Interaction logic for DResultView.xaml
    /// </summary>
    public partial class DResultView : UserControl
    {
        public DResultView()
        {
            InitializeComponent();

            if (lvReasons.ItemsSource != null)
            {
                var view = (CollectionView)CollectionViewSource.GetDefaultView(lvReasons.ItemsSource);
                var groupDescription = new PropertyGroupDescription("TargetFile");
                view.GroupDescriptions.Add(groupDescription);


            }
        }
        public DResultView(DetectionResult viewModel)
        {
            InitializeComponent();

            this.DataContext = viewModel;

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (lvReasons.ItemsSource != null)
            {
                var view = (CollectionView)CollectionViewSource.GetDefaultView(lvReasons.ItemsSource);
                var groupDescription = new PropertyGroupDescription("TargetFile");
                view.GroupDescriptions.Add(groupDescription);


            }
        }


        // Global objects
        CollectionView blcv;
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        // Header click event


        // Sort code
        private void Sort(string sortBy, ListSortDirection direction)
        {
            if (blcv == null) blcv =  (CollectionView)CollectionViewSource .GetDefaultView(lvReasons.ItemsSource);
            blcv.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            blcv.SortDescriptions.Add(sd);
            blcv.Refresh();
        }
        private void LvReasons_Click(object sender, RoutedEventArgs e)
        {
            ListSortDirection direction;

            if (e.OriginalSource is GridViewColumnHeader headerClicked)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(header, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }
    }
}