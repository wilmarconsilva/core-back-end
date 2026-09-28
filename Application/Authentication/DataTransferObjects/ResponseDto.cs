namespace Application.Authentication.DataTransferObjects
{
    public class ResponseDto
    {
        public required AccessTokenDto AccessTokenDto { get; init; }

        public required RefreshTokenDto RefreshTokenDto { get; set; }
    }
}