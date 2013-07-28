namespace AutoTest.ExampleLibrary.Issues.Issue001
{
    using System;

    public static class InterfaceGenericMethods
    {
        /// <summary>
        /// Gets a value indicating if the <see cref="GenericMethod{T}"/> classValue parameter has been tested.
        /// </summary>
        public static bool ClassValueTested { get; private set; }

        /// <summary>
        /// Gets a value indicating if the <see cref="GenericMethod{T}"/> stringValue parameter has been tested.
        /// </summary>
        public static bool StringValueTested { get; private set; }

        public interface ITest1
        {
            string String1 { get; set; }
        }

        public interface ITest2
        {
            string String2 { get; set; }
        }

        public static void GenericMethod<TClass>(TClass classValue, string stringValue)
            where TClass : ITest1, ITest2
        {
            ClassValueTested = false;

            if (classValue == null)
            {
                ClassValueTested = true;
                throw new ArgumentNullException("classValue");
            }

            if (stringValue == null)
            {
                StringValueTested = true;
                throw new ArgumentNullException("stringValue");
            }

            throw new Exception("Shouldn't ever get here.");
        }
    }
}
