using System;
using System.ComponentModel;
using System.Resources;
using System.Runtime.CompilerServices;

namespace WorkManager.Core
{
    public class LocalizeAttribute : DescriptionAttribute
    {
        private ResourceManager _resourceManager;
        private readonly Type _callerType;
        private readonly string _resourceKey;

        public LocalizeAttribute(Type callerType, Type resourceType, string resourceKey)
        {
            _resourceManager = new ResourceManager(resourceType);
            _callerType = callerType;
            _resourceKey = resourceKey;
        }

        public override string Description {
            get
            {
                string desc = _resourceManager.GetString(_callerType.Name+"_"+_resourceKey);
                return string.IsNullOrWhiteSpace(desc) ? $"[No Resource {_resourceKey}]" : desc;
            }
        }
    }
}