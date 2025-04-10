using APPLICATION.DTOs;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;

namespace APPLICATION.Services
{
    public class ResponseService<T>
    {
        public T Response { get; private set; }
        public ObjectResult Result { get; private set; }

        public ResponseService()
        {
            if (typeof(T) == typeof(AddRefResponse))
            {
                var response = new AddRefResponse
                {
                    StatusCode = 200,
                    Data = new { isValid = true }
                };

                Response = (T)(object)response;
                Result = new ObjectResult(response)
                {
                    StatusCode = response.StatusCode
                };
            }
        }

        public ResponseService(Exception ex)
        {
            if (typeof(T) == typeof(AddRefResponse))
            {
                var detail = ex.InnerException is PostgresException pgEx
                    ? pgEx.MessageText
                    : ex.Message ?? "ERROR";

                var response = new AddRefResponse
                {
                    StatusCode = 400,
                    Data = new { isValid = false, error = new RefError { Detail = detail } }
                };

                Response = (T)(object)response;
                Result = new ObjectResult(response)
                {
                    StatusCode = response.StatusCode
                };
            }
        }
    }
}
