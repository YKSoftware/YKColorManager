namespace YKColorManager.Models
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class ProductInfo
    {
        public static string Title { get; private set; }

        public static string Description { get; private set; }

        public static string Configuration { get; private set; }

        public static string Company { get; private set; }

        public static string Product { get; private set; }

        public static string Copyright { get; private set; }

        public static string Trademark { get; private set; }

        public static string Culture { get; private set; }

        public static string Version { get; private set; }

        public static string FileVersion { get; private set; }

        public static string FileName { get; private set; }

        public static string VersionString { get; private set; }

        public static bool IsBeta
        {
            get
            {
#if BETA
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        static ProductInfo()
        {
            var assembly = Assembly.GetEntryAssembly();
            FileName = assembly.GetName().Name;
            Version = assembly.GetName().Version.ToString();
            FileVersion = GetAttributeName<AssemblyFileVersionAttribute>(assembly, a => a.Version);
            Title = GetAttributeName<AssemblyTitleAttribute>(assembly, a => a.Title);
            Description = GetAttributeName<AssemblyDescriptionAttribute>(assembly, a => a.Description);
            Configuration = GetAttributeName<AssemblyConfigurationAttribute>(assembly, a => a.Configuration);
            Company = GetAttributeName<AssemblyCompanyAttribute>(assembly, a => a.Company);
            Product = GetAttributeName<AssemblyProductAttribute>(assembly, a => a.Product);
            Copyright = GetAttributeName<AssemblyCopyrightAttribute>(assembly, a => a.Copyright);
            Trademark = GetAttributeName<AssemblyTrademarkAttribute>(assembly, a => a.Trademark);
            Culture = GetAttributeName<AssemblyCultureAttribute>(assembly, a => a.Culture);

            var version = assembly.GetName().Version;
            VersionString = string.Concat(new string[]
            {
                "Ver.",
                version.Major.ToString(),
                ".",
                version.Minor.ToString(),
                ".",
                version.Build.ToString(),
                IsBeta ? " β" : "",
                version.Revision != 0 ? " Rev." : "",
                version.Revision != 0 ? version.Revision.ToString() : "",
                IsDebug ? " Debug" : "",
            });
        }

        private static string GetAttributeName<T>(Assembly assembly, Func<T, string> selector) where T : Attribute
        {
            var attr = assembly.GetCustomAttributes(typeof(T), true).Cast<T>().FirstOrDefault();
            return (attr == null) ? "" : selector(attr);
        }
    }
}
