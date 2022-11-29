using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common
{
    public class JWTHelper
    {
        private static string secret = "+TZere4d=C2mzr_dGZZwgRg0eGdDrl";

        public static JWTPayload CreatePayload(int id, string account, int groupId)
        {
            return new JWTPayload { Id = id, Account = account, GroupId = groupId, ExpiredTime = DateTime.UtcNow.AddDays(1) };
        }

        public static JWTPayload DecodePayload(string token)
        {
            return Jose.JWT.Decode<JWTPayload>(token, Encoding.UTF8.GetBytes(secret), Jose.JwsAlgorithm.HS256);
        }

        public static string GetToken(JWTPayload payload)
        {
            return Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), Jose.JwsAlgorithm.HS256); ;
        }
    }
}
