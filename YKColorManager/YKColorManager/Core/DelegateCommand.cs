namespace YKColorManager
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// ICommand を実装したクラスを表します。
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// ICommand の実行時の処理
        /// </summary>
        private Action<object> _execute;

        /// <summary>
        /// ICommand の実行可能判別処理
        /// </summary>
        private Func<object, bool> _canExecute;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="execute">ICommand の実行時の処理を指定します。</param>
        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="execute">ICommand の実行時の処理を指定します。</param>
        /// <param name="canExecute">ICommand の実行可能判別処理を指定します。</param>
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        #region ICommand のメンバ
        /// <summary>
        /// 実行可能判別結果変更時に発生します。
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// 実行可能判別処理をおこないます。
        /// </summary>
        /// <param name="parameter">必要なパラメータを指定します。</param>
        /// <returns>実行可能である場合に true を返します。</returns>
        public bool CanExecute(object parameter)
        {
            return this._canExecute != null ? this._canExecute(parameter) : true;
        }

        /// <summary>
        /// 実行時の処理をおこないます。
        /// </summary>
        /// <param name="parameter">必要なパラメータを指定します。</param>
        public void Execute(object parameter)
        {
            if (this._execute != null)
                this._execute(parameter);
        }
        #endregion ICommand のメンバ
    }
}
