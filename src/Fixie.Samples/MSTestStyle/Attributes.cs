using System;

namespace Fixie.Samples.MSTestStyle
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TestClassAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class ClassInitializeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class TestInitializeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class TestMethodAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class TestCleanUpAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class ClassCleanUpAttribute : Attribute { }
}
