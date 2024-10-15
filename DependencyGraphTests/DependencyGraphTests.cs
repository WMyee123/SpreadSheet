// <copyright file="DependencyGraphTests.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// Written by William Myers for CS 3500, September 2024
namespace CS3500.DependencyGraphTests;

using CS3500.DependencyGraph;

/// <summary>
///     A class to test the functionality of the DependencyGraph class and the functionality within it,
///     allowing for further construction of more complex systems when necessary.
/// </summary>
[TestClass]
public class DependencyGraphTests
{
    /// <summary>
    ///     Check that when a graph is formed, it contains the correct number of values within it,
    ///     that being 0 as no values should be initialized upon creation.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_ConstructorTest_TrueSize()
    {
        DependencyGraph testGraph = new DependencyGraph();

        Assert.IsTrue(testGraph.Size == 0);
    }

    /// <summary>
    ///     Find that when given a series of compounding values, all dependent together, the values will not create
    ///     new instances of the values but rather, add the connections to existing ones, if already present.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_SizeTest_LargeGraph()
    {
        DependencyGraph testGraph = new DependencyGraph();

        // Add far more than 10 values to the graph, connecting every node together in multiple ways
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                testGraph.AddDependency(i.ToString(), j.ToString());
            }
        }

        Assert.IsTrue(testGraph.Size == 100); // Ensure that the graph only contains 10 values
    }

    /// <summary>
    ///     Ensure that when adding a dependency that already exists, nothing changes and only one connection
    ///     between the two values exists, rather than multiple, allowing for potential errors with removing
    ///     dependencies.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_AddDependencyTest_DependencyExists()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("A", "C"); // A value is inserted between them to ensure that no memory can be used to fix this error and it must be identified during insertion
        testGraph.AddDependency("A", "B");

        string[] dependents = testGraph.GetDependents("A").ToArray();
        List<string> visited = new List<string>();

        // Ensure that only 2 values are connected to "A"
        for (int i = 0; i < dependents.Length; i++)
        {
            Assert.IsTrue(!visited.Contains(dependents[i]));
            visited.Add(dependents[i]);
        }
    }

    /// <summary>
    ///     Ensure that any value in the graph not present does not return true, as they would not have any dependents if they are not within the graph.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_HasDependentsTest_FalseCheck()
    {
        DependencyGraph testGraph = new DependencyGraph();

        Assert.IsFalse(testGraph.HasDependents("H"));
    }

    /// <summary>
    ///     Ensure that when a value present within the graph with one or more dependents shows that it contains those dependents.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_HasDependentsTest_TrueCheck()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("A", "C");
        testGraph.AddDependency("C", "B");
        testGraph.AddDependency("B", "E");
        testGraph.AddDependency("E", "H");

        Assert.IsTrue(testGraph.HasDependents("A")); // This was added to make sure that more than 1 dependent has no affect on the return value
        Assert.IsTrue(testGraph.HasDependents("E"));
    }

    /// <summary>
    ///     Ensure that when removing dependencies from an empty graph, nothing is changed as there
    ///     is no dependency to remove.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_RemoveDependencyTest_EmptyGraph()
    {
        DependencyGraph testGraph = new DependencyGraph();

        Assert.IsTrue(testGraph.Size == 0);

        testGraph.RemoveDependency("A", "H");

        Assert.IsTrue(testGraph.Size == 0);
    }

    /// <summary>
    ///     Ensure that when presented with a dependency in a full graph that does not exist, nothing changes and
    ///     no dependencies present with either value within them are unaffected.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_RemoveDependencyTest_FilledGraph_False()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("A", "C");
        testGraph.AddDependency("C", "B");
        testGraph.AddDependency("B", "E");
        testGraph.AddDependency("E", "H");

        Assert.IsFalse(testGraph.GetDependents("A").Contains("H"));

        testGraph.RemoveDependency("A", "H");

        Assert.IsFalse(testGraph.GetDependents("A").Contains("H"));

        // Ensure that no changes occurred within the dependencies that do exist
        Assert.IsTrue(testGraph.GetDependents("A").Contains("B"));
        Assert.IsTrue(testGraph.GetDependents("A").Contains("C"));
        Assert.IsTrue(testGraph.GetDependees("H").Contains("E"));
    }

    /// <summary>
    ///     Ensure that when removing a dependency that exists, proper functionality is achieved and no additional changes are made.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_RemoveDependencyTest_FilledGraph_True()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("A", "C");
        testGraph.AddDependency("C", "B");
        testGraph.AddDependency("B", "E");
        testGraph.AddDependency("E", "H");

        Assert.IsTrue(testGraph.GetDependents("C").Contains("B"));

        testGraph.RemoveDependency("C", "B");

        Assert.IsFalse(testGraph.GetDependents("C").Contains("B"));

        // Ensure that no changes occurred within the extra dependencies
        Assert.IsTrue(testGraph.GetDependents("B").Contains("E"));
        Assert.IsTrue(testGraph.GetDependees("C").Contains("A"));
        Assert.IsTrue(testGraph.GetDependees("B").Contains("A"));
    }

    /// <summary>
    ///     Ensure that when finding the dependees of values in an empty graph,
    ///     false is returned as they would not have dependees.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_HasDependeesTest_EmptyGraph()
    {
        DependencyGraph testGraph = new DependencyGraph();

        Assert.IsFalse(testGraph.HasDependees("A"));
        Assert.IsFalse(testGraph.HasDependees("E"));
    }

    /// <summary>
    ///     Ensure that when finding if a value has dependees in a filled graph without being in the graph,
    ///     it returns false as it would not have dependees.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_HasDependeesTest_FullGraph_False()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("A", "C");
        testGraph.AddDependency("C", "B");
        testGraph.AddDependency("B", "E");
        testGraph.AddDependency("E", "H");

        Assert.IsFalse(testGraph.HasDependees("F"));
        Assert.IsFalse(testGraph.HasDependees("A")); // Ensure that a dependee with no dependees does not have any itself
    }

    /// <summary>
    ///     Ensure that when presented a filled graph with dependees of the designated values, true is returned.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_HasDependeesTest_FullGraph_True()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("A", "C");
        testGraph.AddDependency("C", "B");
        testGraph.AddDependency("B", "E");
        testGraph.AddDependency("E", "H");

        Assert.IsTrue(testGraph.HasDependees("B")); // Make sure that a value with more than 1 dependee is properly represented
        Assert.IsTrue(testGraph.HasDependees("E"));
    }

    /// <summary>
    ///     <para>
    ///         Ensure that the number of dependees within a given value in an empty graph will be 0.
    ///     </para>
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_GetDependeesTest_EmptyGraph()
    {
        DependencyGraph testGraph = new DependencyGraph();

        Assert.IsTrue(testGraph.GetDependees("A").Count() == 0);
    }

    /// <summary>
    ///     Ensure that a given value within a graph with multiple dependees provides all three properly.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_GetDependeesTest_FilledGraph()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("C", "B");
        testGraph.RemoveDependency("B", "E");
        testGraph.AddDependency("R", "B");

        Assert.IsTrue(testGraph.GetDependees("B").Count() == 3);

        IEnumerable<string> dependees = testGraph.GetDependees("B");

        Assert.IsTrue(dependees.Contains("A"));
        Assert.IsTrue(dependees.Contains("C"));
        Assert.IsTrue(dependees.Contains("R"));
    }

    /// <summary>
    ///     Ensure that a value given within the denpendee position with none of its own will return a list of 0 values,
    ///     as it has 0 dependees connecting to it.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_GetDependeesTest_FilledGraph_EmptyReturn()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "C");
        testGraph.AddDependency("B", "C");
        testGraph.RemoveDependency("B", "R");
        testGraph.AddDependency("E", "R");

        Assert.IsTrue(testGraph.GetDependees("B").Count() == 0);
        Assert.IsTrue(testGraph.GetDependees("A").Count() == 0);
    }

    /// <summary>
    ///     Ensure that a value removed from the dependent position, but remaining in the dependee position,
    ///     thus keeping them in the graph maintains that it no longer has a dependee attached to it.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_GetDependeesTest_DependeeRemoved()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("B", "C");
        testGraph.RemoveDependency("A", "B");

        Assert.IsTrue(testGraph.GetDependees("B").Count() == 0);
    }

    /// <summary>
    ///     Ensure that an empty graph, when given a value to for the dependents of, returns an empty list.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_GetDependentsTest_EmptyGraph()
    {
        DependencyGraph testGraph = new DependencyGraph();

        Assert.IsTrue(testGraph.GetDependents("B").Count() == 0);
    }

    /// <summary>
    ///     Ensure that when finding if a value has dependents in a filled graph without being in the graph,
    ///     it returns false as it would not have dependents.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_HasDependentsTest_FullGraph_False()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("A", "C");
        testGraph.AddDependency("C", "B");
        testGraph.AddDependency("B", "E");
        testGraph.AddDependency("F", "H");

        Assert.IsFalse(testGraph.HasDependents("E"));
        Assert.IsFalse(testGraph.HasDependents("H")); // Ensure that a dependent with no dependents does not have any itself
    }

    /// <summary>
    ///     Ensure that when finding the dependents of values in an empty graph,
    ///     false is returned as they would not have dependents.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_HasDependentsTest_EmptyGraph()
    {
        DependencyGraph testGraph = new DependencyGraph();

        Assert.IsFalse(testGraph.HasDependents("A"));
        Assert.IsFalse(testGraph.HasDependents("E"));
    }

    /// <summary>
    ///     Ensure that when presented a filled graph with dependents of the designated values, true is returned.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_HasDependentsTest_FullGraph_True()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("A", "C");
        testGraph.AddDependency("C", "B");
        testGraph.AddDependency("B", "E");
        testGraph.AddDependency("E", "H");

        Assert.IsTrue(testGraph.HasDependents("B")); // Make sure that a value with more than 1 dependent is properly represented
        Assert.IsTrue(testGraph.HasDependents("E"));
    }

    /// <summary>
    ///     Ensure that a given value within a graph with multiple dependents provides all three properly.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_GetDependentsTest_FilledGraph()
    {
        DependencyGraph testGraph = new DependencyGraph();
        testGraph.AddDependency("B", "A");
        testGraph.AddDependency("B", "C");
        testGraph.RemoveDependency("B", "A");
        testGraph.AddDependency("B", "E");

        Assert.IsTrue(testGraph.GetDependents("B").Count() == 2);

        IEnumerable<string> dependents = testGraph.GetDependents("B");

        Assert.IsTrue(dependents.Contains("C"));
        Assert.IsTrue(dependents.Contains("E"));
    }

    /// <summary>
    ///         Ensure that a value given within the dependent position with none of its own will return a list of 0 values,
    ///         as it has 0 dependents connecting to it.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_GetDependentsTest_FilledGraph_EmptyReturn()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "D");
        testGraph.AddDependency("B", "C");
        testGraph.RemoveDependency("B", "R");
        testGraph.AddDependency("E", "R");

        Assert.IsTrue(testGraph.GetDependents("R").Count() == 0);
        Assert.IsTrue(testGraph.GetDependents("C").Count() == 0);
    }

    /// <summary>
    ///     Ensure that when replacing a value's dependees, it functions properly and
    ///     replaces all the values with the new list of value assigned.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_ReplaceDependeesTest()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("C", "B");
        testGraph.RemoveDependency("B", "E");
        testGraph.AddDependency("R", "B");

        List<string> newDependees = new List<string>();

        newDependees.Add("7");

        testGraph.ReplaceDependees("A", newDependees);

        Assert.IsTrue(testGraph.GetDependees("A").Count() == 1);
    }

    /// <summary>
    ///     Ensure that when replacing a value's dependees with a new value, it will not add its own value to this new list.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_ReplaceDependeesTest_SameNode()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("C", "B");
        testGraph.RemoveDependency("B", "E");
        testGraph.AddDependency("R", "B");

        List<string> newDependees = new List<string>();

        newDependees.Add("A");

        testGraph.ReplaceDependees("A", newDependees);

        Assert.IsTrue(testGraph.GetDependees("A").Count() == 1);
    }

    /// <summary>
    ///     Ensure that when replacing the dependents of a value, each value is removed and reset to a new list of values.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_ReplaceDependentsTest()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("C", "B");
        testGraph.RemoveDependency("B", "E");
        testGraph.AddDependency("R", "B");

        List<string> newDependents = new List<string>();

        newDependents.Add("7");

        testGraph.ReplaceDependents("A", newDependents);

        Assert.IsTrue(testGraph.GetDependents("A").Count() == 1);
    }

    /// <summary>
    ///     Ensure that when replacing the dependents of a value with itself, it is unable to do so.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void DependencyGraph_ReplaceDependentsTest_SameNode()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("C", "B");
        testGraph.RemoveDependency("B", "E");
        testGraph.AddDependency("R", "B");

        List<string> newDependents = new List<string>();

        newDependents.Add("A");

        testGraph.ReplaceDependents("A", newDependents);

        Assert.IsTrue(testGraph.GetDependents("A").Count() == 1);
    }
}