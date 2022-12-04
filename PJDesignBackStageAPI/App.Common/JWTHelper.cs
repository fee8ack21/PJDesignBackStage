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
        public static JWTPayload CreatePayload(int id, string account, int groupId)
        {
            return new JWTPayload { Id = id, Account = account, GroupId = groupId, ExpiredTime = DateHelper.GetNowDate().AddDays(1) };
        }

        public static JWTPayload DecodePayload(string token)
        {
            return Jose.JWT.Decode<JWTPayload>(token, Encoding.UTF8.GetBytes(AppSettingHelper.GetSection("TokenSecret")?.Value ?? ""), Jose.JwsAlgorithm.HS256);
        }

        public static string GetToken(JWTPayload payload)
        {
            return Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(AppSettingHelper.GetSection("TokenSecret")?.Value ?? ""), Jose.JwsAlgorithm.HS256); ;
        }
    }
}
