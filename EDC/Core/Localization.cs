using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC
{
    public static class Localization
    {
        /// <summary>
        /// Страница {0} из {1}
        /// </summary>
        public static string Page
        {
            get
            {
                return "Страница {0} из {1}";
            }
        }

        /// <summary>
        /// "Записи {0} - {1} из {2}"
        /// </summary>
        public static string Records
        {
            get
            {
                return "Записи {0} - {1} из {2}";
            }
        }

        /// <summary>
        /// Записей на страницу
        /// </summary>
        public static string RecordsOnPage
        {
            get
            {
                return "Записей на страницу";
            }
        }
    }
}