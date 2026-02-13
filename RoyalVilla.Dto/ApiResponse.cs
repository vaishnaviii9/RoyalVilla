using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyalVilla.Dto
{
    public class ApiResponse<TData>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public TData? Data { get; set; }
        public object? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse<TData> Create(bool success, int statusCode, string message, TData? data = default, object? errors = null)
        {
            return new ApiResponse<TData>
            {
                Success = success,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = errors
            };
        }
        // Returns a successful response (200 OK) with data.
        // Used when a request completes successfully and returns a result.
        public static ApiResponse<TData> Ok(TData data, string message) =>
            Create(true, 200, message, data);

        // Returns a successful response (201 Created) with data.
        // Commonly used after creating a new resource.
        public static ApiResponse<TData> CreatedAt(TData data, string message) =>
            Create(true, 201, message, data);

        // Returns a successful response (204 No Content).
        // Used when an operation succeeds but there is no data to return.
        public static ApiResponse<TData> NoContent(string message = "Operation completed successfully") =>
            Create(true, 204, message);

        // Returns a failure response (404 Not Found).
        // Used when the requested resource does not exist.
        public static ApiResponse<TData> NotFound(string message = "Resource not found") =>
            Create(false, 404, message);

        // Returns a failure response for invalid input.
        // Used when the client sends bad or invalid request data.
        // Optional `errors` can include validation details.
        public static ApiResponse<TData> BadRequest(string message, object? errors = null) =>
            Create(false, 404, message, errors: errors);

        // Returns a failure response (409 Conflict).
        // Used when a request conflicts with the current state of the resource
        // (e.g., duplicate entries or version conflicts).
        public static ApiResponse<TData> Conflict(string message) =>
            Create(false, 409, message);

        // Returns a failure response with a custom HTTP status code.
        // Used for handling generic or unexpected errors where a specific helper
        // method (e.g., BadRequest, NotFound, Conflict) is not applicable.
        // Optional `errors` can be used to include detailed error information.
        public static ApiResponse<TData> Error(int statusCode, string message, object? errors = null) =>
            Create(false, statusCode, message, errors: errors);


    }
}