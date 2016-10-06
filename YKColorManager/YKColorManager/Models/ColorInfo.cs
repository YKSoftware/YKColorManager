namespace YKColorManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Media;

    public class ColorInfo : NotificationObject
    {
        private static List<KeyValuePair<string, Color>> _colorList;
        public List<KeyValuePair<string, Color>> ColorList
        {
            get
            {
                if (_colorList == null)
                {
                    _colorList = typeof(Colors).GetProperties()
                                 .Where(n => n.PropertyType == typeof(Color))
                                 .Select(n => new KeyValuePair<string, Color>(n.Name, (Color)(n.GetValue(null, null))))
                                 //.OrderBy(n => n.Value.ColorToInt32())
                                 .ToList();
                }
                return _colorList;
            }
        }

        public ColorInfo()
        {
            this.CurrentColor = Colors.Black;
        }

        private int _index;
        public int Index
        {
            get { return this._index; }
            set { SetProperty(ref this._index, value); }
        }

        private Color _currentColor;
        public Color CurrentColor
        {
            get { return this._currentColor; }
            set
            {
                if (SetProperty(ref this._currentColor, value))
                {
                    this._a = this._currentColor.A;
                    this._r = this._currentColor.R;
                    this._g = this._currentColor.G;
                    this._b = this._currentColor.B;
                    RaisePropertyChanged("A");
                    RaisePropertyChanged("R");
                    RaisePropertyChanged("G");
                    RaisePropertyChanged("B");
                    UpdateColorCode();
                    UpdateCurrentComparableColor();
                }
            }
        }

        private Color _currentComparableColor;
        public Color CurrentComparableColor
        {
            get { return this._currentComparableColor; }
            set
            {
                if (SetProperty(ref this._currentComparableColor, value))
                {
                    var color = this._currentComparableColor;
                    color.A = this.CurrentColor.A;
                    this._currentColor = color;
                    RaisePropertyChanged("CurrentColor");

                    this._a = this._currentColor.A;
                    this._r = this._currentColor.R;
                    this._g = this._currentColor.G;
                    this._b = this._currentColor.B;
                    RaisePropertyChanged("A");
                    RaisePropertyChanged("R");
                    RaisePropertyChanged("G");
                    RaisePropertyChanged("B");
                    UpdateColorCode();
                }
            }
        }

        private string _colorCode;
        public string ColorCode
        {
            get { return this._colorCode; }
            set
            {
                if (SetProperty(ref this._colorCode, value.ToUpper()))
                {
                    this._currentColor = Parse(this._colorCode);
                    this._a = this._currentColor.A;
                    this._r = this._currentColor.R;
                    this._g = this._currentColor.G;
                    this._b = this._currentColor.B;
                    RaisePropertyChanged("CurrentColor");
                    RaisePropertyChanged("A");
                    RaisePropertyChanged("R");
                    RaisePropertyChanged("G");
                    RaisePropertyChanged("B");
                    UpdateCurrentComparableColor();
                }
            }
        }

        private byte _a;
        public byte A
        {
            get { return this._a; }
            set
            {
                if (SetProperty(ref this._a, value))
                {
                    UpdateColorCode();
                    UpdateCurrentColor();
                }
            }
        }

        private byte _r;
        public byte R
        {
            get { return this._r; }
            set
            {
                if (SetProperty(ref this._r, value))
                {
                    UpdateColorCode();
                    UpdateCurrentColor();
                }
            }
        }

        private byte _g;
        public byte G
        {
            get { return this._g; }
            set
            {
                if (SetProperty(ref this._g, value))
                {
                    UpdateColorCode();
                    UpdateCurrentColor();
                }
            }
        }

        private byte _b;
        public byte B
        {
            get { return this._b; }
            set
            {
                if (SetProperty(ref this._b, value))
                {
                    UpdateColorCode();
                    UpdateCurrentColor();
                }
            }
        }

        private void UpdateColorCode()
        {
            this._colorCode = string.Concat(new string[]
            {
                "#",
                this.A.ToString("X2"),
                this.R.ToString("X2"),
                this.G.ToString("X2"),
                this.B.ToString("X2"),
            });
            RaisePropertyChanged("ColorCode");
        }

        private void UpdateCurrentColor()
        {
            this._currentColor = Color.FromArgb(this.A, this.R, this.G, this.B);
            RaisePropertyChanged("CurrentColor");
            UpdateCurrentComparableColor();
        }

        private void UpdateCurrentComparableColor()
        {
            var color = this.CurrentColor;
            color.A = (byte)0xff;
            this._currentComparableColor = color;
            RaisePropertyChanged("CurrentComparableColor");
        }

        private Color Parse(string code)
        {
            if (code.Length < 7) return Colors.Transparent;
            if (code.First() != '#') return Colors.Transparent;

            byte a, r, g, b;
            if (code.Length == 7)
            {
                a = (byte)0xff;
                if (!byte.TryParse(code.Substring(1, 2), NumberStyles.HexNumber, null, out r)) return Colors.Transparent;
                if (!byte.TryParse(code.Substring(3, 2), NumberStyles.HexNumber, null, out g)) return Colors.Transparent;
                if (!byte.TryParse(code.Substring(5, 2), NumberStyles.HexNumber, null, out b)) return Colors.Transparent;
            }
            else if (code.Length == 9)
            {
                if (!byte.TryParse(code.Substring(1, 2), NumberStyles.HexNumber, null, out a)) return Colors.Transparent;
                if (!byte.TryParse(code.Substring(3, 2), NumberStyles.HexNumber, null, out r)) return Colors.Transparent;
                if (!byte.TryParse(code.Substring(5, 2), NumberStyles.HexNumber, null, out g)) return Colors.Transparent;
                if (!byte.TryParse(code.Substring(7, 2), NumberStyles.HexNumber, null, out b)) return Colors.Transparent;
            }
            else
            {
                return Colors.Transparent;
            }

            return Color.FromArgb(a, r, g, b);
        }
    }
}
