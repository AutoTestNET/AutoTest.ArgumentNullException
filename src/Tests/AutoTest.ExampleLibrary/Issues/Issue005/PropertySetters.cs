namespace AutoTest.ExampleLibrary.Issues.Issue005
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PropertySetters
    {
        private string DoNotTest1
        {
            set
            {
                throw new NotImplementedException("The setter for DoNotTest1 should not be tested for ArgumentNullException.");
            }
        }

        public string DoNotTest2
        {
            set
            {
                throw new NotImplementedException("The setter for DoNotTest2 should not be tested for ArgumentNullException.");
            }
        }
    }
}
