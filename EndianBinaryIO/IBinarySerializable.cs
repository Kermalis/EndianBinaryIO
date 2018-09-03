namespace EndianBinaryIO
{
    public interface IBinarySerializable
    {
        void Read(EndianBinaryReader er);
        void Write(EndianBinaryWriter ew);
    }
}
