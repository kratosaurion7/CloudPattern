using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudPatterns.Retry
{
    public class Retry
    {
        public async Task<T> RetryFunc<T>(Func<T> func, int intervalTime, int totalTimeout)
        {
            DateTime timeoutLimit = DateTime.Now.Add(TimeSpan.FromMilliseconds(totalTimeout));

            while(true)
            {
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken x = source.Token;

                Task<T> wanted = new Task<T>(func, x);

                source.CancelAfter(TimeSpan.FromMilliseconds(intervalTime));
                var result = await wanted;

                if (wanted.Status == TaskStatus.RanToCompletion)
                {
                    return result;
                }

                if (DateTime.Now > timeoutLimit)
                {
                    return default(T);
                }

                source.Dispose();
            }
        }
    }
}
