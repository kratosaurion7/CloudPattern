using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.BusinessRules
{
    public class GenericRule<T> : BusinessRule<T>
    {
        private Predicate<T> CheckCondition;

        public GenericRule(Predicate<T> checkCondition)
        {
            CheckCondition = checkCondition;
        }

        public override bool Process(T businessObject)
        {
            return CheckCondition(businessObject);
        }
    }
}
