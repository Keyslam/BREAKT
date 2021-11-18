using System.Runtime.CompilerServices;

public interface ILogger {
	LogLevel GetLogLevel();
	void SetLogLevel(LogLevel logLevel);

	void Log(LogLevel logLevel, string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null);
	void LogIf(LogLevel logLevel, bool expression, string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null);

	void Flush();
}