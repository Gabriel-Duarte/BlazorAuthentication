﻿namespace BlazorAuthentication.Client.Model
{
    public class TokenDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiresIn { get; set; }
    }
}
