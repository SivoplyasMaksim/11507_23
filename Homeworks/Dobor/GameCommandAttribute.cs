using System;
using System.Collections.Generic;
using System.Text;

namespace Dobor
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GameCommandAttribute : Attribute
    {
        public string CommandName { get; }
        public string Description { get; }

        public GameCommandAttribute(string commandName, string description)
        {
            CommandName = commandName;
            Description = description;
        }
    }
}
