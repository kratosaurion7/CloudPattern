using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.BusinessRules
{
    public class GenericRule<T> : BusinessRule<T>
    {
        public string RuleName { get; private set; }

        private Predicate<T> CheckCondition;

        public GenericRule(Predicate<T> checkCondition)
        {
            CheckCondition = checkCondition;

            ContinueProcessingOtherRules = true;
        }

        public GenericRule(string ruleName, Predicate<T> checkCondition)
        {
            RuleName = ruleName;
            CheckCondition = checkCondition;

            ContinueProcessingOtherRules = true;
        }

        public override bool Process(T businessObject)
        {
            return CheckCondition(businessObject);
        }
    }
}
