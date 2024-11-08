namespace UserCRUDBackendAPI.Models
{
    // Basic user details
    public class BaseUserModel
    {
        public string UserName { get; set; } = null!;

        public string Mail { get; set; } = "";

        public string PhoneNumber { get; set; } = "";

        public string Skillsets { get; set; } = "";

        public string Hobby { get; set; } = "";
    }

    // User class with unique id
    public class UserModel : BaseUserModel
    {
        public int Id { get; set; }
    }
}
