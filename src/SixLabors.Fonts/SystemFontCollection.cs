﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SixLabors.Fonts
{
#if FILESYSTEM
    /// <summary>
    /// Provides a collection of fonts.
    /// </summary>
    internal sealed class SystemFontCollection : IReadOnlyFontCollection
    {
        private readonly FontCollection collection = new FontCollection();

        internal SystemFontCollection()
        {
            string[] paths = new[] {
                // windows directories
                "%SYSTEMROOT%\\Fonts",

                // linux directlty list
                "~/.fonts/",
                "/usr/local/share/fonts/",
                "/usr/share/fonts/",

                // mac fonts
                "~/Library/Fonts/",
                "/Library/Fonts/",
                "/Network/Library/Fonts/",
                "/System/Library/Fonts/",
                "/System Folder/Fonts/",
            };

            string[] expanded = paths.Select(x => Environment.ExpandEnvironmentVariables(x)).ToArray();
            string[] found = expanded.Where(x => Directory.Exists(x)).ToArray();

            IEnumerable<string> files = found.SelectMany(x => Directory.EnumerateFiles(x, "*.ttf", SearchOption.AllDirectories));

            foreach(string path in files)
            {
                try
                {
                    this.collection.Install(new FileFontInstance(path));
                }
                catch
                {
                    // we swollow exceptions installing system fonts as we hold no garantees about permissions etc.
                }
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="FontFamily"/> objects associated with this <see cref="FontCollection"/>.
        /// </summary>
        /// <value>
        /// The families.
        /// </value>
        public IEnumerable<FontFamily> Families => this.collection.Families;

        ///<inheritdocs />
        public FontFamily Find(string fontFamily) => this.collection.Find(fontFamily);
        ///<inheritdocs />
        public bool TryFind(string fontFamily, out FontFamily family) => this.collection.TryFind(fontFamily, out family);
    }
#endif
}
