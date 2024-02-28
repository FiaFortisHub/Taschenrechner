using System.Text.RegularExpressions;

namespace Taschenrechner;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Geben Sie eine Rechnung ein (z.B.: 4 + 5,3 * -2), oder 'exit' zum beenden");

            string input = Console.ReadLine() ?? string.Empty; // reads term

            // exit case
            if (input.ToLower() == "exit")
            {
                break;
            }

            // validates before calculating
            if (Validate(input) == false)
            {
                continue;
            }

            // term into an array
            string[] termSplit = input.Split(' ');

            Console.WriteLine($"Ergebnis: {Calculate(InfixToPrefixConverter.InfixToPrefix(termSplit))}");
        }
    }

    // function to calculate the term
    public static double Calculate(string[] prefix)
    {
        Stack<double> stack = new Stack<double>();

        for (int i = prefix.Length - 1; i >= 0; i--)
        {
            if (InfixToPrefixConverter.IsOperator(prefix[i]))
            {
                double operand1 = stack.Pop();
                double operand2 = stack.Pop();

                switch (prefix[i])
                {
                    case "+":
                        stack.Push(operand1 + operand2);
                        break;
                    case "-":
                        stack.Push(operand1 - operand2);
                        break;
                    case "*":
                        stack.Push(operand1 * operand2);
                        break;
                    case "/":
                        if (operand2 != 0)
                        {
                            stack.Push(operand1 / operand2);
                        }
                        else
                        {
                            Console.WriteLine("Teilung durch Null nicht möglich");
                            return 1;
                        }
                        break;
                    case "^":
                        stack.Push(Math.Pow(operand1, operand2));
                        break;
                }
            }
            else
            {
                stack.Push(double.Parse(prefix[i]));
            }
        }

        return stack.Pop();
    }

    // function to validate console input
    public static bool Validate(string input)
    {

        string[] termSplit = input.Split(' ');
        int Pointer = 0;
        int spaceCount = Regex.Count(input, " ");
        int count = 0;
        Regex regexItem1 = new Regex(@"^-?[0-9]+(\,[0-9]+)?$");
        Regex regexItem2 = new Regex(@"^\+|-|\*|/|\^$");

        // check for empty or only whitespaces
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Ihre Eingabe ist ungültig.");
            return false;
        }


        // check for complete term
        if (spaceCount % 2 == 1)
        {

            Console.WriteLine("Ihre Eingabe beinhaltet zu viele Whitespaces, oder ist kein vollständiger Term und ist somit ungültig.");
            return false;
        }

        // check for valid term
        foreach (string elem in termSplit)
        {
            bool check = true;
            bool brace = false;

            // check for operand
            if (Pointer % 2 == 0)
            {
                check = regexItem1.IsMatch(elem);
            }

            //check for operator
            else
            {
                check = regexItem2.IsMatch(elem);
            };

            brace = CheckBraces(elem, ref count);

            if (!check && !brace || count < 0)
            {
                Console.WriteLine("Ihre Eingabe entspricht keinem gültigen Term.");
                return false;
            }
            else if (brace)
            {
                continue;
            }
            Pointer += 1;
        }
        
        if (count > 0)
        {
            return false;
        }
        return true;
    }

    public static bool CheckBraces(string elem, ref int count)
    {

        if (elem == "(")
        {
            count += 1;
            return true;
        }
        else if (elem == ")")
        {
            count -= 1;
            return true;
        }
        return false;
    }
}