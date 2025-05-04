using Microsoft.Data.SqlClient;

namespace SchoolData
{public class UserDTO
    {
        public UserDTO(int userId, string name, string password, string email)
        {
            UserId = userId;
            Name = name;
            Password = password;
            Email = email;
        }

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
       
    }

    public class UserData
    {
        public static int AddNewUser(UserDTO userDTO)
        { int UserID = 0;
            using (SqlConnection connection = new SqlConnection(ClsSettingsDataAccess.ConnectionString)) { 
            using (SqlCommand command=new SqlCommand("AddNewUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", userDTO.Name);
                    command.Parameters.AddWithValue("@Password", userDTO.Password);
                    command.Parameters.AddWithValue("@Email", userDTO.Email);
                    try { connection.Open();
                    object result = command.ExecuteScalar();
                        if(result!=null&&int.TryParse(result.ToString(),out int id)){
                            UserID = id;
                        }
                    }catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                     
                }

            }
                return UserID;
        }

        public static bool UpdateUser(UserDTO userDTO)
        {
            bool updated = false;
            using (SqlConnection connection = new SqlConnection(ClsSettingsDataAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("UpdateUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userDTO.UserId);
                    command.Parameters.AddWithValue("@Name", userDTO.Name);
                    command.Parameters.AddWithValue("@Password", userDTO.Password);
                    command.Parameters.AddWithValue("@Email", userDTO.Email);
                    try
                    {
                        connection.Open();
                       updated= command.ExecuteNonQuery()>0;
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                       
                    }
                }
            }
            return updated;
        }                               

        public static bool DeleteUser(int UserID)
        {
            bool deleted = false;
            using (SqlConnection connection = new SqlConnection(ClsSettingsDataAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("DeleteUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", UserID);
                    try
                    {
                        connection.Open();
                        deleted = command.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return deleted;
        }
        public static UserDTO GetUserById(int userId)
        {
            UserDTO userDTO = null;
            using (SqlConnection connection = new SqlConnection(ClsSettingsDataAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetUserById", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            userDTO = new UserDTO(
                         reader.GetInt32(reader.GetOrdinal("UserId")),
                            reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetString(reader.GetOrdinal("Password")),
                            reader.GetString(reader.GetOrdinal("Email"))
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return userDTO;
        }
        public static List<UserDTO> GetAllUsers()
        {
            List<UserDTO> users = new List<UserDTO>();
            using (SqlConnection connection = new SqlConnection(ClsSettingsDataAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetAllUsers", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            UserDTO userDTO = new UserDTO(
                                reader.GetInt32(reader.GetOrdinal("UserId")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("Password")),
                                reader.GetString(reader.GetOrdinal("Email"))
                            );
                            users.Add(userDTO);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return users;
        }
    }
}
