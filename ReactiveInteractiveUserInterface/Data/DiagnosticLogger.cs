using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TP.ConcurrentProgramming.Data
{
    public class DiagnosticLogger : IDiagnosticLogger
    {
        private readonly Channel<string> _channel;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly Task _writerTask;
        private readonly string _filePath;

        private int _droppedMessagesCount = 0;

        public DiagnosticLogger(string filePath = "diagnostics.txt", int capacity = 10_000)
        {
            _filePath = filePath;

            BoundedChannelOptions options = new(capacity)
            {
                FullMode = BoundedChannelFullMode.DropWrite,
                SingleReader = true,
                SingleWriter = false
            };

            _channel = Channel.CreateBounded<string>(options);
            _writerTask = Task.Run(WriteLoop);
        }

        public int DroppedMessagesCount => _droppedMessagesCount;

        public void Log(BallDiagnosticData data)
        {
            string line = Serialize(data);

            if (!_channel.Writer.TryWrite(line))
                Interlocked.Increment(ref _droppedMessagesCount);
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

        private async Task WriteLoop()
        {
            using FileStream fileStream = new(
                _filePath,
                FileMode.Append,
                FileAccess.Write,
                FileShare.Read);

            await foreach (string line in _channel.Reader.ReadAllAsync(_cancellationTokenSource.Token))
            {
                byte[] bytes = Encoding.ASCII.GetBytes(line + Environment.NewLine);
                await fileStream.WriteAsync(bytes, 0, bytes.Length, _cancellationTokenSource.Token);
                await fileStream.FlushAsync(_cancellationTokenSource.Token);
            }
        }

        public void Dispose()
        {
            _channel.Writer.TryComplete();
            _cancellationTokenSource.CancelAfter(2000);

            try
            {
                _writerTask.Wait();
            }
            catch
            {
            }

            _cancellationTokenSource.Dispose();
        }
    }
}
