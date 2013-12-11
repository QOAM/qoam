namespace RU.Uci.OAMarket.Website.ViewModels
{
    public abstract class PagedViewModel
    {
        protected PagedViewModel()
        {
            this.Page = 1;
            this.PageSize = 10;
        }
        
        public int Page { get; set; }
        public int PageSize { get; protected set; }
    }
}