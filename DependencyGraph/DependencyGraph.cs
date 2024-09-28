// <copyright file="DependencyGraph.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> William Myers </authors>
// <date> 9/27/2024 </date>

// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta
// Version 1.3 - H. James de St. Germain Fall 2024
// (Clarified meaning of dependent and dependee.)
// (Clarified names in solution/project structure.)
namespace CS3500.DependencyGraph;

using System.Xml.Linq;

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
    private Dictionary<string, List<string>> dependees;
    private Dictionary<string, List<string>> dependents;
    private int size; // The number of pairs in the DependencyGraph

    /// <summary>
    ///     Initializes a new instance of the <see cref="DependencyGraph"/> class.
    ///     The initial DependencyGraph is empty.
    /// </summary>
    public DependencyGraph()
    {
        // Initialize the list of nodes present in the graph
        this.dependees = new Dictionary<string, List<string>>();
        this.dependents = new Dictionary<string, List<string>>();
        this.size = 0;
    }

    /// <summary>
    ///     Gets the number of ordered pairs in the DependencyGraph.
    /// </summary>
    public int Size
    {
        get { return this.size; }
    }

    /// <summary>
    ///     Reports whether the given node has dependents (i.e., other nodes depend on it).
    /// </summary>
    /// <param name="nodeName"> The name of the node.</param>
    /// <returns> true if the node has dependents. </returns>
    public bool HasDependents(string nodeName)
    {
        this.dependents.TryGetValue(nodeName, out var dependentList);
        if (dependentList != null)
        {
            return dependentList.Count > 0;
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
        this.dependees.TryGetValue(nodeName, out var dependeeList);
        if (dependeeList != null)
        {
            return dependeeList.Count > 0;
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
        this.dependents.TryGetValue(nodeName, out var dependentList);
        if (dependentList != null)
        {
            return dependentList;
        }

        return new List<string>();
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
        this.dependees.TryGetValue(nodeName, out var dependeeList);
        if (dependeeList != null)
        {
            return dependeeList;
        }

        return new List<string>();
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
        // Determine if either node is present within the graph, adding them to it if not
        if (!this.dependees.TryGetValue(dependent, out var dependeeList))
        {
            dependeeList = new List<string>();
            this.dependees.Add(dependent, dependeeList);
        }

        if (!this.dependents.TryGetValue(dependee, out var dependentList))
        {
            dependentList = new List<string>();
            this.dependents.Add(dependee, dependentList);
        }

        if (dependeeList.Contains(dependee))
        {
            return;
        }

        // add the dependents and dependees to either one in this process
        dependeeList.Add(dependee);
        dependentList.Add(dependent);
        this.size++;
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
        if (this.dependees.TryGetValue(dependent, out var dependeeList) && this.dependents.TryGetValue(dependee, out var dependentList))
        {
            dependentList.Remove(dependent);
            dependeeList.Remove(dependee);
            this.size--;

            if (dependentList.Count == 0)
            {
                this.dependents.Remove(dependee);
            }

            if (dependeeList.Count == 0)
            {
                this.dependees.Remove(dependent);
            }
        }
    }

    /// <summary>
    ///     Removes all existing ordered pairs of the form (nodeName, *). Then, for each t in newDependents, adds the ordered pair (nodeName, t).
    /// </summary>
    /// <param name="nodeName"> The name of the node who's dependents are being replaced. </param>
    /// <param name="newDependents"> The new dependents for nodeName. </param>
    public void ReplaceDependents(string nodeName, IEnumerable<string> newDependents)
    {
        if (this.dependents.TryGetValue(nodeName, out var dependentsList))
        {
            List<string> tempList = dependentsList;
            while (tempList.Count > 0)
            {
                this.RemoveDependency(nodeName, tempList.Last());
            }
        }

        foreach (string dependent in newDependents)
        {
            this.AddDependency(nodeName, dependent);
        }
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
        if (this.dependees.TryGetValue(nodeName, out var dependeesList))
        {
            List<string> tempList = dependeesList;
            while (tempList.Count > 0)
            {
                this.RemoveDependency(tempList.Last(), nodeName);
            }
        }

        foreach (string dependee in newDependees)
        {
            this.AddDependency(dependee, nodeName);
        }
    }
}