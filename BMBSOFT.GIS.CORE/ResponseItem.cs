namespace BASE.CORE
{
    public class ResponseItem<T>
    {
        public T Result { get; set; }
        public int? Err { get; set; }
        public ResponseErrorData ResponseError { get; set; }
    }
}
