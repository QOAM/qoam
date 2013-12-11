namespace RU.Uci.OAMarket.Website.Helpers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using RU.Uci.OAMarket.Domain;

    using Validation;

    public static class SelectListItemExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(this IList<Language> languages, string optionalText)
        {
            Requires.NotNull(languages, "languages");

            return languages.Select(ToSelectListItem).ToOptionalSelectListItems(optionalText);
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(this IList<Subject> keywords, string optionalText)
        {
            Requires.NotNull(keywords, "keywords");

            return keywords.Select(ToSelectListItem).ToOptionalSelectListItems(optionalText);
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(this IList<Institution> institutions, string optionalText)
        {
            Requires.NotNull(institutions, "institutions");

            return institutions.Select(ToSelectListItem).ToOptionalSelectListItems(optionalText);
        }

        private static SelectListItem ToSelectListItem(this Language language)
        {
            return new SelectListItem { Text = language.Name, Value = language.Id.ToString(CultureInfo.InvariantCulture) };
        }

        private static SelectListItem ToSelectListItem(this Subject subject)
        {
            return new SelectListItem { Text = subject.Name, Value = subject.Id.ToString(CultureInfo.InvariantCulture) };
        }

        private static SelectListItem ToSelectListItem(this Institution institution)
        {
            return new SelectListItem { Text = institution.Name, Value = institution.Id.ToString(CultureInfo.InvariantCulture) };
        }

        private static IEnumerable<SelectListItem> ToOptionalSelectListItems(this IEnumerable<SelectListItem> selectListItems, string optionalText)
        {
            var optionalSelectListItems = selectListItems.ToList();
            optionalSelectListItems.Insert(0, new SelectListItem { Text = optionalText, Value = string.Empty });

            return optionalSelectListItems;
        }
    }
}