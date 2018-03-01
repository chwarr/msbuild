// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;
using Xunit;

namespace Microsoft.Build.UnitTests
{
    sealed public class RemoveDuplicates_Tests
    {
        /// <summary>
        /// Pass one item in, get the same item back.
        /// </summary>
        [Fact]
        public void OneItemNop()
        {
            var t = new RemoveDuplicates();
            t.BuildEngine = new MockEngine();

            t.Inputs = new ITaskItem[] { new TaskItem("MyFile.txt") };

            bool success = t.Execute();
            Assert.True(success);
            Assert.Equal(1, t.Filtered.Length);
            Assert.Equal("MyFile.txt", t.Filtered[0].ItemSpec);
            Assert.False(t.HadAnyDuplicates);
        }

        /// <summary>
        /// Pass in two of the same items.
        /// </summary>
        [Fact]
        public void TwoItemsTheSame()
        {
            var t = new RemoveDuplicates();
            t.BuildEngine = new MockEngine();

            t.Inputs = new ITaskItem[] { new TaskItem("MyFile.txt"), new TaskItem("MyFile.txt") };

            bool success = t.Execute();
            Assert.True(success);
            Assert.Equal(1, t.Filtered.Length);
            Assert.Equal("MyFile.txt", t.Filtered[0].ItemSpec);
            Assert.True(t.HadAnyDuplicates);
        }

        /// <summary>
        /// Pass in two items that are different.
        /// </summary>
        [Fact]
        public void TwoItemsDifferent()
        {
            var t = new RemoveDuplicates();
            t.BuildEngine = new MockEngine();

            t.Inputs = new ITaskItem[] { new TaskItem("MyFile1.txt"), new TaskItem("MyFile2.txt") };

            bool success = t.Execute();
            Assert.True(success);
            Assert.Equal(2, t.Filtered.Length);
            Assert.Equal("MyFile1.txt", t.Filtered[0].ItemSpec);
            Assert.Equal("MyFile2.txt", t.Filtered[1].ItemSpec);
            Assert.False(t.HadAnyDuplicates);
        }

        /// <summary>
        /// Case should not matter.
        /// </summary>
        [Fact]
        public void CaseInsensitive()
        {
            var t = new RemoveDuplicates();
            t.BuildEngine = new MockEngine();

            t.Inputs = new ITaskItem[] { new TaskItem("MyFile.txt"), new TaskItem("MyFIle.tXt") };

            bool success = t.Execute();
            Assert.True(success);
            Assert.Equal(1, t.Filtered.Length);
            Assert.Equal("MyFile.txt", t.Filtered[0].ItemSpec);
            Assert.True(t.HadAnyDuplicates);
        }

        /// <summary>
        /// No inputs should result in zero-length outputs.
        /// </summary>
        [Fact]
        public void MissingInputs()
        {
            var t = new RemoveDuplicates();
            t.BuildEngine = new MockEngine();
            bool success = t.Execute();

            Assert.True(success);
            Assert.Equal(0, t.Filtered.Length);
            Assert.False(t.HadAnyDuplicates);
        }
    }
}
