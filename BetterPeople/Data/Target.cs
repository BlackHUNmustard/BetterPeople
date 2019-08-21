using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace BetterPeople.Data
{
    public class Target
    {
        public ulong ServerId;
        public ulong ClientId;
        public string TargetUser;

        public Target(ulong serverid, ulong clientid, string targetuser)
        {
            ServerId = serverid;
            ClientId = clientid;
            TargetUser = targetuser;
        }

        public Target(ulong serverid, IUser User)
        {
            ServerId = serverid;
            ClientId = User.Id;
            TargetUser = User.Username;
        }
    }
}
