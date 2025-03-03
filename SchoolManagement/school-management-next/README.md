# School Management System - Next.js Frontend

This is a Next.js implementation of the School Management System frontend, built with Next.js and shadcn/ui. It provides a modern, responsive interface for managing students, teachers, and classes.

## Features

- **Student Management**: Register, update, and delete student records. Assign students to classes.
- **Teacher Management**: Register, update, and delete teacher records. Assign teachers to classes.
- **Class Management**: Create, update, and delete class records. Assign teachers and students to classes.
- **Modern UI**: Built with Next.js and shadcn/ui for a clean, responsive interface.
- **Dark Mode**: Supports light and dark mode with theme switching.
- **Responsive Design**: Works on desktop and mobile devices.

## Technologies Used

- **Next.js**: React framework for server-rendered applications
- **React**: JavaScript library for building user interfaces
- **TypeScript**: Typed JavaScript for better developer experience
- **Tailwind CSS**: Utility-first CSS framework
- **shadcn/ui**: Reusable UI components built with Radix UI and Tailwind CSS
- **Axios**: Promise-based HTTP client for API requests
- **Tanstack Table**: Headless UI for building powerful tables and datagrids

## Getting Started

### Prerequisites

- Node.js 18.x or later
- npm or yarn

### Installation

1. Clone the repository
2. Install dependencies:

```bash
cd school-management-next
npm install
```

3. Create a `.env.local` file with the following content:

```
NEXT_PUBLIC_API_URL=https://localhost:7370
```

4. Start the development server:

```bash
npm run dev
```

5. Open [http://localhost:3000](http://localhost:3000) in your browser.

## API Integration

This frontend connects to the School Management API, which is built with ASP.NET Core and uses the Sekiban event sourcing framework. The API provides endpoints for managing students, teachers, and classes.

## Project Structure

- `src/app`: Next.js app router pages
- `src/components`: Reusable UI components
- `src/lib`: Utility functions and API clients
- `public`: Static assets

## Development

### Running the Development Server

```bash
npm run dev
```

### Building for Production

```bash
npm run build
```

### Running in Production Mode

```bash
npm start
```

## License

This project is licensed under the MIT License.
