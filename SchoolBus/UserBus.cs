using SchoolData;

namespace SchoolBus
{
    public class UserBus
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public enum EnMode { AddNew = 1, Update=2}
        public EnMode Mode = EnMode.AddNew;
        public UserDTO userDTO
        {
            get
            {
                return new UserDTO(UserId, Name, Password, Email);
            }
        }

        public UserBus(UserDTO userDTO,EnMode mode =EnMode.AddNew)
        {
            Name =userDTO. Name;
            UserId =userDTO. UserId;
            Password =userDTO. Password;
            Email =userDTO. Email;
            Mode = mode;
        }

      private bool _AddNewUser()
        {
            int userId = UserData.AddNewUser(userDTO);
            if (userId > 0)
            {
                UserId = userId;
                return true;
            }
            return false;
        }

        private bool _UpdateUser() { 
        return UserData.UpdateUser(userDTO);
        }

        public bool Save()
        {
            bool result = false;
            switch (Mode)
            {
                case EnMode.AddNew:
                    result = _AddNewUser();
                    break;
                case EnMode.Update:
                    result = _UpdateUser();
                    break;
            }
            return result;
        }

        public static bool DeleteUser(int userId)
        {
            return UserData.DeleteUser(userId);
        }
        public static UserBus GetUserBy(int userId)
        {
            UserBus userBus =new UserBus(UserData.GetUserById(userId),EnMode.Update);
            return userBus;
        }

        public static List<UserDTO> GetAllUsers()
        {
            return UserData.GetAllUsers();
        }
    }
}
