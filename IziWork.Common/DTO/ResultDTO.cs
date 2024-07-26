namespace IziWork.Common.DTO
{
    public class ResultDTO
    {
        public ResultDTO()
        {
            ErrorCodes = new List<int>();
            Messages = new List<string>();
        }
        public List<int> ErrorCodes { get; set; }
        public List<string> Messages { get; set; }
        public object Object { get; set; }
        public bool IsSuccess => !ErrorCodes.Any();
    }

    public class ArrayResultDTO
    {
        public ArrayResultDTO()
        {
            Data = new object { };
            Count = 0;
        }
        public int Count { get; set; }
        public object Data { get; set; }
    }
}
