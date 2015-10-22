﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public class ComponentMatchingFamily<T> : IFamily<T>
    {
        private List<T> _nodes;

        public ComponentMatchingFamily()
        {
            _nodes = new List<T>();
        }

        public IEnumerable<T> Nodes
        {
            get { return _nodes; }
        }
    }
}
