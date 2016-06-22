using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudPatterns.Retry
{
    /// <summary>
    /// Class that handles a single operation that can be re-tried every X miliseconds until a limit timeout has been hit.
    /// 
    /// This class is used to be triggered only once, if it should be launched a second time a new object should be recreated.
    /// </summary>
    /// <typeparam name="T">Generic type</typeparam>
    public class Retry<T>
    {
        private Func<T> FunctionToCall;
        private int IntervalTime;
        private int TotalTimeout;

        private CancellationTokenSource MyCancellingSource;
        private CancellationToken CurrentOperationCancelToken;

        /// <summary>
        /// Constructor to build the object in a runnable state.
        /// </summary>
        /// <param name="func">Function to call and retry</param>
        /// <param name="intervalTime">Span of time during which an operation will be awaited for.</param>
        /// <param name="totalTimeout">Duration after which the whole Retry operation will be stopped and cancelled.</param>
        public Retry(Func<T> func, int intervalTime, int totalTimeout)
        {
            FunctionToCall = func;
            IntervalTime = intervalTime;
            TotalTimeout = totalTimeout;

            MyCancellingSource = new CancellationTokenSource();
            CurrentOperationCancelToken = MyCancellingSource.Token;
        }

        /// <summary>
        /// Start the operation.
        /// </summary>
        /// <remarks>This operation should be started asynchronously.</remarks>
        /// <returns>Returns a task that can be awaited for results.</returns>
        public async Task<T> Start()
        {
            DateTime timeoutLimit = DateTime.Now.Add(TimeSpan.FromMilliseconds(TotalTimeout));

            while(CurrentOperationCancelToken.IsCancellationRequested == false)
            {
                CancellationTokenSource currentIntervalSource = new CancellationTokenSource();
                CancellationToken currentIntervalToken = currentIntervalSource.Token;

                Task<T> wanted = new Task<T>(FunctionToCall, currentIntervalToken);

                // Start a cancel until the next interval starts
                currentIntervalSource.CancelAfter(TimeSpan.FromMilliseconds(IntervalTime));

                var result = await wanted; // Start the operation and block until it's done.

                if (wanted.Status == TaskStatus.RanToCompletion)
                {
                    return result;
                }

                // Not sure which to use, check timeout expiration or if it was cancelled.
                if (DateTime.Now > timeoutLimit || currentIntervalToken.IsCancellationRequested)
                {
                    return default(T);
                }

                currentIntervalSource.Dispose();
            }

            return default(T); // Cancelled
        }

        /// <summary>
        /// Cancel the current running operation.
        /// </summary>
        public void Cancel()
        {
            MyCancellingSource.Cancel();
        }
    }
}
