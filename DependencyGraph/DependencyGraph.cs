using System.Xml.Linq;

// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta
// Version 1.3 - H. James de St. Germain Fall 2024
// (Clarified meaning of dependent and dependee.)
// (Clarified names in solution/project structure.)
namespace CS3500.DependencyGraph;

/// <summary>
///     <para>
///         (s1,t1) is an ordered pair of strings, meaning t1 depends on s1.
///         (in other words: s1 must be evaluated before t1.)
///     </para>
///     <para>
///         A DependencyGraph can be modeled as a set of ordered pairs of strings.
///         Two ordered pairs (s1,t1) and (s2,t2) are considered equal if and only
///         if s1 equals s2 and t1 equals t2.
///     </para>
///     <remarks>
///         Recall that sets never contain duplicates.
///         If an attempt is made to add an element to a set, and the element is already
///         in the set, the set remains unchanged.
///     </remarks>
///     <para>
///         Given a DependencyGraph DG:
///     </para>
///     <list type="number">
///         <item>
///             If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
///             (The set of things that depend on s.)
///         </item>
///         <item>
///             If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
///             (The set of things that s depends on.)
///         </item>
///     </list>
///     <para>
///         For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}.
///     </para>
///     <code>
///         dependents("a") = {"b", "c"}
///         dependents("b") = {"d"}
///         dependents("c") = {}
///         dependents("d") = {"d"}
///         dependees("a") = {}
///         dependees("b") = {"a"}
///         dependees("c") = {"a"}
///         dependees("d") = {"b", "d"}
///     </code>
/// </summary>
public class DependencyGraph
{
    private List<DependencyNode> connectionNodes; // The nodes containing values, allowing for reading and manipulation of the DependencyGraph

    /// <summary>
    ///     Initializes a new instance of the <see cref="DependencyGraph"/> class.
    ///     The initial DependencyGraph is empty.
    /// </summary>
    public DependencyGraph()
    {
        connectionNodes = new List<DependencyNode> (); // Initialize the list of nodes present in the graph
    }


    /// <summary>
    ///     The number of ordered pairs in the DependencyGraph.
    /// </summary>
    public int Size
    {
        get { return connectionNodes.Count; }
    }


    /// <summary>
    ///     Reports whether the given node has dependents (i.e., other nodes depend on it).
    /// </summary>
    /// <param name="nodeName"> The name of the node.</param>
    /// <returns> true if the node has dependents. </returns>
    public bool HasDependents(string nodeName)
    {
        // Get the node present within the graph
        DependencyNode currNode = GetNode(nodeName);

        // Esnure it is not null and return if it has dependents
        if (currNode != null)
        {
            return currNode.GetDependents().Count > 0;
        }

        return false;
    }


    /// <summary>
    ///     Reports whether the given node has dependees (i.e., depends on one or more other nodes).
    /// </summary>
    /// <returns> true if the node has dependees.</returns>
    /// <param name="nodeName">The name of the node.</param>
    public bool HasDependees(string nodeName)
    {
        // Get the node present within the graph
        DependencyNode currNode = GetNode(nodeName);

        // Esnure it is not null and return if it has dependees
        if (currNode != null)
        {
            return currNode.GetDependees().Count > 0;
        }

        return false;
    }


    /// <summary>
    ///     <para>
    ///         Returns the dependents of the node with the given name.
    ///     </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependents of nodeName. </returns>
    public IEnumerable<string> GetDependents(string nodeName)
    {
        // Get the node present within the graph
        DependencyNode currNode = GetNode(nodeName);

        // Ensuring the node is present, return a list of each dependent for this node
        if (currNode != null)
        {
            return currNode.GetDependents();
        }

        return new List<string> ();
    }


    /// <summary>
    ///     <para>
    ///         Returns the dependees of the node with the given name.
    ///     </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependees of nodeName. </returns>
    public IEnumerable<string> GetDependees(string nodeName)
    {
        // Get the node present within the graph
        DependencyNode currNode = GetNode(nodeName);

        // Ensuring the node is present, return a list of each dependee for this node
        if (currNode != null)
        {
            return currNode.GetDependees();
        }

        return new List<string> ();
    }


    /// <summary>
    ///     <para>
    ///         Adds the ordered pair (dependee, dependent), if it doesn't already exist (otherwise nothing happens).
    ///     </para>
    ///     <para>
    ///         This can be thought of as: dependee must be evaluated before dependent.
    ///     </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first.</param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until after the other node has been. </param>
    public void AddDependency(string dependee, string dependent)
    {
        // Do nothing if the dependee and dependent are the same
        if(dependee == dependent)
        {
            return;
        }

        // Determine if either node is present within the graph, adding them to it if not
        DependencyNode dependeeNode = GetNode(dependee);
        DependencyNode dependentNode = GetNode(dependent);
        if (dependeeNode == null)
        {
            dependeeNode = new DependencyNode(dependee);
            connectionNodes.Add(dependeeNode);
        }
        if (dependentNode == null)
        {
            dependentNode = new DependencyNode(dependent);
            connectionNodes.Add(dependentNode);
        }

        if (dependeeNode.dependents.Contains(dependent))
        {
            return;
        }

        // add the dependents and dependees to either one in this process
        dependeeNode.dependents.Add(dependent);
        dependentNode.dependees.Add(dependee);
    }


    /// <summary>
    ///     <para>
    ///         Removes the ordered pair (dependee, dependent), if it exists (otherwise nothing happens).
    ///     </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first. </param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until the other node has been. </param>
    public void RemoveDependency(string dependee, string dependent)
    {
        // Determine if both nodes are present within the graph, removing the dependency if so
        DependencyNode dependeeNode = GetNode(dependee);
        DependencyNode dependentNode = GetNode(dependent);
        if (dependeeNode != null && dependentNode != null)
        {
            dependeeNode.dependents.Remove(dependent);
            dependentNode.dependees.Remove(dependee);
        }
    }


    /// <summary>
    ///     Removes all existing ordered pairs of the form (nodeName, *). Then, for each t in newDependents, adds the ordered pair (nodeName, t).
    /// </summary>
    /// <param name="nodeName"> The name of the node who's dependents are being replaced. </param>
    /// <param name="newDependents"> The new dependents for nodeName. </param>
    public void ReplaceDependents(string nodeName, IEnumerable<string> newDependents)
    {
        DependencyNode node = GetNode(nodeName);
        List<string> tempDependents = new List<string>();

        foreach (string dependent in newDependents)
        {
            if (dependent != node.nodeName)
            {
                tempDependents.Add(dependent);
            }
        }

        node.dependents = tempDependents;
    }


    /// <summary>
    ///     <para>
    ///         Removes all existing ordered pairs of the form (*, nodeName). Then, for each t in newDependees, adds the ordered pair (t, nodeName).
    ///     </para>
    /// </summary>
    /// <param name="nodeName"> The name of the node who's dependees are being replaced. </param>
    /// <param name="newDependees"> The new dependees for nodeName. Could be empty.</param>
    public void ReplaceDependees(string nodeName, IEnumerable<string> newDependees)
    {
        DependencyNode node = GetNode(nodeName);
        List<string> tempDependees = new List<string>();

        foreach (string dependee in newDependees)
        {
            if (dependee != node.nodeName)
            {
                tempDependees.Add(dependee);
            }
        }

        node.dependees = tempDependees;
    }


    /// <summary>
    ///     <para>
    ///         A private helper method to find nodes already present within the graph, returning a null value if it does not exist
    ///          - This is done by scanning through all nodes within the graph to find the intended one
    ///     </para>
    /// </summary>
    /// <param name="nodeName"></param>
    /// <returns>
    ///     A node corresponding to the value passed into it
    /// </returns>
    private DependencyNode GetNode(string nodeName)
    {
        foreach (DependencyNode node in connectionNodes)
        {
            if (node.nodeName == nodeName)
            {
                return node;
            }
        }

        return null;
    }


    /// <summary>
    ///     <para>
    ///         A private, internal class to define nodes and their dependents and dependees
    ///     </para>
    ///     <list type="bullet">
    ///         <item>
    ///             nodeName - The value that is stored for other dependents and dependees to reference
    ///         </item>
    ///         <item>
    ///             dependents - The dependents corresponding to this value
    ///         </item>
    ///         <item>
    ///             dependees - The dependees corresponding to this value
    ///         </item>
    ///     </list>
    /// </summary>
    internal class DependencyNode
    {
        public string nodeName;
        public List<string> dependents;
        public List<string> dependees;


        /// <summary>
        ///     A constructor to create a new node for storing of dependents and dependees within the graph
        /// </summary>
        /// <param name="nodeName">
        ///     The value held within the node, being used for reference within other nodes, connecting the graph
        /// </param>
        public DependencyNode(string nodeName)
        {
            this.nodeName = nodeName;
            dependees = new List<string>();
            dependents = new List<string>();
        }


        /// <summary>
        ///     Get the dependees for a specific value
        /// </summary>
        /// <returns>
        ///     A list of all node values that are correlated as a dependee to this current value
        /// </returns>
        public List<string> GetDependees()
        {
            return dependees;
        }


        /// <summary>
        ///     Get the dependents of this specific value
        /// </summary>
        /// <returns>
        ///     A list of all node values that are correlated as a dependent with this current value
        /// </returns>
        public List<string> GetDependents()
        {
            return dependents;
        }
    }
}