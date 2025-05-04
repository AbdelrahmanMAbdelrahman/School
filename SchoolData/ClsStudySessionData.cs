using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolData
{
    public class ClsStudySessionDTO
    {
        public ClsStudySessionDTO(int studySessionId, int userId, DateTime startTime, 
            DateTime endTime, string topic, string notes)
        {
            StudySessionId = studySessionId;
            UserId = userId;
            StartTime = startTime;
            EndTime = endTime;
            Topic = topic;
            Notes = notes;
        }

        public int StudySessionId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Topic { get; set; }
        public string Notes { get; set; }
    }
    public class ClsStudySessionData
    {
        public static int AddNewStudySession(ClsStudySessionDTO studySessionDTO)
        {
            int studySessionId = 0;
            using (SqlConnection connection = new SqlConnection(ClsSettingsDataAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("AddNewStudySession", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", studySessionDTO.UserId);
                    command.Parameters.AddWithValue("@StartDate", studySessionDTO.StartTime);
                    command.Parameters.AddWithValue("@EndDate", studySessionDTO.EndTime);
                    command.Parameters.AddWithValue("@Topic", studySessionDTO.Topic);
                    command.Parameters.AddWithValue("@Notes", studySessionDTO.Notes);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            studySessionId = id;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return studySessionId;
        }
        public static bool UpdateStudySession(ClsStudySessionDTO studySessionDTO)
        {
            using (SqlConnection connection = new SqlConnection(ClsSettingsDataAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("UpdateStudySession", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SessionID", studySessionDTO.StudySessionId);
                    command.Parameters.AddWithValue("@UserID", studySessionDTO.UserId);
                    command.Parameters.AddWithValue("@StartDate", studySessionDTO.StartTime);
                    command.Parameters.AddWithValue("@EndDate", studySessionDTO.EndTime);
                    command.Parameters.AddWithValue("@Topic", studySessionDTO.Topic);
                    command.Parameters.AddWithValue("@Notes", studySessionDTO.Notes);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
            }
        }
        public static bool DeleteStudySession(int studySessionId)
        {
            using (SqlConnection connection = new SqlConnection(ClsSettingsDataAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("DeleteStudySession", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudySessionID", studySessionId);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
            }

        }
        public static List<ClsStudySessionDTO> GetAllStudySession()
        {
            List<ClsStudySessionDTO> studySessionList = new List<ClsStudySessionDTO>();
            using (SqlConnection connection = new SqlConnection(ClsSettingsDataAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetAllStudySessions", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClsStudySessionDTO studySessionDTO = new ClsStudySessionDTO(
                                    reader.GetInt32(reader.GetOrdinal("SessionID")),
                                    reader.GetInt32(reader.GetOrdinal("UserID")),
                                    reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                    reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                    reader.GetString(reader.GetOrdinal("Topic")),
                                    reader.GetString(reader.GetOrdinal("Notes"))
                                );
                                studySessionList.Add(studySessionDTO);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return studySessionList;
        }

        public static ClsStudySessionDTO GetStudySessionByID(int id)
        {
            ClsStudySessionDTO studySessionDTO = null;
            using (SqlConnection connection = new SqlConnection(ClsSettingsDataAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetStudySessionBySessionID", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudySessionID", id);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                studySessionDTO = new ClsStudySessionDTO(
                                    reader.GetInt32(reader.GetOrdinal("StudySessionID")),
                                    reader.GetInt32(reader.GetOrdinal("UserID")),
                                    reader.GetDateTime(reader.GetOrdinal("StartTime")),
                                    reader.GetDateTime(reader.GetOrdinal("EndTime")),
                                    reader.GetString(reader.GetOrdinal("Topic")),
                                    reader.GetString(reader.GetOrdinal("Notes"))
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
            return studySessionDTO;
        }
        }
}
