using System.IO;

namespace NConsole.Options
{
    using Xunit;
    using Xunit.Abstractions;
 
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
            var optionSet = new RequiredValuesOptionSet();

            // Populate the OptionSet with some nominal options.

            // ReSharper disable UnusedVariable
            var name = optionSet.AddRequiredVariable<string>("n");
            var age = optionSet.AddRequiredVariable<int>("a");
            var age2 = optionSet.AddRequiredVariable<int>("b");
            var age3 = optionSet.AddRequiredVariable<int>("c");
            // ReSharper restore UnusedVariable

            const string ConsoleName = "Test";

            using (var consoleManager = new ConsoleManager(ConsoleName, optionSet))
            {
                using (var writer = new StringWriter())
                {
                    consoleManager.TryParseOrShowHelp(writer).AssertFalse();
                    writer.ToString().AssertContains(ConsoleName + ": error parsing arguments:");
                }
            }
        }

        /// <summary>
        /// Should respond to required-but-not-present-options with show-help.
        /// </summary>
        [Fact]
        public void Should_Respond_To_Missing_Required_VariableLists_With_Show_Help()
        {
            var optionSet = new RequiredValuesOptionSet();

            // Populate the OptionSet with some nominal options.

            // ReSharper disable UnusedVariable
            var n = optionSet.AddRequiredVariableList<string>("n");
            var a = optionSet.AddRequiredVariableList<int>("a");
            var m = optionSet.AddRequiredVariableList<string>("m");
            // ReSharper restore UnusedVariable

            const string ConsoleName = "Test";

            using (var consoleManager = new ConsoleManager(ConsoleName, optionSet))
            {
                using (var writer = new StringWriter())
                {
                    consoleManager.TryParseOrShowHelp(writer).AssertFalse();
                    writer.ToString().AssertContains(ConsoleName + ": error parsing arguments:");
                }
            }
        }

        /// <summary>
        /// Should respond to help-mode.
        /// </summary>
        [Fact]
        public void Should_Respond_To_Help_Arg()
        {
            var optionSet = new RequiredValuesOptionSet();

            // Add a non-required variable because we want to verify the help-arg.

            // ReSharper disable once UnusedVariable
            var name = optionSet.AddVariable<string>("n");

            // TODO: TBD: may define canned internal constants...
            const string HelpPrototype = "h|help";
            const string Description = "TESTMODE";

            using (var consoleManager = new ConsoleManager("Test", optionSet, HelpPrototype, Description))
            {
                using (var writer = new StringWriter())
                {
                    consoleManager.TryParseOrShowHelp(writer, HelpPrototype).AssertFalse();
                    writer.ToString().AssertContains(Description);
                }
            }
        }

        /// <summary>
        /// Should show-help for remaining-args.
        /// </summary>
        [Fact]
        public void Show_Show_Help_For_Remaining_Args()
        {
            var optionSet = new RequiredValuesOptionSet();

            // This one can be a required-variable, no problem, but that's it.

            // ReSharper disable once UnusedVariable
            var name = optionSet.AddRequiredVariable<string>("n");

            const string ConsoleName = "Test";

            using (var consoleManager = new ConsoleManager(ConsoleName, optionSet))
            {
                // Then we should have some remaining args.
                var args = "-n ThisIsName UnknownOptionCausesErrorShowHelp".Split(' ');

                using (var writer = new StringWriter())
                {
                    consoleManager.TryParseOrShowHelp(writer, args).AssertFalse();
                    writer.ToString().AssertContains(ConsoleName + ": error parsing arguments:");
                }
            }
        }
    }
}
