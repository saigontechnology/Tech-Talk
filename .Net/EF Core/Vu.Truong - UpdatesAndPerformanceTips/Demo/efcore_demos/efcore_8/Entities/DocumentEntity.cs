namespace efcore_demos.Entities;
internal class DocumentEntity : BaseEntity
{
    public DocumentEntity()
    {

    }

    public DocumentEntity(string name, string description, DocumentBranchType branchType, HierarchyId hierarchyId)
    {
        Name = name;
        Description = description;
        BranchType = branchType;
        HierarchyPath = hierarchyId;
    }

    public string Name { get; set; }
    public string Description { get; set; }

    public DocumentBranchType BranchType { get; set; }

    public HierarchyId HierarchyPath { get; set; }
}
