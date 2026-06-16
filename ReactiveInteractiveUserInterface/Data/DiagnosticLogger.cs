using System.Collections.Concurrent;
using System.Globalization;
using System.Text;

namespace TP.ConcurrentProgramming.Data
{
    public class DiagnosticLogger : IDiagnosticLogger
    {
        private readonly ConcurrentQueue<string> _queue = new();
        private readonly TimeProvider _timeProvider;
        private readonly ITimer _flushTimer;
        private readonly string _filePath;
        private readonly int _capacity;

        private int _queuedMessagesCount = 0;
        private int _droppedMessagesCount = 0;
        private int _flushInProgress = 0;
        private int _disposed = 0;

        public DiagnosticLogger(
            string filePath = "diagnostics.txt",
            int capacity = 10_000,
            TimeProvider? timeProvider = null)
        {
            _filePath = filePath;
            _capacity = capacity;
            _timeProvider = timeProvider ?? TimeProvider.System;

            string? directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);

            _flushTimer = _timeProvider.CreateTimer(
                FlushCallback,
                null,
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(100));
        }

        public int DroppedMessagesCount => _droppedMessagesCount;

        public void Log(BallDiagnosticData data)
        {
            if (Volatile.Read(ref _disposed) == 1)
                return;

            string line = Serialize(data);

            int newCount = Interlocked.Increment(ref _queuedMessagesCount);

            if (newCount > _capacity)
            {
                Interlocked.Decrement(ref _queuedMessagesCount);
                Interlocked.Increment(ref _droppedMessagesCount);
                return;
            }

            _queue.Enqueue(line);
        }

        private void FlushCallback(object? state)
        {
            Flush();
        }

        private void Flush()
        {
            if (Interlocked.Exchange(ref _flushInProgress, 1) == 1)
                return;

            try
            {
                List<string> lines = [];

                while (_queue.TryDequeue(out string? line))
                {
                    Interlocked.Decrement(ref _queuedMessagesCount);
                    lines.Add(line);
                }

                if (lines.Count == 0)
                    return;

                File.AppendAllLines(_filePath, lines, Encoding.ASCII);
            }
            finally
            {
                Interlocked.Exchange(ref _flushInProgress, 0);
            }
        }

        private static string Serialize(BallDiagnosticData data)
        {
            return string.Join(";",
                data.Timestamp.ToString("O", CultureInfo.InvariantCulture),
                data.BallId.ToString(CultureInfo.InvariantCulture),
                data.X.ToString(CultureInfo.InvariantCulture),
                data.Y.ToString(CultureInfo.InvariantCulture),
                data.VelocityX.ToString(CultureInfo.InvariantCulture),
                data.VelocityY.ToString(CultureInfo.InvariantCulture),
                data.Mass.ToString(CultureInfo.InvariantCulture),
                data.Diameter.ToString(CultureInfo.InvariantCulture));
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 1)
                return;

            _flushTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            _flushTimer.Dispose();

            SpinWait.SpinUntil(
                () => Volatile.Read(ref _flushInProgress) == 0,
                TimeSpan.FromSeconds(2));

            Flush();
        }
    }
}