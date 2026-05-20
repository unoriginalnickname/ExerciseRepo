using System;
using System.Collections.Generic;
using System.Text;

namespace Övning___4.ViewModel
{
    public class OperationResult
    {
        public bool Success { get; }
        public string Message { get; }
        public object? Data { get; }

        public OperationResult(bool success, string message, object? data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static OperationResult Ok(string message, object? data = null) => new(true, message, data);
        public static OperationResult Fail(string message) => new(false, message);
    }
}
