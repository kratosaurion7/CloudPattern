using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.BusinessRules
{
    public class BusinessRulesRepository<T> // T needs to be a common object root
    {
        public List<BusinessRule<T>> RuleSet;

        public BusinessRulesRepository()
        {
            RuleSet = new List<BusinessRule<T>>();
        }

        public bool Apply(T objectTest)
        {
            bool completeResult = true;

            foreach (var item in RuleSet)
            {
                bool testResult = item.Process(objectTest);

                if (item.ContinueProcessingOtherRules == false)
                {
                    return testResult;
                }

                completeResult = completeResult && testResult;
            }

            return completeResult;
        }

        public IEnumerable<BusinessRule<T>> GetPassedRules(T objectTest)
        {
            return RuleSet.Where(p => p.Process(objectTest) == true);
        }

        public IEnumerable<BusinessRule<T>> GetFailedRules(T objectTest)
        {
            return RuleSet.Where(p => p.Process(objectTest) == false);
        }
    }
}
