﻿using Microsoft.Azure.WebJobs.Script;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs.Script.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Logging.ApplicationInsights;

namespace Azure.Functions.Cli.Diagnostics
{
    public class ConsoleLoggerFactoryBuilder : ILoggerFactoryBuilder
    {
        public void AddLoggerProviders(ILoggerFactory factory, ScriptHostConfiguration scriptConfig, ScriptSettingsManager settingsManager)
        {
            // Automatically register App Insights if the key is present
            if (!string.IsNullOrEmpty(settingsManager?.ApplicationInsightsInstrumentationKey))
            {
                ITelemetryClientFactory clientFactory = scriptConfig.HostConfig.GetService<ITelemetryClientFactory>() ??
                    new ConsoleTelemetryClientFactory(settingsManager.ApplicationInsightsInstrumentationKey, scriptConfig.LogFilter.Filter);

                scriptConfig.HostConfig.LoggerFactory.AddApplicationInsights(clientFactory);
            }

            // TODO: Get trace level, add console logger
            scriptConfig.HostConfig.LoggerFactory.AddColoredConsole((cat, lvl) => cat.StartsWith("Worker."), false);
        }
    }
}
