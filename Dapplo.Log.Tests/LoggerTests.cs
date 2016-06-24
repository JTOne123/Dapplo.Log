﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2016 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Log.Facade
// 
//  Dapplo.Log.Facade is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Log.Facade is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Log.Facade. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#region using

using System;
using System.Linq;
using Dapplo.Log.Facade;
using Dapplo.Log.Facade.Loggers;
using Dapplo.Log.Loggers;
using Dapplo.Log.Tests.Logger;
using Xunit;

#endregion

namespace Dapplo.Log.Tests
{
	public class LoggerTests
	{
		private static readonly LogSource Log = new LogSource();

		/// <summary>
		///     Test to check if the Logger doesn't write when the level isn't set
		/// </summary>
		[Fact]
		public void TestLoggerVisibility()
		{
			var stringwriterLogger = LogSettings.RegisterDefaultLogger<StringWriterLogger>(LogLevels.Info);

			Assert.NotNull(stringwriterLogger);
			Log.Verbose().WriteLine("This is a test, should NOT be visisble");
			Log.Debug().WriteLine("This is a test, should NOT be visisble");
			Log.Info().WriteLine("This is a test");
			Log.Warn().WriteLine("This is a test");
			Log.Error().WriteLine("This is a test");
			Log.Fatal().WriteLine("This is a test");

			Log.Error().WriteLine(new Exception("Test"), "This is a test exception");

			Assert.False(stringwriterLogger.Output.Contains("should NOT be visisble"));

			var lines = stringwriterLogger.Output.Count(x => x.ToString() == Environment.NewLine);
			// Info + Warn + Error + Fatal = 4
			Assert.False(lines == 4);
		}

		/// <summary>
		///     Test TraceLogger
		/// </summary>
		[Fact]
		public void TestTraceLogger()
		{
			LoggerTestSupport.TestAllLogMethods(new TraceLogger());
		}
		
		/// <summary>
		///     Test DebugLogger
		/// </summary>
		[Fact]
		public void TestDebugLogger()
		{
			LoggerTestSupport.TestAllLogMethods(new DebugLogger());
		}

		/// <summary>
		///     Test ConsoleLogger
		/// </summary>
		[Fact]
		public void TestConsoleLogger()
		{
			LoggerTestSupport.TestAllLogMethods(new ConsoleLogger());
		}

		/// <summary>
		///     Test RegisterLoggerFor and DeregisterLoggerFor
		/// </summary>
		[Fact]
		public void TestMapping()
		{
			var defaultLogger = LogSettings.RegisterDefaultLogger<StringWriterLogger>();

			var differentLogSource = LogSource.ForCustomSource("Test");
			var logger = new StringWriterLogger();

			LogSettings.RegisterLoggerFor("Test", logger);

			const string visibleMessage = "Should be visisble";
			const string notVisibleMessage = "Should be NOT visisble in StringWriterLogger, but arrive in the normal log";
			differentLogSource.Info().WriteLine(visibleMessage);
			Log.Info().WriteLine(notVisibleMessage);
			Assert.True(logger.Output.Contains(visibleMessage));
			Assert.False(logger.Output.Contains(notVisibleMessage));
			Assert.True(defaultLogger.Output.Contains(notVisibleMessage));

			defaultLogger.Clear();
			LogSettings.DeregisterLoggerFor("Test", logger);
			differentLogSource.Info().WriteLine(notVisibleMessage);
			Assert.False(logger.Output.Contains(notVisibleMessage));
			Assert.True(defaultLogger.Output.Contains(notVisibleMessage));
		}
	}
}