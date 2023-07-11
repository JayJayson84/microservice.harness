using System.Reflection;

namespace MicroService.Example.Mappers;

public static class AssemblyInfo
{
    public static Assembly GetAssembly() => typeof(AssemblyInfo).Assembly;
}