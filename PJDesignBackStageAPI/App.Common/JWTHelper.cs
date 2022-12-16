using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common
{
    public class JwtHelper
    {
        public static JwtPayload CreatePayload(int id, string account, int groupId)
        {
            return new JwtPayload { Id = id, Account = account, GroupId = groupId, ExpiredTime = DateHelper.GetNowDate().AddDays(1) };
        }

        public static JwtPayload DecodePayload(string token)
        {
            return Jose.JWT.Decode<JwtPayload>(token, Encoding.UTF8.GetBytes(AppSettingHelper.GetSection("TokenSecret")?.Value ?? ""), Jose.JwsAlgorithm.HS256);
        }

        public static string GetToken(JwtPayload payload)
        {
            return Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(AppSettingHelper.GetSection("TokenSecret")?.Value ?? ""), Jose.JwsAlgorithm.HS256); ;
        }
    }
}
