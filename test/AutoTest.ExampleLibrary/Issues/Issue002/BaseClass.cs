namespace AutoTest.ExampleLibrary.Issues.Issue002
{
    using System;

    public abstract class AbstractBase
    {
        public void AMethod(object data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
        }

        public abstract void MethodToCall(object data);
    }

    public class BaseClass : AbstractBase
    {
        public bool BaseCalled { get; private set; }

        public override void MethodToCall(object data1)
        {
            if (data1 == null)
                throw new ArgumentNullException(nameof(data1));
            BaseCalled = true;
        }
    }

    public class DerivedClass : BaseClass
    {
        public bool DerivedCalled { get; private set; }

        public override void MethodToCall(object data2)
        {
            if (data2 == null)
                throw new ArgumentNullException(nameof(data2));
            DerivedCalled = true;
        }
    }
}
