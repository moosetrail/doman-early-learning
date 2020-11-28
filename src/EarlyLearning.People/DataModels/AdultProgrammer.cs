﻿using System.Collections.Generic;
using System.Linq;
using EarlyLearning.Core.People;

namespace EarlyLearning.People.DataModels
{
    public class AdultProgrammer: AppUser
    {
        private List<string> _childIds;

        public AdultProgrammer(string email)
        {
            Email = email;
            Id = "AdultProgrammer/" + Email;
            _childIds = new List<string>();
        }

        public string Id { get; private set; }

        public string Email { get; private set; }

        public IEnumerable<string> ChildIds
        {
            get => _childIds;
            private set => _childIds = value.ToList();
        }

        public IEnumerable<string> AddChild(string childId)
        {
            _childIds.Add(childId);
            return _childIds;
        }
    }
}