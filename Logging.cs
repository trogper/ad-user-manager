using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdUserManager
{
    class Logging
    {
        private static EventLog eventLog;
        private static bool enabled;

        public static void init()
        {
            enabled = Properties.Settings.Default.logging_enabled;

            if (!enabled)
                return;

            eventLog = new EventLog("Application");
            eventLog.Source = "AdUserManager";

            if (!EventLog.SourceExists("AdUserManager"))
            {
                try
                {
                    EventLog.CreateEventSource("AdUserManager", "Application");
                    // TODO messagebox("eventlog source created");
                }
                catch (Exception ex)
                {
                    enabled = false;
                    throw new ApplicationException("Event log source could not be created. Try running as admin once to create the source. Exception: " + ex.Message);
                }
            }
            
        }

        public static void logInfo(string message)
        {
            if (enabled)
                eventLog.WriteEntry(message, EventLogEntryType.Information);
        }

        public static void logWarning(string message)
        {
            if (enabled)
                eventLog.WriteEntry(message, EventLogEntryType.Warning);
        }

        public static void logError(string message)
        {
            if (enabled)
                eventLog.WriteEntry(message, EventLogEntryType.Error);
        }

    }
}
