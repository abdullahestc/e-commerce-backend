namespace ECommerceAPI.Common
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public bool NotFound { get; set; }
        public string Message { get; set; }

        public static ServiceResult SuccessResult(string message = null) =>
            new ServiceResult { Success = true, Message = message };

        public static ServiceResult ErrorResult(string message) =>
            new ServiceResult { Success = false, Message = message };

        public static ServiceResult NotFoundResult(string message) =>
            new ServiceResult { NotFound = true, Message = message };
    }
    
    public class ServiceResult<T> : ServiceResult
    {
        public T Data { get; set; }

        public static ServiceResult<T> SuccessResult(T data, string message = null) =>
            new ServiceResult<T> { Success = true, Data = data, Message = message };

        public static ServiceResult<T> ErrorResult(string message) =>
            new ServiceResult<T> { Success = false, Message = message };

        public static ServiceResult<T> NotFoundResult(string message) =>
            new ServiceResult<T> { NotFound = true, Message = message };

    }
}