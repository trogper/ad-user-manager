using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdUserManager
{
    class Utils
    {
        public static DateTime getNextExpirationDate()
        {
            DateTime date = Properties.Settings.Default.expiration_date;

            return getNextExpirationDate(date);
        }

        public static DateTime getNextExpirationDate(DateTime date)
        {
            string extendPeriod = Properties.Settings.Default.expiration_extend_period;

            do
                if (extendPeriod == "year")
                    date = date.AddYears(1);
                else
                    date = date.AddMonths(1);

            while (DateTime.Now.CompareTo(date) >= 0);

            return date;
        }

        public static void validateProperties()
        {
            if (Properties.Settings.Default.expiration_enabled)
            {
                string extendPeriod = Properties.Settings.Default.expiration_extend_period;
                if (extendPeriod != "year" && extendPeriod != "month")
                    throw new ApplicationException("invalid expiration_extend_period setting value");

                DateTime date = Properties.Settings.Default.expiration_date;
                if (date.CompareTo(DateTime.Now) > 0)
                    throw new ApplicationException("invalid expiration_date setting value, is not in the past");
            }


            string ManagedGroupsDN = Properties.Settings.Default.groups_dn;
            string ManagedUsersDN = Properties.Settings.Default.users_dn;
            string DomainName = Properties.Settings.Default.domain_name;

            if (string.IsNullOrEmpty(ManagedUsersDN) || string.IsNullOrEmpty(ManagedGroupsDN))
            {
                throw new ApplicationException("Settings user_dn and group_dn must not be empty");
            }

            if (string.IsNullOrEmpty(DomainName))
            {
                DomainName = LdapUtils.ExtractDomainName(ManagedUsersDN);
                if (string.IsNullOrEmpty(DomainName))
                    throw new ApplicationException("Setting domain_name not provided and could not extract it from user_dn");
            }
        }
    }
}
