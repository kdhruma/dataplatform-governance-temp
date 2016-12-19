using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MDM.WCFServices;

namespace MDM.WCFServices.App_Code
{
    public static class AppInitializer
    {
        public static void AppInitialize()
        {
            try
            {
                MDM.WCFServices.MDMWCFBase baseService = new MDMWCFBase(false);
            }
            catch (Exception)
            { 
                //What to do??
            }
        }
    }
}