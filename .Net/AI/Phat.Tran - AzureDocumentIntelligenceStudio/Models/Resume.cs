namespace AzureDocumentIntelligenceStudio.Models
{
    public class Resume
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Overview { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public List<string>? RawTechnicalSkills { get; set; }
        public List<string>? TechnicalSkills { get; set; }
        public List<string>? Certifications { get; set; }
        public List<string>? Awards { get; set; }
        public List<WorkExperience>? WorkExperiences { get; set; }
        public List<Education>? Educations { get; set; }
    }

    public class WorkExperience
    {
        public string? Project { get; set; }
        public string? Position { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? Description { get; set; }
        public string? Region { get; set; }
    }

    public class Education
    {
        public string? SchoolName { get; set; }
        public string? Level { get; set; }
        public string? Majority { get; set; }
        public string? Date { get; set; }
    }
}
