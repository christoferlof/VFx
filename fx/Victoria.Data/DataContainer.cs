using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Victoria.Data {
    [DataContract]
    public class DataContainer<TRecord> {

        public DataContainer() {
            Records = new List<TRecord>();
        }

        [DataMember]
        public List<TRecord> Records { get; set; }
    }
}