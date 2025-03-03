'use client'

import { useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { DataTable } from "@/components/ui/data-table";
import { ColumnDef } from "@tanstack/react-table";
import { Student } from "@/types/student";
import { AddStudentDialog } from "./add-student-dialog";
import { Button } from "@/components/ui/button";
import { useRouter } from "next/navigation";
import {
  registerStudent,
  updateStudent,
  deleteStudent,
  assignStudentToClass,
  removeStudentFromClass,
  getStudents
} from "./actions";

interface StudentsClientProps {
  initialStudents: Student[];
}

export default function StudentsClient({ initialStudents }: StudentsClientProps) {
  const router = useRouter();
  const [students, setStudents] = useState<Student[]>(initialStudents);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchStudents = async (filters?: { name?: string; studentId?: string }) => {
    try {
      setLoading(true);
      const result = await getStudents(filters?.name, filters?.studentId);
      if (result.error) {
        setError(result.error);
      } else if (result.data) {
        setStudents(result.data);
        setError(null);
      }
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (studentId: string) => {
    if (!confirm("Are you sure you want to delete this student?")) return;

    const result = await deleteStudent(studentId);
    if (result.error) {
      alert(result.error);
    } else {
      fetchStudents();
    }
  };

  const columns: ColumnDef<Student>[] = [
    {
      accessorKey: "name",
      header: "Name",
    },
    {
      accessorKey: "studentIdNumber",
      header: "Student ID",
    },
    {
      accessorKey: "age",
      header: "Age",
    },
    {
      accessorKey: "email",
      header: "Email",
    },
    {
      accessorKey: "phoneNumber",
      header: "Phone",
    },
    {
      accessorKey: "classId",
      header: "Class",
      cell: ({ row }) => {
        const classId = row.getValue("classId");
        return classId ? "Assigned" : "Not assigned";
      },
    },
    {
      id: "actions",
      header: "Actions",
      cell: ({ row }) => {
        const student = row.original;
        return (
          <div className="flex items-center gap-2">
            <Button 
              variant="outline" 
              size="sm"
              onClick={() => router.push(`/students/${student.studentId}/edit`)}
            >
              Edit
            </Button>
            <Button 
              variant="outline" 
              size="sm"
              onClick={() => handleDelete(student.studentId)}
            >
              Delete
            </Button>
          </div>
        );
      },
    },
  ];

  return (
    <div className="container py-6">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Students</h1>
        <AddStudentDialog onSuccess={() => fetchStudents()} />
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Student List</CardTitle>
        </CardHeader>
        <CardContent>
          {loading ? (
            <div className="flex justify-center items-center h-24">
              <p>Loading students...</p>
            </div>
          ) : error ? (
            <div className="bg-red-50 p-4 rounded-md text-red-500">
              <p>{error}</p>
            </div>
          ) : students.length === 0 ? (
            <div className="text-center p-4">
              <p className="mb-4">No students found. Add a new student to get started.</p>
            </div>
          ) : (
            <DataTable
              columns={columns}
              data={students}
              searchColumn="name"
              searchPlaceholder="Search by name..."
            />
          )}
        </CardContent>
      </Card>
    </div>
  );
}
