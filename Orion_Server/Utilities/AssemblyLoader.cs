#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace OrionServer.Utilities
{
    public class AssemblyLoader
    {
        public static string[] GetAssemblies()
        {
            List<string> assemblies = new();
            string[] directories = Directory.GetDirectories(Constants.ModulesFolder);

            foreach (string directory in directories)
            {
                if (AssemblyExists(directory))
                    assemblies.Add(directory);
            }

            return assemblies.ToArray();
        }

        private static bool AssemblyExists(string name)
        {
            return File.Exists($"{Constants.ModulesFolder}/{name}/{name}.dll");
        }

        private void LoadAssembly()
        {
            _assembly = Assembly.LoadFile($"{Constants.ModulesFolder}/{_moduleName}/{_moduleName}.dll");
            _mainType = _assembly.GetType(_moduleName, true, false);
            _handle = Activator.CreateInstance(_mainType);
        }

        private string _moduleName;
        private Assembly _assembly;
        private Type _mainType;
        private object? _handle;
        private Dictionary<string, MethodInfo> _methods = new();
        public AssemblyLoader(string name)
        {
            if (!AssemblyExists(name))
                throw new Exception($"Assembly \"{name}\" does not exist. Make sure it is placed in \"{Constants.ModulesFolder}\" forlder");

            _moduleName = name;
        }

        public void Run(string name, object[] arguments)
        {
            if (_handle == null)
                throw new Exception($"Assembly \"{_moduleName}\" did not load properly");
            
            MethodInfo func = null;
            
            try
            {
                func = _methods[name];
            }
            catch
            {
                func = _mainType.GetMethod(name);
                _methods.Add(name, func);
            }
        }
    }
}