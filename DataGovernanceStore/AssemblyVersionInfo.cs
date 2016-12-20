using System.Reflection;
using System.Runtime.CompilerServices; 
using System.Runtime.InteropServices;
// This is a global assemblyInfo file, which controls following attributes for all project's assemblies
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyCompany("Riversand Technologies")]
//[assembly: AssemblyProduct("Riversand MDM Center")]
#if DEBUG
[assembly: AssemblyProduct("MDMCenter [Debug]")]
#else
[assembly: AssemblyProduct("MDMCenter [Release]")]
#endif
[assembly: AssemblyCopyright("Copyright © Riversand Technologies 2016")]
[assembly: AssemblyTrademark("")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
// 
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("7.8.1100.10244")]
[assembly: AssemblyFileVersion("7.8.1100.10244")]