using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Victoria.Test.Runner;

namespace Victoria.Test.Tests.Unit.Runner {

    //TODO: Ignore attribute for base classes to allow test method inheritance
    public class WorkItemTests : ContextSpecification {

        protected MemberInfo Method;
        protected ManualResetEvent Handle;
        protected bool Run;
        protected WorkItem CompletedWorkItem;

        public override void Context() {
            Handle = new ManualResetEvent(false);
        }

        public override void Because() {
            WorkItem.Enqueue(Method, new WorkItemTestInstanceProvider(),
                (s, e) => {
                    Run = true; 
                    Handle.Set();
                    CompletedWorkItem = (WorkItem)s;
                });
            Handle.WaitOne(1000);
        }

        public class FailingWorkItemTests : WorkItemTests {
            public override void Context() {
                base.Context();
                Method = typeof(ClassForWorkItemTest).GetMember("Kaboom").Single();
            }

            [Fact]
            public void should_report_result() {
                Assert.True(!CompletedWorkItem.Result);
            }

            [Fact]
            public void should_provide_information_about_exception() {
                Assert.True(CompletedWorkItem.ResultMessage.Contains("InvalidProgramException"));
            }

            [Fact]
            public void should_run_enqueued_work_item() {
                Assert.True(Run);
            }

            [Fact]
            public void should_provide_name_of_the_completed_method() {
                Assert.Equal(Method.Name, CompletedWorkItem.TestMethod.Name);
            }

            [Fact]
            public void should_provide_a_description_of_the_result() {
                Assert.True(!String.IsNullOrEmpty(CompletedWorkItem.ResultMessage));
            }
        }

        public class SucceedingWorkItemTests : WorkItemTests {
            public override void Context() {
                base.Context();
                Method = typeof(ClassForWorkItemTest).GetMember("RunMe").Single();
            }

            [Fact]
            public void should_report_result() {
                Assert.True(CompletedWorkItem.Result);
            }
            [Fact]
            public void should_run_enqueued_work_item() {
                Assert.True(Run);
            }

            [Fact]
            public void should_provide_name_of_the_completed_method() {
                Assert.Equal(Method.Name, CompletedWorkItem.TestMethod.Name);
            }

            [Fact]
            public void should_provide_a_description_of_the_result() {
                Assert.True(!String.IsNullOrEmpty(CompletedWorkItem.ResultMessage));
            }
        }

        public class ClassForWorkItemTest {
            public void RunMe() { }
            public void Kaboom() {
                throw new InvalidProgramException();
            }
        }

        public class WorkItemTestInstanceProvider : ITestClassInstanceProvider {
            public object CreateInstance(Type testClass) {
                return new ClassForWorkItemTest();
            }
        }
    }
}