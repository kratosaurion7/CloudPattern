using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.BusinessRules
{
    public abstract class BusinessRule<T>
    {
        public bool ContinueProcessingOtherRules { get; set; }

        public abstract bool Process(T businessObject);
    }
}
