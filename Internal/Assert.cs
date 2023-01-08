using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.Internal
{
    internal class Assert
    {
        [DebuggerHidden]
        public static void ThrowIfNull([NotNull] object? value, [CallerArgumentExpression("value")] string? paramName = null)
        {
            if (value is null)
            {
                throw new ArgumentNullException(paramName, "must not be null");
            }
        }

        [DebuggerHidden]
        public static void ThrowIfNotEqual<TValue>(TValue expected, TValue value)
        {
            if (expected is null && value is null)
            {
                return;
            }
            if (expected?.Equals(value) != true)
            {
                throw new ArgumentNullException($"value {value} must match expected {expected}");
            }
        }

        [DebuggerHidden]
        public static void ThrowIfEqual<TValue>(TValue expected, TValue value)
        {
            if (expected is null && value is null)
            {
                throw new ArgumentNullException($"value {value} must not match expected {expected}");
            }
            if (expected?.Equals(value) == true)
            {
                throw new ArgumentNullException($"value {value} must not match expected {expected}");
            }
        }
    }
}
