// <copyright file="SpreadsheetTests.cs" company="UofU-CS3500">
//   Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> William Myers </authors>
// <date> 9/20/2024 </date>

namespace SpreadsheetTests;

using CS3500.Formula;
using CS3500.Spreadsheet;

/// <summary>
///     A test suite for checking the Spreadsheet class for errors in its functions,
///     provided the individual cases for errors that may occur in their implementation and
///     the way that the class is organized
/// </summary>
[TestClass]
public class SpreadsheetTests
{
    /// <summary>
    ///     Check that when setting a cell to hold an integer or decimal value, of any form,
    ///     it is able to properly do so
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestSetCellContents_Integer()
    {
        Spreadsheet testSheet = new Spreadsheet();

        string[] tempArr = { "A1" };
        string[] actualArr = testSheet.SetContentsOfCell("A1", "=B1 + 4").ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }

        actualArr = testSheet.SetContentsOfCell("A1", "-4").ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]); // Check for negative values
        }

        // Check for multiple forms of decimal values being utilized in a cell
        actualArr = testSheet.SetContentsOfCell("A1", "0.0423").ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }


        actualArr = testSheet.SetContentsOfCell("A1", "4.23e-2").ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]); // Check for negative values
        }
    }


    /// <summary>
    ///     Ensure that when setting a cell to hold a string, it can properly do so
    ///     <remarks>
    ///         When storing an empty string, the cell should be deleted from memory
    ///     </remarks>
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestSetCellContents_String()
    {
        Spreadsheet testSheet = new Spreadsheet();

        string[] tempArr = { "A1" };
        string[] actualArr = testSheet.SetContentsOfCell("A1", "Test String").ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]); // Check for negative values
        }
    }


    /// <summary>
    ///     Ensure that when adding a formula with no dependencies, it can properly be inserted into the spreadsheet with no errors
    /// </summary>
    [TestMethod]
    public void Spreadsheet_TestSetCellContents_Formula_NoDependencies()
    {
        Spreadsheet testSheet = new Spreadsheet();

        string[] tempArr = { "A1" };
        string[] actualArr = testSheet.SetContentsOfCell("A1", "=B1 + 4").ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }
    }


    /// <summary>
    ///     Ensure that dependencies can be asserted both directly and indirectly when changing the value within a cell
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestSetCellContents_Formula_Dependencies()
    {
        Spreadsheet testSheet = new Spreadsheet();
        string[] tempArr = { "A1" };
        string[] actualArr = testSheet.SetContentsOfCell("A1", "=B1 + 4").ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }

        tempArr = new string[] { "B1", "A1" };
        actualArr = testSheet.SetContentsOfCell("B1", "=C1 * 2").ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }

        tempArr = new string[] { "C1", "B1", "A1" };
        actualArr = testSheet.SetContentsOfCell("C1", "15").ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }
    }

    /// <summary>
    ///     Ensure that when a formula causes multiple dependencies to exist, 
    ///     the values properly adjust the other cells without adjusting additional present cells
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestSetCellContents_Formula_MultipleDependenciesInFormula()
    {
        Spreadsheet testSheet = new Spreadsheet();
        string[] tempArr = { "A1" };
        string[] actualArr = testSheet.SetContentsOfCell("A1", "=B1 + C1").ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }

        tempArr = new string[] { "B1", "A1" };
        actualArr = testSheet.SetContentsOfCell("B1", "=2").ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }

        tempArr = new string[] { "C1", "A1" };
        actualArr = testSheet.SetContentsOfCell("C1", "15").ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }
    }

    /// <summary>
    ///     Test that when resetting a formula with dependencies to a value of a double, these dependencies are removed from the spreadsheet
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestSetCellContents_Formula_ResetFormulaToDouble()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("A1", "=B1 + 5");
        Assert.AreEqual(new Formula("B1 + 5"), testSheet.GetCellContents("A1"));

        testSheet.SetContentsOfCell("A1", "15.7");
        Assert.AreEqual(15.7, testSheet.GetCellContents("A1"));

        string[] tempArr = new string[] { "B1" };
        string[] actualArr = testSheet.SetContentsOfCell("B1", "14").ToArray();
        for (int i = 0; i < actualArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }
    }

    /// <summary>
    ///     Test that when resetting a formula with dependencies to a value of a string, these dependencies are removed from the spreadsheet
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestSetCellContents_Formula_ResetFormulaToString()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("A1", "=B1 + 5");
        Assert.AreEqual(new Formula("B1 + 5"), testSheet.GetCellContents("A1"));

        // Reset A1 to contain a string
        testSheet.SetContentsOfCell("A1", "Test");
        Assert.AreEqual("Test", testSheet.GetCellContents("A1"));

        // Check that A1 is no longer dependent on B1
        string[] tempArr = new string[] { "B1" };
        string[] actualArr = testSheet.SetContentsOfCell("B1", "14").ToArray();
        for (int i = 0; i < actualArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }
    }


    /// <summary>
    ///     Test that when resetting a formula with dependencies to a value of a new formula, these dependencies are removed from the spreadsheet
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestSetCellContents_Formula_ResetFormulaToFormula()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("A1", "=B1 + 5");
        Assert.AreEqual(new Formula("B1 + 5"), testSheet.GetCellContents("A1"));

        // Reset the contents of A1 to not reference B1
        testSheet.SetContentsOfCell("A1", "=C1 + 7");
        Assert.AreEqual(new Formula("C1 + 7"), testSheet.GetCellContents("A1"));

        // Check that B1 is not a dependent of A1
        string[] tempArr = new string[] { "B1" };
        string[] actualArr = testSheet.SetContentsOfCell("B1", "14").ToArray();
        for (int i = 0; i < actualArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }

        // Check that changing the new dependent cell affects A1 properly
        tempArr = new string[] { "C1", "A1" };
        actualArr = testSheet.SetContentsOfCell("C1", "14").ToArray();
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
    [Timeout(2000)]
    [ExpectedException (typeof(CircularException))]
    public void Spreadsheet_TestSetCellContents_Formula_CircularDependency()
    {
        Spreadsheet testSheet = new Spreadsheet();

        string[] tempArr = { "A1" };
        string[] actualArr = testSheet.SetContentsOfCell("A1", "=B1 + 4").ToArray();
        for (int i = 0; i < tempArr.Length; i++)
        {
            Assert.AreEqual(tempArr[i], actualArr[i]);
        }

        testSheet.SetContentsOfCell("B1", "=A1 * 2");
    }

    /// <summary>
    ///     Ensure that when using invalid characters, an exception is thrown
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [ExpectedException (typeof(InvalidNameException))]
    public void Spreadsheet_TestInvalidName_NonLetterValue()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("/15", "5");
    }

    /// <summary>
    ///     Ensure that when giving an invalid name with numbers intertwined by letters, 
    ///     an exception is thrown
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [ExpectedException(typeof(InvalidNameException))]
    public void Spreadsheet_TestInvalidName_LetterAfterNumber()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("A15A", "5");
    }

    /// <summary>
    ///     Ensure that when referencing a node that does not exist, an exception is thrown to address this error while passing in a string
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [ExpectedException (typeof(InvalidNameException))]
    public void Spreadsheet_TestSetCellContents_InvalidName_String()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("17A", "test");
    }

    /// <summary>
    ///     Ensure that when referencing a node that does not exist, an exception is thrown to address this error while passing in a number
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [ExpectedException(typeof(InvalidNameException))]
    public void Spreadsheet_TestSetCellContents_InvalidName_double()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("17A", "15.0");
    }

    /// <summary>
    ///     Ensure that when referencing a node that does not exist, an exception is thrown to address this error while passing in a formula
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [ExpectedException(typeof(InvalidNameException))]
    public void Spreadsheet_TestSetCellContents_InvalidName_Formula()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("17A", "=5 + A1");
    }


    /// <summary>
    ///     Ensure that when setting a node that is a dependee of other nodes, it accurately changes the others
    ///     when the value within it is changed
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestSetCellContents_AdjustingDependencies()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("A1", "=B1 + 4");
        testSheet.SetContentsOfCell("B1", "3");

        // Assert.AreEqual(new Formula("B1 + 4"), testSheet.GetCellContents("A1"));
        Assert.AreEqual(3.0, testSheet.GetCellContents("B1"));

        testSheet.SetContentsOfCell("B1", "14");
        // Assert.AreEqual(new Formula("B1 + 4"), testSheet.GetCellContents("A1"));
        Assert.AreEqual(14.0, testSheet.GetCellContents("B1"));
    }


    /// <summary>
    ///     Ensure that when getting the non-empty cells within the spreadsheet, 
    ///     the function properly represents an empty sheet
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
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
    ///     Ensure that when getting all the non-empty cells within the spreadsheet,
    ///     the function can determine a non-empty cell with an empty dependent cell by only returning the filled cell and
    ///     not the dependent cell
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestGetAllNonEmptyCells_DependencyOnEmptyCell()
    {
        Spreadsheet testSheet = new Spreadsheet();
        testSheet.SetContentsOfCell("A1", "=B1 + 15");

        List<string> actualList = testSheet.GetNamesOfAllNonemptyCells().ToList();

        ISet<string> testSet = new HashSet<string>();
        testSet.Add("A1");

        for (int i = 0; i < testSet.Count; i++)
        {
            Assert.AreEqual(testSet.ToList()[i], actualList[i]);
        }
    }


    /// <summary>
    ///     Ensure that when getting all the non-empty cells within the spreadsheet, 
    ///     the function does not return a cell that has been filled with an empty value after being filled once prior
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestGetAllNonEmptyCells_FilledThanEmptiedCell()
    {
        Spreadsheet testSheet = new Spreadsheet();
        testSheet.SetContentsOfCell("A1", "=B1 + 15");
        testSheet.SetContentsOfCell("A1", "");

        ISet<string> testSet = new HashSet<string>();
        testSet.Add("A1");

        List<string> actualList = testSheet.GetNamesOfAllNonemptyCells().ToList();

        for (int i = 0; i < actualList.Count; i++)
        {
            Assert.AreEqual(testSet.ToList()[i], actualList[i]);
        }
    }

    /// <summary>
    ///     Ensure that when getting a cell's contents, an exception is thrown if the name of the cell is invalid 
    ///     and thus, does not exist
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [ExpectedException(typeof(InvalidNameException))]
    public void Spreadsheet_TestGetCellContents_InvalidCell()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.GetCellContents("17A");
    }

    /// <summary>
    ///     Ensure that when given a formula in the form of a string, only a string is returned, rather than a new formula
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestGetCellContents_StringThatIsFormula()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("A1", "1 + 2");

        Assert.AreEqual("1 + 2", testSheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Ensure that when given a number in the form of a string, the same type of object is returned, rather than a double
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestGetCellContents_StringThatIsNumber()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("A1", "1");

        Assert.AreEqual(1.0, testSheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Ensure that when getting the contents of a non-active cell, an empty string is returned
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestGetCellContents_ValidNameEmptyCell()
    {
        Spreadsheet testSheet = new Spreadsheet();;

        Assert.AreEqual(string.Empty, testSheet.GetCellContents("A1"));
    }

    /// <summary>
    ///     Ensure that with a large number fo active cells, the spreadsheet can accruately manipulate one cell and have many other update
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestStress_SetCellContents()
    {
        Spreadsheet testSheet = new Spreadsheet();
        const int SIZE = 250;
        List<string> cellNames = new List<string>();

        for (int i = 0; i <= SIZE; i++)
        {
            cellNames.Add($"A{i}");
        }

        for(int i  = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j++)
            {
                testSheet.SetContentsOfCell(cellNames[i], $"={cellNames[j]} + {cellNames[j + 1]}");
            }
        }

        List<string> changedCells = testSheet.SetContentsOfCell(cellNames[SIZE], "15").ToList();
        cellNames.Reverse();
        cellNames.Remove(cellNames[1]);
        for (int i = 0; i < SIZE - 1; i++)
        {
            Assert.AreEqual(cellNames[i], changedCells[i]);
        }
    }


    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestGetCellValue_Formula_Valid()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("A1", "=B1 + 4");
        testSheet.SetContentsOfCell("B1", "15");

        Assert.AreEqual(19.0, testSheet.GetCellValue("A1"));
    }


    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestGetCellValue_Formula_Invalid()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("A1", "=B1 + 4");

        Assert.IsTrue(testSheet.GetCellValue("A1") is FormulaError);
    }


    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestGetCellValue_Double()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("A1", "4.17");

        if (testSheet.GetCellValue("A1") is double value)
        {
            Assert.AreEqual(4.17, value, 1e-9);
        }
    }


    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestGetCellValue_String()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("A1", "test phrase");

        if (testSheet.GetCellValue("A1") is string value)
        {
            Assert.AreEqual("test phrase", value);
        }
    }


    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestIndexer_FilledCells()
    {
        Spreadsheet testSheet = new Spreadsheet();
        const int SIZE = 100;

        for (double i = 1; i < SIZE; i++)
        {
            testSheet.SetContentsOfCell($"A{i}", i.ToString());
        }

        for (double i = 1; i < SIZE; i++)
        {
            Assert.AreEqual(i, testSheet[$"A{i}"]);
        }
    }


    [TestMethod]
    [Timeout(2000)]
    [ExpectedException(typeof(InvalidNameException))]
    public void Spreadsheet_TestIndexer_EmptyCell()
    {
        Spreadsheet testSheet = new Spreadsheet();

        _ = testSheet["A1"];
    }


    [TestMethod]
    [Timeout(2000)]
    public void Spreadsheet_TestSave_Changed()
    {
        Spreadsheet testSheet = new Spreadsheet();

        testSheet.SetContentsOfCell("A1", "5");
        testSheet.Save("testSheet.json");

        testSheet.SetContentsOfCell("A1", "15");
        Assert.AreEqual(15.0, testSheet.GetCellValue("A1"));

        testSheet.Load("testSheet.json");
        Assert.AreEqual(5.0, testSheet.GetCellValue("A1"));
    }
}