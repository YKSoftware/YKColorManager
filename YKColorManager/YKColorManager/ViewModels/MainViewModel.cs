namespace YKColorManager.ViewModels
{
    using System.Collections.ObjectModel;
    using YKColorManager.Models;

    public class MainViewModel : NotificationObject
    {
        public MainViewModel()
        {
            this.ColorList = new ObservableCollection<object>();
            this.ColorList.Add(new ColorInfo());
        }

        public string Title { get { return ProductInfo.Title + " " + ProductInfo.VersionString; } }

        private ObservableCollection<object> _colorList;
        public ObservableCollection<object> ColorList
        {
            get { return this._colorList; }
            private set { SetProperty(ref this._colorList, value); }
        }

        private DelegateCommand _newProcessCommand;
        public DelegateCommand NewProcessCommand
        {
            get
            {
                return this._newProcessCommand ?? (this._newProcessCommand = new DelegateCommand(_ =>
                {
                    System.Diagnostics.Process.Start("YKColorManager.exe");
                }));
            }
        }

        private DelegateCommand _addCommand;
        public DelegateCommand AddCommand
        {
            get
            {
                return this._addCommand ?? (this._addCommand = new DelegateCommand(_ =>
                {
                    this.ColorList.Add(new ColorInfo());
                }));
            }
        }

        private DelegateCommand _removeCommand;
        public DelegateCommand RemoveCommand
        {
            get
            {
                return this._removeCommand ?? (this._removeCommand = new DelegateCommand(
                p =>
                {
                    var item = p as ColorInfo;
                    this.ColorList.Remove(item);
                },
                _ => this.ColorList.Count > 1));
            }
        }

        private DelegateCommand _upCommand;
        public DelegateCommand UpCommand
        {
            get
            {
                return this._upCommand ?? (this._upCommand = new DelegateCommand(
                p =>
                {
                    var item = p as ColorInfo;
                    var oldIndex = this.ColorList.IndexOf(item);
                    var newIndex = this.ColorList.IndexOf(item) - 1;
                    this.ColorList.Move(oldIndex, newIndex);
                },
                p =>
                {
                    var item = p as ColorInfo;
                    var index = this.ColorList.IndexOf(item) - 1;
                    return index >= 0;
                }));
            }
        }

        private DelegateCommand _downCommand;
        public DelegateCommand DownCommand
        {
            get
            {
                return this._downCommand ?? (this._downCommand = new DelegateCommand(
                p =>
                {
                    var item = p as ColorInfo;
                    var oldIndex = this.ColorList.IndexOf(item);
                    var newIndex = this.ColorList.IndexOf(item) + 1;
                    this.ColorList.Move(oldIndex, newIndex);
                },
                p =>
                {
                    var item = p as ColorInfo;
                    var index = this.ColorList.IndexOf(item) + 1;
                    return index < this.ColorList.Count;
                }));
            }
        }
    }
}
