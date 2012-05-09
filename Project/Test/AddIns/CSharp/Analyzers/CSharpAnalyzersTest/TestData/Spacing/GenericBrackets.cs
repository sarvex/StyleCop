﻿namespace CSharpAnalyzersTest.TestData.Spacing
{
    using System.Collections.Generic;

    internal class GenericBrackets
    {
        public class SA1015TestCode<T>: object // missing space between > and :
        {
            private List<int>ints1; // missing space between > and identifier

            private void MethodName2<T1> (T1 x) // redundant space between > and (
            {
                List<int>ints2; // missing space between > and identifier
                this.MethodName2<T1> (x); // redundant space between > and (
            }
        }

        public class SA1015TestCode2<T> : object 
        {
            private List<int> ints1; 
            private void MethodName2<T1>(T1 x) 
            {
                List<int> ints2;
                this.MethodName2<T1>(x); 
            }
        }
    }
}
