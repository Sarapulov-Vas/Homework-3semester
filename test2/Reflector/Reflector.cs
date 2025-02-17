// <copyright file="Reflector.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace Reflector;
using System.Reflection;
using System.Text;

/// <summary>
/// A class for print the structure of a class.
/// </summary>
public static class Reflector
{
    /// <summary>
    /// Method for printing class structure.
    /// </summary>
    /// <param name="someClass">Class.</param>
    public static void PrintStructure(Type someClass)
    {
        var file = File.Create($"../../../{someClass.Name}.cs");
        var writer = new StreamWriter(file);
        var structure = GetClassStructure(someClass);
        writer.WriteLine(structure);
        writer.Flush();
    }

    /// <summary>
    /// The class comparison method.
    /// </summary>
    /// <param name="firstClass">First class.</param>
    /// <param name="secondClass">Second class.</param>
    /// <returns>Class diff.</returns>
    public static string DiffClasses(Type firstClass, Type secondClass)
    {
        StringBuilder stringBuilder = new ();
        stringBuilder.Append(DiffMembers(firstClass.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static),
                    secondClass.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)));

        stringBuilder.Append(DiffMembers(firstClass.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static),
                    secondClass.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)));

        return stringBuilder.ToString();
    }

    private static string DiffMembers(MemberInfo[] firstMembers, MemberInfo[] secondMembers)
    {
        StringBuilder stringBuilder = new();
        var membersInFirst = firstMembers.Select(m => m.Name).ToHashSet();
        var membersInSecond = secondMembers.Select(m => m.Name).ToHashSet();

        var onlyInFirst = membersInFirst.Except(membersInSecond);
        var onlyInSecond = membersInSecond.Except(membersInFirst);

        foreach (var member in onlyInFirst)
        {
            stringBuilder.Append(member);
        }

        foreach (var member in onlyInSecond)
        {
            stringBuilder.Append(member);
        }

        return stringBuilder.ToString();
    }

    private static string GetClassStructure(Type someClass)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"{GetClassVisibility(someClass)} class {someClass.Name}\n{{\n");
        foreach (var field in someClass.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
        {
            stringBuilder.Append($"    {GetFieldVisibility(field)} {GetFieldStatic(field)} " +
                $"{field.FieldType.Name} {field.Name};\n");
        }

        foreach (var method in someClass.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
        {
            if (method.GetParameters().Length != 0)
            {
                stringBuilder.Append($"    {GetMethodVisibility(method)} {GetMethodStatic(method)} " +
                    $"{method.ReturnType.Name} {method.Name} (");
                foreach (var param in method.GetParameters())
                {
                    stringBuilder.Append($"{param.ParameterType.Name} {param.Name}, ");
                }

                stringBuilder.Remove(stringBuilder.Length - 2, 2);
                stringBuilder.Append(");\n");
            }
        }

        foreach (var nestedClass in someClass.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
        {
            Console.WriteLine(nestedClass.Name);
            stringBuilder.Append(GetClassStructure(nestedClass));
        }

        stringBuilder.Append("\n}");
        return stringBuilder.ToString();
    }

    private static string GetClassVisibility(Type type)
        => type.IsPublic ? "public" : "private";

    private static string GetFieldVisibility(FieldInfo type)
        => type.IsPublic ? "public" : type.IsFamily ? "protected" : type.IsAssembly ? "internal" : "private";

    private static string GetFieldStatic(FieldInfo type)
        => type.IsStatic ? "static" : string.Empty;

    private static string GetMethodVisibility(MethodInfo type)
        => type.IsPublic ? "public" : type.IsFamily ? "protected" : type.IsAssembly ? "internal" : "private";

    private static string GetMethodStatic(MethodInfo type)
        => type.IsStatic ? "static" : string.Empty;
}
