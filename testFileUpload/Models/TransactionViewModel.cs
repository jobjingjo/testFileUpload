using System.Runtime.Serialization;

namespace testFileUpload.Models
{
    [DataContract]
    public class TransactionViewModel
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        [DataMember(Name = "payment", EmitDefaultValue = false)]
        public string Payment { get; set; }

        [DataMember(Name = "Status", EmitDefaultValue = false)]
        public string Status { get; set; }
    }
}