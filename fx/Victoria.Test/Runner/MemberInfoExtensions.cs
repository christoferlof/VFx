using System.Linq;
using System.Reflection;

namespace Victoria.Test.Runner {
    public static class MemberInfoExtensions {
        public static bool IsMarkedWith<TAttribute>(this MemberInfo memberInfo) {
            return memberInfo
                .GetCustomAttributes(typeof (TAttribute), false)
                .Any();
        }
    }
}