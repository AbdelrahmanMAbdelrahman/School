using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using SchoolBus;
using SchoolData;

namespace SchoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        [HttpPost("AddNewUser", Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult AddNewUser(UserDTO userDTO)
        {
            if (string.IsNullOrWhiteSpace(userDTO.Name) ||
                string.IsNullOrWhiteSpace(userDTO.Password) ||
                string.IsNullOrWhiteSpace(userDTO.Email))
            {
                return BadRequest("Name, Password and Email are required");
            }
            UserBus userBus = new UserBus(userDTO, UserBus.EnMode.AddNew);
            if (!userBus.Save())
            {
                return BadRequest("Error in adding new user");
            }
            userDTO.UserId = userBus.UserId;
            return CreatedAtRoute("AddnewUser", new { userDTO });
        }
        [HttpGet("GetUserBy", Name = "GetUserBy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserBus> GetUserBy(int UserID)
        {
            if (UserID <= 0)
            {
                return BadRequest("in correct user id ");
            }
            UserBus user = UserBus.GetUserBy(UserID);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }
        [HttpGet("GetAllUser", Name = "GetAllUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetAllUser()
        {
            List<UserDTO> userList = UserBus.GetAllUsers();
            if (userList == null || userList.Count == 0)
            {
                return NotFound("No users found");
            }
            return Ok(userList);
        }


        [HttpPut("UpdateUser", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateUser(UserDTO userDTO)
        {
            if (userDTO.UserId <= 0 || string.IsNullOrWhiteSpace(userDTO.Name) || string.IsNullOrWhiteSpace(userDTO.Password) ||
                string.IsNullOrWhiteSpace(userDTO.Email))
            {
                return BadRequest("in correct user data");
            }
            UserBus userBus = UserBus.GetUserBy(userDTO.UserId);
            if (userBus == null)
            {
                return NotFound("user not found");
            }
            userBus.UserId = userDTO.UserId;
            userBus.Name = userDTO.Name;
            userBus.Password = userDTO.Password;
            userBus.Email = userDTO.Email;
            if (!userBus.Save())
            {
                return BadRequest("Error in updating user");
            }
            return Ok(new { userDTO });
        }

        [HttpDelete("DeleteUser", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteUser(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID");
            }
            if (!UserBus.DeleteUser(userId))
            {
                return NotFound("User not found");
            }
            return Ok("User deleted successfully");
        }

        [HttpPost("AddNewTask", Name = "AddNewTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult AddNewTask(ClsTaskDTO taskDTO)
        {
            if (string.IsNullOrWhiteSpace(taskDTO.Title) ||
                string.IsNullOrWhiteSpace(taskDTO.Description) ||
                taskDTO.DeadLine == null ||
                taskDTO.UserId <= 0 || taskDTO.Priority < 0)
            {
                return BadRequest("Title, Description, DeadLine and UserId are required");
            }
            TaskBus taskBus = new TaskBus(taskDTO, TaskBus.EnMode.AddNew);
            if (!taskBus.Save())
            {
                return BadRequest("Error in adding new task");
            }
            taskDTO.TaskId = taskBus.TaskId;
            return CreatedAtRoute("AddNewTask", new { taskDTO });
        }
        [HttpGet("GetTaskBy", Name = "GetTaskBy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetTaskByID(int TaskID) {
            if (TaskID <= 0)
            {
                return BadRequest("invalid task ID");
            }
            TaskBus task = TaskBus.GetTaskBy(TaskID);
            if (task == null)
            {
                return NotFound("Task not found");
            }
            return Ok(task);
        }
        [HttpGet("GetAllTasks", Name = "GetAllTasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetAllTasks()
        {
            List<ClsTaskDTO> taskList = TaskBus.GetAllTasks();
            if (taskList == null || taskList.Count == 0)
            {
                return NotFound("No tasks found");
            }
            return Ok(taskList);
        }
        [HttpPut("UpdateTask", Name = "UpdateTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateTask(ClsTaskDTO taskDTO) { 
        if(taskDTO.TaskId <= 0 || string.IsNullOrWhiteSpace(taskDTO.Title) ||
            string.IsNullOrWhiteSpace(taskDTO.Description) || taskDTO.DeadLine == null ||
            taskDTO.UserId <= 0 || taskDTO.Priority < 0)
            {
                return BadRequest("in correct task data");
            }
            TaskBus taskBus = TaskBus.GetTaskBy(taskDTO.TaskId);
            if (taskBus == null)
            {
                return NotFound("task not found");
            }
            taskBus.TaskId = taskDTO.TaskId;
            taskBus.Title = taskDTO.Title;
            taskBus.Description = taskDTO.Description;
            taskBus.DeadLine = taskDTO.DeadLine;
            taskBus.UserId = taskDTO.UserId;
            taskBus.Priority = taskDTO.Priority;
            if (!taskBus.Save())
            {
                return BadRequest("Error in updating user");
            }
            return Ok(new { taskDTO });
        }

        [HttpDelete("DeleteTask",Name ="DeleteTAsk")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteTask(int TaskID)
        {
            if (TaskID <= 0)
            {
                return BadRequest("invalid task ID");
            }
            TaskBus task = TaskBus.GetTaskBy(TaskID);
            if (task == null)
            {
                return NotFound("Task not found");
            }
           if(!TaskBus.DeleteTask(TaskID))
            {
                return BadRequest("error deleting the task");
            }
            return Ok("deleted succesffuly");
        }


        [HttpPost("AddNewStudySession", Name = "AddNewStudySession")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult AddNewSession(ClsStudySessionDTO clsStudySessionDTO)
        {
           if(clsStudySessionDTO.UserId <= 0 || clsStudySessionDTO.StartTime == null ||
                clsStudySessionDTO.EndTime == null || string.IsNullOrWhiteSpace(clsStudySessionDTO.Topic) ||
                string.IsNullOrWhiteSpace(clsStudySessionDTO.Notes))
            {
                return BadRequest("UserId, StartTime, EndTime, Topic and Notes are required");
            }
            ClsStudySession clsStudySession = new ClsStudySession(clsStudySessionDTO, ClsStudySession.EnMode.AddNew);
            if (!clsStudySession.Save())
            {
                return BadRequest("Error in adding new session");
            }
            clsStudySessionDTO.StudySessionId = clsStudySession.StudySessionId;
            return CreatedAtRoute("AddNewStudySession", new { clsStudySessionDTO });
        }

        [HttpGet("GetStudySessionById", Name = "GetStudySessionById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetStudySessionByID(int id) { 
        if(id <= 0)
            {
                return BadRequest("invalid session ID");
            }
            ClsStudySession clsStudySession = new ClsStudySession();
            clsStudySession.StudySessionId = id;
            clsStudySession. GetStudySessionById();
            if (clsStudySession == null)
            {
                return NotFound("session not found");
            }
            return Ok(clsStudySession);
        }
        [HttpGet("GetAllStudySessions",Name ="GetAllStudySession")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetAllStudySession()
        {
            List<ClsStudySessionDTO> Sessions = ClsStudySession.GetAllStudySessions();
            if (Sessions.Count <= 0)
            {
                return NotFound("No Sessions found");
            }
            return Ok(Sessions);

        }
        [HttpPut("UpdateStudySession", Name = "UpdateStudySession")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateStudySession(ClsStudySessionDTO Session) { 
        if(Session.UserId <= 0 || Session.StudySessionId <= 0 || Session.StartTime == null ||
            Session.EndTime == null || string.IsNullOrWhiteSpace(Session.Topic) ||
            string.IsNullOrWhiteSpace(Session.Notes))
            {
                return BadRequest("UserId, StudySessionId, StartTime, EndTime, Topic and Notes are required");
            }
            ClsStudySession clsStudySession = new ClsStudySession(Session, ClsStudySession.EnMode.Update);
            if (!clsStudySession.Save())
            {
                return BadRequest("Error in updating session");
            }
            return Ok(new { Session });
        }

        [HttpDelete("DeleteStudySession", Name = "DeleteStudySession")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteStudySession(int StudySessionId)
        {
            if (StudySessionId <= 0)
            {
                return BadRequest("Invalid session ID");
            }
            ClsStudySession clsStudySession = new ClsStudySession();
            clsStudySession.StudySessionId = StudySessionId;
            if (!clsStudySession.DeleteStudySession())
            {
                return NotFound("session not found");
            }
            return Ok("session deleted successfully");
        }
    }
}
