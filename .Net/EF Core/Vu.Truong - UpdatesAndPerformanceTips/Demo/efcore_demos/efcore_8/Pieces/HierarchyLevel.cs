
using Microsoft.SqlServer.Types;

namespace efcore_demos.Pieces;
internal class HierarchyLevel : IExample
{
    private readonly DemoDbContext _dbContext;

    public HierarchyLevel(DemoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        //// Get nodes and display it by level
        //for(short level = 0; level <= 4; level++)
        //{
        //    var documents = await _dbContext.Documents.Where(doc => doc.HierarchyPath.GetLevel() == level).ToListAsync(cancellationToken);
        //    documents.Select(x => x.Name).Dump($"Node level {level}");
        //}

        //// get all descendant node of a declared hierarchy
        //var hierarchyId = HierarchyId.Parse("/1/");

        //var allDocuments = await _dbContext.Documents
        //    .Where(x => x.HierarchyPath != hierarchyId && x.HierarchyPath.IsDescendantOf(hierarchyId))
        //    .OrderBy(x => x.HierarchyPath.GetLevel())
        //    .ThenBy(x => x.HierarchyPath)
        //    .ToListAsync(cancellationToken);

        //foreach (var document in allDocuments)
        //{
        //    Console.WriteLine($"Document: {document.Name} - Level: {document.HierarchyPath.GetLevel()}, {document.HierarchyPath}");
        //}

        List<DocumentEntity> currentDocuments = null;
        HierarchyId currentHierarchyId = null;

        do
        {
            try
            {
                var input = "/";
                HierarchyId hierarchyId = null;
                if (currentDocuments is not null)
                {
                    Console.Write("Enter a node hierarchy ('x' to end, 'z' to up 1 level, 'a' to add): ");
                    input = Console.ReadLine();

                    if (input == "x")
                    {
                        break;
                    }

                    if (input == "z" && currentHierarchyId is not null)
                    {
                        if (currentHierarchyId == HierarchyId.GetRoot())
                        {
                            Console.WriteLine("This is start of hierarchy.");
                            continue;
                        }

                        hierarchyId = currentHierarchyId.GetAncestor(1);
                    }
                }

                if (hierarchyId is null)
                {
                    if (int.TryParse(input, out int index))
                    {
                        if (currentDocuments[index].BranchType == DocumentBranchType.File)
                        {
                            Console.WriteLine("This is end of hierarchy.");
                            continue;
                        }

                        hierarchyId = currentDocuments[index]?.HierarchyPath;
                    }
                    else
                    {
                        hierarchyId = HierarchyId.Parse(input);
                    }
                }

                currentHierarchyId = hierarchyId;

                currentDocuments = await _dbContext.Documents
                    .Where(x => x.HierarchyPath != hierarchyId && x.HierarchyPath.IsDescendantOf(hierarchyId) && x.HierarchyPath.GetAncestor(1) == hierarchyId)
                    .OrderBy(x => x.HierarchyPath.GetLevel())
                    .ThenBy(x => x.HierarchyPath)
                    .ToListAsync(cancellationToken);

                for (var i = 0; i < currentDocuments.Count; i++)
                {
                    var document = currentDocuments[i];
                    Console.WriteLine($"{i}. {(document.BranchType)}: {document.Name} - Level: {document.HierarchyPath.GetLevel()}, {document.HierarchyPath}");
                }
            }
            catch (Exception ex)
            {
                ex.Dump();
            }
        }
        while (true);
    }
}
