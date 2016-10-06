namespace YKColorManager.Views.Behaviors
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    public class TextBoxBehavior
    {
        public static readonly DependencyProperty IsAllSelectWhenFocusedProperty = DependencyProperty.RegisterAttached("IsAllSelectWhenFocused", typeof(bool), typeof(TextBoxBehavior), new PropertyMetadata(false, OnIsAllSelectWhenFocusedPropertyChanged));

        public static bool GetIsAllSelectWhenFocused(DependencyObject target)
        {
            return (bool)target.GetValue(IsAllSelectWhenFocusedProperty);
        }

        public static void SetIsAllSelectWhenFocused(DependencyObject target, bool value)
        {
            target.SetValue(IsAllSelectWhenFocusedProperty, value);
        }

        private static void OnIsAllSelectWhenFocusedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox != null)
            {
                var isAllSelectWhenFocused = GetIsAllSelectWhenFocused(textbox);
                if (isAllSelectWhenFocused)
                {
                    textbox.GotFocus += OnGotFocus;
                }
                else
                {
                    textbox.GotFocus -= OnGotFocus;
                }
            }
        }

        private static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => (sender as TextBox).SelectAll()));
        }
    }
}
