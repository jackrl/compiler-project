using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniPlInterpreter;

namespace MiniPlInterpreter.Tests
{
    [TestClass]
    public class ScannerTest
    {
        [TestMethod]
        public void SampleProgram1Test()
        {
            var source = @"var X : int := 4 + (6 * 2);
                           print X;";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 17);
            Assert.IsTrue(scanner.Errors.Count == 0);
        }

        [TestMethod]
        public void SampleProgram2Test()
        {
            var source = @"var nTimes : int := 0;
                           print ""How many times ? "";
                           read nTimes;
                           var x : int;
                           for x in 0..nTimes - 1 do
                               print x;
                               print "" : Hello, World!\n"";
                           end for;
                           assert(x = nTimes); ";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 44);
            Assert.IsTrue(scanner.Errors.Count == 0);
        }

        [TestMethod]
        public void SampleProgram3Test()
        {
            var source = @"print ""Give a number"";
                           var n : int;
                           read n;
                           var v : int := 1;
                           var i : int;
                           for i in 1..n do
                               v:= v * i;

                           end for;
                           print ""The result is: "";
                           print v;";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 46);
            Assert.IsTrue(scanner.Errors.Count == 0);
        }

        [TestMethod]
        public void BlockCommentDoesntEndErrorTest()
        {
            var source = @"/*               
                           !*+-/< // operators";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();
            Assert.IsTrue(scanner.Errors.Count == 1);
        }

        [TestMethod]
        public void NotRecognizedCharacterErrorTest()
        {
            var source = @"(($))";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();
            Assert.IsTrue(scanner.Errors.Count == 1);
        }

        [TestMethod]
        public void SingleDotErrorTest()
        {
            var source = @"((.))";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();
            Assert.IsTrue(scanner.Errors.Count == 1);
        }

        [TestMethod]
        public void StringScanningTest()
        {
            var source = @"""Some String""";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens[0].Type == TokenType.STRING);
            Assert.IsTrue(tokens[0].Literal.Equals("Some String"));
            Assert.IsTrue(scanner.Errors.Count == 0);
        }

        [TestMethod]
        public void UnterminatedStringScanningErrorTest()
        {
            var source = @"""Some String";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();
            Assert.IsTrue(scanner.Errors.Count == 1);
        }

        [TestMethod]
        public void IntegerScanningTest()
        {
            var source = @"   123  ";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens[0].Type == TokenType.INTEGER);
            Assert.IsTrue(tokens[0].Literal.Equals(123));
            Assert.IsTrue(scanner.Errors.Count == 0);
        }

        [TestMethod]
        public void IdentifierScanningTest()
        {
            var source = @"abc_12asda";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens[0].Type == TokenType.IDENTIFIER);
            Assert.IsTrue(tokens[0].Lexeme.Equals("abc_12asda"));
            Assert.IsTrue(scanner.Errors.Count == 0);
        }

        [TestMethod]
        public void KeywordScanningTest()
        {
            var source = @"print";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens[0].Type == TokenType.PRINT);
            Assert.IsTrue(tokens[0].Lexeme.Equals("print"));
            Assert.IsTrue(scanner.Errors.Count == 0);
        }

        [TestMethod]
        public void StringWithEscapeCharactersTest()
        {
            var source = "\"jklaj\\ndlkas\\\"hjkadh\\\\kjash\"";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens[0].Type == TokenType.STRING);
            Assert.IsTrue(tokens[0].Literal.Equals("jklaj\ndlkas\"hjkadh\\kjash"));
            Assert.IsTrue(scanner.Errors.Count == 0);
        }

        [TestMethod]
        public void StringWithUnrecognizedEscapeCharacterErrorTest()
        {
            var source = "\"abc\\1def\"";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens[0].Type == TokenType.STRING);
            Assert.IsTrue(tokens[0].Literal.Equals("abc\\1def"));
            Assert.IsTrue(scanner.Errors.Count == 1);
        }
    }
}
