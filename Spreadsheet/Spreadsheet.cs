﻿// <copyright file="DeleteMeClass1.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// Written by Joe Zachary for CS 3500, September 2013
// Update by Profs Kopta and de St. Germain
// - Updated return types
// - Updated documentation
namespace CS3500.Spreadsheet;

using CS3500.DependencyGraph;
using CS3500.Formula;
using System.ComponentModel;

using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

/// <summary>
/// <para>
/// Thrown to indicate that a change to a cell will cause a circular dependency.
/// </para>
/// </summary>
public class CircularException : Exception
{
}

/// <summary>
/// <para>
/// Thrown to indicate that a name parameter was invalid.
/// </para>
/// </summary>
public class InvalidNameException : Exception
{
}

/// <summary>
/// <para>
/// An Spreadsheet object represents the state of a simple spreadsheet. A
/// spreadsheet represents an infinite number of named cells.
/// </para>
/// <para>
/// Valid Cell Names: A string is a valid cell name if and only if it is one or
/// more letters followed by one or more numbers, e.g., A5, BC27.
/// </para>
/// <para>
/// Cell names are case insensitive, so "x1" and "X1" are the same cell name.
/// Your code should normalize (uppercased) any stored name but accept either.
/// </para>
/// <para>
/// A spreadsheet represents a cell corresponding to every possible cell name. (This
/// means that a spreadsheet contains an infinite number of cells.) In addition to
/// a name, each cell has a contents and a value. The distinction is important.
/// </para>
/// <para>
/// The <b>contents</b> of a cell can be (1) a string, (2) a double, or (3) a Formula.
/// If the contents of a cell is set to the empty string, the cell is considered empty.
/// </para>
/// <para>
/// By analogy, the contents of a cell in Excel is what is displayed on
/// the editing line when the cell is selected.
/// </para>
/// <para>
/// In a new spreadsheet, the contents of every cell is the empty string. Note:
/// this is by definition (it is IMPLIED, not stored).
/// </para>
/// <para>
/// The <b>value</b> of a cell can be (1) a string, (2) a double, or (3) a FormulaError.
/// (By analogy, the value of an Excel cell is what is displayed in that cell's position
/// in the grid.)
/// </para>
/// <list type="number">
/// <item>If a cell's contents is a string, its value is that string.</item>
/// <item>If a cell's contents is a double, its value is that double.</item>
/// <item>
/// <para>
/// If a cell's contents is a Formula, its value is either a double or a FormulaError,
/// as reported by the Evaluate method of the Formula class. For this assignment,
/// you are not dealing with values yet.
/// </para>
/// </item>
/// </list>
/// <para>
/// Spreadsheets are never allowed to contain a combination of Formulas that establish
/// a circular dependency. A circular dependency exists when a cell depends on itself.
/// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
/// A1 depends on B1, which depends on C1, which depends on A1. That's a circular
/// dependency.
/// </para>
/// </summary>
public class Spreadsheet
{
    private Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
    private DependencyGraph dependencies = new DependencyGraph();

    /// <summary>
    /// Provides a copy of the names of all of the cells in the spreadsheet
    /// that contain information (i.e., not empty cells).
    /// </summary>
    /// <returns>
    /// A set of the names of all the non-empty cells in the spreadsheet.
    /// </returns>
    public ISet<string> GetNamesOfAllNonemptyCells()
    {
        return cells.Keys.ToHashSet();
    }

    /// <summary>
    /// Returns the contents (as opposed to the value) of the named cell.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    /// Thrown if the name is invalid.
    /// </exception>
    ///
    /// <param name="name">The name of the spreadsheet cell to query. </param>
    /// <returns>
    /// The contents as either a string, a double, or a Formula.
    /// See the class header summary.
    /// </returns>
    public object GetCellContents(string name)
    {
        if (!ValidCell(name))
        {
            throw new InvalidNameException();
        }

        if (cells.TryGetValue(name, out Cell currCell))
        {
            return currCell.GetContents();
        }

        return string.Empty;
    }

    /// <summary>
    /// Set the contents of the named cell to the given number.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    /// If the name is invalid, throw an InvalidNameException.
    /// </exception>
    ///
    /// <param name="name"> The name of the cell. </param>
    /// <param name="number"> The new content of the cell. </param>
    /// <returns>
    /// <para>
    /// This method returns an ordered list consisting of the passed in name
    /// followed by the names of all other cells whose value depends, directly
    /// or indirectly, on the named cell.
    /// </para>
    /// <para>
    /// The order must correspond to a valid dependency ordering for recomputing
    /// all of the cells, i.e., if you re-evaluate each cell in the order of the list,
    /// the overall spreadsheet will be correctly updated.
    /// </para>
    /// <para>
    /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    /// list [A1, B1, C1] is returned, i.e., A1 was changed, so then A1 must be
    /// evaluated, followed by B1 re-evaluated, followed by C1 re-evaluated.
    /// </para>
    /// </returns>
    public IList<string> SetCellContents(string name, double number)
    {
        // Check that the cell that is being set has a valid name
        if (!ValidCell(name))
        {
            throw new InvalidNameException();
        }

        // If a cell is empty, set that sell to contain the provided number, otherwise remove the cell and
        // add it back to the spreadsheet, resetting the contents of the cell in the process
        if (!cells.TryGetValue(name, out Cell cellVal))
        {
            cells.Add(name, new Cell(number));
        }
        else
        {
            List<string> dependents = dependencies.GetDependents(name).ToList();
            foreach (string dependentCell in dependents)
            {
                dependencies.RemoveDependency(name, dependentCell);
            }

            cells.Remove(name);
            cells.Add(name, new Cell(number));
        }

        return GetCellsToRecalculate(name).ToList(); // Return a list of all cells that need to be recalculated for proper representation in the spreadsheet
    }

    /// <summary>
    /// The contents of the named cell becomes the given text.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    /// If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="text"> The new content of the cell. </param>
    /// <returns>
    /// The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    public IList<string> SetCellContents(string name, string text)
    {
        // Check that the cell that is being set has a valid name
        if (!ValidCell(name))
        {
            throw new InvalidNameException();
        }

        // If a cell is empty, set that sell to contain the provided string, otherwise remove the cell and
        // add it back to the spreadsheet, resetting the contents of the cell in the process
        if (!cells.TryGetValue(name, out Cell cellVal))
        {
            cells.Add(name, new Cell(text));
        }
        else
        {
            List<string> dependents = dependencies.GetDependents(name).ToList();
            foreach (string dependentCell in dependents)
            {
                dependencies.RemoveDependency(name, dependentCell);
            }

            cells.Remove(name);
            cells.Add(name, new Cell(text));
        }

        return GetCellsToRecalculate(name).ToList(); // Return a list of all cells that need to be recalculated for proper representation in the spreadsheet
    }

    /// <summary>
    /// Set the contents of the named cell to the given formula.
    /// </summary>
    /// <exception cref="InvalidNameException">
    /// If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    /// <para>
    /// If changing the contents of the named cell to be the formula would
    /// cause a circular dependency, throw a CircularException.
    /// </para>
    /// <para>
    /// No change is made to the spreadsheet.
    /// </para>
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="formula"> The new content of the cell. </param>
    /// <returns>
    /// The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    public IList<string> SetCellContents(string name, Formula formula)
    {
        // Check that the cell that is being set has a valid name
        if (!ValidCell(name))
        {
            throw new InvalidNameException();
        }

        // If a cell is empty, set that sell to contain the provided Formula, otherwise remove the cell and
        // add it back to the spreadsheet, resetting the contents of the cell in the process
        if (!cells.TryGetValue(name, out Cell cellVal))
        {
            cells.Add(name, new Cell(formula));
        }
        else
        {
            cells.Remove(name);
            cells.Add(name, new Cell(formula));
        }

        // Replace dependencies in the graph, using the variables present in the Formula provided to link these cells together
        dependencies.ReplaceDependents(name, formula.GetVariables().ToList());

        return GetCellsToRecalculate(name).ToList(); // Return a list of all cells that need to be recalculated for proper representation in the spreadsheet
    }

    /// <summary>
    /// Returns an enumeration, without duplicates, of the names of all cells whose
    /// values depend directly on the value of the named cell.
    /// </summary>
    /// <param name="name"> This <b>MUST</b> be a valid name. </param>
    /// <returns>
    /// <para>
    /// Returns an enumeration, without duplicates, of the names of all cells
    /// that contain formulas containing name.
    /// </para>
    /// <para>For example, suppose that: </para>
    /// <list type="bullet">
    /// <item>A1 contains 3</item>
    /// <item>B1 contains the formula A1 * A1</item>
    /// <item>C1 contains the formula B1 + A1</item>
    /// <item>D1 contains the formula B1 - C1</item>
    /// </list>
    /// <para> The direct dependents of A1 are B1 and C1. </para>
    /// </returns>
    private IEnumerable<string> GetDirectDependents(string name)
    {
        return dependencies.GetDependees(name);
    }

    /// <summary>
    /// <para>
    /// This method is implemented for you, but makes use of your GetDirectDependents.
    /// </para>
    /// <para>
    /// Returns an enumeration of the names of all cells whose values must
    /// be recalculated, assuming that the contents of the cell referred
    /// to by name has changed. The cell names are enumerated in an order
    /// in which the calculations should be done.
    /// </para>
    /// <exception cref="CircularException">
    /// If the cell referred to by name is involved in a circular dependency,
    /// throws a CircularException.
    /// </exception>
    /// <para>
    /// For example, suppose that:
    /// </para>
    /// <list type="number">
    /// <item>
    /// A1 contains 5
    /// </item>
    /// <item>
    /// B1 contains the formula A1 + 2.
    /// </item>
    /// <item>
    /// C1 contains the formula A1 + B1.
    /// </item>
    /// <item>
    /// D1 contains the formula A1 * 7.
    /// </item>
    /// <item>
    /// E1 contains 15
    /// </item>
    /// </list>
    /// <para>
    /// If A1 has changed, then A1, B1, C1, and D1 must be recalculated,
    /// and they must be recalculated in an order which has A1 first, and B1 before C1
    /// (there are multiple such valid orders).
    /// The method will produce one of those enumerations.
    /// </para>
    /// <para>
    /// PLEASE NOTE THAT THIS METHOD DEPENDS ON THE METHOD GetDirectDependents.
    /// IT WON'T WORK UNTIL GetDirectDependents IS IMPLEMENTED CORRECTLY.
    /// </para>
    /// </summary>
    /// <param name="name"> The name of the cell. Requires that name be a valid cell name.</param>
    /// <returns>
    /// Returns an enumeration of the names of all cells whose values must
    /// be recalculated.
    /// </returns>
    private IEnumerable<string> GetCellsToRecalculate(string name)
    {
        LinkedList<string> changed = new();
        HashSet<string> visited = [];
        Visit(name, name, visited, changed);
        return changed;
    }

    /// <summary>
    ///     A helper for the GetCellsToRecalculate method.
    /// </summary>
    private void Visit(string start, string name, ISet<string> visited, LinkedList<string> changed)
    {
        // Add each cell that can be visited, recursively looking at each dependent of the current cell
        visited.Add(name); // Mark that the current node was visited
        foreach (string dependent in GetDirectDependents(name))
        {
            // Unless the cell is the starting cell, continue searching through each dependent node until none are left
            if (dependent.Equals(start))
            {
                throw new CircularException();
            }
            else if (!visited.Contains(dependent))
            {
                Visit(start, dependent, visited, changed);
            }
        }

        changed.AddFirst(name); // Add the most recently visited cell to the front of the list to allow for proper representation of the dependencies in the spreadsheet
    }

    /// <summary>
    ///     A helper method to determine if a given cell's name is valid for searching within the spreadsheet
    /// </summary>
    /// <param name="name"> The cell's given name to determine validity of </param>
    /// <returns> A boolean statement stating if the given cell is valid for searching in the spreadsheet </returns>
    private bool ValidCell(string name)
    {
        bool foundLetter = false; // Determine if the first value found is a letter as that is required for a proper cell name
        string prevLetter = string.Empty;

        // Look through each cell and determine what it is and if it follows the rules of a cell's naming convention
        for (int i = 0; i < name.Length; i++)
        {
            string currVal = name[i].ToString();

            // If the value is a number, determine that it came after a letter and if not, return false
            if (double.TryParse(currVal, out double number))
            {
                if (!foundLetter)
                {
                    return false;
                }
            }

            // If the current character is a letter, mark that a letter was visited
            else
            {
                if (char.IsLetter(currVal.ToCharArray()[0]))
                {
                    foundLetter = true;
                    if (prevLetter != string.Empty && !char.IsLetter(prevLetter.ToCharArray()[0]))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            prevLetter = currVal;
        }

        return foundLetter; // Return whether the value is valid or not
    }

    /// <summary>
    ///     <para>
    ///         A class to define a cell and the contents stored within it
    ///     </para>
    ///     <list type="number">
    ///         <item>
    ///             contents - The value stored within the cell for reference in the spreadsheet
    ///         </item>
    ///     </list>
    /// </summary>
    internal class Cell
    {
        private object contents; // A string representation of the values that can be stored in the cell

        /// <summary>
        ///     Initializes a new instance of the <see cref="Cell"/> class, containing a number represented as a double.
        /// </summary>
        /// <param name="data"> The number that is to be stored within the cell. </param>
        public Cell(double data)
        {
            contents = data;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Cell"/> class, containing a string of characters.
        /// </summary>
        /// <param name="data"> The string that is to be stored within the cell. </param>
        public Cell(string data)
        {
            contents = data;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Cell"/> class, containing a formula for finding a value represented in this formula.
        /// </summary>
        /// <param name="data"> The formula that is to be stored within the cell. </param>
        public Cell(Formula data)
        {
            contents = data;
        }

        /// <summary>
        ///     Get the contents that are stored within the cell, returning a string representation of these contents.
        /// </summary>
        /// <returns> A string representation of the contents within the cell for manipulation within the spreadsheet. </returns>
        public object GetContents()
        {
            return contents;
        }
    }
}