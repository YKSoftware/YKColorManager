namespace YKColorManager
{
    using System.Windows;
    using YKColorManager.ViewModels;
    using YKColorManager.Views;
    using YKToolkit.Controls;

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ThemeManager.Instance.Initialize("Light Orange");

            var w = new MainView();
            var vm = new MainViewModel();

            w.DataContext = vm;
            w.Show();
        }
    }
}
