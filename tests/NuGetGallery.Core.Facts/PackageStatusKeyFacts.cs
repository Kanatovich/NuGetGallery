﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Xunit;

namespace NuGetGallery
{
    public class PackageStatusKeyFacts
    {
        public void AssertDeletedNotChanged()
        {
            Assert.Equal(1, PackageStatusKey.Deleted);
        }
    }
}
