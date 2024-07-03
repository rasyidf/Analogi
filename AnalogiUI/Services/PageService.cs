using Wpf.Ui;

namespace AnalogiUI.Services
{
    /// <summary>
    /// Service that provides pages for navigation.
    /// </summary>
    public class PageService : IPageService
    {
        /// <summary>
        /// Service which provides the instances of pages.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates new instance and attaches the <see cref="IServiceProvider"/>.
        /// </summary>
        public PageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public T? GetPage<T>()
            where T : class
        {
            return !typeof(FrameworkElement).IsAssignableFrom(typeof(T))
                ? throw new InvalidOperationException("The page should be a WPF control.")
                : (T?)_serviceProvider.GetService(typeof(T));
        }

        /// <inheritdoc />
        public FrameworkElement? GetPage(Type pageType)
        {
            return !typeof(FrameworkElement).IsAssignableFrom(pageType)
                ? throw new InvalidOperationException("The page should be a WPF control.")
                : _serviceProvider.GetService(pageType) as FrameworkElement;
        }
    }
}
