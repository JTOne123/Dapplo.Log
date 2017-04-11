﻿#region Usings

using System;
using Dapplo.Log.Loggers;

#endregion

namespace Dapplo.Log.ConsoleExample
{
    internal class Program
    {
        private static void Main()
        {
            LogSource log = new LogSource();
            var logger = LogSettings.RegisterDefaultLogger<ColorConsoleLogger>();
            logger.LogLevel = LogLevels.Verbose;
            log.Verbose().WriteLine("This is {0}", LogLevels.Verbose);
            log.Debug().WriteLine("This is {0}", LogLevels.Debug);
            log.Info().WriteLine("This is {0}", LogLevels.Info);
            log.Warn().WriteLine("This is {0}", LogLevels.Warn);
            log.Error().WriteLine("This is {0}", LogLevels.Error);
            log.Fatal().WriteLine("This is {0}", LogLevels.Fatal);
            Console.ReadLine();
        }
    }
}