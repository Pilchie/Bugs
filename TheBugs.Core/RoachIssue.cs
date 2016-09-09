﻿using Octokit;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBugs
{
    public sealed class RoachIssue
    {
        public RoachIssueId Id { get; }
        public string Assignee { get; }
        public RoachMilestone Milestone { get; }
        public string Title { get; }
        public bool IsOpen { get; }
        public ImmutableArray<string> Labels;
        public DateTimeOffset? UpdatedAt;

        public int Number => Id.Number;
        public RoachRepoId RepoId => Id.RepoId;
        public string UriString => $"https://github.com/{RepoId.Owner}/{RepoId.Name}/issues/{Number}";
        public Uri Url => new Uri(UriString);

        public RoachIssue(RoachIssueId id, string assignee, RoachMilestone milestone, string title, bool isOpen, IEnumerable<string> labels, DateTimeOffset? updatedAt)
        {
            Id = id;
            Assignee = assignee;
            Milestone = milestone;
            Title = title;
            IsOpen = isOpen;
            Labels = labels.ToImmutableArray();
            UpdatedAt = updatedAt;
        }

        public RoachIssue(RoachRepoId repoId, Issue issue) : this(
            new RoachIssueId(repoId, issue.Number),
            issue.Assignee?.Login ?? TheBugsConstants.UnassignedName, 
            new RoachMilestone(repoId, issue.Milestone),
            issue.Title, 
            issue.State == ItemState.Open, 
            issue.Labels.Select(x => x.Name).ToImmutableArray(),
            issue.UpdatedAt)
        {

        }

        public RoachIssue(Repository repo, Issue issue) : this(new RoachRepoId(repo), issue)
        {

        }

        public override string ToString() => $"{Id.RepoId} {Id.Number} {Assignee}";
    }
}
