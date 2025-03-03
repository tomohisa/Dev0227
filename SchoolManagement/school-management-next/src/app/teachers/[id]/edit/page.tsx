"use client";

import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Teacher } from "@/types/teacher";

interface EditPageProps {
  params: {
    id: string;
  };
}

export default function EditTeacherPage({ params }: EditPageProps) {
  const router = useRouter();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [formData, setFormData] = useState({
    name: "",
    subject: "",
    email: "",
    phoneNumber: "",
    address: "",
  });

  useEffect(() => {
    const fetchTeacher = async () => {
      try {
        const response = await fetch(`/api/teachers/${params.id}`);
        if (!response.ok) {
          throw new Error("Failed to fetch teacher");
        }
        const teacher: Teacher = await response.json();
        setFormData({
          name: teacher.name,
          subject: teacher.subject,
          email: teacher.email,
          phoneNumber: teacher.phoneNumber,
          address: teacher.address,
        });
      } catch (err) {
        console.error("Error fetching teacher:", err);
        setError("Failed to load teacher information");
      } finally {
        setLoading(false);
      }
    };

    fetchTeacher();
  }, [params.id]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);

    try {
      const response = await fetch(`/api/teachers/${params.id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(formData),
      });

      if (!response.ok) {
        throw new Error("Failed to update teacher");
      }

      router.push("/teachers");
    } catch (err) {
      console.error("Error updating teacher:", err);
      setError("Failed to update teacher information");
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="container py-6">
        <Card>
          <CardContent className="p-6">
            <div className="flex justify-center items-center h-24">
              <p>Loading teacher information...</p>
            </div>
          </CardContent>
        </Card>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container py-6">
        <Card>
          <CardContent className="p-6">
            <div className="bg-red-50 p-4 rounded-md text-red-500">
              <p>{error}</p>
            </div>
            <Button className="mt-4" onClick={() => router.push("/teachers")}>
              Back to Teachers
            </Button>
          </CardContent>
        </Card>
      </div>
    );
  }

  return (
    <div className="container py-6">
      <Card>
        <CardHeader>
          <CardTitle>Edit Teacher</CardTitle>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="grid w-full items-center gap-1.5">
              <Label htmlFor="name">Name</Label>
              <Input
                type="text"
                id="name"
                name="name"
                value={formData.name}
                onChange={handleChange}
                required
              />
            </div>

            <div className="grid w-full items-center gap-1.5">
              <Label htmlFor="subject">Subject</Label>
              <Input
                type="text"
                id="subject"
                name="subject"
                value={formData.subject}
                onChange={handleChange}
                required
              />
            </div>

            <div className="grid w-full items-center gap-1.5">
              <Label htmlFor="email">Email</Label>
              <Input
                type="email"
                id="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                required
              />
            </div>

            <div className="grid w-full items-center gap-1.5">
              <Label htmlFor="phoneNumber">Phone Number</Label>
              <Input
                type="text"
                id="phoneNumber"
                name="phoneNumber"
                value={formData.phoneNumber}
                onChange={handleChange}
                required
              />
            </div>

            <div className="grid w-full items-center gap-1.5">
              <Label htmlFor="address">Address</Label>
              <Input
                type="text"
                id="address"
                name="address"
                value={formData.address}
                onChange={handleChange}
                required
              />
            </div>

            <div className="flex gap-4">
              <Button type="submit" disabled={loading}>
                {loading ? "Saving..." : "Save Changes"}
              </Button>
              <Button
                type="button"
                variant="outline"
                onClick={() => router.push("/teachers")}
              >
                Cancel
              </Button>
            </div>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
