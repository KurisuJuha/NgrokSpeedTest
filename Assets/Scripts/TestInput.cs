using JuhaKurisu.PopoTools.ByteSerializer;

namespace JuhaKurisu.NgrokSpeedTest
{
    public class TestInput : IpopoSerialize
    {
        public bool right;
        public bool left;
        public bool up;
        public bool down;
        public (int x, int y) inputVec
        {
            get
            {
                int x = 0;
                int y = 0;

                if (right) x++;
                if (left) x--;
                if (up) y++;
                if (down) y--;

                return (x, y);
            }
        }

        public void Deserialize(DataReader reader)
        {
            right = reader.ReadBoolean();
            left = reader.ReadBoolean();
            up = reader.ReadBoolean();
            down = reader.ReadBoolean();
        }

        public void Serialize(DataWriter writer)
        {
            writer.Append(right)
                .Append(left)
                .Append(up)
                .Append(down);
        }
    }
}