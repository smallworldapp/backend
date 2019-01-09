using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using SmallWorld.Auth;

namespace SmallWorld.Models.Providers
{
    public class AuthProvider
    {
        private static readonly HashAlgorithm HashAlgorithm = SHA256.Create();

        private readonly IDictionary<Guid, Session> sessions = new ConcurrentDictionary<Guid, Session>();
        private readonly AdminCredentials adminCredentials;

        public AuthProvider(SmallWorldOptions options)
        {
            adminCredentials = options.AdminLogin;
        }

        public bool AuthorizeAdmin(string email, string password)
        {
            if (!string.Equals(email, adminCredentials.Email, StringComparison.OrdinalIgnoreCase))
                return false;

            var hash = Convert.ToBase64String(HashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password)));

            if (!string.Equals(hash, adminCredentials.PasswordHash, StringComparison.Ordinal))
                return false;

            return true;
        }

        public void AddSession(Session session)
        {
            sessions[session.Token] = session;
        }

        public Session GetSession(HttpContext context)
        {
            var header = context.Request.Headers["auth-token"];

            if (!header.Any())
                return null;

            if (!Guid.TryParse(header.First(), out Guid id))
                return null;

            if (!sessions.TryGetValue(id, out Session session))
                return null;

            if (session.IsExpired)
            {
                sessions.Remove(id);
                return null;
            }

            session.Update();
            return session;
        }
    }
}