using System.ComponentModel.DataAnnotations.Schema;

namespace UserAPI.Domain
{
    public abstract class UserBase
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }
    }
}
