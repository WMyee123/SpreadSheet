using CS3500.Formula;
using CS3500.Spreadsheet;

namespace SpreadsheetTests;


[TestClass]
public class SpreadsheetTests
{
    [TestMethod]
    public void Spreadsheet_TestSetCellContents_Integer()
    {
        Spreadsheet testSheet = new Spreadsheet();

        Assert.AreEqual(["A1"], testSheet.SetCellContents("A1", 15));
        Assert.AreEqual(["A1"], testSheet.SetCellContents("A1", -15)); // Check for negative values

        // Check for multiple forms of decimal values being utilized in a cell
        Assert.AreEqual(["A1"], testSheet.SetCellContents("A1", 0.0423));
        Assert.AreEqual(["A1"], testSheet.SetCellContents("A1", 4.23E-2));
    }


    [TestMethod]
    public void Spreadsheet_TestSetCellContents_String()
    {
        Spreadsheet testSheet = new Spreadsheet();

        Assert.AreEqual(["A1"], testSheet.SetCellContents("A1", "Test String"));
    }


    [TestMethod]
    public void Spreadsheet_TestSetCellContents_Formula_NoDependencies()
    {
        Spreadsheet testSheet = new Spreadsheet();

        Assert.AreEqual(["A1"], testSheet.SetCellContents("A1", new Formula("17 + 4")));
    }


    [TestMethod]
    public void Spreadsheet_TestSetCellContents_Formula_Dependencies()
    {
        Spreadsheet testSheet = new Spreadsheet();

        Assert.AreEqual(["A1"], testSheet.SetCellContents("A1", new Formula("B1 + 4")));
        Assert.AreEqual(["B1, A1"], testSheet.SetCellContents("B1", new Formula("C1 * 2")));
        Assert.AreEqual(["C1, B1, A1"], testSheet.SetCellContents("C1", 15));
    }


    [TestMethod]
    public void Spreadsheet_TestSetCellContents_Formula_CircularDependency()
    {
        Spreadsheet testSheet = new Spreadsheet();

        Assert.AreEqual(["A1"], testSheet.SetCellContents("A1", new Formula("B1 + 4")));
        Assert.ThrowsException<CircularException>(testSheet.SetCellContents("B1", new Formula("A1 * 2")));
    }


    [TestMethod]
    public void Spreadsheet_TestSetCellContents_InvalidName()
    {
        Spreadsheet testSheet = new Spreadsheet();

        Assert.ThrowsException<InvalidNameException>(testSheet.SetCellContents("17A", "null"));
    }


    [TestMethod]
    public void Spreadsheet_TestGetAllNonEmptyCells_OnlyEmptyCells()
    {
        Spreadsheet testSheet = new Spreadsheet();
        ISet<string> testSet = new HashSet<string>();

        Assert.AreEqual(testSet, testSheet.GetNamesOfAllNonemptyCells());
    }


    [TestMethod]
    public void Spreadsheet_TestGetAllNonEmptyCells_DependencyOnEmptyCell()
    {
        Spreadsheet testSheet = new Spreadsheet();
        testSheet.SetCellContents("A1", new Formula("B1 + 15"));
        ISet<string> testSet = new HashSet<string>();
        testSet.Add("A1");

        Assert.AreEqual(testSet, testSheet.GetNamesOfAllNonemptyCells());
    }


    [TestMethod]
    public void Spreadsheet_TestGetAllNonEmptyCells_FilledThanEmptiedCell()
    {
        Spreadsheet testSheet = new Spreadsheet();
        testSheet.SetCellContents("A1", new Formula("B1 + 15"));
        testSheet.SetCellContents("A1", "");

        ISet<string> testSet = new HashSet<string>();

        Assert.AreEqual(testSet, testSheet.GetNamesOfAllNonemptyCells());
    }


    [TestMethod]
    public void Spreadsheet_TestGetCellContents_InvalidCell()
    {
        Spreadsheet testSheet = new Spreadsheet();

        Assert.ThrowsException<InvalidNameException>((Action)testSheet.GetCellContents("17A"));
    }
}