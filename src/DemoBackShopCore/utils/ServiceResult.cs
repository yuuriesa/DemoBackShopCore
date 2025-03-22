namespace DemoBackShopCore.Utils
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }

        public static ServiceResult<T> SuccessResult(T data, int statusCode = 200)
        => new ServiceResult<T>() {Success = true, Data = data, StatusCode = statusCode};

        public static ServiceResult<T> ErrorResult(string message, int statusCode)
        => new ServiceResult<T>() {Success = false, Message = message, StatusCode = statusCode};
    }
}