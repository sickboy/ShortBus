// <copyright company="SIX Networks GmbH">
//     Copyright (c) SIX Networks GmbH. All rights reserved. Do not remove this notice.
// </copyright>

using System;
using System.Collections.Generic;
using SimpleInjector;

namespace ShortBus.SimpleInjector
{
    public class SimpleInjectorDependencyResolver : IDependencyResolver
    {
        readonly Container _container;

        public SimpleInjectorDependencyResolver(Container container) {
            _container = container;
        }

        public object GetInstance(Type type) {
            return _container.GetInstance(type);
        }

        public IEnumerable<T> GetInstances<T>() where T : class {
            return _container.GetAllInstances<T>();
        }
    }
}