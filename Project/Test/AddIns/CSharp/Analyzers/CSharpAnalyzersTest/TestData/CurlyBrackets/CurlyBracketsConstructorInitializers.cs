using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpAnalyzersTest.TestData
{
    public class CurlyBracketsConstructorInitializers
    {
        // Allowed constructor initializers
        public CurlyBracketsConstructorInitializers(bool temp)
            : base(delegate() { return true; })
        {
        }
    }
}