# Active Context: School Management System

## Current Focus

The School Management System is currently in active development with a focus on implementing the core functionality for managing students, teachers, and classes. The system uses the Sekiban event sourcing framework with a Blazor web frontend, and now also includes a Next.js frontend implementation.

### Active Development Areas

1. **Core Domain Implementation**
   - Student management (registration, updates, deletion)
   - Teacher management (registration, updates, deletion)
   - Class management (creation, updates, deletion)
   - Relationship management between entities

2. **User Interface Development**
   - Responsive Blazor components
   - Modal-based forms for data entry
   - Search and filtering capabilities
   - Tabular data presentation
   - Next.js frontend with shadcn/ui components

3. **Event Sourcing Infrastructure**
   - Command handling and validation
   - Event projection and state management
   - Query optimization
   - Orleans integration

4. **Testing Implementation**
   - Domain unit testing with Sekiban's in-memory framework
   - End-to-end testing with Playwright
   - Test coverage for core domain logic
   - Relationship validation testing

## Recent Changes

1. **Next.js Frontend Implementation**
   - Created a Next.js implementation of the frontend using shadcn/ui components
   - Implemented API clients for students, teachers, and classes
   - Created pages for students, teachers, and classes with data tables
   - Implemented dialog components for adding new entities
   - Added form validation and error handling
   - Implemented dark mode support with theme switching
   - **NEW**: Implemented BFF (Backend For Frontend) pattern for all API calls
   - **NEW**: Improved layout with proper centering and responsive design
   - **NEW**: Removed direct API calls from client components

2. **Student Management**
   - Implemented student registration, update, and deletion
   - Added student-to-class assignment functionality
   - Implemented search by name and student ID
   - Created UI components for student management
   - Added duplicate studentId checking to prevent duplicates
   - Moved duplicate checking logic to domain workflows for better separation of concerns
   - Added dialog component for adding new students in Next.js frontend
   - **NEW**: Converted client-side API calls to server actions

3. **Teacher Management**
   - Implemented teacher registration, update, and deletion
   - Added teacher-to-class assignment functionality
   - Created UI components for teacher management
   - Added duplicate teacherId checking to prevent duplicates
   - Moved duplicate checking logic to domain workflows for better separation of concerns
   - Added dialog component for adding new teachers in Next.js frontend
   - **NEW**: Converted client-side API calls to server actions

4. **Class Management**
   - Implemented class creation, update, and deletion
   - Added student and teacher assignment to classes
   - Created UI components for class management
   - Added dialog component for adding new classes in Next.js frontend
   - **NEW**: Converted client-side API calls to server actions

5. **UI Enhancements**
   - Added viewport scaling feature for better adaptability
   - Implemented responsive design for different screen sizes
   - Enhanced modal dialogs for data entry
   - Improved form validation
   - Created modern UI with shadcn/ui components in Next.js frontend
   - **NEW**: Improved layout with proper centering for all pages
   - **NEW**: Enhanced container styling with responsive padding and maximum width
   - **NEW**: Fixed navigation menu positioning

6. **Testing Enhancements**
   - Created SchoolManagement.Domain.Tests project
   - Implemented comprehensive unit tests for Student entity
   - Implemented comprehensive unit tests for Teacher entity
   - Implemented comprehensive unit tests for Class entity
   - Added relationship tests to verify entity connections
   - Established testing patterns using Given-When-Then approach

7. **Architecture Improvements**
   - **NEW**: Implemented BFF pattern for all API calls
   - **NEW**: Created server-side services for data fetching
   - **NEW**: Implemented server actions for client-server communication
   - **NEW**: Separated client and server components for better architecture
   - **NEW**: Improved error handling and state management

## Current Decisions and Considerations

1. **Frontend Implementation**
   - Maintaining both Blazor and Next.js frontends
   - Using shadcn/ui for modern UI components in Next.js
   - Implementing responsive design for all screen sizes
   - Supporting dark mode with theme switching
   - **NEW**: Using BFF pattern for all API calls
   - **NEW**: Separating client and server components

2. **Event Sourcing Implementation**
   - Using Sekiban for event sourcing infrastructure
   - Implementing proper command validation
   - Ensuring correct event projection
   - Managing aggregate relationships
   - Using query-based validation for duplicate checking
   - Leveraging source-generated domain types

3. **UI/UX Decisions**
   - Using Bootstrap for responsive design in Blazor
   - Using shadcn/ui for responsive design in Next.js
   - Implementing modal dialogs for focused interactions
   - Using form validation for data integrity
   - Providing search and filtering capabilities
   - **NEW**: Ensuring proper layout centering for all pages
   - **NEW**: Using responsive padding and maximum width for containers

4. **Performance Considerations**
   - Optimizing query performance
   - Implementing efficient projections
   - Managing Orleans grain lifecycle
   - Ensuring responsive UI
   - **NEW**: Using server-side data fetching for improved performance

5. **Testing Strategy**
   - Using Sekiban's in-memory testing for domain logic
   - Using Playwright for end-to-end testing
   - Implementing page object models
   - Testing critical user flows
   - Ensuring cross-browser compatibility
   - Following Given-When-Then pattern for event sourcing tests

## Next Steps

1. **Short-term Tasks**
   - Enhance search and filtering capabilities
   - Implement batch operations for efficiency
   - Add data export functionality
   - Improve error handling and user feedback
   - Add additional data validation rules
   - Expand unit test coverage for edge cases
   - Complete the Next.js frontend implementation

2. **Medium-term Goals**
   - Implement reporting functionality
   - Add user authentication and authorization
   - Enhance data visualization
   - Implement advanced filtering and sorting
   - Add integration tests for API endpoints

3. **Long-term Vision**
   - Extend to additional educational entities (courses, departments)
   - Implement academic performance tracking
   - Add scheduling and calendar functionality
   - Integrate with external systems
   - Implement comprehensive test automation

## Open Questions

1. **Technical Questions**
   - What is the optimal event schema for future extensibility?
   - How to handle event schema evolution?
   - What is the best approach for handling large aggregate projections?
   - How to optimize query performance for large datasets?
   - How to test complex event sourcing scenarios?
   - Which frontend implementation (Blazor or Next.js) should be the primary focus?
   - **NEW**: How to further optimize the BFF pattern for better performance?
   - **NEW**: What additional server actions should be implemented?

2. **Product Questions**
   - What additional features would provide the most value to users?
   - How to prioritize feature development?
   - What metrics should be tracked to measure success?
   - How to gather and incorporate user feedback?

## Active Challenges

1. **Technical Challenges**
   - Ensuring consistent event projection
   - Managing complex relationships between entities
   - Optimizing query performance
   - Handling concurrent updates
   - Testing event sourcing systems effectively
   - Maintaining two frontend implementations
   - **NEW**: Managing server-side state with server actions
   - **NEW**: Ensuring proper error handling with BFF pattern

2. **UX Challenges**
   - Providing intuitive interfaces for complex operations
   - Ensuring responsive design across devices
   - Balancing feature richness with simplicity
   - Providing appropriate feedback for asynchronous operations
   - **NEW**: Maintaining consistent layout across all pages
   - **NEW**: Ensuring proper spacing and alignment

3. **Testing Challenges**
   - Maintaining test coverage as the domain evolves
   - Testing complex state transitions
   - Ensuring tests remain fast and reliable
   - Balancing unit tests and integration tests
   - Handling source-generated code in tests
   - **NEW**: Testing server actions and BFF pattern

## Current Priorities

1. **High Priority**
   - Stabilize core CRUD operations
   - Ensure reliable relationship management
   - Optimize query performance
   - Enhance error handling
   - Maintain comprehensive test coverage
   - Complete the Next.js frontend implementation
   - **NEW**: Refine the BFF pattern implementation
   - **NEW**: Ensure consistent layout across all pages

2. **Medium Priority**
   - Improve search and filtering
   - Enhance UI responsiveness
   - Add data validation
   - Implement basic reporting
   - Expand test scenarios

3. **Low Priority**
   - Add advanced features
   - Implement data export
   - Enhance visualization
   - Add customization options
   - Implement performance testing

## Lessons Learned

1. **Sekiban Testing**
   - Sekiban generates domain types at build time via source generation
   - Tests must reference the generated types from the correct namespace
   - In-memory testing provides a fast and reliable way to test domain logic
   - The Given-When-Then pattern works well for event sourcing tests
   - Need to understand the relationship between commands, events, and projectors
   - SekibanInMemoryTestBase provides an Executor property that implements ISekibanExecutor for testing workflows

2. **Project Structure**
   - Separate test projects help maintain clean architecture
   - Domain tests focus on business logic without UI or database dependencies
   - End-to-end tests validate the complete system
   - Test organization should mirror domain structure
   - Domain workflows should be placed in a dedicated Workflows folder

3. **Dependency Abstraction**
   - Use ISekibanExecutor interface instead of concrete SekibanOrleansExecutor for better testability
   - Domain workflows should depend on abstractions rather than concrete implementations
   - The ISekibanExecutor interface is in the Sekiban.Pure.Executors namespace
   - This approach enables easier unit testing and better separation of concerns

4. **Frontend Development**
   - Next.js provides a modern, responsive frontend framework
   - shadcn/ui offers high-quality, customizable UI components
   - React's component model enables efficient UI development
   - Form validation is critical for data integrity
   - Dialog components provide focused user interactions
   - Dark mode support enhances user experience
   - **NEW**: BFF pattern improves security and performance
   - **NEW**: Server actions provide a clean way to handle server-side operations
   - **NEW**: Proper layout centering is essential for good user experience
   - **NEW**: Tailwind's container class needs additional styling for proper centering
