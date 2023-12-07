namespace TheQuestion.Models.Answer
{
    public class AnswerList
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }
}
