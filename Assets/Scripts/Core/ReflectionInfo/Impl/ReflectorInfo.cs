using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Attributes;
using Core.Commands.Binding;
using UnityEngine;

namespace Core.ReflectionInfo.Impl
{
    public sealed class ReflectorInfo : IReflectorInfo
    {
        public List<PropertyInfo> Injects { get; private set; }
        public List<MethodInfo> PostConstructs { get; private set; }
        public List<MethodInfo> PreDestroys { get; private set; }

        public void Build(Type type)
        {
            Injects = GetInjects(type);
            PostConstructs = GetMethods<PostConstruct>(type);
            PreDestroys = GetMethods<PreDestroy>(type);
        }

        private static List<PropertyInfo> GetInjects(IReflect type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return properties.Where(propertyInfo => propertyInfo.GetCustomAttributes(typeof(Inject), true).Length > 0).ToList();
        }

        private static List<MethodInfo> GetMethods<T>(IReflect type) where T : Attribute
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            return methods.Where(methodInfo => methodInfo.GetCustomAttributes(typeof(T), true).Length > 0).ToList();
        }
    }
}
