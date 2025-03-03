"use client";

import { useState, useEffect } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { DataTable } from "@/components/ui/data-table";
import { ColumnDef } from "@tanstack/react-table";
import { Student, StudentApi } from "@/lib/api";
import { AddStudentDialog } from "./add-student-dialog";
import { Button } from "@/components/ui/button";

export default function StudentsPage() {
  const [students, setStudents] = useState<Student[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchStudents = async () => {
      try {
        setLoading(true);
        const data = await StudentApi.getStudents();
        setStudents(data);
        setError(null);
      } catch (err) {
        console.error("Error fetching students:", err);
        setError("Failed to load students. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchStudents();
  }, []);

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
            <Button variant="outline" size="sm">
              Edit
            </Button>
            <Button variant="outline" size="sm">
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
        <AddStudentDialog />
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
