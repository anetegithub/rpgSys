using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace rpgSys.Log
{
    internal static class Logger
    {
        internal static void LookAfter(Action Action)
        {
            try { Action(); }
            catch (Exception ex)
            {
                var trace = new XmlWriterTraceListener(HttpContext.Current.Server.MapPath("~/Log/" + DateTime.Now.ToShortDateString() + ".txt"));
                trace.TraceEvent(new TraceEventCache(), ex.Message, TraceEventType.Error, 0);
                trace.Close();
            }
        }
    }
}