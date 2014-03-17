namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;

    public interface ILanguageRepository
    {
        IList<Language> All { get; }

        void InsertBulk(IEnumerable<Language> languages);
    }
}