namespace QOAM.Website.Helpers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using QOAM.Core;

    using Validation;

    public static class SelectListItemExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(this IList<BlockedISSN> blockedIssns)
        {
            Requires.NotNull(blockedIssns, nameof(blockedIssns));

            return blockedIssns.Select(ToSelectListItem);
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(this IList<Language> languages, string optionalText)
        {
            Requires.NotNull(languages, nameof(languages));

            return languages.Select(ToSelectListItem).ToOptionalSelectListItems(optionalText);
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(this IList<Subject> keywords, string optionalText)
        {
            Requires.NotNull(keywords, nameof(keywords));

            return keywords.Select(ToSelectListItem).ToOptionalSelectListItems(optionalText);
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(this IList<Institution> institutions, string optionalText)
        {
            Requires.NotNull(institutions, nameof(institutions));

            return institutions.Select(ToSelectListItem).OrderBy(i => i.Text).ToOptionalSelectListItems(optionalText);
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(this IList<Corner> corners, string optionalText)
        {
            Requires.NotNull(corners, nameof(corners));

            return corners.Select(ToSelectListItem).ToOptionalSelectListItems(optionalText);
        }

        private static SelectListItem ToSelectListItem(this BlockedISSN blockedIssn)
        {
            return new SelectListItem { Text = blockedIssn.ISSN, Value = blockedIssn.ISSN };
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
            return new SelectListItem { Text = institution.Name.Trim(), Value = institution.Id.ToString(CultureInfo.InvariantCulture) };
        }

        private static SelectListItem ToSelectListItem(this Corner corner)
        {
            return new SelectListItem { Text = corner.Name, Value = corner.Id.ToString(CultureInfo.InvariantCulture) };
        }

        private static IEnumerable<SelectListItem> ToOptionalSelectListItems(this IEnumerable<SelectListItem> selectListItems, string optionalText)
        {
            var optionalSelectListItems = selectListItems.ToList();
            optionalSelectListItems.Insert(0, new SelectListItem { Text = optionalText, Value = string.Empty });

            return optionalSelectListItems;
        }
    }
}