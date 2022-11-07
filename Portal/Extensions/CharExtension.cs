using System;

namespace Portal.Extensions
{
    public static class CharExtension
    {
        //Método para verificar se é impar
        public static bool IsOddNumber(this Char @char)
        {
            return @char.ToInt() % 2 != 0;
        }

        //Método para converter para inteiro
        public static int ToInt(this Char @char)
        {
            return int.Parse(@char.ToString());
        }
    }
}
