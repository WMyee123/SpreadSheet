// <copyright file="Formula.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <summary>
//   <para>
//     This code is provides to start your assignment.  It was written
//     by Profs Joe, Danny, and Jim.  You should keep this attribution
//     at the top of your code where you have your header comment, along
//     with the other required information.
//   </para>
//   <para>
//     You should remove/add/adjust comments in your file as appropriate
//     to represent your work and any changes you make.
//   </para>
// </summary>

namespace CS3500.Formula;

using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
///   <para>
///     This class represents formulas written in standard infix notation using standard precedence
///     rules.  The allowed symbols are non-negative numbers written using double-precision
///     floating-point syntax; variables that consist of one ore more letters followed by
///     one or more numbers; parentheses; and the four operator symbols +, -, *, and /.
///   </para>
///   <para>
///     Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
///     a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable;
///     and "x 23" consists of a variable "x" and a number "23".  Otherwise, spaces are to be removed.
///   </para>
///   <para>
///     For Assignment Two, you are to implement the following functionality:
///   </para>
///   <list type="bullet">
///     <item>
///        Formula Constructor which checks the syntax of a formula.
///     </item>
///     <item>
///        Get Variables
///     </item>
///     <item>
///        ToString
///     </item>
///   </list>
/// </summary>
public class Formula
{
    /// <summary>
    ///   All variables are letters followed by numbers.  This pattern
    ///   represents valid variable name strings.
    /// </summary>
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

    /// <summary>
    ///     This value stores the phrase, normalized and reduced to be just the tokens with spaces between them,
    ///     allowing for simplified representations of the formula.
    /// </summary>
    private string fullForm;

    /// <summary>
    ///   Initializes a new instance of the <see cref="Formula"/> class.
    ///   <para>
    ///     Creates a Formula from a string that consists of an infix expression written as
    ///     described in the class comment.  If the expression is syntactically incorrect,
    ///     throws a FormulaFormatException with an explanatory Message.  See the assignment
    ///     specifications for the syntax rules you are to implement.
    ///   </para>
    ///   <para>
    ///     Non Exhaustive Example Errors:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///        Invalid variable name, e.g., x, x1x  (Note: x1 is valid, but would be normalized to X1)
    ///     </item>
    ///     <item>
    ///        Empty formula, e.g., string.Empty
    ///     </item>
    ///     <item>
    ///        Mismatched Parentheses, e.g., "(("
    ///     </item>
    ///     <item>
    ///        Invalid Following Rule, e.g., "2x+5"
    ///     </item>
    ///   </list>
    /// </summary>
    /// <param name="formula"> The string representation of the formula to be created.</param>
    public Formula( string formula )
    {
        List<string> tokens = GetTokens(formula); // A list of every token within the formula for analysis
        string prevToken = string.Empty; // The previous token, stored for ensuring the formula is valid
        int parenthesisCount = 0; // A count of the number of open/closing parenthesis, ensuring balance between the two tokens

        // Store and normalize the formula, allowing for proper representation of the tokens and values present within
        fullForm = string.Empty;
        foreach(string currToken in tokens)
        {
            if(double.TryParse(currToken, out double r))
            {
                fullForm += double.Parse(currToken);
            }
            else
            {
                fullForm += currToken.ToUpper();
            }
        }

        // Throw an exception if the formula is empty
        if(tokens.Count == 0)
        {
            throw new FormulaFormatException("Empty Formulas Not Valid");
        }

        string normToken;

        // Count the tokens, checking for validity, if the token is not valid an exception is thrown
        foreach (string token in tokens)
        {
            normToken = token.ToUpper();

            // Count the numbers of opening and closing parenthesis, comparing the counts to ensure balance
            if (normToken == "(")
            {
                parenthesisCount++;
            }
            else if (normToken == ")")
            {
                parenthesisCount--;
            }

            // Check for token validity throwing an exception if the token is not
            if (prevToken != string.Empty && !TokenRulesValid(normToken, prevToken))
            {
                throw new FormulaFormatException("Token not valid");
            }
            else if ((normToken == "+" || normToken == "-" || normToken == "*" || normToken == "/") && prevToken == null)
            {
                throw new FormulaFormatException("Token not Valid");
            }
            else if(!GetVariables().Contains(normToken) && !float.TryParse(normToken, out float r) && !(normToken == "+" || normToken == "-" || normToken == "*" || normToken == "/" || normToken == "(" || normToken == ")"))
            {
                throw new FormulaFormatException("Token not Valid");
            }

            prevToken = normToken; // Reset the previous token to the current one such that the validity can be properly checked
        }

        if(prevToken == "+" || prevToken == "-" || prevToken == "*" || prevToken == "/")
        {
            throw new FormulaFormatException("Formula Cannot End on Operator");
        }

        // Throw an exception if the opening and closing parenthesis counts are not equal
        if (parenthesisCount != 0)
        {
            throw new FormulaFormatException("Opening and Closing Parenthesis are not matching");
        }
    }

    /// <summary>
    ///   <para>
    ///     Returns a set of all the variables in the formula.
    ///   </para>
    ///   <remarks>
    ///     Important: no variable may appear more than once in the returned set, even
    ///     if it is used more than once in the Formula.
    ///   </remarks>
    ///   <para>
    ///     For example, if N is a method that converts all the letters in a string to upper case:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>new("x1+y1*z1").GetVariables() should enumerate "X1", "Y1", and "Z1".</item>
    ///     <item>new("x1+X1"   ).GetVariables() should enumerate "X1".</item>
    ///   </list>
    /// </summary>
    /// <returns> the set of variables (string names) representing the variables referenced by the formula. </returns>
    public ISet<string> GetVariables( )
    {
        HashSet<string> variables = new HashSet<string>();

        foreach(string token in GetTokens(fullForm))
        {
            string normToken = token.ToUpper();

            if (IsVar(normToken))
            {
                variables.Add(normToken);
            }
        }

        return variables;
    }

    /// <summary>
    ///   <para>
    ///     Returns a string representation of a canonical form of the formula.
    ///   </para>
    ///   <para>
    ///     The string will contain no spaces.
    ///   </para>
    ///   <para>
    ///     If the string is passed to the Formula constructor, the new Formula f
    ///     will be such that this.ToString() == f.ToString().
    ///   </para>
    ///   <para>
    ///     All of the variables in the string will be normalized.  This
    ///     means capital letters.
    ///   </para>
    ///   <para>
    ///       For example:
    ///   </para>
    ///   <code>
    ///       new("x1 + y1").ToString() should return "X1+Y1"
    ///       new("X1 + 5.0000").ToString() should return "X1+5".
    ///   </code>
    ///   <para>
    ///     This code should execute in O(1) time.
    ///   <para>
    /// </summary>
    /// <returns>
    ///   A canonical version (string) of the formula. All "equal" formulas
    ///   should have the same value here.
    /// </returns>
    public override string ToString( )
    {
        return fullForm;
    }

    /// <summary>
    ///   Reports whether "token" is a variable.  It must be one or more letters
    ///   followed by one or more numbers.
    /// </summary>
    /// <param name="token"> A token that may be a variable. </param>
    /// <returns> true if the string matches the requirements, e.g., A1 or a1. </returns>
    private static bool IsVar( string token )
    {
        // notice the use of ^ and $ to denote that the entire string being matched is just the variable
        string standaloneVarPattern = $"^{VariableRegExPattern}$";
        return Regex.IsMatch( token, standaloneVarPattern );
    }

    /// <summary>
    ///   <para>
    ///     Given an expression, enumerates the tokens that compose it.
    ///   </para>
    ///   <para>
    ///     Tokens returned are:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>left paren</item>
    ///     <item>right paren</item>
    ///     <item>one of the four operator symbols</item>
    ///     <item>a string consisting of one or more letters followed by one or more numbers</item>
    ///     <item>a double literal</item>
    ///     <item>and anything that doesn't match one of the above patterns</item>
    ///   </list>
    ///   <para>
    ///     There are no empty tokens; white space is ignored (except to separate other tokens).
    ///   </para>
    /// </summary>
    /// <param name="formula"> A string representing an infix formula such as 1*B1/3.0. </param>
    /// <returns> The ordered list of tokens in the formula. </returns>
    private static List<string> GetTokens( string formula )
    {
        List<string> results = [];

        string lpPattern = @"\(";
        string rpPattern = @"\)";
        string opPattern = @"[\+\-*/]";
        string doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        string spacePattern = @"\s+";

        // Overall pattern
        string pattern = string.Format(
                                        "({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                        lpPattern,
                                        rpPattern,
                                        opPattern,
                                        VariableRegExPattern,
                                        doublePattern,
                                        spacePattern);

        // Enumerate matching tokens that don't consist solely of white space.
        foreach ( string s in Regex.Split( formula, pattern, RegexOptions.IgnorePatternWhitespace ) )
        {
            if ( !Regex.IsMatch( s, @"^\s*$", RegexOptions.Singleline ) )
            {
                results.Add( s );
            }
        }

        return results;
    }

    /// <summary>
    ///     Check that the token rules are valid while constructing the Formula object, ensuring that an invalid formula
    ///     is not allowed throughout this process.
    /// </summary>
    /// <param name="token">
    ///     This is the token being inspected to determine the validity of it following the previous token in the formula.
    /// </param>
    /// <param name="prevToken">
    ///     The token that determines the validity of the current token, allowing for proper understanding of the formula's requirements.
    /// </param>
    /// <returns> A boolean statement as to if the tokens present in the formula are in a valid ordering between one another. </returns>
    private bool TokenRulesValid(string token, string prevToken)
    {
        // Check the conditions of when the previous token was a variable
        if (GetVariables().Contains(prevToken))
        {
            // Return true if an operator follows a variable
            if (token == "+" || token == "-" || token == "/" || token == "*")
            {
                return true;
            }

            // Return true if a closing parenthesis follows a variable, however an opening parenthesis cannot follow one
            else if (token == ")")
            {
                return true;
            }
        }

        // Check the conditions for if the previous token was an operator
        else if (prevToken == "+" || prevToken == "-" || prevToken == "/" || prevToken == "*")
        {
            // Return true if a variable or integer follows an operator
            if (GetVariables().Contains(token) || float.TryParse(token, out float i))
            {
                return true;
            }

            // Return true if a parenthesis follows an operator
            else if (token == "(" || token == ")")
            {
                return true;
            }
        }

        // Check the conditions for if the previous token was an opening parenthesis
        else if (prevToken == "(")
        {
            // Return true if a variable or integer follows an opening parenthesis
            if (GetVariables().Contains(token) || float.TryParse(token, out float i))
            {
                return true;
            }

            // Return true if a second opening parenthesis follows one
            else if (token == "(")
            {
                return true;
            }
        }

        // Check the conditions for if the previous token was a closing parenthesis
        else if (prevToken == ")")
        {
            // Return true if an operator follows a closing parenthesis
            if (token == "+" || token == "-" || token == "/" || token == "*")
            {
                return true;
            }

            // Return true if a second closing parenthesis follows one
            else if (token == ")")
            {
                return true;
            }
        }

        // Check the consitions for if the previous token was an integer
        else if (float.TryParse(prevToken, out float i))
        {
            // Return true if an operator follows an integer
            if (token == "+" || token == "-" || token == "/" || token == "*")
            {
                return true;
            }

            // Return true if a closing parenthesis follows an integer
            else if (token == ")")
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///   <para>
    ///     Reports whether f1 == f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are the same.</returns>
    public static bool operator ==(Formula f1, Formula f2) => f1.Equals(f2);

    /// <summary>
    ///   <para>
    ///     Reports whether f1 != f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are not equal to each other.</returns>
    public static bool operator !=(Formula f1, Formula f2) => !f1.Equals(f2);

    /// <summary>
    ///   <para>
    ///     Determines if two formula objects represent the same formula.
    ///   </para>
    ///   <para>
    ///     By definition, if the parameter is null or does not reference
    ///     a Formula Object then return false.
    ///   </para>
    ///   <para>
    ///     Two Formulas are considered equal if their canonical string representations
    ///     (as defined by ToString) are equal.
    ///   </para>
    /// </summary>
    /// <param name="obj"> The other object.</param>
    /// <returns>
    ///   True if the two objects represent the same formula.
    /// </returns>
    public override bool Equals(object? obj)
    {
        // Ensure that the object being compared to is another formula
        if(obj is Formula f)
        {
            // Get the tokens of both formulas
            List<string> thisTokens = GetTokens(fullForm);
            List<string> otherTokens = GetTokens(f.ToString());

            // Compare the two lists of tokens, ensuring that each one is equal, thus implying that the full equation is equal
            for (int i = 0; i < thisTokens.Count; i++)
            {
                if (thisTokens[i] != otherTokens[i])
                {
                    return false; // If any token is not equal, false is returned as the formulas are not the same
                }
            }

            return true;
        }

        return false;
    }

    /// <summary>
    ///   <para>
    ///     Evaluates this Formula, using the lookup delegate to determine the values of
    ///     variables.
    ///   </para>
    ///   <remarks>
    ///     When the lookup method is called, it will always be passed a Normalized (capitalized)
    ///     variable name.  The lookup method will throw an ArgumentException if there is
    ///     not a definition for that variable token.
    ///   </remarks>
    ///   <para>
    ///     If no undefined variables or divisions by zero are encountered when evaluating
    ///     this Formula, the numeric value of the formula is returned.  Otherwise, a
    ///     FormulaError is returned (with a meaningful explanation as the Reason property).
    ///   </para>
    ///   <para>
    ///     This method should never throw an exception.
    ///   </para>
    /// </summary>
    /// <param name="lookup">
    ///   <para>
    ///     Given a variable symbol as its parameter, lookup returns the variable's (double) value
    ///     (if it has one) or throws an ArgumentException (otherwise).  This method should expect
    ///     variable names to be capitalized.
    ///   </para>
    /// </param>
    /// <returns> Either a double or a formula error, based on evaluating the formula.</returns>
    public object Evaluate(Lookup lookup)
    {
        // Create the stacks to store values and operators in the formula
        Stack<string> valueStack = new Stack<string>();
        Stack<string> operatorStack = new Stack<string>();

        foreach(string token in GetTokens(fullForm))
        {
            // If the token is an integer, these instructions are followed
            if (double.TryParse(token, out double d))
            {
                // If an operator is present and a multiplier or dividing symbol, the proper math is performed, otherwise the value is added to the value stack
                if (operatorStack.Count > 0 && (operatorStack.Peek() == "/" || operatorStack.Peek() == "*"))
                {
                    object newVal = MultiplyOrDivideTokens(d, operatorStack, valueStack);
                    if (newVal is double)
                    {
                        valueStack.Push(((double)newVal).ToString());
                    }
                    else
                    {
                        return newVal;
                    }
                }
                else
                {
                    valueStack.Push(d.ToString());
                }
            }

            // If the token is a variable, these instructions are followed
            else if (GetVariables().Contains(token))
            {
                try
                {
                    object currNum = lookup(token); // Get the value of this variable

                    if (currNum is double)
                    {
                        d = (double)currNum;

                        // The same instructions are done as for an integer if no exception is thrown
                        if (operatorStack.Count > 0 && (operatorStack.Peek() == "/" || operatorStack.Peek() == "*"))
                        {
                            object newVal = MultiplyOrDivideTokens(d, operatorStack, valueStack);
                            if (newVal is double)
                            {
                                valueStack.Push(((double)newVal).ToString());
                            }
                            else
                            {
                                return newVal;
                            }
                        }
                        else
                        {
                            valueStack.Push(d.ToString());
                        }
                    }
                }

                // If no vlaue is found for this variable, a formulaError is returned
                catch (Exception)
                {
                    {
                        return new FormulaError("No Reference to Variable " + token.ToString());
                    }
                }
            }

            // If the token is a multiplier or dividing symbol, the operator is added to its stack
            else if (token == "*" || token == "/")
            {
                operatorStack.Push(token);
            }

            // If the token is a plus or minus symbol, these instructions are followed
            else if (token == "+" || token == "-")
            {
                // If another plus or minus sybol is at the top of the operator stack, that operator is analyzed and implemented,
                // regardless of this, the operator is pushed onto its stack after this check
                if (operatorStack.Count > 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
                {
                    object newToken = AddOrSubtractTokens(operatorStack, valueStack);
                    valueStack.Push(((double)newToken).ToString());
                }

                operatorStack.Push(token);
            }

            // If the token is a closing parenthesis, these isntructions are followed
            else if (token == ")")
            {
                string topOfStack = operatorStack.Peek();

                // If the top of the stack is a plus or minus symbol first, the operator is implemented and the found value pushed onto the its stack
                if (topOfStack == "+" || topOfStack == "-")
                {
                    object newToken = AddOrSubtractTokens(operatorStack, valueStack);
                    if (newToken is double)
                    {
                        valueStack.Push(((double)newToken).ToString());
                    }
                }

                // If the top of the stack is now an opening parenthesis, remove it form the operator stack
                topOfStack = operatorStack.Peek();
                if (topOfStack == "(")
                {
                    operatorStack.Pop();
                }

                // If the top of the operator stack is a multiplier or division symbol, implement the operator and return the result
                // This can result an OperatorError if dividing by zero
                topOfStack = operatorStack.Peek();
                if (topOfStack == "*" || topOfStack == "/")
                {
                    object newVal = MultiplyOrDivideTokens(double.Parse(valueStack.Pop()), operatorStack, valueStack);
                    if (newVal is double)
                    {
                        valueStack.Push(((double)newVal).ToString());
                    }
                    else
                    {
                        return newVal;
                    }
                }
            }
        }

        // If there are still operator in that stack, it will always be a plus or minus symbol,
        // thus implementing this symbol and pushing it to the stack allows for proper implementation
        if (operatorStack.Count > 0)
        {
            double newVal = AddOrSubtractTokens(operatorStack, valueStack);
            valueStack.Push(newVal.ToString());
        }

        return valueStack.Pop();
    }

    /// <summary>
    ///   <para>
    ///     Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
    ///     case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two
    ///     randomly-generated unequal Formulas have the same hash code should be extremely small.
    ///   </para>
    /// </summary>
    /// <returns> The hashcode for the object. </returns>
    public override int GetHashCode()
    {
        return fullForm.GetHashCode();
    }

    /// <summary>
    ///     <para>
    ///         Add or subtract two values from the top of the value stack, reading the value at the top of the operator to determine the operation necessary.
    ///     </para>
    /// </summary>
    /// <param name="opStack"> The operator stack that is currently being utilized. </param>
    /// <param name="valStack"> The value stack that is currently being utilized. </param>
    /// <returns> The value after adding or subtracting the value at the top of the stack. </returns>
    private double AddOrSubtractTokens(Stack<string> opStack, Stack<string> valStack)
    {
        string currOperator = opStack.Pop();

        double newVal = 0;
        if (currOperator == "+")
        {
            double num1 = double.Parse(valStack.Pop());
            double num2 = double.Parse(valStack.Pop());

            newVal = num2 + num1;
        }
        else if (currOperator == "-")
        {
            double num1 = double.Parse(valStack.Pop());
            double num2 = double.Parse(valStack.Pop());

            newVal = num2 - num1; // Num2 is placed first due to the stack being reversed and the second number being first in the order provided
        }

        return newVal;
    }

    /// <summary>
    ///     <para>
    ///         Multiply or divide a value from the value at the top of the value stack, using the operator at the top of the operator stack to determine the operation to implement.
    ///     </para>
    /// </summary>
    /// <param name="currToken"> The token that is to be multiplied or divided from, this being the divisor. </param>
    /// <param name="opStack"> The operator stack that is read from to determine whether to multiply or divide values. </param>
    /// <param name="valStack"> The value stack that is used to multiply or divide from the top of, with it representing the numerator. </param>
    /// <returns> An object representing the value after the operation, or a FormulaError object due to dividing by zero. </returns>
    private object MultiplyOrDivideTokens(double currToken, Stack<string> opStack, Stack<string> valStack)
    {
        string op = opStack.Pop();
        string num = valStack.Pop();

        if (op == "*")
        {
            currToken = currToken * double.Parse(num);
        }

        // If the operator is determined to divide the values, it divides the top of the value stack from the current token, returning a FormulaError if the current token is 0
        else
        {
            if(currToken == 0)
            {
                return new FormulaError("Cannot Divide By Zero");
            }
            else
            {
                currToken = double.Parse(num) / currToken;
            }
        }

        return currToken;
    }
}

/// <summary>
/// Used as a possible return value of the Formula.Evaluate method.
/// </summary>
public class FormulaError
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaError"/> class.
    ///   <para>
    ///     Constructs a FormulaError containing the explanatory reason.
    ///   </para>
    /// </summary>
    /// <param name="message"> Contains a message for why the error occurred.</param>
    public FormulaError(string message)
    {
        Reason = message;
    }

    /// <summary>
    ///  Gets the reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
}

/// <summary>
///   Any method meeting this type signature can be used for
///   looking up the value of a variable.  In general the expected behavior is that
///   the Lookup method will "know" about all variables in a formula
///   and return their appropriate value.
/// </summary>
/// <exception cref="ArgumentException">
///   If a variable name is provided that is not recognized by the implementing method,
///   then the method should throw an ArgumentException.
/// </exception>
/// <param name="variableName">
///   The name of the variable (e.g., "A1") to lookup.
/// </param>
/// <returns> The value of the given variable (if one exists). </returns>
public delegate double Lookup(string variableName);

/// <summary>
///   Used to report syntax errors in the argument to the Formula constructor.
/// </summary>
public class FormulaFormatException : Exception
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaFormatException"/> class.
    ///   <para>
    ///      Constructs a FormulaFormatException containing the explanatory message.
    ///   </para>
    /// </summary>
    /// <param name="message"> A developer defined message describing why the exception occured.</param>
    public FormulaFormatException(string message)
        : base(message)
    {
        // All this does is call the base constructor. No extra code needed.
    }
}