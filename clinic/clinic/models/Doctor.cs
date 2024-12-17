namespace clinic.models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Person_id { get; set; }
        public string? Password { get; set; }
        public int? Category_id { get; set; }
        public string? Category_name {  get; set; }
        public int? Doctor_review {  get; set; }
        public string? Picture { get; set; }

        public IFormFile? Profile_img { get; set; }
        public IFormFile? Cv { get; set; }
    }
}
