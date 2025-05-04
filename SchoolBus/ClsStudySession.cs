using SchoolData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBus
{
    public class ClsStudySession
    {
        public int StudySessionId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Topic { get; set; }
        public string Notes { get; set; }
        public enum EnMode { AddNew = 1, Update = 2 }
        public EnMode Mode = EnMode.AddNew;
        public ClsStudySessionDTO ClsStudySessionDTO
        {
            get
            {
                return new ClsStudySessionDTO(StudySessionId, UserId, StartTime, EndTime, Topic, Notes);
            }
        }
        public ClsStudySession()
        {
            StudySessionId = 0;
            UserId = 0 ;
            StartTime =  DateTime.Now ;
            EndTime =DateTime.Now ;
            Topic = "";
            Notes = "";
            Mode = EnMode.AddNew;
        }
        public ClsStudySession(ClsStudySessionDTO studySessionDTO, EnMode mode=EnMode.AddNew)
        {
            StudySessionId = studySessionDTO.StudySessionId;
            UserId = studySessionDTO.UserId;
            StartTime = studySessionDTO.StartTime;
            EndTime = studySessionDTO.EndTime;
            Topic = studySessionDTO.Topic;
            Notes = studySessionDTO.Notes;
            Mode = mode;
        }
        private bool _AddNewStudySession()
        {
            int studySessionId = ClsStudySessionData.AddNewStudySession(ClsStudySessionDTO);
            if (studySessionId > 0)
            {
                StudySessionId = studySessionId;
                return true;
            }
            return false;
        }
        private bool _UpdateStudySession()
        {
            return ClsStudySessionData.UpdateStudySession(ClsStudySessionDTO);
        }
        public bool Save()
        {
            bool result = false;
            switch (Mode)
            {
                case EnMode.AddNew:
                    result = _AddNewStudySession();
                    break;
                case EnMode.Update:
                    result = _UpdateStudySession();
                    break;
            }
            return result;
        }
        public static List<ClsStudySessionDTO> GetAllStudySessions()
        {
            return ClsStudySessionData.GetAllStudySession();
        }

        public   ClsStudySession GetStudySessionById()
        {
            ClsStudySessionDTO clsStudySessionDTO =ClsStudySessionData.GetStudySessionByID(StudySessionId);
            if(clsStudySessionDTO != null)
            {
            return new ClsStudySession(clsStudySessionDTO,EnMode.Update);
                 
            }
            return null;
        }
        public bool DeleteStudySession()
        {
            return ClsStudySessionData.DeleteStudySession(StudySessionId);
        }
       
    }
}
