﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="WebOS - http://www.coolwebos.com">
//   Copyright © Dixin 2010 http://weblogs.asp.net/dixin
// </copyright>
// <summary>
//   Defines the TypeExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dixin.Reflection
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

    public static partial class TypeExtensions
    {
        #region Methods

        internal static FieldInfo GetBaseField(this Type type, string name)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            Type @base = type.BaseType;
            if (@base == null)
            {
                return null;
            }

            return @base.GetTypeField(name) ?? @base.GetBaseField(name);
        }

        internal static PropertyInfo GetBaseIndex(this Type type, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            Type @base = type.BaseType;
            if (@base == null)
            {
                return null;
            }

            return @base.GetTypeIndex(args) ?? @base.GetBaseIndex(args);
        }

        internal static MethodInfo GetBaseMethod(this Type type, string name, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            Type @base = type.BaseType;
            if (@base == null)
            {
                return null;
            }

            return @base.GetTypeMethod(name, args) ?? @base.GetBaseMethod(name, args);
        }

        internal static PropertyInfo GetBaseProperty(this Type type, string name)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            Type @base = type.BaseType;
            if (@base == null)
            {
                return null;
            }

            return @base.GetTypeProperty(name) ?? @base.GetBaseProperty(name);
        }

        internal static MethodInfo GetInterfaceMethod(this Type type, string name, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            return type.GetInterfaces()
                .Select(type.GetInterfaceMap)
                .SelectMany(mapping => mapping.TargetMethods)
                .FirstOrDefault(
                    method =>
                    method.Name.Split('.').Last().Equals(name, StringComparison.Ordinal) &&
                    method.GetParameters().Count() == args.Length &&
                    method.GetParameters().Select((parameter, index) =>
                        parameter.ParameterType.IsInstanceOfType(args[index])).Aggregate(true, (a, b) => a && b));
        }

        internal static FieldInfo GetTypeField(this Type type, string name)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            return type
                .GetRuntimeFields()
                .FirstOrDefault(field => field.Name.Equals(name, StringComparison.Ordinal));
        }

        internal static PropertyInfo GetTypeIndex(this Type type, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            return type
                .GetRuntimeProperties()
                .FirstOrDefault(
                    property =>
                    property.GetIndexParameters().Any() &&
                    property.GetIndexParameters().Select(
                        (parameter, index) => parameter.ParameterType == args[index].GetType()).Aggregate(
                            true, (a, b) => a && b));
        }

        internal static MethodInfo GetTypeMethod(this Type type, string name, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            return type
                .GetRuntimeMethods()
                .FirstOrDefault(method =>
                    method.Name.Equals(name, StringComparison.Ordinal)
                    && method.GetParameters().Count() == args.Length
                    && method
                        .GetParameters()
                        .Select((parameter, index) =>
                                parameter.ParameterType == (args[index] != null ? args[index].GetType() : typeof(object)))
                        .All(match => true));
        }

        internal static PropertyInfo GetTypeProperty(this Type type, string name)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            return type
                .GetRuntimeProperties()
                .FirstOrDefault(property => property.Name.Equals(name, StringComparison.Ordinal));
        }

        #endregion
    }

    public static partial class TypeExtensions
    {
        public static MemberInfo[] GetPublicDeclaredMembers(this Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            return type.GetMembers(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }
    }

    public static partial class TypeExtensions
    {
        public static MemberInfo[] GetPublicMembers(this Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            return type.GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        }
    }

    public static partial class TypeExtensions
    {
        public static bool IsAssignableTo(this Type from, Type to)
        {
            Contract.Requires<ArgumentNullException>(to != null);
            Contract.Requires<ArgumentNullException>(from != null);

            if (to.IsAssignableFrom(from))
            {
                return true;
            }

            if (!to.IsGenericTypeDefinition)
            {
                return false;
            }

            if (from.IsGenericType && from.GetGenericTypeDefinition() == to)
            {
                return true; // Collection<int> is assignable to Collection<>.
            }

            if (to.IsInterface && from.GetInterfaces().Any(
                @interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == to))
            {
                return true; // Collection<>/Collection<int> assignable to IEnumerable<>/ICollection<>.
            }

            Type baseOfFrom = from.BaseType;
            return baseOfFrom != null && IsAssignableTo(baseOfFrom, to);
        }
    }

    public static partial class TypeExtensions
    {
        public static TValue GetField<TValue>(this object @object, string name)
        {
            Contract.Requires<ArgumentNullException>(@object != null);

            return (TValue)@object.GetType().GetTypeField(name).GetValue(@object);
        }

        public static void SetField(this object @object, string name, object value)
        {
            Contract.Requires<ArgumentNullException>(@object != null);

            @object.GetType().GetTypeField(name).SetValue(@object, value);
        }

        public static TValue GetProperty<TValue>(this object @object, string name)
        {
            Contract.Requires<ArgumentNullException>(@object != null);

            return (TValue)@object.GetType().GetTypeProperty(name).GetValue(@object);
        }

        public static void SetProperty(this object @object, string name, object value)
        {
            Contract.Requires<ArgumentNullException>(@object != null);

            @object.GetType().GetTypeProperty(name).SetValue(@object, value);
        }
    }
}