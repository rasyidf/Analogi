using Analogi.Core;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
            if (blcv == null) blcv = (CollectionView)CollectionViewSource.GetDefaultView(lvReasons.ItemsSource);
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
                        switch (_lastDirection)
                        {
                            case ListSortDirection.Ascending:
                                direction = ListSortDirection.Descending;
                                break;
                            default:
                                direction = ListSortDirection.Ascending;
                                break;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(header, direction);

                    switch (direction)
                    {
                        case ListSortDirection.Ascending:
                            headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowUp"] as DataTemplate;
                            break;
                        default:
                            headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;
                            break;
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