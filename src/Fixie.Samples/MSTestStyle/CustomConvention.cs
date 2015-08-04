using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fixie.Samples.MSTestStyle
{
    public class CustomConvention : Convention
    {
        public CustomConvention()
        {
            Classes
                .HasOrInherits<TestClassAttribute>();

            Methods
                .HasOrInherits<TestMethodAttribute>();

            ClassExecution
                .CreateInstancePerCase();

            FixtureExecution
                .Wrap<FixtureSetUpTearDown>();

            CaseExecution
                .Wrap<SetUpTearDown>();
        }
    }

    class SetUpTearDown : CaseBehavior
    {
        public void Execute(Case @case, Action next)
        {
            @case.Class.InvokeAll<TestInitializeAttribute>(@case.Fixture.Instance);
            next();
            @case.Class.InvokeAll<TestCleanUpAttribute>(@case.Fixture.Instance);
        }
    }

    class FixtureSetUpTearDown : FixtureBehavior
    {
        public void Execute(Fixture fixture, Action next)
        {
            fixture.Class.Type.InvokeAll<ClassInitializeAttribute>(fixture.Instance);
            next();
            fixture.Class.Type.InvokeAll<ClassCleanUpAttribute>(fixture.Instance);
        }
    }

    public static class BehaviorBuilderExtensions
    {
        public static void InvokeAll<TAttribute>(this Type type, object instance)
            where TAttribute : Attribute
        {
            foreach (var method in Has<TAttribute>(type))
            {
                try
                {
                    method.Invoke(instance, null);
                }
                catch (TargetInvocationException exception)
                {
                    throw new PreservedException(exception.InnerException);
                }
            }
        }

        static IEnumerable<MethodInfo> Has<TAttribute>(Type type) where TAttribute : Attribute
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.HasOrInherits<TAttribute>());
        }
    }
}
