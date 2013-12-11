namespace RU.Uci.OAMarket.Domain.Repositories
{
    using System.Collections.Generic;

    public interface IQuestionRepository
    {
        IList<Question> All { get; }
    }
}