﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheBugs.Utils
{
    internal static class FilterUtil
    {
        internal static class CompilerTeam
        {
            internal static bool AssignedToTeam(RoachIssue issue)
            {
                switch (issue.Assignee.ToLower())
                {
                    case "jaredpar":
                    case "jcouv":
                    case "agocke":
                    case "alekseyts":
                    case "cston":
                    case "gafter":
                    case "tyoverby":
                    case "vsadov":
                        return true;
                    default:
                        return false;
                }
            }

            internal static bool AssignedToArea(RoachIssue issue)
            {
                return issue.Labels.Contains("Area-Compilers");
            }

            internal static bool IsIssue(RoachIssue issue)
            {
                if (IsNotBug(issue))
                {
                    return false;
                }

                if (AssignedToTeam(issue))
                {
                    return true;
                }

                if (AssignedToArea(issue) && issue.Assignee == Constants.UnassignedName)
                {
                    return true;
                }

                return false;
            }

        }

        internal static class IdeTeam
        {
            internal static bool AssignedToTeam(RoachIssue issue)
            {
                switch (issue.Assignee.ToLower())
                {
                    case "pilchie":
                    case "balajikris":
                    case "brettfo":
                    case "cyrusnajmabadi":
                    case "dpoeschl":
                    case "dustincampbell":
                    case "jasonmalinowski":
                    case "jmarolf":
                    case "rchande":
                    case "tmeschter":
                        return true;
                    default:
                        return false;
                }
            }

            internal static bool AssignedToArea(RoachIssue issue)
            {
                return issue.Labels.Contains("Area-Ide");
            }

            internal static bool IsIssue(RoachIssue issue)
            {
                if (IsNotBug(issue))
                {
                    return false;
                }

                if (AssignedToTeam(issue))
                {
                    return true;
                }

                if (AssignedToArea(issue) && issue.Assignee == Constants.UnassignedName)
                {
                    return true;
                }

                return false;
            }

        }

        internal static bool IsNotBug(RoachIssue issue)
        {
            return
                issue.Labels.Contains("Documentation") ||
                issue.Labels.Contains("Question");
        }
    }
}