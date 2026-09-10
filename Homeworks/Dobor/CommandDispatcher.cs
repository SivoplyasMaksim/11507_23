using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Dobor
{
    public class CommandDispatcher
    {
        private readonly Dictionary<string, (MethodInfo Method, object Instance)> _commands = new();

        public CommandDispatcher(object target)
        {
            var methods = target.GetType().GetMethods();
            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<GameCommandAttribute>();
                if (attr != null)
                {
                    _commands["/" + attr.CommandName] = (method, target);
                }
            }
        }

        public void PrintCommands()
        {
            foreach (var kvp in _commands)
            {
                var desc = kvp.Value.Method.GetCustomAttribute<GameCommandAttribute>()!.Description;
                Console.WriteLine($"   - Зарегистрирована команда: {kvp.Key} — {desc}");
            }
        }

        public void Execute(string commandName)
        {
            if (_commands.TryGetValue(commandName, out var cmd))
            {
                cmd.Method.Invoke(cmd.Instance, null);
            }
        }
    }
}
