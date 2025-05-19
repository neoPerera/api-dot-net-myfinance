using APPLICATION.DTOs;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.ComponentModel.DataAnnotations;

namespace APPLICATION.Services
{
    public class ResponseService<T>
    {
        public T Response { get; private set; } = default!; // Initialize with default value to satisfy non-nullable property

        public ResponseService()
        {
            if (typeof(T) == typeof(CommonResponse))
            {
                var response = new CommonResponse
                {
                    StatusCode = 200,
                    Data = new { isValid = true }
                };
                Response = (T)(object)response;
            }
        }

        public ResponseService(object data)
        {
            if (typeof(T) == typeof(CommonResponse))
            {
                var response = new CommonResponse
                {
                    StatusCode = 200,
                    Data = data
                };
                Response = (T)(object)response;
            }
        }

        public ResponseService(Exception ex)
        {
            if (typeof(T) == typeof(CommonResponse))
            {
                var detail = ex.InnerException is PostgresException pgEx
                    ? pgEx.MessageText
                    : ex.Message ?? "ERROR";

                var response = new CommonResponse
                {
                    StatusCode = 400,
                    Data = new { isValid = false, error = new CommonError { Detail = detail } }
                };

                Response = (T)(object)response;
            }
        }
    }
}
