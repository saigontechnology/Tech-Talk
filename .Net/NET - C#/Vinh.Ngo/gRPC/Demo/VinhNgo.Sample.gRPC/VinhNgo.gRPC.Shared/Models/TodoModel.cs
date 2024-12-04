using System.Runtime.Serialization;

namespace VinhNgo.gRPC.Shared.Models;

[DataContract]
public class TodoModel
{
    [DataMember(Order = 1)]
    public string Message { get; set; }
}


[DataContract]
public class TodoReply
{
    [DataMember(Order = 1)]
    public string Name { get; set; }
}