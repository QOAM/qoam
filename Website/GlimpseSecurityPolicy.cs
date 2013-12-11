namespace RU.Uci.OAMarket.Website
{
    using Glimpse.AspNet.Extensions;
    using Glimpse.Core.Extensibility;

    public class GlimpseSecurityPolicy : IRuntimePolicy
    {
        public RuntimeEvent ExecuteOn
        {
            get
            {
                return RuntimeEvent.EndRequest;
            }
        }

        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            return policyContext.GetHttpContext().User.IsInRole("Admin") ? RuntimePolicy.On : RuntimePolicy.Off;
        }
    }
}