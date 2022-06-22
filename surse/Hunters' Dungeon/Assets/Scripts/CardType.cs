public enum CardType
{
    WEAPON_MELEE,
    WEAPON_RANGED,
    ITEM,
    OTHER
};

public static class CardTypeToString
{
    public static string CardToString(CardType ct)
    {
        return ct switch
        {
            CardType.WEAPON_MELEE => "Melee Weapon",
            CardType.WEAPON_RANGED => "Ranged Weapon",
            CardType.ITEM => "Item",
            CardType.OTHER => "Other",
            _ => "Other"
        };
    }
}