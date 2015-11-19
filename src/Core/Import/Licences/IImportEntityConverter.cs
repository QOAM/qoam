namespace QOAM.Core.Import.Licences
{
    using System.Collections.Generic;
    using System.Data;

    public interface IImportEntityConverter
    {
        List<UniversityLicense> Execute(DataSet data);
    }

    //public interface IImportCommand<in TArgs, out TResult> : IImportCommand
    //{
    //    TResult Execute(TArgs args);
    //}
}