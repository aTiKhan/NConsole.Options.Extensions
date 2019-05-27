using System.Collections.Generic;
using System.Linq;

namespace NConsole.Options
{
    using Xunit;
    using Xunit.Abstractions;
    using static Constants;
    using static Domain;

    /// <summary>
    /// Variable tests.
    /// </summary>
    public class VariableTests : TestFixtureBase
    {
        public VariableTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// Should get simple Variables.
        /// </summary>
        [Fact]
        public void Should_Get_Simple_Variables()
        {
            var options = new OptionSet();

            // TODO: TBD: consider refactoring to more of an arrange/act/assert pattern, helper methods, etc.
            var name = options.AddVariable<string>(en);
            var age = options.AddVariable<int>(ay);

            /* TODO: The splitskies are clever, but are also error prone. We're assuming
             * by this that the string is as it appears at the command line, when this is
             * not the case. This is as the text appears after the command line parser is
             * through presenting the args to the application. */
            var args = $"{Dash}{en} {FindThisString} {Dash}{ay}{Colon}{D}".SplitArgumentMashUp();

            options.Parse(args);

            // TODO: TBD: may also verify Variable<>.Equals(Variable<>)...
            age.Value.AssertEqual(D);
            name.Value.AssertEqual(FindThisString);
        }

        /// <summary>
        /// Should detect switches.
        /// </summary>
        [Fact]
        public void Should_Detect_Switches()
        {
            var options = new OptionSet();

            var n = options.AddSwitch(en);
            var a = options.AddSwitch(ay);
            var b = options.AddSwitch(bee);

            var args = $"{Dash}{en} {Dash}{ay}".SplitArgumentMashUp();

            options.Parse(args);

            void Verify(bool actual, bool expected)
            {
                actual.AssertEqual(expected);
            }

            Verify(n, true);
            Verify(n.Enabled, true);

            Verify(a, true);
            Verify(a.Enabled, true);

            Verify(b, false);
            Verify(b.Enabled, false);
        }

        /// <summary>
        /// Should not throw exception when variable is set multiple times.
        /// </summary>
        [Fact]
        public void Should_Not_Throw_Exception_Multiset_Variable()
        {
            var options = new OptionSet();

            // ReSharper disable UnusedVariable
            var n = options.AddVariable<string>(en);
            // ReSharper restore UnusedVariable

            //TODO: Screaming for an NUnit-test-case-coverage.
            var args = ($"{Dash}{en}{Colon}{Noah}"
                        + $" {Dash}{en}{Colon}{Moses}"
                        + $" {Dash}{en}{Colon}{David}").SplitArgumentMashUp();

            // Which, to XUnit, "not throwing" is simply allowing the "normal" execution path to resolve itself.
            options.Parse(args);
        }

        /// <summary>
        /// Should process VariableLists.
        /// </summary>
        [Fact]
        public void Should_Process_VariableLists()
        {
            var optionSet = new OptionSet();

            var n = optionSet.AddVariableList<string>(en);
            var a = optionSet.AddVariableList<int>(ay);

            //TODO: Screaming for an NUnit-test-case-coverage.
            var args = ($"{Dash}{en} {FindThisString}"
                        + $" {Dash}{en}{Colon}{Findit2} {Dash}{en}{Colon}{Findit3}"
                        + $" {Dash}{ay}{A} {Dash}{ay}{B} {Dash}{ay}{C} {Dash}{ay}{Colon}{D}").SplitArgumentMashUp();

            optionSet.Parse(args);

            // ReSharper disable PossibleMultipleEnumeration
            void VerifyA(IEnumerable<int> x)
            {
                x.AssertContainsAll(D).Count().AssertEqual(4);
            }
            // ReSharper restore PossibleMultipleEnumeration

            // ReSharper disable PossibleMultipleEnumeration
            void VerifyN(IEnumerable<string> x)
            {
                x.AssertContainsAll(FindThisString).Count().AssertEqual(3);
            }
            // ReSharper restore PossibleMultipleEnumeration

            VerifyA(a);
            VerifyA(a.Values);

            VerifyN(n);
            VerifyN(n.Values);
        }

        /// <summary>
        /// Should process VariableMatrices.
        /// </summary>
        [Fact]
        public void Should_Process_Matrices()
        {
            var options = new OptionSet();

            var n = options.AddVariableMatrix<string>(en.MaySpecify());

            // ReSharper disable CommentTypo
            /* Specify the args as an array instead of the splitskies, in particular
             * on account of the Message= use case. Actually, at this level, quotes
             * should not enter into the mix, because those are command-line beasties. */
            // ReSharper restore CommentTypo

            // Remember, the Default Pairwise Separator is the Comma (',').
            var args = GetRange(
                $"{Dash}{en}{Colon}{Hello}{Comma}{World}"
                , $"{Dash}{en}{Color}{Equal}{Red}"
                , $"{Dash}{en}{Colon}{Message}{Comma}{Hello} {With} {Spaces}"
                , $"{Dash}{en}{Colon}{Name}{Comma}{Yeshua}"
                , $"{Dash}{en}{FavNHL}{Colon}{NewJerseyDevils}"
            );

            options.Parse(args.Select(x => $"{Dash}{x.Trim()}").ToArray());

            // This runs dangerously close to testing the Options themselves.
            void Verify(IReadOnlyDictionary<string, string> x)
            {
                x.AssertContainsKeys(Name, Hello, Message)
                    .AssertDoesNotContainsKeys(Color, FavNHL)
                    .AssertContains(Name, Yeshua)
                    .AssertContains(Hello, World);
            }

            // Should not be the Same instance, technically.
            n.Matrix.AssertNotSame(n);

            Verify(n);
            Verify(n.Matrix);
        }
    }
}
