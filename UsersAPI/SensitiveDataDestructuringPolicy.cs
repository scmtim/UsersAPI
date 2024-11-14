using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Serilog.Events;
using System.Reflection;

namespace Microsoft.Extensions.Logging;

public class SensitiveDataDestructuringPolicy : IDestructuringPolicy
{
    private const string DEFAULT_MASK_VALUE = "******";
    
    public SensitiveDataDestructuringPolicy()
    {
    }

    public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
    {
        var sensitiveKeywords = new string[]{ "Email" };
        var maskValue = DEFAULT_MASK_VALUE;

        if (!sensitiveKeywords.Any())
        {
            result = new StructureValue(new LogEventProperty[] { });

            return false;
        }

        var props = value.GetType().GetTypeInfo().DeclaredProperties;
        var logEventProperties = new List<LogEventProperty>();

        foreach (var propertyInfo in props)
        {
            if (sensitiveKeywords.Any(x => x.Equals(propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                logEventProperties.Add(new LogEventProperty(propertyInfo.Name, propertyValueFactory.CreatePropertyValue(maskValue)));
            }
            else
            {
                logEventProperties.Add(new LogEventProperty(propertyInfo.Name, propertyValueFactory.CreatePropertyValue(propertyInfo.GetValue(value))));
            }
        }

        result = new StructureValue(logEventProperties);

        return true;
    }
}