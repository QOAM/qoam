namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;

    public interface IQuestionRepository
    {
        IList<Question> All { get; }
    }
}