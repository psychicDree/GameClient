public interface INetworkData
{
    string DataType { get; }
    byte[] ToByteArray();
}

[Serializable]
public class NetworkTransform : INetworkData
{
    public float[] Position { get; set; }
    public float[] Rotation { get; set; }

    public string DataType => "NetworkTransform";

    public byte[] ToByteArray()
    {
        using (var memoryStream = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, this);
            return memoryStream.ToArray();
        }
    }
}

[Serializable]
public class NetworkMessage : INetworkData
{
    public string Message { get; set; }

    public string DataType => "NetworkMessage";

    public byte[] ToByteArray()
    {
        using (var memoryStream = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, this);
            return memoryStream.ToArray();
        }
    }
}
