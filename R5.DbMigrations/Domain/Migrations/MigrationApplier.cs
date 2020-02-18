using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Domain.Migrations
{
	public class MigrationApplier
	{
		private DbMigration _migration { get; }
		private ILogger _logger { get; }
		private Stopwatch _stopwatch { get; }
		private DateTime? _startTime { get; set; }
		private MigrationLog.MigrationResultType? _result { get; set; }
		private object _additionalContext { get; set; }

		public MigrationApplier(
			DbMigration migration,
			ILogger logger)
		{
			_migration = migration
				?? throw new ArgumentNullException(nameof(migration));
			_logger = logger
				?? throw new ArgumentNullException(nameof(logger));
			_stopwatch = new Stopwatch();
		}

		public async Task<MigrationLog.ApplyAttempt> ApplyAsync(object context)
		{
			_logger.LogInformation("Starting migration attempt for {@Migration}", _migration);

			try
			{
				_stopwatch.Start();
				_startTime = DateTime.UtcNow;

				await _migration.ApplyAsync(context);
				_result = MigrationLog.MigrationResultType.Completed;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Migration failed.");
				_result = MigrationLog.MigrationResultType.Error;
				_additionalContext = new
				{
					Exception = ex,
					Message = ex.Message,
					StackTrace = ex.ToString()
				};
			}
			finally
			{
				_stopwatch.Stop();
			}

			MigrationLog.ApplyAttempt attempt = CreateAttemptLog();
			_logger.LogInformation("Migration finished with {@Result}", attempt);
			return attempt;
		}

		private MigrationLog.ApplyAttempt CreateAttemptLog()
		{
			if (!_startTime.HasValue) throw new InvalidOperationException("Migration doesn't have its' start time recorded yet.");
			if (_stopwatch.IsRunning) throw new InvalidOperationException("Stopwatch is still running for a migration.");
			if (!_result.HasValue) throw new InvalidOperationException("Migration result hasn't been resolved yet.");
			return new MigrationLog.ApplyAttempt
			{
				Start = _startTime.Value,
				ElapsedTimeSeconds = _stopwatch.Elapsed.TotalSeconds,
				Result = _result.Value,
				AdditionalContext = _additionalContext
			};
		}
	}
}
