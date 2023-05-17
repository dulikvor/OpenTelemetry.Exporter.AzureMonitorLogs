using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.Monitor
{
    public class ActivityScope : IDisposable
    {
        public static string Source = "ActivityTrace";
        private static readonly ActivitySource _activitySource = new ActivitySource(Source);


        private readonly Stopwatch _stopWatch;
        private readonly Activity _activity;
        private bool _isDisposed = false;

        public async Task<TResult> Monitor<TResult>(Func<Task<TResult>> function)
        {
            try
            {
                return await function();
            }
            catch (Exception e)
            {
                OnFailure(e);
                throw;
            }
        }

        public async Task Monitor(Func<Task> function)
        {
            try
            {
                await function();
            }
            catch (Exception e)
            {
                OnFailure(e);
                throw;
            }
        }

        public void Monitor(Action function)
        {
            try
            {
                function();
            }
            catch (Exception e)
            {
                OnFailure(e);
                throw;
            }
        }

        public TResult Monitor<TResult>(Func<TResult> function)
        {
            try
            {
                return function();
            }
            catch (Exception e)
            {
                OnFailure(e);
                throw;
            }
        }

        public static ActivityScope Create(string callingType = "", [CallerMemberName] string callerName = "", int stackFrames = 1)
        {
            string typeName = callingType;
            if (string.IsNullOrEmpty(callingType))
            {
                var method = new StackTrace().GetFrame(stackFrames).GetMethod();
                typeName = GetTypeName(method);
            }

            return new ActivityScope($"{typeName}.{callerName}");
        }

        public Activity Activity  { get => _activity; }

        private ActivityScope(string name)
        {
            _stopWatch = Stopwatch.StartNew();
            _activity = _activitySource.StartActivity(name);
            _activity.SetTag("startTime", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
            _activity.Start();
        }

        public void OnFailure(Exception e)
        {
            _activity.SetTag("exception", e.ToString());
            _activity.SetTag("exceptionMessage", e.Message);
        }

        public void Dispose()
        {
            if(_isDisposed)
            {
                return;
            }
            _activity.SetTag("processingTime", _stopWatch.Elapsed.TotalMilliseconds);
            _activity.Dispose();
            _isDisposed = true;
        }

        private static string GetTypeName(MethodBase method)
        {
            string name = method.DeclaringType?.Name;
            if (Regex.IsMatch(name ?? "", @"^[a-zA-Z]+$"))
            {
                return name;
            }

            var type = method.ReflectedType;
            while (type != null && !Regex.IsMatch(type.Name, @"^[a-zA-Z]+$"))
            {
                type = type.ReflectedType;
            }
            return type?.Name ?? "";
        }

    }
}