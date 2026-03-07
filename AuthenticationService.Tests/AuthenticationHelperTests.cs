using System;
using Xunit;
using AuthenticationService.Data.Auth;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace AuthenticationService.Tests
{
    public class AuthenticationHelperTests
    {
        private readonly AuthenticationHelper _authHelper;

        public AuthenticationHelperTests()
        {
            // Mock konfiguracija
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Secret", "URIS_2026_SUPER_SECRET_KEY_123456789!" },
                { "Jwt:LifespanMinutes", "60" }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _authHelper = new AuthenticationHelper(configuration);
        }

        // Test 1 — HashPassword vraca hash koji nije isti kao originalna lozinka
        [Fact]
        public void HashPassword_ReturnsHashedPassword_NotPlainText()
        {
            var password = "testPassword123";
            var hash = _authHelper.HashPassword(password);
            Assert.NotEqual(password, hash);
            Assert.NotNull(hash);
        }

        // Test 2 — VerifyPassword vraca true za ispravnu lozinku
        [Fact]
        public void VerifyPassword_WithCorrectPassword_ReturnsTrue()
        {
            var password = "testPassword123";
            var hash = _authHelper.HashPassword(password);
            var result = _authHelper.VerifyPassword(password, hash);
            Assert.True(result);
        }

        // Test 3 — VerifyPassword vraca false za pogresnu lozinku
        [Fact]
        public void VerifyPassword_WithWrongPassword_ReturnsFalse()
        {
            var password = "testPassword123";
            var hash = _authHelper.HashPassword(password);
            var result = _authHelper.VerifyPassword("pogresnaLozinka", hash);
            Assert.False(result);
        }

        // Test 4 — GenerateJwtToken vraca token sa ispravnim podacima
        [Fact]
        public void GenerateJwtToken_ReturnsValidToken()
        {
            var authUser = new AuthenticationService.Model.AuthUser
            {
                IdUser = 1,
                Username = "testuser",
                Role = "CUSTOMER"
            };

            var result = _authHelper.GenerateJwtToken(authUser);

            Assert.NotNull(result);
            Assert.NotNull(result.Token);
            Assert.Equal("testuser", result.Username);
            Assert.Equal("CUSTOMER", result.Role);
            Assert.Equal(1, result.IdUser);
        }

        // Test 5 — Token expiry je u buducnosti
        [Fact]
        public void GenerateJwtToken_ExpiresInFuture()
        {
            var authUser = new AuthenticationService.Model.AuthUser
            {
                IdUser = 1,
                Username = "testuser",
                Role = "ADMIN"
            };

            var result = _authHelper.GenerateJwtToken(authUser);

            Assert.True(result.ExpiresAt > DateTime.UtcNow);
        }
    }
}