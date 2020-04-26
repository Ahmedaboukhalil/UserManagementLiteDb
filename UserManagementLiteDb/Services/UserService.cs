using LiteDB;
using System.Collections.Generic;
using System.Linq;
using UserManagementLiteDb.LiteDb;
using UserManagementLiteDb.Models;

namespace UserManagementLiteDb.Services
{
    public class UserService: IUserService
    {
        private LiteDatabase _liteDb;

        public UserService(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
        }

        public IEnumerable<User> GetAll()
        {
            var users = _liteDb.GetCollection<User>("Users").FindAll().ToList();

            return users;
        }

        public User GetById(int id)
        {
            var users = _liteDb.GetCollection<User>("Users");

            return users.Find(x => x.Id == id).FirstOrDefault();
        }

        public int Insert(User user, string password)
        {
            try
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                var users = _liteDb.GetCollection<User>("Users");

                users.EnsureIndex(p => p.Id, true);
                return users.Insert(user);
            }
            catch (LiteDB.LiteException ex)
            {
                return 0;
            }
        }

        public bool Update(User user)
        {
            var users = _liteDb.GetCollection<User>("Users");

            return users.Update(user);
        }

        public bool Delete(int id)
        {
            var users = _liteDb.GetCollection<User>("Users");

            var value = new LiteDB.BsonValue(id);

            return users.Delete(value);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerfiyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

    }
}
