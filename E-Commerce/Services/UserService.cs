using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Data;
using E_Commerce.Data.DTO;
using E_Commerce.Data.Entities;
using E_Commerce.Helpers;
using E_Commerce.Http.Requests;
using E_Commerce.Http.Responses;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services
{
    public class UserService
    {
        private readonly IRepository<User> _repository;
        private readonly TokenManger _tokenManger;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> repository , TokenManger tokenManger , IMapper mapper)
        {
            _repository = repository;
            _tokenManger = tokenManger;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> Register(RegisterRequest request)
        {
            if (_repository.Where(u => u.Email == request.Email).Any())
                return new AuthenticationResponse
                {
                    ErrorMessage = "This email is used"
                };
            var user = await _repository.Create( new User
            {
                Email =  request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Name = request.Name,
                Role = User.Roles.User
            });
            await _repository.Save();
            return new AuthenticationResponse
            {
                User = _mapper.Map<UserDTO>(user),
                Token = _tokenManger.Generate(user)
            };
        }

        public async Task<AuthenticationResponse> Login(LoginRequest request)
        {
            var user = _repository.Where(u => u.Email == request.Email).FirstOrDefault();
            if (user == null)
                return new AuthenticationResponse
                {
                    ErrorMessage = "This user is not found"
                };
            
            if (!BCrypt.Net.BCrypt.Verify(hash: user.Password , text: request.Password))
                return new AuthenticationResponse
                {
                    ErrorMessage = "Check your password"
                };
            return new AuthenticationResponse
            {
                User = _mapper.Map<UserDTO>(user),
                Token = _tokenManger.Generate(user)
            };
        }
    }
}