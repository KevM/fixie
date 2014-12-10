﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Fixie.Discovery;

namespace Fixie
{
    public class DiscoveryProxy : MarshalByRefObject
    {
        public IReadOnlyList<MethodGroup> TestMethodGroups(string assemblyFullPath)
        {
            var assembly = Assembly.Load(AssemblyName.GetAssemblyName(assemblyFullPath));
            var runContext = new RunContext(assembly, new Lookup());
            var conventions = new ConventionDiscoverer(runContext).GetConventions();

            var discoveredTestMethodGroups = new List<MethodGroup>();

            foreach (var convention in conventions)
            {
                var classDiscoverer = new ClassDiscoverer(convention.Config);
                var candidateTypes = assembly.GetTypes();
                var testClasses = classDiscoverer.TestClasses(candidateTypes);

                var methodDiscoverer = new MethodDiscoverer(convention.Config);
                foreach (var testClass in testClasses)
                {
                    var distinctMethodGroups = new Dictionary<string, MethodGroup>();

                    foreach (var testMethod in methodDiscoverer.TestMethods(testClass))
                    {
                        var methodGroup = new MethodGroup(testMethod);

                        distinctMethodGroups[methodGroup.FullName] = methodGroup;
                    }

                    discoveredTestMethodGroups.AddRange(distinctMethodGroups.Values);
                }
            }

            return discoveredTestMethodGroups;
        }
    }
}