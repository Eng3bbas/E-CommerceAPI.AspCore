using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Http.Requests
{
    public class RegisterRequest
    {
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }

    }
}