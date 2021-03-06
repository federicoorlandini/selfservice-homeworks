﻿using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace Tasks.WS.Lib
{
    /// <summary>
    /// This class uses the Log4Net library to log all the unhandled exception information
    /// in a text file (see the web.config for the Log4Net configuration)
    /// </summary>
    public class Log4NetExceptionLogger : ExceptionLogger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Log4NetExceptionLogger));

        public Log4NetExceptionLogger()
        {
            XmlConfigurator.Configure();
        }

        public override void Log(ExceptionLoggerContext context)
        {
            log.Error(RequestToString(context.Request), context.Exception);
        }

        private static string RequestToString(HttpRequestMessage request)
        {
            var message = new StringBuilder();
            if (request.Method != null)
                message.Append(request.Method);

            if (request.RequestUri != null)
                message.Append(" ").Append(request.RequestUri);

            return message.ToString();
        }
    }
}