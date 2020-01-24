namespace FlatSharp.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Thrown when FlatSharp.Compiler encounters an error in an FBS file.
    /// </summary>
    public class InvalidFbsFileException : Exception
    {
        public InvalidFbsFileException(IEnumerable<string> errors) : base("Errors in FBS schema")
        {
            this.Errors = errors.ToArray();
        }

        public string[] Errors { get; }
    }
}
