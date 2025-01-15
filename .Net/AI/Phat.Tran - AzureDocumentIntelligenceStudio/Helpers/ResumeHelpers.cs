using Azure.AI.DocumentIntelligence;
using AzureDocumentIntelligenceStudio.Models;

namespace AzureDocumentIntelligenceStudio.Helpers
{
    public interface IResumeHelpers
    {
        Resume ExtractResumeData(AnalyzedDocument document);
    }
    public class ResumeHelpers : IResumeHelpers
    {
        public Resume ExtractResumeData(AnalyzedDocument document)
        {
            var resume = new Resume();

            #region /* Process Signle Fields 
            foreach (KeyValuePair<string, DocumentField> field in document.Fields)
            {
                var fieldValue = field.Value?.ValueString ?? string.Empty;
                switch (field.Key.ToLowerInvariant())
                {
                    case var key when key == nameof(Resume.Name).ToLowerInvariant():
                        resume.Name = fieldValue;
                        break;

                    case var key when key == nameof(Resume.Role).ToLowerInvariant():
                        resume.Role = fieldValue;
                        break;

                    case var key when key == nameof(Resume.Overview).ToLowerInvariant():
                        resume.Overview = fieldValue;
                        break;
                }
            }
            #endregion Process Signle Fields */

            #region /* Process WorkExperiences
            var workExperiences = new List<WorkExperience>();
            if (document.Fields.TryGetValue("WorkExperiences", out var workExperienceFields) && workExperienceFields.FieldType == DocumentFieldType.List)
            {
                foreach (DocumentField item in workExperienceFields.ValueList)
                {
                    if (item.FieldType == DocumentFieldType.Dictionary)
                    {
                        var workExperience = new WorkExperience();
                        foreach (KeyValuePair<string, DocumentField> subItem in item.ValueDictionary)
                        {
                            var fieldValue = subItem.Value?.ValueString ?? string.Empty;
                            switch (subItem.Key.ToLowerInvariant())
                            {
                                case var key when key == nameof(WorkExperience.Project).ToLowerInvariant():
                                    workExperience.Project = fieldValue;
                                    break;

                                case var key when key == nameof(WorkExperience.Position).ToLowerInvariant():
                                    workExperience.Position = fieldValue;
                                    break;

                                case var key when key == nameof(WorkExperience.StartTime).ToLowerInvariant():
                                    workExperience.StartTime = fieldValue;
                                    break;

                                case var key when key == nameof(WorkExperience.EndTime).ToLowerInvariant():
                                    workExperience.EndTime = fieldValue;
                                    break;

                                case var key when key == nameof(WorkExperience.Description).ToLowerInvariant():
                                    workExperience.Description = fieldValue;
                                    break;

                                case var key when key == nameof(WorkExperience.Region).ToLowerInvariant():
                                    workExperience.Region = fieldValue;
                                    break;
                            }
                        }
                        workExperiences.Add(workExperience);
                    }
                }
            }
            resume.WorkExperiences = workExperiences;
            #endregion Process WorkExperiences */

            #region /* Process Educations
            var educations = new List<Education>();
            if (document.Fields.TryGetValue("Education", out var educationFields) && educationFields.FieldType == DocumentFieldType.List)
            {
                foreach (DocumentField item in educationFields.ValueList)
                {
                    if (item.FieldType == DocumentFieldType.Dictionary)
                    {
                        var education = new Education();
                        foreach (KeyValuePair<string, DocumentField> subItem in item.ValueDictionary)
                        {
                            var fieldValue = subItem.Value?.ValueString ?? string.Empty;
                            switch (subItem.Key.ToLowerInvariant())
                            {
                                case var key when key == nameof(Education.SchoolName).ToLowerInvariant():
                                    education.SchoolName = fieldValue;
                                    break;

                                case var key when key == nameof(Education.Majority).ToLowerInvariant():
                                    education.Majority = fieldValue;
                                    break;

                                case var key when key == nameof(Education.Level).ToLowerInvariant():
                                    education.Level = fieldValue;
                                    break;

                                case var key when key == nameof(Education.Date).ToLowerInvariant():
                                    education.Date = fieldValue;
                                    break;
                            }
                        }
                        educations.Add(education);
                    }
                }
            }
            resume.Educations = educations;
            #endregion Process Educations */

            resume.RawTechnicalSkills = ExtractionFielListString(document, nameof(Resume.TechnicalSkills));
            resume.TechnicalSkills = MakupDataListString(resume.RawTechnicalSkills);
            resume.Certifications = ExtractionFielListString(document, nameof(Resume.Certifications));
            resume.Awards = ExtractionFielListString(document, nameof(Resume.Awards));

            return resume;
        }

        private List<string> ExtractionFielListString(AnalyzedDocument document, string fieldName)
        {
            var results = new List<string>();
            if (document.Fields.TryGetValue(fieldName, out var fields) && fields.FieldType == DocumentFieldType.List)
            {
                foreach (DocumentField item in fields.ValueList)
                {
                    if (item.FieldType == DocumentFieldType.Dictionary)
                    {
                        foreach (KeyValuePair<string, DocumentField> subItem in item.ValueDictionary)
                        {
                            var fieldValue = subItem.Value?.ValueString ?? string.Empty;
                            switch (subItem.Key.ToLower())
                            {
                                case "name":
                                    results.Add(fieldValue);
                                    break;
                            }
                        }
                    }
                }
            }
            return results;
        }

        private List<string> MakupDataListString(List<string> inputData)
        {
            var results = new List<string>();

            foreach (var item in inputData)
            {
                var rawMakeUpData = item.Split(',').Select(str => str.TrimStart('(', ',').TrimEnd(')', ',').Trim()).Where(str => !string.IsNullOrWhiteSpace(str)).Select(str => str.Replace("\n", "\\n")).ToList();
                if (rawMakeUpData.Any())
                    results.AddRange(rawMakeUpData);
            }

            return results;
        }
    }
}
