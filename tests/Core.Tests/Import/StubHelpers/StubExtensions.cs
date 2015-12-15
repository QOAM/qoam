using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QOAM.Core.Tests.Import.StubHelpers
{
    public static class StubExtensions
    {
        public static void AddColumns(this DataTable table, params string[] columns)
        {
            foreach (var column in columns)
            {
                table.Columns.Add(column);
            }
        }

        public static void AddRow(this DataTable table, Dictionary<string, string> values)
        {
            var row = table.NewRow();

            foreach (var kvp in values)
                row[kvp.Key] = kvp.Value;
        }

        public static void AddRow(this DataTable table, IEnumerable<string> values)
        {
            var row = table.NewRow();

            foreach (var kvp in values.Select((v, i) => new { Value = v, Index = i }))
                row[kvp.Index] = kvp.Value;

            table.Rows.Add(row);
        }
    }
}