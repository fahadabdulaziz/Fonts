﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

namespace SixLabors.Fonts.Tables
{
    internal sealed class UnknownTable : Table
    {
        internal UnknownTable(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}