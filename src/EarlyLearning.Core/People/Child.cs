using System;

namespace EarlyLearning.Core.People
{
    public class Child : IEquatable<Child>
    {
        /// <summary>
        /// Default constructor used by RavenDb
        /// </summary>
        private Child(){}

        public Child(string id, string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Id = id;
        }

        public string Id { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Child) obj);
        }

        public bool Equals(Child other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && FirstName == other.FirstName && LastName == other.LastName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName);
        }
    }
}