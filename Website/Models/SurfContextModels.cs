namespace RU.Uci.OAMarket.Website.Models
{
    internal class SurfConextAccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
    }

    internal class SurfConextProfile
    {
        public int startIndex { get; set; }
        public int totalResults { get; set; }
        public int itemsPerPage { get; set; }
        public bool filtered { get; set; }
        public bool updatedSince { get; set; }
        public bool sorted { get; set; }
        public SurfConextEntry entry { get; set; }
    }

    internal class SurfConextEntry
    {
        public string id { get; set; }
        public string nickname { get; set; }
        public string displayName { get; set; }
        public object voot_membership_role { get; set; }
        public SurfConextName name { get; set; }
        public SurfConextEmail[] emails { get; set; }
        public SurfConextAccount[] accounts { get; set; }
        public SurfConextOrganization[] organizations { get; set; }
        public object phoneNumbers { get; set; }
        public string[] tags { get; set; }
        public string error { get; set; }
    }

    internal class SurfConextName
    {
        public string formatted { get; set; }
        public string familyName { get; set; }
        public string givenName { get; set; }
    }

    internal class SurfConextEmail
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    internal class SurfConextAccount
    {
        public string username { get; set; }
        public string userId { get; set; }
    }

    internal class SurfConextOrganization
    {
        public string name { get; set; }
        public string type { get; set; }
        public string department { get; set; }
        public string title { get; set; }
    }
}