using System;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.Globalization;

namespace Riversand
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Localization
	{
		public enum Modules
		{
			Web,
			Menu
		}

		public static string GetWebString(string name)
		{
			return Riversand.Localization.GetString(Riversand.Localization.Modules.Web, name, System.Threading.Thread.CurrentThread.CurrentUICulture);
		}

		public static string GetString(Riversand.Localization.Modules module, string name)
		{
			return Riversand.Localization.GetString(module, name, System.Threading.Thread.CurrentThread.CurrentUICulture);
		}

        public static string GetString(Riversand.Localization.Modules module, string name, CultureInfo culture)
        {
            string fullModuleName = "Riversand." + module.ToString() + ".Resources";

            string result = "";
            if (ResourceManagers[fullModuleName] != null)
            {
                ResourceManager manager = (ResourceManager)ResourceManagers[fullModuleName];

                if (!string.IsNullOrEmpty(name))//added by Jitendra to check NULL
                    result = manager.GetString(name, culture);
            }
            return (result != "" && result != null) ? result : name;
        }

		public static Hashtable ResourceManagers = new Hashtable();

		private Localization()
		{
			// Empty private destructor
		}
	}
}
