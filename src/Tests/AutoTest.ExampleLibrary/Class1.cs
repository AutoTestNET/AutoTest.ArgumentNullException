namespace AutoTest.ExampleLibrary
{
    using System;

    public class Class1
    {
        public Class1(object input)
        {
            if (input == null) throw new ArgumentNullException("input");
        }

        public void Amethod(object input)
        {
            if (input == null) throw new ArgumentNullException("input");
        }
    }
}
