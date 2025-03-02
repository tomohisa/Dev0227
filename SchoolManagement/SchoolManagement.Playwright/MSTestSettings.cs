using NUnit.Framework;

// In NUnit 4.x, ParallelScope is in NUnit.Framework.Internal namespace
// and the assembly-level attribute is LevelOfParallelism
[assembly: LevelOfParallelism(4)]
