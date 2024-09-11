namespace CS3500.DependencyGraphTests;

using CS3500.DependencyGraph;


/// <summary>
///     <para>
///         A class to test the functionality of the DependencyGraph class and the functionality within it,
///         allowing for further construction of more complex systems when necessary.
///     </para>
/// </summary>
[TestClass]
public class DependencyGraphTests
{


    /// <summary>
    ///     <para>
    ///         Check that when a graph is formed, it contains the correct number of values within it,
    ///         that being 0 as no values should be initialized upon creation
    ///     </para>
    /// </summary>
    [TestMethod]
    public void DependencyGraph_ConstructorTest_TrueSize()
    {
        DependencyGraph testGraph = new DependencyGraph();

        Assert.IsTrue(testGraph.Size == 0);
    }


    /// <summary>
    ///     <para>
    ///         Find that when given a series of compounding values, all dependent together, the values will not create
    ///         new instances of the values but rather, add the connections to existing ones, if already present
    ///     </para>
    /// </summary>
    [TestMethod]
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

        Assert.IsTrue(testGraph.Size == 10); // Ensure that the graph only contains 10 values
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when adding a dependency that already exists, nothing changes and only one connection 
    ///         between the two values exists, rather than multiple, allowing for potential errors with removing
    ///         dependencies
    ///     </para>
    /// </summary>
    [TestMethod]
    public void DependencyGraph_AddDependencyTest_DependencyExists()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("A", "C"); // A value is inserted between them to ensure that no memory can be used to fix this error and it must be identified during insertion
        testGraph.AddDependency("A", "B");

        string[] dependents = testGraph.GetDependents("A").ToArray();
        List<string> visited = new List<string>();

        // Esnure that only 2 values are connected to "A"
        for(int i = 0;i < dependents.Length; i++)
        {
            Assert.IsTrue(!visited.Contains(dependents[i]));
            visited.Add(dependents[i]);
        }
    }


    /// <summary>
    ///     <para>
    ///         Ensure that any value in the graph not present does not return true, as they would not have any dependents if they are not within the graph
    ///     </para>
    /// </summary>
    [TestMethod]
    public void DependencyGraph_HasDependentsTest_FalseCheck()
    {
        DependencyGraph testGraph = new DependencyGraph();

        Assert.IsFalse(testGraph.HasDependents("H"));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when a value present within the graph with one or more dependents shows that it contains those dependents
    ///     </para>
    /// </summary>
    [TestMethod]
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
    ///     <para>
    ///         Ensure that when removing dependencies from an empty graph, nothing is changed as there
    ///         is no dependency to remove
    ///     </para>
    /// </summary>
    [TestMethod]
    public void DependencyGraph_RemoveDependencyTest_EmptyGraph()
    {
        DependencyGraph testGraph = new DependencyGraph();

        Assert.IsTrue(testGraph.Size == 0);

        testGraph.RemoveDependency("A", "H");

        Assert.IsTrue(testGraph.Size == 0);
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when presented with a dependency in a full graph that does not exist, nothing changes and 
    ///         no dependencies present with either value within them are unaffected
    ///     </para>
    /// </summary>
    [TestMethod]
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
    ///     <para>
    ///         Ensure that when removing a dependency that exists, proper functionality is achieved and no additional changes are made
    ///     </para>
    /// </summary>
    [TestMethod]
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
    ///     <para>
    ///         Ensure that when finding the dependees of values in an empty graph, 
    ///         false is returned as they would not have dependees
    ///     </para>
    /// </summary>
    [TestMethod]
    public void DependencyGraph_HasDependeesTest_EmptyGraph()
    {
        DependencyGraph testGraph = new DependencyGraph();

        Assert.IsFalse(testGraph.HasDependees("A"));
        Assert.IsFalse(testGraph.HasDependees("E"));
    }


    /// <summary>
    ///     <para>
    ///         Ensure that when finding if a value has dependees in a filled graph without being in the graph, 
    ///         it returns false as it would not have dependees
    ///     </para>
    /// </summary>
    [TestMethod]
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
    ///     <para>
    ///         Ensure that when presented a filled graph with dependees of the designated values, true is returned
    ///     </para>
    /// </summary>
    [TestMethod]
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
    ///         Ensure that the number of dependees within a given value in an empty graph will be 0
    ///     </para>
    /// </summary>
    [TestMethod]
    public void DependencyGraph_GetDependeesTest_EmptyGraph()
    {
        DependencyGraph testGraph = new DependencyGraph();

        Assert.IsTrue(testGraph.GetDependees("A").Count() == 0);
    }


    /// <summary>
    ///     <para>
    ///         Ensure that a given value within a graph with multiple dependees provides all three properly
    ///     </para>
    /// </summary>
    [TestMethod]
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
    ///     <para>
    ///         Ensure that a value given within the denpendee position with none of its own will return a list of 0 values,
    ///         as it has 0 dependees connecting to it
    ///     </para>
    /// </summary>
    [TestMethod]
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
    ///     <para>
    ///         Ensure that a value removed from the dependent position, but remaining in the dependee position, 
    ///         thus keeping them in the graph maintains that it no longer has a dependee attached to it
    ///     </para>
    /// </summary>
    [TestMethod]
    public void DependencyGraph_GetDependeesTest_DependeeRemoved()
    {
        DependencyGraph testGraph = new DependencyGraph();

        testGraph.AddDependency("A", "B");
        testGraph.AddDependency("B", "C");
        testGraph.RemoveDependency("A", "B");

        Assert.IsTrue(testGraph.GetDependees("B").Count() == 0);
    }
}