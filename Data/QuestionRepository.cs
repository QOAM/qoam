namespace RU.Uci.OAMarket.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories;

    public class QuestionRepository : Repository, IQuestionRepository
    {
        public QuestionRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IList<Question> All
        {
            get
            {
                return this.DbContext.Questions.ToList();
            }
        }
    }
}