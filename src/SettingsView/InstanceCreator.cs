using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Jakar.SettingsView.Shared
{
	public static class InstanceCreator<T1, T2, T3, TInstance>
	{
		public static Func<T1, T2, T3, TInstance> Create { get; } = CreateInstance();

		private static Func<T1, T2, T3, TInstance> CreateInstance()
		{
			Type[] argsTypes =
			{
				typeof(T1),
				typeof(T2),
				typeof(T3)
			};
			ConstructorInfo constructor = typeof(TInstance).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder, argsTypes, null) ?? throw new NullReferenceException(nameof(constructor));
			ParameterExpression[] args = argsTypes.Select(Expression.Parameter).ToArray();
			return Expression.Lambda<Func<T1, T2, T3, TInstance>>(Expression.New(constructor, args), args).Compile();
		}
	}

	public static class InstanceCreator<T1, T2, TInstance>
	{
		public static Func<T1, T2, TInstance> Create { get; } = CreateInstance();

		private static Func<T1, T2, TInstance> CreateInstance()
		{
			Type[] argsTypes =
			{
				typeof(T1),
				typeof(T2)
			};
			ConstructorInfo constructor = typeof(TInstance).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder, argsTypes, null) ?? throw new NullReferenceException(nameof(constructor));
			ParameterExpression[] args = argsTypes.Select(Expression.Parameter).ToArray();
			return Expression.Lambda<Func<T1, T2, TInstance>>(Expression.New(constructor, args), args).Compile();
		}
	}

	public static class InstanceCreator<T1, TInstance>
	{
		public static Func<T1, TInstance> Create { get; } = CreateInstance();

		private static Func<T1, TInstance> CreateInstance()
		{
			Type[] argsTypes =
			{
				typeof(T1)
			};
			ConstructorInfo constructor = typeof(TInstance).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder, argsTypes, null) ?? throw new NullReferenceException(nameof(constructor));
			ParameterExpression[] args = argsTypes.Select(Expression.Parameter).ToArray();
			return Expression.Lambda<Func<T1, TInstance>>(Expression.New(constructor, args), args).Compile();
		}
	}
}