﻿namespace Common.Dtos
{
    public class LoginResponseDto: BaseResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
