﻿using System.Collections.Generic;

namespace CSharpAnalyzersTest.TestData.Spacing
{
    internal class PositiveSymbol
    {
        private int Method1(int paramName)
        {
            List<int> list = new List<int>(this.Method1( +42));
            return list[ +42];
        }
    }
}
