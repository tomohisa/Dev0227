"use client";

import { useState, useEffect } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { DataTable } from "@/components/ui/data-table";
import { ColumnDef } from "@tanstack/react-table";
import { Class, ClassApi } from "@/lib/api";
import { AddClassDialog } from "./add-class-dialog";
import { Button } from "@/components/ui/button";

export default function ClassesPage() {
  const [classes, setClasses] = useState<Class[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchClasses = async () => {
      try {
        setLoading(true);
        const data = await ClassApi.getClasses();
        setClasses(data);
        setError(null);
      } catch (err) {
        console.error("Error fetching classes:", err);
        setError("Failed to load classes. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchClasses();
  }, []);

  const columns: ColumnDef<Class>[] = [
    {
      accessorKey: "name",
      header: "Name",
    },
    {
      accessorKey: "classCode",
      header: "Class Code",
    },
    {
      accessorKey: "description",
      header: "Description",
    },
    {
      accessorKey: "teacherId",
      header: "Teacher",
      cell: ({ row }) => {
        const teacherId = row.getValue("teacherId");
        return teacherId ? "Assigned" : "Not assigned";
      },
    },
    {
      accessorKey: "studentIds",
      header: "Students",
      cell: ({ row }) => {
        const studentIds = row.original.studentIds;
        return studentIds && studentIds.length > 0 ? `${studentIds.length} students` : "No students";
      },
    },
    {
      id: "actions",
      header: "Actions",
      cell: ({ row }) => {
        const classItem = row.original;
        return (
          <div className="flex items-center gap-2">
            <Button variant="outline" size="sm">
              Edit
            </Button>
            <Button variant="outline" size="sm">
              Manage
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
        <h1 className="text-3xl font-bold">Classes</h1>
        <AddClassDialog />
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Class List</CardTitle>
        </CardHeader>
        <CardContent>
          {loading ? (
            <div className="flex justify-center items-center h-24">
              <p>Loading classes...</p>
            </div>
          ) : error ? (
            <div className="bg-red-50 p-4 rounded-md text-red-500">
              <p>{error}</p>
            </div>
          ) : classes.length === 0 ? (
            <div className="text-center p-4">
              <p className="mb-4">No classes found. Add a new class to get started.</p>
            </div>
          ) : (
            <DataTable
              columns={columns}
              data={classes}
              searchColumn="name"
              searchPlaceholder="Search by name..."
            />
          )}
        </CardContent>
      </Card>
    </div>
  );
}
