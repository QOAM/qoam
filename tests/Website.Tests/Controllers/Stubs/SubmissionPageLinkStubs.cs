using System.Collections.Generic;
using QOAM.Core;
using QOAM.Core.Import.SubmissionLinks;

namespace QOAM.Website.Tests.Controllers.Stubs
{
    public static class SubmissionPageLinkStubs
    {
        public static List<SubmissionPageLink> Links()
        { 
            var list = new List<SubmissionPageLink>
            {
                new SubmissionPageLink { ISSN = "1687-8140", Url = "http://ade.sagepub.com/submission" },
                new SubmissionPageLink { ISSN = "2073-4395", Url = "http://www.mdpi.com/journal/agronomy/submission" },
                new SubmissionPageLink { ISSN = "2372-0484", Url = "http://www.aimspress.com/journal/Materials/submission" },
                new SubmissionPageLink { ISSN = "2050-084X", Url = "http://submit.elifesciences.org/cgi-bin/main.plex" }
            };

            return list;
        }

        public static List<SubmissionPageLink> InvalidLinks()
        {
            var list = new List<SubmissionPageLink>
            {
                new SubmissionPageLink { ISSN = "1687-8140", Url = "http://ade.somefakedomain.com/submission" },
                new SubmissionPageLink { ISSN = "2073-4395", Url = "http://www.darealdomain.com/journal/agronomy/submission" },
                new SubmissionPageLink { ISSN = "2372-0484", Url = "http://www.stillbatman.com/journal/Materials/submission" },
                new SubmissionPageLink { ISSN = "1687-7675", Url = "http://www.malicious.com/journals/aess/submission" }
            };

            return list;
        }

        public static List<Journal> Journals()
        {
            return new List<Journal>
            {
                new Journal { Id = 1, ISSN = "1687-8140", Link = "http://ade.sagepub.com" }, 
                new Journal { Id = 2, ISSN = "2073-4395", Link = "http://www.mdpi.com/journal/agronomy" },
                new Journal { Id = 3, ISSN = "2372-0484", Link = "http://www.aimspress.com/journal/Materials" },
                new Journal { Id = 4, ISSN = "2050-084X", Link = "http://elifesciences.org/" }
            };
        }
    }
}