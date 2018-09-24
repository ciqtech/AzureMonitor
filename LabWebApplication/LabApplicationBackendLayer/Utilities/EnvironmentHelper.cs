using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabApplicationBackendLayer.Utilities
{
    public static class EnvironmentHelper
    {
        public const string ENV_BACKEND = "BACKEND_DB";
        public const string ENV_KV_URL = "KV_URL";
        private static Dictionary<string, string> _arguments;
        private const char argumentSeparator = ' ';

        public static Dictionary<string, string> Arguments
        {
            get
            {
                bool argumentsExist = _arguments != null && _arguments.Any();

                if (!argumentsExist)
                {
                    IDictionary environmentVariables = Environment.GetEnvironmentVariables();

                    _arguments = new Dictionary<string, string>();

                    if (environmentVariables.Contains(ENV_BACKEND))
                    {
                        var argumentsHolder = environmentVariables[ENV_BACKEND] as string;
                        _arguments.Add(ENV_BACKEND, argumentsHolder?.Split(argumentSeparator)[0]);
                    }

                    if (environmentVariables.Contains(ENV_KV_URL))
                    {
                        var argumentsHolder = environmentVariables[ENV_KV_URL] as string;
                        _arguments.Add(ENV_KV_URL, argumentsHolder?.Split(argumentSeparator)[0]);
                    }
                }

                return _arguments;
            }
        }
    }
}
