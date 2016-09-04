﻿namespace Dixin.Linq.CSharp
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    internal partial class Data
    {
        internal int value;

        static Data()
        {
            Trace.WriteLine(MethodBase.GetCurrentMethod().Name); // .cctor
        }

        internal Data(int value)
        {
            Trace.WriteLine(MethodBase.GetCurrentMethod().Name); // .ctor
            this.value = value;
        }

        internal int Value
        {
            get { return this.value; }
        }
    }

    internal partial class Data
    {
        public static Data operator +(Data value1, Data value2)
        {
            Trace.WriteLine(MethodBase.GetCurrentMethod().Name); // op_Addition
            return new Data(value1.value + value2.value);
        }

        public static explicit operator int(Data value)
        {
            Trace.WriteLine(MethodBase.GetCurrentMethod().Name); // op_Explicit
            return value.value;
        }

        public static implicit operator Data(int value)
        {
            Trace.WriteLine(MethodBase.GetCurrentMethod().Name); // op_Implicit
            return new Data(value);
        }
    }

    internal partial class CompiledData
    {
        public static Data op_Addition(Data value1, Data value2)
        {
            Trace.WriteLine(MethodBase.GetCurrentMethod().Name);
            return new Data(value1.value + value2.value);
        }

        public static int op_Explicit(Data value)
        {
            Trace.WriteLine(MethodBase.GetCurrentMethod().Name);
            return value.value;
        }

        public static Data op_Implicit(int value)
        {
            Trace.WriteLine(MethodBase.GetCurrentMethod().Name);
            return new Data(value);
        }
    }

    internal static partial class Functions
    {
        internal static void Operators(Data value1, Data value2)
        {
            Data result = value1 + value2; // Compiled to Data.op_Addition(value1, value2).
            int int32 = (int)value1; // Compiled to Data.op_Explicit(value1).
            Data data = 1; // Compiled to Data.op_Implicit(1).
        }
    }

    internal partial class Device
    {
        private string description;

        internal string Description
        {
            get
            {
                Trace.WriteLine(MethodBase.GetCurrentMethod().Name); // get_Description
                return this.description;
            }
            set
            {
                Trace.WriteLine(MethodBase.GetCurrentMethod().Name); // set_Description
                this.description = value;
            }
        }
    }

#if DEMO
    internal partial class CompiledDevice
    {
        private string description;

        internal string Description
        {
            get { return this.get_Description(); }
            set { this.set_Description(value); }
        }

        internal string get_Description() // Body of Description's getter.
        {
            Trace.WriteLine(MethodBase.GetCurrentMethod().Name); // get_Description
            return this.description;
        }

        internal void set_Description(string value) // Body of Description's setter.
        {
            Trace.WriteLine(MethodBase.GetCurrentMethod().Name); // set_Description
            this.description = value;
        }
    }
#endif

    internal static partial class Functions
    {
        internal static void Property(Device device)
        {
            string description = device.Description; // Compiled to device.get_Description).
            device.Description = string.Empty; // Compiled to device.set_Description(string.Empty).
        }
    }

    internal partial class Category
    {
        private readonly Uri[] links;

        internal Category(Uri[] links)
        {
            this.links = links;
        }

        internal Uri this[int index]
        {
            get
            {
                Trace.WriteLine(MethodBase.GetCurrentMethod().Name); // get_Item
                return this.links[index];
            }
            set
            {
                Trace.WriteLine(MethodBase.GetCurrentMethod().Name); // set_Item
                this.links[index] = value;
            }
        }
    }

#if DEMO
    internal partial class CompiledCategory
    {
        private readonly Uri[] links;

        internal Category(Uri[] links)
        {
            this.links = links;
        }

        internal Uri this[int index]
        {
            get { return this.get_Item(index); }
            set { this.set_Item(index, value); }
        }

        internal Uri get_Item(int index) // Body of indexer's getter.
        {
            Trace.WriteLine(MethodBase.GetCurrentMethod().Name); // get_Item
            return this.links[index];
        }

        internal Uri set_Item(int index, Uri value) // Body of indexer's setter.
        {
            Trace.WriteLine(MethodBase.GetCurrentMethod().Name); // set_Item
            this.links[index] = value;
        }
    }
#endif

    internal static partial class Functions
    {
        internal static void Indexer(Category category, Uri newLink)
        {
            Uri link = category[0]; // Compiled to device.get_Item(0).
            category[0] = newLink; // Compiled to device.set_Item(0, uri).
        }
    }

    internal partial class Functions
    {
        internal static void TraceString(Uri uri, FileInfo file, Assembly assembly)
        {
            Trace.WriteLine(uri == null ? string.Empty : uri.ToString());
            Trace.WriteLine(file == null ? string.Empty : file.ToString());
            Trace.WriteLine(assembly == null ? string.Empty : assembly.ToString());
        }
    }

    internal partial class Functions
    {
        internal static void TraceObject(Uri uri, FileInfo file, Assembly assembly)
        {
            Trace.WriteLine(uri);
            Trace.WriteLine(file);
            Trace.WriteLine(assembly);
        }

        internal static void SwapInt32(ref int value1, ref int value2)
        {
            int copyOfValue1 = value1;
            value1 = value2;
            value2 = copyOfValue1;
        }

        internal static void Swap<T>(ref T value1, ref T value2)
        {
            T copyOfValue1 = value1;
            value1 = value2;
            value2 = copyOfValue1;
        }

        internal static IStack<T> PushValue<T>(IStack<T> stack) where T : new()
        {
            stack.Push(new T());
            return stack;
        }

        internal static void ArgumentTypeInference(string value1, string value2)
        {
            Swap<string>(ref value1, ref value2);
            Swap(ref value1, ref value2);
        }

        internal static T Generic1<T>(T value)
        {
            Trace.WriteLine(value);
            return default(T);
        }

        internal static TResult Generic2<T, TResult>(T value)
        {
            Trace.WriteLine(value);
            return default(TResult);
        }

        internal static void ReturnTypeInference()
        {
            int value2 = Generic1(0);
            string value3 = Generic2<int, string>(0); // Generic2<int>(0) cannot be compiled.
        }

        internal static void NullArgumentType()
        {
            Generic1<FileInfo>(null);
            Generic1((FileInfo)null);
            FileInfo file = null;
            Generic1(file);
        }

        internal static void TupleConstructor()
        {
            Trace.WriteLine(new Tuple<bool, int, decimal, string>(true, int.MaxValue, decimal.MaxValue, string.Empty));
        }

        internal static void TupleCreate()
        {
            Trace.WriteLine(Tuple.Create(true, int.MaxValue, decimal.MaxValue, string.Empty));
        }
    }

    internal partial class Data
    {
        internal long InstanceAdd(int value1, long value2)
        {
            return this.value + value1 + value2;
        }

        internal static long StaticAdd(Data @this, int value1, long value2)
        {
            return @this.value + value1 + value2;
        }
    }

    internal partial class Data
    {
        internal long CompiledInstanceAdd(int value1, long value2)
        {
            Data arg1 = this;
            int arg2 = value1;
            long arg3 = value2;
            return this.value + value1 + value2;
        }

        internal static long CompiledStaticAdd(Data @this, int value1, long value2)
        {
            Data arg1 = @this;
            int arg2 = value1;
            long arg3 = value2;
            return @this.value + value1 + value2;
        }
    }

    internal static partial class DataExtensions
    {
        internal static long ExtensionAdd(this Data @this, int value1, long value2)
        {
            return @this.value + value1 + value2;
        }
    }

    internal static partial class Functions
    {
        internal static void CallExtensionMethod(Data data)
        {
            long result = data.ExtensionAdd(1, 2L);
        }
    }

#if DEMO
    internal static partial class DataExtensions
    {
        [Extension]
        internal static long CompiledExtensionAdd(Data @this, int value1, long value2)
        {
            return @this.value + value1 + value2;
        }
    }
#endif

    internal static partial class Functions
    {
        internal static void CompiledCallExtensionMethod(Data data)
        {
            long result = DataExtensions.ExtensionAdd(data, 1, 2L);
        }
    }

    internal partial class Data
    {
        internal bool Equals(Data other)
        {
            return this.value == other.value;
        }
    }

    internal static partial class DataExtensions
    {
        internal static bool Equals(Data @this, Data other)
        {
            return @this.Value == other.Value;
        }
    }

    internal static partial class Functions
    {
        internal static void CallMethods(Data value1, Data value2)
        {
            bool result1 = value1.Equals(value2); // Data.Equals.
            bool result2 = DataExtensions.Equals(value1, value2); // DataExtensions.Equals.
        }
    }

    internal static class DayOfWeekExtensions
    {
        internal static bool IsWeekend(this DayOfWeek dayOfWeek)
        {
            return dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday;
        }
    }
    internal static partial class Functions
    {
        internal static void CallEnumerationExtensionMethod(DayOfWeek dayOfWeek)
        {
            bool result = dayOfWeek.IsWeekend();
        }
    }

    internal partial class Data
    {
        internal bool InstanceEqual(Data other) => this.value == other.value;

        internal static bool StaticEqual(Data @this, Data other) => @this.value == other.value;
    }

    internal static partial class DataExtensions
    {
        internal static bool ExtensionEquals(Data @this, Data other) => @this.Value == other.Value;
    }

    internal partial class Data
    {
        internal void TraceLine() => Trace.WriteLine(this.value);
    }

#if DEMO
    internal partial class Data
    {
        public static Data operator +(Data value1, Data value2) => new Data(value1.value + value2.value); // op_Addition method.

        public static explicit operator int(Data value) => value.value; // op_Explicit method.

        public static implicit operator Data(int value) => new Data(value); // op_Implicit method.
    }

    internal partial class Device
    {
        internal decimal HalfPrice
        {
            get { return this.Price / 2; }
        }
    }
#endif

    internal partial class Device
    {
        internal decimal HalfPrice => this.Price / 2; // get_HalfPrice method.
    }

#if DEMO
    internal partial class Category
    {
        private readonly Uri[] links;

        internal Category(Uri[] links)
        {
            this.links = links;
        }

        internal Uri this[int index] => this.links[index]; // get_Item method.
    }

    [Table(Name = "Production.Product")]
    public partial class Product : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public Product()
        {
            this.OnCreated();
        }

        partial void OnCreated();

        // Other members.
    }

    public partial class Product
    {
        partial void OnCreated()
        {
            Trace.WriteLine($"{nameof(Product)} is created.");
        }
    }
#endif
}

namespace Dixin.Linq.CSharp
{
    using static System.DayOfWeek;
    using static System.Math;
    using static System.Diagnostics.Trace;

    internal static partial class Functions
    {
        internal static void UsingStatic(int value1, int value2)
        {
            int result = Max(value1, value2); // Compiled to Math.Max(value1, value2).
            WriteLine(Monday); // Compiled to Trace.WriteLine(DayOfWeek.Monday).
        }
    }
}

#if DEMO
namespace System.Reflection
{
    using System.Runtime.InteropServices;

	[ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_MethodBase))]
    [ComVisible(true)]
    public abstract class MethodBase : MemberInfo, _MethodBase
    {
        public bool IsAbstract { get; }

        public bool IsPrivate { get; }

        public bool IsPublic { get; }

        public bool IsStatic { get; }

        public bool IsVirtual { get; }

        // public abstract string Name { get; }

        public static MethodBase GetCurrentMethod();

        public virtual MethodBody GetMethodBody();

        public abstract ParameterInfo[] GetParameters();

        public object Invoke(object obj, object[] parameters);
    }
}

namespace System.Diagnostics
{
    public sealed class Trace
    {
        [Conditional("TRACE")]
        public static void WriteLine(string message);

        [Conditional("TRACE")]
        public static void WriteLine(object value);
    }
}

namespace System
{
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

	[Serializable]
	[StructLayout(LayoutKind.Auto)]
    public struct DateTime : IComparable, IFormattable, IConvertible, ISerializable, IComparable<DateTime>, IEquatable<DateTime>
    {
        public DateTime(long ticks);

        public DateTime(int year, int month, int day);

        public DateTime(int year, int month, int day, int hour, int minute, int second);

        public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond);

        // Other constructor overloads.
    }
}

namespace System.Data
{
    using System.Reflection;

    [DefaultMember("Item")]
    public class DataRow
    {
        public object this[DataColumn column] { get; set; }

        public object this[string columnName] { get; set; }

        public object this[int columnIndex] { get; set; }

        // Other indexer overloads.
    }
}

namespace System
{
    using System.Collections;

    internal interface ITuple
    {
    }

    [Serializable]
    public class Tuple<T1, T2, T3, T4> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
    {
        private readonly T1 m_Item1;

        private readonly T2 m_Item2;

        private readonly T3 m_Item3;

        private readonly T4 m_Item4;

        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            this.m_Item1 = item1;
            this.m_Item2 = item2;
            this.m_Item3 = item3;
            this.m_Item4 = item4;
        }

        // Other members.
    }
}

namespace System
{
    public static class Tuple
    {
        public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
        }
    }
}

namespace System.Linq
{
    using System.Collections.Generic;

    public static class Enumerable
    {
        public static IEnumerable<TSource> Where<TSource>(
            this IEnumerable<TSource> source, Func<TSource, bool> predicate);

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);

        public static IEnumerable<TResult> Select<TSource, TResult>(
            this IEnumerable<TSource> source, Func<TSource, TResult> selector);
    }
}
#endif