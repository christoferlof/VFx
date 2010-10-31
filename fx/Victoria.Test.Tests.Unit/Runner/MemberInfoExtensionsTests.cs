using System;
using System.Linq;
using System.Reflection;
using Victoria.Test.Runner;

namespace Victoria.Test.Tests.Unit.Runner {
    public class MemberInfoExtensionsTests {

        private MemberInfo GetMember(string memberName) {
            return typeof(MaybeMarked).GetMember(memberName).Single();
        }

        [Fact]
        public void should_tell_if_member_is_marked_with_attribute() {
            var method = GetMember("Marked");
            Assert.True(method.IsMarkedWith<MyAttribute>());
        }

        [Fact]
        public void should_tell_if_member_is_not_marked_with_attribute() {
            var method = GetMember("NotMarked");
            Assert.True(!method.IsMarkedWith<MyAttribute>());
        }

        public class MaybeMarked {
            
            [My]
            public void Marked(){}
            public void NotMarked(){}
        }

        public class MyAttribute : Attribute {}
    }
}