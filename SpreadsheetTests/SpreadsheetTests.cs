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


    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestSetCellContents_Formula_MultipleDependenciesInFormula()
    {
        Spreadsheet testSheet = new Spreadsheet();
        string[] tempArr = { "A1" };
        string[] actualArr = testSheet.SetCellContents("A1", new Formula("B1 + C1")).ToArray();
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
    ///     Test that when resetting a formula with dependencies to a value of a double, these dependencies are removed from the spreadsheet
    /// </summary>
    [TestMethod]
    public void Spreadsheet_TestSetCellContents_Formula_ResetFormulaToDouble()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetCellContents("A1", new Formula("B1 + 5"));
        Assert.AreEqual(new Formula("B1 + 5"), testSheet.GetCellContents("A1"));

        testSheet.SetCellContents("A1", 15.7);
        Assert.AreEqual(15.7, testSheet.GetCellContents("A1"));

        string[] tempArr = new string[] { "B1" };
        string[] actualArr = testSheet.SetCellContents("B1", 14).ToArray();
        for (int i = 0; i < actualArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }
    }

    /// <summary>
    ///     Test that when resetting a formula with dependencies to a value of a string, these dependencies are removed from the spreadsheet
    /// </summary>
    [TestMethod]
    public void Spreadsheet_TestSetCellContents_Formula_ResetFormulaToString()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetCellContents("A1", new Formula("B1 + 5"));
        Assert.AreEqual(new Formula("B1 + 5"), testSheet.GetCellContents("A1"));

        testSheet.SetCellContents("A1", "Test");
        Assert.AreEqual("Test", testSheet.GetCellContents("A1"));

        string[] tempArr = new string[] { "B1" };
        string[] actualArr = testSheet.SetCellContents("B1", 14).ToArray();
        for (int i = 0; i < actualArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }
    }


    /// <summary>
    ///     Test that when resetting a formula with dependencies to a value of a new formula, these dependencies are removed from the spreadsheet
    /// </summary>
    [TestMethod]
    public void Spreadsheet_TestSetCellContents_Formula_ResetFormulaToFormula()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetCellContents("A1", new Formula("B1 + 5"));
        Assert.AreEqual(new Formula("B1 + 5"), testSheet.GetCellContents("A1"));

        testSheet.SetCellContents("A1", new Formula("C1 + 7"));
        Assert.AreEqual(new Formula("C1 + 7"), testSheet.GetCellContents("A1"));

        string[] tempArr = new string[] { "B1" };
        string[] actualArr = testSheet.SetCellContents("B1", 14).ToArray();
        for (int i = 0; i < actualArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }

        // Check that changing the new dependent cell affects A1 properly
        tempArr = new string[] { "C1", "A1" };
        actualArr = testSheet.SetCellContents("C1", 14).ToArray();
        for (int i = 0; i < actualArr.Length; i++)
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

    [TestMethod]
    [ExpectedException (typeof(InvalidNameException))]
    public void Spreadsheet_TestInvalidName_NonLetterValue()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetCellContents("/15", 5);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void Spreadsheet_TestInvalidName_LetterAfterNumber()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetCellContents("A15A", 5);
    }

    /// <summary>
    ///     <para>
    ///         Ensure that when referencing a node that does not exist, an exception is thrown to address this error while passing in a string
    ///     </para>
    /// </summary>
    [TestMethod]
    [ExpectedException (typeof(InvalidNameException))]
    public void Spreadsheet_TestSetCellContents_InvalidName_String()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetCellContents("17A", "test");
    }

    /// <summary>
    ///     <para>
    ///         Ensure that when referencing a node that does not exist, an exception is thrown to address this error while passing in a number
    ///     </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void Spreadsheet_TestSetCellContents_InvalidName_double()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetCellContents("17A", 15.0);
    }

    /// <summary>
    ///     <para>
    ///         Ensure that when referencing a node that does not exist, an exception is thrown to address this error while passing in a formula
    ///     </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void Spreadsheet_TestSetCellContents_InvalidName_Formula()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetCellContents("17A", new Formula("5 + A1"));
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


    [TestMethod]
    public void Spreadsheet_TestGetCellContents_StringThatIsFormula()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetCellContents("A1", "1 + 2");

        Assert.AreEqual("1 + 2", testSheet.GetCellContents("A1"));
    }


    [TestMethod]
    public void Spreadsheet_TestGetCellContents_StringThatIsNumber()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetCellContents("A1", "1");

        Assert.AreEqual("1", testSheet.GetCellContents("A1"));
    }


    [TestMethod]
    public void Spreadsheet_TestGetCellContents_ValidNameEmptyCell()
    {
        Spreadsheet testSheet = new Spreadsheet();;

        Assert.AreEqual(string.Empty, testSheet.GetCellContents("A1"));
    }


    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestStress_SetCellContents()
    {
        Spreadsheet testSheet = new Spreadsheet();
        const int SIZE = 1000;
        List<string> cellNames = new List<string>();

        for (int i = 0; i <= SIZE; i++)
        {
            cellNames.Add($"A{i}");
        }

        for(int i  = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j++)
            {
                testSheet.SetCellContents(cellNames[i], new Formula($"{cellNames[j]} + {cellNames[j + 1]}"));
            }
        }

        List<string> changedCells = testSheet.SetCellContents(cellNames[SIZE], 15).ToList();
        cellNames.Reverse();
        cellNames.Remove(cellNames[1]);
        for (int i = 0; i < SIZE - 1; i++)
        {
            Assert.AreEqual(cellNames[i], changedCells[i]);
        }
    }
}