namespace BMBSOFT.GIS.CORE
{
    public class ResponseData
    {
        public ResponseData()
        {
        }
        public object Content { get; set; }
        public string Err { get; set; }
    }
    public class ResponseErrorData{
        public ResponseErrorData()
        {
            
        }
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}
