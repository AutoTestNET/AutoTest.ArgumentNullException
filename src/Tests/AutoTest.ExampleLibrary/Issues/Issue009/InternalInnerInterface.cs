namespace AutoTest.ExampleLibrary.Issues.Issue009
{
    using System;

    /// <summary>
    /// Test class used to demonstrate issue 9
    /// https://github.com/AutoTestNET/AutoTest.ArgumentNullException/issues/9
    /// </summary>
    public static class InternalInnerInterface
    {
        internal interface IInternalInterface
        {
        }

        internal static void AMethod<T>(T privateThing)
            where T : class, IInternalInterface
        {
            if (privateThing == null)
                throw new ArgumentNullException("privateThing");
        }
    }
}
