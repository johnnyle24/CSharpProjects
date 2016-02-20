//Johnny Le, u0748407 CS 3500.
//Original Skeleton by Professor Zachary

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
            int left_pares = 0; //Counts the number of left parentheses
            int right_pares = 0; //Counts the number of right parentheses
            parsedFormula = new List<String>(GetTokens(formula)); //variable for storing the formula

            //For empty formulas
            if (parsedFormula.Count() == 0)
            {
                throw new FormulaFormatException("There are no tokens.");
            }

            //Makes sure that the first token is a variable, number, or left parenthesis.
            if (!checkTokenOpen(parsedFormula.First()))
            {
                throw new FormulaFormatException("The starting token is not a variable, number, or a left parenthesis.");
            }

            //Checks if the last token is a variable, number, or left parenthesis.
            if (!checkTokenClosed(parsedFormula.Last()))
            {
                throw new FormulaFormatException("The ending token is not a variable, number, or a right parenthesis.");
            }

            //Initializes previous token
            string prevToken = "";

            //Begins check for each individual token.
            foreach (string token in parsedFormula)
            {

                //Checks if the token is a valid token.
                if (!isValidToken(token))
                {
                    throw new FormulaFormatException("Invalid Token");
                }

                //Checks if the token following the left parenthesis is a variable, number, or left parenthesis.
                if (prevToken == "(" && (!checkTokenOpen(token)))
                {
                    throw new FormulaFormatException("The token following the open parenthesis is not a variable, number, or a left parenthesis.");
                }

                //Increments the count for left parentheses
                if (token == "(")
                {
                    left_pares++;
                }

                //Checks if the token following the right parenthesis is a variable, number, or left parenthesis.
                if (checkTokenClosed(prevToken) && (!Regex.IsMatch(token, @"^(([\+\-*/]|(\))))$")))
                {
                    throw new FormulaFormatException("The token following the closed parenthesis/number/variable is not an operator or closing parenthesis.");
                }

                //Increments the count for right parentheses and throws an exception if the number becomes higher than the left parentheses.
                if (token == ")")
                {
                    right_pares++;
                    if (right_pares > left_pares)
                    {
                        throw new FormulaFormatException("There are more right parentheses than left.");
                    }
                }

                //Stores the current token so that it may be referenced on the subsequent token's checks.
                prevToken = token;
            }

            if (left_pares != right_pares)
            {
                throw new FormulaFormatException("Parentheses are not balanced.");
            }
        }

        private List<String> parsedFormula { get; set; }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            int left_pares = 0; //Counts the number of left parentheses
            int right_pares = 0; //Counts the number of right parentheses
            parsedFormula = new List<String>(GetTokens(formula)); //variable for storing the formula

            //For empty formulas
            if (parsedFormula.Count() == 0)
            {
                throw new FormulaFormatException("There are no tokens.");
            }

            //Makes sure that the first token is a variable, number, or left parenthesis.
            if (!checkTokenOpen(parsedFormula.First()))
            {
                throw new FormulaFormatException("The starting token is not a variable, number, or a left parenthesis.");
            }

            //Checks if the last token is a variable, number, or left parenthesis.
            if (!checkTokenClosed(parsedFormula.Last()))
            {
                throw new FormulaFormatException("The ending token is not a variable, number, or a right parenthesis.");
            }

            //Initializes previous token
            string prevToken = "";

            //Begins check for each individual token.
            foreach (string token in parsedFormula)
            {

                //Checks if the token is a valid token.
                if (!isValidToken(token))
                {
                    throw new FormulaFormatException("Invalid Token");
                }

                //Checks if the token following the left parenthesis is a variable, number, or left parenthesis.
                if (prevToken == "(" && (!checkTokenOpen(token)))
                {
                    throw new FormulaFormatException("The token following the open parenthesis is not a variable, number, or a left parenthesis.");
                }

                //Increments the count for left parentheses
                if (token == "(")
                {
                    left_pares++;
                }

                //Checks if the token following the right parenthesis is a variable, number, or left parenthesis.
                if (checkTokenClosed(prevToken) && (!Regex.IsMatch(token, @"^(([\+\-*/]|(\))))$")))
                {
                    throw new FormulaFormatException("The token following the closed parenthesis/number/variable is not an operator or closing parenthesis.");
                }

                //Increments the count for right parentheses and throws an exception if the number becomes higher than the left parentheses.
                if (token == ")")
                {
                    right_pares++;
                    if (right_pares > left_pares)
                    {
                        throw new FormulaFormatException("There are more right parentheses than left.");
                    }
                }

                //Checks if the normalized token is legal.
                if (!isValidToken(normalize(token)))
                {
                    throw new FormulaFormatException("The normalized token is not a legal variable.");
                }

                //Checks if the the normalized token is valid according to the isValid input.
                if (!isValid(normalize(token)))
                {
                    throw new FormulaFormatException("The normalized token is not valid according to the function.");
                }

                //Checks for consecutive operators
                if ((Regex.IsMatch(token, @"^([\+\-*/])$")) && (Regex.IsMatch(prevToken, @"^([\+\-*/])$")))
                {
                    throw new FormulaFormatException("Consecutive operators are in the formula.");
                }

                //Stores the current token so that it may be referenced on the subsequent token's checks.
                prevToken = token;
            }

            if (left_pares != right_pares)
            {
                throw new FormulaFormatException("Parentheses are not balanced.");
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            return FormulaEvaluator.Evaluator.Evaluate(string.Join(string.Empty, parsedFormula), lookup);
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            HashSet<string> parsedFormulaNumerable = new HashSet<string>();
            foreach (string token in parsedFormula)
            {
                if (isValidVariables(token))
                {
                    parsedFormulaNumerable.Add(token);
                }
            }
            return parsedFormulaNumerable;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            string compiled = "";
            foreach (string token in parsedFormula)
            {
                compiled = compiled + token;
            }
            return compiled;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens, which are compared as doubles, and variable tokens,
        /// whose normalized forms are compared as strings.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            //Return false if the obj is null or it isn't a formula.
            if (Object.ReferenceEquals(obj, null) || !(obj is Formula))
            {
                return false;
            }

            //If the obj is a formula, test if it is equal.
            else if (obj is Formula)
            {
                //Transfers all of the formula from obj into a list.
                List<string> objList = new List<string>(GetTokens(obj.ToString()));
                for (int i = 0; i < parsedFormula.Count(); i++)
                {
                    //Turns the values into a double if possible and compares them.
                    double dub1, dub2;
                    if (Double.TryParse(parsedFormula.ElementAt(i), out dub1) && double.TryParse(objList.ElementAt(i), out dub2))
                    {
                        if (dub1 != dub2)
                        {
                            return false;
                        }
                    }

                    //Compares the strings.
                    else if (!(parsedFormula.ElementAt(i) == objList.ElementAt(i)))
                    {
                        return false;
                    }
                }
            }
            //If all the above tests were passed, return true.
            return true;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {

            if (ReferenceEquals(f1, null))
            {
                return ReferenceEquals(f2, null);
            }
            else
            {
                return f1.Equals(f2);
            }
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Checks if it is a valid token.
        /// </summary>
        private static bool isValidToken(string token)
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

            return Regex.IsMatch(token, pattern, RegexOptions.IgnorePatternWhitespace);
        }

        /// <summary>
        /// Checks if it is a valid variable.
        /// </summary>
        private static bool isValidVariables(string token)
        {
            String varPattern = @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*$";

            return Regex.IsMatch(token, varPattern, RegexOptions.IgnorePatternWhitespace);
        }

        /// <summary>
        /// Checks if it is variable, int, or open parenthesis.
        /// </summary>
        private static bool checkTokenOpen(string token)
        {
            return Regex.IsMatch(token, @"^([a-zA-Z_](?: [a-zA-Z_]|\d)*)|((?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?)|(\()$", RegexOptions.IgnorePatternWhitespace);
        }

        /// <summary>
        /// Checks if it is variable, int, or closing parenthesis.
        /// </summary>
        private static bool checkTokenClosed(string token)
        {
            return Regex.IsMatch(token, @"^([a-zA-Z_](?: [a-zA-Z_]|\d)*)|((?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?)|(\))$", RegexOptions.IgnorePatternWhitespace);
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }


    }
}

