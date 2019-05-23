using System;
using System.Collections.Generic;
using System.Linq;

namespace NConsole.Options
{
    using Xunit;
    using Xunit.Abstractions;

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
            var name = options.AddVariable<string>("n");
            var age = options.AddVariable<int>("a");

            /* TODO: The splitskies are clever, but are also error prone. We're assuming
             * by this that the string is as it appears at the command line, when this is
             * not the case. This is as the text appears after the command line parser is
             * through presenting the args to the application. */
            var args = "-n FindThisString -a:23".Split(' ');
            options.Parse(args);

            // TODO: TBD: may also verify Variable<>.Equals(Variable<>)...
            age.Value.AssertEqual(23);
            name.Value.AssertEqual("FindThisString");
        }

        /// <summary>
        /// Should detect required variables.
        /// </summary>
        [Fact]
        public void Should_Detect_Required_Variables()
        {
            var options = new RequiredValuesOptionSet();

            var name = options.AddRequiredVariable<string>("n");

            // ReSharper disable UnusedVariable
            var age = options.AddRequiredVariable<int>("a");
            var age2 = options.AddRequiredVariable<int>("b");
            var age3 = options.AddRequiredVariable<int>("c");
            // ReSharper restore UnusedVariable

            //TODO: Screaming for NUnit-test-case-coverage.
            var args = "-n FindThisString".Split(' ');
            options.Parse(args);

            /* TODO: Might could (should) also verify that each of the missing ones,
             * as well as found ones, are either there are not there. */
            options.GetMissingVariables().Count().AssertEqual(3);
            name.Value.AssertEqual("FindThisString");
        }

        /// <summary>
        /// Should detect required variable lists.
        /// </summary>
        [Fact]
        public void Should_Detect_Required_VariableLists()
        {
            var options = new RequiredValuesOptionSet();

            var n = options.AddRequiredVariableList<string>("n");
            var a = options.AddRequiredVariableList<int>("a");
            var m = options.AddRequiredVariableList<string>("m");

            //TODO: Screaming for an NUnit-test-case-coverage.
            var args = "-n FindThisString -n:Findit2 -n:Findit3 -a2 -a3 -a5565 -a:23".Split(' ');

            options.Parse(args);

            // TODO: TBD: these verification methods should be refactored...
            // TODO: TBD: as should the constants informing the tests themselves...
            // TODO: TBD: especially if we are going to agree that the scenarios herein attribute a certain vocabulary for consistency sake.
            // ReSharper disable PossibleMultipleEnumeration
            void VerifyN(IEnumerable<string> x)
            {
                x.AssertContainsAll("FindThisString", "Findit2", "Findit3").Count().AssertEqual(3);
            }
            // ReSharper restore PossibleMultipleEnumeration

            // ReSharper disable PossibleMultipleEnumeration
            void VerifyA(IEnumerable<int> x)
            {
                x.AssertContainsAll(2, 3, 5565, 23).Count().AssertEqual(4);
            }
            // ReSharper restore PossibleMultipleEnumeration

            // ReSharper disable PossibleMultipleEnumeration
            void VerifyM(IEnumerable<string> x)
            {
                x.Count().AssertEqual(0);
                options.GetMissingVariables().Count().AssertEqual(1);
            }
            // ReSharper restore PossibleMultipleEnumeration

            VerifyA(a);
            VerifyA(a.Values);

            VerifyN(n);
            VerifyN(n.Values);

            VerifyM(m);
            VerifyM(m.Values);
        }

        /// <summary>
        /// Should detect switches.
        /// </summary>
        [Fact]
        public void Should_Detect_Switches()
        {
            var options = new OptionSet();

            var n = options.AddSwitch("n");
            var a = options.AddSwitch("a");
            var b = options.AddSwitch("b");

            var args = "-n -a".Split(' ');

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
            var optionSet = new OptionSet();

            // ReSharper disable UnusedVariable
            var n = optionSet.AddVariable<string>("n");
            // ReSharper restore UnusedVariable

            //TODO: Screaming for an NUnit-test-case-coverage.
            var args = "-n:Noah -n:Moses -n:David".Split(' ');

            // Which, to XUnit, "not throwing" is simply allowing the "normal" execution path to resolve itself.
            optionSet.Parse(args);
        }

        /// <summary>
        /// Should process VariableLists.
        /// </summary>
        [Fact]
        public void Should_Process_VariableLists()
        {
            var optionSet = new OptionSet();

            var n = optionSet.AddVariableList<string>("n");
            var a = optionSet.AddVariableList<int>("a");

            //TODO: Screaming for an NUnit-test-case-coverage.
            var args = "-n FindThisString -n:Findit2 -n:Findi3 -a2 -a3 -a5565 -a:23".Split(' ');

            optionSet.Parse(args);

            // ReSharper disable PossibleMultipleEnumeration
            void VerifyA(IEnumerable<int> x)
            {
                x.AssertContainsAll(23).Count().AssertEqual(4);
            }
            // ReSharper restore PossibleMultipleEnumeration

            // ReSharper disable PossibleMultipleEnumeration
            void VerifyN(IEnumerable<string> x)
            {
                x.AssertContainsAll("FindThisString").Count().AssertEqual(3);
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
            var optionSet = new OptionSet();

            // TODO: TBD: does it make sense to expose "Separators" to would-be callers?
            // TODO: TBD: that is, apart from the prototype specification itself...
            var n = optionSet.AddVariableMatrix<string>("n");

            // ReSharper disable CommentTypo
            /* Specify the args as an array instead of the splitskies, in particular
             * on account of the Message= use case. Actually, at this level, quotes
             * should not enter into the mix, because those are command-line beasties. */
            // ReSharper restore CommentTypo

            // Remember, the Default Pairwise Separator is the Comma (',').
            var args = GetRange(
                "-n:Hello,World"
                , "-nColor=Red"
                , "-n:Message,Hello With Spaces"
                , "-n:Name,Jesus"
                , "-nFavNHL:NewJerseyDevils"
            );

            optionSet.Parse(args.Select(a => "-" + a.Trim()).ToArray());

            // This runs dangerously close to testing the Options themselves.
            void Verify(IDictionary<string, string> x)
            {
                x.AssertContainsKeys("Name", "Hello", "Message")
                    .AssertDoesNotContainsKeys("Color", "FavNHL")
                    .AssertContains("Name", "Jesus")
                    .AssertContains("Hello", "World");
            }

            Verify(n);
            Verify(n.Matrix);
        }
    }
}
