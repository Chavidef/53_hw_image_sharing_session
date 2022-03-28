using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _53_hw_image_sharing_session.Data
{
    public class ImageRepository
    {
        private string _connectionString;
        public ImageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int UploadImage(string password, string imageFile)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Images (Image, Password, Views) 
                                VALUES (@image, @password, 0) SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@image", imageFile);
            command.Parameters.AddWithValue("@password", password);

            connection.Open();
            return (int)(decimal)command.ExecuteScalar();
            
        }
        public Image GetImageById (int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Images WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            Image image = new Image();
            SqlDataReader reader = command.ExecuteReader(); 
            while(reader.Read())
            {
                image.Id = id;
                image.ImageURL = (string)reader["Image"];
                image.Password = (string)reader["Password"];
                image.Views = (int)reader["Views"];
            }
            return image;
        }
        public void IncrementView(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE Images SET Views = Views + 1
                                WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }
}
