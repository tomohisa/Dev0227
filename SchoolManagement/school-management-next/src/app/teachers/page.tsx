"use client";

import { useState, useEffect } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { DataTable } from "@/components/ui/data-table";
import { ColumnDef } from "@tanstack/react-table";
import { Teacher, TeacherApi } from "@/lib/api";
import { AddTeacherDialog } from "./add-teacher-dialog";
import { Button } from "@/components/ui/button";

export default function TeachersPage() {
  const [teachers, setTeachers] = useState<Teacher[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchTeachers = async () => {
      try {
        setLoading(true);
        const data = await TeacherApi.getTeachers();
        setTeachers(data);
        setError(null);
      } catch (err) {
        console.error("Error fetching teachers:", err);
        setError("Failed to load teachers. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchTeachers();
  }, []);

  const columns: ColumnDef<Teacher>[] = [
    {
      accessorKey: "name",
      header: "Name",
    },
    {
      accessorKey: "teacherIdNumber",
      header: "Teacher ID",
    },
    {
      accessorKey: "subject",
      header: "Subject",
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
      accessorKey: "classIds",
      header: "Classes",
      cell: ({ row }) => {
        const classIds = row.original.classIds;
        return classIds && classIds.length > 0 ? `${classIds.length} classes` : "No classes";
      },
    },
    {
      id: "actions",
      header: "Actions",
      cell: ({ row }) => {
        const teacher = row.original;
        return (
          <div className="flex items-center gap-2">
            <Button variant="outline" size="sm">
              Edit
            </Button>
            <Button variant="outline" size="sm">
              View Classes
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
        <h1 className="text-3xl font-bold">Teachers</h1>
        <AddTeacherDialog />
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Teacher List</CardTitle>
        </CardHeader>
        <CardContent>
          {loading ? (
            <div className="flex justify-center items-center h-24">
              <p>Loading teachers...</p>
            </div>
          ) : error ? (
            <div className="bg-red-50 p-4 rounded-md text-red-500">
              <p>{error}</p>
            </div>
          ) : teachers.length === 0 ? (
            <div className="text-center p-4">
              <p className="mb-4">No teachers found. Add a new teacher to get started.</p>
            </div>
          ) : (
            <DataTable
              columns={columns}
              data={teachers}
              searchColumn="name"
              searchPlaceholder="Search by name..."
            />
          )}
        </CardContent>
      </Card>
    </div>
  );
}
