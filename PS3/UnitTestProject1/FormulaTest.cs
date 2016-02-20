//Johnny Le, u0748407 CS 3500.

using System;
using SpreadsheetUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace FormulaTest
{
    [TestClass]
    public class FormulaTest
    {
        /// <summary>
        ///Checks if constructor works.
        ///</summary>
        [TestMethod()]
        public void ConstructorTest1()
        {
            Formula form = new Formula("4+4");
        }

        /// <summary>
        ///Checks for empty exception.
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof (FormulaFormatException))]
        public void ConstructorTest2()
        {
            Formula form = new Formula("");
        }

        /// <summary>
        ///Checks for exception on incorrect initial tokens.
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest3()
        {
            Formula form = new Formula("+90+5");
        }

        /// <summary>
        ///Checks for exception on incorrect final tokens.
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest4()
        {
            Formula form = new Formula("90+5+");
        }

        /// <summary>
        ///Checks for exception on incorrect token following the open parenthesis.
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest5()
        {
            Formula form = new Formula("()90+5+");
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest6()
        {
            Formula form = new Formula("(90)+5)+");
        }

        /// <summary>
        ///Checks for the invalid token.
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest7()
        {
            Formula form = new Formula("(90)+5&+6");
        }

        /// <summary>
        ///Checks for exception on incorrect token following the closing parenthesis.
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest8()
        {
            Formula form = new Formula("((6)*(*+4");
        }

        /// <summary>
        ///Checks for exception on incorrect token following the closing parenthesis.
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest9()
        {
            Formula form = new Formula("((6)6+(7+4))");
        }

        /// <summary>
        ///Checks for exception when closing parentheses are more numerous than opening parentheses.
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest10()
        {
            Formula form = new Formula("((6))+(7+4))");
        }

        /// <summary>
        ///Checks if the evaluate method computes correctly.
        ///</summary>
        [TestMethod()]
        public void EvaluatorTest1()
        {
            Assert.AreEqual(3.0, new Formula("1+1+1").Evaluate(s => 0));
        }

        /// <summary>
        ///Checks if the evaluate method computes correctly.
        ///</summary>
        [TestMethod()]
        public void EvaluatorTest2()
        {
            Assert.AreEqual(30.0, new Formula("5*4+2*5").Evaluate(s => 0));
        }

        /// <summary>
        ///Checks if the evaluate method computes correctly.
        ///</summary>
        [TestMethod()]
        public void EvaluatorTest3()
        {
            Assert.AreEqual(1.0, new Formula("X1 + X1 + 1").Evaluate(s => 0));
        }

        /// <summary>
        ///Tests the toString method.
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            Formula form = new Formula("1 + 3 + 4 * 5 + 7");
            List<string> formList = new List<string>();
            formList.Add("1");
            formList.Add("+");
            formList.Add("3");
            formList.Add("+");
            formList.Add("4");
            formList.Add("*");
            formList.Add("5");
            formList.Add("+");
            formList.Add("7");
            Assert.AreEqual("1+3+4*5+7", form.ToString());
        }

        /// <summary>
        ///Tests the hash code method.
        ///</summary>
        [TestMethod()]
        public void GeneralTest1()
        {
            Formula form = new Formula("1+3+4*5+7");
            string test = "1+3+4*5+7";

            Assert.AreEqual(test.GetHashCode(), form.GetHashCode());
        }

        /// <summary>
        ///Tests the .equals method.
        ///</summary>
        [TestMethod()]
        public void GeneralTest2()
        {
            Formula form = new Formula("1+3+4*5+7");
            List<string> formList = new List<string>();
            formList.Add("1");
            formList.Add("+");
            formList.Add("3");
            formList.Add("+");
            formList.Add("4");
            formList.Add("*");
            formList.Add("5");
            formList.Add("+");
            formList.Add("7");
            Assert.IsTrue(formList.ToString().Equals(form.ToString()));
        }

        /// <summary>
        ///Tests the == method.
        ///</summary>
        [TestMethod()]
        public void GeneralTest3()
        {
            Formula form = new Formula("1+3+4*5+7");
            Formula form2 = new Formula("1+3+4*5+7");
            Assert.IsTrue(form == form2);
        }

        /// <summary>
        ///Tests the != method.
        ///</summary>
        [TestMethod()]
        public void GeneralTest4()
        {
            Formula form = new Formula("1+3+4*5+7");
            Formula form2 = new Formula("7+5*4+3+1");
            Assert.IsFalse(form != form2);
        }

        /// <summary>
        ///Tests for the exception when the .equals object is null.
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void GeneralTest5()
        {
            List<string> formList = new List<string>();
            formList.Add("1");
            formList.Add("+");
            formList.Add("3");
            formList.Add("+");
            formList.Add("4");
            formList.Add("*");
            formList.Add("5");
            formList.Add("+");
            formList.Add("7");
            Assert.IsFalse(formList.ToString().Equals(null));
        }

        /// <summary>
        ///Tests for the exception when the .equals object is null.
        ///</summary>
        [TestMethod()]
        public void GeneralTest6()
        {
            Formula form = new Formula("1+3+4*5+7");
            List<string> formList = new List<string>();
            formList.Add("1");
            formList.Add("+");
            formList.Add("3");
            formList.Add("+");
            formList.Add("4");
            formList.Add("*");
            formList.Add("5");
            formList.Add("+");
            formList.Add("7");
            Assert.IsTrue(formList.Equals(form.GetVariables()));
        }
    }
}
