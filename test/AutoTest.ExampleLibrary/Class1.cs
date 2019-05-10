namespace AutoTest.ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Class1
    {
        public Class1(object input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
        }

        public void Amethod(object input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
        }
    }
}
