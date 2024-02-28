using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Taschenrechner;

// TODO: unbekannter Operator abfangen (check function mit allem was vorher abgefangen wird), ReadMe, packet erstellen
class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Geben Sie eine Rechnung ein (z.B.: 4 + 5,3 * -2), oder 'exit' zum beenden");
            
            string input = Console.ReadLine(); // reads term

            // validates before calculating
            if (Validate(input) == false) 
            {
                continue;
            }

            // exit case
            if (input.ToLower() == "exit")
            {
                break;
            }

            // term into an array
            string[] termSplit = input.Split(' ');

            /*foreach (string elem in termSplit)
            {
                Console.WriteLine(elem);
            }*/

            // TODO: oneline
            double result = Calculate(InfixToPrefixConverter.InfixToPrefix(termSplit));
            Console.WriteLine($"Ergebnis: {result}");
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
                            throw new DivideByZeroException("Teilung durch Null nicht möglich");
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
    public static bool Validate (string input)
    {
        // check for empty or only whitespaces
        if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Ihre Eingabe ist ungültig.");
                return false;
            }
        
        int spaceCount = Regex.Count(input, " "); 
        
        // check for complete term
        if (spaceCount % 2 == 0) 
        {
            string[] termSplit = input.Split(' ');

            // check for valid term
            foreach (string elem in termSplit) 
            {
                int i = 0;
                bool check = true;
                
                // check for operand
                if (i % 2 == 0)
                {
                    Regex regexItem1 = new Regex(@"^-?[0-9][0-9,^\s]+$");
                    check = regexItem1.IsMatch(elem);
                    i += 1;
                }

                //check for operator
                else {
                    Regex regexItem2 = new Regex(@"^\+|-|\*|/$");
                    check = regexItem2.IsMatch(elem);
                    i += 1;
                 } ;
                
                if (check == false)
                {
                    Console.WriteLine("Ihre Eingabe entspricht keinem gültigen Term. Bitte kontrollieren Sie auf Vollständigkeit und korrektes spacing.");
                    return false;
                }
            }
        }
        else 
        {
            Console.WriteLine("Ihre Eingabe beinhaltet zu viele Whitespaces, oder ist kein vollständiger Term und ist somit ungültig.");
            return false;
        }

        return true;
    }
}