// Johnny Le, u0748407 CS 3500
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using SpreadsheetUtilities;
using SS;
using System.Collections.Generic;

namespace SpreadsheetTest
{
    [TestClass]
    public class SpreadsheetTest
    {
        /// <summary>
        /// Tests if the double setter works.
        /// </summary>
        [TestMethod]
        public void SetTest1()
        {
            spreadsheet sheet = new spreadsheet();
            sheet.SetCellContents("A1", 1.0);
            Assert.AreEqual(sheet.GetCellContents("A1"), 1.0);
        }

        /// <summary>
        /// Tests if the string setter works.
        /// </summary>
        [TestMethod]
        public void SetTest2()
        {
            spreadsheet sheet = new spreadsheet();
            sheet.SetCellContents("A1", "Sally sells seashells by the seashore.");
            Assert.AreEqual(sheet.GetCellContents("A1"), "Sally sells seashells by the seashore.");
        }

        /// <summary>
        /// Tests if the formula setter works.
        /// </summary>
        [TestMethod]
        public void SetTest3()
        {
            Formula form = new Formula("xy");
            spreadsheet sheet = new spreadsheet();
            sheet.SetCellContents("A1", form);
            Assert.AreEqual(sheet.GetCellContents("A1"), form);
        }

        /// <summary>
        /// Tests if the formula setter can overwrite cells.
        /// </summary>
        [TestMethod]
        public void SetTest4()
        {
            Formula form = new Formula("xy");
            Formula form2 = new Formula("x+y");
            Formula form3 = new Formula("x*y");
            spreadsheet sheet = new spreadsheet();
            sheet.SetCellContents("A1", form);
            sheet.SetCellContents("B1", form2);
            sheet.SetCellContents("A1", form3);

            Assert.AreEqual(sheet.GetCellContents("A1"), form3);
        }

        /// <summary>
        /// Tests if the string setter can overwrite cells.
        /// </summary>
        [TestMethod]
        public void SetTest5()
        {

            string word = "word";
            string dynamic = "Lots of words";
            string sentence = "Hey little lady.";

            spreadsheet sheet = new spreadsheet();
            sheet.SetCellContents("A1", word);
            sheet.SetCellContents("B1", dynamic);
            sheet.SetCellContents("A1", sentence);

            Assert.AreEqual(sheet.GetCellContents("A1"), sentence);
        }

        /// <summary>
        /// Tests if the double setter can overwrite cells.
        /// </summary>
        [TestMethod]
        public void SetTest6()
        {

            double num = 3.0;
            double num1 = 40.0;
            double num2 = 200.0;

            spreadsheet sheet = new spreadsheet();
            sheet.SetCellContents("A1", num);
            sheet.SetCellContents("B1", num1);
            sheet.SetCellContents("A1", num2);

            Assert.AreEqual(sheet.GetCellContents("A1"), num2);
        }

        /// <summary>
        /// Tests if the ArgumentNullException appears for the string setter.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetTest7()
        {
            string blank = null;
            spreadsheet sheet = new spreadsheet();
            sheet.SetCellContents("A1", blank);
        }

        /// <summary>
        /// Tests if the ArgumentNullException appears for the formula setter.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetTest8()
        {
            Formula blank = new Formula(null);
            spreadsheet sheet = new spreadsheet();
            sheet.SetCellContents("A1", blank);
        }

        /// <summary>
        /// Tests if the ArgumentNullException appears for the double setter.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetTest9()
        {
            string name = null;
            spreadsheet sheet = new spreadsheet();
            sheet.SetCellContents(name, 3.0);
        }

        /// <summary>
        /// Tests if the InvalidNameException appears for the string setter.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetTest10()
        {
            string name = null;
            spreadsheet sheet = new spreadsheet();
            sheet.SetCellContents(name, "What's up?");
        }

        /// <summary>
        /// Tests if the InvalidNameException appears for the formula setter.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetTest11()
        {
            Formula form = new Formula("x+y");
            string name = null;
            spreadsheet sheet = new spreadsheet();
            sheet.SetCellContents(name, form);
        }

        /// <summary>
        /// Tests if the numerable outputs the same list.
        /// </summary>
        [TestMethod]
        public void NumerableTest()
        {

            List<string> numerable = new List<string>();

            double num = 3.0;
            double num1 = 40.0;
            double num2 = 200.0;

            AbstractSpreadsheet sheet = new spreadsheet();
            sheet.SetCellContents("A1", num);
            sheet.SetCellContents("A2", num1);
            sheet.SetCellContents("A3", num2);
            sheet.SetCellContents("B1", num);
            sheet.SetCellContents("B2", num1);
            sheet.SetCellContents("B4", "");
            sheet.SetCellContents("B3", num);
            sheet.SetCellContents("B5", num1);
            sheet.SetCellContents("B6", num2);

            numerable.Add("B6");
            numerable.Add("B5");
            numerable.Add("B3");
            numerable.Add("B2");
            numerable.Add("B1");

            numerable.Add("A3");
            numerable.Add("A2");
            numerable.Add("A1");


            Assert.AreEqual(sheet.GetNamesOfAllNonemptyCells(), numerable);
        }

        /// <summary>
        /// Tests if the InvalidNameException appears for the GetCellContents method for a null input.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetTest1()
        {
            string name = null;
            spreadsheet sheet = new spreadsheet();
            sheet.GetCellContents(name);
        }

        /// <summary>
        /// Tests if the InvalidNameException appears for the GetCellContents method for an invalid input..
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetTest2()
        {
            string name = "!!!";
            spreadsheet sheet = new spreadsheet();
            sheet.SetCellContents("A1", 3.0);
            sheet.GetCellContents(name);
        }

        /// <summary>
        /// Tests if an empty string will be returned for a non-existent cell.
        /// </summary>
        [TestMethod]
        public void GetTest3()
        {
            spreadsheet sheet = new spreadsheet();
            Assert.AreEqual(sheet.GetCellContents("A1"), string.Empty);
        }

    }
}
