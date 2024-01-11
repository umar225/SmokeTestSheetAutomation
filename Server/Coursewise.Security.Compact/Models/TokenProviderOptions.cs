using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Security.Compact.Models
{
    public class TokenProviderOptions
    {
        /// <summary>
        /// The issuer (iss) claim for generated tokens
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Intended audience for the token.
        /// aud claim of the generated token
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Expiration time for the generated token
        /// </summary>
        public int Expiration { get; set; } = 30;

        /// <summary>
        /// The signing key to use when generating tokens.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Generates a random value (nonce) for each generated token.
        /// </summary>
        /// <remarks>The default nonce is a random GUID.</remarks>
        public Func<Task<string>> NonceGenerator { get; set; }
            = () => Task.FromResult(Guid.NewGuid().ToString());
    }
}
