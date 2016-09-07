﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Web;

namespace TheBugs.Utils
{
    public sealed class DataStorage
    {
        // TODO: move to better storage.
        private static DataStorage s_instance;

        public ImmutableArray<RoachIssue> Issues { get; }
        public ImmutableArray<RoachMilestone> Milestones { get; }
        public ImmutableDictionary<int, RoachMilestone> MilestoneMap { get; }

        public DataStorage(ImmutableArray<RoachIssue> issues)
        {
            Issues = issues;
            MilestoneMap = issues
                .Select(x => x.Milestone)
                .ToImmutableDictionary(x => x.Number);
            Milestones = MilestoneMap.Values.ToImmutableArray();
        }

        public IEnumerable<RoachIssue> Filter(
            string assignee,
            string view,
            IList<int> milestones)
        {
            IEnumerable<RoachIssue> issues = Issues;

            if (view != null)
            {
                switch (view)
                {
                    case "jaredpar":
                        issues = issues.Where(x => FilterUtil.CompilerTeam.IsIssue(x));
                        break;
                    case "pilchie":
                        issues = issues.Where(x => FilterUtil.IdeTeam.IsIssue(x));
                        break;
                }
            }

            if (!string.IsNullOrEmpty(assignee))
            {
                issues = issues.Where(x => x.Assignee == assignee);
            }

            if (milestones.Count > 0)
            {
                issues = issues.Where(x => milestones.Contains(x.Milestone.Number));
            }

            return issues;
        }

        public static DataStorage GetOrCreate(HttpServerUtilityBase server)
        {
            if (s_instance != null)
            {
                return s_instance;
            }

            s_instance = Create(server);
            return s_instance;
        }

        private static DataStorage Create(HttpServerUtilityBase server)
        {
            var path = server.MapPath("~/App_Data/issues.csv");
            using (var stream = File.Open(path, FileMode.Open))
            {
                var issues = CsvUtil.Import(stream);
                return new DataStorage(issues.ToImmutableArray());
            }
        }
   }
}