// <copyright file="Formula_PS2.cs" company="UofU-CS3500">
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
        string prevToken = null; // The previous token, stored for ensuring the formula is valid
        int parenthesisCount = 0; // A count of the number of open/closing parenthesis, ensuring balance between the two tokens

        // Store and normalize the formula, allowing for proper representation of the tokens and values present within
        fullForm = string.Empty;
        foreach(string currToken in tokens)
        {
            if(float.TryParse(currToken, out float r))
            {
                fullForm += float.Parse(currToken);
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
            if (prevToken != null & !TokenRulesValid(normToken, prevToken))
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

        if((prevToken == "+" || prevToken == "-" || prevToken == "*" || prevToken == "/"))
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
    /// 
    /// </summary>
    /// <param name="token">
    ///     This is the token being inspected to determine the validity of it following the previous token in the formula.
    /// </param>
    /// <param name="prevToken">
    ///     The token that determines the validity of the current token, allowing for proper understanding of the formula's requirements.
    /// </param>
    /// <returns></returns>
    private bool TokenRulesValid(string token, string prevToken)
    {
        // Check the conditions of when the previous token was a variable
        if (GetVariables().Contains(prevToken))
        {
            // Return true if an operator follows a variable
            if(token == "+" || token == "-" || token == "/" || token == "*")
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
        else if(prevToken == "+" || prevToken == "-" || prevToken == "/"|| prevToken == "*")
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
        else if(prevToken == "(")
        {
            // Return true if a variable or integer follows an opening parenthesis
            if (GetVariables().Contains(token) || float.TryParse(token, out float i))
            {
                return true;
            }
            // Return true if a second opening parenthesis follows one
            else if(token == "(")
            {
                return true;
            }
        }
        // Check the conditions for if the previous token was a closing parenthesis
        else if(prevToken == ")")
        {
            // Return true if an operator follows a closing parenthesis
            if (token == "+" || token == "-" || token == "/" || token == "*")
            {
                return true;
            }
            // Return true if a second closing parenthesis follows one
            else if(token == ")")
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
            else if(token == ")")
            {
                return true;
            }
        }

        return false;
    }
}


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
    public FormulaFormatException( string message )
        : base( message )
    {
        // All this does is call the base constructor. No extra code needed.
    }
}
