// PlayerSave
using System.Collections.Generic;

public class PlayerSave
{
    public int BestScore;

    public int Coin;

    public int LastIndex;

    public int kill;



    public bool[] Items = new bool[]
    {
        true,
        false,
        false,
        false,
        false,
        false,
        false
    };

    public bool[] seen = new bool[]
    {
        false,
        false,
        false,
        false,
        false,
        false,
        false
    };

    public bool SFX = true;

    public bool Music = true;
}
