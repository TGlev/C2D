using C2D.Shared.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D
{
    public class DatabaseManager
    {
        SQLiteConnection db;
        public DatabaseManager()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "C2D.db");
            db = new SQLiteConnection(databasePath);

            db.CreateTable<User>();
        }

        public User GetUser()
        {
            var query = db.Table<User>();
            if (query.Count() == 0)
                return null;
            return query.First();
        }

        public void SaveUser(User user)
        {
            user.Id = 0;
            if (GetUser() == null)
                db.Insert(user);
            else
                db.Update(user);

            App.UserSettings = user;
        }
    }
}
