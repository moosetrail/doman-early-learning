using System;

namespace Moosetrail.EarlyLearning.Dataclasses.People
{
    public class Child : IEquatable<Child>
    {
        public Child(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string Id { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string SetIdWithNumber(int nbr)
        {
            Id = "Child/" + FirstName + " " + LastName + " - " + nbr;
            return Id;
        }

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