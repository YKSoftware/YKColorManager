namespace YKColorManager
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// INotifyPropertyChanged を実装した抽象クラスを表します。
    /// </summary>
    public abstract class NotificationObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged のメンバ
        /// <summary>
        /// プロパティ値変更時に発生します。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion INotifyPropertyChanged のメンバ

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            var h = this.PropertyChanged;
            if (h != null) h(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// プロパティ値変更用のヘルパです。
        /// </summary>
        /// <typeparam name="T">プロパティの型を示します。</typeparam>
        /// <param name="target">プロパティの実体を指定します。</param>
        /// <param name="value">変更後の値を指定します。</param>
        /// <param name="propertyName">プロパティ名を指定します。</param>
        /// <returns>プロパティ値に変更があった場合に true を返します。</returns>
        protected bool SetProperty<T>(ref T target, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(target, value))
                return false;
            target = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
