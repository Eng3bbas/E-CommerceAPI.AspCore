using E_Commerce.Data.DTO;

namespace E_Commerce.Http.Responses
{
    public class AuthenticationResponse : BaseResponse
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}