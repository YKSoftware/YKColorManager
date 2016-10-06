namespace YKColorManager
{
    using System.Windows.Media;

    public static class Extensions
    {
        public static int ColorToInt32(this Color color)
        {
            return (color.A << 24) + (color.R << 16) + (color.G << 8) + color.B;
        }
    }
}
