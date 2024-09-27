using CS3500.Formula;
using CS3500.Spreadsheet;

namespace SpreadsheetTests;

/// <summary>
///     <para>
///         A test suite for checking the Spreadsheet class for errors in its functions,
///         provided the individual cases for errors that may occur in their implementation and
///         the way that the class is organized
///     </para>
/// </summary>
[TestClass]
public class SpreadsheetTests
{
    /// <summary>
    ///     <para>
    ///         Check that when setting a cell to hold an integer or decimal value, of any form,
    ///         it is able to properly do so
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Spreadsheet_TestSetCellContents_Integer()
    {
        Spreadsheet testSheet = new Spreadsheet();

        string[] tempArr = { "A1" };
        string[] actualArr = testSheet.SetCellContents("A1", new Formula("B1 + 4")).ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }

        actualArr = testSheet.SetCellContents("A1", -4).ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]); // Check for negative values
        }

        // Check for multiple forms of decimal values being utilized in a cell
        actualArr = testSheet.SetCellContents("A1", 0.0423).ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }


        actualArr = testSheet.SetCellContents("A1", 4.23e-2).ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]); // Check for negative values
        }
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when setting a cell to hold a string, it can properly do so
    ///     </para>
    ///     <remarks>
    ///         When storing an empty string, the cell should be deleted from memory
    ///     </remarks>
    /// </summary>
    [TestMethod]
    public void Spreadsheet_TestSetCellContents_String()
    {
        Spreadsheet testSheet = new Spreadsheet();

        string[] tempArr = { "A1" };
        string[] actualArr = testSheet.SetCellContents("A1", "Test String").ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]); // Check for negative values
        }
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when adding a formula with no dependencies, it can properly be inserted into the spreadsheet with no errors
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Spreadsheet_TestSetCellContents_Formula_NoDependencies()
    {
        Spreadsheet testSheet = new Spreadsheet();

        string[] tempArr = { "A1" };
        string[] actualArr = testSheet.SetCellContents("A1", new Formula("B1 + 4")).ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }
    }


    /// <summary>
    ///     <para>
    ///         Ensure that dependencies can be asserted both directly and indirectly when changing the value within a cell
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Spreadsheet_TestSetCellContents_Formula_Dependencies()
    {
        Spreadsheet testSheet = new Spreadsheet();
        string[] tempArr = { "A1" };
        string[] actualArr = testSheet.SetCellContents("A1", new Formula("B1 + 4")).ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }

        tempArr = new string[] { "B1", "A1" };
        actualArr = testSheet.SetCellContents("B1", new Formula("C1 * 2")).ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }

        tempArr = new string[] { "C1", "B1", "A1" };
        actualArr = testSheet.SetCellContents("C1", 15).ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when having dependencies that rely on one another, an exception is thrown to address
    ///         this error
    ///     </para>
    /// </summary>
    [TestMethod]
    [ExpectedException (typeof(CircularException))]
    public void Spreadsheet_TestSetCellContents_Formula_CircularDependency()
    {
        Spreadsheet testSheet = new Spreadsheet();

        string[] tempArr = { "A1" };
        string[] actualArr = testSheet.SetCellContents("A1", new Formula("B1 + 4")).ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }

        testSheet.SetCellContents("B1", new Formula("A1 * 2"));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when referencing a node that does not exist, an exception is thrown to address this error
    ///     </para>
    /// </summary>
    [TestMethod]
    [ExpectedException (typeof(InvalidNameException))]
    public void Spreadsheet_TestSetCellContents_InvalidName()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetCellContents("17A", "null");
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when setting a node that is a dependee of other nodes, it accurately changes the others
    ///         when the value within it is changed
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Spreadsheet_TestSetCellContents_AdjustingDependencies()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetCellContents("A1", new Formula("B1 + 4"));
        testSheet.SetCellContents("B1", 3);

        // Assert.AreEqual(new Formula("B1 + 4"), testSheet.GetCellContents("A1"));
        Assert.AreEqual(3.0, testSheet.GetCellContents("B1"));

        testSheet.SetCellContents("B1", 14);
        // Assert.AreEqual(new Formula("B1 + 4"), testSheet.GetCellContents("A1"));
        Assert.AreEqual(14.0, testSheet.GetCellContents("B1"));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when getting the non-empty cells within the spreadsheet, 
    ///         the function properly represents an empty sheet
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Spreadsheet_TestGetAllNonEmptyCells_OnlyEmptyCells()
    {
        Spreadsheet testSheet = new Spreadsheet();
        ISet<string> testSet = new HashSet<string>();

        for (int i = 0; i < testSet.Count; i++)
        {
            Assert.AreEqual(testSet.ToList()[i], testSheet.GetNamesOfAllNonemptyCells().ToList()[i]);
        }
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when getting all the non-empty cells within the spreadsheet,
    ///         the function can determine a non-empty cell with an empty dependent cell by only returning the filled cell and
    ///         not the dependent cell
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Spreadsheet_TestGetAllNonEmptyCells_DependencyOnEmptyCell()
    {
        Spreadsheet testSheet = new Spreadsheet();
        testSheet.SetCellContents("A1", new Formula("B1 + 15"));

        List<string> actualList = testSheet.GetNamesOfAllNonemptyCells().ToList();

        ISet<string> testSet = new HashSet<string>();
        testSet.Add("A1");

        for (int i = 0; i < testSet.Count; i++)
        {
            Assert.AreEqual(testSet.ToList()[i], actualList[i]);
        }
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when getting all the non-empty cells within the spreadsheet, 
    ///         the function does not return a cell that has been filled with an empty value after being filled once prior
    ///     </para>
    /// </summary>
    [TestMethod]
    public void Spreadsheet_TestGetAllNonEmptyCells_FilledThanEmptiedCell()
    {
        Spreadsheet testSheet = new Spreadsheet();
        testSheet.SetCellContents("A1", new Formula("B1 + 15"));
        testSheet.SetCellContents("A1", "");

        ISet<string> testSet = new HashSet<string>();
        testSet.Add("A1");

        List<string> actualList = testSheet.GetNamesOfAllNonemptyCells().ToList();

        for (int i = 0; i < actualList.Count; i++)
        {
            Assert.AreEqual(testSet.ToList()[i], actualList[i]);
        }
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when getting a cell's contents, an exception is thrown if the name of the cell is invalid 
    ///         and thus, does not exist
    ///     </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void Spreadsheet_TestGetCellContents_InvalidCell()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.GetCellContents("17A");
    }
}