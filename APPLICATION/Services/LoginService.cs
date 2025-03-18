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

        // Authenticate user based on username and password
        public async Task<LoginResponse> AuthenticateAsync(string username, string password)
        {
            // Retrieve user from the repository
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null || password != user.Password)
            {
                return new LoginResponse(false, "Login unsuccessful");
                
            }

            // Verify password (in reality, use hashing verification here)
            //return password == user.Password; // Use password hashing in production
            var token = "XXX";
            return new LoginResponse(true, "Login successful", token);
        }
    }
}
