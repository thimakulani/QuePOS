using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Response
{
    public class ResultDto<T> where T : class
    {
        public T? Data { get; set; } 
        public int StatusCode { get; private set; }
        public string Message { get; set; } = string.Empty!;

        [JsonIgnore]
        public bool isSuccess { get; private set; }
        public ErrorDto? Error { get; private set; }


        public static ResultDto<T> Success(T data, string message, int statusCode)
        {
            return new ResultDto<T> { Data = data, Message = message, StatusCode = statusCode, isSuccess = true };
        }

        public static ResultDto<T> Success(int statusCode)
        {
            return new ResultDto<T> { Data = default, StatusCode = statusCode, isSuccess = true };
        }

        public static ResultDto<T> Fail(ErrorDto errorDto, int statusCode)
        {
            return new ResultDto<T> { Error = errorDto, StatusCode = statusCode, isSuccess = false };
        }

        public static ResultDto<T> Fail(string errorMessage, int statusCode, bool isShow)
        {
            var errorDto = new ErrorDto(errorMessage, isShow);
            return new ResultDto<T> { Error = errorDto, StatusCode = statusCode, isSuccess = false };
        }
    }
}
