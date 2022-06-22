using System.Collections.Generic;

public class Trophy
{
    public TrophyType trophyType;
    public int level;
    public List<int> levels = new List<int>() { 0, 1, 2, 3, 5, 8 };
    public int maxLevel = 5;

    public Trophy(TrophyType tp)
    {
        maxLevel = levels.Count;
        trophyType = tp;
        level = 0;
    }

    public Trophy(Trophy trophy)
    {
        trophyType = trophy.trophyType;
        level = trophy.level;
    }
}
