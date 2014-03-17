namespace QOAM.Website
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
            var httpContextBase = policyContext.GetHttpContext();

            if (httpContextBase == null || httpContextBase.User == null)
            {
                return RuntimePolicy.Off;
            }

            return httpContextBase.User.IsInRole("Admin") ? RuntimePolicy.On : RuntimePolicy.Off;
        }
    }
}