using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Adapty.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [JsonIgnore] // Isso impede que a senha (mesmo criptografada) apareça no JSON de resposta
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }
}