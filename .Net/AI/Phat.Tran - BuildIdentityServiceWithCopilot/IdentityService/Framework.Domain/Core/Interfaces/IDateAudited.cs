namespace Framework.Domain.Core.Interfaces
{
    public interface IDateAudited
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
