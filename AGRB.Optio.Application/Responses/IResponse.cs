﻿namespace RGBA.Optio.Domain.Responses
{
    public interface IResponse
    {
        bool Succeeded { get; set; }

        bool? HasViewPermission { get; set; }

        ICollection<ResponseError> Errors { get; set; }

        ICollection<ResponseMessage> Messages { get; set; }
    }
}