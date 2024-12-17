using System.Reflection.Metadata;

namespace clinic.models
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Person_id { get; set; }
        public string? Password { get; set; }
        public string? VerificationCode { get; set; }
        public string? Picture {  get; set; }
        public IFormFile? Profile_img { get; set; }
        public int? Role_id { get; set; }


    }
}
