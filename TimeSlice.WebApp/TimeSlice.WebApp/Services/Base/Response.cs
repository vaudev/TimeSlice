namespace TimeSlice.WebApp.Services.Base
{
    public class Response<T>
    {
        public string Message { get; set; }
        public string ValidationErrors { get; set; }
        public bool Success;
        public T Data;
    }
}
