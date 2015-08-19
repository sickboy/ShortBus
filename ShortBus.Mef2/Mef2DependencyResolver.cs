// <copyright company="SIX Networks GmbH">
//     Copyright (c) SIX Networks GmbH. All rights reserved. Do not remove this notice.
// </copyright>

using System;
using System.Collections.Generic;
using System.Composition;

namespace ShortBus.Mef
{
    public class MefDependencyResolver : IDependencyResolver
    {
        readonly CompositionContext _container;

        public MefDependencyResolver(CompositionContext container) {
            _container = container;
        }

        public object GetInstance(Type type) {
            return _container.GetExport(type);
        }

        public IEnumerable<T> GetInstances<T>() where T : class
        {
            return _container.GetExports<T>();
        }
    }
}