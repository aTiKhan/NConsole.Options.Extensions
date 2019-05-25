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
            var options = new RequiredValuesOptionSet();

            // Populate the Options with some nominal options.

            // ReSharper disable UnusedVariable
            var name = options.AddRequiredVariable<string>(en);
            var age = options.AddRequiredVariable<int>(ay);
            var age2 = options.AddRequiredVariable<int>(bee);
            var age3 = options.AddRequiredVariable<int>(cee);
            // ReSharper restore UnusedVariable

            using (var consoleManager = new ConsoleManager(Tests, options))
            {
                using (var writer = new StringWriter())
                {
                    consoleManager.TryParseOrShowHelp(writer).AssertFalse();
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
            var options = new RequiredValuesOptionSet();

            // Populate the Options with some nominal options.

            // ReSharper disable UnusedVariable
            var n = options.AddRequiredVariableList<string>(en);
            var a = options.AddRequiredVariableList<int>(ay);
            var m = options.AddRequiredVariableList<string>(em);
            // ReSharper restore UnusedVariable

            using (var consoleManager = new ConsoleManager(Tests, options))
            {
                using (var writer = new StringWriter())
                {
                    consoleManager.TryParseOrShowHelp(writer).AssertFalse();
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
            var options = new RequiredValuesOptionSet();

            // Add a non-required variable because we want to verify the help-arg.

            // ReSharper disable once UnusedVariable
            var name = options.AddVariable<string>(en);

            // TODO: TBD: may define canned internal constants...
            using (var consoleManager = new ConsoleManager(Tests, options, DefaultHelpPrototype, TESTMODE))
            {
                using (var writer = new StringWriter())
                {
                    consoleManager.TryParseOrShowHelp(writer, DefaultHelpPrototype).AssertFalse();
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
            var options = new RequiredValuesOptionSet();

            // This one can be a required-variable, no problem, but that's it.

            // ReSharper disable once UnusedVariable
            var name = options.AddRequiredVariable<string>(en);

            using (var consoleManager = new ConsoleManager(Tests, options))
            {
                // Then we should have some remaining args.
                var args = $"{Dash}{en} {ThisIsName} {UnknownOptionCausesErrorShowHelp}".Split(' ');

                using (var writer = new StringWriter())
                {
                    consoleManager.TryParseOrShowHelp(writer, args).AssertFalse();
                    writer.ToString().AssertContains($"{Tests}: error parsing arguments:");
                }
            }
        }
    }
}
