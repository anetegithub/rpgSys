using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace RuneFramework
{
    internal static class Logger
    {
        private static Int32 CurrentId = 0;
        internal static void LookAfter(Action a)
        {
            try
            {
                a();
            }
            catch(Exception ex)
            {
                var Logr = new XmlWriterTraceListener(Directory.GetCurrentDirectory() + "/log.xml");
                Logr.TraceEvent(new TraceEventCache(), ex.Message + Environment.NewLine + ex.StackTrace, TraceEventType.Error, CurrentId++);
            }
        }
    }
}
