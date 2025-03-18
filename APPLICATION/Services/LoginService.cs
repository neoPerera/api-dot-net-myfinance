using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using CORE.Interfaces;


namespace APPLICATION.Services
{
    public class LoginService : ILogin
    {
        private readonly IUserRepository _userRepository;

        public LoginService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<LoginResponse> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || password != user.Password)
            {
                return new LoginResponse(false, "Login unsuccessful");
                
            }
            var token = "XXX";
            return new LoginResponse(true, "Login successful", token);
        }
    }
}
