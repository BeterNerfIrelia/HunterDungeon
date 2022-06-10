using System.Collections.Generic;
using System;

public static class Dice
{
    static List<string> green = new List<string>() { "0", "0", "1", "+1", "+1", "2" };
    static List<string> yellow = new List<string>() { "0", "1", "1", "+1", "+2", "3" };
    static List<string> red = new List<string>() { "0", "+1", "2", "+2", "3", "4" };
    public static List<string> values = new List<string>();
    public static int rollValue;

    public static int roll(DiceType dt)
    {
        var random = new Random();
        values.Clear();
        int val = 0;
        int index;
        bool running = true;
        while (running)
        {
            index = random.Next(green.Count);
            switch (dt)
            {
                case DiceType.GREEN:
                    {
                        if (green[index].StartsWith("+"))
                        {
                            //val += (green[index][1] - 48);
                            val += int.Parse(green[index][1].ToString());
                        }
                        else
                        {
                            //val += (green[index][0] - 48);
                            val += int.Parse(green[index]);
                            running = false;
                        }
                        values.Add(green[index]);
                        break;
                    }
                case DiceType.YELLOW:
                    {
                        if (yellow[index].StartsWith("+"))
                        {
                            //val += (yellow[index][1] - 48);
                            val += int.Parse(yellow[index][1].ToString());
                        }
                        else
                        {
                            //val += (yellow[index][0] - 48);
                            val += int.Parse(yellow[index]);
                            running = false;
                        }
                        values.Add(yellow[index]);
                        break;
                    }
                case DiceType.RED:
                    {
                        if (red[index].StartsWith("+"))
                        {
                            //val += (red[index][1] - 48);
                            val += int.Parse(red[index][1].ToString());
                        }
                        else
                        {
                            //val += (red[index][0] - 48);
                            val += int.Parse(red[index]);
                            running = false;
                        }
                        values.Add(red[index]);
                        break;
                    }
            }
        }

        rollValue = val;
        return val;
    }
}
