//<!-- Author xkrukh00-->
namespace Charity.DAL.Entities
{
    public abstract class UserEntityBase : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhotoURL { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; }
    }
}
