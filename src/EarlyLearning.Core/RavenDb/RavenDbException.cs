using System;

namespace EarlyLearning.Core.RavenDb
{
    public class RavenDbException : ApplicationException
    {
        public RavenDbException() {}

        public RavenDbException(Exception innerException)
            : base("Working with RavenDb resulted in an exception", innerException) {}
    }
}