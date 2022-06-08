using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCode
{
    static string result;
    public static string Generate() 
    {
        result = "";
        while (result.Length < 6)
            result += GetChar(Random.Range(0, 1));
        return result;
    }

    public static char GetChar(int category)
    {
        switch(category)
        {
            case 0:
                return (char)Random.Range('0', '9');
            default:
                return (char)Random.Range('a', 'z');
        }
    }
}
