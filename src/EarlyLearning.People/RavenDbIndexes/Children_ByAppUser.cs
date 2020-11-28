﻿using System.Collections.Generic;
using System.Linq;
using EarlyLearning.Core.People;
using EarlyLearning.People.DataModels;
using Raven.Client.Documents.Indexes;

namespace EarlyLearning.People.RavenDbIndexes
{
    public class Children_ByAppUser : AbstractMultiMapIndexCreationTask<Children_ByAppUser.Result>
    {
        public class Result
        {
            public string Email { get; set; }

            public IEnumerable<Child> Children { get; set; }
        }

        public Children_ByAppUser()
        {
            AddMap<AdultProgrammer>(adults => from a in adults
                select new Result
                {
                    Email = a.Email,
                    Children = LoadDocument<Child>(a.ChildIds)
                });
        }
    }
}