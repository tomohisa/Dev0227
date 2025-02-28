namespace SchoolManagement.Playwright.Helpers
{
    public static class TestData
    {
        // Student data
        public static readonly (string Name, string Id, string Email, string Phone)[] Students = new[]
        {
            ("John Smith", "S12345", "john.smith@example.com", "555-123-4567"),
            ("Jane Doe", "S67890", "jane.doe@example.com", "555-987-6543"),
            ("Michael Johnson", "S24680", "michael.johnson@example.com", "555-246-8024")
        };

        // Teacher data
        public static readonly (string Name, string Id, string Email, string Phone, string Subject, string Address)[] Teachers = new[]
        {
            ("Dr. Robert Brown", "T12345", "robert.brown@example.com", "555-111-2222", "Mathematics", "123 University Ave, College Town, CA 94321"),
            ("Prof. Sarah Wilson", "T67890", "sarah.wilson@example.com", "555-333-4444", "Science", "456 Campus Dr, Research City, CA 94322"),
            ("Ms. Emily Davis", "T24680", "emily.davis@example.com", "555-555-6666", "English", "789 School St, Education Valley, CA 94323")
        };

        // Class data
        public static readonly (string Name, string Code, string Description)[] Classes = new[]
        {
            ("Mathematics 101", "MATH101", "An introductory course to basic mathematics concepts and principles."),
            ("Science 101", "SCI101", "An introductory course to basic science concepts and principles.")
        };
    }
}
