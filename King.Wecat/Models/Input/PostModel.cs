using System;
using System.Collections.Generic;
using System.Text;

namespace King.Wecat.Models
{
    public class PostModel
    {
        public string DomainId { get; set; }

        public string Signature { get; set; }

        public string Msg_Signature { get; set; }

        public string Timestamp { get; set; }

        public string Nonce { get; set; }

        public string Token { get; set; }

        public string EncodingAESKey { get; set; }

    }
}
