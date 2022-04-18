using System;

namespace Lexica.CLI.Args
{
    internal class ArgsException : Exception
    {
        public ArgsException()
        {
        }

        public ArgsException(string? message) : base(message)
        {
        }

        public ArgsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}