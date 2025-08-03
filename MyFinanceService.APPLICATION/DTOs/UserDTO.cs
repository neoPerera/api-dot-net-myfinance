using System.ComponentModel.DataAnnotations;

namespace MyFinanceService.APPLICATION.DTOs
{
    public class CreateUserRequest
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }
    
    public class UpdateUserRequest
    {
        [StringLength(100)]
        public string? Username { get; set; }
        
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }
    }
    
    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
    
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
    
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? Token { get; set; }
        public UserResponse? User { get; set; }
    }
} 