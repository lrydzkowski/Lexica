using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Words.Models
{
    public class SetInfo
    {
        public long? SetId { get; }

        public SetPath Path { get; }

        public SetInfo(long? setId, SetPath path)
        {
            SetId = setId;
            Path = path;
        }
    }
}
