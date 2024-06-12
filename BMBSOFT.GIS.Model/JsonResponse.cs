namespace BASE.Model
{
    public class JsonResponse
    {
        public object Data { get; set; }

        public string Message { get; set; }

        public bool Success { get; set; }

        public string Pager { get; set; }

        public string Id { get; set; }
    }
}
