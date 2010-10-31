using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using Victoria.Data;
using Victoria.Test;

namespace Driverslog.Tests.Unit.Models {
    public class ActiveRecordTests {

        public ActiveRecordTests() {
            FakeRecord.Clear();
            FakeRecord.SaveChanges();
        }

        
        public class FakeRecord : ActiveRecord<FakeRecord> {
            
            public bool Saved {get;set;}

            public override void OnSaveChanges() {
                Saved = true;
            }
        }

        [Fact]
        public void should_persist_on_save_changes() {
            FakeRecord.Add(new FakeRecord());
            FakeRecord.SaveChanges();

            FakeRecord.All.Clear();

            FakeRecord.Load();
            Assert.Equal(1, FakeRecord.All.Count);
        }

        [Fact]
        public void should_add_record_to_unit_of_work() {
        
            FakeRecord.Add(new FakeRecord());

            Assert.Equal(1, FakeRecord.All.Count);

        }

        [Fact]
        public void should_clear_unit_of_work() {
            FakeRecord.Add(new FakeRecord());
            FakeRecord.Clear();

            Assert.Equal(0, FakeRecord.All.Count);
        }

        [Fact]
        public void should_serialize_unit_of_work_to_isostore() {
            FakeRecord.SaveChanges(); //should create file

            using (var file = IsolatedStorageFile
                .GetUserStoreForApplication()
                .OpenFile("fakerecords.json", FileMode.Open)) { 
            
                Assert.True(file is IsolatedStorageFileStream);
            }
        }

        [Fact]
        public void should_call_onsave_on_record_when_saving_changes() {
            var record = new FakeRecord();
            FakeRecord.Add(record);
            FakeRecord.SaveChanges();

            Assert.True(record.Saved);
        }
    }
}