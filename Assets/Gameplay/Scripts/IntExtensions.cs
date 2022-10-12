using System;

public static class IntExtensions
{
    public static string ToRoman(this int number)
    {
        if ((number < 0) || (number > 10)) throw new ArgumentOutOfRangeException("Insert value betwheen 1 and 10");
        if (number < 1) return string.Empty;
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        throw new ArgumentOutOfRangeException("Something bad happened");
    }
}
