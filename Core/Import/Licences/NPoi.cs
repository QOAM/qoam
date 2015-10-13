//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Mail;
//using NPOI.SS.UserModel;

//namespace QOAM.Core.Import.Licences
//{
//    public static class NPoi
//    {
//        private static ISheet GetFirstSheet(Stream input)
//        {
//            return GetWorkbook(input).GetSheetAt(0);
//        }

//        private static IWorkbook GetWorkbook(Stream input)
//        {
//            return WorkbookFactory.Create(input);
//        }


//        public static IEnumerable<IRow> NonHeaderRows(this ISheet sheet)
//        {
//            for (var rowNum = sheet.FirstRowNum + 1; rowNum <= sheet.LastRowNum; ++rowNum)
//            {
//                yield return sheet.GetRow(rowNum);
//            }
//        }

//        public static string GetCellValueAsString(this IRow row, ImportFileColumn column)
//        {
//            if (row == null)
//            {
//                return null;
//            }

//            var cell = row.GetCell(column.Index);

//            if (cell == null)
//            {
//                return null;
//            }

//            if (cell.CellType == CellType.Numeric)
//            {
//                return ((int)cell.NumericCellValue).ToString();
//            }

//            if (cell.CellType == CellType.String)
//            {
//                return cell.StringCellValue.TrimNullSafe().ToNullIfEmpty();
//            }

//            return null;
//        }

//        public static bool? GetCellValueAsNullableBoolean(this IRow row, ImportFileColumn column)
//        {
//            var value = row.GetCellValueAsString(column);

//            if (value == null)
//            {
//                return null;
//            }

//            if (BooleanTrueValues.Contains(value))
//            {
//                return true;
//            }

//            if (BooleanFalseValues.Contains(value))
//            {
//                return false;
//            }

//            return null;
//        }

//        public static int? GetCellValueAsNullablePositiveInteger(this IRow row, ImportFileColumn column)
//        {
//            if (row == null)
//            {
//                return null;
//            }

//            var cell = row.GetCell(column.Index);

//            if (cell == null)
//            {
//                return null;
//            }

//            if (cell.CellType == CellType.Numeric)
//            {
//                return (int)cell.NumericCellValue;
//            }

//            if (cell.CellType == CellType.String)
//            {
//                if (string.IsNullOrWhiteSpace(cell.StringCellValue))
//                {
//                    return null;
//                }

//                try
//                {
//                    var integerValue = int.Parse(cell.StringCellValue.Trim(), ParseCulture);

//                    if (integerValue < 0)
//                    {
//                        throw new RowParseException(row, string.Format(Errors.Parse_InvalidPositiveNumber, row.RowNum, column.Name, integerValue));
//                    }

//                    return integerValue;
//                }
//                catch (FormatException ex)
//                {
//                    throw new RowParseException(row, string.Format(Errors.Parse_InvalidNumber, row.RowNum, column.Name, cell.StringCellValue), ex);
//                }
//            }

//            return null;
//        }

//        public static int? GetCellValueAsNullableYear(this IRow row, ImportFileColumn column)
//        {
//            var value = row.GetCellValueAsNullablePositiveInteger(column);

//            if (value == null)
//            {
//                return null;
//            }

//            if (value < 1000 || value > 9999)
//            {
//                throw new RowParseException(row, string.Format(Errors.Parse_InvalidYear, row.RowNum, column.Name, value));
//            }

//            return value;
//        }

//        public static DateTime? GetCellValueAsNullableDateTime(this IRow row, ImportFileColumn column)
//        {
//            if (row == null)
//            {
//                return null;
//            }

//            var cell = row.GetCell(column.Index);

//            if (cell == null)
//            {
//                return null;
//            }

//            if (cell.CellType == CellType.Numeric)
//            {
//                return cell.DateCellValue;
//            }

//            if (cell.CellType == CellType.String)
//            {
//                if (string.IsNullOrWhiteSpace(cell.StringCellValue))
//                {
//                    return null;
//                }

//                try
//                {
//                    return DateTime.Parse(cell.StringCellValue.Trim(), ParseCulture);
//                }
//                catch (FormatException ex)
//                {
//                    throw new RowParseException(row, string.Format(Errors.Parse_InvalidDateTime, row.RowNum, column.Name, cell.StringCellValue), ex);
//                }
//            }

//            return null;
//        }

//        public static Guid? GetCellValueAsNullableGuid(this IRow row, ImportFileColumn column)
//        {
//            var value = row.GetCellValueAsString(column);

//            if (string.IsNullOrWhiteSpace(value))
//            {
//                return null;
//            }

//            try
//            {
//                return Guid.Parse(value.Trim());
//            }
//            catch (FormatException ex)
//            {
//                throw new RowParseException(row, string.Format(Errors.Parse_InvalidGuid, row.RowNum, column.Name, value), ex);
//            }
//        }

//        public static MailAddress GetCellValueAsMailAddress(this IRow row, ImportFileColumn column)
//        {
//            var value = row.GetCellValueAsString(column);

//            if (string.IsNullOrWhiteSpace(value))
//            {
//                return null;
//            }

//            try
//            {
//                return new MailAddress(value.Trim());
//            }
//            catch (FormatException ex)
//            {
//                throw new RowParseException(row, string.Format(Errors.Parse_InvalidMailAddress, row.RowNum, column.Name, value), ex);
//            }
//        }

//        public static bool IsBlank(this IRow row)
//        {
//            return row == null || row.Cells == null || row.Cells.All(c => c.IsBlank());
//        }

//        public static bool IsBlank(this ICell cell)
//        {
//            if (cell == null)
//            {
//                return true;
//            }

//            switch (cell.CellType)
//            {
//                case CellType.Unknown:
//                case CellType.Error:
//                case CellType.Blank:
//                    return true;
//                case CellType.String:
//                    return cell.RichStringCellValue == null || string.IsNullOrWhiteSpace(cell.RichStringCellValue.String);
//                default:
//                    return false;
//            }
//        }

//    }
//}