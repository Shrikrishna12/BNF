using System;
using System.Collections.Generic;
using System.Web;

namespace BNF.Constants
{
    internal class Constants
    {
        public static string USER_LANG_PREF_ENG;
        public static string USER_LANG_PREF_AR;

        private string USER_LANG_PREF_AR1
        {
            get
            {
                return USER_LANG_PREF_AR;
            }
            set
            {
                USER_LANG_PREF_AR = value;
            }
        }

        private string USER_LANG_PREF_ENG1
        {
            get
            {
                return USER_LANG_PREF_ENG;
            }
            set
            {
                USER_LANG_PREF_ENG = value;
            }
        }

    }
}