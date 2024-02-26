public class InfixToPrefixConverter
{

    private static readonly Dictionary<char, int> Precedence = new Dictionary<char, int>
    {
        {'+', 1}, {'-', 1},
        {'*', 2}, {'/', 2},
        {'^', 3}
    };

    //function to convert infix form to prefix form
    public static string[] InfixToPrefix(string[] infix)
    {
        Stack<string> operators = new Stack<string>();
        Stack<string> operands = new Stack<string>();

        Array.Reverse(infix);

        foreach (string token in infix)
        {
            if (IsOperand(token))
            {
                operands.Push(token);
            }
            else if (token == ")")
            {
                operators.Push(token);
            }
            else if (token == "(")
            {
                while (operators.Peek() != ")")
                {
                    operands.Push(operators.Pop());
                }
                operators.Pop(); // Pop the ")"
            }
            else if (IsOperator(token))
            {
                while (operators.Count > 0 && operators.Peek() != ")" && Precedence[operators.Peek()[0]] > Precedence[token[0]])
                {
                    operands.Push(operators.Pop());
                }
                operators.Push(token);
            }
        }

        while (operators.Count > 0)
        {
            operands.Push(operators.Pop());
        }

        return operands.ToArray();
    }

    private static bool IsOperand(string token)
    {
        double result;
        return double.TryParse(token, out result);
    }

    public static bool IsOperator(string token)
    {
        return token.Length == 1 && (token[0] == '+' || token[0] == '-' || token[0] == '*' || token[0] == '/' || token[0] == '^');
    }
}