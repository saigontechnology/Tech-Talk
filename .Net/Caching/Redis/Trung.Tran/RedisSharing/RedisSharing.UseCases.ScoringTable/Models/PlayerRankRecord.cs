namespace RedisSharing.UseCases.ScoringTable.Models
{
    public class PlayerRankRecord
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public DateTimeOffset RecordedTime { get; set; }
        public double Score { get; set; }
    }
}
