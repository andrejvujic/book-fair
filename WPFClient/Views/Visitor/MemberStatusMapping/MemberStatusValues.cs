using Core.Models;
using System.Collections.Generic;
using WPFClient.Localization;

namespace WPFClient.Views.Visitor.MemberStatusMapping
{
    public static class MemberStatusValues
    {
        public static List<MemberStatusOption> All
            {
                get =>
                new List<MemberStatusOption>
                {
                    new MemberStatusOption
                    {
                        Value = MemberStatus.R,
                        Label = TranslationSource.Instance["Status_Regular"]
                    },
                    new MemberStatusOption
                    {
                        Value = MemberStatus.V,
                        Label = TranslationSource.Instance["Status_VIP"]
                    }
                };
            }    
    }
}
