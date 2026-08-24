namespace Entities.Dtos
{
    public class UserProfileDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PassWord { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }
}