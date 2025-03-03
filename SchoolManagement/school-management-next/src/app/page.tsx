import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import Link from "next/link";

export default function Home() {
  return (
    <main className="container py-6 md:py-12">
      <div className="flex flex-col items-center text-center">
        <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl lg:text-6xl">
          School Management System
        </h1>
        <p className="mx-auto mt-4 max-w-[700px] text-gray-500 md:text-xl dark:text-gray-400">
          An event-sourced application for managing students, teachers, and classes
        </p>
      </div>

      <div className="grid grid-cols-1 gap-6 mt-12 md:grid-cols-3">
        <Card className="h-full">
          <CardHeader>
            <CardTitle>Students</CardTitle>
            <CardDescription>
              Manage student information and class assignments
            </CardDescription>
          </CardHeader>
          <CardContent>
            <p className="mb-4">
              Register, update, and delete student records. Assign students to classes
              and track their information.
            </p>
            <Link
              href="/students"
              className="inline-flex items-center justify-center rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground ring-offset-background transition-colors hover:bg-primary/90 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50"
            >
              Manage Students
            </Link>
          </CardContent>
        </Card>

        <Card className="h-full">
          <CardHeader>
            <CardTitle>Teachers</CardTitle>
            <CardDescription>
              Manage teacher information and class assignments
            </CardDescription>
          </CardHeader>
          <CardContent>
            <p className="mb-4">
              Register, update, and delete teacher records. Assign teachers to classes
              and track their information.
            </p>
            <Link
              href="/teachers"
              className="inline-flex items-center justify-center rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground ring-offset-background transition-colors hover:bg-primary/90 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50"
            >
              Manage Teachers
            </Link>
          </CardContent>
        </Card>

        <Card className="h-full">
          <CardHeader>
            <CardTitle>Classes</CardTitle>
            <CardDescription>
              Manage classes, teachers, and student enrollment
            </CardDescription>
          </CardHeader>
          <CardContent>
            <p className="mb-4">
              Create, update, and delete class records. Assign teachers and students
              to classes and manage relationships.
            </p>
            <Link
              href="/classes"
              className="inline-flex items-center justify-center rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground ring-offset-background transition-colors hover:bg-primary/90 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50"
            >
              Manage Classes
            </Link>
          </CardContent>
        </Card>
      </div>

      <div className="mt-12">
        <Card>
          <CardHeader>
            <CardTitle>About Event Sourcing</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="mb-4">
              Event sourcing is a design pattern where all changes to an application's state are stored as a sequence of events.
              These events are the source of truth, and the current state is derived by replaying these events.
            </p>
            <p className="mb-4">
              Benefits of event sourcing include:
            </p>
            <ul className="list-disc pl-6 space-y-2">
              <li>Complete audit trail of all changes</li>
              <li>Ability to reconstruct the state at any point in time</li>
              <li>Separation of write and read models</li>
              <li>Improved scalability and performance</li>
            </ul>
          </CardContent>
        </Card>
      </div>
    </main>
  );
}
