using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolData
{
    public class ClsTaskDTO
    {
        public ClsTaskDTO(int taskId, string title, string description, DateTime deadLine,
            int userId, int priority, bool isCompleted)
        {
            TaskId = taskId;
            Title = title;
            Description = description;
            DeadLine = deadLine;
            UserId = userId;
            Priority = priority;
            IsCompleted = isCompleted;
        }

        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        public int UserId { get; set; }
        public int Priority { get; set; }
        public bool IsCompleted { get; set; }
    }
    public   class ClsTaskData
    {
         public static int AddNewTask(ClsTaskDTO taskDTO)
        {int TaskID = 0;
            using (SqlConnection connection =new SqlConnection(ClsSettingsDataAccess.ConnectionString))
            {
                using(SqlCommand command=new SqlCommand ("AddNewTask", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Title", taskDTO.Title);
                    command.Parameters.AddWithValue("@Description", taskDTO.Description);
                    command.Parameters.AddWithValue("@DeadLine", taskDTO.DeadLine);
                    command.Parameters.AddWithValue("@UserId", taskDTO.UserId);
                    command.Parameters.AddWithValue("@Priority", taskDTO.Priority);
                    command.Parameters.AddWithValue("@IsCompleted", taskDTO.IsCompleted);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            TaskID= id;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return TaskID;
        }
        public static bool UpdateTask(ClsTaskDTO taskDTO)
        {
            bool updated = false;
            using (SqlConnection connection = new SqlConnection(ClsSettingsDataAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("UpdateTask", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TaskID", taskDTO.TaskId);
                    command.Parameters.AddWithValue("@Title", taskDTO.Title);
                    command.Parameters.AddWithValue("@Description", taskDTO.Description);
                    command.Parameters.AddWithValue("@DeadLine", taskDTO.DeadLine);
                    command.Parameters.AddWithValue("@UserId", taskDTO.UserId);
                    command.Parameters.AddWithValue("@Priority", taskDTO.Priority);
                    command.Parameters.AddWithValue("@IsCompleted", taskDTO.IsCompleted);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        updated = rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return updated;
        }
        public static bool DeleteTask(int taskId)
        {
            bool deleted = false;
            using (SqlConnection connection = new SqlConnection(ClsSettingsDataAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("DeleteTask", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TaskId", taskId);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        deleted = rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return deleted;
        }
        public static ClsTaskDTO GetTaskById(int taskId)
        {
            ClsTaskDTO task = null;
            using (SqlConnection connection = new SqlConnection(ClsSettingsDataAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetTaskByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TaskID", taskId);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                task = new ClsTaskDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("TaskID")),
                                reader .GetString(reader.GetOrdinal("Title")),
                                      reader.GetString(reader.GetOrdinal("Description")),
                                      reader.GetDateTime(reader.GetOrdinal("DeadLine")),
                                    reader.GetInt32(reader.GetOrdinal("UserId")),
                                     reader.GetInt32(reader.GetOrdinal("Priority")),
                                     reader.GetBoolean(reader.GetOrdinal("IsCompleted"))
                                    
                                 
                                );
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return task;
        }
        public static List<ClsTaskDTO> GetAllTasks()
        {
            List<ClsTaskDTO> tasks = new List<ClsTaskDTO>();
            using (SqlConnection connection = new SqlConnection(ClsSettingsDataAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetAllTasks", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClsTaskDTO task = new ClsTaskDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("TaskID")),
                                      reader.GetString(reader.GetOrdinal("Title")),
                                      reader.GetString(reader.GetOrdinal("Description")),
                                      reader.GetDateTime(reader.GetOrdinal("Deadline")),
                                      reader.GetInt32(reader.GetOrdinal("UserID") ),
                                      reader.GetInt32(reader.GetOrdinal("Priority")),
                                      reader.GetBoolean(reader.GetOrdinal("IsCompleted"))
                                );
                                tasks.Add(task);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return tasks;
        }
    }
}
