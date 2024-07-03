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
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvReasons.ItemsSource);
                PropertyGroupDescription groupDescription = new("TargetFile");
                view.GroupDescriptions.Add(groupDescription);


            }
        }
        public DResultView(DetectionResultViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (lvReasons.ItemsSource != null)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvReasons.ItemsSource);
                PropertyGroupDescription groupDescription = new("TargetFile");
                view.GroupDescriptions.Add(groupDescription);


            }
        }


        // Global objects
        private CollectionView sortedReasonsView;
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        // Header click event


        // Sort code
        private void Sort(string sortBy, ListSortDirection direction)
        {
            sortedReasonsView ??= (CollectionView)CollectionViewSource.GetDefaultView(lvReasons.ItemsSource);
            sortedReasonsView.SortDescriptions.Clear();
            SortDescription sd = new(sortBy, direction);
            sortedReasonsView.SortDescriptions.Add(sd);
            sortedReasonsView.Refresh();
        }
        private void LvReasons_Click(object sender, RoutedEventArgs e)
        {
            ListSortDirection direction;

            if (e.OriginalSource is GridViewColumnHeader headerClicked)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    direction = headerClicked != _lastHeaderClicked
                        ? ListSortDirection.Ascending
                        : _lastDirection switch
                        {
                            ListSortDirection.Ascending => ListSortDirection.Descending,
                            ListSortDirection.Descending => ListSortDirection.Ascending,
                            _ => ListSortDirection.Ascending,
                        };

                    string header = headerClicked.Column.Header as string;
                    Sort(header, direction);

                    headerClicked.Column.HeaderTemplate = direction switch
                    {
                        ListSortDirection.Ascending => Resources["HeaderTemplateArrowUp"] as DataTemplate,
                        _ => Resources["HeaderTemplateArrowDown"] as DataTemplate,
                    };

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