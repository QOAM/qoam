namespace RU.Uci.OAMarket.Domain.Repositories
{
    public interface IScoreCardVersionRepository
    {
        ScoreCardVersion FindCurrent();
    }
}