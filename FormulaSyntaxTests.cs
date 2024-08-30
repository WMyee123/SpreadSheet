// <copyright file="FormulaSyntaxTests.cs" company="UofU-CS3500">
//   Copyright © 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> [Insert Your Name] </authors>
// <date> [Insert the Date] </date>

namespace CS3500.FormulaTests;

using CS3500.Formula2; // Change this using statement to use different formula implementations.

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
    [ExpectedException( typeof(FormulaFormatException))]
    public void FormulaConstructor_TestNoTokens_Invalid( )
    {
        _ = new Formula( "" );  // note: it is arguable that you should replace "" with string.Empty for readability and clarity of intent (e.g., not a cut and paste error or a "I forgot to put something there" error).
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
        _ = new Formula("7 + (x - s) / (2 * 4)");
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
    [ExpectedException ( typeof(FormulaFormatException))]
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
    [ExpectedException ( typeof(FormulaFormatException))]
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
    [ExpectedException ( typeof(FormulaFormatException))]
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
    [ExpectedException ( typeof(FormulaFormatException))]
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
    ///         _ = new Formula("x")
    ///         _ = new Formula("5")
    ///     </code>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstToken_Valid()
    {
        _ = new Formula("(5)");
        _ = new Formula("x");
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
    public void FormulaConstructor_TestFirstTokenNumber_Valid( )
    {
        _ = new Formula( "1+1" );
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
    [ExpectedException ( typeof(FormulaFormatException))]
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
    ///         _ = new Formula("x");
    ///     </code>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLastToken_Valid()
    {
        _ = new Formula("(5)");
        _ = new Formula("3");
        _ = new Formula("x");
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
    [ExpectedException ( typeof(FormulaFormatException))]
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
        _ = new Formula("1 + x");
        _ = new Formula("((z))");
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
    [ExpectedException ( typeof(FormulaFormatException))]
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
        _ = new Formula("x * 7");
        _ = new Formula("7 / x");
    }
}