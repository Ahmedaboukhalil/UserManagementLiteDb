using LiteDB;

namespace UserManagementLiteDb.LiteDb
{
    public interface ILiteDbContext
    {
        LiteDatabase Database { get; }
    }
}
