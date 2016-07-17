using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Images.Data
{
    public class ImageManager
    {
        private string _connString;
        public ImageManager(string connString)
        {
            _connString = connString;
        }
        
        
        public int AddImage (Image image)
        {
            int id = 0;
            DBAction(cmd =>
            {
                cmd.CommandText = "INSERT INTO Images (Username, ImageFileName, dateuploaded, hits)" +
                                  " VALUES (@user, @file, @date, @hits) SELECT @@IDENTITY";
                cmd.Parameters.AddWithValue("@user", image.UserName);
                cmd.Parameters.AddWithValue("@file", image.ImageFileName);
                cmd.Parameters.AddWithValue("@date", image.DateUploaded);
                cmd.Parameters.AddWithValue("@hits", image.Hits);
                id = (int)(decimal)cmd.ExecuteScalar();
            });
            return id;
        }
        public IEnumerable<ImageWithLikes> GetMostRecent()
        {
            List<ImageWithLikes> images = new List<ImageWithLikes>();
            DBAction(cmd =>
                {
                    cmd.CommandText = "SELECT TOP 5 * FROM Images ORDER BY DateUploaded DESC";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                       ImageWithLikes i = new ImageWithLikes();
                       i.Image =  ToImage(reader);
                       i.LikesCount = GetLikeCount(i.Image.Id);
                       images.Add(i);
                    }
                });
            return images;
        }

        public IEnumerable<ImageWithLikes> GetMostPopular()
        {
            List<ImageWithLikes> images = new List<ImageWithLikes>();
            DBAction(cmd =>
            {
                cmd.CommandText = "SELECT TOP 5 * FROM Images ORDER BY Hits DESC";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ImageWithLikes i = new ImageWithLikes();
                    i.Image = ToImage(reader);
                    i.LikesCount = GetLikeCount(i.Image.Id);
                    images.Add(i);
                }
            });
            return images;
        }
        public void IncrementViews(int id)
        {
            DBAction(cmd =>
            {
                cmd.CommandText = "UPDATE Images Set Hits = Hits + 1 WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            });
        }
        public Image GetImageById(int id)
        {
            var image = new Image();
            DBAction(cmd =>
            {
                cmd.CommandText = "SELECT * FROM Images WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                reader.Read();
                image = ToImage(reader);
            });
            return image;
        }
        public void AddImageLike(int userId, int imageId)
        {
            DBAction(cmd =>
            {
                cmd.CommandText = "INSERT INTO ImageLikes (UserId, ImageId) VALUES (@user, @image)";
                cmd.Parameters.AddWithValue("@user", userId);
                cmd.Parameters.AddWithValue("@image", imageId);
                cmd.ExecuteNonQuery();
            });
        }

        public int GetLikeCount (int imageId)
        {
            var count = 0;
            DBAction(cmd =>
                {
                    cmd.CommandText = "SELECT ISNULL(Count(*), 0) FROM ImageLikes WHERE ImageId = @id";
                    cmd.Parameters.AddWithValue("@id", imageId);
                    count = (int)cmd.ExecuteScalar();
                });
            return count;
        }

        public bool CheckIfUserHasLiked(string userName, int imageId)
        {
            bool hasLiked = false;
            DBAction(command =>
            {
            command.CommandText = "SELECT ISNULL(Count(*), 0) FROM ImageLikes il " +
                                  "JOIN Users u ON il.userId = u.id " +
                                  "WHERE u.UserName = @username AND il.imageId = @imageId";
            command.Parameters.AddWithValue("@username", userName);
            command.Parameters.AddWithValue("@imageId", imageId);
            hasLiked = ((int)command.ExecuteScalar() == 1);
            });
            return hasLiked;
        }
        private Image ToImage(SqlDataReader reader)
        {
            Image i = new Image
            {
                Id = (int)reader["id"],
                UserName = (string)reader["username"],
                ImageFileName = (string)reader["imagefilename"],
                DateUploaded = (DateTime)reader["dateuploaded"],
                Hits = (int)reader["hits"]
            };
            return i;
        }
        private void DBAction (Action<SqlCommand> action)
        {
            using (SqlConnection connection = new SqlConnection (_connString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                action(command);
            }
        }
    }
}
