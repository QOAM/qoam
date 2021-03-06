namespace QOAM.Core.Import
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Repositories;
    using Validation;

    public abstract class Import
    {
        public const string MissingPublisherName = "<none indicated>";
        readonly IBlockedISSNRepository blockedIssnRepository;

        protected Import(IBlockedISSNRepository blockedIssnRepository)
        {
            Requires.NotNull(blockedIssnRepository, nameof(blockedIssnRepository));

            this.blockedIssnRepository = blockedIssnRepository;
        }

        protected IList<Journal> ExcludeBlockedIssns(IList<Journal> journals)
        {
            Requires.NotNull(journals, nameof(journals));

            var issnsToBlock = new HashSet<string>(this.blockedIssnRepository.All.Select(b => b.ISSN), StringComparer.InvariantCultureIgnoreCase);

            return journals.Where(j => !issnsToBlock.Contains(j.ISSN)).ToList();
        }
    }
}