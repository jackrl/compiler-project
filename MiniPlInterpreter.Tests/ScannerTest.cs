using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniPlInterpreter;

namespace MiniPlInterpreter.Tests
{
    [TestClass]
    public class ScannerTest
    {
        [TestMethod]
        public void BasicTest()
        {
            var source = @" // this is a comment
                            ((..)) // grouping stuff
                            /******
                                            ****/
                            !*+-/< // operators";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();
            Assert.IsTrue(tokens.Count == 12);
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
    }
}
