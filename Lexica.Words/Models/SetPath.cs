using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Words.Models
{
    public class SetPath
    {
        public string Namespace { get; }

        public string Name { get; }

        public SetPath(string setNamespace, string name)
        {
            Namespace = setNamespace;
            Name = name;
        }
    }
}
