using System.IO;

namespace NConsole.Options
{
    using Xunit;
    using Xunit.Abstractions;
    using static Domain;
    using static ConsoleManager;
    using static Constants;

    /// <summary>
    /// ConsoleManager tests.
    /// </summary>
    public class ConsoleManagerTests : TestFixtureBase
    {
        public ConsoleManagerTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// Should respond to required-but-not-present-options with show-help.
        /// </summary>
        [Fact]
        public void Should_Respond_To_Missing_Required_Variables_With_Show_Help()
        {
            var options = new OptionSet();

            // Populate the Options with some nominal options.

            // ReSharper disable UnusedVariable
            var name = options.AddVariable<string>(en.MustSpecify());
            var age = options.AddVariable<int>(ay.MustSpecify());
            var age2 = options.AddVariable<int>(bee.MustSpecify());
            var age3 = options.AddVariable<int>(cee.MustSpecify());
            // ReSharper restore UnusedVariable

            using (var writer = new StringWriter())
            {
                using (var consoleManager = new OptionSetConsoleManager(Tests, writer, options))
                {
                    consoleManager.TryParseOrShowHelp().AssertFalse();
                    writer.ToString().AssertContains($"{Tests}: error parsing arguments:");
                }
            }
        }

        /// <summary>
        /// Should respond to required-but-not-present-options with show-help.
        /// </summary>
        [Fact]
        public void Should_Respond_To_Missing_Required_VariableLists_With_Show_Help()
        {
            var options = new OptionSet();

            // Populate the Options with some nominal options.

            // ReSharper disable UnusedVariable
            var n = options.AddVariableList<string>(en.MustSpecify());
            var a = options.AddVariableList<int>(ay.MustSpecify());
            var m = options.AddVariableList<string>(em.MustSpecify());
            // ReSharper restore UnusedVariable

            using (var writer = new StringWriter())
            {
                using (var consoleManager = new OptionSetConsoleManager(Tests, writer, options))
                {
                    consoleManager.TryParseOrShowHelp().AssertFalse();
                    writer.ToString().AssertContains($"{Tests}: error parsing arguments:");
                }
            }
        }

        /// <summary>
        /// Should respond to help-mode.
        /// </summary>
        [Fact]
        public void Should_Respond_To_Help_Arg()
        {
            var options = new OptionSet();

            // Add a non-required variable because we want to verify the help-arg.

            // ReSharper disable once UnusedVariable
            var name = options.AddVariable<string>(en.MustSpecify(), TESTMODE);
            var requestHelp = $"{DoubleDash}{DefaultHelpPrototype[0]}";

            // TODO: TBD: may define canned internal constants...
            using (var writer = new StringWriter())
            {
                using (var consoleManager = new OptionSetConsoleManager(Tests, writer, options))
                {
                    consoleManager.TryParseOrShowHelp(requestHelp).AssertFalse();
                    writer.ToString().AssertContains(TESTMODE);
                }
            }
        }

        /// <summary>
        /// Should show-help for remaining-args.
        /// </summary>
        [Fact]
        public void Show_Show_Help_For_Remaining_Args()
        {
            var options = new OptionSet();

            // This one can be a required-variable, no problem, but that's it.

            // ReSharper disable once UnusedVariable
            var name = options.AddVariable<string>(en.MustSpecify());

            // Then we should have some remaining args.
            var args = $"{Dash}{en} {ThisIsName} {UnknownOptionCausesErrorShowHelp}".SplitArgumentMashUp();

            using (var writer = new StringWriter())
            {
                using (var consoleManager = new OptionSetConsoleManager(Tests, writer, options))
                {
                    consoleManager.TryParseOrShowHelp(args).AssertFalse();
                    writer.ToString().AssertContains($"{Tests}: error parsing arguments:");
                }
            }
        }
    }
}
