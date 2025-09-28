using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Logging;

public class PrefixedLogger
{
    private readonly ManualLogSource _baseLogger;
    private string prefix;

    public PrefixedLogger(ManualLogSource baseLogger, object owner)
    {
        _baseLogger = baseLogger;
        prefix = $"{owner.GetType().Name}: ";
    }

    public PrefixedLogger(ManualLogSource baseLogger, Type ownerType)
    {
        _baseLogger = baseLogger;
        prefix = $"{ownerType.Name}: ";
    }

    public void LogMessage(string message) => _baseLogger.LogMessage(prefix + message);
    public void LogInfo(string message) => _baseLogger.LogInfo(prefix + message);
    public void LogDebug(string message) => _baseLogger.LogDebug(prefix + message);
    public void LogWarning(string message) => _baseLogger.LogWarning(prefix + message);
    public void LogError(string message) => _baseLogger.LogError(prefix + message);
}