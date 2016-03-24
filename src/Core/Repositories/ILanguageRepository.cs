namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;

    public interface ILanguageRepository
    {
        IList<Language> All { get; }
        IList<Language> Active { get; }

        void InsertBulk(IEnumerable<Language> languages);
    }
}