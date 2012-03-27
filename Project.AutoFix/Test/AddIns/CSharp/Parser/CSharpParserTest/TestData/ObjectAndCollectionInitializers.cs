namespace StyleCop.CSharpParserTest.TestData
{
    using System;

    public class ObjectAndCollectionInitializers
    {
        public void Method()
        {
            // Points without an argument list.
            Point a = new Point { };        
            Point b = new Point { X = 0 };        
            Point c = new Point { X = 0, };        
            Point d = new Point { X = 0, Y = 1 };        

            // Points with an argument list.
            Point e = new Point(0, 1);
            Point f = new Point(0, 1) { };        
            Point g = new Point(0, 1) { X = 0 };        
            Point h = new Point(0, 1) { X = 0, };        
            Point i = new Point(0, 1) { X = 0, Y = 1 };        

            // Nested object initializers.
            Rectangle r = new Rectangle 
            {
                P1 = new Point { X = 0, Y = 1 },
                P2 = new Point { X = 2, Y = 3 }
            };

            // Nested object initializers with extra commas.
            Rectangle r = new Rectangle 
            {
                P1 = new Point { X = 0, Y = 1, },
                P2 = new Point { X = 2, Y = 3, },
            };

            // Nested object initializers with argument lists.
            Rectangle r = new Rectangle(Point.Empty, Point.Empty)
            {
                P1 = new Point(0, 0) { X = 0, Y = 1 },
                P2 = new Point(0, 0) { X = 2, Y = 3 }
            };

            // Nested object initializers which omit the new expression.
            Rectangle r = new Rectangle 
            {
                P1 = { X = 0, Y = 1 },
                P2 = { X = 2, Y = 3 }
            };

            // Nested object initializers which omit the new expression, extra commas.
            Rectangle r = new Rectangle 
            {
                P1 = { X = 0, Y = 1, },
                P2 = { X = 2, Y = 3, },
            };

            // Collection initializers.
            List<int> digits = new List<int> { };        
            List<int> digits = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };        
            List<int> digits = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, };     

            // Collection initializer with embedded expression lists.
            List<int> digits = new List<int> { { 0, 1, 2 }, { 3, 4, 5}, { 6, 7, 8}, 9, };     
   
            // A combination of nested collection and object initialziers.
            List<Contact> contacts = new List<Contact> 
            {
                new Contact 
                {
                    Name = "Chris Smith",
                    PhoneNumbers = { "206-555-0101", "425-882-8080" }
                },
                new Contact 
                {
                    Name = "Bob Harris",
                    PhoneNumbers = { "650-555-0199" }
                }
            };

            // Anonymous type declarations.
            var p1 = new { };        
            var p2 = new { Name = "Lawnmower" };        
            var p3 = new { Name = "Lawnmower", Price = 495.00 };        
            var p4 = new { Name = "Lawnmower", Price = 495.00, };        
        }
    }
}
