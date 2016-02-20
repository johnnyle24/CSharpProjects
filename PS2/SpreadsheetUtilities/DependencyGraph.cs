// Johnny Le, u0748407
// CS 3500

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// s1 depends on t1 --> t1 must be evaluated before s1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// (Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.)
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        Dictionary<string, HashSet<string>> dependents;
        Dictionary<string, HashSet<string>> dependees;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, HashSet<string>>(); // First dictionary
            dependees = new Dictionary<string, HashSet<string>>(); // Second dictionary

        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph. Creates a count and iterates through the collection.
        /// </summary>
        public int Size
        {
            get
            {
                int count = 0;
                foreach (string key in dependents.Keys)
                {
                    count += dependents[key].Count(); // For every key, adds a count for each value in the hashset
                }
                return count;
            }

        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                return GetDependees(s).Count(); // Counts the size of the hashset.
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (dependees.ContainsKey(s)) // If the string is in the dictionary, return true.
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (dependents.ContainsKey(s)) // If the string is in the dictionary, return true.
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (dependees.ContainsKey(s)) // Returns a list of the dependents if it exists.
            {
                return dependees[s].ToList();
            }
            else
            {
                HashSet<string> mtHash = new HashSet<string>(); // If it doesn't exist, return an empty hash set.
                return mtHash;
            }

        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (dependents.ContainsKey(s)) // Returns a list of the dependees if it exists.
            {
                return dependents[s].ToList();
            }
            else
            {
                HashSet<string> mtHash = new HashSet<string>(); // If it doesn't exist, return an empty hash set.
                return mtHash;
            }
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   s depends on t
        ///
        /// </summary>
        /// <param name="s"> s cannot be evaluated until t is</param>
        /// <param name="t"> t must be evaluated first.  S depends on T</param>
        public void AddDependency(string s, string t)
        {
            HashSet<string> thedependees = new HashSet<string>(); // Creates the hashset to throw into the dictionary
            if (!dependees.ContainsKey(s)) //If a hashset hasn't been created, add t to the empty hashset and add s with the hashset into the dictionary.
            {
                thedependees.Add(t);
                dependees.Add(s, thedependees);
            }
            else // If a hashset already exists for the key, just add t to it.
            {
                dependees[s].Add(t);
            }

            HashSet<string> thedependents = new HashSet<string>(); // Creates the hashset to throw into the dictionary
            if (!dependents.ContainsKey(t)) //If a hashset hasn't been created, add s to the empty hashset and add t with the hashset into the dictionary.
            {
                thedependents.Add(s);
                dependents.Add(t, thedependents);
            }
            else // If a hashset already exists for the key, just add s to it.
            {
                dependents[t].Add(s);
            }

        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s">The string that is dependent on t.</param>
        /// <param name="t">The string that s is dependent on.</param>
        public void RemoveDependency(string s, string t)
        {
            if (dependees.ContainsKey(s)) // Checks if s in dependees
            {
                if (dependees[s].Count() == 1) // If there is only pair corresponding with s, remove it from the hashset
                {
                    dependees.Remove(s);
                }
                else
                {
                    dependees[s].Remove(t); // If there is more than one pair corresponding with s, remove only t.
                }
            }
            if (dependents.ContainsKey(t))
            {
                if (dependents[t].Count() == 1) // If there is only pair corresponding with t, remove it from the hashset
                {
                    dependents.Remove(t);
                }
                else
                {
                    dependents[t].Remove(s); // If there is more than one pair corresponding with t, remove only s.
                }
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (dependees.ContainsKey(s)) // Checks if there is already the string in the dictionary
            {
                foreach (string r in dependees[s].ToList()) // Iterates through the collection and it changes it to a list
                {
                    this.RemoveDependency(s, r);
                }
            }
            foreach (string t in newDependents) // Iterates through the collection and it changes it to a list
            {
                this.AddDependency(s, t);
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (dependents.ContainsKey(s)) // Checks if there is already the string in the dictionary
            {
                foreach (string r in dependents[s].ToList()) // Iterates through the collection and it changes it to a list
                {
                    this.RemoveDependency(r, s);
                }
            }
            foreach (string t in newDependees) // Iterates through the collection and it changes it to a list
            {
                this.AddDependency(t, s);
            }

        }

    }




}


