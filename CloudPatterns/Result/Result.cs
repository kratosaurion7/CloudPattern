using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.Result
{
    /// <summary>
    /// Utility class that can be used to return a single object used to represent multiple states of 
    /// success.
    /// </summary>
    /// <typeparam name="T_RESULT">Return type of a successful value.</typeparam>
    /// <typeparam name="T_ERROR">Type of the error if the operation failed.</typeparam>
    public class Result<T_RESULT, T_ERROR> 
        where T_RESULT : class
        where T_ERROR : System.Exception
    {
        /* 3 types of state
         * 
         * SUCCESS
         * 
         * A result of type SUCCESS means that the function was executed correctly and returned a correct value
         * 
         * FAILED (soft)
         * 
         * A FAILED result means that the function returned an invalid result but still worked in a predictable way. This is 
         * usualy the case when doing disk IO for example. A function would return FAILED when the file does not exist but if
         * the function throws an IOException that would mean a status of ERROR
         * 
         * ERROR (hard)
         * 
         * Indicate that the operation crashed or ended unexpectedly. Nothing can be gleaned from the result of such an operation.
         * The resulting exception is passed into the Error member if caught.
         * 
         * */

        public enum RESULT_STATE
        {
            SUCCESS,
            FAILED,
            ERROR
        }

        // One of them must be null and the other must be set
        public T_RESULT Value { get; set; }
        public T_ERROR Error { get; set; }
        public RESULT_STATE ResultStatus { get; private set; }

        /// <summary>
        /// Create a Result object with the Value and Error field unpopulated. Only a single one should be assigned
        /// and the other should be a default(T).
        /// </summary>
        public Result()
        {
            Value = default(T_RESULT);
            Error = default(T_ERROR);
        }

        /// <summary>
        /// Create a Result object from a successful value.
        /// </summary>
        /// <param name="okResult"></param>
        public Result(T_RESULT okResult)
        {
            Value = okResult;
            Error = default(T_ERROR);

            ResultStatus = RESULT_STATE.SUCCESS;
        }

        /// <summary>
        /// Create a Result object from an error value.
        /// </summary>
        /// <param name="errorResult"></param>
        public Result(T_ERROR errorResult)
        {
            Value = default(T_RESULT);
            Error = errorResult;

            ResultStatus = RESULT_STATE.FAILED;
        }

        /// <summary>
        /// Private implementation that allows quick setup of any state variables.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="error"></param>
        /// <param name="manualState"></param>
        private Result(T_RESULT result, T_ERROR error, RESULT_STATE manualState)
        {
            Value = result;
            Error = error;

            ResultStatus = manualState;
        }

        /// <summary>
        /// This method creates and return a Result object from the execution of the function passed in parameter.
        /// The function is executed and the result is checked, if the result is non-null then the result is a SUCCESS and the 
        /// function result is inserted in the T_RESULT member. If the result is null, the Result is build from the Exception
        /// passed in parameter. If the tested function throws an exception then the Result is constructed from the thrown exception
        /// and a special state of RESULT_STATE.ERROR is applied.
        /// </summary>
        /// <param name="test">Function called to construct the Result object.</param>
        /// <param name="error_if_failed">
        /// Exception that is given if the function call returns an unsuccessful result (currently only null).
        /// </param>
        /// <returns>Result object.</returns>
        /// <remarks>
        /// Problems with generic type constraint was solved thanks to 
        /// http://stackoverflow.com/questions/183923/compiler-fails-converting-a-constrained-generic-type
        /// </remarks>
        public static Result<T_RESULT, T_ERROR> FromTest(Func<T_RESULT> test, Exception error_if_failed) 
        {
            try
            {
                var result = test();

                if (result == null)
                {
                    return new Result<T_RESULT, T_ERROR>(error_if_failed as T_ERROR); // FAILED (Soft)
                }
                else
                {
                    return new Result<T_RESULT, T_ERROR>(result); // SUCCESS
                }
            }
            catch (Exception ex)
            {
               return new Result<T_RESULT, T_ERROR>(null, ex as T_ERROR, RESULT_STATE.ERROR); // ERROR (Hard)
            }
        }

        /// <summary>
        /// Quick check to see if the function result is a success.
        /// </summary>
        /// <returns>True if the Result is a SUCCESS.</returns>
        public bool IsOk()
        {
            return Value != null;
        }

        /// <summary>
        /// Utility method to provice a different syntax to success checking a Result object.
        /// </summary>
        /// <param name="check">Result object to test</param>
        /// <returns>True if the Result is a SUCCESS.</returns>
        public static bool IsOk(Result<T_RESULT, T_ERROR> check)
        {
            return check.IsOk();
        }

        /// <summary>
        /// Quick check to see if the function result is a failure.
        /// </summary>
        /// <returns>True if the Result is FAILED.</returns>
        public bool IsError()
        {
            return Error != null;
        }

        /// <summary>
        /// Utility method to provice a different syntax to error checking a Result object.
        /// </summary>
        /// <param name="check">Result object to test</param>
        /// <returns>True if the Result is FAILED.</returns>
        public static bool IsError(Result<T_RESULT, T_ERROR> check)
        {
            return check.IsError();
        }
    }
}
