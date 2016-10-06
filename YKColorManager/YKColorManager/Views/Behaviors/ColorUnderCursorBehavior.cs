namespace YKColorManager.Views.Behaviors
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class ColorUnderCursorBehavior
    {
        #region POINT 構造体
        /// <summary>
        /// Win32 API によるカーソル位置取得のための Point 構造体を表します。
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            /// 横軸座標を表します。
            /// </summary>
            public int X;

            /// <summary>
            /// 縦軸座標を表します。
            /// </summary>
            public int Y;

            /// <summary>
            /// 新しいインスタンスを生成します。
            /// </summary>
            /// <param name="x">横軸座標を指定します。</param>
            /// <param name="y">縦軸座標を指定します。</param>
            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
        #endregion POINT 構造体

        #region DllImport

        /// <summary>
        /// マウスカーソルの位置を取得します。
        /// </summary>
        /// <param name="pt">マウスカーソルの位置を POINT 構造体として返します。</param>
        /// <returns>正常に取得できた場合に true を返します。</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);

        /// <summary>
        /// DataContext を取得します。
        /// </summary>
        /// <param name="hWnd">ウィンドウハンドルを指定します。</param>
        /// <returns>指定されたウィンドウハンドルに対する DataContext を返します。</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        /// <summary>
        /// 指定された座標のピクセル値を取得します。
        /// </summary>
        /// <param name="hDC">DataContext を指定します。</param>
        /// <param name="X">横軸座標を指定します。</param>
        /// <param name="Y">縦軸座標を指定します。</param>
        /// <returns>取得したピクセル値を返します。</returns>
        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hDC, int X, int Y);

        #endregion DllImport

        #region SelectedColor 添付プロパティ
        /// <summary>
        /// SelectedColor 添付プロパティの定義
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.RegisterAttached("SelectedColor", typeof(Color), typeof(ColorUnderCursorBehavior), new FrameworkPropertyMetadata(default(Color), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// SelectedColor 添付プロパティを取得します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <returns>取得した値を返します。</returns>
        public static Color GetSelectedColor(DependencyObject target)
        {
            return (Color)target.GetValue(SelectedColorProperty);
        }

        /// <summary>
        /// SelectedColor 添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetSelectedColor(DependencyObject target, Color value)
        {
            target.SetValue(SelectedColorProperty, value);
        }
        #endregion SelectedColor 添付プロパティ

        #region IsEnabled 添付プロパティ
        /// <summary>
        /// IsEnabled 添付プロパティの定義
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ColorUnderCursorBehavior), new PropertyMetadata(false, OnIsEnabledPropertyChanged));

        /// <summary>
        /// IsEnabled 添付プロパティを取得します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <returns>取得した値を返します。</returns>
        public static bool GetIsEnabled(DependencyObject target)
        {
            return (bool)target.GetValue(IsEnabledProperty);
        }

        /// <summary>
        /// IsEnabled 添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetIsEnabled(DependencyObject target, bool value)
        {
            target.SetValue(IsEnabledProperty, value);
        }
        #endregion IsEnabled 添付プロパティ

        /// <summary>
        /// IsEnabled 添付プロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void OnIsEnabledPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var isEnabled = GetIsEnabled(button);
            if (isEnabled)
            {
                button.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
                button.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
                button.PreviewMouseMove += OnPreviewMouseMove;
            }
            else
            {
                button.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
                button.PreviewMouseLeftButtonUp -= OnPreviewMouseLeftButtonUp;
                button.PreviewMouseMove -= OnPreviewMouseMove;
            }
        }

        /// <summary>
        /// マウス左ボタンダウンイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Button).CaptureMouse();
        }

        /// <summary>
        /// マウス左ボタンアップイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var button = sender as Button;
            if (button.IsMouseCaptured)
            {
                button.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// マウス移動イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            var button = sender as Button;
            if (button.IsMouseCaptured)
            {
                var color = GetColorUnderCursor();
                SetSelectedColor(button, color);
            }
        }

        /// <summary>
        /// カーソル位置の色情報を取得します。
        /// </summary>
        /// <returns>色情報を System.Windows.Media.Color 構造体として返します。</returns>
        public static Color GetColorUnderCursor()
        {
            IntPtr dc = GetWindowDC(IntPtr.Zero);

            POINT p;
            GetCursorPos(out p);

            long color = GetPixel(dc, p.X, p.Y);
            byte a = 0xff;
            byte r = (byte)(color & 0x000000ff);            // なぜか r と b が逆。
            byte g = (byte)((color & 0x0000ff00) >> 8);
            byte b = (byte)((color & 0x00ff0000) >> 16);
            return Color.FromArgb(a, r, g, b);
        }
    }
}
