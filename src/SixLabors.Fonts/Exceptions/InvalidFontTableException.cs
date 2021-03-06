﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

namespace SixLabors.Fonts.Exceptions
{
    /// <summary>
    /// Exception font loading can throw if it encounteres invalid data during font loading.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class InvalidFontTableException : InvalidFontFileException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFontTableException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="table">The table.</param>
        public InvalidFontTableException(string message, string table)
            : base(message)
        {
            this.Table = table;
        }

        /// <summary>
        /// Gets the table where the error originated.
        /// </summary>
        public string Table { get; }
    }
}
