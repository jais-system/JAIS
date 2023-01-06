namespace Spotify.Entities;

public record SpotifyAuthenticationDto(string ClientId, string ClientSecret, string AccessToken, string RefreshToken);