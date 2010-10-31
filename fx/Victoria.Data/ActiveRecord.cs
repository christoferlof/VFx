using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Victoria.Data {
    [DataContract]
    public class ActiveRecord<TRecord> where TRecord : ActiveRecord<TRecord>, new() {

        private static readonly object _lock = new object();

        private static readonly string FileName = typeof(TRecord).Name.ToLower() + "s.json"; //lame pluralization.. 

        private static readonly ObservableCollection<TRecord> _all = new ObservableCollection<TRecord>();

        public static ObservableCollection<TRecord> All {
            get { return _all; }
        }

        public static void Add(TRecord record) {
            All.Add(record);
        }

        public static void SaveChanges() {
            var data = new DataContainer<TRecord>();
            foreach (var trip in All) {
                data.Records.Add(trip);
            }
            WriteObject(data);
        }

        public static void Clear() {
            All.Clear();
        }

        private static void WriteObject(DataContainer<TRecord> list) {
            lock (_lock) {
                using (var file = GetFile(FileMode.Create)) {
                    GetSerializer().WriteObject(file, list);
                }
            }
        }

        private static DataContractJsonSerializer GetSerializer() {
            return new DataContractJsonSerializer(typeof(DataContainer<TRecord>));
        }

        private static IsolatedStorageFileStream GetFile(FileMode fileMode) {
            var file = IsolatedStorageFile
                .GetUserStoreForApplication()
                .OpenFile(FileName, fileMode);
            return file;
        }

        public static void Load() {
            lock (_lock) {
                using (var file = GetFile(FileMode.OpenOrCreate)) {
                    All.Clear();
                    var list = ReadObject(file);
                    if (list != null) {
                        list.Records.ForEach(t => All.Add(t));
                    }
                }
            }
        }

        private static DataContainer<TRecord> ReadObject(Stream file) {
            var serializer = GetSerializer();
            return serializer.ReadObject(file) as DataContainer<TRecord>;
        }

    }
}