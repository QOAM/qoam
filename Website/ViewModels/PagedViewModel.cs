namespace QOAM.Website.ViewModels
{
    public abstract class PagedViewModel
    {
        protected PagedViewModel()
        {
            this.Page = 1;
            this.PageSize = 15;
        }
        
        public int Page { get; set; }
        public int PageSize { get; protected set; }
    }
}