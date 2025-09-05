using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RecoilNet.Diagnostics
{
    /// <summary>
    /// Represents information about the caller of a method, including file path, member name, and line number.
    /// </summary>
    public struct CallerInfo
    {
        /// <summary>
        /// Gets a caller info instance that represents an unset state.
        /// </summary>
        public static CallerInfo Unset { get; }

        /// <summary>
        /// Gets the full path of the source file that contains the caller.
        /// </summary>
        public readonly string CallerFilePath;

        /// <summary>
        /// Gets the name of the method or property that contains the caller.
        /// </summary>
        public readonly string CallerMemberName;

        /// <summary>
        /// Gets the line number in the source file at which the method is called.
        /// </summary>
        public readonly int CallerLineNumber;

        static CallerInfo()
        {
            Unset = new CallerInfo("", "", 0);
        }

        public CallerInfo(string callerFilePath, string callerMemberName, int callerLineNumber)
        {
            CallerFilePath = callerFilePath ?? throw new ArgumentNullException(nameof(callerFilePath));
            CallerMemberName = callerMemberName ?? throw new ArgumentNullException(nameof(callerMemberName));
            CallerLineNumber = callerLineNumber;
        }

    }
}
