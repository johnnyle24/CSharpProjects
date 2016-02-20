// Johnny Le, u0748407 CS 3500
// Branch PS5
using SS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;

namespace SS
{
    /// <summary>
    /// Creates a spreadsheet that utilizes a dictionary and a dependency graph.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, cell> sheet; //Private dictionary for the sheet

        private DependencyGraph depends; //Dependency graph

        /// <summary>
        /// Constructor that sets the parameter for the sheet.
        /// </summary>
        public Spreadsheet()
        {
            sheet = new Dictionary<string, cell>();
  
            depends = new DependencyGraph();
        }

        /// <summary>
        /// Passes in the name of a cell and returns the contents.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The contents of the cell</returns>
        public override object GetCellContents(string name)
        {

            if (name == null || !isValidVariable(name)) //Checks if it is a proper input
            {
                throw new InvalidNameException("String cannot be null or an invalid.");
            }

            cell sheetcell;

            if (sheet.TryGetValue(name, out sheetcell)) //Checks if it exists.
            {
                return sheetcell.contents;
            }

            return string.Empty; //If the cell doesn't exist, return an empty string.
        }

        /// <summary>
        /// Returns a list of all non empty cells.
        /// </summary>
        /// <param name></param>
        /// <returns>List of all non empty cells.</returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> cellList = new HashSet<string>();
            foreach (string name in sheet.Keys)
            {
                if (!(sheet[name].contents.ToString() == ""))
                {
                    cellList.Add(name);
                }
            }
            return cellList;
        }

        /// <summary>
        /// This method takes in the input of a string and formula.
        /// It determines if the formula and name are proper inputs and then adds
        /// them to the list of dependees as well as the dictionary.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A list of the dependees and the dependent.</returns>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            HashSet<string> cellNDependees = new HashSet<string>(); //HashSet for storing dependees.
            cellNDependees.Add(name);

            if (formula == null)
            {
                throw new ArgumentNullException("The formula parameter is null.");
            }
            else if (name == null || !isValidVariable(name))
            {
                throw new InvalidNameException("Name cannot be null or an invalid input.");
            }

            else
            {
                foreach (string ncell in formula.GetVariables())
                {
                    depends.AddDependency(name, ncell);
                }

                foreach (string ncell in GetCellsToRecalculate(name)) //Throws a circular exception if the cell has a circular dependency
                {
                    cellNDependees.Add(ncell);
                }

                if (sheet.ContainsKey(name)) //Checks if the cell has been created
                {
                    sheet[name].contents = formula; //If the cell exists, makes the contents equal to the input.
                }
                else //Otherwise, create a new cell and add it into the dictionary with the title of name.
                {
                    cell ncell = new cell(formula);
                    sheet.Add(name, ncell);
                }

                return cellNDependees; //Return cell and dependees.
            }

        }

        /// This method takes in the input of a string and text.
        /// It determines if the text and name are proper inputs and then adds
        /// them to the list of dependees as well as the dictionary.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A list of the dependees and the dependent.</returns>
        public override ISet<string> SetCellContents(string name, string text)
        {
            HashSet<string> cellNDependees = new HashSet<string>(); //HashSet for storing dependees.
            cellNDependees.Add(name);
            if (text == null)
            {
                throw new ArgumentNullException("The formula parameter is null.");
            }
            else if (name == null || !isValidVariable(name))
            {
                throw new InvalidNameException("Name cannot be null or an invalid input.");
            }

            else
            {
                foreach (string ncell in GetCellsToRecalculate(name)) //Throws a circular exception if the cell has a circular dependency
                {
                    cellNDependees.Add(ncell);
                }

                if (sheet.ContainsKey(name)) //Checks if the cell has been created
                {
                    sheet[name].contents = text; //If the cell exists, makes the contents equal to the input.
                }
                else //Otherwise, create a new cell and add it into the dictionary with the title of name.
                {
                    cell ncell = new cell(text);
                    sheet.Add(name, ncell);
                }

               // foreach (string dependent in )

                return cellNDependees; //Return cell and dependees.
            }


        }

        /// This method takes in the input of a string and double.
        /// It determines if the double and name are proper inputs and then adds
        /// them to the list of dependees as well as the dictionary.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A list of the dependees and the dependent.</returns>
        public override ISet<string> SetCellContents(string name, double number)
        {

            HashSet<string> cellNDependees = new HashSet<string>(); //HashSet for storing dependees.
            cellNDependees.Add(name);
            if (name == null || !isValidVariable(name))
            {
                throw new InvalidNameException("Name cannot be null or an invalid input.");
            }

            else
            {
                foreach (string ncell in GetCellsToRecalculate(name)) //Throws a circular exception if the cell has a circular dependency
                {
                    cellNDependees.Add(ncell);
                }

                if (sheet.ContainsKey(name)) //Checks if the cell has been created
                {
                    sheet[name].contents = number; //If the cell exists, makes the contents equal to the input.
                }
                else //Otherwise, create a new cell and add it into the dictionary with the title of name.
                {
                    cell ncell = new cell(number);
                    sheet.Add(name, ncell);
                }

                return cellNDependees; //Return cell and dependees.
            }

        }

        /// <summary>
        /// Returns a list of all the dependees. This method acts as a helper method to
        /// visit and GetCellstoRecalculate.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("The formula parameter is null.");
            }
            else if (!isValidVariable(name))
            {
                throw new InvalidNameException("String cannot be an invalid input.");
            }

            return depends.GetDependees(name);
        }

        /// <summary>
        /// A helper method used to determine if a string is valid.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Returns true or false.</returns>
        private bool isValid(string name)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("^({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})$",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            return Regex.IsMatch(name, pattern, RegexOptions.IgnorePatternWhitespace);
        }

        /// <summary>
        /// A helper method used to determine if a string is valid.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Returns true or false.</returns>
        private bool isValidVariable(string name)
        {
            // Patterns for individual tokens
            String varPattern = @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*$";

            return Regex.IsMatch(name, varPattern, RegexOptions.IgnorePatternWhitespace);
        }

        /// <summary>
        /// A private class that is used as a parameter for the sheet dictionary.
        /// It has two characteristics which are contents and value.
        /// </summary>
        private class cell
        {
            public object contents { get; set;}

            public object value { get; set;}

            public cell(object cellContents)
            {
                contents = cellContents;
            }
        }
    }
}
