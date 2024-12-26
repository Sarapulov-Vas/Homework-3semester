// <copyright file="AfterAttribute.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

/// <summary>
/// The attribute of the method executed after test runs.
/// </summary>

namespace TestAttributes;
[AttributeUsage(AttributeTargets.Method)]
public class AfterAttribute : Attribute
{
}
