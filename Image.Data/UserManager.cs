using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Images.Data
{
   public class UserManager
    {
       private string _connString;
       public UserManager(string connString)
       {
           _connString = connString;
       }
       public void AddUser (string firstName, string lastName, string userName, string password)
       {
           string salt = PasswordManager.GenerateSalt();
           string hash = PasswordManager.HashPassword(password, salt);

           DBAction(command =>
               {
                   command.CommandText = "INSERT INTO Users (FirstName, LastName, UserName, PasswordHash, PasswordSalt) VALUES " +
                                         "(@firstName, @lastName, @username, @password, @salt)";
                   command.Parameters.AddWithValue("@firstName", firstName);
                   command.Parameters.AddWithValue("@lastName", lastName);
                   command.Parameters.AddWithValue("@username", userName);
                   command.Parameters.AddWithValue("@password", hash);
                   command.Parameters.AddWithValue("@salt", salt);
                   command.ExecuteNonQuery();
               });

       }

       public User GetUser(string username)
       {
           User user = new User();
           DBAction(cmd =>
               {
                   cmd.CommandText = "SELECT * FROM Users WHERE UserName = @username";
                   cmd.Parameters.AddWithValue("@username", username);
                   var reader = cmd.ExecuteReader();
                   if (!reader.Read())
                   {
                       return;
                   }
                   else
                   {
                   user.UserName = username;
                   user.FirstName = (string)reader["firstname"];
                   user.LastName = (string)reader["lastname"];
                   user.Id = (int)reader["id"];
                   user.PasswordHash = (string)reader["passwordhash"];
                   user.PasswordSalt = (string)reader["passwordsalt"];
                   }
               });
           return user;
       }

       public User LogIn(string userName, string password)
       {
           User u = GetUser(userName);
           if (u == null)
           {
               return null;
           }
          if (PasswordManager.isMatch(u.PasswordHash, password, u.PasswordSalt))
          {
              return u;
          }
          return null;
       }
       private void DBAction(Action<SqlCommand> action)
       {
           using (SqlConnection connection = new SqlConnection(_connString))
           using (SqlCommand command = connection.CreateCommand())
           {
               connection.Open();
               action(command);
           }
       }
    }
}
