using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Przybliżone_całkowanie.Models
{
    /**
     * f(x) = Ax^2+Bx+C
     */
    public class InputFunction
    {
        public Double A = 0;
        public Double B = 0;
        public Double C = 0;

        public Double F(Double x)
        {
            return ((A * x * x) + (B * x) + C);
        }
        public InputFunction(String input)
        {
            var StartNext = 0;
            A = GetNumberBeforeString("x^2", input, StartNext);
            StartNext = input.IndexOf(A + "x^2");
            B = GetNumberBeforeString("x", input, StartNext + (A + "x^2").Length);
            StartNext = input.IndexOf(B + "x", StartNext);
            C = GetNumberBeforeString("", input, StartNext + (B + "x").Length);
        }
        private Double GetNumberBeforeString(String needle, String haystack, Int32 StartFrom)
        {
            try
            {
                haystack = haystack.Substring(StartFrom);
                if (haystack.Contains(needle) && needle != "")
                {
                    var location = haystack.IndexOf(needle) - 1;
                    if (location >= 0)
                        while (true)
                        {
                            if (location < 0)
                            {
                                var found = Double.TryParse(haystack.Substring(0, haystack.IndexOf(needle)), out double x);
                                return x;
                            }
                            String character = haystack[location].ToString();
                            if (Regex.IsMatch(character, @"^\d+$")) // is number
                            {
                            }
                            else if (character.Equals("+") || character.Equals("-"))
                            {
                                var found = Double.TryParse(haystack.Substring(location, haystack.IndexOf(needle)), out double x);
                                return x;
                            }
                            location--;
                        }
                }
                else if (needle == "")
                {
                    Double.TryParse(haystack, out var x);
                    return x;
                }

            }
            catch (Exception e)
            {
                throw new FormatException("Illegal input format");
            }

            throw new FormatException("Illegal input format");
        }

    }
}
