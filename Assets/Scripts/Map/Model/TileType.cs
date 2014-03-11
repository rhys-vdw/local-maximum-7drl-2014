using System;

[Flags]
public enum TileType
{
    None        = 0,
    Floor       = 1 << 1,
    Blocked     = 1 << 2,
    Water       = 1 << 3,
    Lava        = 1 << 4,
    Destroyed   = 1 << 5,
    OutOfBounds = 1 << 6
}
