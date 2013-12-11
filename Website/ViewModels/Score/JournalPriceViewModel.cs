namespace RU.Uci.OAMarket.Website.ViewModels.Score
{
    using RU.Uci.OAMarket.Domain;

    public class JournalPriceViewModel
    {
        public int JournalPriceId { get; set; }
        public decimal? Amount { get; set; }
        public Currency? Currency { get; set; }

        public JournalPrice ToJournalPrice()
        {
            return new JournalPrice
                       {
                           Price = new Price { Amount = this.Amount, Currency = this.Currency }
                       };
        }
    }
}