using Core.Models;
using System.Collections.Generic;

namespace WPFClient.Views.Visitor.MemberStatusMapping
{
    public static class MemberStatusValues
    {
        public static List<MemberStatusOption> All { get; } =
                new List<MemberStatusOption>
                {
                    new MemberStatusOption
                    {
                        Value = MemberStatus.R,
                        Label = "Redovan"
                    },
                    new MemberStatusOption
                    {
                        Value = MemberStatus.V,
                        Label = "VIP"
                    }
                };
    }
}
