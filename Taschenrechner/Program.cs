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

            // check for empty or only whitespaces
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Ihre Eingabe ist ungültig.");
                continue;
            }

            // exit case
            if (input.ToLower() == "exit")
            {
                break;
            }

            // term into an array
            string[] termSplit = input.Split(' ');

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
}