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

            int modulesFolderNameSize = Constants.ModulesFolder.Length;

            foreach (string directory in directories)
            {
                string name = directory.Substring(modulesFolderNameSize + 1);
                if (AssemblyExists(name))
                    assemblies.Add(name);
            }

            return assemblies.ToArray();
        }

        public static bool AssemblyRemove(string name)
        {
            try
            {
                Directory.Delete($"{Constants.ModulesFolder}/{name}", true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool AssemblyExists(string name)
        {
            return File.Exists($"{Constants.ModulesFolder}/{name}/{name}.dll");
        }

        private void LoadAssembly()
        {
            _assembly = Assembly.LoadFile($"{Constants.ModulesFolder}/{_moduleName}/{_moduleName}.dll");
            if (_assembly == null)
                throw new Exception($"Assembly \"{_moduleName}\" can not be loaded");
            _mainType = _assembly.GetType(_moduleName, true, false);
            if (_mainType == null)
                throw new Exception($"Assembly must have a class named as the module name");
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

        public string Run(string name, object[] arguments)
        {
            if (_handle == null || _mainType == null)
                throw new Exception($"Assembly \"{_moduleName}\" did not load properly");
            
            MethodInfo func;
            
            try
            {
                func = _methods[name];
            }
            catch
            {
                func = _mainType.GetMethod(name);
                if (func == null)
                    throw new Exception($"Assembly \"{_moduleName}\" did not include method \"{name}\"");
                _methods.Add(name, func);
            }

            return (string)func.Invoke(_handle, arguments);
        }

        ~AssemblyLoader()
        {
            _assembly = null;
        }
    }
}