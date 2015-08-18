namespace QOAM.Core.Import
{
    using System;
    using System.Collections.Generic;
    using CsvHelper;
    using CsvHelper.Configuration;

    public sealed class DoajImportRecordMap : CsvClassMap<DoajImportRecord>
    {
        private static readonly HashSet<string> HasSealTrueValues = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) {"yes", "true"};

        public DoajImportRecordMap()
        {
            Map(m => m.Title).Name("Journal title");
            Map(m => m.URL).Name("Journal URL");
            Map(m => m.Publisher).Name("Publisher");
            Map(m => m.Language).Name("Full text language");
            Map(m => m.ISSN).Name("Journal ISSN (print version)");
            Map(m => m.Subjects).Name("Keywords");
            Map(m => m.Country).Name("Country of publisher");
            Map(m => m.HasSeal).ConvertUsing(ParseHasSeal);
        }

        private static bool ParseHasSeal(ICsvReaderRow r)
        {
            return HasSealTrueValues.Contains(r.GetField("DOAJ Seal") ?? string.Empty);
        }
    }
}