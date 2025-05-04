using SchoolData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBus
{
    public class TaskBus
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        public int UserId { get; set; }
        public int Priority { get; set; }
        public bool IsCompleted { get; set; }
        public enum EnMode { AddNew = 1, Update = 2 }
        public EnMode Mode = EnMode.AddNew;
        public ClsTaskDTO taskDTO
        {
            get
            {
                return new ClsTaskDTO(TaskId, Title, Description, DeadLine, UserId, Priority, IsCompleted);
            }
        }
        public TaskBus(ClsTaskDTO taskDTO,EnMode mode)
        {TaskId=taskDTO.TaskId;
            Title = taskDTO.Title;
            Description = taskDTO.Description;
            DeadLine = taskDTO.DeadLine;
            UserId = taskDTO.UserId;
            Priority = taskDTO.Priority;
            IsCompleted = taskDTO.IsCompleted;
            Mode = mode;

        }
        private bool _AddNewTask()
        {
            int taskId = ClsTaskData.AddNewTask(taskDTO);
            if (taskId > 0)
            {
                TaskId = taskId;
                return true;
            }
            return false;
        }
        private bool _UpdateTask()
        {
            return ClsTaskData.UpdateTask(taskDTO);
        }
        public bool Save()
        {
            bool result = false;
            switch (Mode)
            {
                case EnMode.AddNew:
                    result = _AddNewTask();
                    break;
                case EnMode.Update:
                    result = _UpdateTask();
                    break;
            }
            return result;
        }
        public static bool DeleteTask(int taskId)
        {
            return ClsTaskData.DeleteTask(taskId);
        }
        public static TaskBus GetTaskBy(int taskId)
        {
            ClsTaskDTO taskDTO = ClsTaskData.GetTaskById(taskId);
            if (taskDTO != null)
            {
                return new TaskBus(taskDTO, EnMode.Update);
            }
            return null;
        }
        public static List<ClsTaskDTO> GetAllTasks()
        {
            List<ClsTaskDTO> taskList = ClsTaskData.GetAllTasks();
          //  List<TaskBus> taskBusList = new List<TaskBus>();
            //foreach (var task in taskList)
            //{
            //    taskBusList.Add(new TaskBus(task, EnMode.Update));
            //}
            return taskList;
        }
    }
}
