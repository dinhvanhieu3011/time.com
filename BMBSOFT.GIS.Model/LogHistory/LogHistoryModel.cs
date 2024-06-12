namespace BASE.Model.LogHistory
{
    public class LogHistoryModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Action { get; set; }
        public string Description { get; set; }
    }
}