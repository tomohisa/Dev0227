'use client'

import { useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { DataTable } from "@/components/ui/data-table";
import { ColumnDef } from "@tanstack/react-table";
import { AddClassDialog } from "./add-class-dialog";
import { Button } from "@/components/ui/button";
import { Class } from "@/lib/server/services/classes";
import {
  createClass,
  updateClass,
  deleteClass,
  assignTeacherToClass,
  removeTeacherFromClass,
  addStudentToClass,
  removeStudentFromClass,
  getClasses
} from "./actions";

interface ClassesClientProps {
  initialClasses: Class[];
}

export default function ClassesClient({ initialClasses }: ClassesClientProps) {
  const [classes, setClasses] = useState<Class[]>(initialClasses);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchClasses = async (filters?: { name?: string; classCode?: string }) => {
    try {
      setLoading(true);
      const result = await getClasses(filters?.name, filters?.classCode);
      if (result.error) {
        setError(result.error);
      } else if (result.data) {
        setClasses(result.data);
        setError(null);
      }
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (classId: string) => {
    if (!confirm("Are you sure you want to delete this class?")) return;

    const result = await deleteClass(classId);
    if (result.error) {
      alert(result.error);
    } else {
      fetchClasses();
    }
  };

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
            <Button 
              variant="outline" 
              size="sm"
              onClick={() => handleDelete(classItem.classId)}
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
        <h1 className="text-3xl font-bold">Classes</h1>
        <AddClassDialog onSuccess={() => fetchClasses()} />
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
