// <copyright file="FormulaSyntaxTests.cs" company="UofU-CS3500">
//   Copyright © 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> William Myers </authors>
// <date> 9/10/2024 </date>

namespace CS3500.FormulaTests;

using CS3500.Formula; // Change this using statement to use different formula implementations.

/// <summary>
///   <para>
///     The following class shows the basics of how to use the MSTest framework,
///     including:
///   </para>
///   <list type="number">
///     <item> How to catch exceptions. </item>
///     <item> How a test of valid code should look. </item>
///   </list>
/// </summary>
[TestClass]
public class FormulaSyntaxTests
{
    // --- Tests for One Token Rule ---

    /// <summary>
    ///   <para>
    ///     This test makes sure the right kind of exception is thrown
    ///     when trying to create a formula with no tokens.
    ///   </para>
    ///   <remarks>
    ///     <list type="bullet">
    ///       <item>
    ///         We use the _ (discard) notation because the formula object
    ///         is not used after that point in the method.  Note: you can also
    ///         use _ when a method must match an interface but does not use
    ///         some of the required arguments to that method.
    ///       </item>
    ///       <item>
    ///         string.Empty is often considered best practice (rather than using "") because it
    ///         is explicit in intent (e.g., perhaps the coder forgot to but something in "").
    ///       </item>
    ///       <item>
    ///         The name of a test method should follow the MS standard:
    ///         https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
    ///       </item>
    ///       <item>
    ///         All methods should be documented, but perhaps not to the same extent
    ///         as this one.  The remarks here are for your educational
    ///         purposes (i.e., a developer would assume another developer would know these
    ///         items) and would be superfluous in your code.
    ///       </item>
    ///       <item>
    ///         Notice the use of the attribute tag [ExpectedException] which tells the test
    ///         that the code should throw an exception, and if it doesn't an error has occurred;
    ///         i.e., the correct implementation of the constructor should result
    ///         in this exception being thrown based on the given poorly formed formula.
    ///       </item>
    ///     </list>
    ///   </remarks>
    ///   <example>
    ///     <code>
    ///        // here is how we call the formula constructor with a string representing the formula
    ///        _ = new Formula( "5+5" );
    ///     </code>
    ///   </example>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestNoTokens_Invalid()
    {
        _ = new Formula("");  // note: it is arguable that you should replace "" with string.Empty for readability and clarity of intent (e.g., not a cut and paste error or a "I forgot to put something there" error).
    }



    // --- Tests for Valid Token Rule ---

    /// <summary>
    ///     <para>
    ///         This test ensures that with a combination of tokens that are valid, 
    ///         it will function properly.
    ///     </para>
    ///     <code>
    ///         Code Within Test:
    ///         _ = new Formula("7 + (x - s) / (2 * 4)")
    ///     </code>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestTokens_Valid()
    {
        _ = new Formula("7 + (x1 - s1) / (2 * 4)");
    }

    /// <summary>
    ///     <para>
    ///         This test ensures that, given a series of invalid tokens, 
    ///         the program does not function as intended.
    ///     </para>
    ///     <code>
    ///         Code Within Test:
    ///         _ = new Formula("? = }")
    ///     </code>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestTokens_Invalid()
    {
        _ = new Formula("? = }");
    }



    // --- Tests for Closing Parenthesis Rule

    /// <summary>
    ///     <para>
    ///         This test ensures that a value within parenthesis balances both opening and closing parenthesis counts,
    ///         allowing for proper anaylisis of any invalid or valid formulas.
    ///     </para>
    ///     <code>
    ///         Code Within Test:
    ///         _ = new Formula("((1)))")
    ///     </code>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosingParenthesis_Invalid()
    {
        _ = new Formula("((1)))"); // The one in this test is to ensure the test does not break the first token rule
    }



    // --- Tests for Balanced Parentheses Rule

    /// <summary>
    ///     <para>
    ///         This test ensures that any open parenthesis is paired with a closing parenthesis,
    ///         allowing for proper implimentation.
    ///     </para>
    ///     <code>
    ///         Code Within Test:
    ///         _ = new Formula("(")
    ///     </code>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestBalancedParenthesis_Invalid()
    {
        _ = new Formula("(");
    }

    /// <summary>
    ///     <para>
    ///         This test ensures that when provided proper closing parenthesis, 
    ///         the intended functionality is found to be true.
    ///     </para>
    ///     <code>
    ///         Code Within test:
    ///         _ = new Formula("(5)")
    ///     </code>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestBalancedParenthesis_Valid()
    {
        _ = new Formula("(5)");
    }



    // --- Tests for First Token Rule

    /// <summary>
    ///     <para>
    ///         This test ensures that any token that comes first does not require previous tokens.
    ///     </para>
    ///     <code>
    ///         Code Within Test:
    ///         _ = new Formula(")5")
    ///         _ = new Formula("+")
    ///     </code>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestFirstToken_Invalid()
    {
        _ = new Formula(")5");
        _ = new Formula("+");
    }

    /// <summary>
    ///     <para>
    ///         This test is for extra conditions to where the formula shoudl be valid, compared to the test below.
    ///     </para>
    ///     <code>
    ///         Code Within Test
    ///         _ = new Formula("(5)")
    ///         _ = new Formula("x1")
    ///         _ = new Formula("5")
    ///     </code>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstToken_Valid()
    {
        _ = new Formula("(5)");
        _ = new Formula("x1");
        _ = new Formula("5");
    }

    /// <summary>
    ///   <para>
    ///     Make sure a simple well formed formula is accepted by the constructor (the constructor
    ///     should not throw an exception).
    ///   </para>
    ///   <remarks>
    ///     This is an example of a test that is not expected to throw an exception, i.e., it succeeds.
    ///     In other words, the formula "1+1" is a valid formula which should not cause any errors.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenNumber_Valid()
    {
        _ = new Formula("1+1");
    }

    // --- Tests for  Last Token Rule ---

    /// <summary>
    ///     <para>
    ///         This test ensures that the last token of the formula is valid, not being an open parnethesis or operator.
    ///     </para>
    ///     <code>
    ///         Code Within Test:
    ///         _ = new Formula("5(");
    ///         _ = new Formula("2 +");
    ///     </code>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestLastToken_Invalid()
    {
        _ = new Formula("5(");
        _ = new Formula("2 +");
    }

    /// <summary>
    ///     <para>
    ///         This test ensures that each case that last token is allowed to be is able to properly function within the formula created.
    ///     </para>
    ///     <code>
    ///         Code Within Test:
    ///         _ = new Formula("(5)");
    ///         _ = new Formula("3");
    ///         _ = new Formula("x1");
    ///     </code>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLastToken_Valid()
    {
        _ = new Formula("(5)");
        _ = new Formula("3");
        _ = new Formula("x1");
    }



    // --- Tests for Parentheses/Operator Following Rule ---

    /// <summary>
    ///     <para>
    ///         This test ensures that any token following a parenthesis or operator is a number or vairable, 
    ///         allowing for proper reading of the formula.
    ///     </para>
    ///     <code>
    ///         Code Within Test:
    ///         _ = new Formula("()");
    ///         _ = new Formula("(1 + )");
    ///     </code>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestFollowing_Invalid()
    {
        _ = new Formula("()");
        _ = new Formula("(1 + )");
    }

    /// <summary>
    ///     <para>
    ///         This test ensures that any possible valid token following an operator or closing parenthesis functions properly within a formula.
    ///     </para>
    ///     <code>
    ///         Code Within Test:
    ///         _ = new Formula("1 + 2");
    ///         _ = new Formula("1 + x");
    ///         _ = new Formula("((z))");
    ///         _ = new Formula("(1 + (2))");
    ///     </code>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFollowing_Valid()
    {
        _ = new Formula("1 + 2");
        _ = new Formula("1 + x1");
        _ = new Formula("((z1))");
        _ = new Formula("(1 + (2))");
    }



    // --- Tests for Extra Following Rule ---

    /// <summary>
    ///     <para>
    ///         This test ensures that in no situation a number or variable follows another number or variable, along with a closing parenthesis.
    ///     </para>
    ///     <code>
    ///         Code Within Test:
    ///         _ = new Formula("(5) 7");
    ///         _ = new Formula("x 7");
    ///         _ = new Formula("7 x");
    ///     </code>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraFollowing_Invalid()
    {
        _ = new Formula("(5) 7");
        _ = new Formula("x 7");
        _ = new Formula("7 x");
    }

    /// <summary>
    ///     <para>
    ///         This test ensure sthat any token that should be valid within the formula is able to function properly when passed in.
    ///     </para>
    ///     <code>
    ///         Code Within Test:
    ///         _ = new Formula("(5) + 7");
    ///         _ = new Formula("x * 7");
    ///         _ = new Formula("7 / x");
    ///     </code>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtraFollowing_Valid()
    {
        _ = new Formula("(5) + 7");
        _ = new Formula("x1 * 7");
        _ = new Formula("7 / x1");
    }


    /// <summary>
    ///     <para>
    ///         This test esnures that when provided a valid formula, the ToString function works appropriately
    ///     </para>
    ///     <code>
    ///         Code Within Test:
    ///          Assert.IsTrue(new Formula("x1 + z1").ToString() == "X1+Z1");
    ///         Assert.IsTrue(new Formula("x1  +     z1").ToString() == "X1+Z1");
    ///     </code>
    /// </summary>
    [TestMethod]
    public void Formula_TestToString_Valid()
    {
        Assert.IsTrue(new Formula("x1 + z1").ToString() == "X1+Z1");
        Assert.IsTrue(new Formula("x1  +     z1").ToString() == "X1+Z1"); // This test ensures that extra spaces are not represented in the ToString Function
    }


    // FURTHER TESTS MADE FOR ASSIGNMENT 4


    /// <summary>
    ///     <para>
    ///         Ensure that when given different integers within a formula, the == syntax will return false, 
    ///         or true if they are the same integers
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestDoubleEquals_Integers()
    {
        Assert.IsTrue(new Formula("1 + 2") == new Formula("1 + 2"));
        Assert.IsFalse(new Formula("1 + 2") == new Formula("1 + 5")); // Check for in the case the two formulas are not equal
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when given doubles or floats, the == syntax can still find proper equality between formulas
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestDoubleEquals_FloatingPoint()
    {
        Assert.IsTrue(new Formula("1 + 0.5") == new Formula("1 + 0.5"));
        Assert.IsFalse(new Formula("1 + 0.5") == new Formula("0.5 + 2")); // Check in the case that the two formulas are slightly different
        Assert.IsTrue(new Formula("1 + 5E-1") == new Formula("1 + 5E-1"));
        Assert.IsTrue(new Formula("1 + 0.5") == new Formula("1 + 5E-1")); // Check to ensure that a combination of two forms is read as an equality, rather than inequality
        Assert.IsFalse(new Formula("1 + 5E-1") == new Formula("1 + 1.5E-1"));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when given parenthesis, a formula can properly be found to be equivalent using the == syntax
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestMethodDoubleEquals_Parenthesis()
    {
        Assert.IsTrue(new Formula("(x1 + 5)") == new Formula("(x1 + 5)"));
        Assert.IsFalse(new Formula("(x1 + 5)") == new Formula("x1 + 5")); // Check in the case that the parenthesis are not present in only 1 formula
        Assert.IsTrue(new Formula("(5)") == new Formula("(5)")); // Check for when only a signle value is present in the parenthesis
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when given different variables, the == syntax will return false, or true if they are the same
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestDoubleEquals_Variables()
    {
        Assert.IsTrue(new Formula("x1 + x2") == new Formula("x1 + x2"));
        Assert.IsFalse(new Formula("x1 + x2") == new Formula("y1 + y2")); // Check for in the case the two formulas are not equal
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when given different signs between formulas, the == syntax will return false if different and true when the same
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestDoubleEquals_Signs()
    {
        Assert.IsTrue(new Formula("x1 - x2") == new Formula("x1 - x2"));
        Assert.IsTrue(new Formula("x1 * x2") == new Formula("x1 * x2"));
        Assert.IsTrue(new Formula("x1 / x2") == new Formula("x1 / x2"));

        Assert.IsFalse(new Formula("x1 / x2") == new Formula("x1 * x2")); // Check that a combination of signs returns false
    }


    /// <summary>
    ///     <para>
    ///         Ensure that, using the == syntax, two series of formulas that are different combinations of 
    ///         one another will not be found to be equal unless they are exactly the same
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestDoubleEquals_Combination()
    {
        Assert.IsTrue(new Formula("1 + x2") == new Formula("1 + x2"));
        Assert.IsFalse(new Formula("1 + x2") == new Formula("x1 + 2")); // Check for when the variables are not the same between the two formulas
        Assert.IsFalse(new Formula("x1 - x2") == new Formula("x1 + x2")); // Check that the sign does not impact the result
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when two formulas, using the Equals function, have different integers involved, 
    ///         will be found to be different formulas
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEquals_Integers()
    {
        Assert.IsTrue(new Formula("1 + 2").Equals(new Formula("1 + 2")));
        Assert.IsFalse(new Formula("1 + 2").Equals(new Formula("1 + 5"))); // Check for in the case the two formulas are not equal
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when given doubles or floats, the Equals function can still find proper equality between formulas
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEquals_FloatingPoint()
    {
        Assert.IsTrue(new Formula("1 + 0.5").Equals(new Formula("1 + 0.5")));
        Assert.IsFalse(new Formula("1 + 0.5").Equals(new Formula("0.5 + 2"))); // Check in the case that the two formulas are slightly different
        Assert.IsTrue(new Formula("1 + 5E-1").Equals(new Formula("1 + 5E-1")));
        Assert.IsTrue(new Formula("1 + 0.5").Equals(new Formula("1 + 5E-1"))); // Check to ensure that a combination of two forms is read as an equality, rather than inequality
        Assert.IsFalse(new Formula("1 + 5E-1").Equals(new Formula("1 + 1.5E-1")));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when given parenthesis, a formula can properly be found to be equivalent using the Equals function
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestMethodEquals_Parenthesis()
    {
        Assert.IsTrue(new Formula("(x1 + 5)").Equals(new Formula("(x1 + 5)")));
        Assert.IsFalse(new Formula("(x1 + 5)").Equals(new Formula("x1 + 5"))); // Check in the case that the parenthesis are not present in only 1 formula
        Assert.IsTrue(new Formula("(5)").Equals(new Formula("(5)"))); // Check for when only a signle value is present in the parenthesis
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when using the Equals function, it can find that two values with 
    ///         the same and different variables are equal or not, respectively
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEquals_Variables()
    {
        Assert.IsTrue(new Formula("x1 + x2").Equals(new Formula("x1 + x2")));
        Assert.IsFalse(new Formula("x1 + x2").Equals(new Formula("y1 + y2"))); // Check for in the case the two formulas are not equal
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when using the Equals function, it can find that two formulas with the same signs, 
    ///         with differences between each sign test, are equivalent to one another
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEquals_Signs()
    {
        Assert.IsTrue(new Formula("x1 - x2").Equals(new Formula("x1 - x2")));
        Assert.IsTrue(new Formula("x1 * x2").Equals(new Formula("x1 * x2")));
        Assert.IsTrue(new Formula("x1 / x2").Equals(new Formula("x1 / x2")));

        Assert.IsFalse(new Formula("x1 / x2").Equals(new Formula("x1 * x2"))); // Check that a combination of signs returns false
    }


    /// <summary>
    ///     <para>
    ///         Ensure that given the Equals function, evaluation finds combinations of values to not be equal,
    ///         with two formulas that are the same being equivalent
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEquals_Combination()
    {
        Assert.IsTrue(new Formula("1 + x2").Equals(new Formula("1 + x2")));
        Assert.IsFalse(new Formula("1 + x2").Equals(new Formula("x1 + 2"))); // Check for when the variables are not the same between the two formulas
        Assert.IsFalse(new Formula("x1 - x2").Equals(new Formula("x1 + x2"))); // Check that the sign does not impact the result
    }


    /// <summary>
    ///     <para>
    ///         Test for when the Equals method is used to equate a formula with something that is not of that type
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEquals_NotValid()
    {
        Assert.IsFalse(new Formula("1 + x2").Equals(42));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when two formulas with different integers are compared, they are found to not be equal
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestNotEquals_Integers()
    {
        Assert.IsFalse(new Formula("1 + 2") != new Formula("1 + 2"));
        Assert.IsTrue(new Formula("1 + 2") != new Formula("1 + 7")); // Ensure the correct result is sent when a false equality is found
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when given doubles or floats, the != syntax can still find proper inequality between formulas
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestNotEquals_FloatingPoint()
    {
        Assert.IsFalse(new Formula("1 + 0.5") != new Formula("1 + 0.5"));
        Assert.IsTrue(new Formula("1 + 0.5") != new Formula("0.5 + 2")); // Check in the case that the two formulas are slightly different
        Assert.IsFalse(new Formula("1 + 5E-1") != new Formula("1 + 5E-1"));
        Assert.IsFalse(new Formula("1 + 0.5") != new Formula("1 + 5E-1")); // Check to ensure that a combination of two forms is read as an equality, rather than inequality
        Assert.IsTrue(new Formula("1 + 5E-1") != new Formula("1 + 1.5E-1"));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when given parenthesis, a formula can properly be found to be not equivalent using the != syntax
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestMethodNotEquals_Parenthesis()
    {
        Assert.IsFalse(new Formula("(x1 + 5)") != new Formula("(x1 + 5)"));
        Assert.IsTrue(new Formula("(x1 + 5)") != new Formula("x1 + 5")); // Check in the case that the parenthesis are not present in only 1 formula
        Assert.IsFalse(new Formula("(5)") != new Formula("(5)")); // Check for when only a signle value is present in the parenthesis
    }


    /// <summary>
    ///     <para>
    ///         Ensure that two formulas with different variables will not be read as equal to one another
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestNotEquals_Variables()
    {
        Assert.IsFalse(new Formula("x1 + x2") != new Formula("x1 + x2"));
        Assert.IsTrue(new Formula("x1 + x2") != new Formula("x1 + x7")); // Ensure the correct result is sent when a false equality is found
    }


    /// <summary>
    ///     <para>
    ///         Ensure that the signs being different determines that the formulas are different from one another
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestNotEquals_Signs()
    {
        Assert.IsTrue(new Formula("1 + 2") != new Formula("1 - 2"));
        Assert.IsTrue(new Formula("1 + 2") != new Formula("1 * 2"));
        Assert.IsTrue(new Formula("1 + 2") != new Formula("1 / 2"));

        Assert.IsFalse(new Formula("1 * 2") != new Formula("1 * 2")); // Ensure that when an equality is found, false is returned for sign changes
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when given a combination of signs, variables, and integers in a series of formulas, 
    ///         each one is properly found to not equal one another
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestNotEquals_Combination()
    {
        Assert.IsTrue(new Formula("x1 + 2") != new Formula("x1 - 2"));
        Assert.IsTrue(new Formula("1 + x2") != new Formula("x1 + 2"));
        Assert.IsTrue(new Formula("1 + 2") != new Formula("1 * x2"));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that given a valid formula, a value is returned properly from evaluation
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEvaluate_Base()
    {
        Formula testFormula = new Formula("x1 + 16");
        Assert.IsTrue("21".Equals(testFormula.Evaluate(x1 => 5)));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when provided zero variables, the formula can still properly being evaluated
    ///     </para>
    ///     <note>
    ///         A lambda expression is still used when calling the evaluation function, 
    ///         due to the requirements of said function
    ///     </note>
    /// </summary>
    [TestMethod]
    public void Formula_TestEvaluate_NoVariables()
    {
        Formula testFormula = new Formula("1 + 14");
        Assert.IsTrue("15".Equals(testFormula.Evaluate(x1 => 5)));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that an error is returned when dividing by a variable representing 0
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEvaluate_VariableIsZero()
    {
        Formula testFormula = new Formula("1 / x1");
        Assert.IsTrue(testFormula.Evaluate(x1 => 0) is Formula.FormulaError);
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when a variable is undefined, an error is returned to the user
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEvaluate_NoVariableDefinition()
    {
        Formula testFormula = new Formula("x2 + 16");
        double myVars(string s)
        {
            if (s == "X1")
                return 5;
            else
                throw new ArgumentException("I don't know that variable");
        };

        Assert.IsTrue(testFormula.Evaluate(myVars) is Formula.FormulaError);

    }


    /// <summary>
    ///     <para>
    ///         Ensure that given multiple vairables, the formula can still be evaluated properly
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEvaluate_MultipleVariables()
    {
        Formula testFormula = new Formula("x1 + x2");
        Assert.IsTrue("10".Equals(testFormula.Evaluate((name) => 5)));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when given a complicated formula, it can properly find it, even with a variable provided
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEvaluate_ExtraSigns()
    {
        Formula testFormula = new Formula("x1 + (6 / 2) * (17 - 8)");
        Assert.IsTrue("32".Equals(testFormula.Evaluate(x1 => 5)));
    }


    /// <summary>
    ///     <para>
    ///         Check that variables can properly be divided and multiplied with, getting the correct value once complete
    ///     </para>
    /// </summary>
    /// <exception cref="ArgumentException"> Throw an exception if a variable appears that is undefined </exception>
    [TestMethod]
    public void Formula_TestEvaluate_DividingVariables()
    {
        Formula testFormula = new Formula("x1 / x2 * x3");
        double testLookup(string s)
        {
            if (s == "X1")
                return 10;
            else if (s == "X2")
                return 2;
            else if (s == "X3")
                return 5;
            else
                throw new ArgumentException("I don't know that variable");
        }

        Assert.IsTrue("25".Equals(testFormula.Evaluate(testLookup)));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when multiplying or dividing a variable, if you cannot find its value, a FormulaError is returned as a result
    ///     </para>
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    [TestMethod]
    public void Formula_TestEvaluate_VariablesNotFound_AfterMultiplyingOrDividing()
    {
        Formula testFormula = new Formula("x1 * x3");
        double testLookup(string s)
        {
            if (s == "X1")
                return 10;
            else if (s == "X2")
                return 2;
            else
                throw new ArgumentException("I don't know that variable");
        }

        Assert.IsTrue(testFormula.Evaluate(testLookup) is Formula.FormulaError);

        testFormula = new Formula("x1 / x3");
        Assert.IsTrue(testFormula.Evaluate(testLookup) is Formula.FormulaError);
    }

    /// <summary>
    ///     <para>
    ///         Ensure that when dividing by zero during evaluation, an error is given back to the user
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEvaluate_DivideByZero()
    {
        Formula.FormulaError testError = new Formula.FormulaError("Cannot Divide By Zero");
        Formula testFormula = new Formula("x1 / 0");
        Assert.IsTrue(testFormula.Evaluate(x1 => 5) is Formula.FormulaError);
    }


    /// <summary>
    ///     <para>
    ///         Ensure that given a decimal floating point value, a proper evaluation fo the given formula is possible 
    ///         both when this value is an integer and a variable
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEvaluate_FloatingPointsDecimal()
    {
        Formula testFormula = new Formula("0.67 + x1");
        Assert.IsTrue("5.67".Equals(testFormula.Evaluate(x1 => 5)));
        Assert.IsTrue("1".Equals(testFormula.Evaluate(x1 => 0.33)));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that given a, shortened by notation form, floating point value, a proper evaluation fo the given formula is possible 
    ///         both when this value is an integer and a variable
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEvaluate_FloatingPointsEForm()
    {
        Formula testFormula = new Formula("6.7E-2 + x1");
        Assert.IsTrue("5.067".Equals(testFormula.Evaluate(x1 => 5)));
        Assert.IsTrue("0.1".Equals(testFormula.Evaluate(x1 => 3.3E-2)));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that all rules following a closing parenthesis being the token are properly followed in the order specified
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEvaluate_ClosingParenthesisRules()
    {
        Formula testFormula = new Formula("12 / (15 - 9)"); // Test for subtraction being followed specifically
        Assert.IsTrue("2".Equals(testFormula.Evaluate(x1 => 5)));

        testFormula = new Formula("12 / (15 + 9)"); // Test for addition being followed specifically
        Assert.IsTrue("0.5".Equals(testFormula.Evaluate(x1 => 5)));

        testFormula = new Formula("12 / (15 - 15)"); // Test for when dividing by zero after closing parenthesis
        Assert.IsTrue(testFormula.Evaluate(x1 => 5) is Formula.FormulaError);
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when adding or subtracting multiple times in a row, the value are added prior to addign the operator to its stack
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestEvaluate_DoubleAdditionAndSubtraction()
    {
        Formula testFormula = new Formula("1 + 5 + 17");
        Assert.IsTrue("23".Equals(testFormula.Evaluate(x1 => 5)));

        testFormula = new Formula("17 - 5 - 1");
        Assert.IsTrue("11".Equals(testFormula.Evaluate(x1 => 5)));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when given two formula that are the same, they return the same hashcode
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestGetHashCode_Valid()
    {
        Formula f1 = new Formula("x1 + 7 - x2");
        Formula f2 = new Formula("x1 + 7 - x2");

        Assert.AreEqual(f1.GetHashCode(), f2.GetHashCode());
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when given two formula with similar aspects, but differently ordered, it returns a seperate hashcode
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestGetHashCode_Invalid_MulitplicationAndDivision()
    {
        Formula f1 = new Formula("x1 * 7 / x2");
        Formula f2 = new Formula("x1 / 7 * x2");

        Assert.AreNotEqual(f1.GetHashCode(), f2.GetHashCode());
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when given two formula with similar aspects, but differently ordered, it returns a seperate hashcode
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestGetHashCode_Invalid_AdditionAndSubtraction()
    {
        Formula f1 = new Formula("x1 + 7 - x2");
        Formula f2 = new Formula("x1 - x2 + 7");

        int f1Hash = f1.GetHashCode();
        int f2Hash = f2.GetHashCode();

        Assert.AreNotEqual(f1Hash, f2Hash);
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when given two variables that represent potentially different values, two seperate hash codes are different
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Formula_TestGetHashCode_DifferentVariables()
    {
        Formula f1 = new Formula("x1 + 7");
        Formula f2 = new Formula("x2 + 7");

        int f1Hash = f1.GetHashCode();
        int f2Hash = f2.GetHashCode();

        Assert.AreNotEqual(f1Hash, f2Hash);
    }
}