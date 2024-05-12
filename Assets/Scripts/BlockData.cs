namespace BlockData {
    public struct BlockNabours {
        public bool North, South, West, East, Up, Down;

        public bool[] Solids {
            get => new bool[6] { North, South, West, East, Up, Down };
            set {
                North = value[0];
                South = value[1];
                West  = value[2];
                East  = value[3];
                Up    = value[4];
                Down  = value[5];
            }
        }
        public BlockNabours(bool[] solids) {
            North = solids[0];
            South = solids[1];
            West  = solids[2];
            East  = solids[3];
            Up    = solids[4];
            Down  = solids[5];
        }
    }
}
