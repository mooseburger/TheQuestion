namespace TheQuestion.Models.Answer
{
    public class PublicAnswer : Data.Models.Answer
    {
        public int LastId { get; set; }
        public bool IsFirst => Id == 1;
        public bool IsLast => Id == LastId;

        public int Previous => Id - 1;
        public int Next => Id + 1;
    }
}
