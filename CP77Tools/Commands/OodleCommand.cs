using System.CommandLine;
using System.CommandLine.Invocation;

namespace CP77Tools.Commands
{
    public class OodleCommand : Command
    {
        private static string Name = "oodle";
        private static string Description = "Some helper functions related to oodle compression.";

        public OodleCommand() : base(Name, Description)
        {
            AddOption(new Option<string>(new[] {"--path", "-p"}, ""));
            AddOption(new Option<bool>(new[] {"--decompress", "-d"}, ""));
            
            Handler = CommandHandler.Create<string, bool>(ConsoleFunctions.OodleTask);
        }
    }
}